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
        //----Controlling------------------------------------------------------
        public float motorForce = 1200f;
        public float breakForce = 2000f;
        public float maxSteerAngle = 30f;
        public float maxSpeed = 80f;
        public AnimationCurve handlingMovement = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public AnimationCurve acceleration = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        //----Wheels------------------------------------------------------------
        public Transform frontLeftWheelTransform;
        public WheelCollider frontLeftWheelCollider;
        public Transform frontRightWheeTransform;
        public WheelCollider frontRightWheelCollider;
        public Transform rearLeftWheelTransform;
        public WheelCollider rearLeftWheelCollider;
        public Transform rearRightWheelTransform;
        public WheelCollider rearRightWheelCollider;

        //----Componets---------------------------------------------------------

        //Motor Suond Settings
        public bool motorSuondSettings_active = false;
        public AudioClip motorSuondSettings_idle;
        public float motorSuondSettings_idleVolume = 0.15f;
        public AudioClip motorSuondSettings_low;
        public float motorSuondSettings_lowVolume = 0.25f;
        public AudioClip motorSuondSettings_mid;
        public float motorSuondSettings_midVolume = 0.5f;
        public AudioClip motorSuondSettings_high;
        public float motorSuondSettings_highVolume = 0.75f;
        
        //Rotating 360Deg Settings
        public bool rotating360DegSettings_active = false;
        public bool rotating360DegSettings_onGroundX = false;
        public bool rotating360DegSettings_onGroundY = true;
        public bool rotating360DegSettings_onGroundZ = false;
        public bool rotating360DegSettings_inAirX = false;
        public bool rotating360DegSettings_inAirY = false;
        public bool rotating360DegSettings_inAirZ = true;
        public float rotating360DegSettings_inAirDistance = 3f;

        //----Private Variables-------------------------------------------------

        //Controlling
        private float horizontalInput;
        private float verticalInput;
        private float currentSteerAngle;
        private float currentbreakForce;
        private bool isBreaking;
        private new Rigidbody rigidbody;
        private InputManger inputManger;
        private float speed { get => Vector3.Magnitude(rigidbody.velocity); }
        private float speedRatio { get => speed / maxSpeed; }
        private CustomText speedText;
        private float fl_wheelColliderSettings_f_stiffness;
        private float fr_wheelColliderSettings_f_stiffness;
        private float rl_wheelColliderSettings_f_stiffness;
        private float rr_wheelColliderSettings_f_stiffness;
        private float fl_wheelColliderSettings_s_stiffness;
        private float fr_wheelColliderSettings_s_stiffness;
        private float rl_wheelColliderSettings_s_stiffness;
        private float rr_wheelColliderSettings_s_stiffness;
        
        //Componets
        private Transform componets;
        private AudioSource motorSuond;

        //calculat a cuorent status
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

        //for set some data
        private void ApplyData()
        {
            fl_wheelColliderSettings_f_stiffness = frontLeftWheelCollider.forwardFriction.stiffness;
            fr_wheelColliderSettings_f_stiffness = frontRightWheelCollider.forwardFriction.stiffness;
            rl_wheelColliderSettings_f_stiffness = rearLeftWheelCollider.forwardFriction.stiffness;
            rr_wheelColliderSettings_f_stiffness = rearRightWheelCollider.forwardFriction.stiffness;

            fl_wheelColliderSettings_s_stiffness = frontLeftWheelCollider.sidewaysFriction.stiffness;
            fr_wheelColliderSettings_s_stiffness = frontRightWheelCollider.sidewaysFriction.stiffness;
            rl_wheelColliderSettings_s_stiffness = rearLeftWheelCollider.sidewaysFriction.stiffness;
            rr_wheelColliderSettings_s_stiffness = rearRightWheelCollider.sidewaysFriction.stiffness;

            //Componets
            componets = new GameObject("Componets").transform;
            componets.parent = transform;

            if (motorSuondSettings_active)//Motor Suond
            {
                GameObject motorSuondGO = new GameObject("Motor Suond");
                motorSuondGO.transform.parent = componets;
                motorSuond = motorSuondGO.AddComponent<AudioSource>();
                motorSuond.loop = true;
            }
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
            HandlingWheelMovement(frontLeftWheelCollider, speedRatio, fl_wheelColliderSettings_f_stiffness, fl_wheelColliderSettings_s_stiffness);
            HandlingWheelMovement(frontRightWheelCollider, speedRatio, fr_wheelColliderSettings_f_stiffness, fr_wheelColliderSettings_s_stiffness);
            HandlingWheelMovement(rearLeftWheelCollider, speedRatio, rl_wheelColliderSettings_f_stiffness, rl_wheelColliderSettings_s_stiffness);
            HandlingWheelMovement(rearRightWheelCollider, speedRatio, rr_wheelColliderSettings_f_stiffness, rr_wheelColliderSettings_s_stiffness);
        }

        private void HandlingWheelMovement(WheelCollider collider, float t, float forwardExtra = 0f, float sidewaysExtra = 0f)
        {
            WheelFrictionCurve forwardFriction = collider.forwardFriction;
            forwardFriction.stiffness = handlingMovement.Evaluate(t) * forwardExtra + forwardExtra;
            collider.forwardFriction = forwardFriction;

            WheelFrictionCurve sidewaysFriction = collider.sidewaysFriction;
            sidewaysFriction.stiffness = handlingMovement.Evaluate(t) * sidewaysExtra + sidewaysExtra;
            collider.sidewaysFriction = sidewaysFriction;
        }

        private void GetInput()
        {
            horizontalInput = inputManger.horizontal;
            verticalInput = inputManger.vertical;

            isBreaking = inputManger.breaking;

            if (inputManger.GetRest() && status == CarStatus.UpsideDown) StartCoroutine(ResetCar());

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
                rigidbody.AddTorque(force * Mathf.Pow(rigidbody.mass, 2));
                yield return new WaitForEndOfFrame();
            }
        }

        private void HandleMotor()
        {
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
            if (motorSuondSettings_active) MotorSuond();

            if (rotating360DegSettings_active) Rotating360Deg();
        }

        private void MotorSuond()
        {
            if (!motorSuondSettings_active) return;
            
            AudioClip clip = null;
            float volume = 0f;

            switch (speedRatio)
            {
                case < 0.01f:
                    //Idle
                    if (motorSuondSettings_idle != null) clip = motorSuondSettings_idle;
                    volume = motorSuondSettings_idleVolume;
                    break;
                case < 0.3f://Low Driving
                    if (motorSuondSettings_low != null) clip = motorSuondSettings_low;
                    volume = motorSuondSettings_lowVolume;
                    break;
                case < 0.6f://Mid Driving
                    if (motorSuondSettings_mid != null) clip = motorSuondSettings_mid;
                    volume = motorSuondSettings_midVolume;
                    break;
                case < 1://High Driving
                    if (motorSuondSettings_high != null) clip = motorSuondSettings_high;
                    volume = motorSuondSettings_highVolume;
                    break;
            }

            if (motorSuond.clip != clip && clip != null)
            {
                motorSuond.clip = clip;
                motorSuond.Play();
            }

            motorSuond.volume = Mathf.Lerp(motorSuond.volume, volume, Time.deltaTime);
        }

        private void Rotating360Deg()
        {
            Vector3 force = Vector3.zero;

            if (inputManger.rotating360Deg && status == CarStatus.OnGround)
            {
                if (rotating360DegSettings_onGroundX) force.x = 1;
                if (rotating360DegSettings_onGroundY) force.y = 1;
                if (rotating360DegSettings_onGroundZ) force.z = 1;

                force *= inputManger.rotating360DegDir;
            }

            if (status == CarStatus.InAir)
            {
                if (!Physics.Raycast(transform.position, -transform.up, rotating360DegSettings_inAirDistance))
                {
                    if (rotating360DegSettings_inAirX) force.x = 1;
                    if (rotating360DegSettings_inAirY) force.y = 1;
                    if (rotating360DegSettings_inAirZ) force.z = 1;
                }
            }

            rigidbody.AddTorque(force * rigidbody.mass * rigidbody.mass);
        }
    }
}