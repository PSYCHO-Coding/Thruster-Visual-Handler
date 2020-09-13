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

using PSYCHO.ThrusterVisualHandlerUserSettings;

// DO NOT EDIT IF YOU DON'T KNOW WHAT YOU'RE DOING.

namespace PSYCHO.ThrusterVisualHandlerData
{
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]

    class ThrusterDataHandler : MySessionComponentBase
    {
        public static ThrusterDataHandler ThrusterDataInstance;

        public override void LoadData()
        {
            ThrusterDataInstance = this;

            var userData = new UserData();
            userData.ConstructThrusterData();
        }

        protected override void UnloadData()
        {
            ThrusterDataInstance = null;
        }

        public string MySubtypeID;
        public Dictionary<string, List<ThrusterData>> SavedThrusterData = new Dictionary<string, List<ThrusterData>>();
        public ThrusterData thrustData;

        public List<ThrusterData> GetThrusterData(string _subtypeID)
        {
            if (!SavedThrusterData.ContainsKey(_subtypeID))
            {
                MyAPIGateway.Utilities.ShowNotification("GetThrusterData was NULL.", 10000);
                return null;
            }

            return SavedThrusterData[_subtypeID];
        }

        public class ThrusterData
        {
            public string EmissiveMaterialName { get; set; }
            public Color OnColor { get; set; }
            public Color OffColor { get; set; }
            public Color NotWorkingColor { get; set; }
            public Color NonFunctionalColor { get; set; }
            public float ThrusterOnEmissiveMultiplier { get; set; }
            public float ThrusterOffEmissiveMultiplier { get; set; }
            public float ThrusterNotWorkingEmissiveMultiplier { get; set; }
            public float ThrusterNonFunctionalEmissiveMultiplier { get; set; }
            public bool ChangeColorByThrustOutput { get; set; }
            public float AntiFlickerThreshold { get; set; }
            public Color ColorAtMaxThrust { get; set; }
            public float MaxThrustEmissiveMultiplierMin { get; set; }
            public float MaxThrustEmissiveMultiplierMax { get; set; }
            public Color ErrorColor { get; set; }
            public Color CurrentColor { get; set; }
            public Color ActiveColor { get; set; }
            public Color InactiveColor { get; set; }
            public float ActiveGlow { get; set; }
            public float InactiveGlow { get; set; }
            public float ThrusterStatus { get; set; }
            public float ThrusterStrength { get; set; }

            public float ThrusterStatusRampUp { get; set; }
            public float ThrusterStatusRampDown { get; set; }

            public float ThrusterStrengthRampUp { get; set; }
            public float ThrusterStrengthRampDown { get; set; }

            public float ThrusterOffRampDown { get; set; }
            public float ThrusterNotWorkingRampDown { get; set; }
            public float ThrusterNonFunctionalRampDown { get; set; }
        }

        public void AddThrusterData(string _subtypeID, string _emissiveName, ThrusterData _thrustData)
        {
            _thrustData.EmissiveMaterialName = _emissiveName;

            if (!SavedThrusterData.ContainsKey(_subtypeID))
                SavedThrusterData[_subtypeID] = new List<ThrusterDataHandler.ThrusterData>();
            SavedThrusterData[_subtypeID].Add(_thrustData);
        }



        public List<IMyUpgradeModule> Upgraders = new List<IMyUpgradeModule>();
        public Dictionary<IMyUpgradeModule, bool> ThrusterPowerState = new Dictionary<IMyUpgradeModule, bool>();

        /*
        public UpgradeData Upgraded;

        public class UpgradeData
        {
            public bool Upgraded { get; set; }
        }
        */



        public Dictionary<string, FlameData> SavedFlameData = new Dictionary<string, FlameData>();
        public FlameData flameData;

        public class FlameData
        {
            public bool UseLights { get; set; }
            public Color LightInnerMin { get; set; }
            public Color LightInnerMax { get; set; }
            public Color LightMiddleMin { get; set; }
            public Color LightMiddleMax { get; set; }
            public Color LightOuterMin { get; set; }
            public Color LightOuterMax { get; set; }

            public Color FlameIdleColorMin { get; set; }
            public Color FlameIdleColorMax { get; set; }
            public Color FlameFullColorMin { get; set; }
            public Color FlameFullColorMax { get; set; }
        }

        public void AddFlameData(string _subtypeID, FlameData _flameData)
        {
            SavedFlameData[_subtypeID] = _flameData;
        }
    }
}
