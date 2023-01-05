using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTools
{
    public class QuadGenerator : Generator
    {
        [SerializeField] private float width = 1f;
        [SerializeField] private float height = 1f;

        public override void Generate()
        {
            name = "Quad"; //Set Mesh Name


            //----Calculation Vertices-----------------------------
            float halfHeight = height * 0.5f;
            float halfWidth = width * 0.5f;

            vertices = new Vector3[4]
            {
            new Vector3 (-halfWidth, -halfHeight, 0),
            new Vector3(-halfWidth, halfHeight, 0),
            new Vector3(halfWidth, -halfHeight, 0),
            new Vector3(halfWidth, halfHeight, 0)
            };
            //-----------------------------------------------------

            //----Calculation Triangles----------------------------
            triangles = new int[6]
            {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            2, 3, 1
            };
            //-----------------------------------------------------

            //----Calculation Normals------------------------------
            normals = new Vector3[4]
            {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
            };
            //-----------------------------------------------------

            //----Calculation UVMap--------------------------------
            uv = new Vector2[4]
            {
              new Vector2(0, 0),
              new Vector2(1, 0),
              new Vector2(0, 1),
              new Vector2(1, 1)
            };
            //-----------------------------------------------------

            base.Generate();
        }
    }
}