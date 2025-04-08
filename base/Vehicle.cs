using UnityEngine;
using ZoloSim.EngineCore.Enums;

// ZoloSim Engine Core - © 2025 Lorenzo Gugliara
// Author: Lorenzo Gugliara | https://github.com/lgugliara
// License: MIT

namespace ZoloSim.EngineCore
{
    // This component aggregates all the key parts of the vehicle
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Engine))]
    [RequireComponent(typeof(Transmission))]
    [RequireComponent(typeof(Steer))]
    [RequireComponent(typeof(Brake))]
    [RequireComponent(typeof(Handbrake))]
    public class Vehicle : MonoBehaviour, IVehicle
    {
        // Rigidbody reference (for physics-based operations)
        public Rigidbody rb => GetComponent<Rigidbody>();

        // Returns the current velocity vector of the vehicle in world space
        public Vector3 velocity => rb.linearVelocity;

        [Header("Main components")]

        #region Engine
        // Core drivetrain components
        public Engine engine => GetComponent<Engine>();
        public Transmission transmission => GetComponent<Transmission>();
        #endregion

        #region Wheels
        // All Wheel components found in children
        public Wheel[] wheels => GetComponentsInChildren<Wheel>();

        // Steering system
        public Steer steer => GetComponent<Steer>();

        // Main brake system
        public Brake brake => GetComponent<Brake>();

        // Secondary brake system (usually rear wheels)
        public Handbrake handbrake => GetComponent<Handbrake>();
        #endregion

        [Header("Data")]

        // Vehicle configuration data (weight, dimensions, etc.)
        public VehicleData data;

        // Input control values (to be used by controllers or agents)
        [Header("I/O Controls")]

        // Throttle input (0 to 1)
        public float _throttle { get; set; } = 0f;

        // Steering input (-1 to 1)
        public float _steer { get; set; } = 0f;

        // Brake input (0 to 1)
        public float _brake { get; set; } = 0f;

        // Handbrake input (0 to 1)
        public float _handbrake { get; set; } = 0f;

        // Clutch engagement input (0 = disengaged, 1 = fully engaged)
        public float _clutchEngagement { get; set; } = 1f;

        /*
            Resets the state of the vehicle and all attached parts.
            Useful for reinitializing the simulation or restarting from a known state.
            This calls the Reset() method on all components derived from Part.
        */
        public virtual void Reset()
        {
            foreach (var part in GetComponents<Part>())
                part.Reset();
        }
    }
}
