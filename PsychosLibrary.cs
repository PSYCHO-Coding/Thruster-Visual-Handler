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

// PSYCHO'S WRAPPED LIBRARY
// PUT THIS IN YOUR CODE TO HAVE ACCESS TO THE METHODS
// PsychoLibrary PLIB = new PsychoLibrary();
// THEN USE LIKE THIS:
// PLIB.SomeMethod();

// DO NOT EDIT IF YOU DON'T KNOW WHAT YOU'RE DOING.

namespace PSYCHO
{
    public class PsychosLibrary
    {
        // ===============================================================================================================================================================================================
        /// <summary>
        /// Set on/off light.
        /// </summary>
        /// <param name="light"></param>
        /// <param name="enabled"></param>
        public void LightOnOff(ref MyLight light, bool enabled)
        {
            if (light == null)
                return;

            light.LightOn = enabled;
            light.UpdateLight();
        }

        /// <summary>
        /// Toggle on/off light.
        /// </summary>
        /// <param name="light"></param>
        public void LightOnOff(ref MyLight light)
        {
            if (light == null)
                return;

            light.LightOn = !light.LightOn;
            light.UpdateLight();
        }

        /// <summary>
        /// Set position of the light.
        /// </summary>
        /// <param name="light"></param>
        /// <param name="position">World coords.</param>
        public void SetLightPosition(ref MyLight light, Vector3D position)
        {
            if (light == null)
            {
                return;
            }

            light.Position = position; //Updates the lights position constantly. You'll need help if you want it somewhere else.
            light.UpdateLight(); //Ignore - tells the game to update the light.
        }

        /// <summary>
        /// Creates a light if null and set its properties. (Experimental). Note, you have to keep track of the light and remove it manually on block close etc.
        /// </summary>
        /// <param name="light"></param>
        /// <param name="enabled"></param>
        /// <param name="color"></param>
        /// <param name="range"></param>
        /// <param name="intensity"></param>
        /// <param name="falloff"></param>
        /// <param name="offset"></param>
        public void SetLightProperties(ref MyLight light, bool enabled = true, Color? color = null, float range = 5f, float falloff = 1f, float intensity = 5f, float offset = 0f)
        {
            var finalColor = color ?? Color.White;

            if (light == null)
            {
                light = new MyLight();
                light = MyLights.AddLight();

                //light.Start(position, finalColor, range, "");
                light.Start("");
            }

            light.LightOn = enabled;
            light.Color = finalColor;
            light.Range = range;
            light.Falloff = falloff;
            light.Intensity = intensity;
            light.PointLightOffset = offset;

            light.UpdateLight();
        }

        /// <summary>
        /// Creates a light. (Experimental). Note, you have to keep track of the light and remove it manually on block close etc.
        /// </summary>
        /// <param name="light"></param>
        /// <param name="enabled"></param>
        /// <param name="color"></param>
        /// <param name="range"></param>
        /// <param name="intensity"></param>
        /// <param name="falloff"></param>
        /// <param name="offset"></param>
        public void AddLight(ref MyLight light, bool enabled = true, Color? color = null, float range = 5f, float intensity = 5f, float falloff = 1f, float offset = 0f)
        {
            if (light != null)
            {
                return;
            }

            var finalColor = color ?? Color.White;

            light = new MyLight();
            light = MyLights.AddLight();

            light.Start("");

            light.LightOn = enabled;
            light.Color = finalColor;
            light.Range = range;
            light.Intensity = intensity;
            light.Falloff = falloff;
            light.PointLightOffset = offset;


            light.UpdateLight();
        }

        public void RemoveLight(ref MyLight light)
        {
            if (light == null)
                return;

            MyLights.RemoveLight(light);
            light = null;
        }



        // ===============================================================================================================================================================================================
        // THRUSTER FLAMES
        // I know you can define this in code but...
        // For a super lazy approach:
        /*
            var ThrusterDef = GetThrusterDefinition(Block);
            if (if ThrusterDef != null)
            {
                FlameFullHandler(Entity, Block, ThrusterDef, SomeColor)
            }
        */
        // Remarks, since flames are applied globally to all flames, you can cache the definition.

        public MyThrustDefinition GetThrusterDefinition(IMyThrust block)
        {
            var thruster = block as MyThrust;
            return thruster.BlockDefinition;
        }

        public void FlameHandler(IMyEntity entity, IMyThrust block, MyThrustDefinition blockDefinition, Color colorIdle, Color colorFull)
        {
            if (entity == null || block.CubeGrid.Physics == null)
                return;

            var thruster = block as MyThrust;
            if (thruster == null || thruster.CubeGrid.Physics == null)
                return;

            uint renderObjectID = entity.Render.GetRenderObjectID();
            if (renderObjectID == 4294967295u)
                return;

            //MyThrustDefinition blockDefinition = thruster.BlockDefinition;

            //if (thruster.CurrentStrength > 0.001f)

            blockDefinition.FlameIdleColor = colorIdle;
            blockDefinition.FlameFullColor = colorFull;

            ((MyRenderComponentThrust)thruster.Render).UpdateFlameAnimatorData();
        }

