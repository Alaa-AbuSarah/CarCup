using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarCup
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(InputManger))]
    public class CarController : MonoBehaviour
    {
        [SerializeField] private Car_Data _data;

        [Space(25)]

        [Header("Componets")]
        [Space]

        [SerializeField] private AudioSource motorSuond;

        [Space(25)]

        [Header("Wheels")]
        [Space]

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
        private InputManger inputManger;
        private float speed = 0f;
        private CustomText speedText;

        private CarStatus status 
        {
            get 
            {
                CarStatus _status = CarStatus.OnGround;

                WheelHit hit;
                if (!frontLeftWheelCollider.GetGroundHit(out hit)
                    &&
                    !frontRightWheelCollider.GetGroundHit(out hit)
                    &&
                    !rearLeftWheelCollider.GetGroundHit(out hit)
                    &&
                    !rearRightWheelCollider.GetGroundHit(out hit))
                {
                    if (Physics.Raycast(transform.position + transform.up, transform.up, 2))
                        _status = CarStatus.UpsideDown;
                    else
                        _status = CarStatus.InAir;

                    Debug.DrawRay(transform.position + transform.up, transform.up, Color.red, 0.5f);
                }

                return _status;
            }
        }

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
            inputManger = GetComponent<InputManger>();
            ApplyData();
            speedText = GameObject.FindObjectOfType<CustomText>();
        }

        private void FixedUpdate()
        {
            GetInput();
            HandleMotor();
            HandleSteering();
            UpdateWheels();
            HandlingMovement();
            HandlingComponets();
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
            horizontalInput = inputManger.horizontal;
            verticalInput = inputManger.vertical;

            isBreaking = inputManger.breaking;

            if (inputManger.GetRest() && status == CarStatus.UpsideDown) StartCoroutine(ResetCar());

            if (inputManger.rotating360Deg && status == CarStatus.OnGround) StartCoroutine(Rotating360Deg());

            if (verticalInput == 0 && horizontalInput == 0)
                isBreaking = true;

            if (speedText != null)
                speedText.UpdatText(speed.ToString("F2"));
        }

        private IEnumerator ResetCar() 
        {
            Vector3 force = -transform.up - transform.right - transform.forward;
            for (int i = 0; i < 5; i++)
            {
                rigidbody.AddTorque(force * Mathf.Pow(rigidbody.mass, 5));
                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator Rotating360Deg()
        {
            Vector3 force = transform.up * inputManger.rotating360DegDir;
            for (int i = 0; i < 5; i++)
            {
                rigidbody.AddTorque(force * Mathf.Pow(rigidbody.mass, 5));
                yield return new WaitForEndOfFrame();
            }
        }

        private void HandleMotor()
        {
            float speedRatio = speed / maxSpeed;

            if (speed < maxSpeed)
            {
                frontLeftWheelCollider.motorTorque = verticalInput * (motorForce + (motorForce - motorForce * acceleration.Evaluate(speedRatio)));
                frontRightWheelCollider.motorTorque = verticalInput * (motorForce + (motorForce - motorForce * acceleration.Evaluate(speedRatio)));
            }
            else
            {
                frontLeftWheelCollider.motorTorque = -verticalInput * motorForce;
                frontRightWheelCollider.motorTorque = -verticalInput * motorForce;
            }
            currentbreakForce = isBreaking ? breakForce : 0;
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

        private void HandlingComponets()
        {
            if (_data.motorSuond.active) MotorSuond();
        }

        private void MotorSuond()
        {
            if (!_data.motorSuond.active) return;

            float speedRatio = speed / maxSpeed;
            AudioClip clip = null;
            float volume = 0f;

            switch (speedRatio)
            {
                case < 0.01f:
                    //Idle
                    if (_data.motorSuond.idle != null) clip = _data.motorSuond.idle;
                    volume = _data.motorSuond.idleVolume;
                    break;
                case < 0.3f://Low Driving
                    if (_data.motorSuond.low != null) clip = _data.motorSuond.low;
                    volume = _data.motorSuond.lowVolume;
                    break;
                case < 0.6f://Mid Driving
                    if (_data.motorSuond.mid != null) clip = _data.motorSuond.mid;
                    volume = _data.motorSuond.midVolume;
                    break;
                case < 1://High Driving
                    if (_data.motorSuond.high != null) clip = _data.motorSuond.high;
                    volume = _data.motorSuond.highVolume;
                    break;
            }

            if (motorSuond.clip != clip && clip != null)
            {
                motorSuond.clip = clip;
                motorSuond.Play();
            }
            motorSuond.volume = Mathf.Lerp(motorSuond.volume, volume, Time.deltaTime);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;

            Vector3 force = -transform.up - transform.right - transform.forward;
            force += transform.position;
            Gizmos.DrawSphere(force, 0.5f);
        }
    }
}