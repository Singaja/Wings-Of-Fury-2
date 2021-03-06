/*
 * Copyright 2008 Adam Witczak, Jakub Tê¿ycki, Kamil S³awiñski, Tomasz Bilski, Emil Hornung, Micha³ Ziober
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
using System.Text;
using Wof.Model.Configuration;
using Wof.Model.Level.Common;
using Wof.Model.Level.LevelTiles;
using Wof.Model.Level.LevelTiles.IslandTiles;
using Wof.Model.Level.LevelTiles.IslandTiles.EnemyInstallationTiles;
using Wof.Model.Level.LevelTiles.Watercraft;
using Wof.Model.Level.Weapon;

namespace Wof.Model.Level.Infantry
{
    /// <summary>
    /// Klasa implementuje logike zachowania
    /// zolnierza na planszy.
    /// </summary>
    /// <author>Michal Ziober</author>
    public class Soldier : IMove, IObject2D
    {
        #region Const
        
        /// <summary>
        /// Zasieg ogniowy
        /// </summary>
        /// <author>Adam Witczak</author>
        public const int FireDistance = 90;

       

        /// <summary>
        /// Wspolczynnik prawdopodobienstwa tego, ze zolnierz 
        /// wejdzie do bunkra.
        /// </summary>
        protected const int ProbabilityCoefficient = 5;

      
        /// <summary>
        /// Liczba pocisków która dysponuje zolnierz
        /// </summary>
        protected const int RocketCount = 1;

        /// <summary>
        /// Wielokrotnosc nominalnej prędkości rakiety samolotowej
        /// </summary>
        protected const float RocketSpeedMultiplier = 0.3f;

        /// <summary>
        /// Przedzial czasu od ostatniego przesuniecia.
        /// </summary>
        protected const int TimeUnit = 1000;
        
        /// <summary>
        /// Wielokrotnosc nominalnej prędkości rakiety samolotowej
        /// </summary>
        protected const float RocketDistanceMultiplier = 0.4f;
      



        #endregion

        #region Enums

        /// <summary>
        /// Stan zolnierza.
        /// </summary>
        public enum SoldierStatus : byte
        {
            /// <summary>
            /// Zolnierz jest zywy
            /// </summary>
            IsAlive = 0,

            /// <summary>
            /// Zolnierz jest martwy
            /// </summary>
            IsDead = 1,

            /// <summary>
            /// Schowal sie do bunkra
            /// </summary>
            InBunker = 2
        }

        #endregion

        #region Fields
        
        /// <summary>
        /// Obiekt zarzadzajacy bronia 
        /// </summary>
        /// <author></author>
        protected WeaponManager weaponManager;

        /// <summary>
        /// Pozycja X na planszy.
        /// </summary>
        protected float xPos;

        /// <summary>
        /// Pozycja Y na planszy.
        /// </summary>
        protected float yPos;

        /// <summary>
        /// Pozycja startowa zolnierza.
        /// </summary>
        protected readonly float startPosition;

        /// <summary>
        /// Pocz¹tkowy indeks z leveltiles
        /// </summary>
        protected int startLevelIndex;

        /// <summary>
        /// Kierunek poruszania.
        /// </summary>
        protected Direction direction;
       
        /// <summary>
        /// Czy zolnierz jest zywy.
        /// </summary>
        protected SoldierStatus _soldierStatus;

        /// <summary>
        /// Szybkosc z jaka porusza sie zolnierz.
        /// </summary>
        protected int speed;

        /// <summary>
        /// Referencja do planszy.
        /// </summary>
        protected readonly Level refToLevel;
        
        
        
        /// <summary>
        /// Licznik czasu, pod czas ktorego zolnierz nie moze
        /// zostac zabity.
        /// </summary>
        protected int protectedTime;

        /// <summary>
        /// Licznik czasu, pod czas ktorego zolnierz nie 
        /// moze wejsc do bunkra.
        /// </summary>
        protected int homelessCounterTime = 0;

        /// <summary>
        /// Czy zolnierz moze wejsc znowu do bunkru.
        /// </summary>
        protected bool canReEnter;

        /// <summary>
        /// Czy zolnierz moze zostac zabity.
        /// </summary>
        protected bool canDie;
        
        
      

        protected bool leftBornTile = false;

        protected SoldierType type;

        public enum SoldierType
        {
            SOLDIER,
            GENERAL,
            SEAMAN
        }
        
        /// <summary>
        /// Wysokoœæ
        /// </summary>
        private const float height = 2.5f;

        /// <summary>
        /// Szerokoœæ 
        /// </summary>
        private const float width = 0.75f;

        #endregion

        private bool hasBazooka;

        private int lastFireTick;

        private int preparingToFireTime = 0;


        /// <summary>
        /// Jak długo żołnierz ładuje broń (czeka przed strzalem) [msek]
        /// </summary>
        protected int firePrepareDelay = 4000;

        /// <summary>
        /// Odstępy pomiędzy możliwością odpalenia kolejnych pocisków [w msek].
        /// </summary>
        protected int fireInterval = 3000;

        /// <summary>
        /// Kat o ktory obraca sie pocisk wystrzelony z bazooki w ciagu sekundy w osi Z
        /// </summary>
        protected float bazookaRotationPerSecond = (float)System.Math.PI * 0.65f;



                        
		
        #region Public Constructor

        /// <summary>
        /// Publiczny konstruktor jednoparametrowy.
        /// </summary>
        /// <param name="posX">Pozycja startowa zolnierza (mierzona w tilesIndex.</param>
        /// <param name="direct">Kierunek w ktorym sie porusza.(Prawo,Lewo)</param>
        /// <param name="level">Referencja do obiektu planszy.</param>
        /// <param name="holdsBazooka">Czy moze strzelac</param> 
        /// <author>Michal Ziober</author>
        /// <param name="offset"></param>
        public Soldier(float posX, Direction direct, Level level, float offset, bool holdsBazooka)//, SoldierType type)
        {
            //przy starcie jest zywy.
            _soldierStatus = SoldierStatus.IsAlive;
            //pozycja startowa - pozycja zniszczonej instalacji
            xPos = posX * LevelTile.TileWidth + offset;
            startPosition = posX;
            startLevelIndex = (int)posX;
            direction = direct;
            refToLevel = level;
            canDie = false;
            canReEnter = false;
            protectedTime = 0;
            lastFireTick = Environment.TickCount;
            weaponManager = new WeaponManager(level, this, RocketCount, 0, 0);
            
            weaponManager.RegisterWeaponToModelEvent += level.rocket_RegisterWeaponEvent;            
            this.hasBazooka = holdsBazooka;
            if(holdsBazooka)
            {
            	weaponManager.SelectWeapon = WeaponType.Rocket;            	
            }
            else
            {
            	weaponManager.SelectWeapon = WeaponType.None;            	
            }
            Random r= new Random();

            // doza losowosci
            firePrepareDelay = (int)(firePrepareDelay * Mathematics.RangeRandom(0.9f, 1.1f));
            fireInterval = (int)(fireInterval * Mathematics.RangeRandom(0.9f, 1.1f));

            bazookaRotationPerSecond = bazookaRotationPerSecond * Mathematics.RangeRandom(0.9f, 1.1f);
            bazookaRotationPerSecond *= Mathematics.RangeRandom(-1, 1) > 0 ? 1.0f : -1.0f; // zamiana kąta
       
        }

        #endregion

        #region Properties
        
        protected bool ShouldFire
        {
          	get {
                PointD diff = refToLevel.UserPlane.Position - Position;
                return diff.EuclidesLength < FireDistance && (diff.X * (int)direction) > 0;  // zolnierz musi zbyc zwrocony w strone samolotu
            
            }
        	
        }

        protected bool CanFire
        {
            get
            {
                return IsAlive && hasBazooka && Environment.TickCount - lastFireTick > fireInterval && Weapon.RocketCount > 0;
            
            }
        }

        public bool HasBazooka 
        {
			get { return hasBazooka; }
		}
        
        public WeaponManager Weapon
        {
            get { return weaponManager; }
        }

        /// <summary>
        /// Ustawia lub pobiera predkosc zolnierza.
        /// </summary>
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }


        /// <summary>
        /// Jesli zwroci true - zolnierz zyje,
        /// w przeciwnym przypadku - zolnierz nie zyje.
        /// </summary>
        public bool IsAlive
        {
            get { return _soldierStatus == SoldierStatus.IsAlive; }
        }

        public void ReturnToBunker()
        {
            this._soldierStatus = SoldierStatus.InBunker;
        }
        /// <summary>
        /// Zwraca stan w ktorym obecnie znajduje sie zolnierz.
        /// <see cref="SoldierStatus"/>
        /// </summary>
        public SoldierStatus Status
        {
            get { return _soldierStatus; }
        }

        public int StartLevelIndex
        {
            get { return startLevelIndex; }
        }

        /// <summary>
        /// Rodzaj ¿o³nierza
        /// </summary>
        public SoldierType Type
        {
            get { return type; }
        }

        /// <summary>
        /// Zwraca pozycje X zolnierza na planszy.
        /// </summary>
        public float XPosition
        {
            get { return xPos; }
        }

        /// <summary>
        /// Zwraca pozycje Y zolnierza na planszy.
        /// </summary>
        public float YPosition
        {
            set { yPos = value; }
            get { return yPos; }
        }


        /// <summary>
        /// Zwraca pozycje zolnierza na planszy.
        /// </summary>
        public PointD Position
        {
            get { return new PointD(xPos, yPos); }
        }

        /// <summary>
        /// Zwraca kierunek w ktorym porusza sie zolnierz.
        /// </summary>
        public Direction Direction
        {
            get { return direction; }
        }

        /// <summary>
        /// Zwraca wartosc czy zolnierz moze zaostac zabity.
        /// </summary>
        public bool CanDie
        {
            get { return canDie; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float StartPosition
        {
            get { return startPosition; }
        }


        #endregion

        #region PrivateMethod

        /// <summary>
        /// Sprawdza sasiednie pole.
        /// </summary>
        /// <param name="index">Indeks pola na liscie.</param>
        /// <returns>Jesli zolnierz moze wejsc na pole zwraca true,
        /// false w przeciwnym przypadku.</returns>
        private bool Check(int index)
        {
            if (!leftBornTile) return true;
            IslandTile tiles = refToLevel.LevelTiles[index] as IslandTile;
            if (tiles == null)
            {
                ShipTile tiles2 = refToLevel.LevelTiles[index] as ShipTile;
                if (tiles2 == null) return false;
                else
                {
                    return tiles2.Traversable;
                }
            }
            else
            {
                return tiles.Traversable;
            }


        }

        /// <summary>
        /// Sprawdza czy sasiednie pole jest bunkrem.
        /// </summary>
        /// <param name="index">Indeks pola na liscie.</param>
        /// <returns>Jesli dane pole jest bunkrem - zwraca true,
        /// w przeciwnym przypadku false.</returns>
        protected bool IsBunker(int index)
        {
            if (index >= 0 && index < refToLevel.LevelTiles.Count)
                return (refToLevel.LevelTiles[index] is BunkerTile);
            else
                return false;
        }

        /// <summary>
        /// Zmienia pozycje zolnierza.
        /// </summary>
        protected void ChangeLocation(int time)
        {

            //jesli idzie w prawo
            if (direction == Direction.Right)
            {
                float tmpPosition = xPos + speed * Mathematics.GetMoveFactor(time, TimeUnit);
                //sprawdza czy moze wejsc na to pole.
                if (Check(Mathematics.PositionToIndex(tmpPosition)))
                    xPos = tmpPosition;
                else //zmienia kierunek
                    direction = Direction.Left;
                // Check(Mathematics.PositionToIndex(tmpPosition));

            } //jesli idzie w lewo
            else
            {
                float tmpPosition = xPos - speed * Mathematics.GetMoveFactor(time, TimeUnit);
                //sprawdza czy moze wejsc na to pole.
                if (Check(Mathematics.PositionToIndex(tmpPosition)))
                    xPos = tmpPosition; //wchodzi na sasiednie pole.
                else //zmienia kierunek.
                    direction = Direction.Right;
                //  Check(Mathematics.PositionToIndex(tmpPosition));
            }
            int index = Mathematics.PositionToIndex(xPos);
            if(index >=  refToLevel.LevelTiles.Count)
            {
                
            }
            else
            {
                LevelTile tile = refToLevel.LevelTiles[index];
                yPos = (tile.YBegin + tile.YEnd) / 2.0f;
            }
           

        }

        #endregion

        #region Public Method

        /// <summary>
        /// Zwraca opis zolnierza.
        /// </summary>
        /// <returns>String z opisem zolnierza.</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Xpos: " + xPos);
            builder.AppendLine("Direction: " + direction.ToString());

            return builder.ToString();
        }

        /// <summary>
        /// Zabija zolnierza.
        /// </summary>
        public void Kill()
        {
            //zabijam zolnierza.
            _soldierStatus = SoldierStatus.IsDead;
        }

        #endregion

        #region IMove Members

        /// <summary>
        /// Zmienia pozycje zolnierza.
        /// </summary>
        /// <param name="time">Czas od ostatniego ruchu w milisekundach.</param>
        public virtual void Move(int time)
        {
            //Jesli zolnierz zyje.
            if (IsAlive)
            {

               
                int tileIndex = Mathematics.PositionToIndex(xPos);
                bool isBunker = IsBunker(tileIndex);
               
                if (tileIndex != startPosition) leftBornTile = true;
                if (canReEnter //czy moze wejsc ponownie do bunkra.
                    && (tileIndex != startPosition) //jesli bunkier nie jest rodzicem.
                    && isBunker //jesli to jest bunkier.
                    && (time % ProbabilityCoefficient == 0)) //losowosc.
                {
                    BunkerTile bunker = refToLevel.LevelTiles[tileIndex] as BunkerTile;
                    if (bunker.IsDestroyed && bunker.CanReconstruct)
                    {
                        bunker.Reconstruct();
                        refToLevel.Controller.OnTileRestored(bunker);
                    }
                    if (!bunker.IsDestroyed && bunker.CanAddSoldier)
                    {
                        //dodaje zolnierza do bunkra.
                        bunker.AddSoldier();
                        //wyslam sygnal do controllera aby usunal zolnierza z widoku.
                        refToLevel.Controller.UnregisterSoldier(this);
                        //usuwam zolnierza z planszy.
                        ReturnToBunker();
                    }
                    else ChangeLocation(time);
                }
                else
                {

                    // czy zolnierz moze strzelac?
                    if (!isBunker && canDie && ShouldFire && CanFire)
                	{
                        if (preparingToFireTime == 0)
                        {
                            // zolnierz zaczyna sie przygotowywać 
                            refToLevel.Controller.OnSoldierPrepareToFire(this, firePrepareDelay);
                        }
                	    preparingToFireTime += time;

                        if(preparingToFireTime >= firePrepareDelay)
                        {
                            Random r = new Random();
                            // strzal
                            preparingToFireTime = 0;
                            PointD dir = refToLevel.UserPlane.Position - Position;
                		    dir.Normalise();
                            PointD moveVector = GameConsts.Rocket.BaseSpeed * RocketSpeedMultiplier * dir;
                            dir.X *= (moveVector.X >= 0) ? 1.0f : -1.0f;
                            float relative = dir.Angle;
                            lastFireTick = Environment.TickCount;
                            Rocket rocket = Weapon.RocketFire(relative, moveVector, bazookaRotationPerSecond);
                            rocket.MaxDistanceToOwner *= RocketDistanceMultiplier * Mathematics.RangeRandom(0.8f, 1.2f);
                            rocket.MaxHeightDistanceToOwner *= RocketDistanceMultiplier * Mathematics.RangeRandom(0.8f, 1.2f);
                            // zmniejsz zasieg rakiet

                            refToLevel.Controller.OnSoldierEndPrepareToFire(this);
                        }

                		
                	}
                    else if (!isBunker && canDie && CanFire)
                	{
                        // !ShouldFire -> samolot jest poza zasiegiem
                        if(preparingToFireTime > 0)
                        {
                            // wracamy do punktu wyjscia bez strzalu
                            preparingToFireTime = 0;
                            refToLevel.Controller.OnSoldierEndPrepareToFire(this);
                        }
                        else
                        {
                            ChangeLocation(time);
                        }
                	}
                    else
                	{
                		ChangeLocation(time);
                	}
                	
                }


                //sprawdza czy ulynal czas bezsmiertelnosci
                if (!canDie)
                {
                    protectedTime += time;
                    if (protectedTime >= TimeUnit)
                        canDie = true;
                }

                //sprawdza czy ponownie moze wejsc do bunkra.
                if (!canReEnter)
                {
                    homelessCounterTime += time;
                    if (homelessCounterTime >= GameConsts.Soldier.HomelessTime)
                        canReEnter = true;
                }
            }
        }



        #endregion
    	
		public PointD Center {
			get {
			    return Bounds.Center;
				//return new PointD( Position.X + width / 2.0f, Position.Y + height / 2.0f);
			}
		}
    	
		public Quadrangle Bounds {
			get {
				return new Quadrangle(new PointD(xPos, yPos), width, height);;
			}
		}
    	
		public float RelativeAngle {
			get {
        		return Bounds.Angle * (float)direction;
        		
			}
		}
    	
		public PointD MovementVector {
			get {
        		return new PointD((int)Direction * speed,0);
			}
		}
    	
		public bool IsEnemy {
			get {
				return true;
			}
		}
    	
		public float AbsoluteAngle {
			get {
				 float realAngle = Bounds.IsObverse
                            ? -RelativeAngle
                            : RelativeAngle;
				 return realAngle;
			}
		}
    	
		public GameConsts.GenericPlane GetConsts()
		{
			return null;
		}
    }
}