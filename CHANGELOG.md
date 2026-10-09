# Changelog

All notable changes to StockSharp will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Security
- **FIXED**: Removed hardcoded QUIK credentials ("quik"/"quik") from Anywhere sample application
  - Credentials now loaded from environment variables `QUIK_LOGIN` and `QUIK_PASSWORD`
  - Added `.env.example` template for secure configuration
- **ADDED**: GitHub Actions CI pipeline with security scanning
  - Gitleaks secret detection on every push/PR
  - OWASP Dependency Check for vulnerability scanning
  - CodeQL static analysis (GitHub Advanced Security)
  - Custom hardcoded credential pattern detection
- **ADDED**: Security hardening in `Directory.Build.props`
  - Deterministic builds for reproducibility
  - High entropy ASLR support
  - Control Flow Guard (where supported)
  - Treat warnings as errors in Release
  - Code analysis enabled by default
- **ADDED**: `.gitleaks.toml` configuration for secret detection
- **ADDED**: Dependency check suppressions for legacy framework assemblies

### Added
- **NEW**: `.editorconfig` for consistent code style across editors/IDEs
- **NEW**: `Directory.Build.props` for centralized build properties and security settings
- **NEW**: `.env.example` template for environment-based configuration
- **NEW**: `CONTRIBUTING.md` with comprehensive contribution guidelines
- **NEW**: `CHANGELOG.md` (this file)
- **NEW**: GitHub Actions CI workflow (`.github/workflows/ci.yml`)
- **NEW**: CodeQL configuration (`.github/codeql-config.yml`)
- **NEW**: VSTest runsettings (`.github/runsettings.xml`)
- **NEW**: Architecture documentation in README.md
- **NEW**: Security documentation references

### Changed
- **IMPROVED**: README.md with comprehensive architecture overview, quick start guide, and development info
- **IMPROVED**: Build system with deterministic, reproducible builds
- **IMPROVED**: Warning level set to 4 with warnings-as-errors in Release
- **IMPROVED**: Code analysis enabled by default for all projects

### Fixed
- **FIXED**: Hardcoded credentials in `Anywhere/Anywhere/MainWindow.xaml.cs`

---

## [4.3.12] - 2024-12-19

### Security
- Security hardening release preparing codebase for customer shipment
- All security improvements from Unreleased section included

### Added
- Modern CI/CD pipeline with GitHub Actions
- Comprehensive security scanning (secrets, dependencies, static analysis)
- Developer experience improvements (EditorConfig, Contributing guide, Changelog)
- Environment-based configuration management

### Changed
- Migrated from legacy build to modern MSBuild with centralized properties
- Updated documentation with architecture diagrams and contribution guidelines

---

## [4.3.11] - 2015-09-23

### Historical Release (Legacy)

This was the last public release before the repository went dormant. Key features at that time:

- **Core Trading Engine**: Order management, position tracking, PnL calculation
- **Connector Framework**: 20+ broker/exchange connectors (IB, QUIK, FIX, LMAX, etc.)
- **Strategy Framework**: Base classes, parameters, optimization, backtesting
- **Market Data**: Real-time and historical data processing (Hydra/S#.Data)
- **Visual Designer**: S#.Studio with drag-and-drop strategy creation
- **Charting**: SciChart-powered WPF charting components
- **Storage**: Binary, CSV, and database storage for market data
- **Risk Management**: Position limits, drawdown controls, commission models

### Known Limitations (2015 Codebase)
- .NET Framework 4.0 (legacy, no longer supported by Microsoft)
- No automated test suite
- No CI/CD pipeline
- Direct DLL references instead of NuGet packages
- Large monolithic files (TraderHelper.cs ~3000 lines, ExchangeBoard.cs ~2000 lines)
- Hardcoded credentials in sample applications
- No security scanning or dependency vulnerability management
- Limited input validation and error handling

---

## Migration Notes

### Upgrading from 4.3.11 to 4.3.12+

**Breaking Changes:**
- Minimum .NET Framework updated to 4.8 (from 4.0)
- Build system now uses `Directory.Build.props` - verify custom build scripts
- Release builds now treat warnings as errors - fix any new warnings
- Code analysis enabled - may surface previously hidden issues

**Required Actions:**
1. Update Visual Studio to 2022 with .NET Framework 4.8 SDK
2. Copy `.env.example` to `.env` and configure credentials
3. Run `nuget restore StockSharp.sln` to restore packages
4. Build with `msbuild StockSharp.sln /p:Configuration=Release`
5. Review CI pipeline output for any new warnings/errors

**Recommended Actions:**
1. Migrate References folder DLLs to NuGet packages
2. Add unit tests for core libraries (Algo, BusinessEntities, Messages)
3. Split large files (TraderHelper.cs, ExchangeBoard.cs) into focused classes
4. Enable nullable reference types where feasible
5. Update connectors to use async/await patterns

---

## Release Process

1. **Version Bump**: Update `AssemblyVersion` in `Directory.Build.props`
2. **Changelog**: Document changes in this file under `## [Unreleased]`
3. **Tag**: Create git tag `v{major}.{minor}.{patch}`
4. **Build**: CI builds and publishes artifacts
5. **Release**: GitHub Release with changelog and binaries
6. **Announce**: Forum, Discord, mailing list

---

## Links

- [GitHub Releases](https://github.com/PreCogSecurity/StockSharp/releases)
- [Release Notes (Legacy)](_ReleaseNotes/CHANGE_LOG_API.md)
- [Release Stages](_ReleaseNotes/RELEASE_STAGES.md)
- [Security Policy](SECURITY.md)