# /iis-status — Show IIS Orchestrator Status

Show the current status of the IIS Orchestrator (running or last completed run),
plus overall project progress and new issues.

## What to do

### 1. Current session status
Read the status file and last 30 lines of the log:
```
D:\github\mRemoteNG\.project-roadmap\scripts\orchestrator-status.json
D:\github\mRemoteNG\.project-roadmap\scripts\orchestrator.log
```

Check if orchestrator processes are alive AND identify each one:
```bash
powershell.exe -NoProfile -Command 'Get-Process python* -ErrorAction SilentlyContinue | ForEach-Object { $p = $_; $cmd = (Get-CimInstance Win32_Process -Filter "ProcessId=$($p.Id)" -ErrorAction SilentlyContinue).CommandLine; [PSCustomObject]@{PID=$p.Id; Start=$p.StartTime.ToString("HH:mm:ss"); CPU=[math]::Round($p.CPU,1); "Mem(MB)"=[math]::Round($p.WorkingSet64/1MB,0); CommandLine=$cmd} } | Format-Table PID, Start, CPU, "Mem(MB)", CommandLine -AutoSize -Wrap'
```

Analyze the process list and identify each process role:
- **Supervisor**: runs `orchestrator_supervisor.py` — should be exactly 1
- **Orchestrator**: runs `iis_orchestrator.py` — should be exactly 1
- **Claude agent**: runs `claude` — spawned by orchestrator for current issue
- **Build/Test worker**: runs `pwsh` or `dotnet` — spawned during build/test phase
- **Orphan**: any python process that doesn't match the above — flag as WARNING

Flag problems:
- Multiple supervisors = orphan from previous session, kill the older one
- Orchestrator without supervisor = unmanaged, could hang forever
- Supervisor without orchestrator = supervisor may be about to restart it (OK if brief)
- Stale processes (running 2+ hours with no status update) = likely hung

Present session summary:
- **Running state**: Is the orchestrator currently active?
- **Current phase**: issues / warnings / done
- **Current task**: What issue is being processed right now
- **Issues**: synced, triaged, implemented, failed, skipped (by reason)
- **Warnings**: start count -> current count (fixed count, % improvement)
- **Commits**: List of commits made this session (hash + message)
- **Errors**: Any errors encountered
- **Cost**: Token usage and USD cost
- **Duration**: How long the session has been running

### 1b. Performance Stats (log-based, multi-session)
Run this script to extract performance metrics from the orchestrator log:

