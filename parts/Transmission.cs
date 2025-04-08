using UnityEngine;
using System.Linq;
using ZoloSim.EngineCore.Enums;
using System.Collections;

// ZoloSim Engine Core - © 2025 Lorenzo Gugliara
// Author: Lorenzo Gugliara | https://github.com/lgugliara
// License: MIT

namespace ZoloSim.EngineCore
{
    public class Transmission : Part
    {
        #region Config
        [Header("Differentials and Wheels")]
        public Differential[] differentials => GetComponents<Differential>();
        public Wheel[] wheels => GetComponents<Differential>().SelectMany(d => d.wheels).ToArray();
        public Differential[] GetMotorDifferentials => differentials.Where(d => d.IsMotor()).ToArray();
        public Wheel[] GetMotorWheels =>
            GetMotorDifferentials
                .SelectMany(d => d.wheels)
                .ToArray();

        [Header("Gears and Ratios")]
        public bool IsNeutral => Gear == 0;
        public bool IsReverse => Gear == -1;
        public int Gear { get; private set; } = 0;
        public float GearRatio => IsReverse ? -data.transmission.reverseRatio : (IsNeutral ? 0 : data.transmission.gearRatios[Gear - 1]);
        public float FinalRatio => IsNeutral ? 0f : GearRatio * data.transmission.differentialRatio;
        #endregion

        #region Automatic Transmission
        public TransmissionType transmissionType = TransmissionType.Manual;
        [Range(4, 64)]
        public int clutchResolution = 8;
        public float shiftDelay = 0.1f;
        TransmissionAlert _transmissionAlert
        {
            get
            {
                if (!IsShifting && !IsNeutral && car._clutchEngagement > 0.9f && Gear < data.transmission.gearRatios.Length && car.engine.rpm > data.engine.maxRPM - (data.engine.maxRPM + data.engine.minRPM) * 0.25f)
                    return TransmissionAlert.ShiftUpRequired;
                else if (!IsShifting && IsNeutral && Gear < data.transmission.gearRatios.Length && car.engine.rpm > data.engine.minRPM + (data.engine.maxRPM + data.engine.minRPM) * 0.25f)
                    return TransmissionAlert.ShiftUpRequired;
                else if (!IsShifting && Gear > 1 && car.engine.rpm < data.engine.minRPM + (data.engine.maxRPM + data.engine.minRPM) * 0.2f)
                    return TransmissionAlert.ShiftDownRequired;
                else if (!IsShifting && !IsNeutral && car.engine.rpm < data.engine.minRPM && car._throttle < 0.1f)
                    return TransmissionAlert.NeutralRequired;
                else
                    return TransmissionAlert.None;
            }
        }

        public TransmissionAlert CurrentTransmissionAlert { get; set; }
        public bool IsShifting { get; set; } = false;
        #endregion

        #region Runtime variables
        public float angularMomentum { get; set; }
        public float torque { get; set; }
        #endregion

        #region Properties
        public float angularVelocity => car.wheels.Average(w => w.wc.AngularVelocity) * FinalRatio;
        public float rpm => car.wheels.Average(w => w.wc.wheel.rpm) * FinalRatio;
        public float horsePower => angularMomentum * rpm / 745.7f;
        #endregion

        void FixedUpdate()
        {
            // Apply torque to wheels
            var direction = IsReverse ? -1 : 1;
            float torquePerWheel = torque * FinalRatio / car.wheels.Length;
            var motorTorque = direction * Mathf.Max(direction * torquePerWheel, 0);
            var brakeTorque = IsReverse ?
                Mathf.Max(torquePerWheel, 0) :
                -Mathf.Min(torquePerWheel, 0);

            foreach (var wheel in car.wheels)
            {
                wheel.wc.MotorTorque = motorTorque;
                wheel.wc.BrakeTorque = brakeTorque;
            }

            angularMomentum -= data.transmission.transmissionLosses * angularMomentum * Time.fixedDeltaTime;

            // Update transmission alert
            CurrentTransmissionAlert = _transmissionAlert;
        }

        public override void Reset()
        {
            torque = 0;
            angularMomentum = 0;
            ShiftNeutral();
        }

        public float GetInertia() => car.wheels.Sum(w => w.wc.wheel.inertia);

        #region Gear Shifting
        public void ShiftUp()
        {
            if (!IsNeutral)
                Gear = Mathf.Clamp(Gear + 1, -1, data.transmission.gearRatios.Length);
            else
                Gear = 1;
        }
        public void ShiftDown()
        {
            if (!IsNeutral)
                Gear = Mathf.Clamp(Gear - 1, -1, data.transmission.gearRatios.Length);
            else
                Gear = -1;
        }
        public void ShiftNeutral()
        {
            Gear = 0;
        }
        #endregion
    }
}