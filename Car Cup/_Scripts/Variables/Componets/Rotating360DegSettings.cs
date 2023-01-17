using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarCup
{
    [System.Serializable]
    public class Rotating360DegSettings
    {
        public bool active = false;

        public bool onGroundX = false;
        public bool onGroundY = true;
        public bool onGroundZ = false;
        
        public bool inAirX = false;
        public bool inAirY = false;
        public bool inAirZ = true;
        public float inAirDistance = 3f;

        public void ResetData()
        {
            active = false;

            onGroundX = false;
            onGroundY = true;
            onGroundZ = false;

            inAirX = false;
            inAirY = false;
            inAirZ = true;
            inAirDistance = 3f;
        }
    }
}