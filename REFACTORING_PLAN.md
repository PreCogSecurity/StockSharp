# Refactoring Plan for Large Files

This document outlines the technical debt in large files and provides a roadmap for incremental refactoring.

## Identified Large Files

### 1. TraderHelper.cs (3,877 lines)
**Location:** `Algo/TraderHelper.cs`

**Current Structure:**
- 2 small classes: `CollectionSecurityProvider`, `ConnectorSecurityProvider`
- 1 massive static class `TraderHelper` with 150+ extension methods

**Functional Domains (should be split):**

| Domain | Methods | Suggested New Class |
|--------|---------|---------------------|
| Price/Quote Manipulation | `GetFilteredQuotes`, `GetMarketPrice`, `GetCurrentPrice`, `ApplyOffset`, `ShrinkPrice` | `PriceHelper` |
| Position/PnL Calculations | `GetPosition`, `GetPnL`, `GetPrice`, `GetOrderVolume` | `PositionHelper` |
| Market Depth Operations | `Sparse`, `Join`, `Group`, `UnGroup`, `EmulateTrades`, `GetDelta`, `AddDelta` | `MarketDepthHelper` |
| Order/Trade Management | `IsCanceled`, `IsMatched`, `ReRegisterClone`, `GetTrades`, `GetMatchedVolume`, `GetAveragePrice`, `GetTheoreticalTrades` | `OrderHelper` |
| Exchange/Trading Time | `IsTradeTime`, `IsTradeDate`, `GetTPlusNDate`, `GetExpiryDates` | `ExchangeHelper` |
| Security Filtering/Lookup | `Filter`, `FilterSecurities`, `GetSecurityCriteria`, `SyncLookupSecurities`, `LookupById`, `LookupByCode`, `LookupAll` | `SecurityHelper` |
| Currency Conversion | `Convert`, `ToMicexCurrencyName`, `FromMicexCurrencyName` | `CurrencyHelper` |
| Portfolio/Position Management | `GetFreeMoney`, `ToCashPosition`, `ToPositionManager`, `ApplyChanges` | `PortfolioHelper` |
| Logging Helpers | `AddOrderInfoLog`, `AddOrderErrorLog` | `LoggingHelper` |
| Message Adapter Conversions | `To`, `ToChannel`, `ToDecimal` | `MessageAdapterHelper` |
| ISO 10962 Classification | `GetIso10962`, `FromIso10962` | `SecurityClassificationHelper` |
| Level1 to Tick/OrderBook | `ToTicks`, `IsContainsTick`, `ToTick`, `ToOrderBooks`, `IsContainsQuotes` | `Level1Converter` |
| Path/Filename Helpers | `SecurityIdToFolderName`, `FolderNameToSecurityId`, `CandleArgToFolderName` | `PathHelper` |
| Latency Calculations | `GetLatency` | `LatencyHelper` |

**Refactoring Approach:**
1. Create new focused classes in `Algo/Helpers/` folder
2. Move extension methods to appropriate helper classes
3. Keep `TraderHelper.cs` as a facade that re-exports all methods (backward compatibility)
4. Mark old methods as `[Obsolete]` with migration guidance
5. Update all call sites incrementally

### 2. ExchangeBoard.cs (3,257 lines)
**Location:** `BusinessEntities/ExchangeBoard.cs`

**Current Structure:**
- Single class `ExchangeBoard` with:
  - ~100 static predefined exchange boards (MOEX, CME, NYSE, etc.)
  - Instance properties: `Code`, `ExpiryTime`, `IsSupportAtomicReRegister`, `IsSupportMarketOrders`, `WorkingTime`
  - Static factory methods: `GetBoard`, `GetOrCreateBoard`, `SaveBoard`
  - Serialization methods: `Load`, `Save`
  - Helper properties: `IsMicex`, `IsUxStock`

