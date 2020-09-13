using System;
using System.Collections.Generic;
using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using VRageMath;
using VRage.ObjectBuilders;
using VRage.Game.Components;
using VRage.ModAPI;
using VRage.Game.ModAPI;
using VRage.Game.Entity;

using System.Text;
using Sandbox.Definitions;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Interfaces.Terminal;
using VRage.Game;
using VRage.Game.Models;
using VRage.Utils;
using VRageRender;
using VRageRender.Import;

using Sandbox.Game.Lights;
using VRageRender.Lights;

using System.Linq;
using BulletXNA;

//using PSYCHO.ThrusterVisualHandlerUserSettings;
using PSYCHO.ThrusterVisualHandlerData;



// DO NOT EDIT IF YOU DON'T KNOW WHAT YOU'RE DOING.



// 69.999999, 104.299998, 165.899991, 191.25

namespace PSYCHO.ThrusterVisualHandler
{
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_Thrust), false)]

    public class ThrusterEmissiveColorsLogic : MyGameLogicComponent
    {
        // DO NOT USE THIS!
        bool EXPERIMENTAL = false;

        IMyThrust block;
        string blockSubtypeID = "";
        MyEntitySubpart subpart;
        string subpartName = "Empty";

        PsychosLibrary PLIB = new PsychosLibrary();
        MyThrustDefinition BlockDefinition;

        float ThrustPercent = 0f;
        float ThrustPercentLast = 0f;

        bool StopUpdates = false;

        ThrusterDataHandler ThrusterDataInstance => ThrusterDataHandler.ThrusterDataInstance;
        private PSYCHO.ThrusterVisualHandlerUserSettings.UserData MyUserData = new PSYCHO.ThrusterVisualHandlerUserSettings.UserData();

        bool HasMultipleEmissives = false;

        List<ThrusterDataHandler.ThrusterData> MyThrusterData = new List<ThrusterDataHandler.ThrusterData>();
        List<ThrusterDataHandler.ThrusterData> StaticThrusterData = new List<ThrusterDataHandler.ThrusterData>();
        List<ThrusterDataHandler.ThrusterData> DynamicThrusterData = new List<ThrusterDataHandler.ThrusterData>();

        int EmissiveMaterialCount = 0;

        Dictionary<string, IMyModelDummy> ModelDummy = new Dictionary<string, IMyModelDummy>();
        Vector3D LightInnerDummyPos;
        Vector3D LightMiddleDummyPos;
        Vector3D LightOuterDummyPos;
        int DummyCount = 0;

        Vector4 DefaultFlameIdleColor;
        Vector4 DefaultFlameFullColor;

        ThrusterDataHandler.ThrusterData StaticData;
        ThrusterDataHandler.ThrusterData DynamicData;

        ThrusterDataHandler.FlameData FlameData;

        Color FlameIdleColorMin;
        Color FlameIdleColorMax;
        Color FlameFullColorMin;
        Color FlameFullColorMax;

        bool UseLights = false;
        Color LightInnerMin;
        Color LightInnerMax;
        Color LightMiddleMin;
        Color LightMiddleMax;
        Color LightOuterMin;
        Color LightOuterMax;

        MyLight LightInner;
        MyLight LightMiddle;
        MyLight LightOuter;

        Matrix MatrixRotator;

        Vector3 BlockColor = Vector3.Zero;
        MyStringHash BlockTexture;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            block = (IMyThrust)Entity;

            NeedsUpdate = MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
        }

        public override void UpdateOnceBeforeFrame()
        {
            if (block == null) // Null check all the things.
                return;

            if (!MyUserData.ThrusterSubtypeIDs.Contains(block.BlockDefinition.SubtypeId))
            {
                block = null;
                return;
            }

            BlockColor = block.SlimBlock.ColorMaskHSV;
            BlockTexture = block.SlimBlock.SkinSubtypeId;

            blockSubtypeID = block.BlockDefinition.SubtypeId;

            BlockDefinition = PLIB.GetThrusterDefinition(block);

            if (MyUserData.EXPERIMENTAL.Contains(blockSubtypeID))
            {
                EXPERIMENTAL = true;

                FlameData = ThrusterDataInstance.SavedFlameData[blockSubtypeID];
                FlameIdleColorMin = FlameData.FlameIdleColorMin;
                FlameIdleColorMax = FlameData.FlameIdleColorMax;
                FlameFullColorMin = FlameData.FlameFullColorMin;
                FlameFullColorMax = FlameData.FlameFullColorMax;

                UseLights = FlameData.UseLights;
                LightInnerMin = FlameData.LightInnerMin;
                LightInnerMax = FlameData.LightInnerMax;
                LightMiddleMin = FlameData.LightMiddleMin;
                LightMiddleMax = FlameData.LightMiddleMax;
                LightOuterMin = FlameData.LightOuterMin;
                LightOuterMax = FlameData.LightOuterMax;
            }

            //subpart = block.GetSubpart(subpartName);
            block.TryGetSubpart(subpartName, out subpart);

            MyThrusterData = ThrusterDataInstance.GetThrusterData(blockSubtypeID);
            EmissiveMaterialCount = MyThrusterData.Count;

            foreach (var data in MyThrusterData)
            {
                // Load data as new instance, set defaults where needed, check for errors or missing values and set to defaults as well.
                if (data.ChangeColorByThrustOutput)
                {
                    DynamicData = new ThrusterDataHandler.ThrusterData();

                    DynamicData.EmissiveMaterialName = data.EmissiveMaterialName;
                    DynamicData.OnColor = data.OnColor;
                    DynamicData.OffColor = data.OffColor;
                    DynamicData.NotWorkingColor = data.NotWorkingColor;
                    DynamicData.NonFunctionalColor = data.NonFunctionalColor;
                    DynamicData.ThrusterOnEmissiveMultiplier = data.ThrusterOnEmissiveMultiplier;
                    DynamicData.ThrusterOffEmissiveMultiplier = data.ThrusterOffEmissiveMultiplier;
                    DynamicData.ThrusterNotWorkingEmissiveMultiplier = data.ThrusterNotWorkingEmissiveMultiplier;
                    DynamicData.ThrusterNonFunctionalEmissiveMultiplier = data.ThrusterNonFunctionalEmissiveMultiplier;
                    DynamicData.ChangeColorByThrustOutput = data.ChangeColorByThrustOutput;
                    DynamicData.AntiFlickerThreshold = data.AntiFlickerThreshold;
                    DynamicData.ColorAtMaxThrust = data.ColorAtMaxThrust;
                    DynamicData.MaxThrustEmissiveMultiplierMin = data.MaxThrustEmissiveMultiplierMin;
                    DynamicData.MaxThrustEmissiveMultiplierMax = data.MaxThrustEmissiveMultiplierMax;
                    DynamicData.ErrorColor = data.ErrorColor;
                    DynamicData.CurrentColor = data.CurrentColor;

                    DynamicData.ActiveColor = data.OnColor;
                    DynamicData.InactiveColor = data.NonFunctionalColor;
                    DynamicData.ActiveGlow = data.ThrusterOnEmissiveMultiplier;
                    DynamicData.InactiveGlow = data.ThrusterNonFunctionalEmissiveMultiplier;
                    DynamicData.ThrusterStatus = 0f;
                    DynamicData.ThrusterStrength = 0f;

                    if (data.ThrusterStatusRampUp == 0f)
                        data.ThrusterStatusRampUp = 0.005f;
                    DynamicData.ThrusterStatusRampUp = data.ThrusterStatusRampUp;

                    if (data.ThrusterStatusRampDown == 0f)
                        data.ThrusterStatusRampDown = 0.005f;
                    DynamicData.ThrusterStatusRampDown = data.ThrusterStatusRampDown;

                    if (data.ThrusterStrengthRampUp == 0f)
                        data.ThrusterStrengthRampUp = 0.005f;
                    DynamicData.ThrusterStrengthRampUp = data.ThrusterStrengthRampUp;

                    if (data.ThrusterStrengthRampDown == 0f)
                        data.ThrusterStrengthRampDown = 0.005f;
                    DynamicData.ThrusterStrengthRampDown = data.ThrusterStrengthRampDown;

                    if (data.ThrusterOffRampDown == 0f)
                        data.ThrusterOffRampDown = 0.005f;
                    DynamicData.ThrusterOffRampDown = data.ThrusterOffRampDown;

                    if (data.ThrusterNotWorkingRampDown == 0f)
                        data.ThrusterNotWorkingRampDown = 0.005f;
                    DynamicData.ThrusterNotWorkingRampDown = data.ThrusterNotWorkingRampDown;

                    if (data.ThrusterNonFunctionalRampDown == 0f)
                        data.ThrusterNonFunctionalRampDown = 0.005f;
                    DynamicData.ThrusterNonFunctionalRampDown = data.ThrusterNonFunctionalRampDown;

                    DynamicThrusterData.Add(DynamicData);
                }
                else
                {
                    StaticData = new ThrusterDataHandler.ThrusterData();

                    StaticData.EmissiveMaterialName = data.EmissiveMaterialName;
                    StaticData.OnColor = data.OnColor;
                    StaticData.OffColor = data.OffColor;
                    StaticData.NotWorkingColor = data.NotWorkingColor;
                    StaticData.NonFunctionalColor = data.NonFunctionalColor;
                    StaticData.ThrusterOnEmissiveMultiplier = data.ThrusterOnEmissiveMultiplier;
                    StaticData.ThrusterOffEmissiveMultiplier = data.ThrusterOffEmissiveMultiplier;
                    StaticData.ThrusterNotWorkingEmissiveMultiplier = data.ThrusterNotWorkingEmissiveMultiplier;
                    StaticData.ThrusterNonFunctionalEmissiveMultiplier = data.ThrusterNonFunctionalEmissiveMultiplier;
                    StaticData.ChangeColorByThrustOutput = data.ChangeColorByThrustOutput;
                    StaticData.AntiFlickerThreshold = data.AntiFlickerThreshold;
                    StaticData.ColorAtMaxThrust = data.ColorAtMaxThrust;
                    StaticData.MaxThrustEmissiveMultiplierMin = data.MaxThrustEmissiveMultiplierMin;
                    StaticData.MaxThrustEmissiveMultiplierMax = data.MaxThrustEmissiveMultiplierMax;
                    StaticData.ErrorColor = data.ErrorColor;
                    StaticData.CurrentColor = data.CurrentColor;

                    StaticData.ActiveColor = data.OnColor;
                    StaticData.InactiveColor = data.NonFunctionalColor;
                    StaticData.ActiveGlow = data.ThrusterOnEmissiveMultiplier;
                    StaticData.InactiveGlow = data.ThrusterNonFunctionalEmissiveMultiplier;
                    StaticData.ThrusterStatus = 0f;
                    StaticData.ThrusterStrength = 0f;

                    if (data.ThrusterStatusRampUp == 0f)
                        data.ThrusterStatusRampUp = 0.005f;
                    StaticData.ThrusterStatusRampUp = data.ThrusterStatusRampUp;

                    if (data.ThrusterStatusRampDown == 0f)
                        data.ThrusterStatusRampDown = 0.005f;
                    StaticData.ThrusterStatusRampDown = data.ThrusterStatusRampDown;

                    if (data.ThrusterStrengthRampUp == 0f)
                        data.ThrusterStrengthRampUp = 0.005f;
                    StaticData.ThrusterStrengthRampUp = data.ThrusterStrengthRampUp;

                    if (data.ThrusterStrengthRampDown == 0f)
                        data.ThrusterStrengthRampDown = 0.005f;
                    StaticData.ThrusterStrengthRampDown = data.ThrusterStrengthRampDown;

                    if (data.ThrusterOffRampDown == 0f)
                        data.ThrusterOffRampDown = 0.005f;
                    StaticData.ThrusterOffRampDown = data.ThrusterOffRampDown;

                    if (data.ThrusterNotWorkingRampDown == 0f)
                        data.ThrusterNotWorkingRampDown = 0.005f;
                    StaticData.ThrusterNotWorkingRampDown = data.ThrusterNotWorkingRampDown;

                    if (data.ThrusterNonFunctionalRampDown == 0f)
                        data.ThrusterNonFunctionalRampDown = 0.005f;
                    StaticData.ThrusterNonFunctionalRampDown = data.ThrusterNonFunctionalRampDown;

                    StaticThrusterData.Add(StaticData);
                }
            }

            if (StaticThrusterData.Any())
            {
                SetEmissives();
            }



            if (DynamicThrusterData.Any())
                NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME;
            else
                NeedsUpdate = MyEntityUpdateEnum.EACH_10TH_FRAME;

            // Hook to events.
            block.IsWorkingChanged += IsWorkingChanged;
            block.PropertiesChanged += PropertiesChanged;

            if (EXPERIMENTAL)
            {
                var blockDefinition = block.SlimBlock.BlockDefinition as MyThrustDefinition;

                DefaultFlameIdleColor = blockDefinition.FlameIdleColor;
                DefaultFlameFullColor = blockDefinition.FlameFullColor;

                block.Model.GetDummies(ModelDummy);
                if (ModelDummy.ContainsKey("flame_light_inner"))
                {
                    DummyCount++;
                    LightInnerDummyPos = ModelDummy["flame_light_inner"].Matrix.Translation;
                }
                if (ModelDummy.ContainsKey("flame_light_middle"))
                {
                    DummyCount++;
                    LightMiddleDummyPos = ModelDummy["flame_light_middle"].Matrix.Translation;
                }
                if (ModelDummy.ContainsKey("flame_light_outer"))
                {
                    DummyCount++;
                    LightOuterDummyPos = ModelDummy["flame_light_outer"].Matrix.Translation;
                }

                MatrixRotator = PLIB.ComputeRotationMatrix(1, 0.2f);
            }
        }



        public override void Close()
        {
            if (block == null)
                return;

            // Unhook from events.
            block.IsWorkingChanged -= IsWorkingChanged;
            block.PropertiesChanged -= PropertiesChanged;

            if (EXPERIMENTAL)
            {
                if (LightInner != null)
                {
                    MyLights.RemoveLight(LightInner);
                    LightInner = null;
                }
                if (LightMiddle != null)
                {
                    MyLights.RemoveLight(LightMiddle);
                    LightMiddle = null;
                }
                if (LightOuter != null)
                {
                    MyLights.RemoveLight(LightOuter);
                    LightOuter = null;
                }
            }

            block = null;
            subpart = null;

            NeedsUpdate = MyEntityUpdateEnum.NONE;
        }



        public void IsWorkingChanged(IMyCubeBlock block)
        {
            if (block == null)
            {
                return;
            }

            //subpart = block.GetSubpart(subpartName);
            block.TryGetSubpart(subpartName, out subpart);

            CheckEmissives();
        }



        public void PropertiesChanged(IMyTerminalBlock block)
        {
            if (block == null)
            {
                return;
            }

            //subpart = block.GetSubpart(subpartName);
            block.TryGetSubpart(subpartName, out subpart);

            CheckEmissives();
        }



        // Handle dynamic color changes.
        public override void UpdateAfterSimulation()
        {
            if (block == null || block.MarkedForClose || block.Closed)
            {
                NeedsUpdate = MyEntityUpdateEnum.NONE;
                return;
            }

            bool blockRepainted = false;

            if (BlockColor != block.SlimBlock.ColorMaskHSV)
            {
                BlockColor = block.SlimBlock.ColorMaskHSV;
                blockRepainted = true;
            }

            if (BlockTexture != block.SlimBlock.SkinSubtypeId)
            {
                BlockTexture = block.SlimBlock.SkinSubtypeId;
                blockRepainted = true;
            }

            if (blockRepainted)
            {
                SetEmissives();
            }

            ThrustPercent = block.CurrentThrust / block.MaxThrust;

            for (int i = 0; i < DynamicThrusterData.Count; i++)
            {
                HandleDynamicEmissives(i);
            }

            ThrustPercentLast = ThrustPercent;

            if (StopUpdates)
            {
                if (EXPERIMENTAL)
                {
                    if (LightInner != null)
                    {
                        MyLights.RemoveLight(LightInner);
                        LightInner = null;
                    }
                    if (LightMiddle != null)
                    {
                        MyLights.RemoveLight(LightMiddle);
                        LightMiddle = null;
                    }
                    if (LightOuter != null)
                    {
                        MyLights.RemoveLight(LightOuter);
                        LightOuter = null;
                    }
                }

                // If we ever need/want to stop updates, should be done here.
                // But since Keen didn't supply event for 'block repainted' or bound it to some other event, it has to keep the code updating for manual check.
                NeedsUpdate = MyEntityUpdateEnum.EACH_10TH_FRAME;
            }
        }



        // Because Keen didn't supply event for 'block repainted' or bound it to some other event, I have to keep the code updating.
        public override void UpdateAfterSimulation10()
        {
            bool blockRepainted = false;

            if (BlockColor != block.SlimBlock.ColorMaskHSV)
            {
                BlockColor = block.SlimBlock.ColorMaskHSV;
                blockRepainted = true;
            }

            if (BlockTexture != block.SlimBlock.SkinSubtypeId)
            {
                BlockTexture = block.SlimBlock.SkinSubtypeId;
                blockRepainted = true;
            }

            if (blockRepainted)
            {
                CheckEmissives();
            }
        }



        // Check thrust data and apply accordingly.
        // Any data set checks should be done here.
        void CheckEmissives()
        {
            if (StaticThrusterData.Any())
            {
                SetEmissives();
            }

            if (DynamicThrusterData.Any())
            {
                StopUpdates = false;
                NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME;
            }
        }



        void SetEmissives()
        {
            for (int i = 0; i < StaticThrusterData.Count; i++)
            {
                HandleStaticEmissives(i);
            }
        }



        // Set emissives that are static, that don't change by some thruster values like thrust output.
        void HandleStaticEmissives(int index)
        {
            Color EmissiveColor = Color.Magenta;
            float EmissiveGlow = 0f;

            if (block.IsFunctional)
            {
                if (block.IsWorking)
                {
                    if (block.Enabled)
                    {
                        EmissiveColor = StaticThrusterData[index].OnColor;
                        EmissiveGlow = StaticThrusterData[index].ThrusterOnEmissiveMultiplier;
                    }
                    else
                    {
                        EmissiveColor = StaticThrusterData[index].OffColor;
                        EmissiveGlow = StaticThrusterData[index].ThrusterOffEmissiveMultiplier;
                    }
                }
                else
                {
                    EmissiveColor = StaticThrusterData[index].NotWorkingColor;
                    EmissiveGlow = StaticThrusterData[index].ThrusterNotWorkingEmissiveMultiplier;
                }
            }
            else
            {
                EmissiveColor = StaticThrusterData[index].NonFunctionalColor;
                EmissiveGlow = StaticThrusterData[index].ThrusterNonFunctionalEmissiveMultiplier;
            }

            block.SetEmissiveParts(StaticThrusterData[index].EmissiveMaterialName, EmissiveColor, EmissiveGlow);
            if (subpart != null)
            {
                subpart.SetEmissiveParts(StaticThrusterData[index].EmissiveMaterialName, EmissiveColor, EmissiveGlow);
            }
        }



        // Handle dynamic emissives, that change based on some thruster data like thrust output.
        // Flame and light effects are done here as well.
        void HandleDynamicEmissives(int index)
        {
            if (block.IsFunctional && block.IsWorking && block.Enabled)
            {
                DynamicThrusterData[index].ThrusterStatus = MathHelper.Clamp(DynamicThrusterData[index].ThrusterStatus + DynamicThrusterData[index].ThrusterStatusRampUp, 0f, 1f);

                if (DynamicThrusterData[index].ThrusterStatus == 1)
                {
                    if (ThrustPercent > ThrustPercentLast && DynamicThrusterData[index].ThrusterStrength < ThrustPercent)
                    {
                        DynamicThrusterData[index].ThrusterStrength = MathHelper.Clamp((DynamicThrusterData[index].ThrusterStrength + DynamicThrusterData[index].ThrusterStrengthRampUp), 0f, 1f);
                    }
                    else if (ThrustPercent < ThrustPercentLast && DynamicThrusterData[index].ThrusterStrength > ThrustPercent)
                    {
                        DynamicThrusterData[index].ThrusterStrength = MathHelper.Clamp((DynamicThrusterData[index].ThrusterStrength - DynamicThrusterData[index].ThrusterStrengthRampDown), 0f, 1f);
                    }
                    else
                    {
                        if (DynamicThrusterData[index].ThrusterStrength < ThrustPercent)
                        {
                            DynamicThrusterData[index].ThrusterStrength = MathHelper.Clamp((DynamicThrusterData[index].ThrusterStrength + DynamicThrusterData[index].ThrusterStrengthRampUp), 0f, ThrustPercent);
                        }
                        else if (DynamicThrusterData[index].ThrusterStrength > ThrustPercent)
                        {
                            DynamicThrusterData[index].ThrusterStrength = MathHelper.Clamp((DynamicThrusterData[index].ThrusterStrength - DynamicThrusterData[index].ThrusterStrengthRampDown), ThrustPercent, 1f);
                        }
                    }

                    DynamicThrusterData[index].ActiveGlow = MathHelper.Lerp(DynamicThrusterData[index].ThrusterOnEmissiveMultiplier, DynamicThrusterData[index].MaxThrustEmissiveMultiplierMax, DynamicThrusterData[index].ThrusterStrength);
                    DynamicThrusterData[index].ActiveColor = Color.Lerp(DynamicThrusterData[index].OnColor, DynamicThrusterData[index].ColorAtMaxThrust, DynamicThrusterData[index].ThrusterStrength);

                    // PROTOTYPE
                    if (EXPERIMENTAL)
                    {
                        //var flameColor = Color.Lerp(FlameFullColorMin, FlameFullColorMax, ThrustPercent);
                        var flameColor = Color.Lerp(FlameFullColorMin, FlameFullColorMax, DynamicThrusterData[index].ThrusterStrength);
                        //FlameHandler(flameColor);
                        PLIB.FlameFullHandler(Entity, block, BlockDefinition, flameColor);
                    }
                }
                else
                {
                    DynamicThrusterData[index].ActiveGlow = MathHelper.Lerp(DynamicThrusterData[index].InactiveGlow, DynamicThrusterData[index].ThrusterOnEmissiveMultiplier, DynamicThrusterData[index].ThrusterStatus);
                    DynamicThrusterData[index].ActiveColor = Color.Lerp(DynamicThrusterData[index].InactiveColor, DynamicThrusterData[index].OnColor, DynamicThrusterData[index].ThrusterStatus);
                }

                block.SetEmissiveParts(DynamicThrusterData[index].EmissiveMaterialName, DynamicThrusterData[index].ActiveColor, DynamicThrusterData[index].ActiveGlow);
                if (subpart != null)
                {
                    subpart.SetEmissiveParts(DynamicThrusterData[index].EmissiveMaterialName, DynamicThrusterData[index].ActiveColor, DynamicThrusterData[index].ActiveGlow);
                }

                if (EXPERIMENTAL && UseLights)
                {
                    if (DummyCount > 0)
                    {
                        var strength = DynamicThrusterData[index].ThrusterStrength;

                        var innerColor = Color.Lerp(LightInnerMin, LightInnerMax, strength);
                        var middleColor = Color.Lerp(LightMiddleMin, LightMiddleMax, strength);
                        var outerColor = Color.Lerp(LightOuterMin, LightOuterMax, strength);

                        PLIB.SetLightProperties(ref LightInner, true, innerColor, 0.7f, 2f, 10f * strength);
                        PLIB.SetLightProperties(ref LightMiddle, true, middleColor, 0.8f, 2f, 20f * strength);
                        PLIB.SetLightProperties(ref LightOuter, true, outerColor, 0.9f, 2f, 10f * strength);

                        PLIB.SetLightPosition(ref LightInner, Vector3D.Transform(LightInnerDummyPos, block.WorldMatrix));
                        PLIB.SetLightPosition(ref LightMiddle, Vector3D.Transform(LightMiddleDummyPos, block.WorldMatrix));
                        PLIB.SetLightPosition(ref LightOuter, Vector3D.Transform(LightOuterDummyPos, block.WorldMatrix));
                    }

                    if (subpart != null)
                    {
                        PLIB.ApplySubpartRotations(subpart, MatrixRotator, Matrix.Zero, Matrix.Zero);
                    }
                }
            }
            else
            {
                //DynamicThrusterData[index].ThrusterStatus = MathHelper.Clamp(DynamicThrusterData[index].ThrusterStatus - DynamicThrusterData[index].ThrusterStatusRampDown, 0f, 1f);

                DynamicThrusterData[index].ThrusterStrength = 0f;

                if (!block.IsFunctional)
                {
                    DynamicThrusterData[index].ThrusterStatus = MathHelper.Clamp(DynamicThrusterData[index].ThrusterStatus - DynamicThrusterData[index].ThrusterNonFunctionalRampDown, 0f, 1f);
                    DynamicThrusterData[index].InactiveGlow = MathHelper.Lerp(DynamicThrusterData[index].ThrusterNonFunctionalEmissiveMultiplier, DynamicThrusterData[index].ActiveGlow, DynamicThrusterData[index].ThrusterStatus);
                    DynamicThrusterData[index].InactiveColor = Color.Lerp(DynamicThrusterData[index].NonFunctionalColor, DynamicThrusterData[index].ActiveColor, DynamicThrusterData[index].ThrusterStatus);
                }
                else if (!block.Enabled)
                {
                    DynamicThrusterData[index].ThrusterStatus = MathHelper.Clamp(DynamicThrusterData[index].ThrusterStatus - DynamicThrusterData[index].ThrusterOffRampDown, 0f, 1f);
                    DynamicThrusterData[index].InactiveGlow = MathHelper.Lerp(DynamicThrusterData[index].ThrusterOffEmissiveMultiplier, DynamicThrusterData[index].ActiveGlow, DynamicThrusterData[index].ThrusterStatus);
                    DynamicThrusterData[index].InactiveColor = Color.Lerp(DynamicThrusterData[index].OffColor, DynamicThrusterData[index].ActiveColor, DynamicThrusterData[index].ThrusterStatus);
                }
                else
                {
                    DynamicThrusterData[index].ThrusterStatus = MathHelper.Clamp(DynamicThrusterData[index].ThrusterStatus - DynamicThrusterData[index].ThrusterNotWorkingRampDown, 0f, 1f);
                    DynamicThrusterData[index].InactiveGlow = MathHelper.Lerp(DynamicThrusterData[index].ThrusterNotWorkingEmissiveMultiplier, DynamicThrusterData[index].ActiveGlow, DynamicThrusterData[index].ThrusterStatus);
                    DynamicThrusterData[index].InactiveColor = Color.Lerp(DynamicThrusterData[index].NotWorkingColor, DynamicThrusterData[index].ActiveColor, DynamicThrusterData[index].ThrusterStatus);
                }

                block.SetEmissiveParts(DynamicThrusterData[index].EmissiveMaterialName, DynamicThrusterData[index].InactiveColor, DynamicThrusterData[index].InactiveGlow);
                if (subpart != null)
                {
                    subpart.SetEmissiveParts(DynamicThrusterData[index].EmissiveMaterialName, DynamicThrusterData[index].InactiveColor, DynamicThrusterData[index].InactiveGlow);
                }

                if (DynamicThrusterData[index].ThrusterStatus == 0)
                {
                    DynamicThrusterData[index].ThrusterStrength = 0f;
                    StopUpdates = true;
                }

                // PROTOTYPE
                if (EXPERIMENTAL)
                {
                    //var flameColor = Color.Lerp(FlameFullColorMin, FlameFullColorMax, ThrustPercent);
                    var flameColor = Color.Lerp(FlameFullColorMin, FlameFullColorMax, DynamicThrusterData[index].ThrusterStatus);
                    //FlameHandler(flameColor);
                    PLIB.FlameFullHandler(Entity, block, BlockDefinition, flameColor);
                }
            }
        }
    }
}
