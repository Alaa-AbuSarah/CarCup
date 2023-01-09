using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarCup
{
    public class RingGenerator : Generator
    {
        [SerializeField] [Range(3, 256)] private int anglesCount = 5;
        [SerializeField] [Range(0.0001f, 25)] private float radius = 1f;

        public override void Generate()
        {
            name = "Ring"; //Set Mesh Name

            //----Calculation Vertices-----------------------------
            List<Vector3> points = new List<Vector3>();
            float circumferenceProgressPerStep = 1 / (float)anglesCount;
            float TAU = 2 * Mathf.PI;
            float radianProgressPerStep = circumferenceProgressPerStep * TAU;

            for (int i = 0; i < anglesCount; i++)
            {
                float currentRadian = radianProgressPerStep * i;
                points.Add(new Vector3
                        (
                            Mathf.Cos(currentRadian) * radius,
                            Mathf.Sin(currentRadian) * radius,
                            0
                        )
                    );
            }

            vertices = points.ToArray();
            //-----------------------------------------------------

            //----Calculation Triangles----------------------------
            int triangleAmount = points.Count - 2;
            List<int> trianglesList = new List<int>();
            for (int i = 0; i < triangleAmount; i++)
            {
                trianglesList.Add(i + 1);
                trianglesList.Add(i + 2);
                trianglesList.Add(0);
            }

            triangles = trianglesList.ToArray();
            //-----------------------------------------------------

            //----Calculation Normals------------------------------
            List<Vector3> normalsList = new List<Vector3>();
            for (int i = 0; i < points.Count; i++)
            {
                normalsList.Add(transform.forward);
            }
            normals = normalsList.ToArray();
            //-----------------------------------------------------

            //----Calculation UVMap--------------------------------
            List<Vector2> uvList = new List<Vector2>();
            for (int i = 0; i < points.Count; i++)
            {
                uvList.Add(points[i]);
            }
            uv = uvList.ToArray();
            //-----------------------------------------------------

            base.Generate();
        }
    }
}