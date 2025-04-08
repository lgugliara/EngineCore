using UnityEngine;

// ZoloSim Engine Core - © 2025 Lorenzo Gugliara
// Author: Lorenzo Gugliara | https://github.com/lgugliara
// License: MIT

namespace ZoloSim.EngineCore
{
    [CreateAssetMenu(fileName = "Brake Data", menuName = "Zolo Simulator/Parts/Brake Data")]
    public class BrakeData : ScriptableObject
    {
        // The maximum brake force that can be applied to the wheels in Newtons (N)
        public float brakeForce;

        // The brake force distribution between the front and rear wheels.
        // A value of 0.0 means all the force is applied to the front wheels,
        // while a value of 1.0 means all the force is applied to the rear wheels.
        [Range(0f, 1f)]
        public float frontRearBalance = 0.5f;
    }
}