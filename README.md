# SmartBorne - borne.core

`borne.core` is the domain and contract library for the SmartBorne solution. It targets .NET Framework 4.8 and contains the shared models, enums, and asynchronous interfaces used by other parts of the solution.

## What the solution contains

- **Entities** for reservations, consumption readings, thresholds, tariffs, tariff brackets, and borne configuration.
- **Local borne state models** for cached reservations, tariff schedules, session state, and pending events.
- **Enums** describing energy types (`Electricite`, `Eau`) and threshold levels (`Avertissement`, `Critique`).
- **Infrastructure contracts** for configuration, consumption, reservations, thresholds, tariffs, tariff brackets, local storage, and card-tag hashing. The asynchronous methods accept cancellation tokens where applicable.
- **Compatibility support** for `IsExternalInit`, used by the record types while targeting .NET Framework.

This project defines domain types and interfaces; it does not currently include concrete database/local-store implementations or an executable application.

## Project layout

```text
SmartBorne.slnx
borne.core/
  Compatibility/
  Entities/
  Enums/
  IInfra/
  Properties/
```

## Build

Open `SmartBorne.slnx` in Visual Studio with .NET Framework 4.8 targeting support, or build the project with MSBuild:

```powershell
msbuild .\borne.core\borne.core.csproj
```
