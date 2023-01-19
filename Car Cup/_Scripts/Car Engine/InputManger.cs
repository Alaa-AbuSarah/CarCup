using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarCup
{
    public class InputManger : MonoBehaviour
    {
        public float horizontal = 0f;
        public float vertical = 0f;
        public bool breaking = false;
        public bool reset = false;
        public bool rotating360Deg = false;
        public int rotating360DegDir;

        private CustomButton gasButton;
        private CustomButton braeckButton;
        private CustomButton leftArrowButton;
        private CustomButton rightArrowButton;
        private CustomButton handBrakeButton;
        private CustomButton resetButton;

        private float rotating360DegTimer = 0f;

        private void Start()
        {
            //----Find control buttons-------------------------------------------------
            CustomButton[] customButtons = GameObject.FindObjectsOfType<CustomButton>();
            foreach (CustomButton customButton in customButtons)
            {
                switch (customButton.type)
                {
                    case CustomButtonType.Gas:
                        gasButton = customButton;
                        break;
                    case CustomButtonType.Braeck:
                        braeckButton = customButton;
                        break;
                    case CustomButtonType.LeftArrow:
                        leftArrowButton = customButton;
                        break;
                    case CustomButtonType.RightArrow:
                        rightArrowButton = customButton;
                        break;
                    case CustomButtonType.HandBrake:
                        handBrakeButton = customButton;
                        break;
                    case CustomButtonType.Reset:
                        resetButton = customButton;
                        break;
                }
                //------------------------------------------------------------------------
            }
        }

        private void Update()
        {
            //----Calculating horizontal input----------------------------------------
            if (leftArrowButton == null || rightArrowButton == null)//No horizontal buttons
                horizontal = Input.GetAxis("Horizontal");
            else
                horizontal = Mathf.Lerp(horizontal, (rightArrowButton.pressured) ? 1 : (leftArrowButton.pressured) ? -1 : 0, Time.fixedDeltaTime * 10);
            //------------------------------------------------------------------------

            //----Calculating vertical input------------------------------------------
            if (gasButton == null || braeckButton == null)//No vertical buttons
                vertical = Input.GetAxis("Vertical");
            else
                vertical = Mathf.Lerp(vertical, (gasButton.pressured) ? 1 : (braeckButton.pressured) ? -1 : 0, Time.fixedDeltaTime * 10);
            //------------------------------------------------------------------------

            //----Calculating breaking input------------------------------------------
            if (handBrakeButton == null)//No breaking button button
                breaking = Input.GetKey(KeyCode.Space);
            else
                breaking = handBrakeButton.pressured;
            //------------------------------------------------------------------------

            //----Calculating reset input------------------------------------------
            if (resetButton == null)//No reset button button
                reset = Input.GetKeyUp(KeyCode.R);
            else
                reset = resetButton.pressured;
            //------------------------------------------------------------------------

            //----Keep the values moderate--------------------------------------------
            if (Mathf.Abs(horizontal) < 0.05f)
                horizontal = 0f;
            else if (horizontal > 0.95f)
                horizontal = 1f;
            else if (horizontal < -0.95f)
                horizontal = -1f;

            if (Mathf.Abs(vertical) < 0.05)
                vertical = 0f;
            else if (vertical > 0.95f)
                vertical = 1f;
            else if (vertical < -0.95f)
                vertical = -1f;
            //------------------------------------------------------------------------


            //----Calculating rotating360Deg input------------------------------------------
            if (leftArrowButton == null || rightArrowButton == null)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    rotating360DegTimer = Time.time;
                    rotating360Deg = true;
                    rotating360DegDir = 1;
                }
                else if (Input.GetKeyDown(KeyCode.Q))
                {
                    rotating360DegTimer = Time.time;
                    rotating360Deg = true;
                    rotating360DegDir = -1;
                }
                else if (rotating360DegTimer + 0.25f < Time.time)
                {
                    rotating360Deg = false;
                    rotating360DegDir = 0;
                }
            }
            else
            {
                if (rightArrowButton.clickCount == 2)
                {
                    rotating360DegTimer = Time.time;
                    rotating360Deg = true;
                    rotating360DegDir = 1;
                }
                else if (leftArrowButton.clickCount == 2)
                {
                    rotating360DegTimer = Time.time;
                    rotating360Deg = true;
                    rotating360DegDir = -1;
                }
                else if (rotating360DegTimer + 0.25f < Time.time)
                {
                    rotating360Deg = false;
                    rotating360DegDir = 0;
                }
            }
            //------------------------------------------------------------------------
        }

        //----Return reset value and handling reset button if have it-----------------
        public bool GetRest()
        {
            if (resetButton != null) resetButton.pressured = false;
            return reset;
        }
        //-----------------------------------------------------------------------------
    }
}