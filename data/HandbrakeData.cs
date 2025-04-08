using UnityEngine;

// ZoloSim Engine Core - © 2025 Lorenzo Gugliara
// Author: Lorenzo Gugliara | https://github.com/lgugliara
// License: MIT

namespace ZoloSim.EngineCore
{
    [CreateAssetMenu(fileName = "Handbrake Data", menuName = "Zolo Simulator/Parts/Handbrake Data")]
    public class HandbrakeData : ScriptableObject
    {
        // Break force in Newton (N)
        public float brakeForce;
    }
}