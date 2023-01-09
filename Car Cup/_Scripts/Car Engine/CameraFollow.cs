using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarCup
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Vector3 offset = new Vector3(2, 3, -5);
        [SerializeField] private Transform target;
        [SerializeField] private float translateSpeed = 10f;
        [SerializeField] private float rotationSpeed = 12f;

        private void FixedUpdate()
        {
            HandleTranslation();
            HandleRotation();
        }

        private void HandleTranslation()
        {
            var targetPosition = target.TransformPoint(offset);
            transform.position = Vector3.Lerp(transform.position, targetPosition, translateSpeed * Time.deltaTime);
        }
        private void HandleRotation()
        {
            var direction = target.position - transform.position;
            var rotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }

        public void SetTarget(Transform newTarget) => target = newTarget;
    }
}