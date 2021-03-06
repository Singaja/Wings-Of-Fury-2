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
using System.Collections.Generic;
using FSLOgreCS;
using Mogre;
using Wof.Controller;
using Wof.Misc;
using Wof.Model.Level;
using Wof.Model.Level.Common;
using Wof.Model.Level.Infantry;
using Wof.View.Effects;
using Wof.View.VertexAnimation;
using Math=Mogre.Math;

namespace Wof.View
{
    internal class SoldierView : VertexAnimable, IDisposable
    {
        protected static int soldierCounter = 1;

        protected int soldierID;

        protected Soldier soldier;

        public Soldier Soldier
        {
            get { return soldier; }
        }

        protected SceneManager sceneMgr;
        protected IFrameWork framework;

        // SOLDIER
        protected SceneNode soldierNode;
        protected Entity soldierModel;

      
        protected Entity weapon;

        //protected SceneNode parentNode;
        protected AnimationState animationState;
        protected AnimationState runAnimationState;
        protected AnimationState die1AnimationState;
        protected AnimationState die2AnimationState;
        protected AnimationState prepareToFireAnimationState;


      
       
     
        protected static Stack<SoldierView> soldierAvailablePool;
        protected static Dictionary<Soldier, SoldierView> soldierUsedPool;

        #region Minimap representation

        protected MinimapItem minimapItem = null;
        private FSLSoundEntity dieSound = null;
        protected FSLSoundObject prepareToFireSound = null;
   
        public MinimapItem MinimapItem
        {
            get { return minimapItem; }
        }

        #endregion


        public void Dispose()
        {
            if (dieSound != null)
            {
                 SoundManager3D.Instance.RemoveSound(dieSound.Name);
                 //dieSound.Destroy();
                 dieSound = null; 
            }
            if(prepareToFireSound != null)
            {
                SoundManager3D.Instance.RemoveSound(prepareToFireSound.Name);
               // prepareToFireSound.Destroy();
                prepareToFireSound = null; 
            }
        
            GC.SuppressFinalize(this);

        }

        public static void InitPool(int poolSize, IFrameWork framework)
        {
            soldierAvailablePool = new Stack<SoldierView>(poolSize);
            soldierUsedPool = new Dictionary<Soldier, SoldierView>(poolSize);

            for (int i = 0; i < poolSize; i++)
            {
                SoldierView dummyView = new SoldierView(framework);
                soldierAvailablePool.Push(dummyView);
            }
        }

        public static void DestroyPool()
        {
            while(soldierAvailablePool.Count > 0)
            {
                soldierAvailablePool.Pop().Dispose();
            }
            soldierAvailablePool.Clear();
            
            Dictionary<Soldier,SoldierView>.Enumerator e = soldierUsedPool.GetEnumerator();
            while(e.MoveNext())
            {
                e.Current.Value.Dispose();
            }
            soldierUsedPool.Clear();
        }



        public static SoldierView GetInstance(Soldier soldier)
        {
            if(soldierAvailablePool.Count == 0) return null;
            SoldierView sv = soldierAvailablePool.Pop();
            sv.soldier = soldier;
            sv.postInitOnScene();
            soldierUsedPool.Add(soldier, sv);
            return sv;
        }

        public static void FreeInstance(Soldier soldier, bool hide)
        {
            SoldierView sv = soldierUsedPool[soldier];
            sv.soldierNode.SetVisible(!hide);

            sv.StopBlood();

            if (EngineConfig.DisplayingMinimap)
            {
                if (sv.minimapItem != null)
                {
                    sv.minimapItem.Hide();
                }
            }
           
            soldierUsedPool.Remove(soldier);
            soldierAvailablePool.Push(sv);
        }

     
       

        public void PlaySoldierDeathSound()
        {
            if (EngineConfig.SoundEnabled)
            {
                dieSound.Play();
                //SoundManager3D.Instance.UpdateSoundObjects();
            }
        }

     
     

