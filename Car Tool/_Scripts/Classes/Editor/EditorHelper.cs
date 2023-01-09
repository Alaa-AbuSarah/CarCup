#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CarCup
{
    public static class EditorHelper
    {
        //
        // Summary:
        //     Make a layer WheelFrictionCurve field.
        //
        // Parameters:
        //   label:
        //     Optional label in top of the field.
        //
        //   wheelFrictionCurve:
        //     The wheelFrictionCurve shown in the field.
        //
        // Returns:
        //     The WheelFrictionCurve updated by the user.
        public static WheelFrictionCurve WheelFrictionCurveField(WheelFrictionCurve wheelFrictionCurve, string label = " ")
        {
            WheelFrictionCurve value = wheelFrictionCurve;

            EditorGUILayout.LabelField(label, LableStyle0002());

            EditorGUILayout.Space();

            value.extremumSlip = EditorGUILayout.FloatField("Extremum Slie", value.extremumSlip);
            if (value.extremumSlip < 0.001f) value.extremumSlip = 0.001f;

            value.extremumValue = EditorGUILayout.FloatField("Extremum Value", value.extremumValue);
            if (value.extremumValue < 0.001f) value.extremumValue = 0.001f;

            value.asymptoteSlip = EditorGUILayout.FloatField("Asymptote Slip", value.asymptoteSlip);
            if (value.asymptoteSlip < 0.001f) value.asymptoteSlip = 0.001f;

            value.asymptoteValue = EditorGUILayout.FloatField("Asymptote Value", value.asymptoteValue);
            if (value.asymptoteValue < 0.001f) value.asymptoteValue = 0.001f;

            value.stiffness = EditorGUILayout.FloatField("Stiffness", value.stiffness);
            if (value.stiffness < 0) value.stiffness = 0;

            return value;
        }
        //
        // Summary:
        //     Make a wheel collider settings field.
        //
        // Parameters:
        //   wheelFrictionCurve:
        //     The wheel collider settings shown in the field.
        //
        //   label:
        //     Optional label in top of the field.
        //
        // Returns:
        //     The wheel collider settings updated by the user.
        public static WheelColliderSettings WheelColliderSettingsField(WheelColliderSettings wheelColliderSettings, string label = " ")
        {
            WheelColliderSettings _settings = wheelColliderSettings;

            GuiLine();
            EditorGUILayout.LabelField(label, LableStyle0004());
            GuiLine();
            EditorGUILayout.Space();

            _settings.mass = EditorGUILayout.Slider("Mass", _settings.mass, 0.0001f, 1000f);
            _settings.radius = EditorGUILayout.Slider("Radius", _settings.radius, 0f, 100f);
            _settings.wheelDampingRate = EditorGUILayout.Slider("Wheel Damping Rate", _settings.wheelDampingRate, 0.0001f, 10f);
            _settings.suspensionDistance = EditorGUILayout.Slider("Suspension Distance", _settings.suspensionDistance, 0, 10f);
            _settings.forceAppPointDistance = EditorGUILayout.Slider("Force App Point Distance", _settings.forceAppPointDistance, -10f, 10f);

            EditorGUILayout.Space();

            _settings.center = EditorGUILayout.Vector3Field("Center", _settings.center);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Suspension Spring", LableStyle0002());
            _settings.spring = EditorGUILayout.Slider("Spring", _settings.spring, 0f, 1000000f);
            _settings.damper = EditorGUILayout.Slider("Damper", _settings.damper, 0f, 1000000f);
            _settings.targetPosition = EditorGUILayout.Slider("Target Position", _settings.targetPosition, 0f, 10f);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Forward Friction", LableStyle0002());
            _settings.f_extremumSlip = EditorGUILayout.Slider("Fxtremum Slip", _settings.f_extremumSlip, 0.001f, 1000f);
            _settings.f_extremumValue = EditorGUILayout.Slider("ExtremumValue", _settings.f_extremumValue, 0.001f, 1000f);
            _settings.f_asymptoteSlip = EditorGUILayout.Slider("AsymptoteSlip", _settings.f_asymptoteSlip, 0.001f, 1000f);
            _settings.f_asymptoteValue = EditorGUILayout.Slider("AsymptoteValue", _settings.f_asymptoteValue, 0.001f, 1000f);
            _settings.f_stiffness = EditorGUILayout.Slider("Stiffness", _settings.f_stiffness, 0f, 1000f);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Sideways Friction", LableStyle0002());
            _settings.s_extremumSlip = EditorGUILayout.Slider("Fxtremum Slip", _settings.s_extremumSlip, 0.001f, 1000f);
            _settings.s_extremumValue = EditorGUILayout.Slider("ExtremumValue", _settings.s_extremumValue, 0.001f, 1000f);
            _settings.s_asymptoteSlip = EditorGUILayout.Slider("AsymptoteSlip", _settings.s_asymptoteSlip, 0.001f, 1000f);
            _settings.s_asymptoteValue = EditorGUILayout.Slider("AsymptoteValue", _settings.s_asymptoteValue, 0.001f, 1000f);
            _settings.s_stiffness = EditorGUILayout.Slider("Stiffness", _settings.s_stiffness, 0f, 1000f);

            return _settings;
        }
        //
        // Summary:
        //     Draw a horizontal line.
        //
        public static void GuiLine(int i_height = 1)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, i_height);
            rect.height = i_height;
            EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        }
        //
        // Summary:
        //     Make a angle editor tool with slider in right.
        //
        // Parameters:
        //   rect:
        //     rect for draw position.
        //
        //   value:
        //     the value of tool.
        //
        //   min and max:
        //     for claming value.
        //
        //   background:
        //     image behind the angle.
        //
        //   cursor:
        //     image draw the angle.
        //
        //   label:
        //     Optional label in left of the field.
        //
        //   drawValue:
        //     Optional for draw the tool value.
        //
        // Returns:
        //     The modified value by user.
        public static float GuiAngle(Rect rect, float value, float min, float max, Texture2D background, Texture2D cursor, string label = " ", bool drawValue = false)
        {
            float _value = value;

            Rect valueLabelRect = rect;
            valueLabelRect.x -= rect.width - 30;

            Rect labelRect = rect;
            labelRect.y -= 15;
            labelRect.x -= rect.width;

            Rect sliderRect = rect;
            sliderRect.x += rect.width;

            GUI.DrawTexture(rect, background);

            Matrix4x4 matrix = GUI.matrix;
            GUIUtility.RotateAroundPivot(_value, rect.center);
            GUI.DrawTexture(rect, cursor);
            GUI.matrix = matrix;

            if (drawValue)
                GUI.Label(valueLabelRect, _value.ToString("F2"), LableStyle0003());

            GUI.Label(labelRect, label, LableStyle0003());

            _value = GUI.VerticalSlider(sliderRect, _value, max, min);

            return _value;
        }
        public static GUIStyle LableStyle0001 => new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
        public static GUIStyle LableStyle0002()
        {
            GUIStyle lableStyle = new GUIStyle();

            lableStyle.normal.textColor = Color.white;
            lableStyle.fontStyle = FontStyle.Bold;
            lableStyle.fontSize = 12;
            lableStyle.alignment = TextAnchor.UpperLeft;

            return lableStyle;
        }
        public static GUIStyle LableStyle0003()
        {
            GUIStyle lableStyle = new GUIStyle();

            lableStyle.normal.textColor = Color.white;
            lableStyle.fontStyle = FontStyle.Normal;
            lableStyle.fontSize = 15;
            lableStyle.alignment = TextAnchor.MiddleLeft;

            return lableStyle;
        }
        public static GUIStyle LableStyle0004()
        {
            GUIStyle lableStyle = new GUIStyle();

            lableStyle.normal.textColor = Color.white;
            lableStyle.fontStyle = FontStyle.Bold;
            lableStyle.fontSize = 15;
            lableStyle.alignment = TextAnchor.UpperCenter;

            return lableStyle;
        }
        public static GUIStyle LableStyle0005()
        {
            GUIStyle lableStyle = new GUIStyle();

            lableStyle.normal.textColor = Color.white;
            lableStyle.fontStyle = FontStyle.Bold;
            lableStyle.fontSize = 12;
            lableStyle.alignment = TextAnchor.UpperCenter;

            return lableStyle;
        }
        public static Texture2D GetTexture2DFromPath(string path)
        {
            string filename = path;
            var rawData = System.IO.File.ReadAllBytes(filename);
            Texture2D tex = new Texture2D(2, 2); // Create an empty Texture size doesn't matter (she said)
            tex.LoadImage(rawData);

            return tex;
        }
    }
}
#endif