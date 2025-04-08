using UnityEngine;

// ZoloSim Engine Core - © 2025 Lorenzo Gugliara
// Author: Lorenzo Gugliara | https://github.com/lgugliara
// License: MIT

namespace ZoloSim.EngineCore
{
    [CreateAssetMenu(fileName = "Vehicle Data", menuName = "Zolo Simulator/Vehicle", order = 0)]
    public class VehicleData : ScriptableObject
    {
        // Optional display name of the car, mainly for UI or editor reference
        public string Name;

        [Header("Vehicle parts")]

        // Core car components / parts
        public EngineData engine;
        public TransmissionData transmission;
        public SteeringData steering;
        public BrakeData brake;
        public HandbrakeData handbrake;
    }
}