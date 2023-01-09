#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace CarCup
{
    [CustomEditor(typeof(Car_Data))]
    public class Car_DataEditor : Editor
    {
        private Car_Data _Data;

        private int WheelColliderSettingsFieldIndex = 0;

        private void OnEnable() => _Data = target as Car_Data;

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();
            EditorHelper.GuiLine(2);
            EditorGUILayout.LabelField("Controller", EditorHelper.LableStyle0004(), GUILayout.ExpandWidth(true));
            EditorHelper.GuiLine(2);
            EditorGUILayout.Space();

            _Data.mass = EditorGUILayout.Slider("Mass", _Data.mass, 500f, 5000f);
            _Data.motorForce = EditorGUILayout.Slider("Motor Force", _Data.motorForce, 1000f, 5000f);
            _Data.breakForce = EditorGUILayout.Slider("Break Force", _Data.breakForce, 1500f, 10000f);
            _Data.maxSpeed = EditorGUILayout.Slider("Max Speed", _Data.maxSpeed, 0f, 500f);

            Texture2D angleSteering = EditorHelper.GetTexture2DFromPath("Assets/Car Cup/Demo Assets/Icons/Steering.png");
            Texture2D angleCursor = EditorHelper.GetTexture2DFromPath("Assets/Car Cup/Demo Assets/Icons/Angle Cursor.png");
            _Data.maxSteerAngle = EditorHelper.GuiAngle(new Rect(Screen.width / 2 - 40, 150, 128, 128), _Data.maxSteerAngle, 1f, 90f, angleSteering, angleCursor, "Max Steer Angle", true);

            EditorGUILayout.Space(170);

            _Data.handlingMovement = EditorGUILayout.CurveField("Handling", _Data.handlingMovement);
            _Data.acceleration = EditorGUILayout.CurveField("Acceleration", _Data.acceleration);

            EditorGUILayout.Space(15);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("FL")) WheelColliderSettingsFieldIndex = (WheelColliderSettingsFieldIndex == 1) ? 0 : 1;
            if (GUILayout.Button("FR")) WheelColliderSettingsFieldIndex = (WheelColliderSettingsFieldIndex == 2) ? 0 : 2;
            if (GUILayout.Button("RL")) WheelColliderSettingsFieldIndex = (WheelColliderSettingsFieldIndex == 3) ? 0 : 3;
            if (GUILayout.Button("RR")) WheelColliderSettingsFieldIndex = (WheelColliderSettingsFieldIndex == 4) ? 0 : 4;

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(15);

            switch (WheelColliderSettingsFieldIndex)
            {
                case 1:
                    _Data.fl_wheelColliderSettings = EditorHelper.WheelColliderSettingsField(_Data.fl_wheelColliderSettings, "Front Left");
                    break;
                case 2:
                    _Data.fr_wheelColliderSettings = EditorHelper.WheelColliderSettingsField(_Data.fr_wheelColliderSettings, "Front Right");
                    break;
                case 3:
                    _Data.rl_wheelColliderSettings = EditorHelper.WheelColliderSettingsField(_Data.rl_wheelColliderSettings, "Rear Left");
                    break;
                case 4:
                    _Data.rr_wheelColliderSettings = EditorHelper.WheelColliderSettingsField(_Data.rr_wheelColliderSettings, "Rear Right");
                    break;
            }

            EditorGUILayout.Space(50);
            EditorHelper.GuiLine(2);
            if (GUILayout.Button("Reset")) _Data.ResetData();
            EditorHelper.GuiLine(2);
        }
    }
}
#endif