        protected void preInitOnScene()
        {
            soldierModel = sceneMgr.CreateEntity("Soldier" + soldierID.ToString(), "Soldier.mesh");
           
            soldierNode =
                sceneMgr.RootSceneNode.CreateChildSceneNode("SoldierNode" + soldierID.ToString(),
                                                            new Vector3(-1000000, -1000000, 0));

            if (EngineConfig.SoundEnabled)
            {
                float rand = Math.RangeRandom(0.0f, 5.0f);
                if (rand >= 0 && rand < 1.0)
                {
                    dieSound = SoundManager3D.Instance.CreateSoundEntity(SoundManager3D.C_SOLDIER_DIE_1, soldierNode, false, false);
                    dieSound.SetBaseGain(3.5f);
                   
                }
                else if (rand >= 1 && rand < 2.0)
                {
                    dieSound = SoundManager3D.Instance.CreateSoundEntity(SoundManager3D.C_SOLDIER_DIE_2, soldierNode, false, false);
                    dieSound.SetBaseGain(4.0f);
                }else
                if (rand >= 2 && rand < 3.0)
                {
                    dieSound = SoundManager3D.Instance.CreateSoundEntity(SoundManager3D.C_SOLDIER_DIE_3, soldierNode, false, false);
                    dieSound.SetBaseGain(3.0f);
                }
                else if (rand >= 3 && rand < 4.0)
                {
                    dieSound = SoundManager3D.Instance.CreateSoundEntity(SoundManager3D.C_SOLDIER_DIE_4, soldierNode, false, false);
                    dieSound.SetBaseGain(2.0f);
                }
                else
                {
                    dieSound = SoundManager3D.Instance.CreateSoundEntity(SoundManager3D.C_SOLDIER_DIE_5, soldierNode, false, false);
                    dieSound.SetBaseGain(2.0f);
                }
                dieSound.SetReferenceDistance(60); // make it a bit louder but dissapear faster

              //  prepareToFireSound = SoundManager3D.Instance.CreateSoundEntity(SoundManager3D.C_MISSILE_LOCK, soldierNode, true, false);
   
            
            }


            soldierNode.AttachObject(soldierModel);
            soldierNode.Scale(1.3f, 1.35f, 1.3f);
            runAnimationState = soldierModel.GetAnimationState("run");
            die1AnimationState = soldierModel.GetAnimationState("die1");
            die2AnimationState = soldierModel.GetAnimationState("die2");
            prepareToFireAnimationState = soldierModel.GetAnimationState("prepareToFire");

            soldierNode.SetVisible(false);
           
            if (EngineConfig.DisplayingMinimap)
            {
                if (minimapItem == null)
                {
                    minimapItem =
                    new MinimapItem(soldierNode, framework.MinimapMgr, "Cube.mesh", new ColourValue(0.97f, 1.0f, 0.66f),
                                    soldierModel);
                    minimapItem.ScaleOverride = new Vector2(4, 7); // stala wysokosc bunkra, niezale�na od bounding box 
                }
                minimapItem.Refresh();
                minimapItem.Hide();
            }


            
      
    
            
        }
        
      
        
        public void showArrow(EffectsManager.EffectType type)
        {
            float arrowWidth = 2.5f;
            float arrowHeight = 2.5f;
            switch (type)
            {
                case EffectsManager.EffectType.MISSILE_LOCK_ARROW:
                    arrowWidth = 2.5f;
                    arrowHeight = 5.0f;
                    break;
            }
           
            Quaternion q = new Quaternion();
            q.FromAngleAxis(new Radian(new Degree(90)), Vector3.UNIT_X );
            q *= new Quaternion(new Radian(new Degree(90)), Vector3.UNIT_Z);
            EffectsManager.Singleton.RectangularEffect(sceneMgr, soldierNode, soldierNode.Name + "Arrow", type, new Vector3(0, soldierModel.BoundingBox.Size.y + arrowHeight, 0), new Vector2(arrowWidth, arrowHeight), q, true);

        }

