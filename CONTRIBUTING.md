# Contributing to StockSharp

Thank you for your interest in contributing to StockSharp! This document provides guidelines for contributing to the project.

## Table of Contents

1. [Code of Conduct](#code-of-conduct)
2. [Getting Started](#getting-started)
3. [Development Environment](#development-environment)
4. [Coding Standards](#coding-standards)
5. [Security Guidelines](#security-guidelines)
6. [Testing](#testing)
7. [Pull Request Process](#pull-request-process)
8. [Reporting Issues](#reporting-issues)
9. [License](#license)

## Code of Conduct

This project adheres to a [Code of Conduct](CODE_OF_CONDUCT.md). By participating, you are expected to uphold this code. Please report unacceptable behavior to security@precogsecurity.com.

## Getting Started

### Prerequisites

- **Visual Studio 2022** (Community, Professional, or Enterprise) with .NET Framework 4.8 SDK
- **Git** for version control
- **Windows 10/11** (required for WPF components)

### Clone and Build

```bash
# Clone the repository
git clone https://github.com/PreCogSecurity/StockSharp.git
cd StockSharp

# Restore dependencies (NuGet packages and References)
nuget restore StockSharp.sln

# Build the solution
msbuild StockSharp.sln /p:Configuration=Release /p:Platform="Any CPU"
```

### Project Structure

```
StockSharp/
├── Algo/                 # Core algorithmic trading engine
├── Algo.History/         # Historical data processing
├── Algo.Strategies/      # Trading strategies framework
├── Alerts/               # Alert/notification system
├── Anywhere/             # Sample WPF application (demo)
├── BusinessEntities/     # Core domain models (Security, Order, Portfolio, etc.)
├── Community/            # Community extensions
├── Connectors/           # Broker/exchange connectors (IB, QUIK, FIX, etc.)
├── Documentation/        # Documentation and DocViewer
├── Hydra/                # Market data server (S#.Data)
├── Localization/         # Multi-language support
├── Logging/              # Logging infrastructure
├── Messages/             # Message types for market data/trading
├── Samples/              # Sample applications
├── Studio/               # S#.Studio visual designer
├── Xaml/                 # WPF controls and charting
├── Xaml.Charting/        # Charting components (SciChart)
├── Xaml.Diagram/         # Visual strategy designer
├── References/           # Third-party DLLs (legacy, pre-NuGet)
├── _ReleaseNotes/        # Release notes and changelogs
├── .github/              # GitHub Actions CI/CD workflows
├── .editorconfig         # Editor configuration
├── Directory.Build.props # Centralized build properties
├── StockSharp.sln        # Main solution file
└── StockSharpAssemblyInfo.cs # Shared assembly attributes
```

## Development Environment

### Recommended VS Extensions

- **StyleCop.Analyzers** - Code style enforcement
- **SonarLint** - Code quality analysis
- **EditorConfig** - Consistent editor settings
- **GitHub Actions** - CI/CD integration

### Build Configurations

- **Debug** - Full symbols, no optimization, DEBUG/TRACE defined
- **Release** - Optimized, portable PDBs, TRACE defined, warnings as errors

### Running Tests

```bash
# Build first
msbuild StockSharp.sln /p:Configuration=Release

# Run tests (when test projects exist)
vstest.console.exe bin\Release\*Test*.dll /logger:trx
```

## Coding Standards

### C# Style Guide

We follow **Microsoft C# Coding Conventions** with these additions:

1. **StyleCop.Analyzers** rules are enforced (see `stylecop.json`)
2. **EditorConfig** settings in `.editorconfig` must be followed
3. **File-scoped namespaces** preferred (C# 10+)
4. **Nullable reference types** enabled where possible
5. **Expression-bodied members** for simple methods/properties
6. **Async/await** over Task.Result/.Wait()

### Naming Conventions

| Element | Convention | Example |
|---------|------------|---------|
| Classes/Interfaces | PascalCase | `Security`, `IConnector` |
| Methods | PascalCase | `ConnectAsync()`, `GetSecurity()` |
| Properties | PascalCase | `IsConnected`, `Portfolio` |
| Fields (private) | `_camelCase` | `_securities`, `_isConnected` |
| Constants | PascalCase | `DefaultMaxDepth` |
| Parameters | camelCase | `securityId`, `cancellationToken` |
| Type Parameters | PascalCase with `T` prefix | `TMessage`, `TAdapter` |

### Code Organization

- **One type per file** (enforced by StyleCop)
- **Regions** discouraged; use small, focused classes instead
- **XML documentation** for all public APIs (`///` comments)
- **No commented-out code** - use version control instead

### Security-Critical Code

- **Never hardcode secrets** (passwords, API keys, connection strings)
- **Use environment variables** or secure configuration (see `.env.example`)
- **Validate all inputs** - never trust external data
- **Use `SecureString`** for passwords in memory
- **Enable security mitigations** (ASLR, DEP, CFG) via `Directory.Build.props`

## Security Guidelines

### Reporting Security Vulnerabilities

**DO NOT** create public issues for security vulnerabilities. Instead:

1. Email **security@precogsecurity.com** with details
2. Include steps to reproduce, impact assessment, and suggested fix
3. Allow 90 days for coordinated disclosure before public discussion

### Secure Coding Practices

1. **Input Validation**
   - Validate all external inputs (FIX messages, API responses, user input)
   - Use allow-lists over block-lists
   - Sanitize data before logging (no PII/secrets in logs)

2. **Authentication & Authorization**
   - Use secure credential storage (Windows Credential Manager, Key Vault)
   - Implement proper session management
   - Validate permissions on every sensitive operation

3. **Cryptography**
   - Use `System.Security.Cryptography` APIs
   - Never implement custom crypto
   - Use TLS 1.2+ for all network communication
   - Rotate keys regularly

4. **Dependencies**
   - Keep dependencies updated (Dependabot alerts enabled)
   - Scan for vulnerabilities in CI (OWASP Dependency Check)
   - Prefer NuGet packages over bundled DLLs

5. **Secrets Management**
   - `.env` files in `.gitignore` (use `.env.example` as template)
   - CI/CD secrets in GitHub Secrets / Azure Key Vault
   - No secrets in code, config files, or logs

## Testing

### Test Structure (Planned)

```
Tests/
├── StockSharp.Algo.Tests/           # Core engine tests
├── StockSharp.BusinessEntities.Tests/ # Domain model tests
├── StockSharp.Messages.Tests/        # Message serialization tests
├── StockSharp.Connectors.Tests/      # Connector integration tests
└── StockSharp.Integration.Tests/     # End-to-end tests
```

### Test Guidelines

- **Unit tests** for pure logic (indicators, math, helpers)
- **Integration tests** for connectors (require credentials)
- **Property-based tests** for financial calculations
- **Contract tests** for message serialization
- **Target: >80% code coverage** for core libraries

### Running Tests Locally

```bash
# Build test projects
msbuild Tests\Tests.sln /p:Configuration=Release

# Run with coverage
dotnet test Tests\Tests.sln --collect:"XPlat Code Coverage"
```

## Pull Request Process

### Before Submitting

1. **Sync with upstream** - rebase on latest `main`
2. **Run full build** - `msbuild StockSharp.sln /p:Configuration=Release`
3. **Run StyleCop** - no new warnings
4. **Run tests** - all existing tests pass
5. **Update documentation** - README, XML comments, CHANGELOG

### PR Requirements

- [ ] Clear title and description
- [ ] Linked issue (if applicable)
- [ ] Small, focused changes (one concern per PR)
- [ ] No breaking changes without major version bump
- [ ] Security review for auth/crypto/secret changes
- [ ] CI passes (build, style, security scan, tests)

### Review Process

1. **Automated checks** must pass (CI pipeline)
2. **Code review** by at least one maintainer
3. **Security review** for sensitive areas
4. **Merge** after approval (squash and merge preferred)

### Commit Messages

Follow [Conventional Commits](https://www.conventionalcommits.org/):

```
feat: add new OrderBook indicator
fix: resolve memory leak in CandleManager
docs: update README with architecture diagram
sec: remove hardcoded QUIK credentials
refactor: split TraderHelper into focused classes
test: add unit tests for BlackScholes
chore: update NuGet packages
```

## Reporting Issues

### Bug Reports

Use the bug report template. Include:

- **Environment**: OS, .NET version, VS version
- **Steps to reproduce**: Minimal code sample
- **Expected vs actual behavior**
- **Logs/stack traces** (sanitized)
- **Connector/broker** if applicable

### Feature Requests

Use the feature request template. Describe:

- **Problem** being solved
- **Proposed solution** with API sketch
- **Alternatives considered**
- **Impact** on existing code

### Questions

- Check [documentation](http://stocksharp.com/doc) first
- Search existing issues
- Ask on [forum](http://stocksharp.com/forum) or Discord

## License

StockSharp is licensed under **LGPL 3.0**. By contributing, you agree that your contributions will be licensed under the same terms.

See [LICENSE](LICENSE) for full text.

---

**Maintainers**: PreCog Security Team  
**Contact**: security@precogsecurity.com  
**Website**: https://precogsecurity.com