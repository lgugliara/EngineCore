using UnityEngine;
using System.Collections;
using System.Linq;

// ZoloSim Engine Core - © 2025 Lorenzo Gugliara
// Author: Lorenzo Gugliara | https://github.com/lgugliara
// License: MIT

namespace ZoloSim.EngineCore
{
    public class Ignition : Part
    {
        public Transmission transmission => GetComponent<Transmission>();

        public bool IsIgnited { get; set; }
        public bool IsIgniting { get; private set; }

        [Range(0f, 1f)]
        public float IgnitionLevel { get; private set; }

        [Range(0f, 1f)]
        float MinimumThreshold = 0;
        float LimiterThreshold = 1;
        float TCSLimiter = 1;

        void FixedUpdate()
        {
            if (!IsIgniting && car.engine.rpm <= data.engine.minRPM * 0.5f)
                car.engine.ignition.IsIgnited = false;

            bool isIgnitedOrIgniting = car.engine.ignition.IsIgnited || IsIgniting;
            float availableInjection = data.engine.injection.injectionCurve.Evaluate(car.engine.rpm / data.engine.maxRPM);

            float injection = isIgnitedOrIgniting ? car._throttle * availableInjection : 0;

            if (isIgnitedOrIgniting)
            {
                // Minimum ignition
                if (car.engine.rpm < data.engine.minRPM)
                {
                    MinimumThreshold = Mathf.Lerp(MinimumThreshold, 1f, 0.1f);
                    injection = Mathf.Max(MinimumThreshold, injection);
                }
                else
                    MinimumThreshold = Mathf.Lerp(MinimumThreshold, 0f, 0.5f);

                // Limiter
                if (car.engine.rpm >= data.engine.maxRPM)
                {
                    LimiterThreshold = Mathf.Lerp(LimiterThreshold, 0f, 0.5f);
                    injection = Mathf.Min(LimiterThreshold, injection);
                }
                else
                    LimiterThreshold = Mathf.Lerp(LimiterThreshold, 1f, 0.1f);

                // Traction control system
                TCSLimiter = data.transmission.HasTCS && transmission.GetMotorWheels.Any(w => w.IsTRS) ?
                    Mathf.Lerp(TCSLimiter, 0f, 0.1f) :
                    TCSLimiter = Mathf.Lerp(TCSLimiter, 1f, 0.1f);

                IgnitionLevel *= TCSLimiter;
            }

            IgnitionLevel = injection;
        }

        public void Ignite()
        {
            if (!car.engine.ignition.IsIgnited && !IsIgniting)
                StartCoroutine(IgniteCoroutine());
        }

        private IEnumerator IgniteCoroutine()
        {
            IsIgniting = true;

            // check every 10ms if the engine is ignited for 2 seconds max
            for (int i = 0; i < 20; i++)
            {
                if (car.engine.rpm >= data.engine.minRPM)
                    break;

                yield return new WaitForSeconds(0.1f);
            }

            IsIgniting = false;
            car.engine.ignition.IsIgnited = true;
        }
    }
}