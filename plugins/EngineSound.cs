using UnityEngine;

// ZoloSim Engine Core - © 2025 Lorenzo Gugliara
// Author: Lorenzo Gugliara | https://github.com/lgugliara
// License: MIT

namespace ZoloSim.EngineCore
{
    [RequireComponent(typeof(Engine))]
    [RequireComponent(typeof(ChuckSubInstance))]
    public class EngineSound : Part
    {
#region Chuck VST
        string chuckCode = @"
                // Variabili globali
                0 => global int isIgnited;
                0 => global float mainGain;
                0 => global float rpm;
                0.5 => global float crankshaftPositionFactor;
                1 => global int numCylinders;
                0 => global float throttle;
                1 => global float compRatio;
                1000 => global float lpfValue;
                5000 => global float hpfValue;

                // Oscillatori armonici
                SinOsc baseOsc => Gain g => dac;
                SinOsc harmonic0 => g;
                SinOsc harmonic1 => g;
                SinOsc harmonic2 => g;

                while (true)
                {
                    if(isIgnited == 1)
                    {
                        // Calcola una volta le variabili
                        numCylinders * crankshaftPositionFactor => float burnRate;
                        numCylinders / crankshaftPositionFactor => float gainRate;
                        throttle * (compRatio - 1.0) + 1.0 => float compThrottle;

                        // Frequenza del suono basata su RPM e numero di cilindri
                        (rpm / 120.0) * burnRate => float baseFreq;
                        baseFreq * (1.00 + Math.random2f(-0.05, 0.05)) => baseOsc.freq;
                        baseFreq * (0.50 + Math.random2f(-0.1, 0.1)) => harmonic0.freq;
                        baseFreq * (2.00 + Math.random2f(-0.1, 0.1)) => harmonic1.freq;
                        baseFreq * (4.00 + Math.random2f(-0.1, 0.1)) => harmonic2.freq;

                        // Riduci il gain delle armoniche
                        Math.sqrt(gainRate * compThrottle) => float throttleGain;
                        baseOsc.gain(0.1 * throttleGain);
                        harmonic0.gain(0.06 * throttleGain);
                        harmonic1.gain(0.03 * throttleGain);
                        harmonic2.gain(0.02 * throttleGain);
                    }

                    g.gain(mainGain);
                    
                    10::ms => now;
                }
            ";
#endregion

#region Properties
        ChuckSubInstance chuckInstance;
#endregion

#region Getters
        float crankshaftPositionFactor => car.data.engine.crankshaftPosition == CrankshaftPosition.Flatplane ? 0.5f : 1f;
        int numCylinders => car.data.engine.cylinders;
        float compRatio => car.data.engine.compressionRatio;
        float lowPassFilter => 1000f;
        float highPassFilter => 5000f;

        // Runtime values
        bool isInitialized;
        int isIgnited => car.engine.ignition.IsIgnited ? 1 : 0;
        float mainGain = 0f;
        float rpm => car.engine.rpm;
        float throttle => car.engine.ignition.IgnitionLevel;
#endregion

        void Start()
        {
            chuckInstance = GetComponent<ChuckSubInstance>();
            if(chuckInstance?.chuckMainInstance == null)
                return;

            chuckInstance.RunCode(chuckCode);
            SetInitialValues();
        }

        void Update()
        {
            if(chuckInstance?.chuckMainInstance == null)
                return;

            SetRuntimeValues();
        }

        void SetInitialValues()
        {
            chuckInstance.SetFloat("crankshaftPositionFactor", crankshaftPositionFactor);
            chuckInstance.SetInt("numCylinders", numCylinders);
            chuckInstance.SetFloat("compRatio", compRatio);
            chuckInstance.SetFloat("lpfValue", lowPassFilter);
            chuckInstance.SetFloat("hpfValue", highPassFilter);
        }

        void SetRuntimeValues()
        {
            if(isIgnited == 1)
                mainGain = Mathf.Lerp(mainGain, 1f, 0.5f);
            else
                mainGain = Mathf.Lerp(mainGain, 0f, 0.5f);

            chuckInstance.SetInt("isIgnited", isIgnited);
            chuckInstance.SetFloat("mainGain", mainGain);
            chuckInstance.SetFloat("rpm", rpm);
            chuckInstance.SetFloat("throttle", throttle);
            chuckInstance.SetInt("numCylinders", numCylinders);
        }
    }
}