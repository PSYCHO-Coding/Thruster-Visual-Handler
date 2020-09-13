




ONLY CHANGE 'UserSettings.cs' if you don't know what you're doing or want to change the core implementation.

To define Emissive settings, copy-paste and edit the following:

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
            ThrusterDataInstance.thrustData.ColorAtMaxThrust = new Color(255, 40, 10, 255);
            ThrusterDataInstance.thrustData.MaxThrustEmissiveMultiplierMin = 1f;
            ThrusterDataInstance.thrustData.MaxThrustEmissiveMultiplierMax = 50f;
            ThrusterDataInstance.thrustData.ThrusterStatusRampUp = 0.1f;
            ThrusterDataInstance.thrustData.ThrusterStatusRampDown = 0.005f;
            ThrusterDataInstance.thrustData.ThrusterStrengthRampUp = 0.01f;
            ThrusterDataInstance.thrustData.ThrusterStrengthRampDown = 0.007f;
            ThrusterDataInstance.thrustData.ThrusterOffRampDown = 0.1f;
            ThrusterDataInstance.thrustData.ThrusterNotWorkingRampDown = 0.007f;
            ThrusterDataInstance.thrustData.ThrusterNonFunctionalRampDown = 0.005f;



To apply it to thrusters, copy-paste and edit the following:

            ThrusterDataInstance.AddThrusterData("YourThrusterSubtypeID", "YourThrustersEmissiveMaterialName", ThrusterDataInstance.thrustData);
Note, you can apply same settings to mulitple thrusters or emssivies materials.


EXAMPLES:

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
            ThrusterDataInstance.thrustData.ColorAtMaxThrust = new Color(255, 40, 10, 255);
            ThrusterDataInstance.thrustData.MaxThrustEmissiveMultiplierMin = 1f;
            ThrusterDataInstance.thrustData.MaxThrustEmissiveMultiplierMax = 50f;
            ThrusterDataInstance.thrustData.ThrusterStatusRampUp = 0.1f;
            ThrusterDataInstance.thrustData.ThrusterStatusRampDown = 0.005f;
            ThrusterDataInstance.thrustData.ThrusterStrengthRampUp = 0.01f;
            ThrusterDataInstance.thrustData.ThrusterStrengthRampDown = 0.007f;
            ThrusterDataInstance.thrustData.ThrusterOffRampDown = 0.1f;
            ThrusterDataInstance.thrustData.ThrusterNotWorkingRampDown = 0.007f;
            ThrusterDataInstance.thrustData.ThrusterNonFunctionalRampDown = 0.005f;

            ThrusterDataInstance.AddThrusterData("LargeGridSmallThruster", "Emissive", ThrusterDataInstance.thrustData);
            ThrusterDataInstance.AddThrusterData("SmallGridSmallThruster", "Emissive", ThrusterDataInstance.thrustData);

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
            ThrusterDataInstance.thrustData.ColorAtMaxThrust = new Color(255, 40, 10, 255);
            ThrusterDataInstance.thrustData.MaxThrustEmissiveMultiplierMin = 1f;
            ThrusterDataInstance.thrustData.MaxThrustEmissiveMultiplierMax = 50f;
            ThrusterDataInstance.thrustData.ThrusterStatusRampUp = 0.1f;
            ThrusterDataInstance.thrustData.ThrusterStatusRampDown = 0.005f;
            ThrusterDataInstance.thrustData.ThrusterStrengthRampUp = 0.01f;
            ThrusterDataInstance.thrustData.ThrusterStrengthRampDown = 0.007f;
            ThrusterDataInstance.thrustData.ThrusterOffRampDown = 0.1f;
            ThrusterDataInstance.thrustData.ThrusterNotWorkingRampDown = 0.007f;
            ThrusterDataInstance.thrustData.ThrusterNonFunctionalRampDown = 0.005f;

            ThrusterDataInstance.AddThrusterData("LargeGridLargeThruster", "Emissive", ThrusterDataInstance.thrustData);
            ThrusterDataInstance.AddThrusterData("SmallGridLargeThruster", "Emissive", ThrusterDataInstance.thrustData);