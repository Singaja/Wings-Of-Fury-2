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
using Wof.Model.Level.Common;
using Wof.Model.Level.LevelTiles.IslandTiles.EnemyInstallationTiles;
using Wof.Model.Level.LevelTiles.Watercraft.ShipManagers;

namespace Wof.Model.Level.LevelTiles.Watercraft
{
    /// <summary>
    /// Klasa abstarkcyjna implementujaca czesci lotniskowca.
    /// </summary>
    public abstract class ShipTile : LevelTile, IDestroyable, IRefsToLevel, ISinkComponent
    {  
        #region Private Fields

       

        /// <summary>
        /// Zmienna informujaca czy na dane pole moze wejsc zolnierz.
        /// Jesli zmienna jest ustawiona na true, to zolnierz moze wejsc;
        /// w przeciwnym przypadku zolnierz nie moze wejsc na dane pole.
        /// </summary>
        private bool traversable;

        /// <summary>
        /// Referencja do obiektu level.
        /// </summary>
        protected Level refToLevel;

        /// <summary>
        /// Obiekt zarzadzajacy tym elementem.
        /// </summary>
        protected ShipManager _shipManager;
        protected SinkComponent sinkComponent;
    

        #endregion;

       

        #region Public Constructor

        /// <summary>
        /// Konstruktor piecioparametrowy.
        /// </summary>
        /// <param name="yBegin">Wysokosc poczatku obiektu.</param>
        /// <param name="yEnd">Wysokosc konca obiektu.</param>
        /// <param name="hitBound">Prostokat opisujacy obiekt.</param>
        /// <param name="type">Typ obiektu.</param>
        /// <param name="collisionRectangles">Lista prostokatow z ktorymi moga wystapic zderzenia.</param>
        public ShipTile(float yBegin, float yEnd, float viewXShift, Quadrangle hitBound, int type, List<Quadrangle> collisionRectangles, bool traversable)
            : base(yBegin, yEnd, viewXShift, hitBound, collisionRectangles)
        {
            this.type = type;
            this.traversable = traversable;
            sinkComponent = new SinkComponent(this,this);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Pobiera lub ustawia obiekt zarzadzajacy tym elementem
        /// w skali calego statku.
        /// </summary>
        public ShipManager ShipOwner
        {
            get { return _shipManager; }
            set { _shipManager = value; }
        }

        /// <summary>
        /// Zwraca informacje o tym czy zolnierz moze wejsc na to pole.
        /// </summary>
        public bool Traversable
        {
            get { return this.traversable; }
        }

      


        /// <summary>
        /// Ustawia prywatna referencje do planszy.
        /// </summary>
        public Level LevelProperties
        {
            set { refToLevel = value; }
            get { return refToLevel; }
        }

        #endregion


     



      

        

        public bool IsDestroyed
        {
            get { return (_shipManager.State == ShipState.Destroyed); }
        }

      
        public virtual void Destroy()
        {
            StartSinking();
        }


      


       


        public float Submerge(float time, float timeUnit)
        {
            float amount = sinkComponent.DoSubmerge(time, timeUnit);

          
            attractorForce.X = 0;
            attractorForce.Y = -amount;

            if (amount > 0)
            {
                // toni�cie si� nie zako�czy�o
              //  refToLevel.Controller.OnShipSubmerging(this);
                return amount;
            }
            if (this is BeginShipTile)
            {
                this.attractorForce = new PointD(0, 0);
              //  refToLevel.Controller.OnShipSubmerged(this as BeginShipTile);

            }

            return 0;
        }

        protected bool DestroyAndSinkShipElement(LevelTile t)
        {
            if (t is ShipTile && !(t as ShipTile).IsSinking)
            {
                (t as ShipTile).Destroy();
            }
            else
                if (t is ShipBunkerTile && !(t as ShipBunkerTile).IsSinking)
                {
                    (t as ShipBunkerTile).Destroy();
                    (t as ShipBunkerTile).StartSinking();
                    refToLevel.Controller.OnTileDestroyed(this, null);
                    // kill soldiers
                    refToLevel.KillSoldiers(t.TileIndex, 2, true, false, true);
                }
                else
                {
                    return false;
                }
            return true;
        }

      

        #region Implementation of ISinkComponent

        public float DoSinking(float time, float timeUnit)
        {
            float amount = sinkComponent.DoSinking(time, timeUnit);
            
            attractorForce.X = 0;
            attractorForce.Y = -amount;

            if (amount > 0)
            {
                // TODO: nie dzia�a jak nale�y. znalezc bardziej eleganckie rozwi�zanie
                /*if(this.depth > this.MaxY)
                {
                    // under water
                      refToLevel.Controller.OnShipUnderWater(this);
                }
                */

                // toni�cie si� nie zako�czy�o
                refToLevel.Controller.OnShipSinking(this);
                return amount;
            }
            if (this is BeginShipTile)
            {
                this.attractorForce = new PointD(0, 0);
                refToLevel.Controller.OnShipSunk(this as BeginShipTile);

            }

            return 0;
        }

        public float DoSubmerge(float time, float timeUnit)
        {
            float amount = sinkComponent.DoSubmerge(time, timeUnit);
            return amount;
        }

        public float DoEmerge(float time, float timeUnit)
        {
            float amount = sinkComponent.DoEmerge(time, timeUnit);
            return amount;
        }

        public float Depth
        {
            get { return sinkComponent.Depth; }
        }

        public bool IsSinking
        {
            get { return sinkComponent.IsSinking; }
        }

        public bool IsSunkDown
        {
            get { return sinkComponent.IsSunkDown; }
        }


        public bool IsSubmerged
        {
            get { return sinkComponent.IsSubmerged; }
        }

        public bool IsSubmerging
        {
            get { return sinkComponent.IsSubmerging; }
        }

        public bool IsEmerging
        {
            get { return sinkComponent.IsEmerging; }
        }

        public void StartSinking()
        {
            sinkComponent.StartSinking();
        }

        public void StopSinking()
        {
            sinkComponent.StopSinking();
        }



        public void StartSubmerging()
        {
            sinkComponent.StartSubmerging();
        }

        public void StopSubmerging()
        {
            sinkComponent.StopSubmerging();
        }



        public void StartEmerging()
        {
            sinkComponent.StartEmerging();
        }

        public void StopEmerging()
        {
            sinkComponent.StopEmerging();
        }

		public bool IsEmerged {
			get {
				return sinkComponent.IsEmerged;
			}
		}
    	
        public float SubmergeTimeElapsed {
			get {
				return sinkComponent.SubmergeTimeElapsed;
			}
		}
    	
		public float SinkingTimeElapsed {
			get {
				return sinkComponent.SinkingTimeElapsed;
			}
		}

        public LevelTile Tile
        {
            get
            {
                return sinkComponent.Tile;
            }
        }
	
        #endregion

    	
		
		
    	
		
    }
}