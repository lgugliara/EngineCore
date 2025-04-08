using UnityEngine;
using System.Linq;

// ZoloSim Engine Core - © 2025 Lorenzo Gugliara
// Author: Lorenzo Gugliara | https://github.com/lgugliara
// License: MIT

namespace ZoloSim.EngineCore
{
    public class Steer : Part
    {
        void FixedUpdate()
        {
            // Applica l'angolo di sterzo alle ruote anteriori
            foreach (var w in car.wheels.Where(w => w.transform.localPosition.z > 0))
                w.wc.SteerAngle = car._steer * data.steering.steerAngle;
        }
    }
}
