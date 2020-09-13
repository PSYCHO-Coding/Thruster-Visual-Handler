using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Sandbox.Common;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game;
using VRage.Common.Utils;
using VRageMath;
using VRage;
using VRage.Game;
using VRage.ObjectBuilders;
using VRage.Game.Components;
using Sandbox.ModAPI.Interfaces.Terminal;
using VRage.ModAPI;
using VRage.Utils;
using VRage.Library.Utils;
using VRage.Game.ModAPI;
using Sandbox.Game.EntityComponents;
using VRage.Input;
using Sandbox.Game.GameSystems;
using VRage.Game.VisualScripting;
using Sandbox.Game.World;
using Sandbox.Game.Components;
using VRageRender.Animations;
using SpaceEngineers.Game.ModAPI;
using ProtoBuf;
using BlendTypeEnum = VRageRender.MyBillboard.BlendTypeEnum;
using Sandbox.ModAPI.Weapons;
using System.ComponentModel.Design;
using PSYCHO;
using MyVisualScriptLogicProvider = Sandbox.Game.MyVisualScriptLogicProvider;

using Sandbox.Game.Lights;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.ObjectBuilders.Definitions;

using IMyControllableEntity = Sandbox.Game.Entities.IMyControllableEntity;
using Sandbox.Game.Entities.Character.Components;
using VRage.Game.SessionComponents;
using System.Data;
using VRage.Game.VisualScripting.Missions;

using PSYCHO.ThrusterVisualHandlerData;

