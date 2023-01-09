using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarCup
{
    public class CarController : MonoBehaviour
    {
        [SerializeField] private Car_Data _data;

        [Space(25)]

        [SerializeField] private Transform frontLeftWheelTransform;
        [SerializeField] private WheelCollider frontLeftWheelCollider;

        [Space]

        [SerializeField] private Transform frontRightWheeTransform;
        [SerializeField] private WheelCollider frontRightWheelCollider;

        [Space]

        [SerializeField] private Transform rearLeftWheelTransform;
        [SerializeField] private WheelCollider rearLeftWheelCollider;

        [Space]

        [SerializeField] private Transform rearRightWheelTransform;
        [SerializeField] private WheelCollider rearRightWheelCollider;

        private float motorForce;
        private float breakForce;
        private float maxSteerAngle;
        private float maxSpeed;
        private AnimationCurve handlingMovement;
        private AnimationCurve acceleration;

        private float horizontalInput;
        private float verticalInput;
        private float currentSteerAngle;
        private float currentbreakForce;
        private bool isBreaking;
        private new Rigidbody rigidbody;
        private float speed = 0f;

        private CustomButton gas;
        private CustomButton braeck;
        private CustomButton leftArrow;
        private CustomButton rightArrow;
        private CustomButton handBrake;
        private CustomButton reset;
        private CustomText speedText;

        private void ApplyData()
        {
            motorForce = _data.motorForce;
            breakForce = _data.breakForce;
            maxSteerAngle = _data.maxSteerAngle;
            maxSpeed = _data.maxSpeed;
            handlingMovement = _data.handlingMovement;
            acceleration = _data.acceleration;
            rigidbody.mass = _data.mass;

            ApplyDataToWheelCollider(frontLeftWheelCollider, _data.fl_wheelColliderSettings);
            ApplyDataToWheelCollider(frontRightWheelCollider, _data.fr_wheelColliderSettings);
            ApplyDataToWheelCollider(rearLeftWheelCollider, _data.rl_wheelColliderSettings);
            ApplyDataToWheelCollider(rearRightWheelCollider, _data.rr_wheelColliderSettings);
        }

        private void ApplyDataToWheelCollider(WheelCollider collider, WheelColliderSettings settings)
        {
            JointSpring jointSpring = new JointSpring();
            WheelFrictionCurve f_wheelFrictionCurve = new WheelFrictionCurve();
            WheelFrictionCurve s_wheelFrictionCurve = new WheelFrictionCurve();

            collider.mass = settings.mass;
            collider.radius = settings.radius;
            collider.wheelDampingRate = settings.wheelDampingRate;
            collider.suspensionDistance = settings.suspensionDistance;
            collider.forceAppPointDistance = settings.forceAppPointDistance;
            collider.center = settings.center;

            jointSpring.spring = settings.spring;
            jointSpring.damper = settings.damper;
            jointSpring.targetPosition = settings.targetPosition;
            collider.suspensionSpring = jointSpring;

            f_wheelFrictionCurve.extremumSlip = settings.f_extremumSlip;
            f_wheelFrictionCurve.extremumValue = settings.f_extremumValue;
            f_wheelFrictionCurve.asymptoteSlip = settings.f_asymptoteSlip;
            f_wheelFrictionCurve.asymptoteValue = settings.f_asymptoteValue;
            f_wheelFrictionCurve.stiffness = settings.f_stiffness;
            collider.forwardFriction = f_wheelFrictionCurve;

            s_wheelFrictionCurve.extremumSlip = settings.s_extremumSlip;
            s_wheelFrictionCurve.extremumValue = settings.s_extremumValue;
            s_wheelFrictionCurve.asymptoteSlip = settings.s_asymptoteSlip;
            s_wheelFrictionCurve.asymptoteValue = settings.s_asymptoteValue;
            s_wheelFrictionCurve.stiffness = settings.s_stiffness;
            collider.sidewaysFriction = s_wheelFrictionCurve;
        }

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();

            ApplyData();

            CustomButton[] customButtons = GameObject.FindObjectsOfType<CustomButton>();
            foreach (CustomButton customButton in customButtons)
            {
                switch (customButton.type)
                {
                    case CustomButtonType.Gas:
                        gas = customButton;
                        break;
                    case CustomButtonType.Braeck:
                        braeck = customButton;
                        break;
                    case CustomButtonType.LeftArrow:
                        leftArrow = customButton;
                        break;
                    case CustomButtonType.RightArrow:
                        rightArrow = customButton;
                        break;
                    case CustomButtonType.HandBrake:
                        handBrake = customButton;
                        break;
                    case CustomButtonType.Reset:
                        reset = customButton;
                        break;
                }
            }

            speedText = GameObject.FindObjectOfType<CustomText>();
        }

        private void FixedUpdate()
        {
            GetInput();
            HandleMotor();
            HandleSteering();
            UpdateWheels();
            HandlingMovement();
        }

        private void HandlingMovement()
        {
            speed = Vector3.Magnitude(rigidbody.velocity);

            float speedRatio = speed / maxSpeed;

            HandlingWheelMovement(frontLeftWheelCollider, speedRatio, 1, 1);
            HandlingWheelMovement(frontRightWheelCollider, speedRatio, 1, 1);
            HandlingWheelMovement(rearLeftWheelCollider, speedRatio, 1, 1);
            HandlingWheelMovement(rearRightWheelCollider, speedRatio, 1, 1);
        }

        private void HandlingWheelMovement(WheelCollider collider, float t, float forwardExtra = 0f, float sidewaysExtra = 0f)
        {
            WheelFrictionCurve forwardFriction = collider.forwardFriction;
            forwardFriction.stiffness = handlingMovement.Evaluate(t) + forwardExtra;
            collider.forwardFriction = forwardFriction;

            WheelFrictionCurve sidewaysFriction = collider.sidewaysFriction;
            sidewaysFriction.stiffness = handlingMovement.Evaluate(t / 2) + sidewaysExtra;
            collider.sidewaysFriction = sidewaysFriction;
        }

        private void GetInput()
        {
            if (leftArrow == null || rightArrow == null) horizontalInput = Input.GetAxis("Horizontal");
            else horizontalInput = Mathf.Lerp(horizontalInput, (rightArrow.pressured) ? 1 : (leftArrow.pressured) ? -1 : 0, Time.fixedDeltaTime * 10);
            if (gas == null || braeck == null) verticalInput = Input.GetAxis("Vertical");
            else verticalInput = Mathf.Lerp(verticalInput, (gas.pressured) ? 1 : (braeck.pressured) ? -1 : 0, Time.fixedDeltaTime * 10);
            if (handBrake == null) isBreaking = Input.GetKey(KeyCode.Space);
            else isBreaking = handBrake.pressured;

            if (Mathf.Abs(horizontalInput) < 0.05f) horizontalInput = 0f;
            else if (horizontalInput > 0.95f) horizontalInput = 1f;
            else if (horizontalInput < -0.95f) horizontalInput = -1f;

            if (Mathf.Abs(verticalInput) < 0.05) verticalInput = 0f;
            else if (verticalInput > 0.95f) verticalInput = 1f;
            else if (verticalInput < -0.95f) verticalInput = -1f;

            if (reset != null && reset.pressured)
            {
                reset.pressured = false;
                transform.Rotate(transform.forward, 180);
                transform.Translate(transform.up);
            }

            if (speedText != null)
                speedText.UpdatText(speed.ToString("F2"));
        }

        private void HandleMotor()
        {
            float speedRatio = speed / maxSpeed;

            if (speed < maxSpeed)
            {
                frontLeftWheelCollider.motorTorque = verticalInput * motorForce + (motorForce - motorForce * acceleration.Evaluate(speedRatio));
                frontRightWheelCollider.motorTorque = verticalInput * motorForce + (motorForce - motorForce * acceleration.Evaluate(speedRatio));
            }
            else
            {
                frontLeftWheelCollider.motorTorque = -verticalInput * motorForce;
                frontRightWheelCollider.motorTorque = -verticalInput * motorForce;
            }
            currentbreakForce = isBreaking ? breakForce : (verticalInput == 0) ? breakForce / 4 : 0;
            ApplyBreaking();
        }

        private void ApplyBreaking()
        {
            frontRightWheelCollider.brakeTorque = currentbreakForce;
            frontLeftWheelCollider.brakeTorque = currentbreakForce;
            rearLeftWheelCollider.brakeTorque = currentbreakForce;
            rearRightWheelCollider.brakeTorque = currentbreakForce;
        }

        private void HandleSteering()
        {
            currentSteerAngle = maxSteerAngle * horizontalInput;
            frontLeftWheelCollider.steerAngle = currentSteerAngle;
            frontRightWheelCollider.steerAngle = currentSteerAngle;
        }

        private void UpdateWheels()
        {
            UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
            UpdateSingleWheel(frontRightWheelCollider, frontRightWheeTransform);
            UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
            UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        }

        private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
        {
            Vector3 pos;
            Quaternion rot;
            wheelCollider.GetWorldPose(out pos, out rot);
            wheelTransform.rotation = rot;
            wheelTransform.position = pos;
        }
    }
}