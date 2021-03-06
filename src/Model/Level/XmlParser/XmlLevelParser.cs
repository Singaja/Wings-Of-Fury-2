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
using System.IO;
using System.Text;
using System.Xml;
using Wof.Controller;
using Wof.Controller.Screens;
using Wof.Model.Exceptions;
using Wof.Model.Level.Common;
using Wof.Model.Level.LevelTiles;
using Wof.Model.Level.LevelTiles.AircraftCarrierTiles;
using Wof.Model.Level.LevelTiles.IslandTiles;
using Wof.Model.Level.LevelTiles.IslandTiles.EnemyInstallationTiles;
using Wof.Model.Level.LevelTiles.IslandTiles.ExplosiveObjects;
using Wof.Model.Level.LevelTiles.Watercraft;
using Wof.Model.Level.LevelTiles.Watercraft.ShipManagers;
using Wof.Model.Configuration;

namespace Wof.Model.Level.XmlParser
{
    public class XmlLevelParser : IDisposable
    {
        #region Fields

        /// <summary>
        /// Pora dnia.
        /// </summary>
        private DayTime dayTime;


        private bool enhancedOnly;

        private string levelFile;

        
        
        /// <summary>
        /// Rodzaj misji
        /// </summary>
        private MissionType missionType = MissionType.BombingRun;

        /// <summary>
        /// Liczba mysliwcow wroga.
        /// </summary>
        private int enemyFighters;
        
         /// <summary>
        /// Liczba bombowcow wroga.
        /// </summary>
        private int enemyBombers;
        
        
        /// <summary>
        /// Czas do pojawienia sie pierwszego samolotu wroga
        /// </summary>
        private int timeToFirstEnemyPlane;
        
         /// <summary>
        /// Czas do pojawienia sie nast�pnego samolotu wroga
        /// </summary>
        private int timeToNextEnemyPlane;

        /// <summary>
        /// Obiekt wczytujacy plik tiles.xml
        /// </summary>
        private TilesManager tilesManager;

        /// <summary>
        /// Tablica obiektow znajdujacych sie w pliku tiles.xml
        /// </summary>
        private TilesNode[] tileNodeArray;

        /// <summary>
        /// Lista obiektow znajdujacych sie na planszy.
        /// </summary>
        private List<LevelTile> levelTiles;
        
         /// <summary>
        /// Lista obiektow znajdujacych sie na planszy.
        /// </summary>
        private List<Achievement> achievements;


        /// <summary>
        /// Lista statk�w znajdujacych sie na planszy.
        /// </summary>
        private List<ShipManager> shipManagers;

        #endregion

        #region Public Constructor 


       

