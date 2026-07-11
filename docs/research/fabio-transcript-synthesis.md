# Fabio Transcript Synthesis

Sources:

```text
transcription.txt      Chart Fanatics live/model walkthrough
trascription_1.txt    Words of Wisdom New York live session
```

## Conclusion

Fabio does not describe one static strategy. He separates market state, location, aggression, and management. The project must not turn discretionary language into a hidden entry rule.

```text
Model 1: New York continuation
imbalance -> low-volume-node / pullback -> aggressive participation -> continuation.

Model 2: London mean reversion
balance/compression -> first drive outside -> return inside -> opposite aggression -> balance bulk/POC.
```

The current implementation studies Model 2 advanced variant only. It has no operational entries.

## Evidence Map

```text
Market state before prediction:
- Market is usually balanced; imbalance is the condition for continuation.
- London indices more often mean revert; New York is preferred for trend continuation.
- Do not use a model outside the session/regime for which it was observed.

Location:
- Simple version: prior-day / prior-balance profile.
- Advanced version: identify compressed candles/dealing range and plot its profile.
- POC/bulk is the likely reversion target, not necessarily the opposite edge.
- Low-volume nodes refine continuation/reaction locations.

Order flow:
- Big trades are confirmation, not a signal by themselves.
- Compare aggression with price result: aggression without follow-through indicates absorption.
- Passive protection at a boundary matters more than a raw volume number.
- CVD is useful as a pressure proxy; mixed CVD in consolidation is a reason to stand aside.

Execution:
- Reversion waits for the first drive outside and the return inside; do not fade the first move blindly.
- Continuation waits for acceptance/break-and-test, not the initial breakout impulse.
- A stop belongs near the proven protection/failure level; Fabio often exits before obvious stop liquidity.
- Fast break-even, partial exits and dynamic trailing are discretionary management, not yet an automated contract.
```

## Study Contract

Each `DynamicCompression` must be `READY` before any study event. The study then records:

```text
HIGH/LOW TEST
A completed bar reaches the corresponding compression boundary.

ABSORPTION / REVERSION
Price breaches a boundary; aggressive volume at that boundary is large; the bar returns inside; an opposite big trade confirms within two bars.

BREAKOUT ACCEPTANCE
The existing lifecycle confirms two closes beyond the range; study requires directional big-trade volume and directional CVD across those acceptance bars.
```

Generated candidates are explicitly non-operational:

```text
REVERSION_LONG / REVERSION_SHORT
- stop candidate: two ticks inside the failed compression extreme
- target candidate: compression POC

BREAKOUT_LONG / BREAKOUT_SHORT
- stop candidate: two ticks inside the broken boundary
- target: intentionally undefined until a separate breakout/retest model is specified
```

## Excluded For Now

```text
- order placement and PnL
- scale-in / partial exits / trailing management
- New York continuation model
- breakout retest entry after acceptance
- account-level daily loss rules
- a claim that an algorithm knows the compression Fabio would draw
```

The next decision is evidence-based: inspect the study candidates on chart/logs, compare them with the two videos, then choose one isolated entry contract for shadow testing before any operational trade is restored.
