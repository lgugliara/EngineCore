using UnityEngine;

// ZoloSim Engine Core - © 2025 Lorenzo Gugliara
// Author: Lorenzo Gugliara | https://github.com/lgugliara
// License: MIT

namespace ZoloSim.EngineCore
{
    [CreateAssetMenu(fileName = "Injection Data", menuName = "Zolo Simulator/Parts/Injection Data")]
    public class InjectionData : ScriptableObject
    {
        // Fuel injection curve (x: RPM ratio [0-1], y: efficiency [0-1])
        public AnimationCurve injectionCurve;

        // Combustion duration in seconds (s), typically in the range of 0.0001–0.002
        [Range(0.0001f, 0.002f)]
        public float combustionTime = 0.001f;
    }
}
