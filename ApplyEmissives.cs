using Sandbox.ModAPI;
using System.Linq;
using System.Threading.Tasks;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Interfaces.Terminal;
using SpaceEngineers.Game.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRageMath;
using VRage.Utils;

// Can't figure out how to get actual cube blocks that don't have terminal access and aren't part of a specific block type.
// Commented out for the time being.

// Only if we want to use it in the update per frame.
/*
// In Class field.
private int tick;

// In update per frame.
tick++;

if (tick % 3 == 0)
{
    if (BlockColor != block.GetDiffuseColor())
    {
        BlockColor = block.GetDiffuseColor();
        block.SetEmissiveParts("Emissive2", new Color(242, 110, 80), 10f);
    }
}
*/

namespace PSYCHO.ApplyEmissives
{
    /*
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_Cockpit), false, "HK_Cockpit_Small", "HK_Cockpit_Small2")]

    public class ApplyEmissivesLogic : MyGameLogicComponent
    {
        private int tick;
        IMyCockpit block;
        Vector3 BlockColor = Vector3.Zero;
        MyStringHash BlockTexture;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            block = Entity as IMyCockpit;

            NeedsUpdate = MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
        }

        public override void UpdateOnceBeforeFrame()
        {
            if (block == null)
                return;

            block.SetEmissiveParts("Emissive2", new Color(242, 110, 80), 10f);

            NeedsUpdate = MyEntityUpdateEnum.EACH_100TH_FRAME;
        }

        public override void UpdateBeforeSimulation100()
        {
            if (block == null)
            {
                NeedsUpdate = MyEntityUpdateEnum.NONE;
                return;
            }
            */
            // Only if we want to use it within some frame set not available natively.
            /*
            tick++;

            if (tick % 3 == 0)
            {
                if (BlockColor != block.GetDiffuseColor())
                {
                    BlockColor = block.GetDiffuseColor();
                    block.SetEmissiveParts("Emissive2", new Color(242, 110, 80), 10f);
                }
            }
            */
            /*
            if (BlockColor != block.SlimBlock.ColorMaskHSV)
            {
                BlockColor = block.SlimBlock.ColorMaskHSV;
                block.SetEmissiveParts("Emissive2", new Color(242, 110, 80), 10f);
            }

            if (BlockTexture != block.SlimBlock.SkinSubtypeId)
            {
                BlockTexture = block.SlimBlock.SkinSubtypeId;
                block.SetEmissiveParts("Emissive2", new Color(242, 110, 80), 10f);
            }
        }
    }
    */

    // IGNORE THIS!
    /*
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_Cockpit), false, "SubtypeID")]

    public class ApplyEmissivesLogic : MyGameLogicComponent
    {
        private int tick;
        IMyCockpit block;
        Vector3 BlockColor = Vector3.Zero;
        MyStringHash BlockTexture;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            block = Entity as IMyCockpit;

            NeedsUpdate = MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
        }

        public override void UpdateOnceBeforeFrame()
        {
            if (block == null)
                return;

            block.SetEmissiveParts("Emissive2", new Color(242, 110, 80), 10f);

            NeedsUpdate = MyEntityUpdateEnum.EACH_100TH_FRAME;
        }

        public override void UpdateBeforeSimulation100()
        {
            if (block == null)
            {
                NeedsUpdate = MyEntityUpdateEnum.NONE;
                return;
            }

            if (BlockColor != block.SlimBlock.ColorMaskHSV)
            {
                BlockColor = block.SlimBlock.ColorMaskHSV;
                block.SetEmissiveParts("Emissive2", new Color(242, 110, 80), 10f);
            }

            if (BlockTexture != block.SlimBlock.SkinSubtypeId)
            {
                BlockTexture = block.SlimBlock.SkinSubtypeId;
                block.SetEmissiveParts("Emissive2", new Color(242, 110, 80), 10f);
            }
        }
    }
    */

    /*
    [MyEntityComponentDescriptor(typeof(MyCubeBuilder), false, "SuperThrusterShieldLarge", "SuperThrusterShieldSmall")]

    public class ApplyEmissivesLogic : MyGameLogicComponent
    {
        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            var block = Entity as IMyCubeBlock;

            if (block == null)
                return;

            block.SetEmissiveParts("Emissive2", new Color(242, 110, 80), 10f);
        }
    }
    */
}
