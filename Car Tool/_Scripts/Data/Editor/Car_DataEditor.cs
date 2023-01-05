#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace CarTools
{
    [CustomEditor(typeof(Car_Data))]
    public class Car_DataEditor : Editor
    {
        private Car_Data _Data;

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

            Texture2D angleSteering = EditorHelper.GetTexture2DFromPath("Assets/Car Tool/Demo Assets/Icons/Steering.png");
            Texture2D angleCursor = EditorHelper.GetTexture2DFromPath("Assets/Car Tool/Demo Assets/Icons/Angle Cursor.png");
            _Data.maxSteerAngle = EditorHelper.GuiAngle(new Rect(Screen.width / 2 - 40, 170, 128, 128), _Data.maxSteerAngle, 1f, 90f, angleSteering, angleCursor, "Max Steer Angle", true);

            EditorGUILayout.Space(200);
            _Data.handlingMovement = EditorGUILayout.CurveField("Handling Movement", _Data.handlingMovement);

            EditorGUILayout.Space(50);
            if (GUILayout.Button("Reset")) _Data.ResetData();
        }
    }
}
#endif