```bash
cd D:/github/mRemoteNG && python -c "
import sys, os, re
from datetime import datetime, timedelta
from collections import defaultdict
sys.stdout.reconfigure(encoding='utf-8', errors='replace')

LOG = '.project-roadmap/scripts/orchestrator.log'
STATUS = '.project-roadmap/scripts/orchestrator-status.json'

# Read last 30K lines (~7 days)
lines = []
try:
    with open(LOG, encoding='utf-8', errors='replace') as f:
        lines = f.readlines()
    if len(lines) > 30000:
        lines = lines[-30000:]
except FileNotFoundError:
    print('NO_LOG=1')
    sys.exit(0)

# Parse log timestamp
def parse_ts(line):
    m = re.match(r'(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2})', line)
    return datetime.strptime(m.group(1), '%Y-%m-%d %H:%M:%S') if m else None

# Patterns
PAT_COMMIT = re.compile(r'committed ([0-9a-f]{7,8})')
PAT_ALL_FAILED = re.compile(r'All agents in chain failed')
PAT_OPUS_FAILED = re.compile(r'Opus fallback also failed')
PAT_OPUS_FALLBACK = re.compile(r'Sonnet failed.*retrying.*with Opus')
PAT_TIMEOUT = re.compile(r'TIMEOUT')
PAT_SESSION = re.compile(r'TEST HYGIENE \(pre-flight\)')
PAT_ISSUE_START = re.compile(r'\[(\d+)/(\d+)\] Issue #(\d+)')

# Collect events per day
today = datetime.now().strftime('%Y-%m-%d')
days = defaultdict(lambda: {'commits': 0, 'all_failed': 0, 'opus_failed': 0,
                             'opus_fallback': 0, 'timeouts': 0,
                             'sessions': [], 'first_ts': None, 'last_ts': None,
                             'issues_started': 0})

# Session tracking
sessions = []  # list of (start_ts, day)
current_session_start = None

for line in lines:
    ts = parse_ts(line)
    if not ts:
        continue
    day = ts.strftime('%Y-%m-%d')
    d = days[day]
    if d['first_ts'] is None or ts < d['first_ts']:
        d['first_ts'] = ts
    if d['last_ts'] is None or ts > d['last_ts']:
        d['last_ts'] = ts

    if PAT_SESSION.search(line):
        current_session_start = ts
        sessions.append({'start': ts, 'day': day, 'commits': 0, 'failures': 0})
    if PAT_COMMIT.search(line):
        d['commits'] += 1
        if sessions:
            sessions[-1]['commits'] += 1
    if PAT_ALL_FAILED.search(line):
        d['all_failed'] += 1
        if sessions:
            sessions[-1]['failures'] += 1
    if PAT_OPUS_FAILED.search(line):
        d['opus_failed'] += 1
    if PAT_OPUS_FALLBACK.search(line):
        d['opus_fallback'] += 1
    if PAT_TIMEOUT.search(line):
        d['timeouts'] += 1
    if PAT_ISSUE_START.search(line):
        d['issues_started'] += 1

# Calculate metrics
def calc(d):
    commits = d['commits']
    failures = d['all_failed']
    total = commits + failures
    rate = int(commits / total * 100) if total else 0
    first = d['first_ts']
    last = d['last_ts']
    hours = (last - first).total_seconds() / 3600 if first and last else 0
    avg_min = int(hours * 60 / commits) if commits else 0
    cph = round(commits / hours, 1) if hours > 0.1 else 0
    return commits, failures, rate, d['opus_fallback'], avg_min, cph, hours

# Current session from status.json
sess_commits = 0
sess_failures = 0
sess_opus_fb = 0
try:
    import json
    st = json.load(open(STATUS, encoding='utf-8'))
    sess_commits = len([c for c in st.get('commits', []) if c.get('success', False)])
    sess_failures = len(st.get('errors', {}))
except:
    pass

# Today stats
td = days.get(today)
if td:
    t_commits, t_failures, t_rate, t_opus, t_avg, t_cph, t_hours = calc(td)
else:
    t_commits = t_failures = t_rate = t_opus = t_avg = t_hours = 0
    t_cph = 0.0

# 7-day stats
week_ago = (datetime.now() - timedelta(days=7)).strftime('%Y-%m-%d')
w_commits = w_failures = w_opus = 0
w_hours = 0.0
active_days = 0
for day_key, dv in days.items():
    if day_key >= week_ago:
        c, f, _, o, _, _, h = calc(dv)
        w_commits += c
        w_failures += f
        w_opus += o
        w_hours += h
        if c > 0 or f > 0:
            active_days += 1
w_total = w_commits + w_failures
w_rate = int(w_commits / w_total * 100) if w_total else 0
w_avg = int(w_hours * 60 / w_commits) if w_commits else 0
w_cph = round(w_commits / w_hours, 1) if w_hours > 0.1 else 0
w_per_day = round(w_commits / max(active_days, 1), 0)
w_fail_day = round(w_failures / max(active_days, 1), 0)
w_opus_day = round(w_opus / max(active_days, 1), 0)

# Session stats from status.json
sess_total = sess_commits + sess_failures
sess_rate = int(sess_commits / sess_total * 100) if sess_total else 0

# Print table
print('--- Performance (log-based) ------------------------------------')
print(f'                    Session    Today     7-day avg')
print(f'  Commits:         {sess_commits:>6}    {t_commits:>6}     {w_per_day:.0f}/day')
print(f'  Failures:        {sess_failures:>6}    {t_failures:>6}     {w_fail_day:.0f}/day')
print(f'  Success rate:    {sess_rate:>5}%   {t_rate:>5}%    {w_rate:>4}%')
print(f'  Opus fallbacks:  {sess_opus_fb:>6}    {t_opus:>6}     {w_opus_day:.0f}/day')
print(f'  Avg min/commit:       ?    {t_avg:>6}     {w_avg:>4}')
print(f'  Commits/hour:         ?    {t_cph:>6}     {w_cph:>4}')
print()

# Today sessions detail
today_sessions = [s for s in sessions if s['day'] == today]
if today_sessions:
    print(f'  Today: {len(today_sessions)} session(s), {t_hours:.1f}h')
    for idx, s in enumerate(today_sessions):
        end = today_sessions[idx+1]['start'] if idx+1 < len(today_sessions) else datetime.now()
        dur = (end - s['start']).total_seconds() / 3600
        st_str = s['start'].strftime('%H:%M')
        end_str = end.strftime('%H:%M') if idx+1 < len(today_sessions) else 'now'
        print(f'    S{idx+1}: {st_str}-{end_str} ({dur:.1f}h) — {s[\"commits\"]} commits, {s[\"failures\"]} failures')
print()

# 7-day daily breakdown
print('  7-day daily:')
for day_key in sorted(days.keys()):
    if day_key >= week_ago:
        c, f, r, o, _, _, h = calc(days[day_key])
        marker = ' <-- today' if day_key == today else ''
        print(f'    {day_key}: {c:>3} commits, {f:>2} fail, {r:>3}% ok, {o:>2} opus fb ({h:.1f}h){marker}')
"
```

