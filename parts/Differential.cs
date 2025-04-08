using UnityEngine;
using System.Collections;
using System.Linq;

// ZoloSim Engine Core - © 2025 Lorenzo Gugliara
// Author: Lorenzo Gugliara | https://github.com/lgugliara
// License: MIT

namespace ZoloSim.EngineCore
{
    public class Differential : Part
    {
        public Transmission transmission => GetComponent<Transmission>();

        public DifferentialType differentialType = DifferentialType.Open;

        public DifferentialPosition differentialPosition;

        public Wheel[] wheels;

        public float Torque = 0f;

        public float MotorTorque => wheels.Sum(w => w.wc.MotorTorque);
        public float BrakeTorque => wheels.Sum(w => w.wc.BrakeTorque);

        void FixedUpdate()
        {
            if (!IsMotor())
                return;

            var differentialsCount = transmission.GetMotorDifferentials.Length;
            Torque = transmission.torque / differentialsCount;

            switch (differentialType)
            {
                case DifferentialType.Open:
                    OpenDistribution();
                    break;
                case DifferentialType.LimitedSlip:
                    LimitedSlipDistribution();
                    break;
                case DifferentialType.Locked:
                    LockedDistribution();
                    break;
            }
        }

        private void OpenDistribution()
        {

        }

        private void LimitedSlipDistribution()
        {

        }

        private void LockedDistribution()
        {
            //var avgSpeed = angularVelocity;
            var avgTorque = Torque / wheels.Length;

            foreach (var w in wheels)
            {
                //w.wheelCollider.rotationSpeed = avgSpeed;
                w.wc.MotorTorque = Mathf.Max(0f, avgTorque);
                w.wc.BrakeTorque = Mathf.Min(0f, avgTorque);
            }
        }

        public bool IsMotor() =>
            (differentialPosition == DifferentialPosition.Front && data.transmission.tractionType == TractionType.FWD) ||
            (differentialPosition == DifferentialPosition.Rear && data.transmission.tractionType == TractionType.RWD) ||
            data.transmission.tractionType == TractionType.AWD;
    }
}