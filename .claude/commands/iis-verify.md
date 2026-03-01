# /iis-verify — Full Quality Verification Report

Run a complete quality check (build, tests, analyzer warnings, CI status, SonarCloud)
and produce a single actionable checklist of everything that needs fixing.

## What to do

### 1. Build Verification
```bash
cd D:/github/mRemoteNG && pwsh -NoProfile -ExecutionPolicy Bypass -File build.ps1 -NoRestore 2>&1 | tee /tmp/iis-verify-build.txt | tail -5
```
Extract:
- Build result (success/fail)
- Build time
- Error count: `grep -c ": error " /tmp/iis-verify-build.txt`

If build fails, STOP and report errors immediately.

### 2. Analyzer Warnings Summary
```bash
grep -E ": warning (CA|MA|RCS)" /tmp/iis-verify-build.txt | grep -v wpftmp | sed 's/.*warning //' | sed 's/ \[.*//' | cut -d: -f1 | sort | uniq -c | sort -rn > /tmp/iis-verify-warnings.txt
cat /tmp/iis-verify-warnings.txt
```
Calculate total: `awk '{s+=$1}END{print s}' /tmp/iis-verify-warnings.txt`

### 3. Test Verification
```bash
cd D:/github/mRemoteNG && bash run-tests-core.sh 2>&1 | tee /tmp/iis-verify-tests.txt | tail -15
```
Extract: Total, Passed, Failed, Time.

### 4. CI Status (GitHub Actions)
```bash
# Last 5 workflow runs on fork
gh run list --repo robertpopa22/mRemoteNG --limit 5 --json status,conclusion,name,headBranch,createdAt,databaseId | python -c "
import json, sys
sys.stdout.reconfigure(encoding='utf-8', errors='replace')
runs = json.load(sys.stdin)
for r in runs:
    status = r.get('conclusion') or r.get('status', '?')
    icon = {'success':'OK','failure':'FAIL','cancelled':'SKIP','in_progress':'RUN'}.get(status, status)
    print(f'  [{icon:>4}] {r[\"name\"]:30s} {r[\"headBranch\"]:15s} {r[\"createdAt\"][:16]}  id={r[\"databaseId\"]}')
"

# If any FAIL, get details
gh run list --repo robertpopa22/mRemoteNG --limit 5 --json conclusion,databaseId | python -c "
import json, sys
runs = json.load(sys.stdin)
failed = [r for r in runs if r.get('conclusion') == 'failure']
if failed:
    print(f'FAILED_RUN_IDS={\" \".join(str(r[\"databaseId\"]) for r in failed)}')
else:
    print('NO_FAILURES=1')
" > /tmp/iis-verify-ci-status.txt
cat /tmp/iis-verify-ci-status.txt
```

For each failed run, get the failure details:
```bash
# Only run if there are failures
source /tmp/iis-verify-ci-status.txt 2>/dev/null
if [ -z "$NO_FAILURES" ] && [ -n "$FAILED_RUN_IDS" ]; then
  for rid in $FAILED_RUN_IDS; do
    echo "=== Run $rid ==="
    gh run view $rid --repo robertpopa22/mRemoteNG --log-failed 2>&1 | tail -30
  done
fi
```

### 5. SonarCloud Status
```bash
# Get SonarCloud quality gate status
curl -s "https://sonarcloud.io/api/qualitygates/project_status?projectKey=robertpopa22_mRemoteNG" 2>/dev/null | python -c "
import json, sys
sys.stdout.reconfigure(encoding='utf-8', errors='replace')
try:
    data = json.load(sys.stdin)
    status = data.get('projectStatus', {})
    gate = status.get('status', 'UNKNOWN')
    print(f'Quality Gate: {gate}')
    for cond in status.get('conditions', []):
        s = cond.get('status', '?')
        metric = cond.get('metricKey', '?')
        actual = cond.get('actualValue', '?')
        threshold = cond.get('errorThreshold', '?')
        icon = 'OK' if s == 'OK' else 'FAIL'
        print(f'  [{icon:>4}] {metric}: {actual} (threshold: {threshold})')
except Exception as e:
    print(f'SonarCloud API error: {e}')
" 2>/dev/null || echo "SonarCloud: unavailable (check SONAR_TOKEN)"
```

