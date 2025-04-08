using UnityEngine;

// ZoloSim Engine Core - © 2025 Lorenzo Gugliara
// Author: Lorenzo Gugliara | https://github.com/lgugliara
// License: MIT

namespace ZoloSim.EngineCore
{
    [CreateAssetMenu(fileName = "Clutch Data", menuName = "Zolo Simulator/Parts/Clutch Data")]
    public class ClutchData : ScriptableObject
    {
        [Header("Disk properties")]

        // Number of clutch disks
        [Range(1, 5)]
        public int disks = 1;
        public int surfaces => disks * 2;

        // Diameter of the clutch in inches and conversion to meters (m)
        public float diameterInches;
        public float diameterMetric => diameterInches * 0.0254f;
        public float radius => diameterMetric * 0.5f;

        [Header("Spring")]

        // Spring compression force espressed in Newtons (N)
        public float compression;

        public float maxTorque => compression * surfaces * radius;
    }
}