namespace PSYCHO.ThrusterUpgrade
{
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_UpgradeModule), false, "SuperThrusterUpgrade")]

    public class ThrusterUpgradeBlocks : MyGameLogicComponent
    {
        ThrusterDataHandler ThrusterDataInstance => ThrusterDataHandler.ThrusterDataInstance;

        private IMyUpgradeModule m_upgrade;
        public Color UpgradeEmissiveColor = new Color(0, 0, 0);

        Vector3 BlockColor = Vector3.Zero;
        MyStringHash BlockTexture;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            m_upgrade = Entity as IMyUpgradeModule;

            NeedsUpdate = MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
        }

        private void PropertiesChanged(IMyTerminalBlock obj)
        {
            MyAPIGateway.Utilities.ShowNotification("PropertiesChanged");
            CheckAndSetEmissives();
        }

        private void IsWorkingChanged(IMyCubeBlock obj)
        {
            CheckAndSetEmissives();
        }

        public override void Close()
        {
            NeedsUpdate = MyEntityUpdateEnum.NONE;

            m_upgrade.PropertiesChanged -= PropertiesChanged;
            m_upgrade.IsWorkingChanged -= IsWorkingChanged;

            if (ThrusterDataInstance.Upgraders.Contains(m_upgrade))
                ThrusterDataInstance.Upgraders.Remove(m_upgrade);

            if (ThrusterDataInstance.ThrusterPowerState.ContainsKey(m_upgrade))
                ThrusterDataInstance.ThrusterPowerState.Remove(m_upgrade);

            m_upgrade = null;
        }

        public override void UpdateOnceBeforeFrame()
        {
            if (m_upgrade == null)
                return;

            BlockColor = m_upgrade.SlimBlock.ColorMaskHSV;
            BlockTexture = m_upgrade.SlimBlock.SkinSubtypeId;

            m_upgrade.PropertiesChanged += PropertiesChanged;
            m_upgrade.IsWorkingChanged += IsWorkingChanged;

            m_upgrade.SetEmissiveParts("Emissive2", new Color(242, 110, 80), 10f);

            //m_upgrade.SetEmissiveParts("EmissiveUpgrade", Color.AliceBlue, 1f);

            CheckAndSetEmissives();

            NeedsUpdate = MyEntityUpdateEnum.EACH_100TH_FRAME;
        }

        public override void UpdateAfterSimulation100()
        {
            bool updateEmissive = false;

            if (BlockColor != m_upgrade.SlimBlock.ColorMaskHSV)
            {
                updateEmissive = true;
            }

            if (BlockTexture != m_upgrade.SlimBlock.SkinSubtypeId)
            {
                updateEmissive = true;
            }

            if (ThrusterDataInstance.ThrusterPowerState.ContainsKey(m_upgrade))
            {
                updateEmissive = true;
            }

            if (updateEmissive)
            {
                CheckAndSetEmissives();
            }
        }

        void CheckAndSetEmissives()
        {
            if (m_upgrade.IsFunctional)
            {
                //if (m_upgrade.IsWorking)
                if (ThrusterDataInstance.ThrusterPowerState.ContainsKey(m_upgrade) && ThrusterDataInstance.ThrusterPowerState[m_upgrade] == true)
                {
                    if (m_upgrade.Enabled)
                    {
                        m_upgrade.SetEmissiveParts("Emissive2", new Color(242, 110, 80), 10f);
                        //m_upgrade.SetEmissiveParts("EmissiveUpgrade", new Color(40, 110, 255), 1f);

                        if (ThrusterDataInstance.Upgraders.Contains(m_upgrade))
                            m_upgrade.SetEmissiveParts("EmissiveUpgrade", Color.LightGreen, 1f);
                        else
                            m_upgrade.SetEmissiveParts("EmissiveUpgrade", new Color(40, 110, 255), 1f);
                    }
                    else
                    {
                        m_upgrade.SetEmissiveParts("Emissive2", Color.Black, 0f);
                        m_upgrade.SetEmissiveParts("EmissiveUpgrade", Color.Red, 1f);
                    }
                }
                else
                {
                    m_upgrade.SetEmissiveParts("Emissive2", Color.Black, 0f);
                    m_upgrade.SetEmissiveParts("EmissiveUpgrade", Color.Red, 1f); // Should check only for power and be black but since it's mothafukin Keen...
                }
            }
            else
            {
                m_upgrade.SetEmissiveParts("Emissive2", Color.Black, 0f);
                m_upgrade.SetEmissiveParts("EmissiveUpgrade", Color.Black, 0f);
            }
        }
    }

    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_Thrust), false, "SuperThruster_Small")]

    public class ThrusterUpgradeLogic : MyGameLogicComponent
    {
        ThrusterDataHandler ThrusterDataInstance => ThrusterDataHandler.ThrusterDataInstance;

        private IMyThrust m_thrust;
        private IMyCubeBlock m_parent;
        private MyCubeBlock m_cube;
        //private IMyCubeBlock m_upgrade;
        //private IMyUpgradeModule m_upgrade;
        private IMyUpgradeModule m_upgrade;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            base.Init(objectBuilder);

            m_thrust = Entity as IMyThrust;
            m_parent = Entity as IMyCubeBlock;
            m_cube = Entity as MyCubeBlock;

            NeedsUpdate = MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
        }

        public override void UpdateOnceBeforeFrame()
        {
            if (m_thrust == null)
                return;

            m_thrust.SetEmissiveParts("Emissive2", new Color(242, 110, 80), 10f);
            m_thrust.SetEmissiveParts("EmissiveUpgrade", new Color(40, 110, 255), 1f);

            m_parent.AddUpgradeValue("Thrust", 0f);

            m_thrust.IsWorkingChanged += IsWorkingChanged;
            m_parent.OnUpgradeValuesChanged += OnUpgradeValuesChanged;

            //NeedsUpdate = MyEntityUpdateEnum.EACH_100TH_FRAME;
        }

        private void IsWorkingChanged(IMyCubeBlock obj)
        {
            CheckAndSetEmissives();
        }

        void CheckAndSetEmissives()
        {
            if (!ThrusterDataInstance.Upgraders.Contains(m_upgrade))
            {
                ThrusterDataInstance.Upgraders.Add(m_upgrade);

                if (m_upgrade.IsFunctional && m_thrust.IsWorking && m_upgrade.Enabled)
                    m_upgrade.SetEmissiveParts("EmissiveUpgrade", Color.LightGreen, 1f);
            }

            bool isWorking = false;

            if (m_thrust.IsWorking)
            {
                isWorking = true;
            }

            if (m_upgrade != null)
                ThrusterDataInstance.ThrusterPowerState[m_upgrade] = isWorking;
        }

        public override void Close()
        {
            NeedsUpdate = MyEntityUpdateEnum.NONE;

            m_thrust.IsWorkingChanged -= IsWorkingChanged;
            m_parent.OnUpgradeValuesChanged -= OnUpgradeValuesChanged;

            if (m_upgrade != null)
            {
                m_upgrade.SetEmissiveParts("EmissiveUpgrade", new Color(40, 110, 255), 1f);

                if (ThrusterDataInstance.Upgraders.Contains(m_upgrade))
                {
                    ThrusterDataInstance.Upgraders.Remove(m_upgrade);
                }

                if (ThrusterDataInstance.ThrusterPowerState.ContainsKey(m_upgrade))
                {
                    ThrusterDataInstance.ThrusterPowerState.Remove(m_upgrade);
                }

                m_upgrade = null;
            }

            m_thrust = null;
            m_parent = null;
        }

        private void OnUpgradeValuesChanged()
        {
            if (m_thrust == null || m_parent == null)
                return;

            if (m_parent.UpgradeValues.ContainsKey("Thrust"))
            {
                m_thrust.ThrustMultiplier = m_parent.UpgradeValues["Thrust"] + 1f;
                m_thrust.PowerConsumptionMultiplier = m_parent.UpgradeValues["Thrust"] + 1f;
            }

            if (m_cube != null)
            {
                Dictionary<long, MyCubeBlock.AttachedUpgradeModule> upgrades = m_cube.CurrentAttachedUpgradeModules;

                if (upgrades.Count != 0)
                {
                    m_thrust.SetEmissiveParts("EmissiveUpgrade", Color.LightGreen, 1f);
                    foreach (var upgradeRelation in upgrades)
                    {
                        if (upgradeRelation.Value.Block.BlockDefinition.SubtypeId == "SuperThrusterUpgrade")
                        {
                            m_upgrade = upgradeRelation.Value.Block;

                            CheckAndSetEmissives();

                            break;
                        }
                    }
                }
                else
                {
                    m_thrust.SetEmissiveParts("EmissiveUpgrade", new Color(40, 110, 255), 1f);
                }
            }
        }



        public override void UpdateBeforeSimulation100()
        {
            if (m_thrust == null || m_thrust.MarkedForClose || m_thrust.Closed)
            {
                m_parent.OnUpgradeValuesChanged -= OnUpgradeValuesChanged;
                NeedsUpdate = MyEntityUpdateEnum.NONE;
                return;
            }

            //m_thrust.SetEmissiveParts("EmissiveUpgrade", Color.AliceBlue, 1f);

            //m_thrust.SetEmissiveParts("EmissiveUpgrade", Color.AliceBlue, 1f);

            // Attempt at changing the upgrade block that upgrades the thruster change it's upgrade emissive indicators.

            /*
            */
            //var pos = m_thrust.CubeGrid.WorldToGridInteger(m_thrust.GetPosition() + m_thrust.WorldMatrix.Backward * 1.5);
            /*
            var pos = m_thrust.CubeGrid.WorldToGridInteger(m_thrust.SlimBlock.Position + m_thrust.WorldMatrix.Backward * 1.5);

            //m_upgrade = m_thrust.CubeGrid.GetCubeBlock(pos).FatBlock as IMyUpgradeModule;

            IMySlimBlock hitBlock = m_thrust.CubeGrid.GetCubeBlock(pos);

            if (hitBlock != null)
            {
                m_upgrade = hitBlock as IMyTerminalBlock;
            }

            if (m_upgrade != null)
            {
                var checkBlock = m_upgrade as IMyCubeBlock;
                if (checkBlock != null)
                {
                    if (checkBlock.BlockDefinition.SubtypeId == "SuperThrusterUpgrade")
                        m_upgrade.SetEmissiveParts("EmissiveUpgrade", Color.LightGreen, 1f);
                }
            }

            if (m_upgrade != null)
            {
                MyAPIGateway.Utilities.ShowNotification("Upgrade name: " + m_upgrade.CustomName, 1);
            }
            else
            {
                MyAPIGateway.Utilities.ShowNotification("Upgrade is null.", 1);
            }
            */

            /*
            var material = MyStringId.GetOrCompute("Square");
            var color = Color.Black.ToVector4();

            var distance = 1.5f;

            color = Color.Red.ToVector4();
            MySimpleObjectDraw.DrawLine(m_thrust.GetPosition() + m_thrust.WorldMatrix.Backward * distance, m_thrust.GetPosition() + m_thrust.WorldMatrix.Backward * distance + m_thrust.WorldMatrix.Right * 1,
            material, ref color, 0.01f, BlendTypeEnum.PostPP);

            color = Color.Green.ToVector4();
            MySimpleObjectDraw.DrawLine(m_thrust.GetPosition() + m_thrust.WorldMatrix.Backward * distance, m_thrust.GetPosition() + m_thrust.WorldMatrix.Backward * distance + m_thrust.WorldMatrix.Backward * 1,
            material, ref color, 0.01f, BlendTypeEnum.PostPP);

            color = Color.Blue.ToVector4();
            MySimpleObjectDraw.DrawLine(m_thrust.GetPosition() + m_thrust.WorldMatrix.Backward * distance, m_thrust.GetPosition() + m_thrust.WorldMatrix.Backward * distance + m_thrust.WorldMatrix.Up * 1,
            material, ref color, 0.01f, BlendTypeEnum.PostPP);
            */
        }
    }
}
