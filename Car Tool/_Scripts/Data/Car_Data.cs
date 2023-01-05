using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTools
{
    [CreateAssetMenu(fileName = "Car_Data", menuName = "Car Engine/Car Data", order = 1)]
    public class Car_Data : ScriptableObject
    {
        public float motorForce = 1200f;
        public float breakForce = 2000f;
        public float maxSteerAngle = 30f;
        public float maxSpeed = 80f;
        public AnimationCurve handlingMovement = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public float mass = 2000f;

        public void ResetData()
        {
            motorForce = 1200f;
            breakForce = 2000f;
            maxSteerAngle = 30f;
            maxSpeed = 80f;
            handlingMovement = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
            mass = 2000f;
        }
    }
}