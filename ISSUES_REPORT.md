# mRemoteNG — Open Issues Report

Complete snapshot of every open issue in [mRemoteNG/mRemoteNG](https://github.com/mRemoteNG/mRemoteNG/issues), captured for triage and implementation planning.

| | |
|---|---|
| **Source** | <https://github.com/mRemoteNG/mRemoteNG/issues> |
| **Extracted** | 2026-07-30 |
| **Open issues** | 835 |
| **Range** | #153 … #3394 |
| **Oldest opened** | 2016-10-07 |

<details>
<summary>How this list was captured, and why the count is trustworthy</summary>

The GitHub API was not reachable from the environment used to build this report, so the list was gathered from the issue search UI. Naive paging silently drops entries — the HTML-to-text conversion caps at roughly 12 rows per page while GitHub renders 25 — so the issue space was instead partitioned into narrow `created:` date windows, each verified by splitting it in half and checking that the halves reconcile against the whole.

The final total was confirmed by pagination arithmetic: the last page of the open-issue list holds exactly 10 entries, matching this set item-for-item, so 33 full pages x 25 + 10 = **835**. GitHub's Issues tab displays a rounded cached counter of 837; 835 is the exact figure.

</details>

## How to use this report

Every issue appears exactly once, under one category in [Issues by category](#issues-by-category). The indexes below are cross-cutting shortcuts into that same set.

The **Status** column is intentionally blank — fill it in as you triage (`investigating`, `fixed in <branch>`, `wontfix`, `duplicate of #NNNN`).

## Breakdown

| Category | Count |
|---|---:|
| [Security](#security-30) | 30 |
| [Bugs & Crashes](#bugs--crashes-154) | 154 |
| [Features & Enhancements](#features--enhancements-372) | 372 |
| [UI / UX](#ui--ux-57) | 57 |
| [Documentation](#documentation-3) | 3 |
| [Needs verification](#needs-verification-107) | 107 |
| [Other](#other-100) | 100 |
| [Untriaged (no labels)](#untriaged-no-labels-12) | 12 |
| **Total** | **835** |

| Year opened | Count |
|---|---:|
| 2016 | 31 |
| 2017 | 120 |
| 2018 | 121 |
| 2019 | 112 |
| 2020 | 94 |
| 2021 | 71 |
| 2022 | 95 |
| 2023 | 66 |
| 2024 | 36 |
| 2025 | 50 |
| 2026 | 39 |

<details>
<summary>Label frequency (top 30)</summary>

| Label | Count |
|---|---:|
| `Enhancement` | 254 |
| `Need 2 check` | 213 |
| `UI/UX` | 189 |
| `Bug` | 156 |
| `Improvement required` | 121 |
| `1.78.*` | 108 |
| `1.77.3` | 91 |
| `1.77.2` | 83 |
| `Connections` | 81 |
| `Needs implementation` | 55 |
| `DBs` | 45 |
| `Feature Request` | 43 |
| `RDP` | 40 |
| `1.8 (Fenix)` | 35 |
| `In development` | 34 |
| `VNC` | 32 |
| `In progress` | 30 |
| `Security` | 28 |
| `Priority - Low` | 26 |
| `Help Wanted` | 24 |
| `Verified` | 24 |
| `1.77.4` | 23 |
| `Third party` | 21 |
| `Windows` | 21 |
| `Nightly Build` | 21 |
| `Settings` | 18 |
| `Ready` | 16 |
| `Priority - High` | 16 |
| `Priority - Medium` | 14 |
| `.NET (dotNET)` | 14 |

</details>

---

## Start here

### By priority

**High (16)** — [#2633](https://github.com/mRemoteNG/mRemoteNG/issues/2633) · [#2500](https://github.com/mRemoteNG/mRemoteNG/issues/2500) · [#2420](https://github.com/mRemoteNG/mRemoteNG/issues/2420) · [#2419](https://github.com/mRemoteNG/mRemoteNG/issues/2419) · [#1936](https://github.com/mRemoteNG/mRemoteNG/issues/1936) · [#1892](https://github.com/mRemoteNG/mRemoteNG/issues/1892) · [#1853](https://github.com/mRemoteNG/mRemoteNG/issues/1853) · [#1808](https://github.com/mRemoteNG/mRemoteNG/issues/1808) · [#1649](https://github.com/mRemoteNG/mRemoteNG/issues/1649) · [#1585](https://github.com/mRemoteNG/mRemoteNG/issues/1585) · [#1114](https://github.com/mRemoteNG/mRemoteNG/issues/1114) · [#726](https://github.com/mRemoteNG/mRemoteNG/issues/726) · [#601](https://github.com/mRemoteNG/mRemoteNG/issues/601) · [#573](https://github.com/mRemoteNG/mRemoteNG/issues/573) · [#376](https://github.com/mRemoteNG/mRemoteNG/issues/376) · [#306](https://github.com/mRemoteNG/mRemoteNG/issues/306)

**Medium (14)** — [#2785](https://github.com/mRemoteNG/mRemoteNG/issues/2785) · [#2140](https://github.com/mRemoteNG/mRemoteNG/issues/2140) · [#1973](https://github.com/mRemoteNG/mRemoteNG/issues/1973) · [#1931](https://github.com/mRemoteNG/mRemoteNG/issues/1931) · [#1856](https://github.com/mRemoteNG/mRemoteNG/issues/1856) · [#1341](https://github.com/mRemoteNG/mRemoteNG/issues/1341) · [#1321](https://github.com/mRemoteNG/mRemoteNG/issues/1321) · [#1257](https://github.com/mRemoteNG/mRemoteNG/issues/1257) · [#1171](https://github.com/mRemoteNG/mRemoteNG/issues/1171) · [#461](https://github.com/mRemoteNG/mRemoteNG/issues/461) · [#450](https://github.com/mRemoteNG/mRemoteNG/issues/450) · [#336](https://github.com/mRemoteNG/mRemoteNG/issues/336) · [#183](https://github.com/mRemoteNG/mRemoteNG/issues/183) · [#181](https://github.com/mRemoteNG/mRemoteNG/issues/181)

**Low (26)** — [#3328](https://github.com/mRemoteNG/mRemoteNG/issues/3328) · [#2893](https://github.com/mRemoteNG/mRemoteNG/issues/2893) · [#2892](https://github.com/mRemoteNG/mRemoteNG/issues/2892) · [#2891](https://github.com/mRemoteNG/mRemoteNG/issues/2891) · [#2881](https://github.com/mRemoteNG/mRemoteNG/issues/2881) · [#2844](https://github.com/mRemoteNG/mRemoteNG/issues/2844) · [#2697](https://github.com/mRemoteNG/mRemoteNG/issues/2697) · [#2686](https://github.com/mRemoteNG/mRemoteNG/issues/2686) · [#2679](https://github.com/mRemoteNG/mRemoteNG/issues/2679) · [#2655](https://github.com/mRemoteNG/mRemoteNG/issues/2655) · [#2636](https://github.com/mRemoteNG/mRemoteNG/issues/2636) · [#2598](https://github.com/mRemoteNG/mRemoteNG/issues/2598) · [#2228](https://github.com/mRemoteNG/mRemoteNG/issues/2228) · [#2191](https://github.com/mRemoteNG/mRemoteNG/issues/2191) · [#2078](https://github.com/mRemoteNG/mRemoteNG/issues/2078) · [#2077](https://github.com/mRemoteNG/mRemoteNG/issues/2077) · [#2070](https://github.com/mRemoteNG/mRemoteNG/issues/2070) · [#2035](https://github.com/mRemoteNG/mRemoteNG/issues/2035) · [#1914](https://github.com/mRemoteNG/mRemoteNG/issues/1914) · [#1833](https://github.com/mRemoteNG/mRemoteNG/issues/1833) · [#1831](https://github.com/mRemoteNG/mRemoteNG/issues/1831) · [#1655](https://github.com/mRemoteNG/mRemoteNG/issues/1655) · [#346](https://github.com/mRemoteNG/mRemoteNG/issues/346) · [#330](https://github.com/mRemoteNG/mRemoteNG/issues/330) · [#314](https://github.com/mRemoteNG/mRemoteNG/issues/314) · [#202](https://github.com/mRemoteNG/mRemoteNG/issues/202)

### Already in flight

Someone has picked these up or marked them ready — check before starting work so you don't duplicate effort.

**In development (34)** — [#3333](https://github.com/mRemoteNG/mRemoteNG/issues/3333) · [#2843](https://github.com/mRemoteNG/mRemoteNG/issues/2843) · [#2831](https://github.com/mRemoteNG/mRemoteNG/issues/2831) · [#2721](https://github.com/mRemoteNG/mRemoteNG/issues/2721) · [#2666](https://github.com/mRemoteNG/mRemoteNG/issues/2666) · [#2648](https://github.com/mRemoteNG/mRemoteNG/issues/2648) · [#2633](https://github.com/mRemoteNG/mRemoteNG/issues/2633) · [#2585](https://github.com/mRemoteNG/mRemoteNG/issues/2585) · [#2554](https://github.com/mRemoteNG/mRemoteNG/issues/2554) · [#2472](https://github.com/mRemoteNG/mRemoteNG/issues/2472) · [#2460](https://github.com/mRemoteNG/mRemoteNG/issues/2460) · [#2455](https://github.com/mRemoteNG/mRemoteNG/issues/2455) · [#2436](https://github.com/mRemoteNG/mRemoteNG/issues/2436) · [#2429](https://github.com/mRemoteNG/mRemoteNG/issues/2429) · [#2333](https://github.com/mRemoteNG/mRemoteNG/issues/2333) · [#2331](https://github.com/mRemoteNG/mRemoteNG/issues/2331) · [#2306](https://github.com/mRemoteNG/mRemoteNG/issues/2306) · [#2305](https://github.com/mRemoteNG/mRemoteNG/issues/2305) · [#2290](https://github.com/mRemoteNG/mRemoteNG/issues/2290) · [#2270](https://github.com/mRemoteNG/mRemoteNG/issues/2270) · [#2201](https://github.com/mRemoteNG/mRemoteNG/issues/2201) · [#2189](https://github.com/mRemoteNG/mRemoteNG/issues/2189) · [#2173](https://github.com/mRemoteNG/mRemoteNG/issues/2173) · [#2068](https://github.com/mRemoteNG/mRemoteNG/issues/2068) · [#2051](https://github.com/mRemoteNG/mRemoteNG/issues/2051) · [#2032](https://github.com/mRemoteNG/mRemoteNG/issues/2032) · [#1985](https://github.com/mRemoteNG/mRemoteNG/issues/1985) · [#1982](https://github.com/mRemoteNG/mRemoteNG/issues/1982) · [#1871](https://github.com/mRemoteNG/mRemoteNG/issues/1871) · [#1856](https://github.com/mRemoteNG/mRemoteNG/issues/1856) · [#1571](https://github.com/mRemoteNG/mRemoteNG/issues/1571) · [#884](https://github.com/mRemoteNG/mRemoteNG/issues/884) · [#660](https://github.com/mRemoteNG/mRemoteNG/issues/660) · [#274](https://github.com/mRemoteNG/mRemoteNG/issues/274)

**In progress (30)** — [#3333](https://github.com/mRemoteNG/mRemoteNG/issues/3333) · [#3224](https://github.com/mRemoteNG/mRemoteNG/issues/3224) · [#2831](https://github.com/mRemoteNG/mRemoteNG/issues/2831) · [#2826](https://github.com/mRemoteNG/mRemoteNG/issues/2826) · [#2666](https://github.com/mRemoteNG/mRemoteNG/issues/2666) · [#2633](https://github.com/mRemoteNG/mRemoteNG/issues/2633) · [#2585](https://github.com/mRemoteNG/mRemoteNG/issues/2585) · [#2563](https://github.com/mRemoteNG/mRemoteNG/issues/2563) · [#2500](https://github.com/mRemoteNG/mRemoteNG/issues/2500) · [#2499](https://github.com/mRemoteNG/mRemoteNG/issues/2499) · [#2474](https://github.com/mRemoteNG/mRemoteNG/issues/2474) · [#2425](https://github.com/mRemoteNG/mRemoteNG/issues/2425) · [#2420](https://github.com/mRemoteNG/mRemoteNG/issues/2420) · [#2368](https://github.com/mRemoteNG/mRemoteNG/issues/2368) · [#2222](https://github.com/mRemoteNG/mRemoteNG/issues/2222) · [#1986](https://github.com/mRemoteNG/mRemoteNG/issues/1986) · [#1977](https://github.com/mRemoteNG/mRemoteNG/issues/1977) · [#1969](https://github.com/mRemoteNG/mRemoteNG/issues/1969) · [#1952](https://github.com/mRemoteNG/mRemoteNG/issues/1952) · [#1853](https://github.com/mRemoteNG/mRemoteNG/issues/1853) · [#1791](https://github.com/mRemoteNG/mRemoteNG/issues/1791) · [#1656](https://github.com/mRemoteNG/mRemoteNG/issues/1656) · [#1287](https://github.com/mRemoteNG/mRemoteNG/issues/1287) · [#1131](https://github.com/mRemoteNG/mRemoteNG/issues/1131) · [#1065](https://github.com/mRemoteNG/mRemoteNG/issues/1065) · [#1031](https://github.com/mRemoteNG/mRemoteNG/issues/1031) · [#822](https://github.com/mRemoteNG/mRemoteNG/issues/822) · [#660](https://github.com/mRemoteNG/mRemoteNG/issues/660) · [#287](https://github.com/mRemoteNG/mRemoteNG/issues/287) · [#242](https://github.com/mRemoteNG/mRemoteNG/issues/242)

**Ready (16)** — [#1359](https://github.com/mRemoteNG/mRemoteNG/issues/1359) · [#1257](https://github.com/mRemoteNG/mRemoteNG/issues/1257) · [#1171](https://github.com/mRemoteNG/mRemoteNG/issues/1171) · [#601](https://github.com/mRemoteNG/mRemoteNG/issues/601) · [#573](https://github.com/mRemoteNG/mRemoteNG/issues/573) · [#498](https://github.com/mRemoteNG/mRemoteNG/issues/498) · [#461](https://github.com/mRemoteNG/mRemoteNG/issues/461) · [#376](https://github.com/mRemoteNG/mRemoteNG/issues/376) · [#336](https://github.com/mRemoteNG/mRemoteNG/issues/336) · [#317](https://github.com/mRemoteNG/mRemoteNG/issues/317) · [#308](https://github.com/mRemoteNG/mRemoteNG/issues/308) · [#301](https://github.com/mRemoteNG/mRemoteNG/issues/301) · [#208](https://github.com/mRemoteNG/mRemoteNG/issues/208) · [#183](https://github.com/mRemoteNG/mRemoteNG/issues/183) · [#182](https://github.com/mRemoteNG/mRemoteNG/issues/182) · [#181](https://github.com/mRemoteNG/mRemoteNG/issues/181)

**Needs implementation (55)** — [#2809](https://github.com/mRemoteNG/mRemoteNG/issues/2809) · [#2756](https://github.com/mRemoteNG/mRemoteNG/issues/2756) · [#2666](https://github.com/mRemoteNG/mRemoteNG/issues/2666) · [#2655](https://github.com/mRemoteNG/mRemoteNG/issues/2655) · [#2633](https://github.com/mRemoteNG/mRemoteNG/issues/2633) · [#2562](https://github.com/mRemoteNG/mRemoteNG/issues/2562) · [#2480](https://github.com/mRemoteNG/mRemoteNG/issues/2480) · [#2467](https://github.com/mRemoteNG/mRemoteNG/issues/2467) · [#2445](https://github.com/mRemoteNG/mRemoteNG/issues/2445) · [#2417](https://github.com/mRemoteNG/mRemoteNG/issues/2417) · [#2389](https://github.com/mRemoteNG/mRemoteNG/issues/2389) · [#2349](https://github.com/mRemoteNG/mRemoteNG/issues/2349) · [#2325](https://github.com/mRemoteNG/mRemoteNG/issues/2325) · [#2313](https://github.com/mRemoteNG/mRemoteNG/issues/2313) · [#2310](https://github.com/mRemoteNG/mRemoteNG/issues/2310) · [#2277](https://github.com/mRemoteNG/mRemoteNG/issues/2277) · [#2270](https://github.com/mRemoteNG/mRemoteNG/issues/2270) · [#2250](https://github.com/mRemoteNG/mRemoteNG/issues/2250) · [#2191](https://github.com/mRemoteNG/mRemoteNG/issues/2191) · [#2181](https://github.com/mRemoteNG/mRemoteNG/issues/2181) · [#2173](https://github.com/mRemoteNG/mRemoteNG/issues/2173) · [#2140](https://github.com/mRemoteNG/mRemoteNG/issues/2140) · [#2134](https://github.com/mRemoteNG/mRemoteNG/issues/2134) · [#2068](https://github.com/mRemoteNG/mRemoteNG/issues/2068) · [#2035](https://github.com/mRemoteNG/mRemoteNG/issues/2035) · [#2018](https://github.com/mRemoteNG/mRemoteNG/issues/2018) · [#1985](https://github.com/mRemoteNG/mRemoteNG/issues/1985) · [#1982](https://github.com/mRemoteNG/mRemoteNG/issues/1982) · [#1902](https://github.com/mRemoteNG/mRemoteNG/issues/1902) · [#1901](https://github.com/mRemoteNG/mRemoteNG/issues/1901) · [#1896](https://github.com/mRemoteNG/mRemoteNG/issues/1896) · [#1893](https://github.com/mRemoteNG/mRemoteNG/issues/1893) · [#1869](https://github.com/mRemoteNG/mRemoteNG/issues/1869) · [#1828](https://github.com/mRemoteNG/mRemoteNG/issues/1828) · [#1804](https://github.com/mRemoteNG/mRemoteNG/issues/1804) · [#1692](https://github.com/mRemoteNG/mRemoteNG/issues/1692) · [#1676](https://github.com/mRemoteNG/mRemoteNG/issues/1676) · [#1649](https://github.com/mRemoteNG/mRemoteNG/issues/1649) · [#1640](https://github.com/mRemoteNG/mRemoteNG/issues/1640) · [#1515](https://github.com/mRemoteNG/mRemoteNG/issues/1515) · [#1424](https://github.com/mRemoteNG/mRemoteNG/issues/1424) · [#1137](https://github.com/mRemoteNG/mRemoteNG/issues/1137) · [#1105](https://github.com/mRemoteNG/mRemoteNG/issues/1105) · [#1041](https://github.com/mRemoteNG/mRemoteNG/issues/1041) · [#1033](https://github.com/mRemoteNG/mRemoteNG/issues/1033) · [#1026](https://github.com/mRemoteNG/mRemoteNG/issues/1026) · [#958](https://github.com/mRemoteNG/mRemoteNG/issues/958) · [#933](https://github.com/mRemoteNG/mRemoteNG/issues/933) · [#906](https://github.com/mRemoteNG/mRemoteNG/issues/906) · [#675](https://github.com/mRemoteNG/mRemoteNG/issues/675) · [#674](https://github.com/mRemoteNG/mRemoteNG/issues/674) · [#649](https://github.com/mRemoteNG/mRemoteNG/issues/649) · [#423](https://github.com/mRemoteNG/mRemoteNG/issues/423) · [#420](https://github.com/mRemoteNG/mRemoteNG/issues/420) · [#242](https://github.com/mRemoteNG/mRemoteNG/issues/242)

**Verified (24)** — [#2687](https://github.com/mRemoteNG/mRemoteNG/issues/2687) · [#2585](https://github.com/mRemoteNG/mRemoteNG/issues/2585) · [#2420](https://github.com/mRemoteNG/mRemoteNG/issues/2420) · [#2195](https://github.com/mRemoteNG/mRemoteNG/issues/2195) · [#2172](https://github.com/mRemoteNG/mRemoteNG/issues/2172) · [#1965](https://github.com/mRemoteNG/mRemoteNG/issues/1965) · [#1822](https://github.com/mRemoteNG/mRemoteNG/issues/1822) · [#1794](https://github.com/mRemoteNG/mRemoteNG/issues/1794) · [#1760](https://github.com/mRemoteNG/mRemoteNG/issues/1760) · [#1701](https://github.com/mRemoteNG/mRemoteNG/issues/1701) · [#1408](https://github.com/mRemoteNG/mRemoteNG/issues/1408) · [#1359](https://github.com/mRemoteNG/mRemoteNG/issues/1359) · [#1341](https://github.com/mRemoteNG/mRemoteNG/issues/1341) · [#1286](https://github.com/mRemoteNG/mRemoteNG/issues/1286) · [#1257](https://github.com/mRemoteNG/mRemoteNG/issues/1257) · [#1214](https://github.com/mRemoteNG/mRemoteNG/issues/1214) · [#1171](https://github.com/mRemoteNG/mRemoteNG/issues/1171) · [#1121](https://github.com/mRemoteNG/mRemoteNG/issues/1121) · [#853](https://github.com/mRemoteNG/mRemoteNG/issues/853) · [#794](https://github.com/mRemoteNG/mRemoteNG/issues/794) · [#498](https://github.com/mRemoteNG/mRemoteNG/issues/498) · [#330](https://github.com/mRemoteNG/mRemoteNG/issues/330) · [#303](https://github.com/mRemoteNG/mRemoteNG/issues/303) · [#227](https://github.com/mRemoteNG/mRemoteNG/issues/227)

### Not planned

Maintainers have declined these — skip them. [#2491](https://github.com/mRemoteNG/mRemoteNG/issues/2491) · [#1504](https://github.com/mRemoteNG/mRemoteNG/issues/1504) · [#1415](https://github.com/mRemoteNG/mRemoteNG/issues/1415) · [#1256](https://github.com/mRemoteNG/mRemoteNG/issues/1256) · [#1109](https://github.com/mRemoteNG/mRemoteNG/issues/1109) · [#937](https://github.com/mRemoteNG/mRemoteNG/issues/937) · [#346](https://github.com/mRemoteNG/mRemoteNG/issues/346) · [#314](https://github.com/mRemoteNG/mRemoteNG/issues/314)

### By component

| Component | Count | Issues |
|---|---:|---|
| `Connections` | 81 | [#3333](https://github.com/mRemoteNG/mRemoteNG/issues/3333) · [#3328](https://github.com/mRemoteNG/mRemoteNG/issues/3328) · [#3180](https://github.com/mRemoteNG/mRemoteNG/issues/3180) · [#2959](https://github.com/mRemoteNG/mRemoteNG/issues/2959) · [#2843](https://github.com/mRemoteNG/mRemoteNG/issues/2843) · [#2785](https://github.com/mRemoteNG/mRemoteNG/issues/2785) · [#2756](https://github.com/mRemoteNG/mRemoteNG/issues/2756) · [#2706](https://github.com/mRemoteNG/mRemoteNG/issues/2706) · [#2673](https://github.com/mRemoteNG/mRemoteNG/issues/2673) · [#2655](https://github.com/mRemoteNG/mRemoteNG/issues/2655) · [#2651](https://github.com/mRemoteNG/mRemoteNG/issues/2651) · [#2648](https://github.com/mRemoteNG/mRemoteNG/issues/2648) · [#2614](https://github.com/mRemoteNG/mRemoteNG/issues/2614) · [#2598](https://github.com/mRemoteNG/mRemoteNG/issues/2598) · [#2582](https://github.com/mRemoteNG/mRemoteNG/issues/2582) · [#2562](https://github.com/mRemoteNG/mRemoteNG/issues/2562) · [#2554](https://github.com/mRemoteNG/mRemoteNG/issues/2554) · [#2499](https://github.com/mRemoteNG/mRemoteNG/issues/2499) · [#2494](https://github.com/mRemoteNG/mRemoteNG/issues/2494) · [#2480](https://github.com/mRemoteNG/mRemoteNG/issues/2480) · [#2472](https://github.com/mRemoteNG/mRemoteNG/issues/2472) · [#2467](https://github.com/mRemoteNG/mRemoteNG/issues/2467) · [#2463](https://github.com/mRemoteNG/mRemoteNG/issues/2463) · [#2460](https://github.com/mRemoteNG/mRemoteNG/issues/2460) · [#2442](https://github.com/mRemoteNG/mRemoteNG/issues/2442) · [#2420](https://github.com/mRemoteNG/mRemoteNG/issues/2420) · [#2417](https://github.com/mRemoteNG/mRemoteNG/issues/2417) · [#2414](https://github.com/mRemoteNG/mRemoteNG/issues/2414) · [#2409](https://github.com/mRemoteNG/mRemoteNG/issues/2409) · [#2406](https://github.com/mRemoteNG/mRemoteNG/issues/2406) · [#2405](https://github.com/mRemoteNG/mRemoteNG/issues/2405) · [#2404](https://github.com/mRemoteNG/mRemoteNG/issues/2404) · [#2359](https://github.com/mRemoteNG/mRemoteNG/issues/2359) · [#2358](https://github.com/mRemoteNG/mRemoteNG/issues/2358) · [#2349](https://github.com/mRemoteNG/mRemoteNG/issues/2349) · [#2333](https://github.com/mRemoteNG/mRemoteNG/issues/2333) · [#2320](https://github.com/mRemoteNG/mRemoteNG/issues/2320) · [#2313](https://github.com/mRemoteNG/mRemoteNG/issues/2313) · [#2311](https://github.com/mRemoteNG/mRemoteNG/issues/2311) · [#2310](https://github.com/mRemoteNG/mRemoteNG/issues/2310) · [#2306](https://github.com/mRemoteNG/mRemoteNG/issues/2306) · [#2305](https://github.com/mRemoteNG/mRemoteNG/issues/2305) · [#2293](https://github.com/mRemoteNG/mRemoteNG/issues/2293) · [#2277](https://github.com/mRemoteNG/mRemoteNG/issues/2277) · [#2250](https://github.com/mRemoteNG/mRemoteNG/issues/2250) · [#2219](https://github.com/mRemoteNG/mRemoteNG/issues/2219) · [#2209](https://github.com/mRemoteNG/mRemoteNG/issues/2209) · [#2201](https://github.com/mRemoteNG/mRemoteNG/issues/2201) · [#2181](https://github.com/mRemoteNG/mRemoteNG/issues/2181) · [#2150](https://github.com/mRemoteNG/mRemoteNG/issues/2150) · [#2134](https://github.com/mRemoteNG/mRemoteNG/issues/2134) · [#2092](https://github.com/mRemoteNG/mRemoteNG/issues/2092) · [#2078](https://github.com/mRemoteNG/mRemoteNG/issues/2078) · [#2037](https://github.com/mRemoteNG/mRemoteNG/issues/2037) · [#2032](https://github.com/mRemoteNG/mRemoteNG/issues/2032) · [#2031](https://github.com/mRemoteNG/mRemoteNG/issues/2031) · [#2018](https://github.com/mRemoteNG/mRemoteNG/issues/2018) · [#2004](https://github.com/mRemoteNG/mRemoteNG/issues/2004) · [#1739](https://github.com/mRemoteNG/mRemoteNG/issues/1739) · [#1719](https://github.com/mRemoteNG/mRemoteNG/issues/1719) · [#1574](https://github.com/mRemoteNG/mRemoteNG/issues/1574) · [#1571](https://github.com/mRemoteNG/mRemoteNG/issues/1571) · [#1511](https://github.com/mRemoteNG/mRemoteNG/issues/1511) · [#1131](https://github.com/mRemoteNG/mRemoteNG/issues/1131) · [#1099](https://github.com/mRemoteNG/mRemoteNG/issues/1099) · [#1085](https://github.com/mRemoteNG/mRemoteNG/issues/1085) · [#1059](https://github.com/mRemoteNG/mRemoteNG/issues/1059) · [#1033](https://github.com/mRemoteNG/mRemoteNG/issues/1033) · [#1031](https://github.com/mRemoteNG/mRemoteNG/issues/1031) · [#1026](https://github.com/mRemoteNG/mRemoteNG/issues/1026) · [#1006](https://github.com/mRemoteNG/mRemoteNG/issues/1006) · [#980](https://github.com/mRemoteNG/mRemoteNG/issues/980) · [#906](https://github.com/mRemoteNG/mRemoteNG/issues/906) · [#884](https://github.com/mRemoteNG/mRemoteNG/issues/884) · [#839](https://github.com/mRemoteNG/mRemoteNG/issues/839) · [#834](https://github.com/mRemoteNG/mRemoteNG/issues/834) · [#822](https://github.com/mRemoteNG/mRemoteNG/issues/822) · [#726](https://github.com/mRemoteNG/mRemoteNG/issues/726) · [#435](https://github.com/mRemoteNG/mRemoteNG/issues/435) · [#242](https://github.com/mRemoteNG/mRemoteNG/issues/242) · [#213](https://github.com/mRemoteNG/mRemoteNG/issues/213) |
| `RDP` | 40 | [#3202](https://github.com/mRemoteNG/mRemoteNG/issues/3202) · [#3175](https://github.com/mRemoteNG/mRemoteNG/issues/3175) · [#2810](https://github.com/mRemoteNG/mRemoteNG/issues/2810) · [#2735](https://github.com/mRemoteNG/mRemoteNG/issues/2735) · [#2711](https://github.com/mRemoteNG/mRemoteNG/issues/2711) · [#2659](https://github.com/mRemoteNG/mRemoteNG/issues/2659) · [#2625](https://github.com/mRemoteNG/mRemoteNG/issues/2625) · [#2608](https://github.com/mRemoteNG/mRemoteNG/issues/2608) · [#2588](https://github.com/mRemoteNG/mRemoteNG/issues/2588) · [#2578](https://github.com/mRemoteNG/mRemoteNG/issues/2578) · [#2527](https://github.com/mRemoteNG/mRemoteNG/issues/2527) · [#2468](https://github.com/mRemoteNG/mRemoteNG/issues/2468) · [#2434](https://github.com/mRemoteNG/mRemoteNG/issues/2434) · [#2360](https://github.com/mRemoteNG/mRemoteNG/issues/2360) · [#2329](https://github.com/mRemoteNG/mRemoteNG/issues/2329) · [#2309](https://github.com/mRemoteNG/mRemoteNG/issues/2309) · [#2294](https://github.com/mRemoteNG/mRemoteNG/issues/2294) · [#2291](https://github.com/mRemoteNG/mRemoteNG/issues/2291) · [#2263](https://github.com/mRemoteNG/mRemoteNG/issues/2263) · [#2258](https://github.com/mRemoteNG/mRemoteNG/issues/2258) · [#2090](https://github.com/mRemoteNG/mRemoteNG/issues/2090) · [#2054](https://github.com/mRemoteNG/mRemoteNG/issues/2054) · [#1902](https://github.com/mRemoteNG/mRemoteNG/issues/1902) · [#1892](https://github.com/mRemoteNG/mRemoteNG/issues/1892) · [#1880](https://github.com/mRemoteNG/mRemoteNG/issues/1880) · [#1830](https://github.com/mRemoteNG/mRemoteNG/issues/1830) · [#1728](https://github.com/mRemoteNG/mRemoteNG/issues/1728) · [#1715](https://github.com/mRemoteNG/mRemoteNG/issues/1715) · [#1693](https://github.com/mRemoteNG/mRemoteNG/issues/1693) · [#1653](https://github.com/mRemoteNG/mRemoteNG/issues/1653) · [#1631](https://github.com/mRemoteNG/mRemoteNG/issues/1631) · [#1455](https://github.com/mRemoteNG/mRemoteNG/issues/1455) · [#1402](https://github.com/mRemoteNG/mRemoteNG/issues/1402) · [#1113](https://github.com/mRemoteNG/mRemoteNG/issues/1113) · [#1028](https://github.com/mRemoteNG/mRemoteNG/issues/1028) · [#1023](https://github.com/mRemoteNG/mRemoteNG/issues/1023) · [#825](https://github.com/mRemoteNG/mRemoteNG/issues/825) · [#824](https://github.com/mRemoteNG/mRemoteNG/issues/824) · [#463](https://github.com/mRemoteNG/mRemoteNG/issues/463) · [#192](https://github.com/mRemoteNG/mRemoteNG/issues/192) |
| `VNC` | 32 | [#3219](https://github.com/mRemoteNG/mRemoteNG/issues/3219) · [#2634](https://github.com/mRemoteNG/mRemoteNG/issues/2634) · [#2577](https://github.com/mRemoteNG/mRemoteNG/issues/2577) · [#2570](https://github.com/mRemoteNG/mRemoteNG/issues/2570) · [#2491](https://github.com/mRemoteNG/mRemoteNG/issues/2491) · [#2321](https://github.com/mRemoteNG/mRemoteNG/issues/2321) · [#1943](https://github.com/mRemoteNG/mRemoteNG/issues/1943) · [#1905](https://github.com/mRemoteNG/mRemoteNG/issues/1905) · [#1742](https://github.com/mRemoteNG/mRemoteNG/issues/1742) · [#1696](https://github.com/mRemoteNG/mRemoteNG/issues/1696) · [#1679](https://github.com/mRemoteNG/mRemoteNG/issues/1679) · [#1547](https://github.com/mRemoteNG/mRemoteNG/issues/1547) · [#1494](https://github.com/mRemoteNG/mRemoteNG/issues/1494) · [#1327](https://github.com/mRemoteNG/mRemoteNG/issues/1327) · [#1214](https://github.com/mRemoteNG/mRemoteNG/issues/1214) · [#1196](https://github.com/mRemoteNG/mRemoteNG/issues/1196) · [#1195](https://github.com/mRemoteNG/mRemoteNG/issues/1195) · [#1069](https://github.com/mRemoteNG/mRemoteNG/issues/1069) · [#1008](https://github.com/mRemoteNG/mRemoteNG/issues/1008) · [#678](https://github.com/mRemoteNG/mRemoteNG/issues/678) · [#656](https://github.com/mRemoteNG/mRemoteNG/issues/656) · [#640](https://github.com/mRemoteNG/mRemoteNG/issues/640) · [#636](https://github.com/mRemoteNG/mRemoteNG/issues/636) · [#579](https://github.com/mRemoteNG/mRemoteNG/issues/579) · [#573](https://github.com/mRemoteNG/mRemoteNG/issues/573) · [#509](https://github.com/mRemoteNG/mRemoteNG/issues/509) · [#494](https://github.com/mRemoteNG/mRemoteNG/issues/494) · [#484](https://github.com/mRemoteNG/mRemoteNG/issues/484) · [#461](https://github.com/mRemoteNG/mRemoteNG/issues/461) · [#444](https://github.com/mRemoteNG/mRemoteNG/issues/444) · [#274](https://github.com/mRemoteNG/mRemoteNG/issues/274) · [#227](https://github.com/mRemoteNG/mRemoteNG/issues/227) |
| `SSH` | 14 | [#3170](https://github.com/mRemoteNG/mRemoteNG/issues/3170) · [#2651](https://github.com/mRemoteNG/mRemoteNG/issues/2651) · [#2624](https://github.com/mRemoteNG/mRemoteNG/issues/2624) · [#2570](https://github.com/mRemoteNG/mRemoteNG/issues/2570) · [#2459](https://github.com/mRemoteNG/mRemoteNG/issues/2459) · [#2404](https://github.com/mRemoteNG/mRemoteNG/issues/2404) · [#2403](https://github.com/mRemoteNG/mRemoteNG/issues/2403) · [#2359](https://github.com/mRemoteNG/mRemoteNG/issues/2359) · [#2358](https://github.com/mRemoteNG/mRemoteNG/issues/2358) · [#2302](https://github.com/mRemoteNG/mRemoteNG/issues/2302) · [#2251](https://github.com/mRemoteNG/mRemoteNG/issues/2251) · [#1875](https://github.com/mRemoteNG/mRemoteNG/issues/1875) · [#1822](https://github.com/mRemoteNG/mRemoteNG/issues/1822) · [#1736](https://github.com/mRemoteNG/mRemoteNG/issues/1736) |
| `Putty` | 7 | [#2881](https://github.com/mRemoteNG/mRemoteNG/issues/2881) · [#2785](https://github.com/mRemoteNG/mRemoteNG/issues/2785) · [#2473](https://github.com/mRemoteNG/mRemoteNG/issues/2473) · [#2454](https://github.com/mRemoteNG/mRemoteNG/issues/2454) · [#2442](https://github.com/mRemoteNG/mRemoteNG/issues/2442) · [#2296](https://github.com/mRemoteNG/mRemoteNG/issues/2296) · [#398](https://github.com/mRemoteNG/mRemoteNG/issues/398) |
| `DBs` | 45 | [#3333](https://github.com/mRemoteNG/mRemoteNG/issues/3333) · [#3249](https://github.com/mRemoteNG/mRemoteNG/issues/3249) · [#3027](https://github.com/mRemoteNG/mRemoteNG/issues/3027) · [#3005](https://github.com/mRemoteNG/mRemoteNG/issues/3005) · [#2913](https://github.com/mRemoteNG/mRemoteNG/issues/2913) · [#2899](https://github.com/mRemoteNG/mRemoteNG/issues/2899) · [#2687](https://github.com/mRemoteNG/mRemoteNG/issues/2687) · [#2614](https://github.com/mRemoteNG/mRemoteNG/issues/2614) · [#2579](https://github.com/mRemoteNG/mRemoteNG/issues/2579) · [#2511](https://github.com/mRemoteNG/mRemoteNG/issues/2511) · [#2500](https://github.com/mRemoteNG/mRemoteNG/issues/2500) · [#2499](https://github.com/mRemoteNG/mRemoteNG/issues/2499) · [#2498](https://github.com/mRemoteNG/mRemoteNG/issues/2498) · [#2494](https://github.com/mRemoteNG/mRemoteNG/issues/2494) · [#2471](https://github.com/mRemoteNG/mRemoteNG/issues/2471) · [#2453](https://github.com/mRemoteNG/mRemoteNG/issues/2453) · [#2429](https://github.com/mRemoteNG/mRemoteNG/issues/2429) · [#2425](https://github.com/mRemoteNG/mRemoteNG/issues/2425) · [#2323](https://github.com/mRemoteNG/mRemoteNG/issues/2323) · [#2290](https://github.com/mRemoteNG/mRemoteNG/issues/2290) · [#2257](https://github.com/mRemoteNG/mRemoteNG/issues/2257) · [#2252](https://github.com/mRemoteNG/mRemoteNG/issues/2252) · [#2242](https://github.com/mRemoteNG/mRemoteNG/issues/2242) · [#2040](https://github.com/mRemoteNG/mRemoteNG/issues/2040) · [#1986](https://github.com/mRemoteNG/mRemoteNG/issues/1986) · [#1985](https://github.com/mRemoteNG/mRemoteNG/issues/1985) · [#1936](https://github.com/mRemoteNG/mRemoteNG/issues/1936) · [#1934](https://github.com/mRemoteNG/mRemoteNG/issues/1934) · [#1916](https://github.com/mRemoteNG/mRemoteNG/issues/1916) · [#1840](https://github.com/mRemoteNG/mRemoteNG/issues/1840) · [#1836](https://github.com/mRemoteNG/mRemoteNG/issues/1836) · [#1811](https://github.com/mRemoteNG/mRemoteNG/issues/1811) · [#1796](https://github.com/mRemoteNG/mRemoteNG/issues/1796) · [#1784](https://github.com/mRemoteNG/mRemoteNG/issues/1784) · [#1646](https://github.com/mRemoteNG/mRemoteNG/issues/1646) · [#1591](https://github.com/mRemoteNG/mRemoteNG/issues/1591) · [#1589](https://github.com/mRemoteNG/mRemoteNG/issues/1589) · [#1538](https://github.com/mRemoteNG/mRemoteNG/issues/1538) · [#1351](https://github.com/mRemoteNG/mRemoteNG/issues/1351) · [#1283](https://github.com/mRemoteNG/mRemoteNG/issues/1283) · [#872](https://github.com/mRemoteNG/mRemoteNG/issues/872) · [#857](https://github.com/mRemoteNG/mRemoteNG/issues/857) · [#675](https://github.com/mRemoteNG/mRemoteNG/issues/675) · [#660](https://github.com/mRemoteNG/mRemoteNG/issues/660) · [#423](https://github.com/mRemoteNG/mRemoteNG/issues/423) |
| `Credentials` | 3 | [#3333](https://github.com/mRemoteNG/mRemoteNG/issues/3333) · [#3092](https://github.com/mRemoteNG/mRemoteNG/issues/3092) · [#3044](https://github.com/mRemoteNG/mRemoteNG/issues/3044) |
| `Settings` | 18 | [#2987](https://github.com/mRemoteNG/mRemoteNG/issues/2987) · [#2914](https://github.com/mRemoteNG/mRemoteNG/issues/2914) · [#2831](https://github.com/mRemoteNG/mRemoteNG/issues/2831) · [#2756](https://github.com/mRemoteNG/mRemoteNG/issues/2756) · [#2598](https://github.com/mRemoteNG/mRemoteNG/issues/2598) · [#2562](https://github.com/mRemoteNG/mRemoteNG/issues/2562) · [#2561](https://github.com/mRemoteNG/mRemoteNG/issues/2561) · [#2531](https://github.com/mRemoteNG/mRemoteNG/issues/2531) · [#2499](https://github.com/mRemoteNG/mRemoteNG/issues/2499) · [#2480](https://github.com/mRemoteNG/mRemoteNG/issues/2480) · [#2471](https://github.com/mRemoteNG/mRemoteNG/issues/2471) · [#2467](https://github.com/mRemoteNG/mRemoteNG/issues/2467) · [#2463](https://github.com/mRemoteNG/mRemoteNG/issues/2463) · [#2455](https://github.com/mRemoteNG/mRemoteNG/issues/2455) · [#2453](https://github.com/mRemoteNG/mRemoteNG/issues/2453) · [#1719](https://github.com/mRemoteNG/mRemoteNG/issues/1719) · [#722](https://github.com/mRemoteNG/mRemoteNG/issues/722) · [#389](https://github.com/mRemoteNG/mRemoteNG/issues/389) |
| `Import/Export` | 13 | [#2562](https://github.com/mRemoteNG/mRemoteNG/issues/2562) · [#2511](https://github.com/mRemoteNG/mRemoteNG/issues/2511) · [#2487](https://github.com/mRemoteNG/mRemoteNG/issues/2487) · [#2480](https://github.com/mRemoteNG/mRemoteNG/issues/2480) · [#2445](https://github.com/mRemoteNG/mRemoteNG/issues/2445) · [#2419](https://github.com/mRemoteNG/mRemoteNG/issues/2419) · [#2250](https://github.com/mRemoteNG/mRemoteNG/issues/2250) · [#2219](https://github.com/mRemoteNG/mRemoteNG/issues/2219) · [#2078](https://github.com/mRemoteNG/mRemoteNG/issues/2078) · [#2015](https://github.com/mRemoteNG/mRemoteNG/issues/2015) · [#1026](https://github.com/mRemoteNG/mRemoteNG/issues/1026) · [#884](https://github.com/mRemoteNG/mRemoteNG/issues/884) · [#839](https://github.com/mRemoteNG/mRemoteNG/issues/839) |
| `Profiles` | 8 | [#2648](https://github.com/mRemoteNG/mRemoteNG/issues/2648) · [#2636](https://github.com/mRemoteNG/mRemoteNG/issues/2636) · [#2612](https://github.com/mRemoteNG/mRemoteNG/issues/2612) · [#2498](https://github.com/mRemoteNG/mRemoteNG/issues/2498) · [#2467](https://github.com/mRemoteNG/mRemoteNG/issues/2467) · [#2331](https://github.com/mRemoteNG/mRemoteNG/issues/2331) · [#2325](https://github.com/mRemoteNG/mRemoteNG/issues/2325) · [#738](https://github.com/mRemoteNG/mRemoteNG/issues/738) |
| `Add-ons` | 1 | [#3092](https://github.com/mRemoteNG/mRemoteNG/issues/3092) |
| `Windows` | 21 | [#3299](https://github.com/mRemoteNG/mRemoteNG/issues/3299) · [#3180](https://github.com/mRemoteNG/mRemoteNG/issues/3180) · [#3175](https://github.com/mRemoteNG/mRemoteNG/issues/3175) · [#2706](https://github.com/mRemoteNG/mRemoteNG/issues/2706) · [#2659](https://github.com/mRemoteNG/mRemoteNG/issues/2659) · [#2619](https://github.com/mRemoteNG/mRemoteNG/issues/2619) · [#2618](https://github.com/mRemoteNG/mRemoteNG/issues/2618) · [#2608](https://github.com/mRemoteNG/mRemoteNG/issues/2608) · [#2598](https://github.com/mRemoteNG/mRemoteNG/issues/2598) · [#2420](https://github.com/mRemoteNG/mRemoteNG/issues/2420) · [#2329](https://github.com/mRemoteNG/mRemoteNG/issues/2329) · [#2309](https://github.com/mRemoteNG/mRemoteNG/issues/2309) · [#1994](https://github.com/mRemoteNG/mRemoteNG/issues/1994) · [#1830](https://github.com/mRemoteNG/mRemoteNG/issues/1830) · [#1825](https://github.com/mRemoteNG/mRemoteNG/issues/1825) · [#1377](https://github.com/mRemoteNG/mRemoteNG/issues/1377) · [#1174](https://github.com/mRemoteNG/mRemoteNG/issues/1174) · [#1131](https://github.com/mRemoteNG/mRemoteNG/issues/1131) · [#1113](https://github.com/mRemoteNG/mRemoteNG/issues/1113) · [#1086](https://github.com/mRemoteNG/mRemoteNG/issues/1086) · [#787](https://github.com/mRemoteNG/mRemoteNG/issues/787) |
| `Third party` | 21 | [#3247](https://github.com/mRemoteNG/mRemoteNG/issues/3247) · [#3219](https://github.com/mRemoteNG/mRemoteNG/issues/3219) · [#3103](https://github.com/mRemoteNG/mRemoteNG/issues/3103) · [#2881](https://github.com/mRemoteNG/mRemoteNG/issues/2881) · [#2578](https://github.com/mRemoteNG/mRemoteNG/issues/2578) · [#2562](https://github.com/mRemoteNG/mRemoteNG/issues/2562) · [#2487](https://github.com/mRemoteNG/mRemoteNG/issues/2487) · [#2480](https://github.com/mRemoteNG/mRemoteNG/issues/2480) · [#2454](https://github.com/mRemoteNG/mRemoteNG/issues/2454) · [#2360](https://github.com/mRemoteNG/mRemoteNG/issues/2360) · [#2308](https://github.com/mRemoteNG/mRemoteNG/issues/2308) · [#2296](https://github.com/mRemoteNG/mRemoteNG/issues/2296) · [#2179](https://github.com/mRemoteNG/mRemoteNG/issues/2179) · [#2078](https://github.com/mRemoteNG/mRemoteNG/issues/2078) · [#2046](https://github.com/mRemoteNG/mRemoteNG/issues/2046) · [#1943](https://github.com/mRemoteNG/mRemoteNG/issues/1943) · [#1876](https://github.com/mRemoteNG/mRemoteNG/issues/1876) · [#1696](https://github.com/mRemoteNG/mRemoteNG/issues/1696) · [#1679](https://github.com/mRemoteNG/mRemoteNG/issues/1679) · [#1056](https://github.com/mRemoteNG/mRemoteNG/issues/1056) · [#274](https://github.com/mRemoteNG/mRemoteNG/issues/274) |
| `.NET (dotNET)` | 14 | [#3301](https://github.com/mRemoteNG/mRemoteNG/issues/3301) · [#3299](https://github.com/mRemoteNG/mRemoteNG/issues/3299) · [#3298](https://github.com/mRemoteNG/mRemoteNG/issues/3298) · [#3285](https://github.com/mRemoteNG/mRemoteNG/issues/3285) · [#3167](https://github.com/mRemoteNG/mRemoteNG/issues/3167) · [#2998](https://github.com/mRemoteNG/mRemoteNG/issues/2998) · [#2987](https://github.com/mRemoteNG/mRemoteNG/issues/2987) · [#2914](https://github.com/mRemoteNG/mRemoteNG/issues/2914) · [#2810](https://github.com/mRemoteNG/mRemoteNG/issues/2810) · [#2785](https://github.com/mRemoteNG/mRemoteNG/issues/2785) · [#2681](https://github.com/mRemoteNG/mRemoteNG/issues/2681) · [#2678](https://github.com/mRemoteNG/mRemoteNG/issues/2678) · [#1591](https://github.com/mRemoteNG/mRemoteNG/issues/1591) · [#857](https://github.com/mRemoteNG/mRemoteNG/issues/857) |
| `Project Infrastructure` | 10 | [#2826](https://github.com/mRemoteNG/mRemoteNG/issues/2826) · [#2681](https://github.com/mRemoteNG/mRemoteNG/issues/2681) · [#2474](https://github.com/mRemoteNG/mRemoteNG/issues/2474) · [#2454](https://github.com/mRemoteNG/mRemoteNG/issues/2454) · [#2389](https://github.com/mRemoteNG/mRemoteNG/issues/2389) · [#1791](https://github.com/mRemoteNG/mRemoteNG/issues/1791) · [#1449](https://github.com/mRemoteNG/mRemoteNG/issues/1449) · [#306](https://github.com/mRemoteNG/mRemoteNG/issues/306) · [#300](https://github.com/mRemoteNG/mRemoteNG/issues/300) · [#287](https://github.com/mRemoteNG/mRemoteNG/issues/287) |
| `TechnicalDebt` | 8 | [#2899](https://github.com/mRemoteNG/mRemoteNG/issues/2899) · [#2810](https://github.com/mRemoteNG/mRemoteNG/issues/2810) · [#2785](https://github.com/mRemoteNG/mRemoteNG/issues/2785) · [#2025](https://github.com/mRemoteNG/mRemoteNG/issues/2025) · [#1905](https://github.com/mRemoteNG/mRemoteNG/issues/1905) · [#1321](https://github.com/mRemoteNG/mRemoteNG/issues/1321) · [#494](https://github.com/mRemoteNG/mRemoteNG/issues/494) · [#444](https://github.com/mRemoteNG/mRemoteNG/issues/444) |

---

## Issues by category

### Security (30)

_Vulnerabilities and hardening. Triage these first._

| Issue | Title | Labels | Opened | Status |
|---|---|---|---|---|
| [#3361](https://github.com/mRemoteNG/mRemoteNG/issues/3361) | Security vulnerability responsible disclosure | `Bug`, `Security`, `Security Vuln` | 2026-06-26 |  |
| [#3328](https://github.com/mRemoteNG/mRemoteNG/issues/3328) | Credential Audit and Clear | `1.78.*`, `Connections`, `Feature Request`, `Priority - Low`, `Request For Comment`, `Security` | 2026-05-28 |  |
| [#2673](https://github.com/mRemoteNG/mRemoteNG/issues/2673) | Require Password to Disable Password Protect | `Connections`, `Enhancement`, `Need 2 check`, `Security` | 2025-02-12 |  |
| [#2636](https://github.com/mRemoteNG/mRemoteNG/issues/2636) | is it possible to view the password on saved connections? Thanks | `1.77.3`, `Improvement required`, `Priority - Low`, `Profiles`, `Security` | 2024-08-27 |  |
| [#2633](https://github.com/mRemoteNG/mRemoteNG/issues/2633) | Make ICryptographyProvider interface and implementations more secure | `1.77.3`, `In development`, `In progress`, `Needs implementation`, `Priority - High`, `Security` | 2024-08-21 |  |
| [#2598](https://github.com/mRemoteNG/mRemoteNG/issues/2598) | Add Copy/Past login and password | `1.77.4`, `Connections`, `Improvement required`, `Priority - Low`, `Request For Comment`, `Security`, `Settings`, `Windows` | 2024-05-28 |  |
| [#2585](https://github.com/mRemoteNG/mRemoteNG/issues/2585) | CVE-2020-24307 and CVE-2023-30367 | `1.77.2`, `In development`, `In progress`, `Security`, `Security Vuln`, `Verified` | 2024-04-22 |  |
| [#2562](https://github.com/mRemoteNG/mRemoteNG/issues/2562) | Import .xml from Remote Desktop Manager (RDM) | `1.77.4`, `Connections`, `Import/Export`, `Needs implementation`, `Security`, `Settings`, `Third party` | 2024-02-05 |  |
| [#2561](https://github.com/mRemoteNG/mRemoteNG/issues/2561) | External Tools Request | `1.78.*`, `Security`, `Settings` | 2024-02-01 |  |
| [#2460](https://github.com/mRemoteNG/mRemoteNG/issues/2460) | Error when using special characters in credentials | `1.77.3`, `Connections`, `In development`, `Security` | 2023-07-17 |  |
| [#2420](https://github.com/mRemoteNG/mRemoteNG/issues/2420) | Public Disclosure of issue 726 | `1.77.3`, `Connections`, `In progress`, `Priority - High`, `Security`, `Security Vuln`, `Verified`, `Windows` | 2023-04-03 |  |
| [#2419](https://github.com/mRemoteNG/mRemoteNG/issues/2419) | Export Saves password in OPEN TEXT | `1.77.3`, `Import/Export`, `Priority - High`, `Security` | 2023-04-03 |  |
| [#2277](https://github.com/mRemoteNG/mRemoteNG/issues/2277) | FR: More generic vault connector | `1.78.*`, `Connections`, `Needs implementation`, `Security`, `UI/UX` | 2022-08-31 |  |
| [#2274](https://github.com/mRemoteNG/mRemoteNG/issues/2274) | Can't login with encrypted stored password which contains a paragraph § sign | `Improvement required`, `Security` | 2022-08-24 |  |
| [#2195](https://github.com/mRemoteNG/mRemoteNG/issues/2195) | Crafted XML File Code Execution | `1.77.3`, `PowerShell`, `Security`, `Verified` | 2022-04-07 |  |
| [#2189](https://github.com/mRemoteNG/mRemoteNG/issues/2189) | Idea: allow Windows Account for encryption (Data Protection API/DPAPI) | `Enhancement`, `In development`, `Security` | 2022-03-19 |  |
| [#2046](https://github.com/mRemoteNG/mRemoteNG/issues/2046) | Trouble using key-based authentication with external tools | `Improvement required`, `Security`, `Third party` | 2021-09-13 |  |
| [#2040](https://github.com/mRemoteNG/mRemoteNG/issues/2040) | create cache wen connected to db | `1.8 (Fenix)`, `DBs`, `Enhancement`, `Security` | 2021-09-03 |  |
| [#1892](https://github.com/mRemoteNG/mRemoteNG/issues/1892) | Security: CVE-2020-0765 \| Remote Desktop Connection Manager Information Disclosure Vulnerability | `Priority - High`, `RDP`, `Security Vuln` | 2020-10-29 |  |
| [#1646](https://github.com/mRemoteNG/mRemoteNG/issues/1646) | SQL Connections: Don't ask for custom password after every change is done. | `DBs`, `Need 2 check`, `Security` | 2019-11-27 |  |
| [#1635](https://github.com/mRemoteNG/mRemoteNG/issues/1635) | exclamation mark not working in passwords | `Need 2 check`, `Security` | 2019-11-13 |  |
| [#1449](https://github.com/mRemoteNG/mRemoteNG/issues/1449) | File integrity hash | `Enhancement`, `Project Infrastructure`, `Security` | 2019-05-14 |  |
| [#1283](https://github.com/mRemoteNG/mRemoteNG/issues/1283) | Object reference not set to an instance of an object | `Bug`, `DBs`, `Security` | 2019-01-21 |  |
| [#1137](https://github.com/mRemoteNG/mRemoteNG/issues/1137) | export and import with encryption | `1.77.2`, `Need 2 check`, `Needs implementation`, `Security` | 2018-10-10 |  |
| [#1113](https://github.com/mRemoteNG/mRemoteNG/issues/1113) | Settings RDP loadbalanceinfo causes An internal error has occured | `Improvement required`, `RDP`, `Security`, `Windows` | 2018-09-28 |  |
| [#1085](https://github.com/mRemoteNG/mRemoteNG/issues/1085) | Passwords Exported in ClearText (.csv) | `Connections`, `Security` | 2018-08-24 |  |
| [#1026](https://github.com/mRemoteNG/mRemoteNG/issues/1026) | Import connections CSV instead of XML | `1.77.2`, `Connections`, `Enhancement`, `Import/Export`, `Needs implementation`, `Security` | 2018-07-18 |  |
| [#918](https://github.com/mRemoteNG/mRemoteNG/issues/918) | Display password | `Feature Request`, `Security`, `UI/UX` | 2018-03-20 |  |
| [#735](https://github.com/mRemoteNG/mRemoteNG/issues/735) | Feature Request: Save Connection | `Enhancement`, `Security Vuln` | 2017-10-09 |  |
| [#726](https://github.com/mRemoteNG/mRemoteNG/issues/726) | Implement SecureString Class to decrypt sensitve data on use | `1.77.3`, `Connections`, `Priority - High`, `Security`, `Security Vuln` | 2017-09-18 |  |

### Bugs & Crashes (154)

_Confirmed defects, exceptions and crashes._

| Issue | Title | Labels | Opened | Status |
|---|---|---|---|---|
| [#3319](https://github.com/mRemoteNG/mRemoteNG/issues/3319) | Cannot access a disposed object. Object name: 'ConnectionWindow'. | `1.78.*`, `Bug`, `Need 2 check`, `UI/UX` | 2026-05-18 |  |
| [#3308](https://github.com/mRemoteNG/mRemoteNG/issues/3308) | Cannot access a disposed object | `1.78.*`, `Bug`, `Need 2 check` | 2026-05-13 |  |
| [#3307](https://github.com/mRemoteNG/mRemoteNG/issues/3307) | Cannot access a disposed object | `1.78.*`, `Bug`, `Need 2 check`, `Nightly Build` | 2026-05-13 |  |
| [#3301](https://github.com/mRemoteNG/mRemoteNG/issues/3301) | an unhandled exception | `.NET (dotNET)`, `1.78.*`, `Bug`, `Need 2 check` | 2026-05-06 |  |
| [#3299](https://github.com/mRemoteNG/mRemoteNG/issues/3299) | Graph positioning errors in the interface and interface freezing | `.NET (dotNET)`, `1.78.*`, `Bug`, `Need 2 check`, `Nightly Build`, `UI/UX`, `Windows` | 2026-05-04 |  |
| [#3298](https://github.com/mRemoteNG/mRemoteNG/issues/3298) | GUI broken [1.78.2 NB 3405] | `.NET (dotNET)`, `1.78.*`, `Bug`, `Need 2 check` | 2026-05-04 |  |
| [#3285](https://github.com/mRemoteNG/mRemoteNG/issues/3285) | [Bug] 1.78.2.3405 - Application window not visible | `.NET (dotNET)`, `1.78.*`, `Bug`, `Need 2 check`, `Nightly Build`, `UI/UX` | 2026-04-24 |  |
| [#3261](https://github.com/mRemoteNG/mRemoteNG/issues/3261) | can't click the correct element | `1.78.*`, `Bug`, `HighDPI`, `Need 2 check` | 2026-04-08 |  |
| [#3249](https://github.com/mRemoteNG/mRemoteNG/issues/3249) | [Bug] App permanently fails to start after saving invalid SQL Server settings | `1.78.*`, `Bug`, `DBs`, `Need 2 check` | 2026-04-01 |  |
| [#3247](https://github.com/mRemoteNG/mRemoteNG/issues/3247) | [Bug] Inline rename edit box shifts and shows ghost text behind it | `1.78.*`, `Bug`, `Third party`, `UI/UX` | 2026-04-01 |  |
| [#3202](https://github.com/mRemoteNG/mRemoteNG/issues/3202) | Crash when opening another connection after minimizing a full-screen RDP session | `1.78.*`, `Bug`, `Improvement required`, `Need 2 check`, `RDP`, `UI/UX` | 2026-03-06 |  |
| [#3180](https://github.com/mRemoteNG/mRemoteNG/issues/3180) | Main window does not show | `1.78.*`, `Bug`, `Connections`, `Nightly Build`, `Windows` | 2026-02-26 |  |
| [#3163](https://github.com/mRemoteNG/mRemoteNG/issues/3163) | белый экран | `1.78.*`, `Bug`, `Need 2 check` | 2026-02-22 |  |
| [#3069](https://github.com/mRemoteNG/mRemoteNG/issues/3069) | Exeption occurred on closing panel with connections inside | `1.78.*`, `Bug`, `Need 2 check` | 2026-01-02 |  |
| [#3044](https://github.com/mRemoteNG/mRemoteNG/issues/3044) | With external tool, if password contains a comma, the comma acts as a divider and the variable get split - 1.78.2.3228 | `1.78.*`, `Bug`, `Credentials` | 2025-12-11 |  |
| [#3005](https://github.com/mRemoteNG/mRemoteNG/issues/3005) | SQL Server Connection - mRemoteNG 1.78.2 NB 3228 | `1.78.*`, `Bug`, `DBs`, `Need 2 check` | 2025-11-11 |  |
| [#2987](https://github.com/mRemoteNG/mRemoteNG/issues/2987) | Where is user.config in mR 1.78.2 NB 3228? | `.NET (dotNET)`, `1.78.*`, `Bug`, `dependencies`, `Settings` | 2025-11-04 |  |
| [#2907](https://github.com/mRemoteNG/mRemoteNG/issues/2907) | mRemoteNG frequently freezes when opening and closing Options | `1.78.*`, `Bug`, `Options`, `Panels`, `UI/UX` | 2025-10-16 |  |
| [#2899](https://github.com/mRemoteNG/mRemoteNG/issues/2899) | mRemoteNg 1.76.20.24615 cannot use MySQL DB 9.4 as connexion storage backend ? | `1.76.20`, `Bug`, `DBs`, `TechnicalDebt` | 2025-10-15 |  |
| [#2706](https://github.com/mRemoteNG/mRemoteNG/issues/2706) | ver: 1.78.2: The application crashes with an error when closing a tab with open connections. | `1.78.*`, `Bug`, `Connections`, `Need 2 check`, `UI/UX`, `Windows` | 2025-06-21 |  |
| [#2687](https://github.com/mRemoteNG/mRemoteNG/issues/2687) | SQL Server Connect error in current nighly build | `1.78.*`, `Bug`, `DBs`, `Nightly Build`, `Verified` | 2025-04-08 |  |
| [#2510](https://github.com/mRemoteNG/mRemoteNG/issues/2510) | COM object that has been separated from its underlying RCW cannot be used. | `1.77.3`, `Bug`, `Need 2 check` | 2023-10-19 |  |
| [#2459](https://github.com/mRemoteNG/mRemoteNG/issues/2459) | Error when closing group tab with SSH conn | `1.77.3`, `Bug`, `SSH`, `UI/UX` | 2023-07-13 |  |
| [#2294](https://github.com/mRemoteNG/mRemoteNG/issues/2294) | RDP session = full screen is not full screen | `Bug`, `RDP`, `UI/UX` | 2022-10-05 |  |
| [#2287](https://github.com/mRemoteNG/mRemoteNG/issues/2287) | Console pane is out of position with tab panel | `Bug`, `UI/UX` | 2022-09-13 |  |
| [#2270](https://github.com/mRemoteNG/mRemoteNG/issues/2270) | Clicking close on the applications X with a panel open presents an are you sure type message - but response is ignored | `Bug`, `Improvement required`, `In development`, `Needs implementation` | 2022-08-04 |  |
| [#2218](https://github.com/mRemoteNG/mRemoteNG/issues/2218) | Configuration dialog missing in 1.77.2 | `1.77.2`, `Bug` | 2022-05-04 |  |
| [#2179](https://github.com/mRemoteNG/mRemoteNG/issues/2179) | view area is not adjustable when focusing on connection windows | `Bug`, `Third party`, `UI/UX` | 2022-02-25 |  |
| [#2171](https://github.com/mRemoteNG/mRemoteNG/issues/2171) | Config / Connections Tabs not saving positions | `1.77.3`, `Bug`, `UI/UX` | 2022-02-15 |  |
| [#2166](https://github.com/mRemoteNG/mRemoteNG/issues/2166) | Crashes / Tab Issues When Idling and Resized | `1.77.3`, `Bug`, `Improvement required` | 2022-02-11 |  |
| [#2163](https://github.com/mRemoteNG/mRemoteNG/issues/2163) | mRemoteNG crashes if panel containing connection(s) is closed | `1.77.3`, `Bug` | 2022-02-07 |  |
| [#2161](https://github.com/mRemoteNG/mRemoteNG/issues/2161) | Tab bar no longer scrolls when dragging tab in the case of tab strip overflow. | `1.77.3`, `Bug`, `Improvement required`, `UI/UX` | 2022-02-04 |  |
| [#2160](https://github.com/mRemoteNG/mRemoteNG/issues/2160) | Closing the last tab in a panel does not close the panel | `1.77.3`, `Bug`, `Improvement required`, `UI/UX` | 2022-02-04 |  |
| [#2154](https://github.com/mRemoteNG/mRemoteNG/issues/2154) | Duplicate field names - Nightly 1.77.2 | `1.77.3`, `Bug`, `i10n`, `Improvement required`, `Translations` | 2022-02-03 |  |
| [#2152](https://github.com/mRemoteNG/mRemoteNG/issues/2152) | Confusing & broken Inheritance - Nightly 1.77.2 | `1.77.3`, `Bug`, `Improvement required`, `Need 2 check` | 2022-02-03 |  |
| [#2150](https://github.com/mRemoteNG/mRemoteNG/issues/2150) | Configuration file failure - Nightly 1.77.2 | `1.77.3`, `Bug`, `Connections`, `Need 2 check` | 2022-02-01 |  |
| [#2139](https://github.com/mRemoteNG/mRemoteNG/issues/2139) | external tools - telnet not work from mremoteng although it works well with standard CMD | `Bug`, `Need 2 check` | 2022-01-21 |  |
| [#2135](https://github.com/mRemoteNG/mRemoteNG/issues/2135) | BUG: SSH duplicate fails when using SSH Jump Mode | `Bug`, `Improvement required` | 2022-01-19 |  |
| [#2119](https://github.com/mRemoteNG/mRemoteNG/issues/2119) | Restoring Sessions after program start "forgets" opened connections | `Bug` | 2022-01-12 |  |
| [#2118](https://github.com/mRemoteNG/mRemoteNG/issues/2118) | Unhandled Exception when closing mRemoteNG having multiple panels open | `Bug` | 2022-01-12 |  |
| [#2062](https://github.com/mRemoteNG/mRemoteNG/issues/2062) | Split pane can't dock on secondary monitor | `Bug` | 2021-11-03 |  |
| [#1986](https://github.com/mRemoteNG/mRemoteNG/issues/1986) | Can`t connection to sql database | `1.77.2`, `Bug`, `DBs`, `In progress` | 2021-07-05 |  |
| [#1977](https://github.com/mRemoteNG/mRemoteNG/issues/1977) | Column 'Domain' does not belong to table . | `1.77.2`, `Bug`, `In progress` | 2021-06-25 |  |
| [#1969](https://github.com/mRemoteNG/mRemoteNG/issues/1969) | The startup connection file could not be loaded. connectionFilePath cannot be null or empty | `Bug`, `In progress` | 2021-06-11 |  |
| [#1965](https://github.com/mRemoteNG/mRemoteNG/issues/1965) | How to set the port of "Quick connections"？（v1.77.1） | `Bug`, `Verified` | 2021-05-30 |  |
| [#1936](https://github.com/mRemoteNG/mRemoteNG/issues/1936) | MSSQL - Bug with empty username/password | `1.77.2`, `Bug`, `DBs`, `Priority - High` | 2021-02-22 |  |
| [#1934](https://github.com/mRemoteNG/mRemoteNG/issues/1934) | MySQL latest Dev Branch Concurrency Error | `1.77.2`, `Bug`, `DBs` | 2021-02-18 |  |
| [#1928](https://github.com/mRemoteNG/mRemoteNG/issues/1928) | Drag and drop connection problem | `Bug`, `Need 2 check`, `UI/UX` | 2021-02-09 |  |
| [#1923](https://github.com/mRemoteNG/mRemoteNG/issues/1923) | Server Security Default - Auth Failure Continue | `1.77.2`, `Bug`, `Need 2 check` | 2021-01-26 |  |
| [#1916](https://github.com/mRemoteNG/mRemoteNG/issues/1916) | MySQL error Missing the DataColumn 'DisableCursorBlinking' | `1.77.2`, `Bug`, `DBs` | 2020-12-19 |  |
| [#1912](https://github.com/mRemoteNG/mRemoteNG/issues/1912) | incomplete display of the input password dialog when startup | `Bug`, `Need 2 check` | 2020-12-11 |  |
| [#1875](https://github.com/mRemoteNG/mRemoteNG/issues/1875) | Reconnect SSH session with floating window fails with warning/error message | `Bug`, `SSH`, `UI/UX` | 2020-10-06 |  |
| [#1863](https://github.com/mRemoteNG/mRemoteNG/issues/1863) | Invalid value:the value of dockareas connflicts with current dockstate. | `1.77.2`, `Bug`, `Need 2 check` | 2020-09-16 |  |
| [#1858](https://github.com/mRemoteNG/mRemoteNG/issues/1858) | Error while opening rdp or telnet | `Bug`, `Need 2 check` | 2020-09-09 |  |
| [#1853](https://github.com/mRemoteNG/mRemoteNG/issues/1853) | Update check providing old version | `1.77.2`, `1.78.*`, `Bug`, `In progress`, `Priority - High` | 2020-09-03 |  |
| [#1840](https://github.com/mRemoteNG/mRemoteNG/issues/1840) | MSSQL Mode - Unable to save New Connection or New Folder | `1.77.2`, `Bug`, `DBs` | 2020-08-20 |  |
| [#1836](https://github.com/mRemoteNG/mRemoteNG/issues/1836) | SQL connection - error saving connections | `Bug`, `DBs` | 2020-08-18 |  |
| [#1827](https://github.com/mRemoteNG/mRemoteNG/issues/1827) | Lost monitor causes "Object Not Found" exception | `Bug`, `Need 2 check` | 2020-08-06 |  |
| [#1826](https://github.com/mRemoteNG/mRemoteNG/issues/1826) | Copy & Paste not working in RDP Sessions | `1.77.2`, `Bug`, `Improvement required`, `UI/UX` | 2020-07-29 |  |
| [#1825](https://github.com/mRemoteNG/mRemoteNG/issues/1825) | Strange behaviour with Trend Micro | `Bug`, `Enhancement`, `Improvement required`, `Windows` | 2020-07-28 |  |
| [#1824](https://github.com/mRemoteNG/mRemoteNG/issues/1824) | Unable to copy & paste from Local drive to a server connected thru mRemote | `1.77.2`, `Bug` | 2020-07-23 |  |
| [#1822](https://github.com/mRemoteNG/mRemoteNG/issues/1822) | An unhandled exception has occurred | `1.77.2`, `Bug`, `SSH`, `Verified` | 2020-07-23 |  |
| [#1816](https://github.com/mRemoteNG/mRemoteNG/issues/1816) | mremoteng crashing in startup (The process was terminated due to an unhandled exception) | `1.77.2`, `Bug`, `Need 2 check` | 2020-07-13 |  |
| [#1813](https://github.com/mRemoteNG/mRemoteNG/issues/1813) | version 1.77.0.41252 Copy/Past issue | `Bug` | 2020-07-07 |  |
| [#1811](https://github.com/mRemoteNG/mRemoteNG/issues/1811) | unhandled exception MSSQL database settings | `1.77.2`, `Bug`, `DBs`, `Improvement required` | 2020-07-03 |  |
| [#1805](https://github.com/mRemoteNG/mRemoteNG/issues/1805) | mremoteNG quit sundenly on remote desktop machine | `Bug`, `Need 2 check` | 2020-06-26 |  |
| [#1796](https://github.com/mRemoteNG/mRemoteNG/issues/1796) | Error Creation New Folder (MSSQL Mode) | `1.77.2`, `Bug`, `DBs`, `Need 2 check` | 2020-06-16 |  |
| [#1794](https://github.com/mRemoteNG/mRemoteNG/issues/1794) | Crash when closing panel containing a connection tab | `Bug`, `Verified` | 2020-06-16 |  |
| [#1784](https://github.com/mRemoteNG/mRemoteNG/issues/1784) | Error loading MySQL database results in erased configuration | `1.77.2`, `Bug`, `DBs` | 2020-06-09 |  |
| [#1760](https://github.com/mRemoteNG/mRemoteNG/issues/1760) | error importing CSV file if password contains semicolon (delimeter) | `Bug`, `Verified` | 2020-05-21 |  |
| [#1754](https://github.com/mRemoteNG/mRemoteNG/issues/1754) | mRemoteNG prevent using AltGr keys | `Bug`, `Need 2 check`, `UI/UX` | 2020-05-14 |  |
| [#1752](https://github.com/mRemoteNG/mRemoteNG/issues/1752) | Error message popping up when creating the first panel | `1.77.2`, `Bug`, `Need 2 check` | 2020-05-09 |  |
| [#1736](https://github.com/mRemoteNG/mRemoteNG/issues/1736) | files are not sent via ssh | `Bug`, `Need 2 check`, `SSH` | 2020-04-21 |  |
| [#1727](https://github.com/mRemoteNG/mRemoteNG/issues/1727) | View Only mode kicks off logged in user. | `1.77.2`, `Bug`, `Need 2 check` | 2020-04-02 |  |
| [#1715](https://github.com/mRemoteNG/mRemoteNG/issues/1715) | No more than 14 parallel RDP connections possible | `1.77.2`, `Bug`, `RDP` | 2020-03-19 |  |
| [#1701](https://github.com/mRemoteNG/mRemoteNG/issues/1701) | Unexpected when Connection File is in an unmapped Network Drive | `Bug`, `Verified` | 2020-02-26 |  |
| [#1693](https://github.com/mRemoteNG/mRemoteNG/issues/1693) | Error setting extended RDP property 'DeviceScaleFactor' | `1.77.2`, `Bug`, `Need 2 check`, `RDP` | 2020-02-14 |  |
| [#1673](https://github.com/mRemoteNG/mRemoteNG/issues/1673) | Can't connect to Linux Hyper-V Machines, RDCman works fine | `Bug`, `Linux`, `Need 2 check` | 2020-01-19 |  |
| [#1666](https://github.com/mRemoteNG/mRemoteNG/issues/1666) | Session is unlocked by restoring down or maximizing the window | `Bug`, `Improvement required` | 2020-01-07 |  |
| [#1660](https://github.com/mRemoteNG/mRemoteNG/issues/1660) | There are always times when you have lost a connection and are trying to reconnect | `Bug`, `Need 2 check` | 2019-12-20 |  |
| [#1656](https://github.com/mRemoteNG/mRemoteNG/issues/1656) | Application crashes with unhanded exception (Cannot access a disposed object.) | `Bug`, `In progress` | 2019-12-17 |  |
| [#1651](https://github.com/mRemoteNG/mRemoteNG/issues/1651) | Password passed incorrectly to external application when using the ^(caret) character | `Bug`, `Improvement required`, `Need 2 check` | 2019-12-10 |  |
| [#1650](https://github.com/mRemoteNG/mRemoteNG/issues/1650) | Application automatically goes to background in various scenarios | `1.78.*`, `Bug`, `Need 2 check` | 2019-12-09 |  |
| [#1636](https://github.com/mRemoteNG/mRemoteNG/issues/1636) | Index was out of range. Must be non-negative and less than the size of the collection. Parameter name: index | `Bug`, `Need 2 check` | 2019-11-14 |  |
| [#1634](https://github.com/mRemoteNG/mRemoteNG/issues/1634) | Expose protocol as variable for external tools | `1.77.2`, `Bug`, `Enhancement`, `Need 2 check` | 2019-10-28 |  |
| [#1631](https://github.com/mRemoteNG/mRemoteNG/issues/1631) | Right shift not recognised when using Chrome Remote Desktop | `Bug`, `Improvement required`, `RDP`, `UI/UX` | 2019-10-25 |  |
| [#1589](https://github.com/mRemoteNG/mRemoteNG/issues/1589) | SQL deletes all rows and creates new rows for each change to the data | `1.77.2`, `Bug`, `DBs` | 2019-09-19 |  |
| [#1585](https://github.com/mRemoteNG/mRemoteNG/issues/1585) | Unable to Use MSSQL for connections | `1.77.2`, `Bug`, `Priority - High` | 2019-09-18 |  |
| [#1582](https://github.com/mRemoteNG/mRemoteNG/issues/1582) | High DPI Support | `1.77.2`, `Bug`, `Need 2 check` | 2019-09-15 |  |
| [#1557](https://github.com/mRemoteNG/mRemoteNG/issues/1557) | FIPS compliance regression | `Bug` | 2019-09-03 |  |
| [#1461](https://github.com/mRemoteNG/mRemoteNG/issues/1461) | Alt Gr stops working after terminal reconnection | `1.77.2`, `Bug`, `Need 2 check` | 2019-05-24 |  |
| [#1452](https://github.com/mRemoteNG/mRemoteNG/issues/1452) | Tree icon not updated when connection lost | `Bug` | 2019-05-15 |  |
| [#1450](https://github.com/mRemoteNG/mRemoteNG/issues/1450) | KeePass AutoType not passing through to RDP session | `Bug` | 2019-05-15 |  |
| [#1442](https://github.com/mRemoteNG/mRemoteNG/issues/1442) | No keyboard input in TigerVNC(Java) when embedded/integrated in window | `Bug`, `Need 2 check` | 2019-04-26 |  |
| [#1438](https://github.com/mRemoteNG/mRemoteNG/issues/1438) | incorrect smartsize | `Bug` | 2019-05-06 |  |
| [#1427](https://github.com/mRemoteNG/mRemoteNG/issues/1427) | RDP doesn't respect desktop scaling | `Bug` | 2019-04-30 |  |
| [#1408](https://github.com/mRemoteNG/mRemoteNG/issues/1408) | Regression in develop branch: Putty not getting input focus when switching tabs | `Bug`, `Verified` | 2019-04-13 |  |
| [#1386](https://github.com/mRemoteNG/mRemoteNG/issues/1386) | mremoteng crashing when opening new connection file | `Bug` | 2019-04-01 |  |
| [#1377](https://github.com/mRemoteNG/mRemoteNG/issues/1377) | HD Intel 520 Display - mRemoteNG start but unable to stay in foreground | `Bug`, `Need 2 check`, `Windows` | 2019-03-26 |  |
| [#1368](https://github.com/mRemoteNG/mRemoteNG/issues/1368) | Application Crashes after Setting SQL Options | `Bug` | 2019-03-21 |  |
| [#1364](https://github.com/mRemoteNG/mRemoteNG/issues/1364) | Putty sessions not shown in reapply_credential_manager branch | `Bug` | 2019-03-20 |  |
| [#1359](https://github.com/mRemoteNG/mRemoteNG/issues/1359) | Application Theme gets reseted to default after installing a new update | `Bug`, `Ready`, `Verified` | 2019-03-18 |  |
| [#1354](https://github.com/mRemoteNG/mRemoteNG/issues/1354) | Local connection properties not saved when user has read only access to database | `Bug` | 2019-03-15 |  |
| [#1351](https://github.com/mRemoteNG/mRemoteNG/issues/1351) | Randomly, after close and launch again, SQL database is truncate (empty) | `1.77.2`, `Bug`, `DBs` | 2019-03-15 |  |
| [#1341](https://github.com/mRemoteNG/mRemoteNG/issues/1341) | Exception while attempting to create a new theme | `Bug`, `Priority - Medium`, `Verified` | 2019-03-10 |  |
| [#1304](https://github.com/mRemoteNG/mRemoteNG/issues/1304) | Config is wiped when importing multiple *.RDG files | `Bug` | 2019-02-10 |  |
| [#1286](https://github.com/mRemoteNG/mRemoteNG/issues/1286) | Random Errors When Closing Tab | `Bug`, `Verified` | 2019-01-24 |  |
| [#1268](https://github.com/mRemoteNG/mRemoteNG/issues/1268) | Window layout broken if DPS deserialization throws exception | `Bug` | 2019-01-10 |  |
| [#1257](https://github.com/mRemoteNG/mRemoteNG/issues/1257) | Exception after opening Options panel 7 times in within the same mRemoteNG instance | `Bug`, `Priority - Medium`, `Ready`, `Verified` | 2019-01-08 |  |
| [#1214](https://github.com/mRemoteNG/mRemoteNG/issues/1214) | Russian language turning into crap with vnc connection | `Bug`, `Verified`, `VNC` | 2018-12-20 |  |
| [#1213](https://github.com/mRemoteNG/mRemoteNG/issues/1213) | mRemoteNG hang when using ssh with wromg credential | `Bug` | 2018-12-18 |  |
| [#1195](https://github.com/mRemoteNG/mRemoteNG/issues/1195) | mRemoteNG hangs if incorrect credentials are provided | `Bug`, `VNC` | 2018-11-20 |  |
| [#1182](https://github.com/mRemoteNG/mRemoteNG/issues/1182) | Printer Redirecting not working correctly | `Bug` | 2018-11-06 |  |
| [#1174](https://github.com/mRemoteNG/mRemoteNG/issues/1174) | After maximize bad screen | `Bug`, `UI/UX`, `Windows` | 2018-11-02 |  |
| [#1128](https://github.com/mRemoteNG/mRemoteNG/issues/1128) | Connection tree flickers & hourglass cursor when using with multiple users on one SQL database | `Bug` | 2018-10-10 |  |
| [#1127](https://github.com/mRemoteNG/mRemoteNG/issues/1127) | Middle-clicking panel tab to close specific panel closes all open panels | `1.77.2`, `Bug`, `Need 2 check` | 2018-10-09 |  |
| [#1122](https://github.com/mRemoteNG/mRemoteNG/issues/1122) | Focus does not move to search bar when painting existing search filter (selecting with left mouse button down) | `Bug` | 2018-10-03 |  |
| [#1118](https://github.com/mRemoteNG/mRemoteNG/issues/1118) | Putty panels lose focus on touchpad scroll | `Bug` | 2018-09-30 |  |
| [#1095](https://github.com/mRemoteNG/mRemoteNG/issues/1095) | wrong logon information on tab shown when using "connect without credentials" | `Bug` | 2018-08-29 |  |
| [#1090](https://github.com/mRemoteNG/mRemoteNG/issues/1090) | Broke position the form "Components check" | `Bug`, `Improvement required`, `UI/UX` | 2018-08-26 |  |
| [#1063](https://github.com/mRemoteNG/mRemoteNG/issues/1063) | "expand all folders" only applied to visible folders when filtering | `Bug` | 2018-08-05 |  |
| [#1031](https://github.com/mRemoteNG/mRemoteNG/issues/1031) | Can't save ConfCons.xml | `1.77.2`, `Bug`, `Connections`, `In progress` | 2018-07-21 |  |
| [#1021](https://github.com/mRemoteNG/mRemoteNG/issues/1021) | Config and Connection-Tab not visible when moving to secondary Monitor | `Bug`, `Need 2 check`, `UI/UX` | 2018-07-11 |  |
| [#1005](https://github.com/mRemoteNG/mRemoteNG/issues/1005) | Gateway Authentication box hidden behind other windows and cannot be switched to | `Bug`, `Need 2 check` | 2018-06-22 |  |
| [#980](https://github.com/mRemoteNG/mRemoteNG/issues/980) | Cannot select multiple connections, inheritance is not useful in current implementation. | `1.77.2`, `Bug`, `Connections`, `UI/UX` | 2018-05-23 |  |
| [#965](https://github.com/mRemoteNG/mRemoteNG/issues/965) | Open Connection File Menu Option Does Not Work w/ SQL Server Functionality Enabled | `Bug` | 2018-04-24 |  |
| [#912](https://github.com/mRemoteNG/mRemoteNG/issues/912) | RDP Disconnected 6151 | `Bug` | 2018-03-09 |  |
| [#865](https://github.com/mRemoteNG/mRemoteNG/issues/865) | Putty window not maximized after waking from hibernate | `Bug` | 2018-01-15 |  |
| [#860](https://github.com/mRemoteNG/mRemoteNG/issues/860) | New Connection command not in default menu location | `1.77.2`, `Bug`, `Need 2 check`, `UI/UX` | 2018-01-08 |  |
| [#853](https://github.com/mRemoteNG/mRemoteNG/issues/853) | Crashing when many sessions are open | `Bug`, `Vendor/Upstream Issue`, `Verified` | 2017-12-27 |  |
| [#850](https://github.com/mRemoteNG/mRemoteNG/issues/850) | Config panel column width NOT remembered between minimize/maximize action | `1.77.2`, `Bug`, `Need 2 check` | 2017-12-20 |  |
| [#834](https://github.com/mRemoteNG/mRemoteNG/issues/834) | Disk Drive Redirect Option missing from connections inside a folder | `Bug`, `Connections`, `Need 2 check` | 2017-12-08 |  |
| [#828](https://github.com/mRemoteNG/mRemoteNG/issues/828) | Reset Layout | `Bug` | 2017-12-05 |  |
| [#822](https://github.com/mRemoteNG/mRemoteNG/issues/822) | mRemoteNG doesn't start if keyfile not available | `1.77.2`, `Bug`, `Connections`, `In progress` | 2017-11-28 |  |
| [#794](https://github.com/mRemoteNG/mRemoteNG/issues/794) | Reconnected tabs reopen at the end | `Bug`, `Enhancement`, `Panels`, `UI/UX`, `Verified` | 2017-11-10 |  |
| [#755](https://github.com/mRemoteNG/mRemoteNG/issues/755) | Window Selection Behaviour | `Bug` | 2017-10-28 |  |
| [#730](https://github.com/mRemoteNG/mRemoteNG/issues/730) | Remote printing | `Bug`, `Need 2 check` | 2017-09-27 |  |
| [#725](https://github.com/mRemoteNG/mRemoteNG/issues/725) | General tab always tries to open next to the Connections tab | `Bug`, `Need 2 check`, `UI/UX` | 2017-09-18 |  |
| [#631](https://github.com/mRemoteNG/mRemoteNG/issues/631) | external tools | `Bug` | 2017-07-05 |  |
| [#627](https://github.com/mRemoteNG/mRemoteNG/issues/627) | Mouse pointer disappears in edit mode when the background is White | `1.77.2`, `Bug`, `Need 2 check` | 2017-07-02 |  |
| [#540](https://github.com/mRemoteNG/mRemoteNG/issues/540) | mRemote always failed the first RDP Connection | `1.77.2`, `Bug` | 2017-05-05 |  |
| [#520](https://github.com/mRemoteNG/mRemoteNG/issues/520) | Alt Tab on Windows 10 | `Bug`, `Duplicate`, `Need 2 check`, `UI/UX` | 2017-04-20 |  |
| [#463](https://github.com/mRemoteNG/mRemoteNG/issues/463) | RDP to Server 2012 R2 Core Fails using mRemoteNG but works with MS RDP client | `1.77.2`, `Bug`, `RDP` | 2017-03-19 |  |
| [#443](https://github.com/mRemoteNG/mRemoteNG/issues/443) | Credentials for RDP Gateway Not Being Passed Thru | `Bug` | 2017-03-07 |  |
| [#401](https://github.com/mRemoteNG/mRemoteNG/issues/401) | Bug: Panel name must not be translated | `Bug` | 2017-02-13 |  |
| [#398](https://github.com/mRemoteNG/mRemoteNG/issues/398) | mRemoteNG to front | `1.77.3`, `Bug`, `Putty` | 2017-02-10 |  |
| [#354](https://github.com/mRemoteNG/mRemoteNG/issues/354) | Lack of Polish signs after a while. | `Bug` | 2017-01-19 |  |
| [#330](https://github.com/mRemoteNG/mRemoteNG/issues/330) | Dismiss Tab Context Menu When Clicking Inside RDP Frame | `Bug`, `Help Wanted`, `Priority - Low`, `UI/UX`, `Verified` | 2017-01-06 |  |
| [#303](https://github.com/mRemoteNG/mRemoteNG/issues/303) | Printers do not appear to be redirected | `Bug`, `Verified` | 2016-12-07 |  |
| [#290](https://github.com/mRemoteNG/mRemoteNG/issues/290) | Focus issue in PuTTYNG session after switching tabs | `1.77.3`, `Bug`, `Need 2 check` | 2016-12-02 |  |
| [#274](https://github.com/mRemoteNG/mRemoteNG/issues/274) | Cannot connect to TightVNC Server When Unauthenticated | `1.77.3`, `Bug`, `In development`, `Need 2 check`, `Third party`, `VNC` | 2016-11-28 |  |
| [#229](https://github.com/mRemoteNG/mRemoteNG/issues/229) | Bug: Inheriting Appearence settings in RDP sessions doesn't work consistently | `Bug` | 2016-11-07 |  |
| [#227](https://github.com/mRemoteNG/mRemoteNG/issues/227) | Pressing Caps Lock causes lowercase t keypress | `Bug`, `Verified`, `VNC` | 2016-11-07 |  |
| [#220](https://github.com/mRemoteNG/mRemoteNG/issues/220) | mRemoteNG and Gateway and Windows 2016 without NLA | `Bug` | 2016-11-03 |  |

### Features & Enhancements (372)

_Feature requests and improvement asks — the largest bucket._

| Issue | Title | Labels | Opened | Status |
|---|---|---|---|---|
| [#3335](https://github.com/mRemoteNG/mRemoteNG/issues/3335) | Malicious .xml executes any command when triggering a connection via the file or automatically | `1.78.*`, `Improvement required`, `Need 2 check` | 2026-06-04 |  |
| [#3333](https://github.com/mRemoteNG/mRemoteNG/issues/3333) | Recent MSI Builds for installation of mRemoteNG lacking | `1.78.*`, `Connections`, `Credentials`, `DBs`, `Enhancement`, `In development`, `In progress`, `Nightly Build` | 2026-06-03 |  |
| [#3311](https://github.com/mRemoteNG/mRemoteNG/issues/3311) | Add SFTP remote file transfer functionality | `Enhancement`, `Feature Request`, `Need 2 check`, `UI/UX` | 2026-05-14 |  |
| [#3254](https://github.com/mRemoteNG/mRemoteNG/issues/3254) | [Enhancement] Add visual focus indication to CTaskDialog buttons | `1.78.*`, `Enhancement`, `Theming`, `UI/UX` | 2026-04-05 |  |
| [#3252](https://github.com/mRemoteNG/mRemoteNG/issues/3252) | [Enhancement] Support multi-selection Enter key to open multiple connections in Connection Tree | `Enhancement` | 2026-04-02 |  |
| [#3170](https://github.com/mRemoteNG/mRemoteNG/issues/3170) | Opening Command sent too soon during SSH interactive login | `1.78.*`, `Improvement required`, `Nightly Build`, `SSH` | 2026-02-24 |  |
| [#3083](https://github.com/mRemoteNG/mRemoteNG/issues/3083) | Feature-Request: Tabs: show folder/path for connections with duplicate names | `1.78.*`, `Enhancement`, `UI/UX` | 2026-01-15 |  |
| [#2998](https://github.com/mRemoteNG/mRemoteNG/issues/2998) | [Request] Lower / Modify WindowsRegistryTests.cs | `.NET (dotNET)`, `1.78.*`, `dependencies`, `File system`, `Improvement required`, `Nightly Build` | 2025-11-07 |  |
| [#2959](https://github.com/mRemoteNG/mRemoteNG/issues/2959) | Add option to bind Connections and Config panel together | `1.78.*`, `Connections`, `Improvement required`, `UI/UX` | 2025-10-21 |  |
| [#2948](https://github.com/mRemoteNG/mRemoteNG/issues/2948) | Options panel settings should be auto-saved | `1.78.*`, `Improvement required`, `UI/UX` | 2025-10-19 |  |
| [#2912](https://github.com/mRemoteNG/mRemoteNG/issues/2912) | Security options should be greyed out by default | `1.78.*`, `Improvement required`, `Options`, `UI/UX` | 2025-10-16 |  |
| [#2891](https://github.com/mRemoteNG/mRemoteNG/issues/2891) | Color tabs unreadable on subpanels | `1.78.*`, `Improvement required`, `Nightly Build`, `Priority - Low`, `UI/UX` | 2025-10-12 |  |
| [#2880](https://github.com/mRemoteNG/mRemoteNG/issues/2880) | New color options return error Property value is not valid | `1.78.*`, `Enhancement`, `Nightly Build`, `UI/UX` | 2025-10-08 |  |
| [#2844](https://github.com/mRemoteNG/mRemoteNG/issues/2844) | Inconsistent display of missing dependencies | `1.78.*`, `Improvement required`, `Priority - Low`, `UI/UX` | 2025-10-06 |  |
| [#2826](https://github.com/mRemoteNG/mRemoteNG/issues/2826) | Tasks to be completed for a Beta Release | `1.78.*`, `Improvement required`, `In progress`, `Project Infrastructure` | 2025-09-24 |  |
| [#2809](https://github.com/mRemoteNG/mRemoteNG/issues/2809) | Create custom buttons for quick access to commands | `1.78.*`, `Feature Request`, `Needs implementation`, `UI/UX` | 2025-09-17 |  |
| [#2756](https://github.com/mRemoteNG/mRemoteNG/issues/2756) | mRemoteNG.appSettings - Consider moving to AppData Roaming | `1.78.*`, `Connections`, `Improvement required`, `Need 2 check`, `Needs implementation`, `Nightly Build`, `Settings` | 2025-08-22 |  |
| [#2735](https://github.com/mRemoteNG/mRemoteNG/issues/2735) | Lost focus on RDP's in SmartSize mode. | `1.78.*`, `Improvement required`, `Nightly Build`, `RDP`, `UI/UX` | 2025-08-19 |  |
| [#2711](https://github.com/mRemoteNG/mRemoteNG/issues/2711) | Full Screen rdp stuck on multi desktop view | `1.78.*`, `Improvement required`, `RDP`, `UI/UX` | 2025-06-25 |  |
| [#2703](https://github.com/mRemoteNG/mRemoteNG/issues/2703) | Selective xml export, export branches of the fastructure separately | `1.78.*`, `Feature Request` | 2025-06-14 |  |
| [#2697](https://github.com/mRemoteNG/mRemoteNG/issues/2697) | minimize on close | `1.78.*`, `Enhancement`, `Feature Request`, `Priority - Low` | 2025-05-22 |  |
| [#2694](https://github.com/mRemoteNG/mRemoteNG/issues/2694) | The settings window is to tall (v1.78.2.2932) | `1.78.*`, `HighDPI`, `Improvement required`, `Nightly Build`, `UI/UX` | 2025-05-13 |  |
| [#2686](https://github.com/mRemoteNG/mRemoteNG/issues/2686) | Adding additional fields | `1.78.*`, `Improvement required`, `Priority - Low`, `UI/UX` | 2025-04-04 |  |
| [#2681](https://github.com/mRemoteNG/mRemoteNG/issues/2681) | Use dotnet self-contained deployment | `.NET (dotNET)`, `1.78.*`, `Improvement required`, `Installer`, `Project Infrastructure` | 2025-03-31 |  |
| [#2679](https://github.com/mRemoteNG/mRemoteNG/issues/2679) | Feature Request: Text Zoom (Ctrl + Mouse Wheel) Functionality | `1.78.*`, `Enhancement`, `Help Wanted`, `Priority - Low` | 2025-03-28 |  |
| [#2651](https://github.com/mRemoteNG/mRemoteNG/issues/2651) | [Feature Request] Improved SSH Reconnect Experience | `1.77.4`, `Connections`, `Enhancement`, `Improvement required`, `SSH`, `UI/UX` | 2024-11-11 |  |
| [#2624](https://github.com/mRemoteNG/mRemoteNG/issues/2624) | REQ: Integrate zmodem support | `1.77.3`, `Feature Request`, `Need 2 check`, `SSH` | 2024-08-05 |  |
| [#2619](https://github.com/mRemoteNG/mRemoteNG/issues/2619) | A professional version of mRemoteNG tailored for enterprise use? | `1.77.3`, `1.77.4`, `Feature Request`, `Windows` | 2024-07-23 |  |
| [#2532](https://github.com/mRemoteNG/mRemoteNG/issues/2532) | Feature Request: Folders for external tools \| Hide tools from UI | `Enhancement`, `Feature Request`, `UI/UX` | 2023-12-08 |  |
| [#2531](https://github.com/mRemoteNG/mRemoteNG/issues/2531) | Feature request: Option to make mRemoteNG close itself after being idle for too long | `1.77.3`, `Enhancement`, `Improvement required`, `Settings` | 2023-12-06 |  |
| [#2511](https://github.com/mRemoteNG/mRemoteNG/issues/2511) | user defined connection parameters passed as variables to Ext. tool | `1.77.4`, `DBs`, `Enhancement`, `Import/Export`, `Improvement required` | 2023-10-19 |  |
| [#2509](https://github.com/mRemoteNG/mRemoteNG/issues/2509) | Opening Command pass password option | `1.77.4`, `Enhancement`, `Feature Request` | 2023-10-17 |  |
| [#2500](https://github.com/mRemoteNG/mRemoteNG/issues/2500) | Errors with MySQL database - Version Testing | `1.77.3`, `DBs`, `Enhancement`, `Improvement required`, `In progress`, `Priority - High` | 2023-10-06 |  |
| [#2499](https://github.com/mRemoteNG/mRemoteNG/issues/2499) | (Improvement) Writes config files into individual setting folder when in portable mode | `1.77.3`, `Connections`, `DBs`, `Enhancement`, `In progress`, `Settings` | 2023-10-06 |  |
| [#2472](https://github.com/mRemoteNG/mRemoteNG/issues/2472) | Feature request: Add production color frame for production sessions | `1.78.*`, `Connections`, `Improvement required`, `In development`, `Nightly Build`, `UI/UX` | 2023-08-07 |  |
| [#2469](https://github.com/mRemoteNG/mRemoteNG/issues/2469) | Difficult usage of smartsizing in mRemote | `1.77.3`, `Improvement required`, `UI/UX` | 2023-08-02 |  |
| [#2468](https://github.com/mRemoteNG/mRemoteNG/issues/2468) | Feature request: Ability to move full screen RDP session between monitors | `1.77.3`, `Improvement required`, `RDP`, `UI/UX` | 2023-07-26 |  |
| [#2467](https://github.com/mRemoteNG/mRemoteNG/issues/2467) | Use DefaultUsername as environment varible | `1.77.3`, `Connections`, `Feature Request`, `Improvement required`, `Needs implementation`, `Profiles`, `Settings`, `UI/UX` | 2023-07-25 |  |
| [#2442](https://github.com/mRemoteNG/mRemoteNG/issues/2442) | Remove Putty connections from connection list | `1.77.4`, `Connections`, `Enhancement`, `Options`, `Putty` | 2023-06-12 |  |
| [#2436](https://github.com/mRemoteNG/mRemoteNG/issues/2436) | Feature request: Vaultwarden integration (like Thycotic Secret Server) | `1.78.*`, `Feature Request`, `In development` | 2023-05-22 |  |
| [#2430](https://github.com/mRemoteNG/mRemoteNG/issues/2430) | Feature Request: Putty disconnect visibility / behaviour | `1.77.4`, `Improvement required`, `UI/UX` | 2023-04-26 |  |
| [#2428](https://github.com/mRemoteNG/mRemoteNG/issues/2428) | Open Web with Edge Chromium require folder permissions | `1.77.3`, `Improvement required`, `Need 2 check`, `WebView - Chromium` | 2023-04-24 |  |
| [#2414](https://github.com/mRemoteNG/mRemoteNG/issues/2414) | Identical nodes in connections tree | `Connections`, `Improvement required`, `UI/UX` | 2023-03-27 |  |
| [#2409](https://github.com/mRemoteNG/mRemoteNG/issues/2409) | Auto-collapse of Connection tab does not work if clicked on Connection tab. | `1.77.3`, `Connections`, `Improvement required`, `UI/UX` | 2023-03-24 |  |
| [#2406](https://github.com/mRemoteNG/mRemoteNG/issues/2406) | Disconnect has no confirmation dialog which potentially may lead to bad consequences | `1.77.3`, `Connections`, `Improvement required`, `UI/UX` | 2023-03-24 |  |
| [#2405](https://github.com/mRemoteNG/mRemoteNG/issues/2405) | Reconnect has no confirmation dialog which potentially may lead to bad consequences | `1.77.3`, `Connections`, `Improvement required`, `UI/UX` | 2023-03-24 |  |
| [#2404](https://github.com/mRemoteNG/mRemoteNG/issues/2404) | Three suggestions how to improve lost SSH connection | `1.77.3`, `Connections`, `Improvement required`, `SSH`, `UI/UX` | 2023-03-24 |  |
| [#2403](https://github.com/mRemoteNG/mRemoteNG/issues/2403) | Yes/No question with dangerous consequences | `1.77.3`, `Improvement required`, `SSH`, `UI/UX` | 2023-03-24 |  |
| [#2359](https://github.com/mRemoteNG/mRemoteNG/issues/2359) | Save scrollback after reconnectiong SSH session | `1.77.4`, `Connections`, `Improvement required`, `SSH`, `UI/UX` | 2023-02-27 |  |
| [#2358](https://github.com/mRemoteNG/mRemoteNG/issues/2358) | Connection closed by remote host popup | `Connections`, `Improvement required`, `Need 2 check`, `SSH`, `UI/UX` | 2023-02-23 |  |
| [#2349](https://github.com/mRemoteNG/mRemoteNG/issues/2349) | Feature Request: Move to Folder | `Connections`, `Feature Request`, `Needs implementation`, `UI/UX` | 2023-02-13 |  |
| [#2331](https://github.com/mRemoteNG/mRemoteNG/issues/2331) | Allow for multiple connection files to be opened at the same time | `1.78.*`, `Feature Request`, `In development`, `Profiles` | 2023-01-30 |  |
| [#2312](https://github.com/mRemoteNG/mRemoteNG/issues/2312) | Feature Request: Function to see at the Status LED if the Host does have an Authorized DNS Entry | `1.78.*`, `Feature Request`, `UI/UX` | 2022-12-06 |  |
| [#2302](https://github.com/mRemoteNG/mRemoteNG/issues/2302) | Mulitpane Display within a Panel | `Enhancement`, `SSH`, `UI/UX` | 2022-11-21 |  |
| [#2293](https://github.com/mRemoteNG/mRemoteNG/issues/2293) | filter by folder name but list all connections inside | `Connections`, `Improvement required`, `UI/UX` | 2022-10-01 |  |
| [#2291](https://github.com/mRemoteNG/mRemoteNG/issues/2291) | How to Disable Restricted Admin and Remote Credential Guard in Quick Connect for RDP | `Improvement required`, `RDP`, `UI/UX` | 2022-09-23 |  |
| [#2276](https://github.com/mRemoteNG/mRemoteNG/issues/2276) | Multi SSH - Grid | `1.8 (Fenix)`, `Feature Request`, `Multi SSH`, `UI/UX` | 2022-08-26 |  |
| [#2263](https://github.com/mRemoteNG/mRemoteNG/issues/2263) | Issue with stored creds and RDP quick connections | `Improvement required`, `RDP`, `UI/UX` | 2022-07-12 |  |
| [#2240](https://github.com/mRemoteNG/mRemoteNG/issues/2240) | Port number field is not used / ignored on web connections | `Enhancement`, `HTTP/HTTPS` | 2022-06-03 |  |
| [#2238](https://github.com/mRemoteNG/mRemoteNG/issues/2238) | SSH username to putty doesn't include domain when set | `1.77.3`, `Improvement required` | 2022-06-03 |  |
| [#2209](https://github.com/mRemoteNG/mRemoteNG/issues/2209) | [Connections][Filter]: switch between filter results by Up/Down arrow keys | `1.77.3`, `Connections`, `Improvement required`, `UI/UX` | 2022-04-25 |  |
| [#2207](https://github.com/mRemoteNG/mRemoteNG/issues/2207) | [Panels][Tabs]: work area doesn't switch to already activated connection's tab if it's on the other panel | `Improvement required`, `Need 2 check`, `UI/UX` | 2022-04-22 |  |
| [#2196](https://github.com/mRemoteNG/mRemoteNG/issues/2196) | Keep the field Username visible when selecting External Tool | `1.78.*`, `Improvement required`, `Nightly Build`, `UI/UX` | 2022-04-08 |  |
| [#2193](https://github.com/mRemoteNG/mRemoteNG/issues/2193) | feature request: copy password | `1.77.4`, `Feature Request`, `Request For Comment` | 2022-04-04 |  |
| [#2192](https://github.com/mRemoteNG/mRemoteNG/issues/2192) | Need enhance the tab panel as Edge browser does, maximize the screen usage | `HTTP/HTTPS`, `Improvement required`, `UI/UX` | 2022-03-31 |  |
| [#2183](https://github.com/mRemoteNG/mRemoteNG/issues/2183) | ssh(by puttyng) session window can't automatically adjust window follow the window size | `Improvement required`, `UI/UX` | 2022-03-10 |  |
| [#2181](https://github.com/mRemoteNG/mRemoteNG/issues/2181) | veriable for connection (name, ip, other attributes) | `1.77.3`, `Connections`, `Feature Request`, `Needs implementation` | 2022-02-25 |  |
| [#2178](https://github.com/mRemoteNG/mRemoteNG/issues/2178) | search folder name not the connections | `1.77.3`, `Improvement required`, `UI/UX` | 2022-02-25 |  |
| [#2175](https://github.com/mRemoteNG/mRemoteNG/issues/2175) | Focus Remote Session When Mouse is over RDP Area | `1.8 (Fenix)`, `Feature Request` | 2022-02-21 |  |
| [#2165](https://github.com/mRemoteNG/mRemoteNG/issues/2165) | my solution to autostart a specific connection then close mRemoteNG when this connection is closed | `1.8 (Fenix)`, `Improvement required` | 2022-02-10 |  |
| [#2159](https://github.com/mRemoteNG/mRemoteNG/issues/2159) | New or reconnected tabs are always placed at the end of the tab strip | `1.77.3`, `Improvement required`, `UI/UX` | 2022-02-04 |  |
| [#2157](https://github.com/mRemoteNG/mRemoteNG/issues/2157) | Auto-reconnect with different window size - Nightly 1.77.2 | `1.77.3`, `Improvement required`, `Needs User Verification`, `UI/UX` | 2022-02-03 |  |
| [#2155](https://github.com/mRemoteNG/mRemoteNG/issues/2155) | Inheritance field names cut off - Nightly 1.77.2 | `1.77.3`, `Improvement required`, `UI/UX` | 2022-02-03 |  |
| [#2153](https://github.com/mRemoteNG/mRemoteNG/issues/2153) | Config panel cannot be brought back - Nightly 1.77.2 | `1.77.3`, `Improvement required`, `UI/UX` | 2022-02-03 |  |
| [#2142](https://github.com/mRemoteNG/mRemoteNG/issues/2142) | mRemoteNG does not resize connection screens when disconnecting from HDMI - VERSION 1.76.20.24669 | `1.77.3`, `Improvement required`, `Need 2 check`, `UI/UX` | 2022-01-28 |  |
| [#2113](https://github.com/mRemoteNG/mRemoteNG/issues/2113) | REQUEST: Hide MultiSSH text | `Improvement required` | 2022-01-09 |  |
| [#2090](https://github.com/mRemoteNG/mRemoteNG/issues/2090) | RDP Sessions fail after opening like 8 or 10 sessions. have to close MRemoteNG in order to be able to connect again | `Improvement required`, `RDP` | 2021-12-16 |  |
| [#2060](https://github.com/mRemoteNG/mRemoteNG/issues/2060) | REQUEST: Quick connection history | `1.8 (Fenix)`, `Feature Request` | 2021-11-01 |  |
| [#2048](https://github.com/mRemoteNG/mRemoteNG/issues/2048) | New install -launching prompts for password | `Improvement required` | 2021-09-16 |  |
| [#2038](https://github.com/mRemoteNG/mRemoteNG/issues/2038) | BUG: When using SSH TUNNEL option host fingerprints aren't saved | `Improvement required`, `Need 2 check` | 2021-09-01 |  |
| [#2032](https://github.com/mRemoteNG/mRemoteNG/issues/2032) | Change configuration path in options | `1.77.3`, `Connections`, `Improvement required`, `In development` | 2021-08-27 |  |
| [#2031](https://github.com/mRemoteNG/mRemoteNG/issues/2031) | Internal error on imported connection attempt | `Connections`, `Improvement required`, `Need 2 check` | 2021-08-27 |  |
| [#2021](https://github.com/mRemoteNG/mRemoteNG/issues/2021) | Feature request - customise appearance of tabbed session/connections | `Improvement required`, `UI/UX` | 2021-08-12 |  |
| [#2018](https://github.com/mRemoteNG/mRemoteNG/issues/2018) | Feature request to start mRemoteNG with another configuration file sending as parameter | `Connections`, `Feature Request`, `Needs implementation` | 2021-08-06 |  |
| [#2015](https://github.com/mRemoteNG/mRemoteNG/issues/2015) | Microsoft Remote Desktop Connection Manager v2.82 .rdg support | `Import/Export`, `Improvement required` | 2021-08-05 |  |
| [#1998](https://github.com/mRemoteNG/mRemoteNG/issues/1998) | [request] option for show/hide full name of connections on tabs | `Improvement required`, `UI/UX` | 2021-07-11 |  |
| [#1992](https://github.com/mRemoteNG/mRemoteNG/issues/1992) | Non high contrast themes | `Improvement required`, `Theming` | 2021-07-06 |  |
| [#1991](https://github.com/mRemoteNG/mRemoteNG/issues/1991) | Pop-Up "Notification" need attach to panel | `Improvement required`, `UI/UX` | 2021-07-06 |  |
| [#1984](https://github.com/mRemoteNG/mRemoteNG/issues/1984) | More bottom TABS :) | `Improvement required`, `UI/UX` | 2021-06-29 |  |
| [#1981](https://github.com/mRemoteNG/mRemoteNG/issues/1981) | Multi SSH should allow Loading Scripts | `1.8 (Fenix)`, `Improvement required`, `Multi SSH`, `UI/UX` | 2021-06-29 |  |
| [#1974](https://github.com/mRemoteNG/mRemoteNG/issues/1974) | Bypass Windows Proxy | `1.77.2`, `Enhancement` | 2021-06-23 |  |
| [#1973](https://github.com/mRemoteNG/mRemoteNG/issues/1973) | REQUEST: Wide SOCKS4 support | `Feature Request`, `Help Wanted`, `Priority - Medium` | 2021-06-22 |  |
| [#1971](https://github.com/mRemoteNG/mRemoteNG/issues/1971) | Auto reconnect with VNC - Feature request | `Feature Request` | 2021-06-20 |  |
| [#1966](https://github.com/mRemoteNG/mRemoteNG/issues/1966) | This scenario please: like Windows Remote Desktop (fullscreen/taskbar) but with WOL and systray menu? | `Enhancement`, `UI/UX` | 2021-06-08 |  |
| [#1947](https://github.com/mRemoteNG/mRemoteNG/issues/1947) | Request to add a new feature: lock the window size of mRemoteNG | `Feature Request` | 2021-03-28 |  |
| [#1938](https://github.com/mRemoteNG/mRemoteNG/issues/1938) | Feature request: Import from a simple text list | `1.77.2`, `Feature Request` | 2021-02-24 |  |
| [#1931](https://github.com/mRemoteNG/mRemoteNG/issues/1931) | Feature Request: add path parameter for HTTP connections, or allow path in the hostname/IP parameter e.g. 127.0.0.1:9000/admin | `1.77.2`, `Enhancement`, `Priority - Medium` | 2021-02-12 |  |
| [#1922](https://github.com/mRemoteNG/mRemoteNG/issues/1922) | ADD SYNCHRONIZATION OF CONNECTION'S (TEAM WORK) VERY IMPORTANT!!!!!!!!!!! | `1.78.*`, `Feature Request` | 2021-01-21 |  |
| [#1920](https://github.com/mRemoteNG/mRemoteNG/issues/1920) | Feature Request: configurable border size | `1.77.2`, `Enhancement` | 2021-01-08 |  |
| [#1919](https://github.com/mRemoteNG/mRemoteNG/issues/1919) | Reference %name% variable/field in hostname field | `1.8 (Fenix)`, `Feature Request` | 2021-01-07 |  |
| [#1914](https://github.com/mRemoteNG/mRemoteNG/issues/1914) | FR: Column sorting in the ipscan grid | `Enhancement`, `Priority - Low` | 2020-12-11 |  |
| [#1908](https://github.com/mRemoteNG/mRemoteNG/issues/1908) | Subfolders in the Icon folder | `1.77.2`, `Feature Request` | 2020-12-01 |  |
| [#1907](https://github.com/mRemoteNG/mRemoteNG/issues/1907) | Allow arbitrary tunnel to be established before connection | `Feature Request` | 2020-11-25 |  |
| [#1904](https://github.com/mRemoteNG/mRemoteNG/issues/1904) | Feature Request : Add LAPS (Local Administrative Password Connection) as a connection method for RDP sessions | `Feature Request` | 2020-11-19 |  |
| [#1902](https://github.com/mRemoteNG/mRemoteNG/issues/1902) | Feature request: List session function (Like in Remote Desktop Connection Manager) | `1.8 (Fenix)`, `Enhancement`, `Needs implementation`, `RDP` | 2020-11-17 |  |
| [#1901](https://github.com/mRemoteNG/mRemoteNG/issues/1901) | How to "Use all my monitors for the remote session" like mstsc | `Improvement required`, `Needs implementation` | 2020-11-17 |  |
| [#1896](https://github.com/mRemoteNG/mRemoteNG/issues/1896) | Feature request: VMWare Horizon Client emulate/replace | `1.8 (Fenix)`, `Feature Request`, `Needs implementation` | 2020-11-12 |  |
| [#1895](https://github.com/mRemoteNG/mRemoteNG/issues/1895) | Feature issue: Notifications to TAB | `1.77.2`, `Improvement required` | 2020-11-12 |  |
| [#1894](https://github.com/mRemoteNG/mRemoteNG/issues/1894) | Feature request., simple logging | `1.8 (Fenix)`, `Enhancement` | 2020-11-11 |  |
| [#1893](https://github.com/mRemoteNG/mRemoteNG/issues/1893) | Feature Request : Port scanner | `1.77.2`, `Improvement required`, `Needs implementation` | 2020-11-11 |  |
| [#1885](https://github.com/mRemoteNG/mRemoteNG/issues/1885) | Sync connections to Google Drive | `1.77.2`, `Improvement required`, `Need 2 check` | 2020-10-25 |  |
| [#1880](https://github.com/mRemoteNG/mRemoteNG/issues/1880) | Missing server name in error 516 notification msg. | `Improvement required`, `Need 2 check`, `RDP` | 2020-10-14 |  |
| [#1874](https://github.com/mRemoteNG/mRemoteNG/issues/1874) | Use stored credentials in hostname for connexion using HTTP/HTTPs protocol | `Duplicate`, `Enhancement` | 2020-10-05 |  |
| [#1873](https://github.com/mRemoteNG/mRemoteNG/issues/1873) | Random freeze | `Improvement required`, `Need 2 check` | 2020-10-05 |  |
| [#1870](https://github.com/mRemoteNG/mRemoteNG/issues/1870) | Multiple screens | `1.8 (Fenix)`, `Improvement required` | 2020-10-01 |  |
| [#1869](https://github.com/mRemoteNG/mRemoteNG/issues/1869) | Server focus not switching in Connections pane after RDP session established | `Improvement required`, `Needs implementation`, `UI/UX` | 2020-09-29 |  |
| [#1864](https://github.com/mRemoteNG/mRemoteNG/issues/1864) | mRemoteNG config xml stored and opened from OneDrive, OneDrive stuck syncing xml file | `Improvement required`, `Need 2 check` | 2020-09-18 |  |
| [#1850](https://github.com/mRemoteNG/mRemoteNG/issues/1850) | Reduce size of XML connection file | `Enhancement` | 2020-08-30 |  |
| [#1834](https://github.com/mRemoteNG/mRemoteNG/issues/1834) | Request of new feature | `1.8 (Fenix)`, `Feature Request` | 2020-08-17 |  |
| [#1830](https://github.com/mRemoteNG/mRemoteNG/issues/1830) | Cannot change expired password error 3847 | `Improvement required`, `RDP`, `Windows` | 2020-08-10 |  |
| [#1828](https://github.com/mRemoteNG/mRemoteNG/issues/1828) | Smartsizing and Pre-defined Resolution | `Feature Request`, `Needs implementation` | 2020-08-07 |  |
| [#1818](https://github.com/mRemoteNG/mRemoteNG/issues/1818) | RDP Screen Size | `Improvement required`, `UI/UX` | 2020-07-15 |  |
| [#1798](https://github.com/mRemoteNG/mRemoteNG/issues/1798) | Grid lines are invisible in Config panel | `Improvement required`, `UI/UX` | 2020-06-20 |  |
| [#1797](https://github.com/mRemoteNG/mRemoteNG/issues/1797) | Prompt for credentials every time | `1.77.2`, `Enhancement` | 2020-06-18 |  |
| [#1793](https://github.com/mRemoteNG/mRemoteNG/issues/1793) | Select connection node for Actived/Current connection Tab. | `Feature Request`, `Improvement required`, `UI/UX` | 2020-06-15 |  |
| [#1789](https://github.com/mRemoteNG/mRemoteNG/issues/1789) | Add Support to use AD (LAPS feature) to obtain local Administrator login credentials | `Feature Request` | 2020-06-12 |  |
| [#1740](https://github.com/mRemoteNG/mRemoteNG/issues/1740) | Feature Request: Order Port Scan table by column and allow import several protocols | `1.77.2`, `Improvement required`, `UI/UX` | 2020-04-27 |  |
| [#1739](https://github.com/mRemoteNG/mRemoteNG/issues/1739) | Feature Request: SPICE protocol | `Connections`, `Enhancement`, `Need 2 check` | 2020-04-24 |  |
| [#1732](https://github.com/mRemoteNG/mRemoteNG/issues/1732) | Suggestion: automatic backup to the cloud or get data from the cloud | `1.77.2`, `File system`, `Improvement required` | 2020-04-12 |  |
| [#1719](https://github.com/mRemoteNG/mRemoteNG/issues/1719) | Option to disable the automatic backup of the XML parameters | `1.78.*`, `Backups`, `Connections`, `Enhancement`, `Settings` | 2020-03-24 |  |
| [#1682](https://github.com/mRemoteNG/mRemoteNG/issues/1682) | Panel Functionality | `Improvement required`, `UI/UX` | 2020-01-31 |  |
| [#1680](https://github.com/mRemoteNG/mRemoteNG/issues/1680) | Feature Request: Reconnection with Enter | `Enhancement` | 2020-01-24 |  |
| [#1676](https://github.com/mRemoteNG/mRemoteNG/issues/1676) | Feature Request: Integrated App resize event hook | `Feature Request`, `Needs implementation` | 2020-01-21 |  |
| [#1668](https://github.com/mRemoteNG/mRemoteNG/issues/1668) | Feature Request: Colored Borders | `Enhancement`, `Feature Request` | 2020-01-10 |  |
| [#1663](https://github.com/mRemoteNG/mRemoteNG/issues/1663) | No option to disable connection bar in RDP | `Enhancement`, `Need 2 check` | 2019-12-31 |  |
| [#1657](https://github.com/mRemoteNG/mRemoteNG/issues/1657) | Clising the last Session in a Panel should close the Panel | `Improvement required`, `Need 2 check`, `UI/UX` | 2019-12-18 |  |
| [#1649](https://github.com/mRemoteNG/mRemoteNG/issues/1649) | When a master password is defined, mRemoteNG may be lock after a while or when minimized | `1.77.2`, `Improvement required`, `Needs implementation`, `Priority - High` | 2019-12-03 |  |
| [#1640](https://github.com/mRemoteNG/mRemoteNG/issues/1640) | Password protection | `1.8 (Fenix)`, `Enhancement`, `Needs implementation` | 2019-11-19 |  |
| [#1633](https://github.com/mRemoteNG/mRemoteNG/issues/1633) | Feature Request: Show blank panel when clicking on disconnected machine | `1.77.2`, `Enhancement`, `UI/UX` | 2019-10-28 |  |
| [#1628](https://github.com/mRemoteNG/mRemoteNG/issues/1628) | Feature request : a drag&drop switch on/off | `1.77.2`, `Enhancement`, `Need 2 check` | 2019-10-22 |  |
| [#1606](https://github.com/mRemoteNG/mRemoteNG/issues/1606) | Feature request: Custom Resolution | `Enhancement` | 2019-10-02 |  |
| [#1603](https://github.com/mRemoteNG/mRemoteNG/issues/1603) | Suggestion: Combine the filter field with the connect field | `1.8 (Fenix)`, `Feature Request` | 2019-09-30 |  |
| [#1602](https://github.com/mRemoteNG/mRemoteNG/issues/1602) | Automatically configure public key authentication | `1.78.*`, `Enhancement`, `Help Wanted` | 2019-09-30 |  |
| [#1591](https://github.com/mRemoteNG/mRemoteNG/issues/1591) | Feature Request - Multi-user security | `.NET (dotNET)`, `1.78.*`, `DBs`, `Enhancement` | 2019-09-23 |  |
| [#1581](https://github.com/mRemoteNG/mRemoteNG/issues/1581) | Feature request: Move a tab to a new window | `1.8 (Fenix)`, `Enhancement` | 2019-09-13 |  |
| [#1577](https://github.com/mRemoteNG/mRemoteNG/issues/1577) | Unable to Copy/Paste to and from RDP sessions launched via Quick Connect toolbar | `Enhancement`, `Need 2 check`, `Request For Comment` | 2019-09-12 |  |
| [#1574](https://github.com/mRemoteNG/mRemoteNG/issues/1574) | Feature request: create a connection file based on csv | `Connections`, `Enhancement`, `UI/UX` | 2019-09-10 |  |
| [#1571](https://github.com/mRemoteNG/mRemoteNG/issues/1571) | note field | `1.78.*`, `Connections`, `Enhancement`, `In development`, `UI/UX` | 2019-09-09 |  |
| [#1554](https://github.com/mRemoteNG/mRemoteNG/issues/1554) | Seperate Hostname and IP in Port Scan | `Enhancement` | 2019-08-30 |  |
| [#1553](https://github.com/mRemoteNG/mRemoteNG/issues/1553) | Request: msiexec switch to not create shortcut on all users desktop during install | `Enhancement`, `Installer` | 2019-08-28 |  |
| [#1540](https://github.com/mRemoteNG/mRemoteNG/issues/1540) | Tab Order | `Feature Request` | 2019-08-14 |  |
| [#1515](https://github.com/mRemoteNG/mRemoteNG/issues/1515) | 希望提供%!password%进行urlencode编码的解决方案 | `1.77.2`, `Enhancement`, `Need 2 check`, `Needs implementation` | 2019-07-11 |  |
| [#1511](https://github.com/mRemoteNG/mRemoteNG/issues/1511) | Search filter improvement | `Connections`, `Enhancement`, `UI/UX` | 2019-07-10 |  |
| [#1504](https://github.com/mRemoteNG/mRemoteNG/issues/1504) | Enhancement: Export standard RDP file | `Enhancement`, `Not Planned` | 2019-06-27 |  |
| [#1499](https://github.com/mRemoteNG/mRemoteNG/issues/1499) | Feature Request: Reconnect on first connection | `Enhancement` | 2019-06-25 |  |
| [#1484](https://github.com/mRemoteNG/mRemoteNG/issues/1484) | Feature: Support onetime password | `Enhancement` | 2019-06-11 |  |
| [#1472](https://github.com/mRemoteNG/mRemoteNG/issues/1472) | No way to turn off Clipboard Sharing for VNC | `Enhancement` | 2019-06-03 |  |
| [#1466](https://github.com/mRemoteNG/mRemoteNG/issues/1466) | [feature request] Filter output icon | `Enhancement` | 2019-05-28 |  |
| [#1459](https://github.com/mRemoteNG/mRemoteNG/issues/1459) | feature request: prompt to save password | `Enhancement` | 2019-05-24 |  |
| [#1455](https://github.com/mRemoteNG/mRemoteNG/issues/1455) | CTRL+ALT+END inside RDP sessions (Inception) | `Enhancement`, `RDP` | 2019-05-22 |  |
| [#1454](https://github.com/mRemoteNG/mRemoteNG/issues/1454) | Feature Request - Ben.Demystifier for stacktraces | `Enhancement` | 2019-05-17 |  |
| [#1446](https://github.com/mRemoteNG/mRemoteNG/issues/1446) | Support variables in UserField | `Enhancement` | 2019-05-13 |  |
| [#1444](https://github.com/mRemoteNG/mRemoteNG/issues/1444) | Request for an option to keep tabs open after closing connection | `Enhancement` | 2019-05-08 |  |
| [#1432](https://github.com/mRemoteNG/mRemoteNG/issues/1432) | Help Page Should Always Load in Its Own Panel Tab | `Enhancement`, `Request For Comment` | 2019-05-01 |  |
| [#1413](https://github.com/mRemoteNG/mRemoteNG/issues/1413) | Cannot alt tab to session while in full screen | `Improvement required`, `UI/UX` | 2019-04-17 |  |
| [#1388](https://github.com/mRemoteNG/mRemoteNG/issues/1388) | Feature Request: Custom icons for External Tools | `Enhancement` | 2019-04-03 |  |
| [#1380](https://github.com/mRemoteNG/mRemoteNG/issues/1380) | Feature request: Hot keys | `Enhancement` | 2019-03-28 |  |
| [#1376](https://github.com/mRemoteNG/mRemoteNG/issues/1376) | Add tooltips for External tools | `Enhancement` | 2019-03-26 |  |
| [#1371](https://github.com/mRemoteNG/mRemoteNG/issues/1371) | Improve ability to investigate user.config startup issues | `Enhancement` | 2019-03-22 |  |
| [#1349](https://github.com/mRemoteNG/mRemoteNG/issues/1349) | Option to send Ctrl+Alt+Del or Ctrl+Alt_End for nested RDP sessions | `Enhancement` | 2019-03-13 |  |
| [#1346](https://github.com/mRemoteNG/mRemoteNG/issues/1346) | Fully encrypted "confCons.xml" become decrypted after opening in the upgraded version for the first time | `Enhancement`, `Request For Comment` | 2019-03-13 |  |
| [#1333](https://github.com/mRemoteNG/mRemoteNG/issues/1333) | Idea: create a option to Test all Connections - test if all TCP Ports are with status Open (available to connect) | `Enhancement` | 2019-03-02 |  |
| [#1307](https://github.com/mRemoteNG/mRemoteNG/issues/1307) | Request for new Settings: Do not dock to tab when minimazing from Full screen | `Improvement required`, `UI/UX` | 2019-02-13 |  |
| [#1302](https://github.com/mRemoteNG/mRemoteNG/issues/1302) | Add "Connect With Credentials" option | `Enhancement` | 2019-02-07 |  |
| [#1299](https://github.com/mRemoteNG/mRemoteNG/issues/1299) | Logoff Group | `Enhancement`, `Feature Request` | 2019-02-06 |  |
| [#1297](https://github.com/mRemoteNG/mRemoteNG/issues/1297) | External Tool opens behind mRemoteNG window | `Improvement required`, `UI/UX` | 2019-02-02 |  |
| [#1295](https://github.com/mRemoteNG/mRemoteNG/issues/1295) | Feature request: Add support for signed RDP connections | `Enhancement` | 2019-02-01 |  |
| [#1294](https://github.com/mRemoteNG/mRemoteNG/issues/1294) | RDP session on wrong monitor's taskbar | `Improvement required`, `UI/UX` | 2019-01-31 |  |
| [#1291](https://github.com/mRemoteNG/mRemoteNG/issues/1291) | External tool variable replacement helper | `Enhancement` | 2019-01-28 |  |
| [#1290](https://github.com/mRemoteNG/mRemoteNG/issues/1290) | Allow use of connection properties in multi-command | `Enhancement` | 2019-01-28 |  |
| [#1289](https://github.com/mRemoteNG/mRemoteNG/issues/1289) | [CredManager] Provide a way to save credentials when using the Import feature | `Enhancement` | 2019-01-27 |  |
| [#1287](https://github.com/mRemoteNG/mRemoteNG/issues/1287) | Default inheritance option | `1.77.2`, `Improvement required`, `In progress` | 2019-01-26 |  |
| [#1277](https://github.com/mRemoteNG/mRemoteNG/issues/1277) | Tab/panel closing prompt | `Improvement required`, `UI/UX` | 2019-01-13 |  |
| [#1269](https://github.com/mRemoteNG/mRemoteNG/issues/1269) | Allow changing behaviour of external tools toolstrip | `1.77.2`, `Improvement required`, `UI/UX` | 2019-01-11 |  |
| [#1262](https://github.com/mRemoteNG/mRemoteNG/issues/1262) | Use native OpenSSH client on newer Windows 10 | `1.77.4`, `Enhancement`, `MinTTY`, `OpenSSH`, `PowerShell` | 2019-01-10 |  |
| [#1260](https://github.com/mRemoteNG/mRemoteNG/issues/1260) | Begin automating UI acceptance tests | `Enhancement` | 2019-01-09 |  |
| [#1259](https://github.com/mRemoteNG/mRemoteNG/issues/1259) | Allow instantiating FrmMain with an in-memory only Settings instance | `Enhancement` | 2019-01-09 |  |
| [#1256](https://github.com/mRemoteNG/mRemoteNG/issues/1256) | [Feature Enhancement] - Multi SSH - paste multiple lines or commands | `Enhancement`, `Not Planned` | 2019-01-08 |  |
| [#1250](https://github.com/mRemoteNG/mRemoteNG/issues/1250) | [FR] Smart connection groups e.g. connected, recent | `Enhancement` | 2019-01-04 |  |
| [#1244](https://github.com/mRemoteNG/mRemoteNG/issues/1244) | Refactor connection importers to work with the credential manager | `Enhancement` | 2018-12-28 |  |
| [#1233](https://github.com/mRemoteNG/mRemoteNG/issues/1233) | Add Reconnect option to connection tree context menu | `Enhancement` | 2018-12-24 |  |
| [#1219](https://github.com/mRemoteNG/mRemoteNG/issues/1219) | Disable detach panels | `Enhancement` | 2018-12-21 |  |
| [#1206](https://github.com/mRemoteNG/mRemoteNG/issues/1206) | Autologin is not working any more | `Improvement required`, `UI/UX` | 2018-12-10 |  |
| [#1203](https://github.com/mRemoteNG/mRemoteNG/issues/1203) | Menu bar placement | `Enhancement` | 2018-12-07 |  |
| [#1192](https://github.com/mRemoteNG/mRemoteNG/issues/1192) | Feature Request: Template connections with no hostnames | `Enhancement` | 2018-11-15 |  |
| [#1190](https://github.com/mRemoteNG/mRemoteNG/issues/1190) | Support autenticate with private key without have too make an Putty Session | `Enhancement` | 2018-11-15 |  |
| [#1188](https://github.com/mRemoteNG/mRemoteNG/issues/1188) | [feature request]: Search using regexp support using "filter search" | `Enhancement` | 2018-11-14 |  |
| [#1185](https://github.com/mRemoteNG/mRemoteNG/issues/1185) | [Feature Request] Shift+LClick/Ctrl+LClick, select multiple targets, issue commands | `Enhancement` | 2018-11-12 |  |
| [#1171](https://github.com/mRemoteNG/mRemoteNG/issues/1171) | Feature Request: Hide Connection Tabs | `Enhancement`, `Priority - Medium`, `Ready`, `UI/UX`, `Vendor/Upstream Issue`, `Verified` | 2018-11-01 |  |
| [#1169](https://github.com/mRemoteNG/mRemoteNG/issues/1169) | CTRL+ALT+HOME to activate mRemoteNG | `Enhancement` | 2018-11-01 |  |
| [#1157](https://github.com/mRemoteNG/mRemoteNG/issues/1157) | No Logon information on Tab Names | `Enhancement` | 2018-10-26 |  |
| [#1154](https://github.com/mRemoteNG/mRemoteNG/issues/1154) | Feature Request: Default External Tools | `Enhancement` | 2018-10-25 |  |
| [#1152](https://github.com/mRemoteNG/mRemoteNG/issues/1152) | Investigate RDP resize reconnection speed improvements | `Enhancement` | 2018-10-24 |  |
| [#1148](https://github.com/mRemoteNG/mRemoteNG/issues/1148) | Feature Request: Connection Count in Folder Title | `Enhancement` | 2018-10-22 |  |
| [#1131](https://github.com/mRemoteNG/mRemoteNG/issues/1131) | Import CSV in MsSqL for mRemoteNG | `1.77.2`, `Connections`, `Improvement required`, `In progress`, `Windows` | 2018-10-10 |  |
| [#1121](https://github.com/mRemoteNG/mRemoteNG/issues/1121) | [Feature] Quick Connect in CAPS | `Enhancement`, `Verified` | 2018-10-01 |  |
| [#1114](https://github.com/mRemoteNG/mRemoteNG/issues/1114) | [Feature] Multi select / edit connections | `Enhancement`, `Priority - High` | 2018-09-28 |  |
| [#1111](https://github.com/mRemoteNG/mRemoteNG/issues/1111) | [FR][Full Screen]: add Minimize/Maximize/Close buttons | `Enhancement` | 2018-09-24 |  |
| [#1105](https://github.com/mRemoteNG/mRemoteNG/issues/1105) | [Feature] Easier panel window resizing and snapping | `Enhancement`, `Needs implementation`, `UI/UX` | 2018-09-19 |  |
| [#1101](https://github.com/mRemoteNG/mRemoteNG/issues/1101) | [Feature] Recently connected | `Enhancement` | 2018-09-12 |  |
| [#1097](https://github.com/mRemoteNG/mRemoteNG/issues/1097) | Save and restore custom connection panel layout | `Enhancement` | 2018-08-30 |  |
| [#1096](https://github.com/mRemoteNG/mRemoteNG/issues/1096) | Feature Request: UI Improvements to Connection Tree | `Enhancement` | 2018-08-30 |  |
| [#1093](https://github.com/mRemoteNG/mRemoteNG/issues/1093) | Themeing issue - Options -&gt; Theme | `1.77.2`, `Improvement required`, `UI/UX` | 2018-08-28 |  |
| [#1089](https://github.com/mRemoteNG/mRemoteNG/issues/1089) | Ensure connection filtering considers collapsed folders | `Enhancement` | 2018-08-25 |  |
| [#1086](https://github.com/mRemoteNG/mRemoteNG/issues/1086) | Save connection layout | `1.77.2`, `Improvement required`, `UI/UX`, `Windows` | 2018-08-24 |  |
| [#1084](https://github.com/mRemoteNG/mRemoteNG/issues/1084) | Inheritance issue | `Improvement required`, `UI/UX` | 2018-08-24 |  |
| [#1083](https://github.com/mRemoteNG/mRemoteNG/issues/1083) | Closing Connection Improvement | `Improvement required`, `UI/UX` | 2018-08-24 |  |
| [#1077](https://github.com/mRemoteNG/mRemoteNG/issues/1077) | Feature : Still missing Right Mouse - SETTINGS change | `Enhancement`, `UI/UX` | 2018-08-17 |  |
| [#1070](https://github.com/mRemoteNG/mRemoteNG/issues/1070) | Suppressing script issue | `Enhancement` | 2018-08-11 |  |
| [#1069](https://github.com/mRemoteNG/mRemoteNG/issues/1069) | VNC Send Special Keys, Windows Key | `Enhancement`, `VNC` | 2018-08-10 |  |
| [#1066](https://github.com/mRemoteNG/mRemoteNG/issues/1066) | [Feature Request] Zoom / Presentation mode | `Enhancement` | 2018-08-06 |  |
| [#1065](https://github.com/mRemoteNG/mRemoteNG/issues/1065) | UI: Some menu items that open a dialog are missing "..." on the menu item | `1.77.2`, `Improvement required`, `In progress`, `UI/UX` | 2018-08-06 |  |
| [#1049](https://github.com/mRemoteNG/mRemoteNG/issues/1049) | Provide a dump-debug tool for debugging issues from the field | `Enhancement` | 2018-07-27 |  |
| [#1047](https://github.com/mRemoteNG/mRemoteNG/issues/1047) | [Feature Request] Nested credential inheritance for folders | `1.77.2`, `Improvement required`, `UI/UX` | 2018-07-26 |  |
| [#1041](https://github.com/mRemoteNG/mRemoteNG/issues/1041) | [Feature request] Open RemoteNg connection via a CMD in a new tab | `1.8 (Fenix)`, `Enhancement`, `Needs implementation` | 2018-07-25 |  |
| [#1023](https://github.com/mRemoteNG/mRemoteNG/issues/1023) | Wrong (understated) resolution in full screen connection | `Enhancement`, `HighDPI`, `RDP` | 2018-07-13 |  |
| [#1013](https://github.com/mRemoteNG/mRemoteNG/issues/1013) | Request: Screenshot naming | `Enhancement` | 2018-06-28 |  |
| [#1012](https://github.com/mRemoteNG/mRemoteNG/issues/1012) | Feature to see what RDP sessions have been disconnected | `Enhancement` | 2018-06-27 |  |
| [#1011](https://github.com/mRemoteNG/mRemoteNG/issues/1011) | SSH tab activity signaling | `Enhancement` | 2018-06-26 |  |
| [#1008](https://github.com/mRemoteNG/mRemoteNG/issues/1008) | SmartSize: Add option to preserve aspect ratio | `Enhancement`, `UI/UX`, `VNC` | 2018-06-25 |  |
| [#1003](https://github.com/mRemoteNG/mRemoteNG/issues/1003) | [FR][External Tools]: adding hotkeys | `Enhancement` | 2018-06-21 |  |
| [#1000](https://github.com/mRemoteNG/mRemoteNG/issues/1000) | [Connections][Filter]: UI improvements | `Enhancement` | 2018-06-21 |  |
| [#995](https://github.com/mRemoteNG/mRemoteNG/issues/995) | Feature request. independent panels | `1.77.3`, `Enhancement`, `UI/UX` | 2018-06-17 |  |
| [#986](https://github.com/mRemoteNG/mRemoteNG/issues/986) | RDP window not expandable | `Enhancement` | 2018-05-30 |  |
| [#985](https://github.com/mRemoteNG/mRemoteNG/issues/985) | idea. entity instead of multiple connections | `Enhancement` | 2018-05-28 |  |
| [#984](https://github.com/mRemoteNG/mRemoteNG/issues/984) | Create link to connection instead of duplicating? | `Enhancement` | 2018-05-28 |  |
| [#976](https://github.com/mRemoteNG/mRemoteNG/issues/976) | Add plugin support | `Enhancement` | 2018-05-08 |  |
| [#972](https://github.com/mRemoteNG/mRemoteNG/issues/972) | Feature request: Type password, or Type clipboard text | `Enhancement` | 2018-05-04 |  |
| [#969](https://github.com/mRemoteNG/mRemoteNG/issues/969) | Request import bookmarks.html and right-click properties | `Enhancement` | 2018-04-26 |  |
| [#967](https://github.com/mRemoteNG/mRemoteNG/issues/967) | Feature Request - Search inside the session output | `Enhancement` | 2018-04-26 |  |
| [#955](https://github.com/mRemoteNG/mRemoteNG/issues/955) | Add option to increase the connection attempt limit beyond 20 | `Enhancement` | 2018-04-18 |  |
| [#945](https://github.com/mRemoteNG/mRemoteNG/issues/945) | Feature Request - Multiselect when organizing connections | `Enhancement` | 2018-04-12 |  |
| [#943](https://github.com/mRemoteNG/mRemoteNG/issues/943) | Use port specified in connection when running CheckHostAlive | `Enhancement` | 2018-04-10 |  |
| [#939](https://github.com/mRemoteNG/mRemoteNG/issues/939) | [Feature Request] Dynamic Connection Folder | `1.8 (Fenix)`, `Enhancement`, `UI/UX` | 2018-04-06 |  |
| [#937](https://github.com/mRemoteNG/mRemoteNG/issues/937) | Feature Request - log to Syslog server instead of file | `Enhancement`, `Not Planned` | 2018-04-06 |  |
| [#933](https://github.com/mRemoteNG/mRemoteNG/issues/933) | Feature Request - Add startup command for command line | `Enhancement`, `Needs implementation` | 2018-04-04 |  |
| [#932](https://github.com/mRemoteNG/mRemoteNG/issues/932) | Unable to specify user in the Quick Connect field | `Enhancement` | 2018-04-04 |  |
| [#927](https://github.com/mRemoteNG/mRemoteNG/issues/927) | Allow to add more root nodes in element tree - feature request | `Enhancement` | 2018-03-28 |  |
| [#926](https://github.com/mRemoteNG/mRemoteNG/issues/926) | Support for custom user fields, which are able to be passed into external application | `Enhancement` | 2018-03-27 |  |
| [#921](https://github.com/mRemoteNG/mRemoteNG/issues/921) | Add support for excluding certain active connections from multi ssh commands | `Enhancement` | 2018-03-22 |  |
| [#905](https://github.com/mRemoteNG/mRemoteNG/issues/905) | Current selection should reflect connection state in right panel. | `Enhancement` | 2018-03-02 |  |
| [#901](https://github.com/mRemoteNG/mRemoteNG/issues/901) | [Feature Request] Dynamic Connection Folder pulls connection list via REST | `Enhancement` | 2018-03-01 |  |
| [#895](https://github.com/mRemoteNG/mRemoteNG/issues/895) | Integrated winbox.exe clients are used to manage ROS devices | `Enhancement` | 2018-02-22 |  |
| [#891](https://github.com/mRemoteNG/mRemoteNG/issues/891) | Menu for connection tree | `Improvement required`, `UI/UX` | 2018-02-21 |  |
| [#889](https://github.com/mRemoteNG/mRemoteNG/issues/889) | mRemoteNG could work around longstanding "Bypass RDG" bug in MSTSC | `Enhancement` | 2018-02-19 |  |
| [#888](https://github.com/mRemoteNG/mRemoteNG/issues/888) | Connection Templates | `Enhancement` | 2018-02-17 |  |
| [#887](https://github.com/mRemoteNG/mRemoteNG/issues/887) | Import RDM Files from Remote Desktop Manger (Devolutions) | `Enhancement`, `Help Wanted` | 2018-02-16 |  |
| [#886](https://github.com/mRemoteNG/mRemoteNG/issues/886) | Double click to open duplicate session | `1.77.4`, `Enhancement`, `UI/UX` | 2018-02-15 |  |
| [#885](https://github.com/mRemoteNG/mRemoteNG/issues/885) | Improve error message of RDP error 2825 (NLA not enabed) | `Enhancement`, `Help Wanted` | 2018-02-14 |  |
| [#879](https://github.com/mRemoteNG/mRemoteNG/issues/879) | ALT+# to move among tabs | `Enhancement` | 2018-02-05 |  |
| [#876](https://github.com/mRemoteNG/mRemoteNG/issues/876) | Add Wake-On-Lan | `Enhancement` | 2018-01-31 |  |
| [#875](https://github.com/mRemoteNG/mRemoteNG/issues/875) | Allow setting RDP AuthenticationLevel to prevent NLA error 2825 | `Enhancement` | 2018-01-31 |  |
| [#874](https://github.com/mRemoteNG/mRemoteNG/issues/874) | [Feature Request] Allow connections to behave as folders. | `Enhancement` | 2018-01-30 |  |
| [#871](https://github.com/mRemoteNG/mRemoteNG/issues/871) | Enhancement - Pick database on logon | `Enhancement` | 2018-01-25 |  |
| [#866](https://github.com/mRemoteNG/mRemoteNG/issues/866) | HotKey going to quick connect | `Enhancement` | 2018-01-15 |  |
| [#857](https://github.com/mRemoteNG/mRemoteNG/issues/857) | database and multiple users | `.NET (dotNET)`, `1.78.*`, `DBs`, `Enhancement` | 2018-01-04 |  |
| [#851](https://github.com/mRemoteNG/mRemoteNG/issues/851) | Add support for KiTTY | `1.77.4`, `Enhancement`, `Improvement required`, `UI/UX` | 2017-12-21 |  |
| [#848](https://github.com/mRemoteNG/mRemoteNG/issues/848) | FR: Automatic Sort as an inheritable setting on containers | `Enhancement` | 2017-12-18 |  |
| [#847](https://github.com/mRemoteNG/mRemoteNG/issues/847) | RDP fullscreen and redirect keys | `1.77.2`, `Improvement required`, `UI/UX` | 2017-12-15 |  |
| [#837](https://github.com/mRemoteNG/mRemoteNG/issues/837) | extApps.xml file is unencrypted!!! | `Enhancement` | 2017-12-11 |  |
| [#835](https://github.com/mRemoteNG/mRemoteNG/issues/835) | Automatically propagate inheritance | `Enhancement` | 2017-12-09 |  |
| [#825](https://github.com/mRemoteNG/mRemoteNG/issues/825) | request - minimize focus loss during new connections | `Improvement required`, `RDP`, `UI/UX` | 2017-11-30 |  |
| [#816](https://github.com/mRemoteNG/mRemoteNG/issues/816) | Feature Request - Alternative hostname/IP connect option | `Enhancement` | 2017-11-23 |  |
| [#808](https://github.com/mRemoteNG/mRemoteNG/issues/808) | Save column-width of external programs panel | `Enhancement` | 2017-11-17 |  |
| [#793](https://github.com/mRemoteNG/mRemoteNG/issues/793) | Keep tab name when reconnect | `Enhancement` | 2017-11-10 |  |
| [#787](https://github.com/mRemoteNG/mRemoteNG/issues/787) | [Future request] Microsoft remote assistance and/or Remote Control | `1.77.4`, `Enhancement`, `Need 2 check`, `Windows` | 2017-11-09 |  |
| [#774](https://github.com/mRemoteNG/mRemoteNG/issues/774) | Add support for dynamic tab names | `Enhancement` | 2017-11-05 |  |
| [#773](https://github.com/mRemoteNG/mRemoteNG/issues/773) | FR: Make sorting connections reversible | `Enhancement` | 2017-11-05 |  |
| [#768](https://github.com/mRemoteNG/mRemoteNG/issues/768) | Add integration support for some common external apps | `Enhancement` | 2017-11-02 |  |
| [#764](https://github.com/mRemoteNG/mRemoteNG/issues/764) | Search stops at first occurence of a word even if there is an exact match | `Enhancement`, `Need 2 check` | 2017-11-01 |  |
| [#750](https://github.com/mRemoteNG/mRemoteNG/issues/750) | "Reconnect to previously opened sessions on startup" option doesn't work | `Enhancement` | 2017-10-23 |  |
| [#743](https://github.com/mRemoteNG/mRemoteNG/issues/743) | Easier switching between sessions | `Enhancement` | 2017-10-18 |  |
| [#740](https://github.com/mRemoteNG/mRemoteNG/issues/740) | Item encoding in SQL Database | `Enhancement` | 2017-10-13 |  |
| [#739](https://github.com/mRemoteNG/mRemoteNG/issues/739) | integrate with Guacamole database | `Enhancement` | 2017-10-12 |  |
| [#738](https://github.com/mRemoteNG/mRemoteNG/issues/738) | Feature Request: registry key for disabling saving passwords | `1.78.*`, `Feature Request`, `Profiles` | 2017-10-11 |  |
| [#737](https://github.com/mRemoteNG/mRemoteNG/issues/737) | wsltty cannot integrate | `Feature Request` | 2017-10-10 |  |
| [#734](https://github.com/mRemoteNG/mRemoteNG/issues/734) | Feature Request: Launch saved RDP sessions using external tools | `Enhancement` | 2017-10-05 |  |
| [#731](https://github.com/mRemoteNG/mRemoteNG/issues/731) | Allow loading and saving ExternalTools to Database | `Enhancement` | 2017-09-28 |  |
| [#727](https://github.com/mRemoteNG/mRemoteNG/issues/727) | Feature request - option to dialup connection and wait for ip availability? | `Enhancement` | 2017-09-21 |  |
| [#722](https://github.com/mRemoteNG/mRemoteNG/issues/722) | Allow loading and saving ExternalTools XML from different locations | `1.77.3`, `Enhancement`, `Settings` | 2017-09-15 |  |
| [#719](https://github.com/mRemoteNG/mRemoteNG/issues/719) | Ext. App protocol should accept override able parameters | `Enhancement` | 2017-09-15 |  |
| [#717](https://github.com/mRemoteNG/mRemoteNG/issues/717) | True Color (24bit RGB) support | `Enhancement` | 2017-09-15 |  |
| [#703](https://github.com/mRemoteNG/mRemoteNG/issues/703) | Request: Web Based mRemoteNG | `Enhancement` | 2017-09-08 |  |
| [#693](https://github.com/mRemoteNG/mRemoteNG/issues/693) | linux subsystem ubuntu terminal in windows 10 as external tool | `Enhancement`, `Need 2 check`, `WSL` | 2017-09-06 |  |
| [#687](https://github.com/mRemoteNG/mRemoteNG/issues/687) | Key Combinations / Hot key passthrough exceptions. | `Enhancement` | 2017-08-29 |  |
| [#674](https://github.com/mRemoteNG/mRemoteNG/issues/674) | Remote Session Performance Statistics | `Enhancement`, `Needs implementation`, `Reporting`, `Stats` | 2017-08-07 |  |
| [#672](https://github.com/mRemoteNG/mRemoteNG/issues/672) | Credential manager: disallow repo entries that point to the same file | `Enhancement` | 2017-08-05 |  |
| [#667](https://github.com/mRemoteNG/mRemoteNG/issues/667) | Refactor database schema to work with credential manager | `Enhancement` | 2017-07-30 |  |
| [#666](https://github.com/mRemoteNG/mRemoteNG/issues/666) | Option to hide menu strip | `Enhancement` | 2017-07-28 |  |
| [#660](https://github.com/mRemoteNG/mRemoteNG/issues/660) | Credential manager confcons upgrader | `1.78.*`, `DBs`, `Enhancement`, `In development`, `In progress` | 2017-07-26 |  |
| [#656](https://github.com/mRemoteNG/mRemoteNG/issues/656) | Implement a listener mode for incoming reverse VNC connections (custom ports) | `Enhancement`, `VNC` | 2017-07-26 |  |
| [#649](https://github.com/mRemoteNG/mRemoteNG/issues/649) | Cannot integrate any external app | `Improvement required`, `Needs implementation`, `UI/UX` | 2017-07-20 |  |
| [#642](https://github.com/mRemoteNG/mRemoteNG/issues/642) | Custom Icons on Folder/Connection | `Enhancement`, `UI/UX` | 2017-07-17 |  |
| [#640](https://github.com/mRemoteNG/mRemoteNG/issues/640) | Color depth not settable for VNC, causes immediate disconnect from x11VNC | `Enhancement`, `VNC` | 2017-07-14 |  |
| [#630](https://github.com/mRemoteNG/mRemoteNG/issues/630) | Support importing vSphere VM list | `Enhancement` | 2017-07-04 |  |
| [#624](https://github.com/mRemoteNG/mRemoteNG/issues/624) | Ability to import from mtputty | `Enhancement`, `Help Wanted` | 2017-06-30 |  |
| [#621](https://github.com/mRemoteNG/mRemoteNG/issues/621) | Feature request: Add variables to Name field | `Enhancement` | 2017-06-28 |  |
| [#601](https://github.com/mRemoteNG/mRemoteNG/issues/601) | Request: Tab Titles to Follow Current SSH Session | `Enhancement`, `Priority - High`, `Ready`, `UI/UX` | 2017-06-16 |  |
| [#586](https://github.com/mRemoteNG/mRemoteNG/issues/586) | Reconnect when ready by default option | `Enhancement` | 2017-06-14 |  |
| [#578](https://github.com/mRemoteNG/mRemoteNG/issues/578) | Open RDP from command line: Feature Request | `Enhancement` | 2017-06-08 |  |
| [#574](https://github.com/mRemoteNG/mRemoteNG/issues/574) | Thumbnail of Group | `Enhancement` | 2017-06-02 |  |
| [#573](https://github.com/mRemoteNG/mRemoteNG/issues/573) | Ultra VNC compatibility | `Enhancement`, `Help Wanted`, `Priority - High`, `Ready`, `VNC` | 2017-05-30 |  |
| [#571](https://github.com/mRemoteNG/mRemoteNG/issues/571) | External Tools Categories | `Enhancement`, `Help Wanted` | 2017-05-26 |  |
| [#568](https://github.com/mRemoteNG/mRemoteNG/issues/568) | NATIVE IMPORT FROM ROYALTS | `Enhancement`, `Help Wanted` | 2017-05-24 |  |
| [#551](https://github.com/mRemoteNG/mRemoteNG/issues/551) | Add support for X2GO client | `Enhancement`, `Help Wanted` | 2017-05-12 |  |
| [#549](https://github.com/mRemoteNG/mRemoteNG/issues/549) | Request: Another option when SQL fails to connect | `Enhancement` | 2017-05-10 |  |
| [#547](https://github.com/mRemoteNG/mRemoteNG/issues/547) | Add option to automatically start with windows in Tools &gt; Options dialog | `Enhancement` | 2017-05-10 |  |
| [#537](https://github.com/mRemoteNG/mRemoteNG/issues/537) | Wind 10 Taskbar additional task on right click | `Enhancement` | 2017-05-03 |  |
| [#515](https://github.com/mRemoteNG/mRemoteNG/issues/515) | Custom Virtual Channel DLL | `Enhancement` | 2017-04-17 |  |
| [#509](https://github.com/mRemoteNG/mRemoteNG/issues/509) | VNC Chat | `Enhancement`, `VNC` | 2017-04-14 |  |
| [#498](https://github.com/mRemoteNG/mRemoteNG/issues/498) | Custom Resolution Support | `Enhancement`, `Ready`, `UI/UX`, `Verified` | 2017-04-10 |  |
| [#497](https://github.com/mRemoteNG/mRemoteNG/issues/497) | SQL Feature requests | `Enhancement` | 2017-04-10 |  |
| [#494](https://github.com/mRemoteNG/mRemoteNG/issues/494) | Real VNC v5.3.2 | `Enhancement`, `TechnicalDebt`, `Vendor/Upstream Issue`, `VNC` | 2017-04-07 |  |
| [#492](https://github.com/mRemoteNG/mRemoteNG/issues/492) | External Tools do get empty %username% %password% fields | `Enhancement` | 2017-04-07 |  |
| [#484](https://github.com/mRemoteNG/mRemoteNG/issues/484) | support for VNC proxy (mirror) | `Enhancement`, `VNC` | 2017-04-04 |  |
| [#477](https://github.com/mRemoteNG/mRemoteNG/issues/477) | Shortcuts for copy & paste | `Enhancement` | 2017-03-30 |  |
| [#461](https://github.com/mRemoteNG/mRemoteNG/issues/461) | Support connections to vino VNC server | `Enhancement`, `Help Wanted`, `Priority - Medium`, `Ready`, `VNC` | 2017-03-17 |  |
| [#457](https://github.com/mRemoteNG/mRemoteNG/issues/457) | Reconnect on a new Panel should stay in the new panel and not go to the default panel | `Enhancement` | 2017-03-16 |  |
| [#450](https://github.com/mRemoteNG/mRemoteNG/issues/450) | Usability fix: Make connections editable from the right click context menu | `Enhancement`, `Priority - Medium`, `UI/UX` | 2017-03-12 |  |
| [#445](https://github.com/mRemoteNG/mRemoteNG/issues/445) | Currently open connections are NOT viewable in "one single/collective area" | `Enhancement`, `UI/UX` | 2017-03-09 |  |
| [#444](https://github.com/mRemoteNG/mRemoteNG/issues/444) | Add support for MS-Logon UltraVNC | `Enhancement`, `TechnicalDebt`, `VNC` | 2017-03-08 |  |
| [#435](https://github.com/mRemoteNG/mRemoteNG/issues/435) | Feature Request: Allow for folders to set Hostname/IP | `1.78.*`, `Connections`, `Enhancement`, `UI/UX` | 2017-03-06 |  |
| [#426](https://github.com/mRemoteNG/mRemoteNG/issues/426) | Feature Request: Paste Commands | `Enhancement` | 2017-03-02 |  |
| [#423](https://github.com/mRemoteNG/mRemoteNG/issues/423) | Feature Request: disable save config to sql server | `DBs`, `Enhancement`, `Needs implementation` | 2017-03-02 |  |
| [#420](https://github.com/mRemoteNG/mRemoteNG/issues/420) | Feature Request: PROXY support for RDP connections | `1.77.3`, `Enhancement`, `Feature Request`, `Help Wanted`, `Need 2 check`, `Needs implementation` | 2017-03-01 |  |
| [#417](https://github.com/mRemoteNG/mRemoteNG/issues/417) | External tool as tab | `Enhancement` | 2017-02-25 |  |
| [#409](https://github.com/mRemoteNG/mRemoteNG/issues/409) | Feature request: Cloud providers support or multiple user access to configuration | `Enhancement` | 2017-02-21 |  |
| [#397](https://github.com/mRemoteNG/mRemoteNG/issues/397) | Feature Request: Connect another time to a server with CTRL+DOUBLECLICK | `Enhancement`, `UI/UX` | 2017-02-09 |  |
| [#389](https://github.com/mRemoteNG/mRemoteNG/issues/389) | option to hide the saved putty sessions | `1.77.3`, `Enhancement`, `Settings`, `UI/UX` | 2017-02-03 |  |
| [#388](https://github.com/mRemoteNG/mRemoteNG/issues/388) | [Feature Request] Allow for Additional User Fields | `Enhancement` | 2017-02-02 |  |
| [#387](https://github.com/mRemoteNG/mRemoteNG/issues/387) | Ability to import Terminals XML file | `Enhancement` | 2017-02-02 |  |
| [#383](https://github.com/mRemoteNG/mRemoteNG/issues/383) | Add "Remote Audio Capture" option | `Enhancement` | 2017-01-31 |  |
| [#379](https://github.com/mRemoteNG/mRemoteNG/issues/379) | Scripting support on text terminal (Feature Request) | `Enhancement` | 2017-01-29 |  |
| [#376](https://github.com/mRemoteNG/mRemoteNG/issues/376) | Make custom password mandatory | `Enhancement`, `Priority - High`, `Ready` | 2017-01-28 |  |
| [#361](https://github.com/mRemoteNG/mRemoteNG/issues/361) | Add ConEmu | `Enhancement` | 2017-01-24 |  |
| [#356](https://github.com/mRemoteNG/mRemoteNG/issues/356) | Support VMRC | `Enhancement` | 2017-01-23 |  |
| [#351](https://github.com/mRemoteNG/mRemoteNG/issues/351) | Dynamic tab name | `Enhancement` | 2017-01-18 |  |
| [#347](https://github.com/mRemoteNG/mRemoteNG/issues/347) | Additional info to be displayed about SSH/PuTTY sessions under tab bar | `Enhancement` | 2017-01-14 |  |
| [#346](https://github.com/mRemoteNG/mRemoteNG/issues/346) | mRemoteNG Chocolatey Package | `Enhancement`, `Not Planned`, `Priority - Low` | 2017-01-13 |  |
| [#343](https://github.com/mRemoteNG/mRemoteNG/issues/343) | [Request] Minimize tabs on connect | `Enhancement`, `UI/UX` | 2017-01-12 |  |
| [#336](https://github.com/mRemoteNG/mRemoteNG/issues/336) | Handling Multiple Instances | `Enhancement`, `Help Wanted`, `Priority - Medium`, `Ready` | 2017-01-10 |  |
| [#318](https://github.com/mRemoteNG/mRemoteNG/issues/318) | Feature Request: Auto Start/Stop external tool on boot/shutdown | `Enhancement` | 2016-12-22 |  |
| [#317](https://github.com/mRemoteNG/mRemoteNG/issues/317) | RDP: Select which drives to redirect | `Enhancement`, `Help Wanted`, `Ready` | 2016-12-21 |  |
| [#314](https://github.com/mRemoteNG/mRemoteNG/issues/314) | Provide installer in portableapps.com format | `Enhancement`, `Not Planned`, `Priority - Low` | 2016-12-15 |  |
| [#308](https://github.com/mRemoteNG/mRemoteNG/issues/308) | Spread the Joy of mRemoteNG to Mac and Mobile | `1.8 (Fenix)`, `Enhancement`, `Help Wanted`, `Ready` | 2016-12-09 |  |
| [#301](https://github.com/mRemoteNG/mRemoteNG/issues/301) | RDS Session desktop shortcut | `Enhancement`, `Help Wanted`, `Ready` | 2016-12-07 |  |
| [#251](https://github.com/mRemoteNG/mRemoteNG/issues/251) | Add global SmartSize option for RDP/VNC | `1.77.4`, `Enhancement`, `Improvement required` | 2016-11-15 |  |
| [#239](https://github.com/mRemoteNG/mRemoteNG/issues/239) | Improve UX regarding Default Connection values | `Enhancement`, `UI/UX` | 2016-11-10 |  |
| [#238](https://github.com/mRemoteNG/mRemoteNG/issues/238) | Improve UX regarding Default Inheritance | `Enhancement`, `UI/UX` | 2016-11-10 |  |
| [#237](https://github.com/mRemoteNG/mRemoteNG/issues/237) | Improve UX relating to setting encryption password | `Enhancement`, `UI/UX` | 2016-11-10 |  |
| [#226](https://github.com/mRemoteNG/mRemoteNG/issues/226) | Feature Request: Support for RemoteApps | `Enhancement` | 2016-11-07 |  |
| [#213](https://github.com/mRemoteNG/mRemoteNG/issues/213) | Serial option for Putty | `Connections`, `Enhancement` | 2016-11-01 |  |
| [#208](https://github.com/mRemoteNG/mRemoteNG/issues/208) | MR-184: Feature request: Credential repository | `1.78.*`, `Enhancement`, `Ready` | 2016-10-28 |  |
| [#205](https://github.com/mRemoteNG/mRemoteNG/issues/205) | Improve search, sorting and display for connection using TAG | `Enhancement` | 2016-10-27 |  |
| [#202](https://github.com/mRemoteNG/mRemoteNG/issues/202) | Mouse button swapping in RDP sessions | `Enhancement`, `Priority - Low` | 2016-10-26 |  |
| [#192](https://github.com/mRemoteNG/mRemoteNG/issues/192) | RDP using multiple monitors | `1.78.*`, `Enhancement`, `RDP`, `UI/UX` | 2016-10-21 |  |
| [#187](https://github.com/mRemoteNG/mRemoteNG/issues/187) | Implment KeePass integration | `Enhancement`, `Help Wanted` | 2016-10-20 |  |
| [#183](https://github.com/mRemoteNG/mRemoteNG/issues/183) | Better / Full featured File Transfer | `Enhancement`, `Priority - Medium`, `Ready` | 2016-10-19 |  |
| [#182](https://github.com/mRemoteNG/mRemoteNG/issues/182) | Rewrite database (SQL) connection storage | `Enhancement`, `Help Wanted`, `Ready` | 2016-10-19 |  |
| [#181](https://github.com/mRemoteNG/mRemoteNG/issues/181) | Replace PuTTY with SSH.NET | `Enhancement`, `Help Wanted`, `Priority - Medium`, `Ready` | 2016-10-19 |  |
| [#163](https://github.com/mRemoteNG/mRemoteNG/issues/163) | URL handlers | `Enhancement` | 2016-10-12 |  |
| [#153](https://github.com/mRemoteNG/mRemoteNG/issues/153) | Merge connections panel and config panel | `Enhancement` | 2016-10-07 |  |

### UI / UX (57)

_Layout, panels, theming and interaction problems._

| Issue | Title | Labels | Opened | Status |
|---|---|---|---|---|
| [#3336](https://github.com/mRemoteNG/mRemoteNG/issues/3336) | transfer direction unclear | `1.78.*`, `Need 2 check`, `UI/UX` | 2026-06-09 |  |
| [#3224](https://github.com/mRemoteNG/mRemoteNG/issues/3224) | RDP Window Scaling Issues and Panel Order Changes in Recent Versions | `1.78.*`, `In progress`, `Need 2 check`, `UI/UX` | 2026-03-19 |  |
| [#3222](https://github.com/mRemoteNG/mRemoteNG/issues/3222) | Mouse clicks aren't at the cursor position when windows scaling is not at 100% | `1.78.*`, `Need 2 check`, `Nightly Build`, `UI/UX` | 2026-03-18 |  |
| [#3175](https://github.com/mRemoteNG/mRemoteNG/issues/3175) | AD group protected user | `1.78.*`, `Active Directory`, `RDP`, `UI/UX`, `Windows` | 2026-02-25 |  |
| [#2949](https://github.com/mRemoteNG/mRemoteNG/issues/2949) | Options do not belong to File menu | `Need 2 check`, `UI/UX` | 2025-10-19 |  |
| [#2914](https://github.com/mRemoteNG/mRemoteNG/issues/2914) | Empty Option panel when Theme is canceled | `.NET (dotNET)`, `1.78.*`, `Need 2 check`, `Settings`, `UI/UX` | 2025-10-16 |  |
| [#2913](https://github.com/mRemoteNG/mRemoteNG/issues/2913) | SQL Server options fields can't be filled in | `1.78.*`, `DBs`, `Nightly Build`, `UI/UX` | 2025-10-16 |  |
| [#2910](https://github.com/mRemoteNG/mRemoteNG/issues/2910) | Always show panel tabs corrupts Options panel display | `1.78.*`, `Nightly Build`, `Options`, `Panels`, `UI/UX` | 2025-10-16 |  |
| [#2893](https://github.com/mRemoteNG/mRemoteNG/issues/2893) | Orphaned panel after closing several connections and reconnect again | `1.78.*`, `Priority - Low`, `UI/UX` | 2025-10-12 |  |
| [#2892](https://github.com/mRemoteNG/mRemoteNG/issues/2892) | Inconsistent behaviour of Options panel | `1.78.*`, `Panels`, `Priority - Low`, `UI/UX` | 2025-10-12 |  |
| [#2858](https://github.com/mRemoteNG/mRemoteNG/issues/2858) | Unhandled expection when closing panel | `1.78.*`, `Need 2 check`, `Panels`, `UI/UX` | 2025-10-07 |  |
| [#2785](https://github.com/mRemoteNG/mRemoteNG/issues/2785) | PuTTY saved sessions with CJK characters fail to display and connect | `.NET (dotNET)`, `1.78.*`, `Connections`, `i10n`, `Need 2 check`, `Priority - Medium`, `Putty`, `TechnicalDebt`, `UI/UX` | 2025-09-12 |  |
| [#2710](https://github.com/mRemoteNG/mRemoteNG/issues/2710) | Добавить не достающие страницы | `1.78.*`, `UI/UX`, `web site` | 2025-06-24 |  |
| [#2708](https://github.com/mRemoteNG/mRemoteNG/issues/2708) | Добавить флаги к переключателю языков | `1.78.*`, `UI/UX`, `web site` | 2025-06-24 |  |
| [#2685](https://github.com/mRemoteNG/mRemoteNG/issues/2685) | Startup splash screen not centered if display scale &gt; 100% | `1.78.*`, `Need 2 check`, `UI/UX` | 2025-04-03 |  |
| [#2655](https://github.com/mRemoteNG/mRemoteNG/issues/2655) | Server name is not provided in tab | `1.77.2`, `Connections`, `Need 2 check`, `Needs implementation`, `Priority - Low`, `UI/UX` | 2024-11-20 |  |
| [#2618](https://github.com/mRemoteNG/mRemoteNG/issues/2618) | Option from menu bar do not operate for connection in separate windows. | `1.77.3`, `Need 2 check`, `UI/UX`, `Windows` | 2024-07-18 |  |
| [#2608](https://github.com/mRemoteNG/mRemoteNG/issues/2608) | Feature request: Operate multiple remote desktop functions simultaneously and in parallel ! | `1.78.*`, `Need 2 check`, `Question`, `RDP`, `UI/UX`, `Windows` | 2024-06-26 |  |
| [#2575](https://github.com/mRemoteNG/mRemoteNG/issues/2575) | Feature Request - Add option for Connections and Config panel display at the same time when unpinned | `1.77.3`, `Need 2 check`, `UI/UX` | 2024-03-27 |  |
| [#2564](https://github.com/mRemoteNG/mRemoteNG/issues/2564) | incorrect window displaying at scaling 250% | `1.78.*`, `UI/UX` | 2024-02-08 |  |
| [#2526](https://github.com/mRemoteNG/mRemoteNG/issues/2526) | Screen remote very small | `1.78.*`, `Needs User Verification`, `UI/UX` | 2023-11-23 |  |
| [#2473](https://github.com/mRemoteNG/mRemoteNG/issues/2473) | Putty errors are indistinguishable from OK messages | `1.78.*`, `Putty`, `Question`, `UI/UX` | 2023-08-09 |  |
| [#2408](https://github.com/mRemoteNG/mRemoteNG/issues/2408) | Allow two connections tab to merged and so windows splitted horizontally | `1.77.3`, `Need 2 check`, `UI/UX` | 2023-03-24 |  |
| [#2407](https://github.com/mRemoteNG/mRemoteNG/issues/2407) | Allow undock connection tab to new mRemote program instance and allow merge connection tab back to first program instance | `1.77.3`, `Need 2 check`, `UI/UX` | 2023-03-24 |  |
| [#2322](https://github.com/mRemoteNG/mRemoteNG/issues/2322) | Feature Request: Vertical Connections Tabs List | `1.78.*`, `UI/UX` | 2023-01-10 |  |
| [#2313](https://github.com/mRemoteNG/mRemoteNG/issues/2313) | Feature: Option so select multiple Connections at the Overview menue | `1.78.*`, `Connections`, `Needs implementation`, `UI/UX` | 2022-12-06 |  |
| [#2311](https://github.com/mRemoteNG/mRemoteNG/issues/2311) | Feature Request: Status Icon next to each Connection at Overview | `1.78.*`, `Connections`, `UI/UX` | 2022-12-06 |  |
| [#2310](https://github.com/mRemoteNG/mRemoteNG/issues/2310) | Feature Request: Use "Name" as Hostname if no "Hostname" is entered | `1.78.*`, `Connections`, `Needs implementation`, `UI/UX` | 2022-12-06 |  |
| [#2309](https://github.com/mRemoteNG/mRemoteNG/issues/2309) | letter 'c' typed on doubleclick | `Need 2 check`, `RDP`, `UI/UX`, `Windows` | 2022-12-05 |  |
| [#2306](https://github.com/mRemoteNG/mRemoteNG/issues/2306) | Show "Description" in the host list | `1.78.*`, `Connections`, `In development`, `UI/UX` | 2022-11-27 |  |
| [#2305](https://github.com/mRemoteNG/mRemoteNG/issues/2305) | Dynamic windows path variables do not resolve in confCons.xml | `1.77.3`, `Connections`, `In development`, `UI/UX` | 2022-11-24 |  |
| [#2289](https://github.com/mRemoteNG/mRemoteNG/issues/2289) | ALT-TAB & Focus | `Need 2 check`, `UI/UX` | 2022-09-20 |  |
| [#2283](https://github.com/mRemoteNG/mRemoteNG/issues/2283) | Too many "music" | `Need 2 check`, `Sound`, `UI/UX` | 2022-09-07 |  |
| [#2222](https://github.com/mRemoteNG/mRemoteNG/issues/2222) | Mouse pointer does not scale when remote = 1080p, client = 4K (200% DPI) | `1.77.3`, `In progress`, `UI/UX` | 2022-05-09 |  |
| [#2134](https://github.com/mRemoteNG/mRemoteNG/issues/2134) | Implement multi-user environment | `Connections`, `Needs implementation`, `UI/UX` | 2022-01-18 |  |
| [#2120](https://github.com/mRemoteNG/mRemoteNG/issues/2120) | 2022.01.07-1.77.2-nb version does not look good on 4k monitor | `HighDPI`, `UI/UX` | 2022-01-13 |  |
| [#2068](https://github.com/mRemoteNG/mRemoteNG/issues/2068) | [FR] Filter input should hide all connections excluding those match input | `1.77.4`, `Filters`, `In development`, `Needs implementation`, `Search`, `UI/UX` | 2021-11-25 |  |
| [#2037](https://github.com/mRemoteNG/mRemoteNG/issues/2037) | Quick connect does not save history | `1.8 (Fenix)`, `Connections`, `UI/UX` | 2021-09-01 |  |
| [#2036](https://github.com/mRemoteNG/mRemoteNG/issues/2036) | REQUEST: Chromium (webView2) rendering with support for self signed certificates | `1.8 (Fenix)`, `UI/UX`, `WebView - Chromium` | 2021-08-30 |  |
| [#1994](https://github.com/mRemoteNG/mRemoteNG/issues/1994) | Extend menu for HTTP connection | `Need 2 check`, `UI/UX`, `Windows` | 2021-07-07 |  |
| [#1989](https://github.com/mRemoteNG/mRemoteNG/issues/1989) | Panel order blink after close panel | `Need 2 check`, `UI/UX` | 2021-07-06 |  |
| [#1988](https://github.com/mRemoteNG/mRemoteNG/issues/1988) | Auto close panel after close last tab not work, and not exist options in "Options" | `Need 2 check`, `UI/UX` | 2021-07-06 |  |
| [#1987](https://github.com/mRemoteNG/mRemoteNG/issues/1987) | Detachable panels are evil! | `Need 2 check`, `UI/UX` | 2021-07-06 |  |
| [#1982](https://github.com/mRemoteNG/mRemoteNG/issues/1982) | Connection, by default, must use "Panel"(Tab) from it's Folder | `1.8 (Fenix)`, `In development`, `Needs implementation`, `UI/UX` | 2021-06-29 |  |
| [#1950](https://github.com/mRemoteNG/mRemoteNG/issues/1950) | Unclear where to set Zoom level | `Need 2 check`, `UI/UX` | 2021-04-11 |  |
| [#1943](https://github.com/mRemoteNG/mRemoteNG/issues/1943) | When connecting TigerVNC through the mRemoteNG program, multiple people cannot connect at the same time. Please let me know what is the reason. | `Third party`, `UI/UX`, `VNC` | 2021-03-17 |  |
| [#1925](https://github.com/mRemoteNG/mRemoteNG/issues/1925) | Connections panel scrolling down when focussed | `1.77.2`, `Need 2 check`, `UI/UX` | 2021-01-28 |  |
| [#1659](https://github.com/mRemoteNG/mRemoteNG/issues/1659) | Feature Request: Tree branch to exclude from search | `1.8 (Fenix)`, `UI/UX` | 2019-12-19 |  |
| [#1538](https://github.com/mRemoteNG/mRemoteNG/issues/1538) | Use default property value if database value is null | `DBs`, `Need 2 check`, `UI/UX` | 2019-08-12 |  |
| [#1409](https://github.com/mRemoteNG/mRemoteNG/issues/1409) | Features request: Undock, Connection Pannel options, Quick search | `1.8 (Fenix)`, `UI/UX` | 2019-04-16 |  |
| [#1372](https://github.com/mRemoteNG/mRemoteNG/issues/1372) | Default Window Dimensions for Floating Tabs and Panels when Undocked | `UI/UX` | 2019-03-22 |  |
| [#1175](https://github.com/mRemoteNG/mRemoteNG/issues/1175) | Select a custom resolution and set it to smart size also for RDP session | `HighDPI`, `UI/UX` | 2018-11-02 |  |
| [#958](https://github.com/mRemoteNG/mRemoteNG/issues/958) | I cannot drag my connections to other connection panels | `1.8 (Fenix)`, `Needs implementation`, `UI/UX` | 2018-04-20 |  |
| [#954](https://github.com/mRemoteNG/mRemoteNG/issues/954) | connection with options / connect without credentials | `1.77.2`, `Need 2 check`, `UI/UX` | 2018-04-18 |  |
| [#872](https://github.com/mRemoteNG/mRemoteNG/issues/872) | Enhancement - Security at the Folder Level | `1.78.*`, `Active Directory`, `DBs`, `UI/UX` | 2018-01-25 |  |
| [#620](https://github.com/mRemoteNG/mRemoteNG/issues/620) | Mouse Cursor unusable after windows 10 creator update | `1.77.2`, `Need 2 check`, `UI/UX` | 2017-06-28 |  |
| [#399](https://github.com/mRemoteNG/mRemoteNG/issues/399) | Bug: Show connection page after an error if the pane it's autohided | `UI/UX` | 2017-02-10 |  |

### Documentation (3)

_Docs and website issues._

| Issue | Title | Labels | Opened | Status |
|---|---|---|---|---|
| [#2308](https://github.com/mRemoteNG/mRemoteNG/issues/2308) | Documentation update suggestion for External Tools winscp | `Documentation`, `Third party` | 2022-12-04 |  |
| [#2054](https://github.com/mRemoteNG/mRemoteNG/issues/2054) | Lost connection due to resizing the window | `Documentation`, `RDP` | 2021-09-28 |  |
| [#975](https://github.com/mRemoteNG/mRemoteNG/issues/975) | Document the Default Connection and Default Inheritance feature | `Documentation`, `Help Wanted` | 2018-05-08 |  |

### Needs verification (107)

_Reported but not yet reproduced — cheap wins if you can confirm or close them._

| Issue | Title | Labels | Opened | Status |
|---|---|---|---|---|
| [#3231](https://github.com/mRemoteNG/mRemoteNG/issues/3231) | mRemoteNG Portable External Tools %DISK-FLASH% | `Need 2 check` | 2026-03-22 |  |
| [#2881](https://github.com/mRemoteNG/mRemoteNG/issues/2881) | Does PuttyNG run on ARM64? - mRemoteNG requiring Visual C++ Redistributable x64 | `1.78.*`, `ARM`, `Need 2 check`, `Priority - Low`, `Putty`, `Third party` | 2025-10-08 |  |
| [#2843](https://github.com/mRemoteNG/mRemoteNG/issues/2843) | Data lost of changes are performed in two sessions | `1.78.*`, `Connections`, `In development`, `Need 2 check` | 2025-10-06 |  |
| [#2831](https://github.com/mRemoteNG/mRemoteNG/issues/2831) | Backup and restore the mremote NG application settings | `1.78.*`, `In development`, `In progress`, `Needs User Verification`, `Settings` | 2025-09-29 |  |
| [#2661](https://github.com/mRemoteNG/mRemoteNG/issues/2661) | [Feature request] WebAuthn redirection | `1.78.*`, `Need 2 check` | 2025-01-17 |  |
| [#2659](https://github.com/mRemoteNG/mRemoteNG/issues/2659) | Rdc12 breaks mRemoteNG on Win10 | `1.77.3`, `Need 2 check`, `RDP`, `Windows` | 2025-01-12 |  |
| [#2634](https://github.com/mRemoteNG/mRemoteNG/issues/2634) | VNC Domain User | `1.77.3`, `Active Directory`, `Need 2 check`, `VNC` | 2024-08-21 |  |
| [#2628](https://github.com/mRemoteNG/mRemoteNG/issues/2628) | Key Combinations and Caps Lock not passing through if using mRemoteNG through TeamViewer | `1.77.2`, `Need 2 check` | 2024-08-16 |  |
| [#2625](https://github.com/mRemoteNG/mRemoteNG/issues/2625) | Not able to RDP to servers if I disconnect and move to another machine and reconnect to the same session | `1.77.3`, `Citrix`, `Need 2 check`, `RDP` | 2024-08-05 |  |
| [#2606](https://github.com/mRemoteNG/mRemoteNG/issues/2606) | Bulk change inheritance | `1.77.3`, `Need 2 check` | 2024-06-19 |  |
| [#2588](https://github.com/mRemoteNG/mRemoteNG/issues/2588) | Accessing Ubuntu 24 using the "Remote Login" feature | `1.77.3`, `Linux`, `Need 2 check`, `RDP` | 2024-04-27 |  |
| [#2587](https://github.com/mRemoteNG/mRemoteNG/issues/2587) | Tab and app focus issues with nightly 1.77.3 1784 | `1.77.3`, `Need 2 check` | 2024-04-27 |  |
| [#2582](https://github.com/mRemoteNG/mRemoteNG/issues/2582) | Starting .exe from UNC path with config from UNC path not working. Issue is back. | `1.77.3`, `Connections`, `Need 2 check` | 2024-04-17 |  |
| [#2579](https://github.com/mRemoteNG/mRemoteNG/issues/2579) | mRemoteNG with SQLDB to share connections | `1.77.3`, `DBs`, `Need 2 check` | 2024-04-10 |  |
| [#2577](https://github.com/mRemoteNG/mRemoteNG/issues/2577) | Value cannot be null. (Parameter 'stream') | `1.77.3`, `Need 2 check`, `VNC` | 2024-04-03 |  |
| [#2570](https://github.com/mRemoteNG/mRemoteNG/issues/2570) | VNC connection with SSH Tunnel enabled doesn't work | `1.77.3`, `Need 2 check`, `SSH`, `VNC` | 2024-03-07 |  |
| [#2565](https://github.com/mRemoteNG/mRemoteNG/issues/2565) | RDP Load Balance and Redirection Server | `1.77.4`, `Need 2 check` | 2024-02-14 |  |
| [#2558](https://github.com/mRemoteNG/mRemoteNG/issues/2558) | Chrome Remote Desktop | `Need 2 check` | 2024-01-19 |  |
| [#2556](https://github.com/mRemoteNG/mRemoteNG/issues/2556) | Can't access to ESXi host from mRemoteNG | `1.77.3`, `Need 2 check` | 2024-01-16 |  |
| [#2547](https://github.com/mRemoteNG/mRemoteNG/issues/2547) | Add ESXi host in mRemoteNG | `1.77.3`, `Need 2 check` | 2024-01-04 |  |
| [#2527](https://github.com/mRemoteNG/mRemoteNG/issues/2527) | RDP keeps disconnecting from Google VM machine. | `Need 2 check`, `RDP` | 2023-11-25 |  |
| [#2522](https://github.com/mRemoteNG/mRemoteNG/issues/2522) | Right-Click on connection opens it when "Single Click on connection opens it" is selected in Connection Options | `1.77.3`, `Need 2 check`, `Options` | 2023-11-12 |  |
| [#2494](https://github.com/mRemoteNG/mRemoteNG/issues/2494) | Character set 'utf8mb3' is not supported by .Net Framework. | `Connections`, `DBs`, `Need 2 check` | 2023-09-28 |  |
| [#2493](https://github.com/mRemoteNG/mRemoteNG/issues/2493) | "Any connections that (panel) contains will be closed" Y/N question always closes all | `1.77.3`, `Need 2 check` | 2023-09-25 |  |
| [#2491](https://github.com/mRemoteNG/mRemoteNG/issues/2491) | VNC protocol unstable | `1.76.20`, `Need 2 check`, `Needs User Verification`, `Not Planned`, `VNC` | 2023-09-20 |  |
| [#2434](https://github.com/mRemoteNG/mRemoteNG/issues/2434) | RDP configuration does not allow for "prompt for credentials on client:i:1" to be configured within mRemoteNG | `1.77.2`, `Need 2 check`, `Needs User Verification`, `RDP` | 2023-05-12 |  |
| [#2427](https://github.com/mRemoteNG/mRemoteNG/issues/2427) | Gateway login does not pick up credentials if the main connection is picking up default credential from option. | `1.76.20`, `Need 2 check` | 2023-04-18 |  |
| [#2368](https://github.com/mRemoteNG/mRemoteNG/issues/2368) | /cons: switch gets ignored on latest 1.77.3 NB | `1.77.3`, `In progress`, `Need 2 check` | 2023-03-11 |  |
| [#2360](https://github.com/mRemoteNG/mRemoteNG/issues/2360) | Yubikey passthrough via RDP to a Windows Server? | `Need 2 check`, `RDP`, `Third party` | 2023-02-28 |  |
| [#2350](https://github.com/mRemoteNG/mRemoteNG/issues/2350) | Feature Request: Copy All to Clipboard | `Need 2 check` | 2023-02-13 |  |
| [#2329](https://github.com/mRemoteNG/mRemoteNG/issues/2329) | Azure RDP Connection with MFA enabled | `Need 2 check`, `RDP`, `Windows` | 2023-01-26 |  |
| [#2323](https://github.com/mRemoteNG/mRemoteNG/issues/2323) | Connection to MSSQL | `1.77.3`, `DBs`, `Fixed`, `Need 2 check`, `Needs User Verification` | 2023-01-13 |  |
| [#2321](https://github.com/mRemoteNG/mRemoteNG/issues/2321) | VNC connection error | `1.76.20`, `1.77.3`, `Cannot Reproduce`, `Need 2 check`, `Needs User Verification`, `VNC` | 2023-01-09 |  |
| [#2296](https://github.com/mRemoteNG/mRemoteNG/issues/2296) | ssh not staying connected after sleep mode | `Need 2 check`, `Putty`, `Third party` | 2022-10-08 |  |
| [#2284](https://github.com/mRemoteNG/mRemoteNG/issues/2284) | WSL in tab | `Need 2 check`, `WSL` | 2022-09-07 |  |
| [#2269](https://github.com/mRemoteNG/mRemoteNG/issues/2269) | Missing Keyboard indicator after closing RDP | `Need 2 check` | 2022-08-02 |  |
| [#2266](https://github.com/mRemoteNG/mRemoteNG/issues/2266) | mRemoteNG Uninstall issues - no uninstall.exe file (files remain in registry) | `Need 2 check` | 2022-07-28 |  |
| [#2262](https://github.com/mRemoteNG/mRemoteNG/issues/2262) | Cannot connect to VNC put using vnc-4_1_3-x86_win32_viewer it work | `Need 2 check` | 2022-07-12 |  |
| [#2257](https://github.com/mRemoteNG/mRemoteNG/issues/2257) | Given key was not present in Library when saving into MariaDb | `DBs`, `Need 2 check` | 2022-07-07 |  |
| [#2253](https://github.com/mRemoteNG/mRemoteNG/issues/2253) | Problem appeared after renaming the Display/Panel on the config-sidebar, as well after changing the display resolution after plug in/out from the docking station | `Need 2 check` | 2022-06-27 |  |
| [#2241](https://github.com/mRemoteNG/mRemoteNG/issues/2241) | Rendering Engine "Edge Chromium" not working | `Need 2 check` | 2022-06-03 |  |
| [#2233](https://github.com/mRemoteNG/mRemoteNG/issues/2233) | Freeze using Win 10 on Virtualbox | `Cannot Reproduce`, `Need 2 check` | 2022-05-26 |  |
| [#2232](https://github.com/mRemoteNG/mRemoteNG/issues/2232) | In some cases problems with Uppercase | `Need 2 check` | 2022-05-26 |  |
| [#2223](https://github.com/mRemoteNG/mRemoteNG/issues/2223) | Dragging down the full-screen blue title bar does not exit the full-screen mode. | `Need 2 check` | 2022-05-09 |  |
| [#2221](https://github.com/mRemoteNG/mRemoteNG/issues/2221) | Column 'DomainName' does not belong to table tblCons. | `Need 2 check` | 2022-05-07 |  |
| [#2219](https://github.com/mRemoteNG/mRemoteNG/issues/2219) | Import compatibility for Microsoft Remote Desktop Connection Manager v2.90 and above | `1.77.3`, `Connections`, `Import/Export`, `Need 2 check` | 2022-05-05 |  |
| [#2213](https://github.com/mRemoteNG/mRemoteNG/issues/2213) | Quick Connect - No Hostname Specified! | `1.77.2`, `Need 2 check` | 2022-04-28 |  |
| [#2210](https://github.com/mRemoteNG/mRemoteNG/issues/2210) | [Connections][Search bar]: Ctrl + F hotkey doesn't work if Quick Connect toolbar is active | `Need 2 check` | 2022-04-25 |  |
| [#2194](https://github.com/mRemoteNG/mRemoteNG/issues/2194) | nightly build crash..... | `1.77.2`, `Need 2 check` | 2022-04-05 |  |
| [#2182](https://github.com/mRemoteNG/mRemoteNG/issues/2182) | Cannot change domain when importing from AD (latest NB) | `1.77.3`, `Active Directory`, `Need 2 check` | 2022-03-07 |  |
| [#2180](https://github.com/mRemoteNG/mRemoteNG/issues/2180) | search without expand, escape to clear search, multiple search criteria, | `1.77.3`, `Need 2 check` | 2022-02-25 |  |
| [#2164](https://github.com/mRemoteNG/mRemoteNG/issues/2164) | TightVNC Support | `Need 2 check` | 2022-02-08 |  |
| [#2106](https://github.com/mRemoteNG/mRemoteNG/issues/2106) | Shortcut conflict with national keyboard mapping | `Need 2 check` | 2021-12-30 |  |
| [#2105](https://github.com/mRemoteNG/mRemoteNG/issues/2105) | White screen when connecting to VNC | `Need 2 check` | 2021-12-29 |  |
| [#2092](https://github.com/mRemoteNG/mRemoteNG/issues/2092) | The connections file could not be saved as ""! The provided key was not found in the dictionary. | `Connections`, `Need 2 check` | 2021-12-17 |  |
| [#2034](https://github.com/mRemoteNG/mRemoteNG/issues/2034) | How to: SSH Tunnel with SSO password --&gt; With SHARE SSH CONNECTION option | `Need 2 check` | 2021-08-30 |  |
| [#2030](https://github.com/mRemoteNG/mRemoteNG/issues/2030) | No remote audio recording | `Need 2 check` | 2021-08-27 |  |
| [#2017](https://github.com/mRemoteNG/mRemoteNG/issues/2017) | mRemoteNG closes sometimes when trying to establish RDP connection | `Need 2 check` | 2021-08-06 |  |
| [#2009](https://github.com/mRemoteNG/mRemoteNG/issues/2009) | Add the ability to use Bastion services to be able to RDP to cloud instances easily | `Azure`, `Needs User Verification` | 2021-07-31 |  |
| [#2004](https://github.com/mRemoteNG/mRemoteNG/issues/2004) | Bug: All connections stopped work after migrate XML to a new Windows System | `Connections`, `Need 2 check` | 2021-07-25 |  |
| [#1962](https://github.com/mRemoteNG/mRemoteNG/issues/1962) | mRemoteNG sometimes crashes on connection | `Need 2 check` | 2021-05-25 |  |
| [#1961](https://github.com/mRemoteNG/mRemoteNG/issues/1961) | Cannot start mRemoteNG due to an unhandled exception while loading the configuration | `Need 2 check` | 2021-05-18 |  |
| [#1955](https://github.com/mRemoteNG/mRemoteNG/issues/1955) | Unhandled exception when changing theme colors | `Need 2 check` | 2021-04-23 |  |
| [#1944](https://github.com/mRemoteNG/mRemoteNG/issues/1944) | Unhandled Exception - Value cannot be null. Parameter v1. | `Need 2 check` | 2021-03-18 |  |
| [#1939](https://github.com/mRemoteNG/mRemoteNG/issues/1939) | Feature Request: Allow setting name of connection as value of domain | `1.77.2`, `Need 2 check` | 2021-03-01 |  |
| [#1926](https://github.com/mRemoteNG/mRemoteNG/issues/1926) | External tools -&gt; escape colon | `1.77.2`, `Need 2 check` | 2021-01-28 |  |
| [#1921](https://github.com/mRemoteNG/mRemoteNG/issues/1921) | feature request - link connection panel to tab | `Need 2 check` | 2021-01-11 |  |
| [#1911](https://github.com/mRemoteNG/mRemoteNG/issues/1911) | Copy Paste between sessions does not work | `Need 2 check` | 2020-12-07 |  |
| [#1900](https://github.com/mRemoteNG/mRemoteNG/issues/1900) | CRASH by editing/coping data about connection | `Need 2 check` | 2020-11-16 |  |
| [#1897](https://github.com/mRemoteNG/mRemoteNG/issues/1897) | Reset layout not working | `Need 2 check` | 2020-11-13 |  |
| [#1888](https://github.com/mRemoteNG/mRemoteNG/issues/1888) | Connecting to RDP server fails with error 2056 | `Need 2 check` | 2020-10-27 |  |
| [#1877](https://github.com/mRemoteNG/mRemoteNG/issues/1877) | Screenshots can't be removed | `Need 2 check` | 2020-10-06 |  |
| [#1876](https://github.com/mRemoteNG/mRemoteNG/issues/1876) | Kitty sessions not updating console properly. | `Need 2 check`, `Third party` | 2020-10-06 |  |
| [#1866](https://github.com/mRemoteNG/mRemoteNG/issues/1866) | Using Create Bulk Connections fails when attempting to add password to container. | `Need 2 check` | 2020-09-23 |  |
| [#1846](https://github.com/mRemoteNG/mRemoteNG/issues/1846) | `AzureAD\` Prefix is ignored and you have to manually type it every time you want to login via RDP | `1.77.3`, `Need 2 check` | 2020-08-28 |  |
| [#1845](https://github.com/mRemoteNG/mRemoteNG/issues/1845) | Files transferred by SSH in Tools &gt; SSH File Transfer get added quote marks on remote | `Linux`, `Need 2 check` | 2020-08-26 |  |
| [#1808](https://github.com/mRemoteNG/mRemoteNG/issues/1808) | Program will not open. | `Need 2 check`, `Priority - High` | 2020-06-30 |  |
| [#1804](https://github.com/mRemoteNG/mRemoteNG/issues/1804) | Switching between tabbed and fullscreen connections sends titlebar off the screen | `Need 2 check`, `Needs implementation` | 2020-06-25 |  |
| [#1728](https://github.com/mRemoteNG/mRemoteNG/issues/1728) | RDP copy paste filter out numbers when is not EN-US keyboard layout | `Need 2 check`, `RDP` | 2020-04-02 |  |
| [#1696](https://github.com/mRemoteNG/mRemoteNG/issues/1696) | TightVNC closes gracefully using mRemoteNG | `Need 2 check`, `Third party`, `VNC` | 2020-02-20 |  |
| [#1688](https://github.com/mRemoteNG/mRemoteNG/issues/1688) | Integrated Browser / Menu / Bookmarks / URL Link | `Need 2 check` | 2020-02-06 |  |
| [#1685](https://github.com/mRemoteNG/mRemoteNG/issues/1685) | Bug - Can't import more than 1k hosts per Active Directory OU | `Need 2 check` | 2020-01-31 |  |
| [#1679](https://github.com/mRemoteNG/mRemoteNG/issues/1679) | No fullscreen option on VNC | `1.77.2`, `Need 2 check`, `Third party`, `VNC` | 2020-01-24 |  |
| [#1661](https://github.com/mRemoteNG/mRemoteNG/issues/1661) | RDP way too small, affects remote desktop when "Send To..." screen | `Need 2 check` | 2019-12-24 |  |
| [#1655](https://github.com/mRemoteNG/mRemoteNG/issues/1655) | Disabling "Saving connections on exit" doesn't work | `1.77.2`, `Need 2 check`, `Priority - Low` | 2019-12-16 |  |
| [#1653](https://github.com/mRemoteNG/mRemoteNG/issues/1653) | mRemoteNG crash when selecting 'more choices' in RDP | `Need 2 check`, `RDP` | 2019-12-12 |  |
| [#1637](https://github.com/mRemoteNG/mRemoteNG/issues/1637) | Upgrade to 1.77.1 clears stored default credentials | `Need 2 check` | 2019-11-18 |  |
| [#1626](https://github.com/mRemoteNG/mRemoteNG/issues/1626) | Execute mRemoteNG over a Webdav drive | `Need 2 check` | 2019-10-21 |  |
| [#1619](https://github.com/mRemoteNG/mRemoteNG/issues/1619) | Redirect &gt; Audio Capture Inheritance not being saved in 1.77 | `Need 2 check` | 2019-10-12 |  |
| [#1543](https://github.com/mRemoteNG/mRemoteNG/issues/1543) | UI issue - Config menu after inheritance menu for duplicated folder or connection | `1.77.2`, `Need 2 check` | 2019-08-21 |  |
| [#1495](https://github.com/mRemoteNG/mRemoteNG/issues/1495) | The discarded object can not be accessed. Object name: "ConnectionWindow" | `Need 2 check` | 2019-06-20 |  |
| [#1494](https://github.com/mRemoteNG/mRemoteNG/issues/1494) | VNC connections drop | `1.77.2`, `Need 2 check`, `VNC` | 2019-06-19 |  |
| [#1463](https://github.com/mRemoteNG/mRemoteNG/issues/1463) | New connection configuration fields are not inheritted (1.77) | `Need 2 check` | 2019-05-24 |  |
| [#1402](https://github.com/mRemoteNG/mRemoteNG/issues/1402) | Switching to full screen leaves black borders | `Need 2 check`, `RDP` | 2019-04-11 |  |
| [#1056](https://github.com/mRemoteNG/mRemoteNG/issues/1056) | Telnet connexion | `Need 2 check`, `Third party` | 2018-07-31 |  |
| [#1028](https://github.com/mRemoteNG/mRemoteNG/issues/1028) | Cannot use saved credentials | `Need 2 check`, `RDP` | 2018-07-19 |  |
| [#1006](https://github.com/mRemoteNG/mRemoteNG/issues/1006) | [Connections]: folders don't automatically expand when searching or filtering | `Connections`, `Need 2 check` | 2018-06-22 |  |
| [#864](https://github.com/mRemoteNG/mRemoteNG/issues/864) | Unable to open more than ±15-20 simultaneous connections to Windows 2012 R2 and higher | `Need 2 check` | 2018-01-11 |  |
| [#859](https://github.com/mRemoteNG/mRemoteNG/issues/859) | All RDP connections dropped when new connection can't establish | `Need 2 check` | 2018-01-08 |  |
| [#839](https://github.com/mRemoteNG/mRemoteNG/issues/839) | Import from file RDCMan.rdg error while importing the file | `Connections`, `Import/Export`, `Need 2 check` | 2017-12-12 |  |
| [#824](https://github.com/mRemoteNG/mRemoteNG/issues/824) | Still getting error 3334 on 18th 2012 RDP Connection | `Need 2 check`, `RDP` | 2017-11-29 |  |
| [#811](https://github.com/mRemoteNG/mRemoteNG/issues/811) | Error during startup System.Xml.XmlException | `1.77.2`, `Need 2 check` | 2017-11-20 |  |
| [#662](https://github.com/mRemoteNG/mRemoteNG/issues/662) | scrollbars added to RDP window after minimize/restore of mRemoteNG on v1.75 | `1.77.2`, `Need 2 check` | 2017-07-27 |  |
| [#633](https://github.com/mRemoteNG/mRemoteNG/issues/633) | Two-Finger scroll defaults to config panel instead of remote desktop | `Need 2 check` | 2017-07-06 |  |
| [#521](https://github.com/mRemoteNG/mRemoteNG/issues/521) | Unable to input into "Connect:" field for PuTTY | `Need 2 check` | 2017-04-21 |  |
| [#516](https://github.com/mRemoteNG/mRemoteNG/issues/516) | Gecko engine doesn't allow some windows to open | `Need 2 check` | 2017-04-18 |  |
| [#370](https://github.com/mRemoteNG/mRemoteNG/issues/370) | Saved connections not respecting specified logon credentials | `Need 2 check` | 2017-01-25 |  |

### Other (100)

_Labelled, but outside the categories above._

| Issue | Title | Labels | Opened | Status |
|---|---|---|---|---|
| [#3219](https://github.com/mRemoteNG/mRemoteNG/issues/3219) | VncSharpCore | `1.78.*`, `Third party`, `VNC` | 2026-03-17 |  |
| [#3183](https://github.com/mRemoteNG/mRemoteNG/issues/3183) | mRemoteNG Download links broken | `web site` | 2026-02-26 |  |
| [#3167](https://github.com/mRemoteNG/mRemoteNG/issues/3167) | mRemoteNG 1.78.2 NB 3405 (SC) requires .NET 9.0 | `.NET (dotNET)`, `1.78.*`, `Nightly Build` | 2026-02-23 |  |
| [#3103](https://github.com/mRemoteNG/mRemoteNG/issues/3103) | web site mremoteng.COM redirects to malware and other unwanted URLs | `Third party` | 2026-02-05 |  |
| [#3092](https://github.com/mRemoteNG/mRemoteNG/issues/3092) | 1Password integration doesn't fetch username and password for RDP connection | `1.78.*`, `Add-ons`, `Credentials`, `Help Wanted`, `Integrations` | 2026-01-24 |  |
| [#3080](https://github.com/mRemoteNG/mRemoteNG/issues/3080) | LDAP query injection may lead to data exposure - mRemoteNG | `AI`, `critical` | 2026-01-13 |  |
| [#3027](https://github.com/mRemoteNG/mRemoteNG/issues/3027) | Can't use MariaDB Database | `1.78.*`, `DBs` | 2025-11-27 |  |
| [#2972](https://github.com/mRemoteNG/mRemoteNG/issues/2972) | 1password integration doesn't work from credentials inside options | `Integrations` | 2025-10-24 |  |
| [#2810](https://github.com/mRemoteNG/mRemoteNG/issues/2810) | Weird graphical issues between versions.. | `.NET (dotNET)`, `1.78.*`, `Cannot Reproduce`, `Nightly Build`, `RDP`, `TechnicalDebt` | 2025-09-18 |  |
| [#2721](https://github.com/mRemoteNG/mRemoteNG/issues/2721) | Site Download Page Error | `Downloads`, `In development`, `web site` | 2025-07-19 |  |
| [#2678](https://github.com/mRemoteNG/mRemoteNG/issues/2678) | Is mRemoteNG sometimes stable (usable) again? | `.NET (dotNET)`, `1.78.*` | 2025-03-25 |  |
| [#2666](https://github.com/mRemoteNG/mRemoteNG/issues/2666) | NB with SAVING feature | `In development`, `In progress`, `Needs implementation`, `Nightly Build` | 2025-01-28 |  |
| [#2648](https://github.com/mRemoteNG/mRemoteNG/issues/2648) | [Featurerequest] call Powershell and let response modify parameter | `1.77.4`, `Connections`, `In development`, `Profiles` | 2024-10-28 |  |
| [#2614](https://github.com/mRemoteNG/mRemoteNG/issues/2614) | Centrally stored credentials not accepted for Linux (SSH) | `1.78.*`, `Connections`, `DBs`, `Linux` | 2024-07-13 |  |
| [#2612](https://github.com/mRemoteNG/mRemoteNG/issues/2612) | Possibility of providing separate credentials for logging in to Hyper-v virtual machine | `1.77.4`, `Profiles` | 2024-07-05 |  |
| [#2610](https://github.com/mRemoteNG/mRemoteNG/issues/2610) | New release at some point? | `1.77.3`, `RTFM` | 2024-07-03 |  |
| [#2578](https://github.com/mRemoteNG/mRemoteNG/issues/2578) | Inconsistent Behaviour of Single vs Multiple Sessions when using mRemote + RDP Gateway | `Question`, `RDP`, `Third party` | 2024-04-08 |  |
| [#2563](https://github.com/mRemoteNG/mRemoteNG/issues/2563) | Cannot select default connection file on startup | `1.77.3`, `Duplicate`, `In progress` | 2024-02-07 |  |
| [#2557](https://github.com/mRemoteNG/mRemoteNG/issues/2557) | XmingPortablePuttySessions.Watcher.StartWatching() failed: 'sessions' does not exist. | `1.76.20`, `Startup` | 2024-01-17 |  |
| [#2554](https://github.com/mRemoteNG/mRemoteNG/issues/2554) | Feature Request: Shared username/password | `1.77.4`, `Connections`, `In development` | 2024-01-15 |  |
| [#2498](https://github.com/mRemoteNG/mRemoteNG/issues/2498) | Set SQL Server connection for all users by default | `1.78.*`, `DBs`, `Profiles` | 2023-10-05 |  |
| [#2492](https://github.com/mRemoteNG/mRemoteNG/issues/2492) | SSH tunneling option is not shown in v1.76.20 | `1.76.20` | 2023-09-22 |  |
| [#2487](https://github.com/mRemoteNG/mRemoteNG/issues/2487) | [Feature Request] Allow import from SecureCRT | `Import/Export`, `Third party` | 2023-09-05 |  |
| [#2480](https://github.com/mRemoteNG/mRemoteNG/issues/2480) | How to import sessions from SecureCRT or any other application? | `Connections`, `Import/Export`, `Needs implementation`, `Settings`, `Third party` | 2023-08-28 |  |
| [#2474](https://github.com/mRemoteNG/mRemoteNG/issues/2474) | Request: Promote a new version to stable | `1.77.3`, `In progress`, `Project Infrastructure` | 2023-08-11 |  |
| [#2471](https://github.com/mRemoteNG/mRemoteNG/issues/2471) | Migration DB from Stable (v1.76.20) to Nightly (v1.77.3.1784-NB) | `1.77.3`, `DBs`, `Settings` | 2023-08-03 |  |
| [#2463](https://github.com/mRemoteNG/mRemoteNG/issues/2463) | Import from Active Directory: Unable to import from different domain than where the application is installed | `1.77.3`, `Active Directory`, `Connections`, `Settings` | 2023-07-19 |  |
| [#2455](https://github.com/mRemoteNG/mRemoteNG/issues/2455) | Opening Tab is not "Connexions" | `1.77.3`, `In development`, `Settings` | 2023-06-28 |  |
| [#2454](https://github.com/mRemoteNG/mRemoteNG/issues/2454) | Dependency Management - Updating the included dependency | `1.77.3`, `Log4Net`, `Project Infrastructure`, `Putty`, `Third party` | 2023-06-24 |  |
| [#2453](https://github.com/mRemoteNG/mRemoteNG/issues/2453) | Cannot migrate to SQL, default behavior now loads from SQL | `1.77.3`, `DBs`, `Settings` | 2023-06-23 |  |
| [#2445](https://github.com/mRemoteNG/mRemoteNG/issues/2445) | Import of VNC connection files | `1.77.4`, `Import/Export`, `Needs implementation` | 2023-06-14 |  |
| [#2429](https://github.com/mRemoteNG/mRemoteNG/issues/2429) | MySQL database connection needs a table | `1.77.3`, `DBs`, `In development` | 2023-04-25 |  |
| [#2425](https://github.com/mRemoteNG/mRemoteNG/issues/2425) | Unable to set up SQL | `1.78.*`, `DBs`, `In progress` | 2023-04-12 |  |
| [#2417](https://github.com/mRemoteNG/mRemoteNG/issues/2417) | RustDesk integration feature | `1.77.4`, `Connections`, `Needs implementation` | 2023-04-01 |  |
| [#2389](https://github.com/mRemoteNG/mRemoteNG/issues/2389) | Split the solution in multiple projects | `1.78.*`, `Needs implementation`, `Project Infrastructure` | 2023-03-17 |  |
| [#2333](https://github.com/mRemoteNG/mRemoteNG/issues/2333) | Feature Request: Automatic XML Import from URI | `1.78.*`, `Connections`, `In development` | 2023-01-31 |  |
| [#2332](https://github.com/mRemoteNG/mRemoteNG/issues/2332) | The big search rewamp | `1.78.*`, `Filters`, `Search` | 2023-01-30 |  |
| [#2325](https://github.com/mRemoteNG/mRemoteNG/issues/2325) | FEATURE REQUEST - 2FA to open application | `1.78.*`, `Needs implementation`, `Profiles` | 2023-01-16 |  |
| [#2320](https://github.com/mRemoteNG/mRemoteNG/issues/2320) | add default password for each protocol - particularly windows and putty | `1.78.*`, `Connections` | 2023-01-04 |  |
| [#2304](https://github.com/mRemoteNG/mRemoteNG/issues/2304) | centralized login/pwd, usable on a set of connections | `1.78.*` | 2022-11-22 |  |
| [#2290](https://github.com/mRemoteNG/mRemoteNG/issues/2290) | mysql db problem - new item creation impossible | `1.77.3`, `DBs`, `In development` | 2022-09-21 |  |
| [#2282](https://github.com/mRemoteNG/mRemoteNG/issues/2282) | Settings in Russian tranlation not saved | `1.77.4`, `Translations` | 2022-09-07 |  |
| [#2258](https://github.com/mRemoteNG/mRemoteNG/issues/2258) | Restricted admin support \| mRemoteNG v1.76.20.24615 | `1.77.3`, `RDP` | 2022-07-07 |  |
| [#2252](https://github.com/mRemoteNG/mRemoteNG/issues/2252) | MSSQL Limitation | `DBs` | 2022-06-22 |  |
| [#2251](https://github.com/mRemoteNG/mRemoteNG/issues/2251) | [Question]How to have mRemoteNG use KiTTY instead of PuTTY ? | `SSH` | 2022-06-22 |  |
| [#2250](https://github.com/mRemoteNG/mRemoteNG/issues/2250) | Import Microsoft Remote Desktop Client backups | `Connections`, `Import/Export`, `Needs implementation` | 2022-06-22 |  |
| [#2242](https://github.com/mRemoteNG/mRemoteNG/issues/2242) | [Suggestion] LiteDB option for new beta integrations | `1.8 (Fenix)`, `DBs` | 2022-06-06 |  |
| [#2228](https://github.com/mRemoteNG/mRemoteNG/issues/2228) | New nightly build | `1.77.3`, `Priority - Low` | 2022-05-19 |  |
| [#2206](https://github.com/mRemoteNG/mRemoteNG/issues/2206) | HTTP problem opening Intel management web interface (MEB) pop-up | `1.77.3`, `WebView - Chromium` | 2022-04-20 |  |
| [#2201](https://github.com/mRemoteNG/mRemoteNG/issues/2201) | Process to make local drives available by default while using mRemoteng utility to remote desktop into Win Servers | `1.77.3`, `Connections`, `In development` | 2022-04-14 |  |
| [#2197](https://github.com/mRemoteNG/mRemoteNG/issues/2197) | Frage betreffend Passwortlänge | `Question` | 2022-04-11 |  |
| [#2191](https://github.com/mRemoteNG/mRemoteNG/issues/2191) | Idea: opennig http/https in external OS browser | `HTTP/HTTPS`, `Needs implementation`, `Priority - Low` | 2022-03-31 |  |
| [#2173](https://github.com/mRemoteNG/mRemoteNG/issues/2173) | Disable "port scan" option in application | `1.77.3`, `In development`, `Needs implementation` | 2022-02-16 |  |
| [#2172](https://github.com/mRemoteNG/mRemoteNG/issues/2172) | Quick Connect Toolbar not working anymore (v1.77.2) | `1.77.3`, `Verified` | 2022-02-15 |  |
| [#2140](https://github.com/mRemoteNG/mRemoteNG/issues/2140) | Do not update remote clipboard before pasting or until a copy is made | `Clipboard`, `Needs implementation`, `Priority - Medium` | 2022-01-24 |  |
| [#2078](https://github.com/mRemoteNG/mRemoteNG/issues/2078) | REQUEST: Import .moba files, from MobaXTerm | `Connections`, `Import/Export`, `Priority - Low`, `Third party` | 2021-12-11 |  |
| [#2077](https://github.com/mRemoteNG/mRemoteNG/issues/2077) | Merge and auto-export configfile | `Priority - Low` | 2021-12-08 |  |
| [#2070](https://github.com/mRemoteNG/mRemoteNG/issues/2070) | REQUEST: Additional Command Line Switches | `Priority - Low` | 2021-11-29 |  |
| [#2051](https://github.com/mRemoteNG/mRemoteNG/issues/2051) | REQUEST: Support for connection presets | `1.8 (Fenix)`, `In development` | 2021-09-23 |  |
| [#2035](https://github.com/mRemoteNG/mRemoteNG/issues/2035) | REQUEST: VNC "View only" toggle | `1.8 (Fenix)`, `Needs implementation`, `Priority - Low` | 2021-08-30 |  |
| [#2025](https://github.com/mRemoteNG/mRemoteNG/issues/2025) | Retire VncSHarp/VncSharpNG | `Request For Comment`, `TechnicalDebt` | 2021-08-15 |  |
| [#1985](https://github.com/mRemoteNG/mRemoteNG/issues/1985) | Change SQL Server support of mRemoteNG | `1.8 (Fenix)`, `DBs`, `In development`, `Needs implementation` | 2021-06-29 |  |
| [#1952](https://github.com/mRemoteNG/mRemoteNG/issues/1952) | Mikrotik WInbox Integration BUG | `1.77.2`, `In progress` | 2021-04-17 |  |
| [#1951](https://github.com/mRemoteNG/mRemoteNG/issues/1951) | Feature Request: Using local devices in Remote session | `Help Wanted` | 2021-04-16 |  |
| [#1915](https://github.com/mRemoteNG/mRemoteNG/issues/1915) | Saving Host and IP separately | `1.8 (Fenix)` | 2020-12-16 |  |
| [#1905](https://github.com/mRemoteNG/mRemoteNG/issues/1905) | Newer versions of RFB not supported in vncsharp | `TechnicalDebt`, `VNC` | 2020-11-22 |  |
| [#1871](https://github.com/mRemoteNG/mRemoteNG/issues/1871) | Confcons location on ftp like keepass | `1.77.2`, `In development` | 2020-10-02 |  |
| [#1862](https://github.com/mRemoteNG/mRemoteNG/issues/1862) | Will Not Remain Running and Quits Right After Start | `Support Request` | 2020-09-14 |  |
| [#1856](https://github.com/mRemoteNG/mRemoteNG/issues/1856) | Command snippet/library | `1.77.2`, `In development`, `Priority - Medium` | 2020-09-07 |  |
| [#1837](https://github.com/mRemoteNG/mRemoteNG/issues/1837) | Can't find Use VM ID property? | `Duplicate` | 2020-08-19 |  |
| [#1833](https://github.com/mRemoteNG/mRemoteNG/issues/1833) | MRemoteNG & external tool SCCM | `Priority - Low` | 2020-08-12 |  |
| [#1831](https://github.com/mRemoteNG/mRemoteNG/issues/1831) | Feature Request: Move window with keyboard shortcuts | `1.8 (Fenix)`, `Priority - Low` | 2020-08-12 |  |
| [#1814](https://github.com/mRemoteNG/mRemoteNG/issues/1814) | Credentials Profiles | `1.8 (Fenix)` | 2020-07-10 |  |
| [#1791](https://github.com/mRemoteNG/mRemoteNG/issues/1791) | Remove gh-pages branch | `In progress`, `Project Infrastructure` | 2020-06-13 |  |
| [#1742](https://github.com/mRemoteNG/mRemoteNG/issues/1742) | Dead keys (',",~,etc) not working for VNC connection to vSphere VM, unless held continuously | `VNC` | 2020-04-29 |  |
| [#1692](https://github.com/mRemoteNG/mRemoteNG/issues/1692) | mRemoteNG from command line | `1.77.2`, `Needs implementation` | 2020-02-12 |  |
| [#1684](https://github.com/mRemoteNG/mRemoteNG/issues/1684) | Panel should close when last containing connection/tab closes | `Duplicate` | 2020-01-31 |  |
| [#1549](https://github.com/mRemoteNG/mRemoteNG/issues/1549) | mremote does not responds to keyboard inputs | `Cannot Reproduce` | 2019-08-26 |  |
| [#1547](https://github.com/mRemoteNG/mRemoteNG/issues/1547) | Clipboard issues on VNC X11 connections; Cannot disable syncing on VNC. | `VNC` | 2019-08-24 |  |
| [#1424](https://github.com/mRemoteNG/mRemoteNG/issues/1424) | SQL multiuser support | `1.8 (Fenix)`, `Needs implementation` | 2019-04-29 |  |
| [#1415](https://github.com/mRemoteNG/mRemoteNG/issues/1415) | REST interface for mremoteng for better teams connections syncing | `Not Planned` | 2019-04-18 |  |
| [#1356](https://github.com/mRemoteNG/mRemoteNG/issues/1356) | tabs name trimming on highDPI screen | `HighDPI` | 2019-03-16 |  |
| [#1327](https://github.com/mRemoteNG/mRemoteNG/issues/1327) | keyboard vnc issue | `VNC` | 2019-02-23 |  |
| [#1321](https://github.com/mRemoteNG/mRemoteNG/issues/1321) | Simplify process to add new connection properties | `Priority - Medium`, `TechnicalDebt` | 2019-02-18 |  |
| [#1308](https://github.com/mRemoteNG/mRemoteNG/issues/1308) | Multiple flaws with floating connection windows | `Help Wanted` | 2019-02-13 |  |
| [#1196](https://github.com/mRemoteNG/mRemoteNG/issues/1196) | Can't copy/paste into VNC connection | `VNC` | 2018-11-27 |  |
| [#1109](https://github.com/mRemoteNG/mRemoteNG/issues/1109) | Status of hosts | `Not Planned` | 2018-09-24 |  |
| [#1099](https://github.com/mRemoteNG/mRemoteNG/issues/1099) | Saving password in internal browser | `Citrix`, `Connections` | 2018-09-05 |  |
| [#1059](https://github.com/mRemoteNG/mRemoteNG/issues/1059) | Windows 10 High Contrast (green on black) theme clashes with default mRemoteNG theme, for connection tabs | `Connections`, `Theming` | 2018-08-01 |  |
| [#1033](https://github.com/mRemoteNG/mRemoteNG/issues/1033) | Integrate with Passwordsafe | `1.8 (Fenix)`, `Connections`, `Needs implementation` | 2018-07-23 |  |
| [#906](https://github.com/mRemoteNG/mRemoteNG/issues/906) | The most useful feature | `1.8 (Fenix)`, `Connections`, `Needs implementation` | 2018-03-03 |  |
| [#884](https://github.com/mRemoteNG/mRemoteNG/issues/884) | Add synchronization of connection's (TEAM WORK) | `1.77.2`, `Connections`, `Import/Export`, `In development` | 2018-02-09 |  |
| [#678](https://github.com/mRemoteNG/mRemoteNG/issues/678) | VNC connections close on their own after unspecified amount of time | `VNC` | 2017-08-12 |  |
| [#675](https://github.com/mRemoteNG/mRemoteNG/issues/675) | Password encryption based on a certificate | `1.8 (Fenix)`, `DBs`, `Needs implementation` | 2017-08-07 |  |
| [#636](https://github.com/mRemoteNG/mRemoteNG/issues/636) | in VNC when the client is not avalilable apps freezes for 3 minutes. | `VNC` | 2017-07-11 |  |
| [#579](https://github.com/mRemoteNG/mRemoteNG/issues/579) | Send Ctrl+Alt+Delete to VNC | `VNC` | 2017-06-10 |  |
| [#306](https://github.com/mRemoteNG/mRemoteNG/issues/306) | CII - resolve no_leaked_credentials | `Priority - High`, `Project Infrastructure` | 2016-12-09 |  |
| [#300](https://github.com/mRemoteNG/mRemoteNG/issues/300) | Auto submit new releases to reddit and twitter community | `Project Infrastructure`, `update` | 2016-12-06 |  |
| [#287](https://github.com/mRemoteNG/mRemoteNG/issues/287) | Core Infrastructure Initiative (CII) Best Practices | `In progress`, `Project Infrastructure` | 2016-12-01 |  |
| [#242](https://github.com/mRemoteNG/mRemoteNG/issues/242) | Fix RootNodeInfo object graph | `1.78.*`, `Connections`, `In progress`, `Needs implementation` | 2016-11-10 |  |

### Untriaged (no labels) (12)

_No labels at all — need a first pass._

| Issue | Title | Labels | Opened | Status |
|---|---|---|---|---|
| [#3394](https://github.com/mRemoteNG/mRemoteNG/issues/3394) | connections & config panel always hide when focus disappears | — | 2026-07-22 |  |
| [#3385](https://github.com/mRemoteNG/mRemoteNG/issues/3385) | VNC connection fails with "Value cannot be null. (Parameter 'stream')" on 1.78.2 Nightly Build 3405 | — | 2026-07-16 |  |
| [#3377](https://github.com/mRemoteNG/mRemoteNG/issues/3377) | Login dialog not activated after starting mRemoteNG | — | 2026-07-10 |  |
| [#3373](https://github.com/mRemoteNG/mRemoteNG/issues/3373) | Auto-resize is not working | — | 2026-07-06 |  |
| [#3178](https://github.com/mRemoteNG/mRemoteNG/issues/3178) | Reconnect to previously opened sessions on startup isn't working in NB 3405 | — | 2026-02-25 |  |
| [#2876](https://github.com/mRemoteNG/mRemoteNG/issues/2876) | Inconsistent icons on active connection right mouse menu | — | 2025-10-08 |  |
| [#2833](https://github.com/mRemoteNG/mRemoteNG/issues/2833) | Dependency Dashboard | — | 2025-09-29 |  |
| [#2239](https://github.com/mRemoteNG/mRemoteNG/issues/2239) | Web connections need tooling to paste in username and password | — | 2022-06-03 |  |
| [#2237](https://github.com/mRemoteNG/mRemoteNG/issues/2237) | SSH connections dropping if I switch tabs | — | 2022-06-03 |  |
| [#2217](https://github.com/mRemoteNG/mRemoteNG/issues/2217) | Remote Desktop Gateway Password | — | 2022-05-03 |  |
| [#1197](https://github.com/mRemoteNG/mRemoteNG/issues/1197) | Scaling the size of text, applications and other elements 125% on a remote machine. | — | 2018-11-27 |  |
| [#1183](https://github.com/mRemoteNG/mRemoteNG/issues/1183) | On startup any selected item is disregarded shortly after | — | 2018-11-08 |  |

