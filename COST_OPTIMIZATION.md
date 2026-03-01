# Cost Optimization & Budget Estimation — mRemoteNG IIS Orchestrator

> Generated: 2026-02-26 based on orchestrator.log analysis ($323.93 spent, 112 commits)

---

## Budget Estimation: Can we finish with 25% remaining?

### Current State
| Metric | Value |
|--------|-------|
| Total issues in DB | 842 |
| Resolved (commits + wontfix/dup) | 615 (73%) |
| **Remaining to implement** | **176 triaged** |
| Testing (need verification) | 17 |
| Needs human | 4 |
| Time until deadline (Mar 1, 19:00) | ~73h (3.1 days) |

### Cost Projections

**Scenario A: Current rates (Sonnet-primary, historical)**
- Cost per committed issue: $2.36 avg
- Cost per failed issue: $0.44 avg
- Success rate: 76%
- Per issue attempt: $2.36 × 0.76 + $0.44 × 0.24 = $1.90
- 176 issues × $1.90 = **~$334 needed**
- Plus retries on failures: 176 × 0.24 × $1.90 = **+$80**
- **Total estimate: ~$414**

**Scenario B: Opus-only (new flow, activated today)**
- Expected cost per impl call: ~$1.70 (Opus direct)
- Expected success rate: ~85% (based on Opus fallback data: 89%)
- Triage: $0.16/issue
- Per issue: ($1.70 + $0.16) = $1.86
- 176 issues × $1.86 = **~$327**
- Retries: 176 × 0.15 × $1.86 = +$49
- **Total estimate: ~$376**

**Scenario C: Opus-only + optimizations (see below)**
- Reduced output tokens (--max-turns 8): ~$1.20/impl
- Retry cap (max 2): eliminates money sinks
- Per issue: $1.20 + $0.16 = $1.36
- 176 issues × $1.36 = **~$239**
- Retries (capped): 176 × 0.15 × $1.36 = +$36
- **Total estimate: ~$275**

### Verdict

| Scenario | Cost | 25% limit needed | Fits in 25%? |
|----------|------|-------------------|--------------|
| A (old Sonnet flow) | ~$414 | $1,656 | YES |
| B (Opus-only, current) | ~$376 | $1,504 | YES |
| C (Opus + optimizations) | ~$275 | $1,100 | YES |

**DA, 25% e suficient** pentru toate scenariile. Chiar si in worst-case ($414), 25% din orice limita Claude API rezonabila ($500+/mo) acopera.

### Time constraint (more important!)

| Rate | Issues/hour | Time for 176 issues | Fits in 73h? |
|------|-------------|---------------------|--------------|
| Current: 2.8 commits/h | 2.8 | 63h | TIGHT |
| With Opus (faster resolve): 3.5/h | 3.5 | 50h | YES |
| With optimizations: 4.0/h | 4.0 | 44h | YES |

**Timpul e mai critic decat bugetul.** La rata curenta de ~2.8 commits/ora si ~60% uptime, avem ~44h productive. Cu 2.8/h = ~123 issues. **Nu vom termina toate 176 fara optimizari.**

Cu Opus + optimizari (4.0/h): ~176 issues in 44h = fezabil dar strans.

---

## Waste Analysis Summary

### Where money goes (total: $323.93)

| Category | Amount | % | Status |
|----------|--------|---|--------|
| Double-pay (Sonnet fail + Opus retry) | $151.31 | 47% | **FIXED** (Opus-only) |
| Failed issues (never committed) | $60.13 | 18% | Reducible |
| Triage overhead | $23.94 | 7% | OK (cheap) |
| Pre-analysis | $2.39 | <1% | Disabled |
| Productive spend | ~$86 | 27% | Good |

### Key metrics
- Avg cost per successful commit: **$2.36**
- Avg cost per failed issue: **$0.44**
- Success rate: **76%** (all-time) → **83%** (today with Opus)
- Output/Input token ratio: **39:1** (output dominates cost)

---

## Optimization Recommendations

### 1. DONE: Opus-only implementation
- **Impact**: Eliminates $87 double-pay waste (27% of total)
- **Status**: Activated 2026-02-26
- **Expected savings**: ~$0.70/issue

