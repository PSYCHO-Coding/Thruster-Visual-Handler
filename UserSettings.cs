using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRageMath;
using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using VRage.ObjectBuilders;
using VRage.Game.Components;
using VRage.ModAPI;
using VRage.Game.ModAPI;
using VRage;
using System.Runtime.CompilerServices;

using PSYCHO.ThrusterVisualHandlerData;

namespace PSYCHO.ThrusterVisualHandlerUserSettings
{
    public class UserData
    {
        // DO NOT EDIT!!!
        ThrusterDataHandler ThrusterDataInstance => ThrusterDataHandler.ThrusterDataInstance;

        // ========================
        // USER CHANGABLE VARIABLES
        // ========================

        // NOT RECOMMENDED!!!
        public readonly HashSet<string> EXPERIMENTAL = new HashSet<string>()
        {
            "SuperThruster_Small"
        };

        // ADD THRUSTERS HERE.
        public readonly HashSet<string> ThrusterSubtypeIDs = new HashSet<string>()
        {
            "SuperThruster_Small"
        };

        public void ConstructThrusterData()
        {
            // NOT RECOMMENDED!!!
            // 69.999999, 104.299998, 165.899991, 191.25 // Standard ion flame color translated from percent float to RGB.
            ThrusterDataInstance.flameData = new ThrusterDataHandler.FlameData();
            /*
            // Set flames to invisible for testing.
            ThrusterDataInstance.flameData.FlameIdleColorMin = new Color(0, 0, 0, 0);
            ThrusterDataInstance.flameData.FlameIdleColorMax = new Color(0, 0, 0, 0);
            ThrusterDataInstance.flameData.FlameFullColorMin = new Color(0, 0, 0, 0);
            ThrusterDataInstance.flameData.FlameFullColorMax = new Color(0, 0, 0, 0);
            ThrusterDataInstance.AddFlameData("SuperThruster_Small", ThrusterDataInstance.flameData);
            // ----------------------
            */
            ThrusterDataInstance.flameData.FlameIdleColorMin = new Color(0, 0, 0, 0);
            ThrusterDataInstance.flameData.FlameIdleColorMax = new Color(0, 0, 0, 0);
            ThrusterDataInstance.flameData.FlameFullColorMin = new Color(40, 104, 255, 255);
            ThrusterDataInstance.flameData.FlameFullColorMax = new Color(255, 200, 255, 255);

            ThrusterDataInstance.flameData.UseLights = true;
            ThrusterDataInstance.flameData.LightInnerMin = Color.Red;
            ThrusterDataInstance.flameData.LightInnerMax = Color.Orange;
            ThrusterDataInstance.flameData.LightMiddleMin = Color.Red;
            ThrusterDataInstance.flameData.LightMiddleMax = Color.DarkOrange;
            ThrusterDataInstance.flameData.LightOuterMin = Color.Black;
            ThrusterDataInstance.flameData.LightOuterMax = Color.Red;
            ThrusterDataInstance.AddFlameData("SuperThruster_Small", ThrusterDataInstance.flameData);

            // ==============================================================
            // COPY AND EDIT THIS PER THRUSTER / THRUSTER + EMISSIVE MATERIAL
            // ==============================================================

            // ONLY EDIT VALUES THAT ARE PROPERLY ANOTATED WITH '// EDIT'

            // ============================================================== COPY BLOCK START
            // DO NOT EDIT
            ThrusterDataInstance.thrustData = new ThrusterDataHandler.ThrusterData();

            // EDIT STATIC
            ThrusterDataInstance.thrustData.OnColor = new Color(0, 20, 255, 0);
            ThrusterDataInstance.thrustData.OffColor = new Color(0, 0, 0);
            ThrusterDataInstance.thrustData.NotWorkingColor = new Color(0, 0, 0);
            ThrusterDataInstance.thrustData.NonFunctionalColor = new Color(0, 0, 0);
            ThrusterDataInstance.thrustData.ThrusterOnEmissiveMultiplier = 1f;
            ThrusterDataInstance.thrustData.ThrusterOffEmissiveMultiplier = 0f;
            ThrusterDataInstance.thrustData.ThrusterNotWorkingEmissiveMultiplier = 0f;
            ThrusterDataInstance.thrustData.ThrusterNonFunctionalEmissiveMultiplier = 0f;
            // EDIT DYNAMIC
            ThrusterDataInstance.thrustData.ChangeColorByThrustOutput = true;
            ThrusterDataInstance.thrustData.AntiFlickerThreshold = 0.01f;
            ThrusterDataInstance.thrustData.ColorAtMaxThrust = new Color(255, 40, 10, 0);
            ThrusterDataInstance.thrustData.MaxThrustEmissiveMultiplierMin = 1f;
            ThrusterDataInstance.thrustData.MaxThrustEmissiveMultiplierMax = 50f;
            ThrusterDataInstance.thrustData.ThrusterStatusRampUp = 0.1f;
            ThrusterDataInstance.thrustData.ThrusterStatusRampDown = 0.005f;
            ThrusterDataInstance.thrustData.ThrusterStrengthRampUp = 0.01f;
            ThrusterDataInstance.thrustData.ThrusterStrengthRampDown = 0.007f;
            ThrusterDataInstance.thrustData.ThrusterOffRampDown = 0.1f;
            ThrusterDataInstance.thrustData.ThrusterNotWorkingRampDown = 0.007f;
            ThrusterDataInstance.thrustData.ThrusterNonFunctionalRampDown = 0.005f;

            ThrusterDataInstance.AddThrusterData("SuperThruster_Small", "Emissive", ThrusterDataInstance.thrustData);
            // ============================================================== COPY BLOCK END

            // ============================================================== COPY BLOCK START
            ThrusterDataInstance.thrustData = new ThrusterDataHandler.ThrusterData();

            // EDIT STATIC
            ThrusterDataInstance.thrustData.OnColor = new Color(0, 0, 0, 0); // HEAT SET TO BLACK AND TRANSPARENT?
            ThrusterDataInstance.thrustData.OffColor = new Color(0, 0, 0);
            ThrusterDataInstance.thrustData.NotWorkingColor = new Color(0, 0, 0);
            ThrusterDataInstance.thrustData.NonFunctionalColor = new Color(0, 0, 0);
            ThrusterDataInstance.thrustData.ThrusterOnEmissiveMultiplier = 0f;
            ThrusterDataInstance.thrustData.ThrusterOffEmissiveMultiplier = 0f;
            ThrusterDataInstance.thrustData.ThrusterNotWorkingEmissiveMultiplier = 0f;
            ThrusterDataInstance.thrustData.ThrusterNonFunctionalEmissiveMultiplier = 0f;
            // EDIT DYNAMIC
            ThrusterDataInstance.thrustData.ChangeColorByThrustOutput = true;
            ThrusterDataInstance.thrustData.AntiFlickerThreshold = 0.01f;
            //ThrusterDataInstance.thrustData.ColorAtMaxThrust = new Color(255, 40, 10, 255); // HEAT SET TO BRIGHT ORANGE AND OPAQUE
            ThrusterDataInstance.thrustData.ColorAtMaxThrust = new Color(255, 40, 10, 255); // HEAT SET TO BRIGHT ORANGE AND OPAQUE
            ThrusterDataInstance.thrustData.MaxThrustEmissiveMultiplierMin = 1f;
            ThrusterDataInstance.thrustData.MaxThrustEmissiveMultiplierMax = 1f;
            ThrusterDataInstance.thrustData.ThrusterStatusRampUp = 0.1f;
            ThrusterDataInstance.thrustData.ThrusterStatusRampDown = 0.005f;
            ThrusterDataInstance.thrustData.ThrusterStrengthRampUp = 0.01f;
            ThrusterDataInstance.thrustData.ThrusterStrengthRampDown = 0.007f;
            ThrusterDataInstance.thrustData.ThrusterOffRampDown = 0.1f;
            ThrusterDataInstance.thrustData.ThrusterNotWorkingRampDown = 0.007f;
            ThrusterDataInstance.thrustData.ThrusterNonFunctionalRampDown = 0.005f;

            ThrusterDataInstance.AddThrusterData("SuperThruster_Small", "EmissiveHeat", ThrusterDataInstance.thrustData);
            // ============================================================== COPY BLOCK END

            // ============================================================== COPY BLOCK START
            ThrusterDataInstance.thrustData = new ThrusterDataHandler.ThrusterData();

            // EDIT STATIC
            ThrusterDataInstance.thrustData.OnColor = new Color(242, 110, 80, 255); // HEAT SET TO BLACK AND TRANSPARENT?
            ThrusterDataInstance.thrustData.OffColor = new Color(0, 0, 0);
            ThrusterDataInstance.thrustData.NotWorkingColor = new Color(0, 0, 0);
            ThrusterDataInstance.thrustData.NonFunctionalColor = new Color(0, 0, 0);
            ThrusterDataInstance.thrustData.ThrusterOnEmissiveMultiplier = 10f;
            ThrusterDataInstance.thrustData.ThrusterOffEmissiveMultiplier = 0f;
            ThrusterDataInstance.thrustData.ThrusterNotWorkingEmissiveMultiplier = 0f;
            ThrusterDataInstance.thrustData.ThrusterNonFunctionalEmissiveMultiplier = 0f;
            // EDIT DYNAMIC
            ThrusterDataInstance.thrustData.ChangeColorByThrustOutput = false;
            ThrusterDataInstance.thrustData.AntiFlickerThreshold = 0.01f;
            //ThrusterDataInstance.thrustData.ColorAtMaxThrust = new Color(255, 40, 10, 255); // HEAT SET TO BRIGHT ORANGE AND OPAQUE
            ThrusterDataInstance.thrustData.ColorAtMaxThrust = new Color(255, 40, 10, 255); // HEAT SET TO BRIGHT ORANGE AND OPAQUE
            ThrusterDataInstance.thrustData.MaxThrustEmissiveMultiplierMin = 1f;
            ThrusterDataInstance.thrustData.MaxThrustEmissiveMultiplierMax = 1f;
            ThrusterDataInstance.thrustData.ThrusterStatusRampUp = 0.1f;
            ThrusterDataInstance.thrustData.ThrusterStatusRampDown = 0.005f;
            ThrusterDataInstance.thrustData.ThrusterStrengthRampUp = 0.01f;
            ThrusterDataInstance.thrustData.ThrusterStrengthRampDown = 0.007f;
            ThrusterDataInstance.thrustData.ThrusterOffRampDown = 0.1f;
            ThrusterDataInstance.thrustData.ThrusterNotWorkingRampDown = 0.007f;
            ThrusterDataInstance.thrustData.ThrusterNonFunctionalRampDown = 0.005f;

            ThrusterDataInstance.AddThrusterData("SuperThruster_Small", "Emissive2", ThrusterDataInstance.thrustData);
            // ============================================================== COPY BLOCK END
        }

        // ==========================
        // END OF CHANGABLE VARIABLES
        // ==========================
    }
}
