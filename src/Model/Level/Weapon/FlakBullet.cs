﻿/*
 * Created by SharpDevelop.
 * User: awitczak
 * Date: 2012-06-25
 * Time: 12:46
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Wof.Model.Configuration;
using Wof.Model.Level.Common;
using Wof.Model.Level.Infantry;
using Wof.Model.Level.LevelTiles;
using Wof.Model.Level.LevelTiles.IslandTiles.EnemyInstallationTiles;
using Wof.Model.Level.LevelTiles.IslandTiles.ExplosiveObjects;
using Wof.Model.Level.LevelTiles.Watercraft;
using Wof.Model.Level.Planes;

namespace Wof.Model.Level.Weapon
{
	/// <summary>
	/// Description of FlakBullet.
	/// </summary>
	public class FlakBullet : MissileBase
	{
		
		protected IObject2D target;
		protected static Random mRand  = new Random();
		protected readonly float maxFlyingDistance;
		
	
		
		public FlakBullet(float x, float y, Level level, IObject2D owner, IObject2D target, float fireAngle, float initialSpeed)
			: base(x,y, GetInitialVector(owner, target, initialSpeed) , level, fireAngle, owner)
        {
			 this.target = target;
             boundRectangle = new Quadrangle(new PointD(x, y), 1, 1);  
			 PointD diffVector = (target.Center - owner.Center);             
             maxFlyingDistance = diffVector.EuclidesLength * mRand.Next(90, 110) / 100.0f;
             diffVector.Normalise();      
             diffVector.X *= -1;                        	
             SetZRotationPerSecond(diffVector.X * 0.9f); // zaginanie toru lotu do ziemi
        }
		
		
		protected override bool OutOfFuel() {
			if(!base.OutOfFuel()) {
				
				if(travelledDistance >= maxFlyingDistance) {
					Destroy();
					return true;
				}
			}
			return false;
		}
		protected static PointD GetInitialVector(IObject2D owner, IObject2D target, float initialSpeed) {
			
			
        	float speedCoeff = 2 * target.MovementVector.EuclidesLength /  GameConsts.P47Plane.Singleton.MaxSpeed;
        	float distanceCoeff = FlakBunkerTile.GetAccuracyCoefficient((target.Bounds.Center - owner.Center).EuclidesLength);
       
        	float xSpread = distanceCoeff * speedCoeff * target.Bounds.Width * GameConsts.FlakBunker.FireSpreadX;
            float ySpread = distanceCoeff * speedCoeff * target.Bounds.Height * GameConsts.FlakBunker.FireSpreadY;
            
            float xPos  = mRand.Next((int)(target.Bounds.Center.X - xSpread * 0.5f), (int)(target.Bounds.Center.X + xSpread * 0.5f));
            float yPos  = mRand.Next((int)(target.Bounds.Center.Y*1.2f - ySpread * 0.5f), (int)(target.Bounds.Center.Y*1.2f + ySpread * 0.5f));
            PointD flakPosition = new PointD(xPos, yPos);
            
          	PointD direction = (flakPosition - owner.Center);
            
            direction.Normalise();
            
            return initialSpeed *direction;
		}
		
		
		public float GetDamage(IObject2D obj) {
			
			float dist = (obj.Bounds.Center - Position).EuclidesLength;                
      
            if(dist < GameConsts.FlakBunker.DamageRange)
            {
            	float illuminationMultiplier = 1.0f;
            	if(ammunitionOwner is BunkerTile) 
            	{
            		if((ammunitionOwner as BunkerTile).IsIlluminatedShot) 
            		{
            			illuminationMultiplier = 2.0f;
            		}
            	}
            	float damageCoeff = ((GameConsts.FlakBunker.DamageRange - dist) / GameConsts.FlakBunker.DamageRange);
            	float damage = illuminationMultiplier * GameConsts.FlakBunker.MaxDamagePerHit * damageCoeff * GameConsts.GenericPlane.CurrentUserPlane.HitCoefficient;
            	return damage;
            }
            
            return 0;
            
		}
		
		protected override void ChangePosition(int time)
        {
            float coefficient = Mathematics.GetMoveFactor(time, MoveInterval);

            timeCounter += time;
           
           
               // Console.WriteLine(flyVector.X);

            float minFlyingSpeed = Owner.IsEnemy ? GameConsts.EnemyFighter.Singleton.RangeFastWheelingMaxSpeed * GameConsts.EnemyFighter.Singleton.MaxSpeed : GameConsts.GenericPlane.CurrentUserPlane.RangeFastWheelingMaxSpeed * GameConsts.GenericPlane.CurrentUserPlane.MaxSpeed;


            // rakieta wytraca prędkość uzyskaną od samolotu
            if (Math.Abs(flyVector.X) > Math.Abs(minFlyingSpeed * GameConsts.Rocket.BaseSpeed))
            {
                flyVector.X *= 0.995f;
            }

            if (Math.Abs(flyVector.Y) > Math.Abs(minFlyingSpeed * GameConsts.Rocket.BaseSpeed))
            {
                flyVector.Y *= 0.995f;
            }

            float angle = zRotationPerSecond * coefficient;
            //  boundRectangle.Rotate(angle);
            //  moveVector.Rotate(PointD.ZERO, angle);
            relativeAngle += angle * (int)Direction;
            flyVector.Rotate(PointD.ZERO, angle);

            PointD vector = new PointD(flyVector.X * coefficient, flyVector.Y * coefficient);
            boundRectangle.Move(vector);
            moveVector = vector; // orientacyjnie bo inne metody z tego korzystaja
            
            travelledDistance += vector.EuclidesLength;
          
        }
		
		protected override void CheckCollisionWithUserPlane()
		{
			Plane p = refToLevel.UserPlane;
            if (p != null)
            {
            	
            	bool hit = false;
                float damage = GetDamage(p);              
                
                if(damage>0)
                {
                  
	             	refToLevel.UserPlane.Hit(damage, 0);     
	               	hit=true;	   
	                //powiadamia controler o trafieniu.	               

                    Destroy();
                }
                              
            }
		}

	    protected override void CheckCollisionWithEnemyPlanes()
	    {
	      
	    }
		protected override void CheckCollisionWithGround()
		{
			
			
			if(this.Position.Y < 0) {
				Destroy();

			}
		/*
            LevelTile tile;
			int index = Mathematics.PositionToIndex(Position.X);
            if (index >= 0 && index < refToLevel.LevelTiles.Count)
            {
            	tile = refToLevel.LevelTiles[index];
             	CollisionType c = tile.InCollision(this.boundRectangle);
            	if (c == CollisionType.None) return;
            	
            	//jesli nie da sie zniszczyc dany obiekt z dzialka.
                if(c == CollisionType.Hitbound || c == CollisionType.CollisionRectagle)
                {
                    if (refToLevel.LevelTiles[index] is BarrelTile)
                    {
	                    BarrelTile barrel = refToLevel.LevelTiles[index] as BarrelTile;
	                    if (!barrel.IsDestroyed)
	                    {
	                        barrel.Destroy();
	                        refToLevel.Controller.OnTileDestroyed(barrel, null);
	                        this.refToLevel.Statistics.HitByGun += refToLevel.KillVulnerableSoldiers(index, 2, false);
	                    }
	                }
                    else 
                    {
	                    this.refToLevel.Statistics.HitByGun += refToLevel.KillVulnerableSoldiers(index, 0, false);
                    }
	                  	
                } 
                else if(c == CollisionType.Altitude) 
                {
                	refToLevel.Controller.OnTileBombed(tile, this);
                }
                
            	refToLevel.Controller.OnGunHit(refToLevel.LevelTiles[index], Position.X, Math.Max(this.Position.Y, 1));

               
               
            }*/
		}
	}
}
