![Logo](https://avatars0.githubusercontent.com/u/10113234?v=3&s=100)
# StockSharp - Algorithmic Trading Platform

[![Build Status](https://github.com/PreCogSecurity/StockSharp/workflows/CI%20Build%20and%20Security%20Scan/badge.svg)](https://github.com/PreCogSecurity/StockSharp/actions)
[![License: LGPL 3.0](https://img.shields.io/badge/License-LGPL%203.0-blue.svg)](LICENSE)
[![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.8-blue.svg)](https://dotnet.microsoft.com/download/dotnet-framework)
[![Security Scan](https://img.shields.io/badge/Security-Scanned-brightgreen.svg)](.github/workflows/ci.yml)

[StockSharp Home][1] | [Documentation][2] | [Download Zip][3] | [Contributing](CONTRIBUTING.md) | [Changelog](CHANGELOG.md)
----------

## Introduction

**StockSharp (S#)** is a free, open-source algorithmic trading platform for .NET Framework. It enables trading on global markets (American, European, Asian, Russian) across asset classes: stocks, futures, options, cryptocurrencies, forex, and more.

**Key Capabilities:**
- **Manual & Automated Trading** - Discretionary trading or algorithmic robots (conventional & HFT)
- **35+ Market Data Sources** - Real-time and historical data via Hydra (S#.Data)
- **Visual Strategy Designer** - S#.Studio for no-code strategy development
- **Direct Market Access (DMA)** - Low-latency connectivity to exchanges
- **Backtesting & Optimization** - Historical simulation with realistic market modeling

**Supported Connections:** FIX/FAST, LMAX, Rithmic, Fusion/Blackwood, Interactive Brokers, OpenECry, Sterling, IQFeed, E*Trade, BTCE, BitStamp, QUIK, Transaq, SmartCom, CQG, OANDA, and many more.

---

## Architecture

### High-Level Overview

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                            StockSharp Platform                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐   │
│  │   S#.Studio  │  │   S#.Data    │  │   S#.API     │  │  Connectors  │   │
│  │  (Visual     │  │  (Hydra      │  │  (Core       │  │  (Broker/    │   │
│  │   Designer)  │  │   Server)    │  │   Library)   │  │   Exchange)  │   │
│  └──────┬───────┘  └──────┬───────┘  └──────┬───────┘  └──────┬───────┘   │
│         │                 │                 │                 │            │
│         └─────────────────┼─────────────────┼─────────────────┘            │
│                           ▼                 ▼                              │
│                  ┌─────────────────────────────────────┐                   │
│                  │         Core Infrastructure         │                   │
│                  │  ┌─────────┐ ┌─────────┐ ┌────────┐ │                   │
│                  │  │Messages │ │Business │ │ Logging│ │                   │
│                  │  │         │ │Entities │ │        │ │                   │
│                  │  └─────────┘ └─────────┘ └────────┘ │                   │
│                  └─────────────────────────────────────┘                   │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Core Libraries

| Library | Namespace | Purpose |
|---------|-----------|---------|
| **Messages** | `StockSharp.Messages` | Message types for market data, orders, positions, and system events |
| **BusinessEntities** | `StockSharp.BusinessEntities` | Domain models: Security, Order, Portfolio, Position, Trade, MarketDepth |
| **Algo** | `StockSharp.Algo` | Core trading engine: connectors, strategies, candles, risk, PnL, storage |
| **Algo.Strategies** | `StockSharp.Algo.Strategies` | Strategy base classes, parameters, reporting, testing framework |
| **Algo.History** | `StockSharp.Algo.History` | Historical data download, storage, and replay |
| **Logging** | `StockSharp.Logging` | Structured logging with multiple sinks |
| **Localization** | `StockSharp.Localization` | Multi-language resource management |
| **Xaml** | `StockSharp.Xaml` | WPF controls for trading UI (charts, order books, position grids) |

### Connector Architecture

Each connector implements `IMessageAdapter` for a specific broker/exchange:

```
IMessageAdapter (Messages)
    │
    ├── FixMessageAdapter          → FIX/FAST protocol
    ├── InteractiveBrokersMessageAdapter → IB TWS/Gateway
    ├── QuikMessageAdapter         → QUIK (DDE/Lua)
    ├── LmaxMessageAdapter         → LMAX Exchange
    ├── RithmicMessageAdapter      → Rithmic
    ├── TransaqMessageAdapter      → Transaq
    ├── SmartComMessageAdapter     → SmartCom
    ├── PlazaMessageAdapter        → MOEX Plaza
    ├── CryptoMessageAdapter       → BitStamp, Binance, etc.
    └── ... (20+ connectors)
```

**Message Flow:**
```
External System → Connector → MessageAdapter → Core Engine → Strategy → Risk Manager → Connector → External System
                    ↑                                                      │
                    └──────────────── Market Data ────────────────────────┘
```

### Strategy Framework

```
Strategy (Base)
    │
    ├── ProcessState (Stopped, Starting, Started, Stopping)
    ├── Parameters (StrategyParam<T> with optimization support)
    ├── Child Strategies (composite patterns)
    ├── Risk Rules (position limits, drawdown, margin)
    ├── Commission Rules (per-trade, per-volume, tiered)
    ├── Slippage Models (fixed, percentage, volume-based)
    └── Testing/Emulation (HistoryEmulationConnector, MarketEmulator)
```

### Data Storage

```
IStorageRegistry
    │
    ├── SecurityStorage          → Instrument metadata
    ├── MarketDataStorage        → Ticks, OrderLog, Level1, MarketDepth
    ├── CandleStorage            → Time-frame candles (1s to 1M)
    ├── OrderLogStorage          → Full order log
    ├── NewsStorage              → News headlines/body
    └── Backup (S3, Azure, Glacier) → Cloud archival
```

### Hydra (S#.Data) - Market Data Server

```
Hydra Core
    │
    ├── Plugin System (35+ sources)
    │   ├── Exchange Plugins (MOEX, NYSE, NASDAQ, etc.)
    │   ├── Crypto Plugins (Binance, Coinbase, BitStamp, etc.)
    │   ├── Forex Plugins (OANDA, FXCM, etc.)
    │   └── Custom Plugins (extensible)
    │
    ├── Storage Backends
    │   ├── Local (binary, CSV, SQLite)
    │   ├── SQL Server / PostgreSQL
    │   └── Cloud (S3, Azure Blob)
    │
    └── API (REST, WCF, TCP) for consumers
```

---

## S#.Studio

S#.Studio is a free graphical environment for trading on any exchanges. Features:

- **Visual Strategy Designer** - Drag-and-drop strategy creation
- **C# Strategy Editor** - Full IDE with IntelliSense, debugging
- **Real-time Charts** - SciChart-powered high-performance charting
- **Order Management** - DOM, order book, position monitoring
- **Backtesting UI** - Visual parameter optimization

[More info...](http://stocksharp.com/products/studio/)

### Screenshots

![Terminal](http://stocksharp.com/file.ashx?t=forum&size=400x200&fid=2099)
![Designer](http://stocksharp.com/file.ashx?t=forum&size=400x200&fid=2091)
[more on official site...](http://stocksharp.com/products/studio/)

---

## S#.Data (Hydra)

S#.Data is a free application for downloading and storing market data from 35+ sources.

- **Automated Download** - Scheduled, incremental, gap-filling
- **Multiple Formats** - Binary (fast), CSV (interop), Database (query)
- **Cloud Backup** - AWS S3, Azure Blob, Glacier
- **Plugin Architecture** - Add new data sources without core changes

[More info...](http://stocksharp.com/products/hydra/)

---

## S#.API

S#.API is a free C# library for programmers using Visual Studio. Create any trading strategy:

- **Long-term positional** to **HFT** strategies
- **Direct Market Access (DMA)** to exchanges
- **Full order lifecycle** management
- **Risk management** built-in

[More info...](http://stocksharp.com/products/api/)

---

## Quick Start

### Prerequisites

- Windows 10/11
- Visual Studio 2022 with .NET Framework 4.8 SDK
- Git

### Build from Source

```bash
# Clone
git clone https://github.com/PreCogSecurity/StockSharp.git
cd StockSharp

# Restore dependencies
nuget restore StockSharp.sln

# Build Release
msbuild StockSharp.sln /p:Configuration=Release /p:Platform="Any CPU"
```

### Run Samples

```bash
# Anywhere sample (WPF demo with QUIK)
cd Anywhere\Anywhere
msbuild Anywhere.csproj /p:Configuration=Release
bin\Release\Anywhere.exe

# Set QUIK credentials via environment variables
$env:QUIK_LOGIN="your_login"
$env:QUIK_PASSWORD="your_password"
```

### Configuration

Copy `.env.example` to `.env` and fill in your credentials:

```bash
cp .env.example .env
# Edit .env with your actual values
```

**Never commit `.env` files!** They are in `.gitignore`.

---

## Development

### Coding Standards

- **StyleCop.Analyzers** enforced (see `stylecop.json`)
- **EditorConfig** for consistent formatting (see `.editorconfig`)
- **Security-first** - no hardcoded secrets, input validation, secure defaults
- **Documentation** - XML comments for all public APIs

See [CONTRIBUTING.md](CONTRIBUTING.md) for full guidelines.

### CI/CD Pipeline

GitHub Actions workflow (`.github/workflows/ci.yml`) runs on every PR:

1. **Build** - Debug & Release configurations
2. **StyleCop** - Code style analysis
3. **Security Scan** - Gitleaks (secrets), hardcoded credential check
4. **Dependency Check** - OWASP vulnerability scanning
5. **Tests** - Unit & integration tests (when available)
6. **CodeQL** - Static analysis (GitHub Advanced Security)

### Adding a New Connector

1. Create project in `Connectors/<BrokerName>/`
2. Implement `IMessageAdapter` 
3. Add message types for broker-specific data
4. Add sample in `Samples/<BrokerName>/`
5. Update documentation

---

## Support

- **Free Support**: [Forum](http://stocksharp.com/forum) | [Discord](https://discord.gg/stocksharp)
- **Commercial Support**: [Algotrading Courses](http://edu.stocksharp.com)
- **Security Issues**: Email **security@precogsecurity.com** (see [SECURITY.md](SECURITY.md))
- **Documentation**: [stocksharp.com/doc](http://stocksharp.com/doc)

---

## License

StockSharp is licensed under the **LGPL 3.0 license**. See [LICENSE](LICENSE) for details.

```
Copyright © PreCog Security 2024
```

---

## Acknowledgments

- **Ecng Framework** - Core utilities (collections, serialization, XAML)
- **MathNet.Numerics** - Mathematical functions for indicators
- **SciChart** - High-performance WPF charting
- **MoreLINQ** - Extended LINQ operators
- **NCalc** - Mathematical expression evaluation
- **All Contributors** - Past and present

---

  [1]: http://stocksharp.com
  [2]: http://stocksharp.com/doc
  [3]: https://github.com/PreCogSecurity/StockSharp/releases
  [4]: http://edu.stocksharp.com
  [5]: http://stocksharp.com/forum
  [6]: http://stocksharp.com/broker