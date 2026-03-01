# /iis-update — Update IIS Orchestrator Configuration

Update or improve the IIS Orchestrator script based on findings from recent runs.

## Usage

The user will describe what needs to be changed:
- `/iis-update add exclusion for TestX` — add a test to the exclusion filter
- `/iis-update increase timeout to 20min` — change CLAUDE_TIMEOUT
- `/iis-update improve prompt for cascade` — enhance the warning fix prompt
- `/iis-update add stale process xyz.exe` — add to STALE_PROCESSES list

## What to do

1. Read the current orchestrator script:
   ```
   D:\github\mRemoteNG\.project-roadmap\scripts\iis_orchestrator.py
   ```
2. Read the latest errors from the log/status to understand the issue:
   ```
   D:\github\mRemoteNG\.project-roadmap\scripts\orchestrator.log
   D:\github\mRemoteNG\.project-roadmap\scripts\orchestrator-status.json
   ```
3. Make the requested changes to `iis_orchestrator.py`
4. Show the user what was changed
5. Do NOT run the orchestrator — just update the script

## Common updates

### Test exclusion filter
Location: `TEST_FILTER` variable (line ~43)
Format: `&FullyQualifiedName!~TestClassName`

### Stale processes
Location: `STALE_PROCESSES` list (line ~178)
Add process name as string to the list.

### Claude prompt improvement
Location: `flux_warnings()` function, the `prompt` variable
Be careful with f-string formatting and triple quotes.

### Timeouts
Location: Constants at top of file:
- `BUILD_TIMEOUT = 300`
- `TEST_TIMEOUT = 300`
- `CLAUDE_TIMEOUT = 600`

### Warning codes
Location: `WARNING_CODES` list (line ~67)
Order = priority order for fixing.