### 6. Git Status
```bash
cd D:/github/mRemoteNG && echo "Branch: $(git branch --show-current)" && echo "Uncommitted:" && git status --short | head -20 && echo "Unpushed:" && git log --oneline origin/main..HEAD | head -10
```

### 7. Warning Fixability Analysis
Classify each warning rule into fixability tiers:
```bash
cat /tmp/iis-verify-warnings.txt | python -c "
import sys
sys.stdout.reconfigure(encoding='utf-8', errors='replace')

# Classification of rules by fix difficulty
AUTOFIX = {
    'CA1805': 'Remove default init',
    'CA1510': 'Use ThrowIfNull',
    'CA2263': 'Use generic Enum overload',
    'CA1825': 'Use Array.Empty<T>()',
    'CA1834': 'Use StringBuilder.Append(char)',
    'CA1822': 'Mark as static',
    'CA1507': 'Use nameof',
    'CA1865': 'Use char overload',
    'CA1866': 'Use char.IsAscii*',
    'CA1847': 'Use string.Contains(char)',
    'CA1854': 'Use TryGetValue',
    'CA1875': 'Use Array/Span overload',
    'RCS1075': 'Avoid empty catch',
    'CA1512': 'Use ObjectDisposedException.ThrowIf',
    'CA1513': 'Use ObjectDisposedException.ThrowIf',
}

SIMPLE_AGENT = {
    'CA1310': 'Specify StringComparison',
    'CA1309': 'Use ordinal StringComparison',
    'CA1304': 'Specify CultureInfo',
    'MA0074': 'Use StringComparison overload',
    'MA0006': 'Use string.Equals',
    'CA1806': 'Don\\'t ignore return value',
    'CA2201': 'Don\\'t raise base Exception',
    'CA1069': 'Enum duplicate values',
    'CA1051': 'Don\\'t expose public fields',
    'CA2211': 'Don\\'t expose static mutable fields',
    'CA1711': 'Remove type suffix from name',
    'CA1725': 'Match parameter names',
    'CA1707': 'Remove underscore (suppress in tests)',
    'CA1859': 'Use concrete type for perf',
    'CA1862': 'Use char-based overloads',
    'MA0009': 'Add regex timeout',
    'MA0015': 'Specify enum comparison',
    'MA0016': 'Specify enum comparison',
    'MA0019': 'Use EventArgs.Empty',
    'MA0056': 'Add cancellation token',
    'RCS1102': 'Make class static',
    'CA1816': 'Call GC.SuppressFinalize',
    'CA2208': 'Correct ArgumentException usage',
    'CA2251': 'Use string.Equals',
    'CA1838': 'Avoid StringBuilder for P/Invoke',
    'CA1850': 'Prefer HashData over ComputeHash',
    'CA1872': 'Prefer Convert.ToHexString',
    'CA1868': 'Unnecessary Contains before Remove',
    'CA1869': 'Cache ILogger',
    'MA0005': 'Use Array.Empty',
    'MA0008': 'Add StructLayoutAttribute',
    'MA0012': 'Don\\'t raise in property',
    'MA0023': 'Add region',
    'MA0047': 'Declare types in namespace',
    'MA0091': 'Sender should be this',
    'MA0134': 'Observe result of async call',
    'MA0144': 'Use short-circuiting operator',
    'MA0158': 'Use System.Threading.Lock',
}

CAREFUL = {
    'CA1305': 'Specify IFormatProvider',
    'MA0002': 'Use IEqualityComparer',
    'CA1863': 'Use CompositeFormat',
    'CA1311': 'Specify culture for ToUpper/ToLower',
    'CA2249': 'Use string.Contains(char, comparison)',
    'MA0069': 'Non-virtual public event',
    'MA0099': 'Use Regex source gen',
    'RCS1155': 'Use Enum.HasFlag',
    'CA1010': 'Implement generic collection',
    'CA1001': 'Implement IDisposable',
    'CA5351': 'Don\\'t use broken crypto',
    'CA5350': 'Don\\'t use weak crypto',
    'CA5369': 'Use XmlReader',
}

SUPPRESS = {
    'MA0021': 'Return Task directly (style)',
    'CA1036': 'Override comparison operators',
    'CA1050': 'Declare types in namespace',
    'CA1067': 'Override Equals',
    'CA1715': 'Interface prefix I',
    'CA1716': 'Don\\'t use language keywords',
    'CA1720': 'Don\\'t use type names',
    'CA1710': 'Use correct suffix',
    'CA1018': 'Mark with AttributeUsage',
    'CA2020': 'Prevent IntPtr zero',
    'CA2215': 'Call base.Dispose',
    'RCS1203': 'Use AttributeUsage',
    'MA0010': 'Mark IFormatProvider',
    'MA0014': 'Don\\'t raise in finally',
    'MA0025': 'Implement disposal pattern',
    'MA0055': 'Don\\'t use finalizer',
    'MA0060': 'Zero not first enum',
    'MA0061': 'Enum methods should use Enum',
    'MA0062': 'Non-flags should not have attr',
    'MA0065': 'Default constraint',
    'MA0084': 'Use short-form DateTimeOffset',
    'MA0095': 'Missing enum member',
    'MA0096': 'Default value for enum property',
    'MA0097': 'Class should be sealed/static',
    'MA0140': 'Both methods should exist',
}

tiers = {'Autofix (safe)': {}, 'Simple Agent': {}, 'Careful Review': {}, 'Suppress/Skip': {}, 'Unknown': {}}

total = 0
for line in sys.stdin:
    line = line.strip()
    if not line: continue
    parts = line.split()
    count = int(parts[0])
    code = parts[1]
    total += count

    if code in AUTOFIX:
        tiers['Autofix (safe)'][code] = (count, AUTOFIX[code])
    elif code in SIMPLE_AGENT:
        tiers['Simple Agent'][code] = (count, SIMPLE_AGENT[code])
    elif code in CAREFUL:
        tiers['Careful Review'][code] = (count, CAREFUL[code])
    elif code in SUPPRESS:
        tiers['Suppress/Skip'][code] = (count, SUPPRESS[code])
    else:
        tiers['Unknown'][code] = (count, '(unclassified)')

print(f'Total analyzer warnings: {total}')
print()

for tier_name, rules in tiers.items():
    if not rules: continue
    tier_total = sum(v[0] for v in rules.values())
    print(f'--- {tier_name} ({tier_total} warnings) ---')
    for code, (count, desc) in sorted(rules.items(), key=lambda x: -x[1][0]):
        print(f'  {count:>5}  {code}  {desc}')
    print()
"
```

