/*
 * Copyright 2008 Adam Witczak, Jakub T�ycki, Kamil S�awi�ski, Tomasz Bilski, Emil Hornung, Micha� Ziober
 *
 * This file is part of Wings Of Fury 2.
 * 
 * Freeware Licence Agreement
 * 
 * This licence agreement only applies to the free version of this software.
 * Terms and Conditions
 * 
 * BY DOWNLOADING, INSTALLING, USING, TRANSMITTING, DISTRIBUTING OR COPYING THIS SOFTWARE ("THE SOFTWARE"), YOU AGREE TO THE TERMS OF THIS AGREEMENT (INCLUDING THE SOFTWARE LICENCE AND DISCLAIMER OF WARRANTY) WITH WINGSOFFURY2.COM THE OWNER OF ALL RIGHTS IN RESPECT OF THE SOFTWARE.
 * 
 * PLEASE READ THIS DOCUMENT CAREFULLY BEFORE USING THE SOFTWARE.
 *  
 * IF YOU DO NOT AGREE TO ANY OF THE TERMS OF THIS LICENCE THEN DO NOT DOWNLOAD, INSTALL, USE, TRANSMIT, DISTRIBUTE OR COPY THE SOFTWARE.
 * 
 * THIS DOCUMENT CONSTITUES A LICENCE TO USE THE SOFTWARE ON THE TERMS AND CONDITIONS APPEARING BELOW.
 * 
 * The Software is licensed to you without charge for use only upon the terms of this licence, and WINGSOFFURY2.COM reserves all rights not expressly granted to you. WINGSOFFURY2.COM retains ownership of all copies of the Software.
 * 1. Licence
 * 
 * You may use the Software without charge.
 *  
 * You may distribute exact copies of the Software to anyone.
 * 2. Restrictions
 * 
 * WINGSOFFURY2.COM reserves the right to revoke the above distribution right at any time, for any or no reason.
 *  
 * YOU MAY NOT MODIFY, ADAPT, TRANSLATE, RENT, LEASE, LOAN, SELL, REQUEST DONATIONS OR CREATE DERIVATE WORKS BASED UPON THE SOFTWARE OR ANY PART THEREOF.
 * 
 * The Software contains trade secrets and to protect them you may not decompile, reverse engineer, disassemble or otherwise reduce the Software to a humanly perceivable form. You agree not to divulge, directly or indirectly, until such trade secrets cease to be confidential, for any reason not your own fault.
 * 3. Termination
 * 
 * This licence is effective until terminated. The Licence will terminate automatically without notice from WINGSOFFURY2.COM if you fail to comply with any provision of this Licence. Upon termination you must destroy the Software and all copies thereof. You may terminate this Licence at any time by destroying the Software and all copies thereof. Upon termination of this licence for any reason you shall continue to be bound by the provisions of Section 2 above. Termination will be without prejudice to any rights WINGSOFFURY2.COM may have as a result of this agreement.
 * 4. Disclaimer of Warranty, Limitation of Remedies
 * 
 * TO THE FULL EXTENT PERMITTED BY LAW, WINGSOFFURY2.COM HEREBY EXCLUDES ALL CONDITIONS AND WARRANTIES, WHETHER IMPOSED BY STATUTE OR BY OPERATION OF LAW OR OTHERWISE, NOT EXPRESSLY SET OUT HEREIN. THE SOFTWARE, AND ALL ACCOMPANYING FILES, DATA AND MATERIALS ARE DISTRIBUTED "AS IS" AND WITH NO WARRANTIES OF ANY KIND, WHETHER EXPRESS OR IMPLIED. WINGSOFFURY2.COM DOES NOT WARRANT, GUARANTEE OR MAKE ANY REPRESENTATIONS REGARDING THE USE, OR THE RESULTS OF THE USE, OF THE SOFTWARE WITH RESPECT TO ITS CORRECTNESS, ACCURACY, RELIABILITY, CURRENTNESS OR OTHERWISE. THE ENTIRE RISK OF USING THE SOFTWARE IS ASSUMED BY YOU. WINGSOFFURY2.COM MAKES NO EXPRESS OR IMPLIED WARRANTIES OR CONDITIONS INCLUDING, WITHOUT LIMITATION, THE WARRANTIES OF MERCHANTABILITY OR FITNESS FOR A PARTICULAR PURPOSE WITH RESPECT TO THE SOFTWARE. NO ORAL OR WRITTEN INFORMATION OR ADVICE GIVEN BY WINGSOFFURY2.COM, IT'S DISTRIBUTORS, AGENTS OR EMPLOYEES SHALL CREATE A WARRANTY, AND YOU MAY NOT RELY ON ANY SUCH INFORMATION OR ADVICE.
 * 
 * IMPORTANT NOTE: Nothing in this Agreement is intended or shall be construed as excluding or modifying any statutory rights, warranties or conditions which by virtue of any national or state Fair Trading, Trade Practices or other such consumer legislation may not be modified or excluded. If permitted by such legislation, however, WINGSOFFURY2.COM' liability for any breach of any such warranty or condition shall be and is hereby limited to the supply of the Software licensed hereunder again as WINGSOFFURY2.COM at its sole discretion may determine to be necessary to correct the said breach.
 * 
 * IN NO EVENT SHALL WINGSOFFURY2.COM BE LIABLE FOR ANY SPECIAL, INCIDENTAL, INDIRECT OR CONSEQUENTIAL DAMAGES (INCLUDING, WITHOUT LIMITATION, DAMAGES FOR LOSS OF BUSINESS PROFITS, BUSINESS INTERRUPTION, AND THE LOSS OF BUSINESS INFORMATION OR COMPUTER PROGRAMS), EVEN IF WINGSOFFURY2.COM OR ANY WINGSOFFURY2.COM REPRESENTATIVE HAS BEEN ADVISED OF THE POSSIBILITY OF SUCH DAMAGES. IN ADDITION, IN NO EVENT DOES WINGSOFFURY2.COM AUTHORISE YOU TO USE THE SOFTWARE IN SITUATIONS WHERE FAILURE OF THE SOFTWARE TO PERFORM CAN REASONABLY BE EXPECTED TO RESULT IN A PHYSICAL INJURY, OR IN LOSS OF LIFE. ANY SUCH USE BY YOU IS ENTIRELY AT YOUR OWN RISK, AND YOU AGREE TO HOLD WINGSOFFURY2.COM HARMLESS FROM ANY CLAIMS OR LOSSES RELATING TO SUCH UNAUTHORISED USE.
 * 5. General
 * 
 * All rights of any kind in the Software which are not expressly granted in this Agreement are entirely and exclusively reserved to and by WINGSOFFURY2.COM.
 * 
 * 
 */

