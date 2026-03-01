# /iis-supervisor — Stop orchestrator & restart with supervisor

Stops any running orchestrator instance (from any session), then restarts it
under the self-healing supervisor wrapper.

## Usage

The user may specify arguments after the command:
- `/iis-supervisor` — stop + restart with default args (all modes)
- `/iis-supervisor issues` — restart focused on issues only
- `/iis-supervisor issues --max-issues 10` — restart with limits
- `/iis-supervisor stop` — stop only, do not restart
- `/iis-supervisor status` — show current health without changes

## What to do

### Step 1: Health check (always first)
```bash
python D:/github/mRemoteNG/.project-roadmap/scripts/orchestrator_supervisor.py --check
```
Show the user what was found (healthy / failures detected + recovered).

### Step 2: Stop any running orchestrator
```bash
# Check for running instances
ps aux | grep -i "iis_orchestrator\|orchestrator_supervisor" | grep -v grep

# Check lock file
cat D:/github/mRemoteNG/.project-roadmap/scripts/orchestrator.lock 2>/dev/null || echo "No lock file"
```

If processes found, kill them:
```bash
# Kill orchestrator processes (get PIDs from ps output above)
kill <PID>

# Remove stale lock file
rm -f D:/github/mRemoteNG/.project-roadmap/scripts/orchestrator.lock
```

If user specified `stop` — stop here. Report what was killed and exit.

### Step 3: Verify clean state
```bash
python D:/github/mRemoteNG/.project-roadmap/scripts/orchestrator_supervisor.py --check
```
Must be fully healthy before restart. If not, report issues to user.

### Step 4: Start supervisor (background)
If user specified `status` — skip this step.

Parse any arguments the user provided (e.g., `issues --max-issues 10`) and pass them:
```bash
python D:/github/mRemoteNG/.project-roadmap/scripts/orchestrator_supervisor.py --orchestrator-args "<user args>" &
```
Run in background. The supervisor will:
- Start the orchestrator
- Monitor every 30 seconds
- Auto-recover from 8 failure modes
- Restart on crash with exponential backoff

### Step 5: Confirm startup
Wait 5 seconds, then verify:
```bash
ps aux | grep -i "orchestrator_supervisor" | grep -v grep
cat D:/github/mRemoteNG/.project-roadmap/scripts/orchestrator-status.json 2>/dev/null
```

### Step 6: Report to user
Show:
- **Previous state**: what was running before (PIDs killed, lock cleaned)
- **Current state**: supervisor PID, orchestrator PID, health status
- **Mode**: what orchestrator args were passed
- **Monitoring**: supervisor checks every 30s, auto-recovers 8 failure modes
- **Logs**: `tail -f .project-roadmap/scripts/supervisor.log`

## Important notes
- The supervisor runs independently — closing this Claude session does NOT stop it
- Supervisor log: `.project-roadmap/scripts/supervisor.log`
- Orchestrator log: `.project-roadmap/scripts/orchestrator.log`
- To stop later: `kill <supervisor PID>` or `/iis-supervisor stop`
- Supervisor handles: stale locks, phantom processes, hung orchestrator, rate-limit cleanup, status file corruption, crashed process recovery