        public void hideArrow(EffectsManager.EffectType type)
        {
            EffectsManager.Singleton.NoSprite(sceneMgr, soldierNode, type, soldierNode.Name + "Arrow");
        }

        protected virtual void postInitOnScene()
        { 
        	// dopiero teraz wiemy jaki to �o�nierz
        	if (Soldier is General)
            {
                soldierModel.SetMaterialName("General");
            }
            else if (Soldier is Seaman)
            {
                //case Soldier.SoldierType.SEAMAN:                
                soldierModel.SetMaterialName("Seaman");
            } else
            {
            	// na razie zwykli zolnierze tez sa seaman
             	soldierModel.SetMaterialName("Seaman");
            }

            if(soldier.HasBazooka)
            {
                Quaternion q = new Quaternion(new Radian(new Degree(90)), Vector3.NEGATIVE_UNIT_X);
                if (!sceneMgr.HasEntity(this.soldierID + "Bazooka"))
                {
                    weapon = sceneMgr.CreateEntity(this.soldierID + "Bazooka", "Bazooka.mesh");
                    soldierModel.AttachObjectToBone("Bip01 R Hand", weapon, q, new Vector3(0.07f, 0, 0));
                }
               
            }

            Run();
            refreshPosition();
            soldierNode.SetVisible(true);
            if (EngineConfig.DisplayingMinimap && minimapItem != null)
            {
                minimapItem.Show();
            }
        }


        public SoldierView(IFrameWork framework)
        {
            this.framework = framework;
            sceneMgr = framework.SceneMgr;
            soldierID = soldierCounter++;

            preInitOnScene();
        }


        public void refreshPosition()
        {
          
            Vector2 v = UnitConverter.LogicToWorldUnits(soldier.Position);
            soldierNode.SetPosition(v.x, v.y - 0.5f, 0);
           
            if (soldier.Direction == Direction.Right)
            {
                soldierNode.Orientation = new Quaternion(Math.HALF_PI, Vector3.NEGATIVE_UNIT_Y);
            }
            else
            {
                soldierNode.Orientation = new Quaternion(Math.HALF_PI, Vector3.UNIT_Y);
            }

            if (EngineConfig.DisplayingMinimap && minimapItem != null && minimapItem.IsVisible)
            {
                minimapItem.Refresh();
            }
        }

      

        private void Bleed()
        {
            float bloodSize = 2.5f;
            Quaternion r = new Quaternion();

            for (int i = 0; i < 3; i++)
            {
                r.FromAngleAxis(new Radian(new Degree(90)), Vector3.UNIT_X);
                r *= new Quaternion(new Radian(new Degree(90)), Vector3.UNIT_Z);
                bloodSize += Mogre.Math.RangeRandom(-bloodSize * 0.2f, bloodSize * 0.2f);
                r *= new Quaternion(new Radian(new Degree(Mogre.Math.RangeRandom(-30, 30))), Vector3.UNIT_X);
                r *= new Quaternion(new Radian(new Degree(90 + Mogre.Math.RangeRandom(-30, 30))), Vector3.UNIT_Y);
                EffectsManager.Singleton.RectangularEffect(sceneMgr, soldierNode, soldierNode.Name + "Blood" + i, EffectsManager.EffectType.BLOOD, new Vector3(0, soldierModel.BoundingBox.Size.y - 0.20f * bloodSize, 0), new Vector2(0.4f * bloodSize, bloodSize), r, false);
            }
        }

        private void StopBlood()
        {
          
            for (int i = 0; i < 3; i++)
            {
               // EffectsManager.Singleton.NoSprite(sceneMgr, soldierNode, EffectsManager.EffectType.BLOOD, soldierNode.Name + "Blood" + i);
            }
        }


