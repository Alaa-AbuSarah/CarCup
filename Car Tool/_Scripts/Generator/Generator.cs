using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTools
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class Generator : MonoBehaviour
    {
        protected new string name = "Generated Mesh"; //Name Of Generated Mesh


        //--------Mesh Data-------------------------------
        protected Vector3[] vertices = default;
        protected int[] triangles = default;
        protected Vector3[] normals = default;
        protected Vector2[] uv = default;


        public virtual void Generate() => ApplyDatatToMesh();

        public virtual void Clear()
        {
            name = "Generated Mesh";

            vertices = default;
            triangles = default;
            normals = default;
            uv = default;

            ApplyDatatToMesh();
        }

        private void ApplyDatatToMesh()
        {
            Mesh mesh;

            mesh = new Mesh();
            mesh.name = name;

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.normals = normals;
            mesh.uv = uv;

            if (GetComponent<MeshRenderer>().sharedMaterial == null)
                SetDefaultMaterial();

            GetComponent<MeshFilter>().mesh = mesh;
        }

        private void SetDefaultMaterial()
        {
            Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            GetComponent<MeshRenderer>().sharedMaterial = material;
        }
    }
}