using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarCup
{
    [System.Serializable]
    public class MotorSuondSettings
    {
        public bool active = false;

        public AudioClip idle;
        public float idleVolume = 0.15f;

        public AudioClip low;
        public float lowVolume = 0.25f;

        public AudioClip mid;
        public float midVolume = 0.5f;

        public AudioClip high;
        public float highVolume = 0.75f;

        public void ResetData()
        {
            active = false;

            idle = null;
            idleVolume = 0.15f;

            low = null;
            lowVolume = 0.25f;

            mid = null;
            midVolume = 0.5f;

            high = null;
            highVolume = 0.75f;
        }
    }
}