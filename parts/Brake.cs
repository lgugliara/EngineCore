using UnityEngine;
using System.Linq;

// ZoloSim Engine Core - © 2025 Lorenzo Gugliara
// Author: Lorenzo Gugliara | https://github.com/lgugliara
// License: MIT

namespace ZoloSim.EngineCore
{
    public class Brake : Part
    {
        void FixedUpdate()
        {
            // Applica la coppia frenante alle ruote
            foreach (Wheel wheel in car.wheels)
            {
                // Rear wheels
                if (wheel.transform.localPosition.z < 0)
                {
                    var brakeTorque = car._brake * data.brake.brakeForce * data.brake.frontRearBalance;
                    wheel.wc.BrakeTorque += brakeTorque;
                }

                // Front wheels
                else if (wheel.transform.localPosition.z > 0)
                {
                    var brakeTorque = car._brake * data.brake.brakeForce * (1 - data.brake.frontRearBalance);
                    wheel.wc.BrakeTorque += brakeTorque;
                }

                // Draw a dev line from the wheel straight up 2 units
                Debug.DrawLine(wheel.transform.position, wheel.transform.position + Vector3.up * 2 * car._brake, Color.blue);
            }
        }
    }
}