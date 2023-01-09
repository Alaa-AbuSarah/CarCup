using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarCup
{
    [System.Serializable]
    public class WheelColliderSettings
    {
        public float mass = 40f;
        public float radius = 0.3f;
        public float wheelDampingRate = 0.5f;
        public float suspensionDistance = 0.25f;
        public float forceAppPointDistance = 1f;

        [Space]

        public Vector3 center = Vector3.zero;

        [Header("Suspension Spring")]
        public float spring = 90000f;
        public float damper = 9000f;
        public float targetPosition = 0.5f;

        [Header("Forward Friction")]
        public float f_extremumSlip = 1.5f;
        public float f_extremumValue = 1.5f;
        public float f_asymptoteSlip = 2f;
        public float f_asymptoteValue = 1f;
        public float f_stiffness = 1.5f;

        [Header("Sideways Friction")]
        public float s_extremumSlip = 1.5f;
        public float s_extremumValue = 2f;
        public float s_asymptoteSlip = 1.8f;
        public float s_asymptoteValue = 1.5f;
        public float s_stiffness = 1.5f;
    }
}