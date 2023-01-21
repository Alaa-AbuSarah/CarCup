#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CarCup
{
    [CustomEditor(typeof(CarController))]
    public class CarControllerEditor:Editor
    {
        CarController _controller;

        private int layoutIndexOne = 1;
        private int layoutIndexTwo = 1;

        private void OnEnable() => _controller = target as CarController;

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Controlling")) layoutIndexOne = 1;
            if (GUILayout.Button("Wheels")) layoutIndexOne = 2;
            if (GUILayout.Button("Componets")) layoutIndexOne = 3;

            EditorGUILayout.EndHorizontal();

            switch (layoutIndexOne)
            {
                case 1:

                    EditorGUILayout.Space();

                    ControllingLayout();

                    break;
//-------------------------------------------------------------------------------------
                case 2:

                    EditorGUILayout.Space();

                    WheelsLayout();

                    break;
//-------------------------------------------------------------------------------------
                case 3:

                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();

                    if (GUILayout.Button("Suond")) layoutIndexTwo = 1;
                    if (GUILayout.Button("Rotating")) layoutIndexTwo = 2;

                    EditorGUILayout.EndHorizontal();

                    ComponetsLayout();

                    break;
            }

            EditorGUILayout.Space(15);
            EditorGUILayout.LabelField($"{layoutIndexOne},{layoutIndexTwo}");
        }

        private void ControllingLayout() 
        {
            _controller.motorForce = EditorGUILayout.FloatField("Motor Force", _controller.motorForce);
            _controller.breakForce = EditorGUILayout.FloatField("Break Force", _controller.breakForce);
            _controller.maxSteerAngle = EditorGUILayout.FloatField("Max Steer Angle", _controller.maxSteerAngle);
            _controller.maxSpeed = EditorGUILayout.FloatField("Max Speed", _controller.maxSpeed);

            EditorGUILayout.Space();

            _controller.handlingMovement = EditorGUILayout.CurveField("Handling", _controller.handlingMovement);
            _controller.acceleration = EditorGUILayout.CurveField("Acceleration", _controller.acceleration);
        }
        private void WheelsLayout() 
        {
            EditorGUILayout.LabelField("Front Left");
            EditorGUILayout.BeginHorizontal();
            _controller.frontLeftWheelTransform = EditorGUILayout.ObjectField(_controller.frontLeftWheelTransform, typeof(Transform), true) as Transform;
            _controller.frontLeftWheelCollider = EditorGUILayout.ObjectField(_controller.frontLeftWheelCollider, typeof(WheelCollider), true) as WheelCollider;
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Front Right");
            EditorGUILayout.BeginHorizontal();
            _controller.frontRightWheeTransform = EditorGUILayout.ObjectField(_controller.frontRightWheeTransform, typeof(Transform), true) as Transform;
            _controller.frontRightWheelCollider = EditorGUILayout.ObjectField(_controller.frontRightWheelCollider, typeof(WheelCollider), true) as WheelCollider;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Rear Left");
            EditorGUILayout.BeginHorizontal();
            _controller.rearLeftWheelTransform = EditorGUILayout.ObjectField(_controller.rearLeftWheelTransform, typeof(Transform), true) as Transform;
            _controller.rearLeftWheelCollider = EditorGUILayout.ObjectField(_controller.rearLeftWheelCollider, typeof(WheelCollider), true) as WheelCollider;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Rear Right");
            EditorGUILayout.BeginHorizontal();
            _controller.rearRightWheelTransform = EditorGUILayout.ObjectField(_controller.rearRightWheelTransform, typeof(Transform), true) as Transform;
            _controller.rearRightWheelCollider = EditorGUILayout.ObjectField(_controller.rearRightWheelCollider, typeof(WheelCollider), true) as WheelCollider;
            EditorGUILayout.EndHorizontal();
        }
        private void ComponetsLayout() 
        {
            switch (layoutIndexTwo)
            {
                case 1:
                    SuondLayout();
                    break;
                case 2:
                    RotatingLayout();
                    break;
            }
        }
        private void SuondLayout() 
        {
            _controller.motorSuondSettings_active = EditorGUILayout.Toggle("Active", _controller.motorSuondSettings_active);

            EditorGUILayout.Space();

            _controller.motorSuondSettings_idle = EditorGUILayout.ObjectField("Idle",_controller.motorSuondSettings_idle, typeof(AudioClip), false) as AudioClip;
            _controller.motorSuondSettings_idleVolume = EditorGUILayout.Slider(_controller.motorSuondSettings_idleVolume, 0, 1);
            EditorGUILayout.Space();

            _controller.motorSuondSettings_low = EditorGUILayout.ObjectField("Idle", _controller.motorSuondSettings_low, typeof(AudioClip), false) as AudioClip;
            _controller.motorSuondSettings_lowVolume = EditorGUILayout.Slider(_controller.motorSuondSettings_lowVolume, 0, 1);
            EditorGUILayout.Space();

            _controller.motorSuondSettings_mid = EditorGUILayout.ObjectField("Idle", _controller.motorSuondSettings_mid, typeof(AudioClip), false) as AudioClip;
            _controller.motorSuondSettings_midVolume = EditorGUILayout.Slider(_controller.motorSuondSettings_midVolume, 0, 1);
            EditorGUILayout.Space();

            _controller.motorSuondSettings_high = EditorGUILayout.ObjectField("Idle", _controller.motorSuondSettings_high, typeof(AudioClip), false) as AudioClip;
            _controller.motorSuondSettings_highVolume = EditorGUILayout.Slider(_controller.motorSuondSettings_highVolume, 0, 1);
            EditorGUILayout.Space();
        }
        private void RotatingLayout() 
        {
            _controller.rotating360DegSettings_active = EditorGUILayout.Toggle("Active", _controller.rotating360DegSettings_active);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("On Ground");
            _controller.rotating360DegSettings_onGroundX = EditorGUILayout.Toggle("X", _controller.rotating360DegSettings_onGroundX);
            _controller.rotating360DegSettings_onGroundY = EditorGUILayout.Toggle("Y", _controller.rotating360DegSettings_onGroundY);
            _controller.rotating360DegSettings_onGroundZ = EditorGUILayout.Toggle("Z", _controller.rotating360DegSettings_onGroundZ);
            
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("In Air");
            _controller.rotating360DegSettings_inAirX = EditorGUILayout.Toggle("X", _controller.rotating360DegSettings_inAirX);
            _controller.rotating360DegSettings_inAirY = EditorGUILayout.Toggle("Y", _controller.rotating360DegSettings_inAirY);
            _controller.rotating360DegSettings_inAirZ = EditorGUILayout.Toggle("Z", _controller.rotating360DegSettings_inAirZ);
            _controller.rotating360DegSettings_inAirDistance = EditorGUILayout.Slider("Distance", _controller.rotating360DegSettings_inAirDistance, 0.5f, 10f);
        }

    }
}
#endif