using System;
using FSLOgreCS;
using Mogre;
using Wof.Controller;
using Wof.Misc;
using Wof.Model.Level.Planes;
using Wof.View.NodeAnimation;
using Plane = Wof.Model.Level.Planes.Plane;

namespace Wof.View
{
    /// <summary>
    /// Wrogi samolot w view
    /// </summary>
    public class EnemyBomberView : EnemyPlaneViewBase
    {

        protected Entity airscrewL;
        protected Entity airscrewR;
        private static int enemyBomberCounter = 1;
        protected Entity bladeL, bladeR;

        public EnemyBomberView(Plane plane, IFrameWork frameWork, SceneNode parentNode)
            : base(plane, frameWork, parentNode, "EnemyBomber" + enemyBomberCounter.ToString())
        {
            //nazwa musi byc unikalnym stringiem
            enemyBomberCounter++;
            random = new Random();
            if (LevelView.IsNightScene)
            {
                InitLight(innerNode, new ColourValue(0.1f, 0.9f, 0.1f), new Vector3(15.1f, 1.25f, -2.9f),
                          new Vector2(2.0f, 2.0f));
                InitLight(innerNode, new ColourValue(0.1f, 0.9f, 0.1f), new Vector3(-15.1f, 1.25f, -2.9f),
                          new Vector2(2.0f, 2.0f));
            }
            
            animationMgr[PlaneNodeAnimationManager.AnimationType.IDLE].Duration *= 2.0f;
           
        }

        public static new string GetMainMeshName()
        {
            return "Betty.mesh";
        }


        /*public void Dispose()
        {
            if (engineSound != null) engineSound.Destroy();

        }*/
        public override void Destroy()
        {
            base.Destroy();
        }

        public override void ResetCameraHolders()
        {

            cameraHolders.Add(planeNode.CreateChildSceneNode(name + "MainCameraHolder"));
            cameraHolders.Add(planeNode.CreateChildSceneNode(name + "BirdCameraHolder"));

            // MAIN CAMERA HOLDER
            cameraHolders[0].ResetOrientation();
            cameraHolders[0].Position = new Vector3(0, 0, 55);
            cameraHolders[0].LookAt(new Vector3(0, 0, -1), Node.TransformSpace.TS_LOCAL);

            // BIRD CAMERA HOLDER
            cameraHolders[1].ResetOrientation();
            cameraHolders[1].Position = new Vector3(0, 35, 0);
            cameraHolders[1].Pitch(new Radian(-Mogre.Math.HALF_PI));
            //cameraHolders[1].Roll(new Radian(Mogre.Math.HALF_PI));




        }

