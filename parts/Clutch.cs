using UnityEngine;
using System.Collections;
using System.Linq;

// ZoloSim Engine Core - © 2025 Lorenzo Gugliara
// Author: Lorenzo Gugliara | https://github.com/lgugliara
// License: MIT

namespace ZoloSim.EngineCore
{
    public class Clutch : Part
    {
        // Runtime variables
        float torque = 0;
        float maxTorque = 0, requiredTorque = 0, effectiveInertia = 0;

        float transmissionAngularVelocity => car.transmission.IsReverse ?
            -car.transmission.angularVelocity :
            car.transmission.angularVelocity;

        float deltaAngularVelocity =>
            car.transmission.IsNeutral ? 
                0 : 
                car.engine.angularVelocity - transmissionAngularVelocity * car.transmission.FinalRatio;

        void FixedUpdate()
        {
            float eng = car._clutchEngagement;

            maxTorque = GetMaxTorque();
            torque = car.transmission.IsNeutral ? 0 : eng * maxTorque;

            float engineMomentum = -torque * Time.fixedDeltaTime;
            float transmissionMomentum = -engineMomentum;
            float transmissionTorque = torque;

            car.transmission.torque = 0;
            if (torque != 0)
            {
                car.engine.angularMomentum += engineMomentum;
                car.transmission.angularMomentum += transmissionMomentum;
                car.transmission.torque = transmissionTorque;
            }
        }

        float GetMaxTorque()
        {
            var deltaW = deltaAngularVelocity;
            float engineInertia = data.engine.inertia;
            float transmissionInertia = car.transmission.GetInertia();
            effectiveInertia = (engineInertia * transmissionInertia) / (engineInertia + transmissionInertia);
            requiredTorque = effectiveInertia * Mathf.Abs(deltaW) / Time.fixedDeltaTime;
            float x = Mathf.Clamp01(data.engine.maxRPM / (Mathf.Abs(deltaW) + data.engine.maxRPM));

            float res = Mathf.Sign(deltaW) * Mathf.Min(requiredTorque, data.engine.clutch.maxTorque * x);
            return res;
        }

        public override void Reset()
        {
            torque = 0;
            maxTorque = 0;
            requiredTorque = 0;
            effectiveInertia = 0;
        }
    }
}