Present the Performance Stats output as-is between the current session summary and overall progress sections.

### 2. Overall project progress (ALL sessions combined)
Run these commands to gather cumulative stats:

```bash
cd D:/github/mRemoteNG && python -c "
import sys, os, json, subprocess, re
sys.stdout.reconfigure(encoding='utf-8', errors='replace')

db_dir = '.project-roadmap/issues-db/upstream'

# 1. Count issues by our_status from DB
status_counts = {}
total_db = 0
for f in os.listdir(db_dir):
    if not f.endswith('.json'): continue
    total_db += 1
    try:
        data = json.load(open(os.path.join(db_dir, f), encoding='utf-8'))
        s = data.get('our_status', 'unknown')
        status_counts[s] = status_counts.get(s, 0) + 1
    except: pass

# 2. Count unique issues with fix commits (ground truth)
# Search both formats: fix(#N) and Fix #N:
commit_issues = set()
for grep_pat in ['^fix(#', '^Fix #']:
    result = subprocess.run(['git', 'log', '--oneline', f'--grep={grep_pat}'], capture_output=True, text=True)
    for line in result.stdout.strip().split('\n'):
        if not line: continue
        # Match fix(#123) or Fix #123: — require exact issue number boundary
        for m in re.finditer(r'(?:fix\(#|Fix #|fix #)(\d+)', line):
            num = int(m.group(1))
            # Exclude false substring matches (e.g. #2274 matching #274)
            # by checking the number exists in the DB
            padded = f'{num:04d}'
            if os.path.exists(os.path.join(db_dir, f'{padded}.json')):
                commit_issues.add(num)

# 3. Cross-reference: issues with commits vs DB status
has_commit_by_status = {}
for inum in commit_issues:
    padded = f'{inum:04d}'
    fp = os.path.join(db_dir, f'{padded}.json')
    if os.path.exists(fp):
        try:
            data = json.load(open(fp, encoding='utf-8'))
            s = data.get('our_status', 'unknown')
            has_commit_by_status[s] = has_commit_by_status.get(s, 0) + 1
        except: pass

# 4. Calculate accurate numbers
issues_with_fixes = len(commit_issues)
closed_no_code = status_counts.get('wontfix', 0) + status_counts.get('duplicate', 0)
# Issues truly resolved = unique issues with fix commits + closed without code
truly_resolved = issues_with_fixes + closed_no_code
# Subtract any wontfix/duplicate that also have commits (avoid double count)
truly_resolved -= has_commit_by_status.get('wontfix', 0) + has_commit_by_status.get('duplicate', 0)

pct = int(truly_resolved / total_db * 100) if total_db else 0
bar_filled = int(pct / 100 * 30)
bar = chr(9608) * bar_filled + chr(9617) * (30 - bar_filled)

# 5. Remaining issues — split into "needs work" vs "awaiting verification"
remaining = total_db - truly_resolved
remaining_testing = status_counts.get('testing', 0)
remaining_triaged = status_counts.get('triaged', 0) - has_commit_by_status.get('triaged', 0)
remaining_new = status_counts.get('new', 0)
remaining_needs_human = status_counts.get('needs_human', 0)

# Count impl_failed (active retry queue for orchestrator)
impl_failed_count = 0
for f2 in os.listdir(db_dir):
    if not f2.endswith('.json'): continue
    try:
        d2 = json.load(open(os.path.join(db_dir, f2), encoding='utf-8'))
        if d2.get('impl_failed') and d2.get('our_status') == 'triaged':
            impl_failed_count += 1
    except: pass

needs_work = remaining_triaged + remaining_new + remaining_needs_human
awaiting_verification = remaining_testing

# Output for Claude to format
print(f'TOTAL_DB={total_db}')
print(f'ISSUES_WITH_FIXES={issues_with_fixes}')
print(f'CLOSED_NO_CODE={closed_no_code}')
print(f'TRULY_RESOLVED={truly_resolved}')
print(f'REMAINING={remaining}')
print(f'NEEDS_WORK={needs_work}')
print(f'AWAITING_VERIFICATION={awaiting_verification}')
print(f'IMPL_FAILED_QUEUE={impl_failed_count}')
print(f'PCT={pct}')
print(f'BAR=[{bar}]')
for s in ['released','testing','triaged','new','wontfix','duplicate','needs_human','impl_failed']:
    print(f'STATUS_{s}={status_counts.get(s, 0)}')
# Show status tracking gap
gap = has_commit_by_status.get('triaged', 0)
print(f'TRIAGED_WITH_COMMITS={gap}')
# Show calculation breakdown for verification
print()
print('--- Calculation ---')
print(f'  Issues with fix commits (unique): {issues_with_fixes}')
print(f'    of which: {has_commit_by_status}')
print(f'  Closed without code (wontfix+duplicate): {closed_no_code}')
print(f'  Double-counted (wontfix/dup with commits): -{has_commit_by_status.get(\"wontfix\", 0) + has_commit_by_status.get(\"duplicate\", 0)}')
print(f'  = Truly resolved: {issues_with_fixes} + {closed_no_code} - {has_commit_by_status.get(\"wontfix\", 0) + has_commit_by_status.get(\"duplicate\", 0)} = {truly_resolved}')
print(f'  = Remaining: {total_db} - {truly_resolved} = {remaining}')
print(f'    Awaiting verification (have commits): {awaiting_verification}')
print(f'    Needs work: {needs_work}')
print(f'      Triaged (retry queue): {remaining_triaged} ({impl_failed_count} in orchestrator queue)')
print(f'      New (not started):     {remaining_new}')
print(f'      Needs human:           {remaining_needs_human}')
"
```