        protected override void initBlade()
        { // BLADE
            bladeNodeL = innerNode.CreateChildSceneNode(name + "_BladeL", new Vector3(4.00f, -0.15f, -8.3f));
            bladeL = sceneMgr.CreateEntity(name + "_BladeL", "P47Blade.mesh");
            bladeNodeL.AttachObject(bladeL);
            bladeL.Visible = true; // tylko kiedy niskie obroty


            bladeNodeR = innerNode.CreateChildSceneNode(name + "_BladeR", new Vector3(-4.00f, -0.15f, -8.3f));
            bladeR = sceneMgr.CreateEntity(name + "_BladeR", "P47Blade.mesh");
            bladeNodeR.AttachObject(bladeR);
            bladeR.Visible = true; // tylko kiedy niskie obroty
            // BLADE

            // AIRSCREW
            airscrewL = sceneMgr.CreateEntity(name + "_AirscrewL", "Airscrew.mesh");
            airscrewL.CastShadows = false;
            bladeNodeL.AttachObject(airscrewL);
            airscrewL.Visible = false;

            airscrewR = sceneMgr.CreateEntity(name + "_AirscrewR", "Airscrew.mesh");
            airscrewR.CastShadows = false;
            bladeNodeR.AttachObject(airscrewR);
            airscrewR.Visible = false;
            // AIRSCREW

        }

      
        protected override void initWheels()
        {
        }

             public override void SetBladeVisibility(bool visible)
        {
            bladeL.Visible = visible;
            bladeR.Visible = visible;
            airscrewL.Visible = visible;
            airscrewR.Visible = visible;
        }

       
        
        public override void ShowTorpedo()
        {

        }

        public override void HideTorpedo()
        {

        }

        public override void OnFireGun()
        {
            base.OnFireGun();
           /* OnFireGunDo(new Vector3(3.6f, 1.0f, -4.3f), new Vector3(-3.6f, 1.0f, -4.3f), new Vector2(4.6f, 3.6f), false, 64);
            OnFireGunDo(new Vector3(1.8f, 0.8f, -3.3f), new Vector3(-1.8f, 0.8f, -3.3f), new Vector2(3.5f, 2.5f), false, 64);
            OnFireGunDo(new Vector3(9.0f, 0.55f, 13.5f), new Vector3(-9.0f, 0.5f, 13.5f), new Vector2(1.5f, 1.5f), true, 35);
*/
        }
        protected override void initOnScene()
        {

            lWingNode = innerNode.CreateChildSceneNode(name + "LWingNode", new Vector3(-8.8f, -0.2f, -1.5f));
            rWingNode = innerNode.CreateChildSceneNode(name + "RWingNode", new Vector3(8.8f, -0.2f, -1.5f));


            // sound
            if (EngineConfig.SoundEnabled)
            {
                engineSound = SoundManager3D.Instance.CreateSoundEntity(SoundManager3D.C_ENEMY_ENGINE_IDLE, this.planeNode, true, false);

                warCrySound = SoundManager3D.Instance.CreateSoundEntity(SoundManager3D.C_ENEMY_WARCRY, this.planeNode, false, false);
                warCrySound2 = SoundManager3D.Instance.CreateSoundEntity(SoundManager3D.C_ENEMY_WARCRY2, this.planeNode, false, false);
                gunSound = SoundManager3D.Instance.CreateSoundEntity(SoundManager3D.C_GUN, this.planeNode, false, false);
            }

            //Entity e = sceneMgr.CreateEntity("");

            planeEntity = sceneMgr.CreateEntity(name + "_Body", GetMainMeshName());
            //  planeEntity.SetMaterialName("P47/Body");
            innerNode.AttachObject(planeEntity);
            outerNode.Scale(new Vector3(0.4f, 0.4f, 0.4f));
            /*
            if (plane != null && this.plane.Direction == Wof.Model.Level.Direction.Right)
            {
                outerNode.LookAt(Vector3.UNIT_X, Node.TransformSpace.TS_WORLD);
            }
            else
            {
                outerNode.LookAt(Vector3.NEGATIVE_UNIT_X, Node.TransformSpace.TS_WORLD);
            }*/



            ViewHelper.AttachAxes(sceneMgr, innerNode, 1.5f);



            refreshPosition();
            base.initOnScene();

            if (plane != null && plane.LocationState == LocationState.Air)
            {
                animationMgr.switchToIdle();
            }
            animationMgr.enableBlade();



            if (EngineConfig.DisplayingMinimap)
            {
                minimapItem =
                    new MinimapItem(outerNode, frameWork.MinimapMgr, "Cube.mesh", ColourValue.Red, planeEntity);
            }


            ResetCameraHolders();
        }
    }
}