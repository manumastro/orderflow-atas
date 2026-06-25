# FabioOrderFlow Documentation Index

## 📚 Documentation Map

### Getting Started

| Document | Description | For |
|----------|-------------|-----|
| [README.md](README.md) | Quick start, build, deploy | Users & Developers |
| [CHANGELOG.md](CHANGELOG.md) | Version history, migration guide | All |

### Architecture & Design

| Document | Description | For |
|----------|-------------|-----|
| [ARCHITECTURE.md](ARCHITECTURE.md) | Full system architecture, data flow, design patterns | Developers |
| [MODULES.md](MODULES.md) | Strategy module development guide with examples | Module Developers |

### Project Structure

| Path | Description |
|------|-------------|
| `src/FabioOrderFlow.cs` | Entry point, orchestrator |
| `src/modules/shared/BalanceZoneTracker/` | Core balance zone tracking |
| `src/modules/shared/SessionDetector/` | Session detection utility |
| `src/modules/LondonMeanReversion/` | Mean reversion strategy module |

---

## 🎯 Quick Links by Role

### I'm a User (want to use the indicator)

1. [README.md § Quick Start](README.md#quick-start) - Build & deploy
2. [README.md § Features](README.md#features) - What it does
3. [README.md § Settings](README.md#settings) - Configuration options
4. [README.md § Log Output](README.md#log-output-examples) - Interpreting logs

### I'm a Developer (want to understand the code)

1. [ARCHITECTURE.md § Overview](ARCHITECTURE.md#overview) - High-level design
2. [ARCHITECTURE.md § Component Roles](ARCHITECTURE.md#component-roles) - Detailed components
3. [ARCHITECTURE.md § Data Flow](ARCHITECTURE.md#data-flow) - How events propagate
4. [README.md § Development](README.md#development) - Project structure

### I Want to Create a New Module

1. [MODULES.md § Module Development Guide](MODULES.md#module-development-guide) - Step-by-step tutorial
2. [MODULES.md § Design Patterns](MODULES.md#design-patterns) - Best practices
3. [MODULES.md § Examples](MODULES.md#examples) - Reference implementations
4. [ARCHITECTURE.md § Extensibility](ARCHITECTURE.md#extensibility) - Extension points

### I Want to Understand the Refactoring

1. [CHANGELOG.md § [2.0.0]](CHANGELOG.md#200---2026-06-25) - What changed
2. [CHANGELOG.md § Migration Guide](CHANGELOG.md#migration-guide) - How to adapt code
3. [CHANGELOG.md § Development Stats](CHANGELOG.md#development-stats) - Refactoring phases

---

## 📖 Reading Path by Goal

### Goal: Deploy and Use

```
README.md § Quick Start
  ↓
README.md § Requirements
  ↓
README.md § Settings
  ↓
README.md § Log Output Examples
```

### Goal: Understand Architecture

```
ARCHITECTURE.md § Overview
  ↓
ARCHITECTURE.md § Component Roles
  ↓
ARCHITECTURE.md § Data Flow
  ↓
ARCHITECTURE.md § Configuration
```

### Goal: Create a Strategy Module

```
MODULES.md § Overview
  ↓
MODULES.md § Existing Modules (study LondonMeanReversionModule)
  ↓
MODULES.md § Module Development Guide (step-by-step)
  ↓
MODULES.md § Design Patterns (best practices)
  ↓
MODULES.md § Examples (reference code)
```

### Goal: Maintain/Extend Core

```
ARCHITECTURE.md § Component Roles § BalanceZoneTracker
  ↓
ARCHITECTURE.md § Data Flow
  ↓
src/modules/shared/BalanceZoneTracker/BalanceZoneTracker.cs
  ↓
ARCHITECTURE.md § Performance Considerations
```

---

## 🔍 Find Information Fast

### How do I...

**...build and deploy the indicator?**  
→ [README.md § Build & Deploy](README.md#build--deploy)

**...enable/disable a module?**  
→ [README.md § Settings](README.md#settings)

**...understand the balance zone state machine?**  
→ [ARCHITECTURE.md § BalanceZoneTracker](ARCHITECTURE.md#2-balancezonetracker-core)

**...create a new strategy module?**  
→ [MODULES.md § Step-by-Step](MODULES.md#step-by-step-create-new-module)

**...read data from BalanceZoneTracker?**  
→ [MODULES.md § Design Patterns § Read-Only Access](MODULES.md#read-only-access)

**...process CumulativeTrades events?**  
→ [MODULES.md § Add CumulativeTrades Support](MODULES.md#4-add-cumulativetrades-support-optional)

**...interpret log output?**  
→ [README.md § Log Output Examples](README.md#log-output-examples) + `../../docs/atas/log-reading.md`

**...understand what changed in v2.0?**  
→ [CHANGELOG.md § [2.0.0]](CHANGELOG.md#200---2026-06-25)

**...migrate from monolith architecture?**  
→ [CHANGELOG.md § Migration Guide](CHANGELOG.md#migration-guide)

**...optimize performance?**  
→ [ARCHITECTURE.md § Performance Considerations](ARCHITECTURE.md#performance-considerations)

**...test a module?**  
→ [MODULES.md § Testing Modules](MODULES.md#testing-modules)

---

## 🗂️ Document Details

### ARCHITECTURE.md
**Length:** ~11KB  
**Sections:** 11  
**Target:** Developers, architects  
**Topics:**
- Component roles (Orchestrator, Tracker, Modules)
- Data flow diagrams (text-based)
- Configuration patterns
- Extensibility guide
- Performance considerations
- Known limitations

### README.md
**Length:** ~4KB  
**Sections:** 7  
**Target:** Users, new developers  
**Topics:**
- Quick start (build & deploy)
- Features overview
- Settings reference
- Log output examples
- Project structure
- Development guide

### CHANGELOG.md
**Length:** ~6KB  
**Sections:** 4  
**Target:** All stakeholders  
**Topics:**
- Version 2.0.0 changes (modular refactoring)
- Previous architecture (monolith)
- Migration guide (users & developers)
- Development stats (phases, commits)
- Future roadmap

### MODULES.md
**Length:** ~14KB  
**Sections:** 13  
**Target:** Module developers  
**Topics:**
- Existing modules (LondonMeanReversion)
- Step-by-step module creation
- Design patterns (DI, read-only, delegation)
- Testing strategies
- Best practices
- Performance tips
- FAQ
- Examples

---

## 📊 Documentation Stats

| Metric | Value |
|--------|-------|
| Total docs | 4 |
| Total size | ~35KB |
| Code examples | 30+ |
| Diagrams (text) | 8 |
| External links | 5 |

---

## 🔗 External Resources

| Resource | Location |
|----------|----------|
| ATAS log parsing guide | `../../docs/atas/log-reading.md` |
| ATAS API reference | `../../docs/atas/api/` |
| Modello 1 spec | `../../Modello-1-TrendFollowing/MODELLO-1-DOCUMENTAZIONE.md` |
| Modello 2 spec | `../../Modello-2-MeanReversion/FabioMeanReversion.md` |

---

## 🚀 Next Steps

**For new users:**
1. Read [README.md](README.md)
2. Build & deploy
3. Test on ATAS with sample data

**For developers:**
1. Read [ARCHITECTURE.md](ARCHITECTURE.md)
2. Study `src/modules/LondonMeanReversion/` code
3. Try creating a simple module following [MODULES.md](MODULES.md)

**For contributors:**
1. Check [CHANGELOG.md § Future Roadmap](CHANGELOG.md#future-roadmap)
2. Pick a task from Technical Debt or Planned Modules
3. Follow design patterns in [MODULES.md](MODULES.md)

---

## 📝 Document Maintenance

**When to update:**
- **README.md**: new features, settings, build changes
- **ARCHITECTURE.md**: component changes, new patterns, API changes
- **CHANGELOG.md**: every release, breaking changes, migrations
- **MODULES.md**: new modules, pattern changes, new examples

**Review cycle:** After every major refactoring or module addition

**Ownership:** Core team (maintain consistency across docs)