        public void FlameIdleHandler(IMyEntity entity, IMyTerminalBlock block, MyThrustDefinition blockDefinition, Color color)
        {
            if (entity == null)
                return;

            var thruster = block as MyThrust;
            if (thruster == null || thruster.CubeGrid.Physics == null)
                return;

            uint renderObjectID = entity.Render.GetRenderObjectID();
            if (renderObjectID == 4294967295u)
                return;

            //MyThrustDefinition blockDefinition = thruster.BlockDefinition;

            //if (thruster.CurrentStrength > 0.001f)

            blockDefinition.FlameIdleColor = color;
            ((MyRenderComponentThrust)thruster.Render).UpdateFlameAnimatorData();
        }

        public void FlameFullHandler(IMyEntity entity, IMyTerminalBlock block, MyThrustDefinition blockDefinition, Color color)
        {
            if (entity == null)
                return;

            var thruster = block as MyThrust;
            if (thruster == null || thruster.CubeGrid.Physics == null)
                return;

            uint renderObjectID = entity.Render.GetRenderObjectID();
            if (renderObjectID == 4294967295u)
                return;

            //MyThrustDefinition blockDefinition = thruster.BlockDefinition;

            //if (thruster.CurrentStrength > 0.001f)

            blockDefinition.FlameFullColor = color;
            ((MyRenderComponentThrust)thruster.Render).UpdateFlameAnimatorData();
        }



        // ===============================================================================================================================================================================================
        // MATRIX ROTATION METHODS // TAGS @matrix @rotations @subpart
        /// <summary>
        /// Compute a rotation matrix.
        /// </summary>
        /// <param name="rotationAxis">1=X, 2=Y and 3=Z.</param>
        /// <param name="rotator">In radians.</param>
        /// <param name="neutral">Not working?</param>
        /// <returns></returns>
        public Matrix ComputeRotationMatrix(int rotationAxis, float rotator, bool neutral = false)
        {
            Matrix rotationMatrix = Matrix.Zero;

            if (!neutral)
            {
                switch (rotationAxis)
                {
                    default:
                        rotationMatrix = Matrix.CreateRotationX(rotator);
                        break;
                    case 2:
                        rotationMatrix = Matrix.CreateRotationY(rotator);
                        break;
                    case 3:
                        rotationMatrix = Matrix.CreateRotationZ(rotator);
                        break;
                }
            }
            else
            {
                switch (rotationAxis)
                {
                    default:
                        rotationMatrix = Matrix.Identity;
                        break;
                    case 2:
                        rotationMatrix = Matrix.Identity;
                        break;
                    case 3:
                        rotationMatrix = Matrix.Identity;
                        break;
                }
            }

            return rotationMatrix;
        }

        /// <summary>
        /// Apply constant rotator onto a subpart.
        /// </summary>
        /// <param name="subpart">Targeted subpart.</param>
        /// <param name="rotMatrixX">Rotation matrix on the X axis.</param>
        /// <param name="rotMatrixY">Rotation matrix on the Y axis.</param>
        /// <param name="rotMatrixZ">Rotation matrix on the Z axis.</param>
        /// <param name="pivotPosX">Pivot position on the X axis.</param>
        /// <param name="pivotPosY">Pivot position on the Y axis.</param>
        /// <param name="pivotPosZ">Pivot position on the Z axis.</param>
        public void ApplySubpartRotations(MyEntitySubpart subpart, Matrix rotMatrixX, Matrix rotMatrixY, Matrix rotMatrixZ, float pivotPosX = 0f, float pivotPosY = 0f, float pivotPosZ = 0f)
        {
            var pivotPos = new Vector3(pivotPosX, pivotPosY, pivotPosZ); // This defines the location of a new pivot point.
            var MatrixTransl1 = Matrix.CreateTranslation(-(pivotPos));
            var MatrixTransl2 = Matrix.CreateTranslation(pivotPos);
            var rotMatrix = subpart.PositionComp.LocalMatrix;
            //rotMatrix *= (MatrixTransl1 * (Matrix.CreateRotationX(Rotator) * Matrix.CreateRotationY(RotY) * Matrix.CreateRotationZ(RotZ)) * MatrixTransl2);
            rotMatrix *= (MatrixTransl1 * rotMatrixX * rotMatrixY * rotMatrixZ * MatrixTransl2);
            subpart.PositionComp.LocalMatrix = rotMatrix;
            //MyAPIGateway.Utilities.ShowNotification(Rotator.ToString(), 1);
        }



        // ===============================================================================================================================================================================================
    }
}
