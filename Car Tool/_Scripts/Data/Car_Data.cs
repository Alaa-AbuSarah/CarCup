using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarCup
{
    [CreateAssetMenu(fileName = "Car_Data", menuName = "Car Engine/Car Data", order = 1)]
    public class Car_Data : ScriptableObject
    {
        public float motorForce = 1200f;
        public float breakForce = 2000f;
        public float maxSteerAngle = 30f;
        public float maxSpeed = 80f;
        public AnimationCurve handlingMovement = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public AnimationCurve acceleration = AnimationCurve.EaseInOut(0, 0, 1, 1);
        public float mass = 2000f;

        public WheelColliderSettings fl_wheelColliderSettings = new WheelColliderSettings();
        public WheelColliderSettings fr_wheelColliderSettings = new WheelColliderSettings();
        public WheelColliderSettings rl_wheelColliderSettings = new WheelColliderSettings();
        public WheelColliderSettings rr_wheelColliderSettings = new WheelColliderSettings();

        public void ResetData()
        {
            motorForce = 1200f;
            breakForce = 2000f;
            maxSteerAngle = 30f;
            maxSpeed = 80f;
            handlingMovement = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
            acceleration = AnimationCurve.EaseInOut(0, 0, 1, 1);
            mass = 2000f;

            fl_wheelColliderSettings = new WheelColliderSettings();
            fr_wheelColliderSettings = new WheelColliderSettings();
            rl_wheelColliderSettings = new WheelColliderSettings();
            rr_wheelColliderSettings = new WheelColliderSettings();
        }
    }
}