### 2. Cap retries per issue (max 2 attempts)
- **Impact**: Prevents money sinks like #551 ($13.46, 8 calls) and #1905 ($11.34, 5 calls)
- **Implementation**: In `chain_implement()`, track attempt count per issue in DB. After 2 failures, mark `impl_failed` and skip.
- **Expected savings**: ~$15-20 over remaining 176 issues
- **Priority**: HIGH — easy to implement, direct cost savings

### 3. Reduce output tokens with --max-turns
- **Impact**: Current avg 15,322 output tokens/implement call. Reducing to 8,000-10,000 saves ~35% per call.
- **Implementation**: Change `max_turns=15` to `max_turns=8` for implement tasks. Claude will be forced to be more concise.
- **Risk**: Some complex issues may need more turns. Monitor success rate.
- **Expected savings**: ~$0.50/issue × 176 = ~$88
- **Priority**: HIGH — biggest potential savings

### 4. Mark persistent failures as needs_human
- **Impact**: Stop throwing money at issues AI can't solve
- **Implementation**: After 2 failed implement attempts across sessions, auto-set `our_status: "needs_human"` in issue JSON
- **Expected savings**: ~$5-10 (small but prevents frustration)
- **Priority**: MEDIUM

### 5. Smarter triage filtering
- **Impact**: Current 75% conversion (triage → implement). 25% triaged never get implemented.
- **Implementation**: Add difficulty scoring to triage. Skip issues that are clearly too complex (multi-repo, needs upstream changes, hardware-specific).
- **Expected savings**: 176 × 0.25 × $0.16 = ~$7 (minimal)
- **Priority**: LOW — triage is already cheap

### 6. Prompt optimization for concise output
- **Impact**: Output tokens are 39x input. Adding "Be concise. Output only code changes, no explanations." to impl prompt.
- **Implementation**: Modify `impl_prompt` in `chain_implement()` to explicitly request minimal output.
- **Expected savings**: ~20-30% output reduction = ~$0.40/issue
- **Priority**: MEDIUM — easy to implement

### 7. Skip already-attempted issues across sessions
- **Impact**: Some issues are retried across sessions because the orchestrator doesn't remember previous failures.
- **Implementation**: Check `chain-context/` files for previous failed attempts before starting. Already partially implemented.
- **Expected savings**: ~$5-10
- **Priority**: LOW — mostly working already

---

## General Claude Usage Lessons (Beyond This Project)

### Cost drivers (in order of impact)
1. **Output tokens** — 5x more expensive than input. Keep responses short.
2. **Retries** — 1 good call beats 3 mediocre calls.
3. **Model choice** — Cost per TASK, not per CALL. Opus at $1.70 that succeeds > Sonnet at $0.80 that needs retry.
4. **Context size** — Grows per turn in multi-turn sessions. Short sessions = cheaper.

### Anti-patterns to avoid
| Anti-pattern | Why it's expensive | Better approach |
|--------------|-------------------|----------------|
| Sonnet-first + Opus-fallback | Pay twice for same task | Go Opus directly for complex tasks |
| Unlimited retries | Money sink on impossible tasks | Cap at 2-3 attempts |
| "Explain and fix" prompts | Double output (explanation + code) | "Fix only" then "explain" separately if needed |
| Long multi-turn sessions | Context grows, each turn more expensive | Short focused sessions, 5-8 turns max |
| Vague prompts | Claude writes novels trying to cover all bases | Specific prompts with exact scope |
| Reading entire files | Huge context for small changes | Send only relevant function + 10 lines context |

### Model selection guide
| Task | Best model | Why |
|------|-----------|-----|
| Triage / classification | Sonnet or Haiku | Cheap, fast, sufficient |
| Simple single-file fix | Sonnet | Good enough, 3x cheaper |
| Complex multi-file change | Opus | Higher success rate saves retries |
| Architecture / planning | Opus | Needs deep reasoning |
| Code review | Sonnet | Pattern matching, not creation |

### ROI metrics to track
- **Cost per resolved task** (not per API call)
- **Success rate by model** (to calibrate model choice)
- **Output token ratio** (output/input — target <20:1)
- **Retry rate** (>20% means prompts or model need improvement)