## Output Format

Present as a single consolidated report:

```
=== mRemoteNG Quality Verification Report ===
Date: YYYY-MM-DD HH:MM

BUILD:      [OK/FAIL]  (time: Xs)
TESTS:      [OK/FAIL]  passed/total (Xs)
CI:         [OK/FAIL]  (last run details)
SONARCLOUD: [OK/FAIL]  (quality gate status)
GIT:        [CLEAN/DIRTY]  branch: main

--- Analyzer Warnings: NNNN total ---
  Autofix (safe):     NNN  (CA1510, CA2263, ...)
  Simple Agent:       NNN  (CA1310, CA1309, ...)
  Careful Review:     NNN  (CA1305, MA0002, ...)
  Suppress/Skip:      NNN  (CA1036, CA1720, ...)

--- Action Items ---
  [ ] Fix N build errors (if any)
  [ ] Fix N test failures (if any)
  [ ] Fix CI failure: workflow X (if any)
  [ ] Fix SonarCloud issues (if any)
  [ ] Fix NNN autofix-safe warnings (next batch: CA1510 64, CA2263 3, ...)
  [ ] Fix NNN agent-fixable warnings (CA1310 29, CA1309 28, ...)
  [ ] Review NNN careful warnings (CA1305 220, MA0002 230, ...)
  [ ] Suppress NNN noise warnings via .editorconfig
  [ ] Push N uncommitted changes (if any)

--- Detailed Warning Breakdown ---
  (full tier breakdown from step 7)
```

### Important Notes
- Run ALL steps (build, tests, CI, sonar) even if one fails — report everything at once
- The Action Items list should be ordered by priority: errors > test failures > CI > warnings
- For warnings, show the NEXT actionable batch (what to fix next)
- If tests are running slow, note it as a potential issue
- Compare warning count with previous run if available in git log
