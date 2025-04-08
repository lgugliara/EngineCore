using UnityEngine;

// ZoloSim Engine Core - © 2025 Lorenzo Gugliara
// Author: Lorenzo Gugliara | https://github.com/lgugliara
// License: MIT

namespace ZoloSim.EngineCore
{
    [CreateAssetMenu(fileName = "Engine Data", menuName = "Zolo Simulator/Parts/Engine Data")]
    public class EngineData : ScriptableObject
    {
        [Header("Drivetrain properties")]

        // Engine displacement in cubic centimeters (cc) and conversion to cubic meters (m³)
        public float displacement;
        public float displacementMetric => displacement * 1e-6f;

        // Number of cylinders
        public int cylinders;

        // Internal friction coefficient (used to simulate power loss)
        [Range(0f, 1f)]
        public float internalFriction;

        [Header("Pistons properties")]

        // Compression ratio (unitless, e.g. 10.5:1)
        public float compressionRatio;

        // Cylinder bore (diameter) in meters (m)
        public float bore;

        // Material of the pistons (affects density and mass)
        public PistonMaterial pistonMaterial;

        [Header("Crankshaft properties")]

        // Type of crankshaft (Flat plane or Cross plane) and mass in kilograms (kg)
        public CrankshaftPosition crankshaftPosition;
        public float crankshaftMass;

        [Header("Performance")]

        // Maximum torque output in horsepower (hp) and convertion to Newton-meters per second (Nm/s)
        public float maxTorque;
        public float maxTorqueMetric => maxTorque * 735.49875f;

        // Minimum and maximum engine RPM
        public float minRPM;
        public float maxRPM;

        [Header("Other parts")]

        // Clutch configuration used by this engine
        public ClutchData clutch;

        // Injection configuration used by this engine
        public InjectionData injection;

        [Header("Audio properties")]

        // Audio configuration for this engine
        public EngineSound sound;

        #region Derived Properties

        // Rotational inertia of the engine [kg·m²] based on bore and moving mass
        public float inertia => (pistonsMass + crankshaftMass) * bore * bore / 8f;

        // Estimated mass of the pistons in kilograms (kg)
        private float pistonsMass => displacementMetric * pistonDensityMetric;

        // Piston density in grams per cubic centimeter (g/cm³) and conversion to kilograms per cubic meter (kg/m³)
        public float pistonDensity =>
            pistonMaterial switch
            {
                PistonMaterial.Aluminum => 2.7f,
                PistonMaterial.Steel => 7.8f,
                PistonMaterial.Titanium => 4.5f,
                _ => 2.7f // Default case
            };

        public float pistonDensityMetric => pistonDensity * 1000f;

        #endregion
    }
}
