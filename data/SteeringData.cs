using UnityEngine;

// ZoloSim Engine Core - © 2025 Lorenzo Gugliara
// Author: Lorenzo Gugliara | https://github.com/lgugliara
// License: MIT

namespace ZoloSim.EngineCore
{
    [CreateAssetMenu(fileName = "Steering Data", menuName = "Zolo Simulator/Parts/Steering Data")]
    public class SteeringData : ScriptableObject
    {
        // Maximum steering angle in degrees
        [Range(0, 90)]
        public float steerAngle;
    }
}