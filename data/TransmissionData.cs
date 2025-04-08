using UnityEngine;

// ZoloSim Engine Core - © 2025 Lorenzo Gugliara
// Author: Lorenzo Gugliara | https://github.com/lgugliara
// License: MIT

namespace ZoloSim.EngineCore
{
    [CreateAssetMenu(fileName = "Transmission Data", menuName = "Zolo Simulator/Parts/Transmission Data")]
    public class TransmissionData : ScriptableObject
    {
        [Header("Traction properties")]

        // Traction type (Rear Wheel Drive, Front Wheel Drive, All Wheel Drive)
        public TractionType tractionType = TractionType.RWD;

        // Traction control system (TCS) configuration used by this transmission
        public bool HasTCS;

        // Transmission friction coefficient (used to simulate power loss)
        [Range(0f, 1f)]
        public float transmissionLosses;

        [Header("Ratios")]

        // Gear ratios, excluding reverse. Add elements to automatically add gears.
        public float[] gearRatios;

        // Reverse gear ratio
        public float reverseRatio;

        // Differential ratio
        public float differentialRatio;
    }
}