**Issues:**
- Violates Single Responsibility Principle (data + factory + registry + serialization)
- Hundreds of static fields for predefined boards
- Hard to maintain and extend
- Mixes configuration data with business logic

**Refactoring Approach:**
1. **Extract Exchange Board Registry** → `ExchangeBoardRegistry` class
   - Move all static predefined boards to a separate registry
   - Use a configuration file (JSON/XML) for board definitions
   - Implement `IExchangeBoardRegistry` interface

2. **Extract Working Time Logic** → `WorkingTime` class (already exists, enhance it)
   - Move `IsTradeTime`, `IsTradeDate`, `GetTPlusNDate` to `WorkingTime` or `ExchangeCalendar`

3. **Simplify ExchangeBoard Class** → Pure data entity
   - Keep only instance properties and serialization
   - Remove static factory methods

3. **Create ExchangeBoardConfiguration** → Configuration-driven boards
   - Load boards from `exchanges.json` at startup
   - Allow custom boards without recompilation

## Incremental Refactoring Roadmap

### Phase 1: Preparation (Week 1-2)
- [ ] Create `Algo/Helpers/` folder structure
- [ ] Create `BusinessEntities/Exchange/` folder structure
- [ ] Add comprehensive tests for current behavior (baseline)
- [ ] Set up code coverage baseline

### Phase 2: TraderHelper Split (Week 3-6)
- [ ] **Week 3:** Extract `PriceHelper`, `PositionHelper`
- [ ] **Week 4:** Extract `MarketDepthHelper`, `OrderHelper`
- [ ] **Week 5:** Extract `ExchangeHelper`, `SecurityHelper`, `CurrencyHelper`
- [ ] **Week 6:** Extract `PortfolioHelper`, `LoggingHelper`, `MessageAdapterHelper`, `PathHelper`, `LatencyHelper`
- [ ] Update `TraderHelper.cs` as facade with `[Obsolete]` attributes
- [ ] Run full test suite after each extraction

### Phase 3: ExchangeBoard Split (Week 7-10)
- [ ] **Week 7:** Create `ExchangeBoardRegistry` with configuration file
- [ ] **Week 8:** Move predefined boards to `exchanges.json`
- [ ] **Week 9:** Extract `ExchangeCalendar` for trading time logic
- [ ] **Week 10:** Simplify `ExchangeBoard` to pure data entity
- [ ] Run full test suite after each step

### Phase 4: Cleanup (Week 11-12)
- [ ] Remove `[Obsolete]` methods after migration period
- [ ] Update documentation
- [ ] Performance benchmarking
- [ ] Code review and merge

## Risk Mitigation

1. **Backward Compatibility:** Keep old methods as `[Obsolete]` facades for at least 2 major versions
2. **Testing:** Achieve >80% coverage on extracted code before refactoring
3. **Incremental:** One helper class per PR, small reviewable changes
4. **Rollback:** Each phase in separate branch, can be reverted independently
5. **Monitoring:** Add performance tests to detect regressions

## File Size Targets

| File | Current | Target | Reduction |
|------|---------|--------|-----------|
| TraderHelper.cs | 3,877 lines | ~200 lines (facade) | 95% |
| ExchangeBoard.cs | 3,257 lines | ~300 lines | 91% |
| New helper files | N/A | 100-300 lines each | N/A |

## Dependencies

- Requires test infrastructure (✅ Done - Tests project created)
- Requires CI pipeline (✅ Done - GitHub Actions configured)
- Requires code coverage tooling (to be added)

## Success Criteria

- [ ] No file exceeds 500 lines (StyleCop rule)
- [ ] Each class has single responsibility
- [ ] All tests pass
- [ ] No performance regression
- [ ] Backward compatibility maintained
- [ ] Documentation updated

---

**Status:** Planning phase  
**Priority:** Medium (technical debt reduction)  
**Estimated Effort:** 12 weeks (incremental)  
**Owner:** TBD