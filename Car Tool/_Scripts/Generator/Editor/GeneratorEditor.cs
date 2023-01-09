#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace CarCup
{
    [CustomEditor(typeof(Generator), true)]
    public class GeneratorEditor : Editor
    {
        private Generator generator;

        private void OnEnable() => generator = target as Generator;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space(25);

            EditorHelper.GuiLine();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Generate"))
                generator.Generate();

            if (GUILayout.Button("Clear"))
                generator.Clear();

            EditorGUILayout.EndHorizontal();

            EditorHelper.GuiLine();
        }
    }
}
#endif