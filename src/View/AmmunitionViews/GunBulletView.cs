﻿/*
 * Created by SharpDevelop.
 * User: awitczak
 * Date: 2012-07-08
 * Time: 17:44
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using Mogre;
using Wof.Controller;
using Wof.Model.Level;
using Wof.Model.Level.Weapon;
using Wof.View.Effects;
using Wof.View.NodeAnimation;
using Math = Mogre.Math;

namespace Wof.View.AmmunitionViews
{
	/// <summary>
	/// Description of GunBulletView.
	/// </summary>
	internal class GunBulletView : MissileBaseView<GunBulletView>
	{
		public GunBulletView(IFrameWork framework) : base(framework)
		{		
         
        }

        
       
       
        
   //     protected List<NodeAnimation.NodeAnimation> animations = new List<NodeAnimation.NodeAnimation>();


        protected string getEffectName(Vector3 gunPos)
        {
            return "GunTrail" + ammunitionID + "_" + gunPos;
        }

        protected string getEffectNameTop(Vector3 gunPos)
        {
            return "GunTrailTop" + ammunitionID + "_" + gunPos;
        }

        protected void hideEffect(Vector3 gunPos)
        {
            EffectsManager.Singleton.HideSprite(sceneMgr, ammunitionNode, EffectsManager.EffectType.GUNTRAIL, getEffectName(gunPos));
            EffectsManager.Singleton.HideSprite(sceneMgr, ammunitionNode, EffectsManager.EffectType.GUNTRAIL, getEffectNameTop(gunPos));
         }

        protected override void preInitOnScene()
        {
            // showaj poprzednio skaszowane sprajty.
           
            float baseWidth = 1.5f;
            // FIXME: przesuwac nodey zamiast tworzyc wiecej niepotrzebnie
            prepareGunEffect(gunPosLeft, baseWidth);
            prepareGunEffect(gunPosRight, baseWidth);
            prepareGunEffect(gunPosMiddle, baseWidth);
            
            hideEffect(gunPosLeft);
            hideEffect(gunPosRight);
            hideEffect(gunPosMiddle);
            Hide();
          
        }
        

        protected void prepareGunEffect(Vector3 gunPos, float baseWidth)
		{
		 	Quaternion orient, trailOrient;

            if (ammunitionNode == null)
            {
                ammunitionNode = sceneMgr.RootSceneNode.CreateChildSceneNode("AmmunitionNode" + ammunitionID);
		 	 
            }
            
		 	  
            orient = new Quaternion(-Math.HALF_PI, Vector3.UNIT_Y);
            trailOrient = new Quaternion(-Math.HALF_PI, Vector3.UNIT_Y);
            trailOrient *= new Quaternion(-Math.HALF_PI, Vector3.UNIT_X);
          

            float trailWidth = baseWidth * Math.RangeRandom(1.0f, 1.1f);
           
        
            Vector3 trailBase = new Vector3(gunPos.x, gunPos.y, gunPos.z);

            
           
        	//animations.Add(
            EffectsManager.Singleton.RectangularEffect(sceneMgr, ammunitionNode,
                                                       getEffectName(gunPos),
                                                       EffectsManager.EffectType.GUNTRAIL,
                                                       trailBase - new Vector3(0, 0, Math.RangeRandom(-0.5f, 0.5f)),
                                                       new Vector2(trailWidth, 1.0f),
                                                       trailOrient, true);
        	//);
       

            orient *= new Quaternion(Math.HALF_PI, Vector3.UNIT_X);
            trailOrient *= new Quaternion(Math.HALF_PI, Vector3.UNIT_X);
        

            
        	//animations.Add(
            EffectsManager.Singleton.RectangularEffect(sceneMgr, ammunitionNode,
                                                       getEffectNameTop(gunPos),
                                                       EffectsManager.EffectType.GUNTRAIL,
                                                       trailBase - new Vector3(0, 0, Math.RangeRandom(0.5f, 2.0f)),
                                                       new Vector2(trailWidth, 1.0f),
                                                       trailOrient, true);
        	//);
           
        	
		}

        private readonly Vector3 gunPosLeft = new Vector3(-1.5f, -0.3f, -0.3f);
        private readonly Vector3 gunPosRight = new Vector3(1.5f, -0.3f, -0.3f);
        private readonly Vector3 gunPosMiddle = new Vector3(0.0f, 0.3f, -0.3f);

        
        public override void postInitOnScene()
        {
        	

        	if (ammunition is GunBullet)
            {
               
                if ((ammunition as GunBullet).IsDoubleView)
                {
                    float baseWidth = 1.5f;
                    prepareGunEffect(gunPosLeft, baseWidth);
                    prepareGunEffect(gunPosRight, baseWidth);
                }
                else
                {
                    
                    float baseWidth = 1.5f;
                    prepareGunEffect(gunPosMiddle, baseWidth);

                }


            }


            //Hide();
          //  base.postInitOnScene();         
            refreshPosition();         
            //ammunitionNode.SetVisible(true, true);
        }

        
               
      	public override void Hide()
        {
            if (ammunitionNode != null) ammunitionNode.SetVisible(false, true);
      		//for(NodeAnimation.VisibilityNodeAnimation ani : animations) {
      		//	ani.
      		//}
            
        }

        public override void updateTime(float timeSinceLastFrameUpdate)
        {          
        }
        
      
        public override void refreshPosition()
        {
        	base.refreshPosition();
            if (ammunition != null)
            {
            	if((ammunition is GunBullet)) 
            	{
            		
            		GunBullet gb = ammunition as GunBullet;
            	
            		float yawAmount = getYawAmount();        
        			float z =  gb.TimeCounter * yawAmount * 0.1f;
        		
        			if(gb.Owner.Direction == Direction.Right){
        				z *= -1;
        			}

       			
        			
        			/*
        				
        			Vector3 v = new Vector3(ammunition.MovementVector.X, ammunition.MovementVector.Y, yawAmount);
        			v.Normalise();
        			if(z>0)
        			{
        			Console.WriteLine(v);
        			}
        			v = new Vector3(1, (float)System.Math.Tan(ammunition.AbsoluteAngle), yawAmount / Mogre.Math.HALF_PI);
        			v.Normalise();
        			if(z>0)
        			{
        			Console.WriteLine(v);
        			}*/
        		
                	ammunitionNode.SetPosition(ammunitionNode.Position.x, ammunitionNode.Position.y, 0 );
               	            	
            	//ammunitionNode.Orientation = ;
            			//ammunitionNode.Orientation *= new Quaternion(initialViewOwnerOrientation.Yaw, Vector3.UNIT_Z);

            			//Console.WriteLine(initialViewOwnerOrientation.Pitch.ValueDegrees+"; "+initialViewOwnerOrientation.Yaw.ValueDegrees+"; "+initialViewOwnerOrientation.Roll.ValueDegrees);
            			
            		//gb.TravelledDistance
            		
           			
            			
            	
            	}
            }
        }
        
	}
}
