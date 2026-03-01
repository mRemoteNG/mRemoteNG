# /iis-orchestrator — Full IIS Session (sync + analyze + orchestrate + report)

Run a complete Issue Intelligence System session: sync from GitHub, analyze, run AI triage/fix, generate reports.

## Usage

The user may specify arguments after the command:
- `/iis-orchestrator` — **full session**: sync → analyze → orchestrate issues → report
- `/iis-orchestrator quick` — sync + analyze + report only (no AI triage, ~15 min)
- `/iis-orchestrator issues` — full session focused on issues only
- `/iis-orchestrator warnings` — full session focused on warnings only
- `/iis-orchestrator issues --max-issues 10` — limit AI triage to 10 issues
- `/iis-orchestrator warnings --max-files 5` — limit files processed
- `/iis-orchestrator --dry-run` — simulate without changes

## What to do

### Step 1: Sync (MANDATORY — always first)
```bash
python D:/github/mRemoteNG/.project-roadmap/scripts/iis_orchestrator.py sync
```
Run in background. Expected duration: ~14 min for 830+ issues. Monitor progress via status output.

### Step 2: Analyze
```bash
python D:/github/mRemoteNG/.project-roadmap/scripts/iis_orchestrator.py analyze
```
Quick — shows categorized issues. Capture the output summary for the user.

### Step 3: Report (pre-orchestrator snapshot)
```bash
python D:/github/mRemoteNG/.project-roadmap/scripts/iis_orchestrator.py report --include-all
```
Generates markdown report. Note the stats for comparison later.

### Step 4: Orchestrate (AI triage + fix)
Skip this step if user specified `quick` mode.
```bash
python D:/github/mRemoteNG/.project-roadmap/scripts/iis_orchestrator.py <issues|warnings|all> [args]
```
Run in background. Expected duration:
- **issues**: ~3 hours for 800 issues (~13s per issue via claude -p)
- **warnings**: ~30 min for 50 files (~36s per file)

Monitor progress by reading the status file periodically:
```bash
cat D:/github/mRemoteNG/.project-roadmap/scripts/orchestrator-status.json
```

### Step 5: Final report
```bash
python D:/github/mRemoteNG/.project-roadmap/scripts/iis_orchestrator.py report --include-all
```
Compare with Step 3 stats to show what changed.

### Step 6: Present summary
Show the user:
- **Sync**: issues synced, new comments, waiting for us count
- **Orchestrator**: triaged / implemented / wontfix / duplicate / needs_info / failed
- **Reports**: link to generated report file
- **Commits**: any commits made by the orchestrator
- **Errors**: list of failures
- **Duration**: total session time (sync + orchestrate)
- **Delta**: what changed vs pre-orchestrator state

## Time estimates

| Mode | Duration | Notes |
|------|----------|-------|
| `quick` | ~15 min | Sync only, no AI |
| `issues` | ~3 hours | 800 issues via claude -p |
| `warnings` | ~30 min | Depends on warning count |
| `all` | ~3.5 hours | Issues + warnings |

## Important notes

- The script uses `claude -p` (headless mode) as sub-agent — it strips CLAUDECODE env vars automatically
- Each file fix is verified independently (build + test) and reverted on failure
- Commits are pushed to origin at the end
- The orchestrator kills stale processes (notepad.exe, testhost.exe) after every step
- Log file: `.project-roadmap/scripts/orchestrator.log`
- Status file: `.project-roadmap/scripts/orchestrator-status.json`
- **JSON DB files are updated during orchestrator run** — each triage decision writes priority, notes, and status to the issue JSON
