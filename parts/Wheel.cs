using UnityEngine;
using NWH.WheelController3D;

// ZoloSim Engine Core - © 2025 Lorenzo Gugliara
// Author: Lorenzo Gugliara | https://github.com/lgugliara
// License: MIT

namespace ZoloSim.EngineCore
{
    public class Wheel : Part
    {
        [Header("Traction Control")]

        // Slip threshold for TRS detection
        public float slipThreshold = 0.5f;
        public bool IsTRS => wc.IsGrounded && Mathf.Abs(angularVelocity - car.velocity.sqrMagnitude) >= slipThreshold;

        // Get the WheelController component and perform calculations
        public WheelController wc => GetComponent<WheelController>();
        public float angularMomentum => wc.wheel.rpm * wc.wheel.inertia;
        public float angularVelocity => wc.wheel.rpm * 2f * Mathf.PI / 60f;

        void Awake()
        {
            wc.meshColliderLayer = car.gameObject.layer;
        }

        // Override the base class method to reset properly the wheel
        public override void Reset()
        {
            wc.wheel.prevAngularVelocity = 0;
            wc.wheel.angularVelocity = 0;
        }
    }
}