        public void Run()
        {
           /* if(prepareToFireSound.IsPlaying())
            {
                prepareToFireSound.Stop();
            }*/

            hideArrow(EffectsManager.EffectType.MISSILE_LOCK_ARROW);

         //   soldierNode.SetScale(1, 1, 1);
            die1AnimationState.Enabled = false;
            die2AnimationState.Enabled = false;
            prepareToFireAnimationState.Enabled = false;

            runAnimationState.Enabled = true;
            runAnimationState.Loop = true;

            animationState = runAnimationState;
            animationState.TimePosition = 0;
        }

        public void PrepareToFire()
        {
            showArrow(EffectsManager.EffectType.MISSILE_LOCK_ARROW);
          //  prepareToFireSound.Play();

         //   soldierNode.Scale(1,10,1);
            runAnimationState.Enabled = false;
            die1AnimationState.Enabled = false;
            die2AnimationState.Enabled = false;

            prepareToFireAnimationState.Enabled = true;
            prepareToFireAnimationState.Loop = false;
            animationState = prepareToFireAnimationState;
            animationState.TimePosition = 0;
        }



        public void DieFromGun(bool blood)
        {
            runAnimationState.Enabled = false;
            prepareToFireAnimationState.Enabled = false;

            die1AnimationState.Enabled = true;
            die1AnimationState.Loop = false;

            animationState = die1AnimationState;
            animationState.TimePosition = 0;
           
            
            if (EngineConfig.DisplayingMinimap)
            {
                minimapItem.Hide();
            }

            hideArrow(EffectsManager.EffectType.HINT_ARROW);
            hideArrow(EffectsManager.EffectType.MISSILE_LOCK_ARROW);

            //  BLOOD
            if (blood && EngineConfig.Gore)
            {
                Bleed();
            }
        }
        
        public void DieFromExplosion(bool blood)
        {
        	
            runAnimationState.Enabled = false;
            prepareToFireAnimationState.Enabled = false;

            die2AnimationState.Enabled = true;
            die2AnimationState.Loop = false;


            animationState = die2AnimationState;
            animationState.TimePosition = 0;

            hideArrow(EffectsManager.EffectType.HINT_ARROW);
            hideArrow(EffectsManager.EffectType.MISSILE_LOCK_ARROW);

            //  BLOOD
            if (blood && EngineConfig.Gore)
            {
                Bleed();
            }

            if (EngineConfig.DisplayingMinimap)
            {
                minimapItem.Hide();
            }
        }


        public Boolean IsAnimationFinished()
        {
            return animationState.HasEnded;
        }

        public void updateTime(float timeSinceLastFrame)
        {
        	
        	if (soldier.IsAlive && EngineConfig.DisplayingMinimap)
            {
                if (((DateTime.Now.Millisecond/100.0f)%100) > 7)
                {
                    if (minimapItem.IsVisible) minimapItem.Hide();
                }
                else
                {
                    if (!minimapItem.IsVisible) minimapItem.Show();
                }
        	}
        	
            if (animationState.AnimationName.Equals("run"))
            {
                timeSinceLastFrame *= (soldier.Speed/5.0f); // dopasuj szybkosc animacji do szybkosci zolnierza
            }
            else if (animationState.AnimationName.StartsWith("die"))
            {
                timeSinceLastFrame *= (soldier.Speed/5.0f);
                ; // na razie w osobnym ifie
            }
            else if (animationState.AnimationName.StartsWith("prepareToFire"))
            {
                timeSinceLastFrame *= (soldier.Speed/2.0f);
               
            }
            animationState.AddTime(timeSinceLastFrame);
            
            // TODO: animacja strza�ki
            
            
        }

        public void rewind()
        {
            animationState.TimePosition = 0;
        }

        public void enableAnimation()
        {
            animationState.Enabled = true;
        }

        public void disableAnimation()
        {
            animationState.Enabled = false;
        }
    }
}