Present the progress using the script output. Key metrics:
- **Issues with fix commits**: ground truth from git log, counting unique issue numbers
- **Closed without code**: wontfix + duplicate (no fix commit needed)
- **Truly resolved**: issues with fix commits + closed without code (deduplicated)
- **Remaining**: total - truly resolved, split into:
  - **Awaiting verification**: have fix commits but not yet released (testing status)
  - **Needs work**: triaged (orchestrator retry queue), new, needs human
- **Calculation**: always show the math so the user can verify
- **Status tracking gap**: issues marked "triaged" that actually have fix commits
  (flag this as a warning if > 0)

### 3. New issues (last 7 days)
Check for recently created issues on both upstream and fork:

```bash
# Upstream new issues (last 7 days)
gh issue list --repo mRemoteNG/mRemoteNG --state open --json number,title,createdAt,labels --limit 10 | python -c "
import json, sys
from datetime import datetime, timedelta
cutoff = datetime.utcnow() - timedelta(days=7)
issues = json.load(sys.stdin)
recent = [i for i in issues if datetime.fromisoformat(i['createdAt'].rstrip('Z')) > cutoff]
for i in recent:
    labels = ', '.join(l['name'] for l in i.get('labels', []))
    lbl = f' [{labels}]' if labels else ''
    print(f'  #{i[\"number\"]} {i[\"title\"]}{lbl}')
if not recent:
    print('  (none)')
"

# Fork new issues (last 7 days)
gh issue list --repo robertpopa22/mRemoteNG --state open --json number,title,createdAt --limit 10 | python -c "
import json, sys
from datetime import datetime, timedelta
cutoff = datetime.utcnow() - timedelta(days=7)
issues = json.load(sys.stdin)
recent = [i for i in issues if datetime.fromisoformat(i['createdAt'].rstrip('Z')) > cutoff]
for i in recent:
    print(f'  #{i[\"number\"]} {i[\"title\"]}')
if not recent:
    print('  (none)')
"
```