        public XmlLevelParser(String path)
        {
            
            if (!File.Exists(path))
                throw new LevelFileNotFoundException(Path.GetFileName(path));

            levelFile = path;
            Initialize();
            if (!tilesManager.IsReadOK)
                throw new IOException("Error while reading tiles file ..." + path);

            tileNodeArray = new TilesNode[tilesManager.Dictionary.Values.Count];
            tilesManager.Dictionary.Values.CopyTo(tileNodeArray, 0);

            Read(path);
           
            SetIndex();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Funkcja inicjalizuje prywatne zmienne.
        /// </summary>
        private void Initialize()
        {
            levelTiles = new List<LevelTile>();
            achievements = new List<Achievement>();
            shipManagers = new List<ShipManager>();
            tilesManager = new TilesManager();

            timeToFirstEnemyPlane = GameConsts.EnemyFighter.Singleton.DefaultTimeToFirstEnemyPlane;
            timeToNextEnemyPlane = GameConsts.EnemyFighter.Singleton.DefaultTimeToNextEnemyPlane;
        }

        /// <summary>
        /// Ustawia index dla kazdego elementu na planszy.
        /// </summary>
        private void SetIndex()
        {
            if (levelTiles != null && levelTiles.Count > 0)
            {
                int count = levelTiles.Count;
                for (int i = 0; i < count; i++)
                {
                    levelTiles[i].TileIndex = i;
                }
            }
        }

        public static string GetAsString(byte[] bytes)
        {
            StringBuilder s = new StringBuilder();
            int length = bytes.Length;
            for (int n = 0; n < length; n++)
            {
                s.Append((int) bytes[n]);
                if (n != length - 1)
                {
                    s.Append(' ');
                }
            }
            return s.ToString();
        }

        #region Private Xml Services Methods



        public static void PeekMissionDetails(String path, out MissionType missionType, out bool enhancedOnly,  out List<Achievement> achievements)
        {

           
            if (!File.Exists(path))
                throw new LevelFileNotFoundException(Path.GetFileName(path));
            XmlReader reader = null;
            string contents = RijndaelSimple.Decrypt(File.ReadAllText(path));
            reader = XmlReader.Create(new StringReader(contents));

            enhancedOnly = false;
            missionType = MissionType.BombingRun; // nie zdefiniowano
            List<Achievement> achievementList = new List<Achievement>();
             
            while (reader.Read())
            {
            	  if (reader.Name.Equals(Nodes.Achievement)) {
	            	  Achievement achievement;
	            	  if ((achievement = ReadAchievement(reader)) == null){	                            
	            	  }
	            	  achievementList.Add(achievement);    
            	  }
                                
                  if (reader.Name.Equals(Nodes.Level) )
                  {
                      if (reader.HasAttributes)
                      {
                          for (int i = 0; i < reader.AttributeCount; i++)
                          {
                              reader.MoveToAttribute(i); 
                              
                              if (reader.Name.Equals(Attributes.MissionType, StringComparison.InvariantCultureIgnoreCase))
                              {
                                   missionType = GetMissionTypeForName(reader.Value);
                              }                              
                              else if (reader.Name.Equals(Attributes.EnhancedOnly, StringComparison.InvariantCultureIgnoreCase))
                              {
                                  try
                                  {
                                      enhancedOnly = Int32.Parse(reader.Value.Trim()) > 0 ? true : false;
                                  }
                                  catch (Exception)
                                  {

                                      enhancedOnly = false;
                                  }
                                   
                              }
                                  
                          }
                      }
                     
                  }
            }
           
           achievements = achievementList;

        }


        public static bool SaveLevel(Level level, String filename)
        {
            
            try
            {
               
                File.Create(filename).Close();
           
                XmlDocument xml = new XmlDocument();
                XmlNode docNode = xml.CreateXmlDeclaration("1.0", "UTF-8", null);
                xml.AppendChild(docNode);

                XmlNode rootNode = xml.CreateElement(Nodes.Level);

                XmlAttribute att = xml.CreateAttribute(Attributes.DayTime);
                att.Value = StringValueAttribute.GetStringValue(level.LevelParser.dayTime);
                rootNode.Attributes.Append(att);

                att = xml.CreateAttribute(Attributes.EnemyFighters);
                att.Value = level.LevelParser.enemyFighters.ToString();
                rootNode.Attributes.Append(att);
                
                att = xml.CreateAttribute(Attributes.EnemyBombers);
                att.Value = level.LevelParser.enemyBombers.ToString();
                rootNode.Attributes.Append(att);
               
                
                att = xml.CreateAttribute(Attributes.EnhancedOnly);
                att.Value = level.LevelParser.enhancedOnly ? "1" : "0";
                rootNode.Attributes.Append(att);

                

                att = xml.CreateAttribute(Attributes.MissionType);
                att.Value = StringValueAttribute.GetStringValue(level.LevelParser.missionType);
                rootNode.Attributes.Append(att);

                att = xml.CreateAttribute(Attributes.TimeToFirstEnemyPlane);
                att.Value = level.LevelParser.timeToFirstEnemyPlane.ToString();
                rootNode.Attributes.Append(att);

                att = xml.CreateAttribute(Attributes.TimeToNextEnemyPlane);
                att.Value = level.LevelParser.timeToNextEnemyPlane.ToString();
                rootNode.Attributes.Append(att);

                xml.AppendChild(rootNode);

                XmlElement xmlTile = null;
                foreach (var tile in level.LevelTiles)
                {
                    rootNode.AppendChild(xml.CreateWhitespace("\n"));
                    if (tile is OceanTile)
                    {
                      
                        OceanTile oceanTile = tile as OceanTile;
                        xmlTile = xml.CreateElement(oceanTile.GetXMLName);
                        AppendAttribute(xml, xmlTile, Attributes.Variation, oceanTile.Variant);
                        rootNode.AppendChild(xmlTile);
                    }
                    else if (tile is IslandTile)
                    {
                       
                        // island
                        if (tile is BeginIslandTile)
                        {
                            BeginIslandTile t = tile as BeginIslandTile;
                            xmlTile = xml.CreateElement(t.GetXMLName);
                            AppendAttribute(xml, xmlTile, Attributes.Variation, t.Variant);
                            AppendAttribute(xml, xmlTile, Attributes.Mesh, t.MeshName);
                        }
                        else if (tile is MiddleIslandTile || tile is EndIslandTile || tile is BarrelTile)
                        {
                            IslandTile t = tile as IslandTile;
                            xmlTile = xml.CreateElement(t.GetXMLName);
                            AppendAttribute(xml, xmlTile, Attributes.Variation, t.Variant);
                        }
                        else

                        // instalacje
                        if (tile is EnemyInstallationTile)
                        {
                            EnemyInstallationTile t = tile as EnemyInstallationTile;
                            xmlTile = xml.CreateElement(t.GetXMLName);
                            AppendAttribute(xml, xmlTile, Attributes.Variation, t.Variant);
                            AppendAttribute(xml, xmlTile, Attributes.NumSoldiers, t.SoldierCount);
                            AppendAttribute(xml, xmlTile, Attributes.Traversable, t.Traversable);
                            AppendAttribute(xml, xmlTile, Attributes.NumGenerals, t.GeneralCount);
                        }
                       
                    }
                    else if (tile is AircraftCarrierTile)
                    {
                        AircraftCarrierTile t = tile as AircraftCarrierTile;
                        xmlTile = xml.CreateElement(t.GetXMLName);
                        AppendAttribute(xml, xmlTile, Attributes.Variation, t.Variant);
                    }
                    else if (tile is BeginShipTile)
                    {
                        BeginShipTile t = tile as BeginShipTile;
                        xmlTile = xml.CreateElement(t.GetXMLName);
                        AppendAttribute(xml, xmlTile, Attributes.Variation, t.Variant);
                        AppendAttribute(xml, xmlTile, Attributes.Traversable, t.Traversable);
                        AppendAttribute(xml, xmlTile, Attributes.Type, t.TypeOfEnemyShip);
                    }
                    else if (tile is ShipTile)
                    {
                        ShipTile t = tile as ShipTile;
                        xmlTile = xml.CreateElement(t.GetXMLName);
                        AppendAttribute(xml, xmlTile, Attributes.Variation, t.Variant);
                        AppendAttribute(xml, xmlTile, Attributes.Traversable, t.Traversable);
                    }

                    if(xmlTile != null)
                    {
                        rootNode.AppendChild(xmlTile);
                    } else
                    {
                        // nieznany tile
                    }
                    
                }
                File.WriteAllText(filename.Replace(C_LEVEL_POSTFIX, ".xml"), xml.InnerXml);

                File.WriteAllText(filename, RijndaelSimple.Encrypt(xml.InnerXml));
               
              
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        protected static void AppendAttribute(XmlDocument xml, XmlElement parent, string name, object value)
        {
            XmlAttribute att = xml.CreateAttribute(name);
            att.Value = value.ToString();
            parent.Attributes.Append(att);
        }

        /// <summary>
        /// Funkcja wczytujaca plik level-x.xml.
        /// </summary>
        /// <param name="fileName">Nazwa pliku.</param> 
        private bool Read(String fileName)
        {
            XmlReader reader = null;
            try
            {
                int firstDigitPosition = fileName.LastIndexOf("level-") + 6;
                int numberLenght = fileName.LastIndexOf(".") - firstDigitPosition;
                string levelNumer = fileName.Substring(firstDigitPosition, numberLenght);
                
                // automatically reencode XML file to DAT file
                if (EngineConfig.AutoEncodeXMLs && File.Exists(fileName.Replace(C_LEVEL_POSTFIX, ".xml")))
                {
                    File.WriteAllText(fileName.Replace(".xml", C_LEVEL_POSTFIX), RijndaelSimple.Encrypt(File.ReadAllText(fileName.Replace(C_LEVEL_POSTFIX, ".xml"))));
                }
             

                string contents = RijndaelSimple.Decrypt(File.ReadAllText(fileName.Replace(".xml", C_LEVEL_POSTFIX)));

             

                int level;
                if(Int32.TryParse(levelNumer, out level))
                {
                    if (!SHA1_Hash.ValidateLevel(level, contents))
                        throw new Exception("Level is corrupted!");
                }

               

                reader = XmlReader.Create(new StringReader(contents));
                while (reader.Read())
                {
                    // Process only the elements
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        //Ocean
                        if (reader.Name.Equals(Nodes.Ocean))
                        {
                            if (!ReadOcean(reader))
                                throw new XmlException("ocean");
                        }
                            //Level attributes
                        else if (reader.Name.Equals(Nodes.Level))
                        {
                            if (!ReadLevelAttributes(reader))
                                throw new XmlException("level");
                        }
                            //IslandEnd
                        else if (reader.Name.Equals(Nodes.IslandEnd))
                        {
                            if (!ReadIslandEnd(reader))
                                throw new XmlException("Island End");
                        }
                            //IslandBegin
                        else if (reader.Name.Equals(Nodes.IslandBegin))
                        {
                            if (!ReadIslandBegin(reader))
                                throw new XmlException("Island begin");
                        }
                      
                            //WoodenBunker, Barracks,Concretebunker,Fortress bunker
                        else if (reader.Name.Equals(Nodes.WoodenBunker) ||
                                 reader.Name.Equals(Nodes.Barrack) ||
                                 reader.Name.Equals(Nodes.FlakBunker) ||
                                 reader.Name.Equals(Nodes.ShipWoodenBunker) ||
                                 reader.Name.Equals(Nodes.ShipConcreteBunker) ||
                                 reader.Name.Equals(Nodes.ConcreteBunker) ||
                                 reader.Name.Equals(Nodes.FortressBunker))
                        {
                            if (!ReadBunkerOrBarrack(reader,reader.Name))
                                throw new XmlException("Bunker");
                        }
                            //AircroftCarier
                        else if (reader.Name.Contains(Nodes.AircraftCarrier))
                        {
                            if (!ReadAircraftCarrierElement(reader, reader.Name))
                                throw new XmlException("AircraftCarier");
                        }
                            // Ship
                        else if (reader.Name.Contains(Nodes.Ship))
                        {
                            if (!ReadShipElement(reader, reader.Name))
                                throw new XmlException("Ship");
                        }

                            //terrain
                        else if (reader.Name.Equals(Nodes.Terrain))
                        {
                            if (!ReadTerrain(reader))
                                throw new XmlException("Terrain");
                        }
                            //barrels
                        else if (reader.Name.Equals(Nodes.Barrels))
                        {
                            if (!ReadBarrels(reader))
                                throw new XmlException("Barrels");
                        }  else if (reader.Name.Equals(Nodes.Achievement))
                        {
                        	 
                        	Achievement achievement;
                        	 if ((achievement =ReadAchievement(reader)) == null){
                                throw new XmlException("achievement");
                        	 }
                        	 achievements.Add(achievement);
                        }
                    }
                }

                return true;
            }
            catch (XmlException exc)
            {
                throw new LevelCorruptedException("Element " + exc.Message + " nie zostal wczytany !",
                                                  Path.GetFileName(fileName));
            }
            catch (Exception exc)
            {
                throw new LevelCorruptedException(exc.Message, Path.GetFileName(fileName));
            }
            finally
            {
                try
                {
                    if (reader != null)
                        reader.Close();
                }
                catch
                {
                }
            }
        }
   
        #region Read Terrain

        private bool ReadTerrain(XmlReader reader)
        {
            int width = -1;
            int variation = 0;
            bool traversable = true;
            if (reader.HasAttributes) //Read attributes
            {
                for (int i = 0; i < reader.AttributeCount; i++)
                {
                    reader.MoveToAttribute(i);
                    if (reader.Name.Equals(Attributes.Width))
                    {
                        try
                        {
                            width = Int32.Parse(reader.Value);
                        }
                        catch
                        {
                            return false;
                        }
                    }
                    else if (reader.Name.Equals(Attributes.Variation))
                    {
                        try
                        {
                            variation = Int32.Parse(reader.Value);
                        }
                        catch
                        {
                            return false;
                        }
                    }
                    else if (reader.Name.Equals(Attributes.Traversable))
                    {
                        try
                        {
                            traversable = Boolean.Parse(reader.Value);
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
            }
            else return false;
            //Add to list
            TilesNode node;
            if (variation < 0) variation = 0;
            node = GetTilesForID(TilesNode.GenerateID(Nodes.Terrain,variation));
            
            if (node == null) return false;
            if (width <= 0) width = 1;

            for (int i = 0; i < width; i++)
            {
                MiddleIslandTile terrain =
                    new MiddleIslandTile(node.YStart, node.YEnd, node.ViewXShift, node.HitRectangle, variation, node.CollisionRectangle, traversable);
                levelTiles.Add(terrain);
            }
            return true;
        }

        #endregion

        #region Read Barrels

        private bool ReadBarrels(XmlReader reader)
        {
            int variation = 0;
            int width = 1;
            if (reader.HasAttributes) //Read attributes
            {
                for (int i = 0; i < reader.AttributeCount; i++)
                {
                    reader.MoveToAttribute(i);
                    if (reader.Name.Equals(Attributes.Variation))
                    {
                        try
                        {
                            variation = Int32.Parse(reader.Value);
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
            }
            else return false;
            //Add to list
            TilesNode node;
            if (variation < 0) variation = 0;
            node = GetTilesForID(TilesNode.GenerateID(Nodes.Barrels,variation));
            if (node == null) return false;
            if (width <= 0) width = 1;

            for (int i = 0; i < width; i++)
            {
                BarrelTile barrel = new BarrelTile(node.YStart, node.YEnd, node.ViewXShift, node.CollisionRectangle[0], variation, node.CollisionRectangle);
                levelTiles.Add(barrel);
            }
            return true;
        }

        #endregion

        #region Read Aircraft

        private bool ReadAircraftCarrierElement(XmlReader reader, String fullName)
        {
            int variation = 0;
            int width = 1;
            if (reader.HasAttributes)
            {
                for (int i = 0; i < reader.AttributeCount; i++)
                {
                    reader.MoveToAttribute(i);
                    if (reader.Name.Equals(Attributes.Width))
                    {
                        try
                        {
                            width = Int32.Parse(reader.Value);
                        }
                        catch
                        {
                            return false;
                        }
                    }

                    if (reader.Name.Equals(Attributes.Variation))
                    {
                        try
                        {
                            variation = Int32.Parse(reader.Value);
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
            }
            else return false;
            //wczytanie sie powiodlo teraz nalezy stworzyc obiekt.
            AircraftCarrierTile aircraft = null;
            TilesNode node = GetTilesForID(TilesNode.GenerateID(fullName, variation));

            if (node == null) return false;
            for (int i = 0; i < width; i++)
            {
                if (fullName.EndsWith(AircraftElement.End))
                    aircraft = new EndAircraftCarrierTile(node.YStart, node.YEnd, node.ViewXShift,
                                                          node.HitRectangle);
                else if (fullName.EndsWith(AircraftElement.Begin))
                    aircraft = new BeginAircraftCarrierTile(node.YStart, node.YEnd, node.ViewXShift,
                                                            node.HitRectangle);
                else if (fullName.EndsWith(AircraftElement.Middle))
                    aircraft = new MiddleAircraftCarrierTile(node.YStart, node.YEnd, node.ViewXShift,
                                                             node.HitRectangle);
                else
                    aircraft = new RestoreAmmunitionCarrierTile(node.YStart, node.YEnd, node.ViewXShift,
                                                                node.HitRectangle);
                levelTiles.Add(aircraft);
            }
            return true;
        }

        #endregion

        #region Read Ship

        private bool ReadShipElement(XmlReader reader, String fullName)
        {
            int width = 1;
            int variation = 0;
            bool traversable = true;
            //domyslny typ statku
            TypeOfEnemyShip typeOfEnemyShip = TypeOfEnemyShip.PatrolBoat;
            if (reader.HasAttributes) //Read attributes
            {
                for (int i = 0; i < reader.AttributeCount; i++)
                {
                    reader.MoveToAttribute(i);
                    if (reader.Name.Equals(Attributes.Width))
                    {
                        try
                        {
                            width = Int32.Parse(reader.Value);
                        }
                        catch
                        {
                            return false;
                        }
                    }
                    else if (reader.Name.Equals(Attributes.Variation))
                    {
                        try
                        {
                            variation = Int32.Parse(reader.Value);
                        }
                        catch
                        {
                            return false;
                        }
                    }
                    else if (reader.Name.Equals(Attributes.Traversable))
                    {
                        try
                        {
                            traversable = Boolean.Parse(reader.Value);
                        }
                        catch
                        {
                            return false;
                        }
                    }
                    else if (reader.Name.Equals(Attributes.Type))
                    {
                        try
                        {
                            typeOfEnemyShip = (TypeOfEnemyShip)Enum.Parse(typeof(TypeOfEnemyShip), reader.Value, true);
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
            }
            else return false;
            //Add to list
            TilesNode node;
            if (variation < 0) variation = 0;
            //wczytanie sie powiodlo teraz nalezy stworzyc obiekt.
            ShipTile pb = null;
            node = GetTilesForID(TilesNode.GenerateID(fullName, variation));

            if (node == null) return false;
            for (int i = 0; i < width; i++)
            {
                if (fullName.EndsWith(ShipElement.End))
                    pb = new EndShipTile(node.YStart, node.YEnd, node.ViewXShift,
                                                          node.HitRectangle, variation, node.CollisionRectangle, traversable);
                else if (fullName.EndsWith(ShipElement.Begin))
                {
                    ShipTile shipTile = new BeginShipTile(node.YStart, node.YEnd, node.ViewXShift,
                                                            node.HitRectangle, variation, node.CollisionRectangle, traversable, typeOfEnemyShip);
                    shipManagers.Add(shipTile.ShipOwner);
                    pb = shipTile;
                }
                else if (fullName.EndsWith(ShipElement.Middle))
                    pb = new MiddleShipTile(node.YStart, node.YEnd, node.ViewXShift, node.HitRectangle, variation, node.CollisionRectangle, traversable);
               
                levelTiles.Add(pb);
            }
            return true;
        }

        #endregion

        #region Bunkers, Barrack, ShipB unkers read

        private bool ReadBunkerOrBarrack(XmlReader reader, String bunkerName)
        {
            int numSoldiers = -1;
            int numGenerals = 0;
            int variation = 0;
            bool hasRockets = false;
            bool traversable = true;
            if (reader.HasAttributes)
            {
                for (int i = 0; i < reader.AttributeCount; i++)
                {
                    reader.MoveToAttribute(i);
                    if (reader.Name.Equals(Attributes.NumSoldiers))
                    {
                        try
                        {
                            numSoldiers = Int32.Parse(reader.Value);
                        }
                        catch
                        {
                            return false;
                        }
                    }
                    if (reader.Name.Equals(Attributes.HasRockets))
                    {
                        try
                        {
                            hasRockets = Int32.Parse(reader.Value) > 0;
                        }
                        catch
                        {
                            return false;
                        }
                    }else if (reader.Name.Equals(Attributes.NumGenerals))
                    {
                        try
                        {
                            numGenerals = Int32.Parse(reader.Value);
                        }
                        catch
                        {
                            return false;
                        }
                    }
                    else if (reader.Name.Equals(Attributes.Variation))
                    {
                        try
                        {
                            variation = Int32.Parse(reader.Value);
                        }
                        catch
                        {
                            return false;
                        }
                    }
                    else if (reader.Name.Equals(Attributes.Traversable))
                    {
                        try
                        {
                            traversable = Boolean.Parse(reader.Value);
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
            }
           // else return false;
            if (variation < 0) variation = 0;
            TilesNode node = GetTilesForID(TilesNode.GenerateID(bunkerName, variation));
            if (node == null) return false;
            BunkerTile bunker = null;
            BarrackTile barrack = null;
            ShipBunkerTile shipbunker = null;

            if (bunkerName.Equals(Nodes.WoodenBunker))
                bunker = new WoodBunkerTile(node.YStart,
                                            node.YEnd, node.ViewXShift, node.HitRectangle, numSoldiers, numGenerals, variation,
                                            node.CollisionRectangle);
            else if (bunkerName.Equals(Nodes.Barrack))
                barrack = new BarrackTile(node.YStart, node.YEnd, node.ViewXShift,
                                          node.HitRectangle, numSoldiers, numGenerals, variation, node.CollisionRectangle);
            else if (bunkerName.Equals(Nodes.ShipWoodenBunker))
                shipbunker = new ShipWoodBunkerTile(node.YStart, node.YEnd, node.ViewXShift,
                                          node.HitRectangle, numSoldiers, numGenerals, variation, node.CollisionRectangle);
            else if (bunkerName.Equals(Nodes.ShipConcreteBunker))
            {
                shipbunker = new ShipConcreteBunkerTile(node.YStart, node.YEnd, node.ViewXShift,
                                                        node.HitRectangle, numSoldiers, numGenerals, variation,
                                                        node.CollisionRectangle);
               
            }  else if (bunkerName.Equals(Nodes.FlakBunker))
            {
                bunker = new FlakBunkerTile(node.YStart, node.YEnd, node.ViewXShift,
                                                        node.HitRectangle, numSoldiers, numGenerals, variation,
                                                        node.CollisionRectangle);
               
            }
            else if (bunkerName.Equals(Nodes.FortressBunker))
                bunker = new FortressBunkerTile(node.YStart, node.YEnd, node.ViewXShift,
                                          node.HitRectangle, numSoldiers, numGenerals, variation, node.CollisionRectangle);

            else
                bunker = new ConcreteBunkerTile(node.YStart, node.YEnd, node.ViewXShift,
                                                node.HitRectangle, numSoldiers, numGenerals, variation, node.CollisionRectangle);

          
            
            if (bunker != null)
            {
                bunker.HasRockets = hasRockets;
                bunker.Traversable = traversable;
                levelTiles.Add(bunker);
            }
              
            else if (shipbunker != null)
            {
             
                shipbunker.HasRockets = hasRockets;
                shipbunker.Traversable = traversable;
                levelTiles.Add(shipbunker);
            }
            else
            {
                barrack.Traversable = traversable;
                levelTiles.Add(barrack);
            }

            return true;
        }

        #endregion

        #region Begin,End island

        private bool ReadIslandBegin(XmlReader reader)
        {
            string mesh = null;
            int variation = 0;
            if (reader.HasAttributes)
            {
                for (int i = 0; i < reader.AttributeCount; i++)
                {
                    reader.MoveToAttribute(i);
                    if (reader.Name.Equals(Attributes.Variation))
                    {
                        try
                        {
                            variation = Int32.Parse(reader.Value);
                        }
                        catch
                        {
                            return false;
                        }
                    }

                    if (reader.Name.Equals(Attributes.Mesh))
                    {
                        mesh = reader.Value;
                    }

                }
            }

            if (mesh == null) return false;
            if (variation < 0) variation = 0;
            TilesNode node = GetTilesForID(TilesNode.GenerateID(Nodes.IslandBegin, variation));
            if (node != null)
            {
                if (node.IsValidYEnd && node.IsValidYStart)
                    levelTiles.Add(new BeginIslandTile(node.YStart, node.YEnd, node.ViewXShift, node.HitRectangle, variation, mesh));
                else return false;
                return true;
            }
            else return false;
        }

        private bool ReadIslandEnd(XmlReader reader)
        {
            int variation = 0;
            if (reader.HasAttributes)
            {
                for (int i = 0; i < reader.AttributeCount; i++)
                {
                    reader.MoveToAttribute(i);
                    if (reader.Name.Equals(Attributes.Variation))
                    {
                        try
                        {
                            variation = Int32.Parse(reader.Value);
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
            }
            if (variation < 0) variation = 0;
            TilesNode node = GetTilesForID(TilesNode.GenerateID(Nodes.IslandEnd, variation));
            if (node != null)
            {
                if (node.IsValidYEnd && node.IsValidYStart)
                    levelTiles.Add(new EndIslandTile(node.YStart, node.YEnd, node.ViewXShift, node.HitRectangle, variation));
                else return false;
                return true;
            }
            else return false;
        }

        #endregion

        #region Read Level Attributes

        
        
        private static Achievement ReadAchievement(XmlReader reader)
        {
        	AchievementType? type = null;
        	int amount = -1;
            if (reader.HasAttributes)
            {
            	try{
	                for (int i = 0; i < reader.AttributeCount; i++)
	                {
	                    reader.MoveToAttribute(i);
	                    if (reader.Name.Equals(Attributes.Type, StringComparison.InvariantCultureIgnoreCase))
	                        type = GetAchievementType(reader.Value);
	
	                    if (reader.Name.Equals(Attributes.Amount, StringComparison.InvariantCultureIgnoreCase))
	                    {
	                        amount = Int32.Parse(reader.Value.Trim());                       
	                    }                    
	
	                }
            	}
            	catch(Exception ex)
            	{
            		throw ex;
            	}
            }
            else return null;
            if(type == null || amount == -1){
            	throw new XmlException("Achievement is invalid - no valid 'type' or 'amount' specified");
            }
            
            Achievement achievement = new Achievement(type.Value, amount);
           

            return achievement;
        }

        /// <summary>
        /// Funkcja wczytuje wlasciwosci dla wezla Level.
        /// </summary>
        /// <param name="reader">Strumien do pliku xml.</param>
        /// <returns>Jesli wczytywanie powiodlo sie zwraca true, 
        /// false - w przeciwnym przypadku</returns>
        private bool ReadLevelAttributes(XmlReader reader)
        {
            if (reader.HasAttributes)
            {
                for (int i = 0; i < reader.AttributeCount; i++)
                {
                    reader.MoveToAttribute(i);
                    if (reader.Name.Equals(Attributes.DayTime, StringComparison.InvariantCultureIgnoreCase))
                        dayTime = GetDayTimeForName(reader.Value);

                    if (reader.Name.Equals(Attributes.EnhancedOnly, StringComparison.InvariantCultureIgnoreCase))
                    {
                        try
                        {
                            enhancedOnly = Int32.Parse(reader.Value.Trim()) > 0 ? true : false;
                        }
                        catch
                        {
                            enhancedOnly = false;
                            return false;
                        }
                    }
                    

                    if (reader.Name.Equals(Attributes.MissionType, StringComparison.InvariantCultureIgnoreCase))
                        missionType = GetMissionTypeForName(reader.Value);

                    if (reader.Name.Equals(Attributes.EnemyFighters, StringComparison.InvariantCultureIgnoreCase))
                    {
                        try
                        {
                            enemyFighters = Int32.Parse(reader.Value.Trim());
                        }
                        catch
                        {
                            enemyFighters = 0;
                            return false;
                        }
                    }
                    
                    if (reader.Name.Equals(Attributes.EnemyBombers, StringComparison.InvariantCultureIgnoreCase))
                    {
                        try
                        {
                            enemyBombers = Int32.Parse(reader.Value.Trim());
                        }
                        catch
                        {
                            enemyBombers = 0;
                            return false;
                        }
                    }

                    if (reader.Name.Equals(Attributes.TimeToFirstEnemyPlane, StringComparison.InvariantCultureIgnoreCase))
                    {
                        try
                        {
                            timeToFirstEnemyPlane = Int32.Parse(reader.Value.Trim());
                        }
                        catch
                        {
                            timeToFirstEnemyPlane = GameConsts.EnemyFighter.Singleton.DefaultTimeToFirstEnemyPlane;
                            return false;
                        }
                    }
                    if (reader.Name.Equals(Attributes.TimeToNextEnemyPlane, StringComparison.InvariantCultureIgnoreCase))
                    {
                        try
                        {
                            timeToNextEnemyPlane = Int32.Parse(reader.Value.Trim());
                        }
                        catch
                        {
                            timeToNextEnemyPlane = GameConsts.EnemyFighter.Singleton.DefaultTimeToNextEnemyPlane;
                            return false;
                        }
                    }    
                }
            }
            else return false;

            return true;
        }

        #endregion

        #region Read Ocean

        /// <summary>
        /// Funkcja wczytuje obiekt Ocean oraz dodaje go
        /// do listy.
        /// </summary>
        /// <param name="reader">Strumien do pliku xml.</param>
        private bool ReadOcean(XmlReader reader)
        {
            int variation = 0;

            // Check if the element has any attributes
            if (reader.HasAttributes)
            {
                int width = 1;
                for (int i = 0; i < reader.AttributeCount; i++)
                {
                    //wczytanie zmiennej Width
                    reader.MoveToAttribute(i);
                    if (reader.Name.Equals(Attributes.Width))
                    {
                        try
                        {
                            width = Int32.Parse(reader.Value);
                        }
                        catch
                        {
                            return false;
                        }
                    }

                    if (reader.Name.Equals(Attributes.Variation))
                    {
                        try
                        {
                            variation = Int32.Parse(reader.Value);
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
              
                //dodanie elementow ocean do listy.
                TilesNode tilesNode = GetTilesForID(TilesNode.GenerateID(Nodes.Ocean, variation));
                if (tilesNode == null) return false;
                if (tilesNode.IsValidYEnd && tilesNode.IsValidYStart)
                {
                    OceanTile ocean = null;
                    for (int i = 0; i < width; i++)
                    {
                        ocean = new OceanTile(tilesNode.YStart, tilesNode.YEnd, tilesNode.ViewXShift, tilesNode.HitRectangle, variation , tilesNode.CollisionRectangle);
                        levelTiles.Add(ocean);
                    }
                }
                reader.MoveToElement();
            }
            else return false;

            return true;
        }

        #endregion

        #endregion

        #region Private Common Methods

        /// <summary>
        /// Funkcja pobiera obiekt typu TilesNode z tablicy 
        /// tiles-ow na podstawie nazwy.
        /// </summary>
        /// <param name="id">Nazwa obiektu.</param>
        /// <returns>Jesli szukany obiekt istnieje w tablicy zostanie zwrocony,
        /// w przeciwnym przypadku zwruci null.</returns>
        private TilesNode GetTilesForID(String id)
        {
            TilesNode tiles = null;
            foreach (TilesNode n in tileNodeArray)
            {
                if (n.IsValidName)
                    if (n.ID.Equals(id)) //|| n.ID.Contains(id))
                    {
                        tiles = n;
                        break;
                    }
            }

            return tiles;
        }

        /// <summary>
        /// Funkcja konwertuje nazwe na pore dnia.
        /// </summary>
        /// <param name="name">Nazwa pory dnia.</param>
        /// <returns>Pora dnia.</returns>
        private DayTime GetDayTimeForName(String name)
        {
            if (name.Equals("dawn", StringComparison.InvariantCultureIgnoreCase))
                return DayTime.Dawn;
            else if (name.Equals("morning", StringComparison.InvariantCultureIgnoreCase))
                return DayTime.Dawn;
             else if (name.Equals("morning2", StringComparison.InvariantCultureIgnoreCase))
                return DayTime.Dawn2;
              else if (name.Equals("dusk", StringComparison.InvariantCultureIgnoreCase))
                return DayTime.Dusk;
             else if (name.Equals("dusk2", StringComparison.InvariantCultureIgnoreCase))
                return DayTime.Dusk2;
            else if (name.Equals("noon", StringComparison.InvariantCultureIgnoreCase))
                return DayTime.Noon;
            else if (name.Equals("foggy", StringComparison.InvariantCultureIgnoreCase))
                return DayTime.Foggy;
            else
                return DayTime.Night;
        }
        
        private static AchievementType GetAchievementType(String type)
        {
        	
        	AchievementType ret = (AchievementType)AchievementType.Parse(typeof(AchievementType), type, true);
        	return ret;
          
        }
        
        
        
        /// <summary>
        /// Funkcja konwertuje stringa na odpowiadajacy mission type
        /// </summary>
        /// <param name="name">Nazwa typu misji</param>
        /// <returns>Pora dnia.</returns>
        private static MissionType GetMissionTypeForName(String name)
        {
            if (name.Equals("BombingRun", StringComparison.InvariantCultureIgnoreCase))
                return MissionType.BombingRun;
            else
            if (name.Equals("Survival", StringComparison.InvariantCultureIgnoreCase))
                return MissionType.Survival;
            else if (name.Equals("Assassination", StringComparison.InvariantCultureIgnoreCase))
                return MissionType.Assassination;
            else if (name.Equals("Dogfight", StringComparison.InvariantCultureIgnoreCase))
                return MissionType.Dogfight;
            else if (name.Equals("Naval", StringComparison.InvariantCultureIgnoreCase))
                return MissionType.Naval;
            else
                return MissionType.BombingRun;
        }
        
        
        
     
        
        

        #endregion

        #endregion

        #region Properties

        public DayTime DayTime
        {
            get { return dayTime; }
        }
        
        public MissionType MissionType
        {
            get { return missionType; }
        }

        public int EnemyFighters
        {
            get { return enemyFighters; }
            //set { enemyPlanes = value; }
        }
        
        public int EnemyBombers
        {
            get { return enemyBombers; }
            //set { enemyPlanes = value; }
        }

        public int TimeToFirstEnemyPlane
        {
            get { return timeToFirstEnemyPlane; }
            //set { timeToFirstEnemyPlane = value; }
        }
        
        public int TimeToNextEnemyPlane
        {
            get { return timeToNextEnemyPlane; }
            //set { timeToNextEnemyPlane = value; }
        }

        /// <summary>
        /// Zwraca liste wczytanych obiektow.
        /// </summary>
        public List<LevelTile> Tiles
        {
            get { return levelTiles; }
        }

        
        public List<Achievement> Achievements
        {
        	get { return achievements; }
        	
        }
        /// <summary>
        /// Zwraca list� statk�w[manager�w] znajdujacych sie na planszy.
        /// </summary>
        public List<ShipManager> ShipManagers
        {
            get { return shipManagers; }
        }

        public string LevelFile
        {
            get { return levelFile; }
        }

        public bool EnhancedOnly
        {
            get { return enhancedOnly; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            tilesManager = null;
            tileNodeArray = null;
            if (levelTiles != null)
                levelTiles.Clear();
            levelTiles = null;
        }

        #endregion

        public const String C_LEVEL_FOLDER = "levels";
        public const String C_LEVEL_PREFIX = "level-";
        public const String C_LEVEL_POSTFIX = ".dat";

        public static String GetLevelFileName(uint levelNo)
        {
            return C_LEVEL_FOLDER + "\\"
                   + C_LEVEL_PREFIX + levelNo + C_LEVEL_POSTFIX;
        }
    }
}