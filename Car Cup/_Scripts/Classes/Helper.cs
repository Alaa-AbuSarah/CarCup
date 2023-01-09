using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarCup
{
    public static class Helper
    {
        //
        // Summary:
        //     Make a WheelFrictionCurve instantiate.
        //
        // Parameters:
        //   extremumSlip:
        //     for set on extremumSlip in WheelFrictionCurve.
        //
        //   extremumValue:
        //     for set on extremumValue in WheelFrictionCurve.
        //
        //   asymptoteSlip:
        //     for set on asymptoteSlip in WheelFrictionCurve.
        //
        //   asymptoteValue:
        //     for set on asymptoteValue in WheelFrictionCurve.
        //
        //   stiffness:
        //     for set on stiffness in WheelFrictionCurve.
        //
        // Returns:
        //     New WheelFrictionCurve by pass parameters.
        public static WheelFrictionCurve MakeWheelFriction(float extremumSlip, float extremumValue, float asymptoteSlip, float asymptoteValue, float stiffness)
        {
            WheelFrictionCurve value = new WheelFrictionCurve();

            value.extremumSlip = extremumSlip;
            value.extremumValue = extremumValue;
            value.asymptoteSlip = asymptoteSlip;
            value.asymptoteValue = asymptoteValue;
            value.stiffness = stiffness;

            return value;
        }
    }
}