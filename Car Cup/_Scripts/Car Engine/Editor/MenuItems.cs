#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CarCup
{
    public class MenuItems : MonoBehaviour
    {
        // Add a menu item to create car GameObjects.
        [MenuItem("GameObject/Car Engine/Car", false, 1)]
        static void CreateNewCar()
        {
            GameObject car = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Car Cup/Prefab/Car.prefab");
            GameObject newCar = Instantiate(car, Vector3.zero, Quaternion.Euler(Vector3.zero));
            newCar.name = "Wonderful car";
            Undo.RegisterCreatedObjectUndo(newCar, "Create " + newCar.name);
            Selection.activeObject = newCar;

            CameraFollow cameraFollow = GameObject.FindObjectOfType<CameraFollow>();
            if (cameraFollow != null) cameraFollow.SetTarget(newCar.transform);
            else 
            {
                Camera.main.gameObject.AddComponent<CameraFollow>();
                CameraFollow newCameraFollow = Camera.main.GetComponent<CameraFollow>();
                newCameraFollow.SetTarget(newCar.transform);
            }
        }

        // Add a menu item to create Mobile Controls GameObjects.
        [MenuItem("GameObject/Car Engine/UI/Mobile Controls", false, 2)]
        static void CreateNewMobileControls()
        {
            GameObject controls = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Car Cup/Prefab/Mobile Controls.prefab");

            Transform canvas = GetCanvas();
            if (canvas != null)
            {
                GameObject newControls = Instantiate(controls, canvas);
                newControls.name = "Mobile Controls";
                Undo.RegisterCreatedObjectUndo(newControls, "Create " + newControls.name);
                Selection.activeObject = newControls;
            }
        }

        // Add a menu item to create Reset Button GameObjects.
        [MenuItem("GameObject/Car Engine/UI/Reset Button", false, 3)]
        static void CreateNewResetButton()
        {
            GameObject button = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Car Cup/Prefab/Reset.prefab");

            Transform canvas = GetCanvas();
            if (canvas != null)
            {
                GameObject newButton = Instantiate(button, canvas);
                newButton.name = "Reset";
                Undo.RegisterCreatedObjectUndo(newButton, "Create " + newButton.name);
                Selection.activeObject = newButton;
            }
        }

        // Add a menu item to create Speed Text GameObjects.
        [MenuItem("GameObject/Car Engine/UI/Speed Text", false, 4)]
        static void CreateNewSpeedText()
        {
            GameObject text = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Car Cup/Prefab/Speed.prefab");

            Transform canvas = GetCanvas();
            if (canvas != null)
            {
                GameObject newText = Instantiate(text, canvas);
                newText.name = "Speed";
                Undo.RegisterCreatedObjectUndo(newText, "Create " + newText.name);
                Selection.activeObject = newText;
            }
        }

        static Transform GetCanvas()
        {
            Canvas canvas = GameObject.FindObjectOfType<Canvas>();

            if (canvas != null) return canvas.transform;
            else
            {
                Debug.LogError("Ther is no canvas in scene");
                return null;
            }
        }
    }
}
#endif