### 4. If no status file exists
Inform the user that no orchestrator run has been recorded yet, but still show
overall progress and new issues sections.

## Format

Present the status in a clean, readable format. Example:

```
IIS Orchestrator Status: RUNNING                          Report: 2026-02-25 10:55
Phase: issues | Task: Claude fixing #730 (Remote printing) — Sonnet

--- Processes --------------------------------------------------
  PID   Start     CPU  Mem   Role
  35156 08:39:21  17.9  17M  Supervisor (orchestrator_supervisor.py)
  30352 08:39:25   6.8  25M  Orchestrator (iis_orchestrator.py --force)
  25260 08:40:12  22.1  16M  WARNING: 2nd Supervisor (orphan?)
  Health: 2 supervisors detected — older one may be orphan from nohup

Issues (this session): 217 synced | 12/217 (6%)
  Implemented: 9 | Failed: 0 | Commented: 9

Commits (this session): 9
  [OK] 053888b fix(#1986): Can't connection to sql database
  [OK] c1f3877 fix(#343): Minimize tabs on connect
  ...

Errors: 3
  [08:51] #1905 — all agents failed
  [09:40] #551  — all agents failed

Cost: $22.97 | Duration: 02:16

--- Performance (log-based) ------------------------------------
                    Session    Today     7-day avg
  Commits:              8       25        22/day
  Failures:             1        4         3/day
  Success rate:        89%      86%       88%
  Opus fallbacks:       2        9         6/day
  Avg min/commit:       ?       21        19
  Commits/hour:         ?      2.8       3.2

  Today: 2 sessions, 8.7h
    S1: 23:12-05:17 (6.1h) — 17 commits, 4 failures
    S2: 05:17-now    (2.7h) —  8 commits, 1 failure

  7-day daily:
    2026-02-19:  12 commits,  2 fail, 86% ok,  3 opus fb (5.2h)
    2026-02-20:  18 commits,  1 fail, 95% ok,  1 opus fb (7.1h)
    2026-02-25:  25 commits,  4 fail, 86% ok,  9 opus fb (8.7h) <-- today

--- Overall Progress (all sessions) ----------------------------
Issues DB: 839 total
  DB Status:  Released: 505 | Testing: 15 | Wontfix: 56 | Duplicate: 24
              Triaged: 86 | New: 149 | Needs Human: 4

  Verified (cross-referenced with git):
    Issues with fix commits: 475 unique
    Closed without code:      80 (wontfix + duplicate)
    Truly resolved:          548/839
    [███████████████████░░░░░░░░░░░] 65%

  Remaining: 291
    Awaiting verification (have commits): 102
    Needs work: 189
      Triaged (retry queue): 143 (143 in orchestrator queue)
      New (not started):       42
      Needs human:              4

  Calculation: 475 + 80 - 7 (double-counted) = 548 resolved
               839 - 548 = 291 remaining (102 awaiting verification + 189 needs work)

Tests: 2925 passing

--- New Issues (last 7 days) -----------------------------------
Upstream (mRemoteNG/mRemoteNG):
  #3173 Possible command injection via Process.Start (2026-02-24)
  #3170 Opening Command sent too soon during SSH login (2026-02-24)
  #3167 mRemoteNG 1.78.2 requires .NET 9.0 (2026-02-23)
  #3166 Jump-host tabs wrong resolution after RDP reconnect (2026-02-23)

Fork (robertpopa22/mRemoteNG):
  #19 [No application window] mRemoteNG 1.81.0-beta.3
```
