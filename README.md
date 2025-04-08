# ZoloSim Engine Core

Modular engine and drivetrain simulation system for Unity.  
Designed to be embedded into larger Unity vehicle simulation projects.

## Features
- Component-based architecture (Vehicle, Part)
- Core engine behavior simulation
- Expandable via plugin system (e.g., EngineSound)

## Structure
- `Vehicle.cs`: Main vehicle logic
- `Part.cs`: Base component module
- `enums/`: Definitions for drivetrain layout, crankshaft position, etc.
- `plugins/`: Optional plugins (e.g., sound simulation)
- `interfaces/`, `data/`, `parts/`: Internal logic and organization

## Usage
Place the the root folder of this project inside your Unity project.  
Import namespaces (e.g., `using ZoloSim.EngineCore;`) to access core functionality.

## Requirements
- Unity (recommended version: 2021.3 or newer)

## License
MIT License – see [LICENSE](./LICENSE) for full details.
