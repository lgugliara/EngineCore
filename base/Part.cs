using UnityEngine;

// ZoloSim Engine Core - © 2025 Lorenzo Gugliara
// Author: Lorenzo Gugliara | https://github.com/lgugliara
// License: MIT

namespace ZoloSim.EngineCore
{
    public abstract class Part : MonoBehaviour, IPart
    {
        public virtual Vehicle car => GetComponent<Vehicle>() ?? GetComponentInParent<Vehicle>();
        public VehicleData data => car.data;

        public virtual void Apply() {}

        public virtual void Reset() {}
    }
}