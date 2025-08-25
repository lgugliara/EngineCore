# Engine Core

This project is a modular engine and drivetrain simulation system for Unity.  
It simulates combustion cycles, torque generation, and drivetrain layout in a flexible and extendable way.

This is the core logic only – it **does not** include wheel physics or suspension simulation.  
For full vehicle simulation, you must install **Wheel Controller 3D** by NWH Coding.

---

## 🔧 Features
- 🔌 Plug-and-play component architecture (`Part`, `Vehicle`)
- 🔥 Engine cycle simulation with RPM and ignition logic
- ⚙️ Differential, clutch, and crankshaft behavior
- 🔊 Sound plugin example (`EngineSound`) with **Chunity** support
- 🧩 Extendable via your own modules

---

## 📦 Requirements
- Unity **2021.3 LTS** or newer
- [**Wheel Controller 3D** – Unity Asset Store](https://assetstore.unity.com/packages/tools/physics/wheel-controller-3d-74512)  
  > Developed by [NWH Coding](https://www.nwhvehiclephysics.com/doku.php/NWH/WheelController3D/index)  
  Required for suspension, traction and full vehicle dynamics.
- [**Chunity (ChucK for Unity)**](https://github.com/ccrma/chunity) (optional)  
  Used for audio synthesis in the included `EngineSound` plugin.

---

## 🗂 Project Structure
```text
EngineCore/
├── base/          # Base classes for all parts
├── parts/         # Core mechanical parts (Engine, Clutch, etc.)
├── data/          # Serializable data containers
├── plugins/       # Optional plugins (e.g., sound simulation)
├── enums/         # Drivetrain configuration constants
├── README.md      # This file
├── .gitignore     # Unity-specific ignore rules
└── LICENSE        # MIT license
```

---

## 🚀 Getting Started

1. Clone or copy the `EngineCore/` folder into your Unity project.
2. Import **[Wheel Controller 3D](https://assetstore.unity.com/packages/tools/physics/wheel-controller-3d-74512)** from the Unity Asset Store.
3. (Optional) Install Chunity to enable audio synthesis via the EngineSound plugin.
4. Add a `Vehicle` component to your vehicle object and hook it up to your custom controller.
5. Extend the engine behavior or use the included plugin examples.

---

## 🧑‍💻 Author

**Zolo**  
[GitHub](https://github.com/zolo86)  
© 2025 — Released under the MIT License.
