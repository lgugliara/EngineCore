using UnityEngine;
using System.Linq;

// ZoloSim Engine Core - © 2025 Lorenzo Gugliara
// Author: Lorenzo Gugliara | https://github.com/lgugliara
// License: MIT

namespace ZoloSim.EngineCore
{
    public class Handbrake : Part
    {
        void FixedUpdate()
        {
            // Applica la coppia frenante alle ruote posteriori
            foreach (Wheel wheel in car.wheels.Where(w => w.transform.localPosition.z < 0))
            {                
                wheel.wc.BrakeTorque += car._handbrake * data.handbrake.brakeForce;

                // Draw a dev line from the wheel straight up 2 units
                Debug.DrawLine(wheel.transform.position, wheel.transform.position + Vector3.up * 2 * car._handbrake, Color.red);
            }
        }
    }
}