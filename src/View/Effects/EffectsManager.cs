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
using System.Collections;
using Mogre;
using Wof.Misc;
using Wof.View.NodeAnimation;
using Math=System.Math;

namespace Wof.View.Effects
{
    /// <summary>
    /// Statyczna klasa pozwalaj�ca na u�ycie prostych, animowanych efekt�w typu sprite, rectangluareffect, particlesystem
    /// Klasa realizuje tak�e klonowanie materia��w oraz preloading efekt�w.
    /// <author>Adam Witczak</author>
    /// </summary>
    public sealed class EffectsManager
    {
        public struct EffectInfo
        {
            public string material;
            public EffectType type;
            public float duration;
        }

        /// <summary>
        /// Jak cz�sto mo�e by� powtarzany efekt typu sprite
        /// </summary>
        private const int minRepeatDelay = 100; // 100 ms


        public enum SmokeType
        {
            NORMAL,
            ROCKET,
            SMOKETRAIL,
            LIGHTSMOKE,
            DARKLIGHTSMOKE
        } ;

        private Hashtable effects = null;
        private ParticleManager smokeMgr;
        private ParticleManager rocketSmokeMgr;
        private ParticleManager trailSmokeMgr;
        private ParticleManager lightSmokeMgr;
        private ParticleManager darkLightSmokeMgr;
        

        private BillboardSet cloudsBS1;
        private BillboardSet cloudsBS2;
        private bool isLoaded = false;
        private Hashtable smokeSystems; // <ParticleSystem> -> <node>

        private static readonly EffectsManager singleton = new EffectsManager();

        public static EffectsManager Singleton
        {
            get { return singleton; }
        }

        public enum EffectType
        {
            EXPLOSION1,
            EXPLOSION2,
            WATERIMPACT1,
            WATERIMPACT2,
            DIRTIMPACT1,
            DIRTIMPACT2,
            FIRE,
            PLANECRASH,
            GUNHIT,
            SUBMERGE,
            SEAGULL,
            WATERTRAIL,
            PLANEWATERTRAIL,
            BLOOD
        }

        private enum MaterialToPreload
        {
            General,
            Soldier,
            Wood,
            Concrete,
            Rocket
        }

        private BillboardSet billboardSet;


        static EffectsManager()
        {
        }

        private EffectsManager()
        {
            Load();
        }

        public void Clear()
        {
            if (effects != null)
            {
                /* foreach (EffectTextureAnimation a in effects.Values)
                {
                    a.Destroy();
                }*/
                effects.Clear();
            }
            if (smokeSystems != null)
            {
                /*  IDictionaryEnumerator i = smokeSystems.GetEnumerator();
                 while (i.MoveNext())
                 {
                      (i.Key as ParticleSystem).RemoveAllEmitters();
                      (i.Value as SceneNode).Creator.DestroyParticleSystem((i.Key as ParticleSystem));
                 }*/
                smokeSystems.Clear();
            }
        }

        public void Load()
        {
            if (!isLoaded)
            {
                smokeSystems = new Hashtable();
                effects = new Hashtable();
                smokeMgr = new ParticleManager("SmokeSystem", "Smokes/Smoke");
                lightSmokeMgr = new ParticleManager("LightSmokeSystem", "Smokes/LightSmoke");
                rocketSmokeMgr = new ParticleManager("RocketSmokeSystem", "Smokes/RocketSmoke");
                trailSmokeMgr = new ParticleManager("TrailSmokeSystem", "Smokes/TrailSmoke");
                darkLightSmokeMgr = new ParticleManager("DarkLightSmokeSystem", "Smokes/DarkLightSmoke");

                BuildMaterials();
                PreloadEffects();
                isLoaded = true;
            }
        }

        private static void BuildMaterials()
        {
            // build materials
            Pass pass;
            MaterialPtr m;

            m = ViewHelper.CloneMaterial("Wood", "DestroyedWood");
            pass = m.GetBestTechnique().GetPass("Texture");
            pass.GetTextureUnitState(0).SetTextureName("oldwood2_destroyed.jpg");
            m = null;

            m = ViewHelper.CloneMaterial("Concrete", "DestroyedConcrete");
            pass = m.GetBestTechnique().GetPass("Texture");
            pass.GetTextureUnitState(0).SetTextureName("concrete_destroyed.jpg");
            m = null;


            // Preload materials
            IEnumerator i = Enum.GetNames(typeof (MaterialToPreload)).GetEnumerator();
            String enumName;
            while (i.MoveNext())
            {
                enumName = (i.Current as String);
                m = MaterialManager.Singleton.Load(enumName, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
                m.Touch();
                m = null;
            }
        }

        public void PreloadEffects2(SceneManager sceneMgr)
        {
            IEnumerator i = Enum.GetNames(typeof (EffectType)).GetEnumerator();
            String matName, spriteName;
            while (i.MoveNext())
            {
                spriteName = (i.Current as String);
                EffectType et = (EffectType) Enum.Parse(typeof (EffectType), spriteName);
                matName = GetEffectInfo(et).material;
                MaterialManager.Singleton.GetByName(matName).Load();
                Singleton.Sprite(sceneMgr, sceneMgr.RootSceneNode, new Vector3(0, 0, 0), new Vector2(0.5f, 0.5f), et,
                                 false, 0);
            }
        }

        private static void PreloadEffects()
        {
            IEnumerator i = Enum.GetNames(typeof (EffectType)).GetEnumerator();
            String matName, spriteName, imgPath;
            TextureUnitState state;
            while (i.MoveNext())
            {
                spriteName = (i.Current as String);
                matName = GetEffectInfo((EffectType) Enum.Parse(typeof (EffectType), spriteName)).material;
                MaterialPtr m = MaterialManager.Singleton.GetByName(matName);
                m.Load();
                m.Touch();
                state = m.GetBestTechnique().GetPass(0).GetTextureUnitState(0);
                for (uint j = 0; j < state.NumFrames; j++)
                {
                    imgPath = state.GetFrameTextureName(j);
                    TexturePtr tex = TextureManager.Singleton.Load(imgPath, "General");
                    tex.Load();
                    tex.Touch();
                    tex = null;
                }
                m = null;
            }

            //  ResourceGroupManager.Singleton.LoadResourceGroup("General", true, false);

            /*
           StringVectorPtr p =
                ResourceGroupManager.Singleton.ListResourceNames("General", false);

           for (int k = 0; k < p.Count; k++)
           {
               Console.WriteLine(p[k]);
           }*/
        }

        public bool EffectExists(string name)
        {
            return effects.ContainsKey(name);
        }

        /// <summary>
        /// Animuje wszystkie efekty.
        /// </summary>
        /// <param name="timeSinceLastFrame"></param>
        public void UpdateTimeAndAnimateAll(float timeSinceLastFrame)
        {
            foreach (NodeAnimation.NodeAnimation a in effects.Values)
            {
                a.updateTime(timeSinceLastFrame);
                a.animate();
            }

            // wylacz emitery ktore sa pod woda
            SceneNode node;
            ParticleSystem system;
            ParticleEmitter emitter;
            IDictionaryEnumerator i = smokeSystems.GetEnumerator();
            while (i.MoveNext())
            {
                system = (i.Key as ParticleSystem);
                emitter = system.GetEmitter(0);
                node = i.Value as SceneNode;
                if (emitter.Enabled && node.WorldPosition.y < 0) // node jest pod wod�
                {
                    emitter.Enabled = false;
                }
                else
                {
                    // dym wraca gdy wylonimy sie znad wody
                    if (node.WorldPosition.y > 0)
                    {
                        emitter.Enabled = true;
                    }
                }
            }
        }

        private ParticleManager GetSmokeManager(SmokeType type)
        {
            ParticleManager m = smokeMgr;
            if (type == SmokeType.NORMAL)
            {
                m = smokeMgr;
            }
            else if (type == SmokeType.LIGHTSMOKE)
            {
                m = lightSmokeMgr;
            }
            else if (type == SmokeType.ROCKET)
            {
                m = rocketSmokeMgr;
            }
            else if (type == SmokeType.SMOKETRAIL)
            {
                m = trailSmokeMgr;
            }else if (type == SmokeType.DARKLIGHTSMOKE)
            {
                m = darkLightSmokeMgr;
            }

            
            return m;
        }

        public void Smoke(SceneManager sceneMgr, SceneNode parent, SmokeType type, Vector3 localPosition,
                          Vector3 direction, Vector2 defaultParticleSize)
        {
            Smoke(sceneMgr, parent, type, localPosition, direction, defaultParticleSize, true);
        }

        public void Smoke(SceneManager sceneMgr, SceneNode parent, SmokeType type, Vector3 localPosition,
                          Vector3 direction, Vector2 defaultParticleSize, bool enabled)
        {
            ParticleManager m = GetSmokeManager(type);
            ParticleSystem e = m.Start(sceneMgr, parent, localPosition, direction, defaultParticleSize, enabled);
            if (e != null) smokeSystems[e] = parent;
        }

        public void Smoke(SceneManager sceneMgr, SceneNode parent, Vector3 localPosition, Vector3 direction,
                          Vector2 defaultParticleSize)
        {
            ParticleSystem e = smokeMgr.Start(sceneMgr, parent, localPosition, direction, defaultParticleSize);
            if (e != null) smokeSystems[e] = parent;
        }


        public void Smoke(SceneManager sceneMgr, SceneNode parent, Vector3 localPosition, Vector3 direction)
        {
            ParticleSystem e = smokeMgr.Start(sceneMgr, parent, localPosition, direction);
            if (e != null) smokeSystems[e] = parent;
        }

        public void Smoke(SceneManager sceneMgr, SceneNode parent)
        {
            ParticleSystem e = smokeMgr.Start(sceneMgr, parent);
            if (e != null) smokeSystems[e] = parent;
        }


        public bool IsSmoking(SceneManager sceneMgr, SceneNode parent, SmokeType type)
        {
            ParticleManager m = GetSmokeManager(type);
            return m.IsEmitting(sceneMgr, parent);
        }

        public bool IsSmoking(SceneManager sceneMgr, SceneNode parent)
        {
            return IsSmoking(sceneMgr, parent, SmokeType.NORMAL);
        }

        public void NoSmoke(SceneManager sceneMgr, SceneNode parent)
        {
            NoSmoke(sceneMgr, parent, SmokeType.NORMAL);
        }

        public void NoSmoke(SceneManager sceneMgr, SceneNode parent, SmokeType type)
        {
            ParticleManager m = GetSmokeManager(type);
            ParticleSystem e = m.Stop(sceneMgr, parent);
            if (e != null && smokeSystems.ContainsKey(e))
            {
                smokeSystems.Remove(e);
            }
        }

        /// <summary>
        /// Dodaje dowoln� animacj�, kt�ra b�dzie automatycznie odswie�ana podczas gry. Je�li animacja istnia�a zwracana zostaje animacja przechowana w EffectsManager.Effects
        /// </summary>
        /// <param name="animation"></param>
        /// <returns></returns>
        public NodeAnimation.NodeAnimation AddCustomEffect(NodeAnimation.NodeAnimation animation)
        {
            if (EffectExists(animation.Name))
            {
                return effects[animation.Name] as NodeAnimation.NodeAnimation;
            }
            else
            {
                effects.Add(animation.Name, animation);
            }
            return animation;
        }

        /// <summary>
        /// Usuwa zadan� animacj� z kolejki renderowania
        /// </summary>
        /// <param name="animation">Animacja efektu do usuni�cia</param>
        /// <returns>True je�li animacja istnia�a. False w przeciwnym wypadku</returns>
        public bool RemoveAnimation(NodeAnimation.NodeAnimation animation)
        {
            if(effects.ContainsKey(animation.Name))
            {
                effects.Remove(animation.Name);
                return true;
            }
            return false;
        }

        public VisibilityNodeAnimation Sprite(SceneManager sceneMgr, SceneNode parent, Vector3 localPosition,
                                              Vector2 size, EffectType type, bool looped)
        {
            return Sprite(sceneMgr, parent, localPosition, size, type, looped, 0);
        }

        public VisibilityNodeAnimation Sprite(SceneManager sceneMgr, SceneNode parent, EffectType type)
        {
            return Sprite(sceneMgr, parent, Vector3.ZERO, new Vector2(25, 25), type, false, 0);
        }

        public VisibilityNodeAnimation Sprite(SceneManager sceneMgr, SceneNode parent, EffectType type, uint index)
        {
            return Sprite(sceneMgr, parent, Vector3.ZERO, new Vector2(25, 25), type, false, index);
        }

        public VisibilityNodeAnimation Sprite(SceneManager sceneMgr, SceneNode parent, EffectType type, uint index,
                                              bool looped)
        {
            return Sprite(sceneMgr, parent, Vector3.ZERO, new Vector2(25, 25), type, looped, index);
        }

        private static EffectInfo GetEffectInfo(EffectType type)
        {
            EffectInfo info;
            info.type = type;

            switch (type)
            {
                case EffectType.EXPLOSION1:
                    info.duration = 1.0f;
                    info.material = "Effects/Explosion1";
                    break;

                case EffectType.EXPLOSION2:
                    info.duration = 1.0f;
                    info.material = "Effects/Explosion2";
                    break;

                case EffectType.PLANECRASH:
                    info.duration = 1.2f;
                    info.material = "Effects/PlaneCrash";
                    break;

                case EffectType.WATERIMPACT1:
                    info.duration = 1.2f;
                    info.material = "Effects/WaterImpact1";
                    break;

                case EffectType.WATERIMPACT2:
                    info.duration = 0.8f;
                    info.material = "Effects/WaterImpact2";
                    break;

                case EffectType.DIRTIMPACT1:
                    info.duration = 0.5f;
                    info.material = "Effects/DirtImpact1";
                    break;

                case EffectType.DIRTIMPACT2:
                    info.duration = 0.8f;
                    info.material = "Effects/DirtImpact1";
                    break;

                case EffectType.FIRE:
                    info.duration = 1.2f;
                    info.material = "Effects/Fire";
                    break;

                case EffectType.SUBMERGE:
                    info.duration = 5.0f;
                    info.material = "Effects/Submerge";
                    break;

                case EffectType.SEAGULL:
                    info.duration = 0.8f;
                    info.material = "Effects/Seagull";
                    break;

                case EffectType.GUNHIT:
                    info.duration = 0.2f;
                    info.material = "Effects/GunHit";
                    break;

                case EffectType.WATERTRAIL:
                    info.duration = 1.6f;
                    info.material = "Effects/WaterTrail";
                    break;

                case EffectType.PLANEWATERTRAIL:
                    info.duration = 3.0f;
                    info.material = "Effects/PlaneWaterTrail";
                    break;

                case EffectType.BLOOD:
                    info.duration = 0.8f;
                    info.material = "Effects/Blood";
                    break;

                default:
                    info.duration = 0.0f;
                    info.material = "";
                    break;
            }
            return info;
        }

        public void HideSprite(SceneManager sceneMgr, SceneNode parent, EffectType type, uint index)
        {
            string aName = parent.Name + "_" + type.ToString() + "_index" + index; // animation name
            string bsName = aName + "BS"; // billboardset name
            VisibilityNodeAnimation ret;

            if (EffectExists(aName))
            {
                ret = effects[aName] as VisibilityNodeAnimation;
                ret.rewind(false);
                ret.Node.SetVisible(false);
            }
        }

        public void NoSprite(SceneManager sceneMgr, SceneNode parent, EffectType type, uint index)
        {
            string aName = parent.Name + "_" + type.ToString() + "_index" + index; // animation name
            string bsName = aName + "BS"; // billboardset name

            if (EffectExists(aName))
            {
                string material = GetEffectInfo(type).material;
                effects.Remove(aName);

                (parent.GetChild(aName + "Node") as SceneNode).DetachAllObjects();
                sceneMgr.DestroySceneNode(aName + "Node");
                sceneMgr.DestroyBillboardSet(bsName);

                // kasowanie materialu powoduje niestabilno��
                // MaterialManager.Singleton.Remove(material + "_" + aName);
            }
        }


        /// <summary>
        /// Startuje animacje efektow. Ustawia odpowiednie entities i buduje sceneNody.
        /// </summary>
        /// <param name="sceneMgr"></param>
        /// <param name="parent"></param>
        /// <param name="localPosition"></param>
        /// <param name="size"></param>
        /// <param name="type">Typ efektu</param>
        /// <param name="index">Aby mog� by� wi�ciej ni� jeden efekt danego typu dla parenta</param>
        /// <param name="looped">Zapetlenie</param>
        /// <returns>Kontroler animacji steruj�cej widoczno�ci� efektu</returns>
        public VisibilityNodeAnimation Sprite(SceneManager sceneMgr, SceneNode parent, Vector3 localPosition,
                                              Vector2 size, EffectType type, bool looped, uint index)
        {
            EffectInfo info = GetEffectInfo(type);
            string material = info.material;
            Billboard billboard;
            SceneNode node;
            // EXPLOSIONS

            string aName = parent.Name + "_" + type.ToString() + "_index" + index; // animation name
            string bsName = aName + "BS"; // billboardset name
            bool exists = false;
            MaterialPtr clonedMaterial = null;
            VisibilityNodeAnimation ret = null;


            if (EffectExists(aName))
            {
                ret = effects[aName] as VisibilityNodeAnimation;

                // efekt nie moze byc za cz�sto uruchamiany 
                if (Environment.TickCount - ret.LastInitTime < minRepeatDelay)
                {
                    return ret;
                }

                exists = true;
                // rebuild
                billboardSet = sceneMgr.GetBillboardSet(bsName);
                billboard = billboardSet.GetBillboard(0);
                billboard.SetDimensions(size.x, size.y);
                billboard.Position = localPosition;
                node = parent.GetChild(aName + "Node") as SceneNode;
                clonedMaterial = MaterialManager.Singleton.GetByName(material + "_" + aName);
                node.SetVisible(true);
            }
            else
            {
                // nie ma takiego efektu            

                // rozne index'y beda wplywaly na tworzenie roznych billboardsetow. 
                // Jest tak dlatego, ze kontroller animacji operuje na pojedynczym nodzie
                // dlatego DLA KAZDEGO nowego efektu nalezy utworzyc nowy node i podpiac pod niego billboardset
                // jeden billboardset nie moze byc podpiety pod kilka node'�w st�d potrzeba utworzenia
                // nowego: node'a, billboardsetu i billboarda dla kazdego nowego efektu (o roznym indexie/typie)
                billboardSet = sceneMgr.CreateBillboardSet(bsName, 1);
                AxisAlignedBox box =
                    new AxisAlignedBox(new Vector3(-size.x, -size.y, 0), new Vector3(size.x, size.y, 0));
                billboardSet.SetBounds(box, Math.Max(size.x, size.y));
                billboardSet.SetDefaultDimensions(size.x, size.y);


                billboard = billboardSet.CreateBillboard(localPosition);
                billboard.SetDimensions(size.x, size.y);

                node = parent.CreateChildSceneNode(aName + "Node");
                node.AttachObject(billboardSet);

                if (MaterialManager.Singleton.ResourceExists(material + "_" + aName))
                {
                    clonedMaterial = MaterialManager.Singleton.GetByName(material + "_" + aName);
                }
                else
                {
                    clonedMaterial = ViewHelper.CloneMaterial(material, material + "_" + aName);
                }
            }

            TextureUnitState unit = clonedMaterial.GetBestTechnique().GetPass(0).GetTextureUnitState(0);
            billboardSet.MaterialName = clonedMaterial.Name;


            if (exists)
            {
                ret.rewind(true);
            }
            else
            {
                float duration = info.duration;
                if (looped)
                {
                    ret =
                        new EffectTextureAnimation(node, unit, duration, aName,
                                                   VisibilityNodeAnimation.VisibilityType.VISIBLE,
                                                   VisibilityNodeAnimation.VisibilityType.VISIBLE);
                }
                else
                {
                    ret =
                        new EffectTextureAnimation(node, unit, duration, aName,
                                                   VisibilityNodeAnimation.VisibilityType.VISIBLE,
                                                   VisibilityNodeAnimation.VisibilityType.HIDDEN);
                }
                ret.Enabled = true;
                effects[aName] = ret;
            }
            ret.Looped = looped;

            clonedMaterial = null;
            return ret;
        }


        /// <summary>
        /// Tworzy efekt oparty na animowanej teksturze na prostok�cie. W odr�nieniu od metody Sprite() efekt jest 3-wymiarowy, niekoniecznie zwr�cony do kamery
        /// UWAGA: domy�lnie prostok�t ma rozmiary 1 x 1 jednostek.
        /// UWAGA 2: Je�li efekt juz istnia� animacja tekstury powr�ci do 0 klatki.
        /// </summary>
        /// <param name="sceneMgr"></param>
        /// <param name="parent"></param>
        /// <param name="localName"></param>
        /// <param name="localPosition"></param>
        /// <param name="localOrientation"></param>
        /// <param name="looped"></param>
        /// <returns>Kontroler animacji steruj�cej widoczno�ci� efektu</returns>
        public VisibilityNodeAnimation RectangularEffect(SceneManager sceneMgr, SceneNode parent, String localName,
                                                         EffectType type, Vector3 localPosition, Vector2 size,
                                                         Quaternion localOrientation, bool looped)
        {
            EffectInfo info = GetEffectInfo(type);
            string material = info.material;

            string aName = parent.Name + "_" + localName; // animation name
            MaterialPtr cloned;
            SceneNode node;
            Entity entity;
            TextureUnitState unit;
            bool exists = false;

            VisibilityNodeAnimation ret;

            if (EffectExists(aName))
            {
                exists = true;
                ret = effects[aName] as VisibilityNodeAnimation;
                // efekt nie moze byc za cz�sto uruchamiany 
                if (Environment.TickCount - ret.LastInitTime < minRepeatDelay)
                {
                    return ret;
                }
                entity = sceneMgr.GetEntity(aName);
                cloned = MaterialManager.Singleton.GetByName(material + "_" + aName);
            }
            else
            {
                entity = sceneMgr.CreateEntity(aName, "TwoSidedPlane.mesh");
                cloned = ViewHelper.CloneMaterial(material, material + "_" + aName);
            }
            entity.CastShadows = false;

            unit = cloned.GetBestTechnique().GetPass(0).GetTextureUnitState(0);
            unit.CurrentFrame = 0;
            entity.SetMaterialName(cloned.Name);
            if (!exists)
            {
                node = parent.CreateChildSceneNode(aName, localPosition, localOrientation);
                node.AttachObject(entity);
                node.Scale(size.x/5.0f, 0, size.y/5.0f); // twosidedrectangle ma 5x5 metrow
                node.Orientation = localOrientation;
            }
            else
            {
                node = sceneMgr.GetSceneNode(aName);
                node.ResetToInitialState();
                node.Position = localPosition;
                node.Scale(size.x/5.0f, 0, size.y/5.0f);
                node.Orientation = localOrientation;
            }
            //node.ShowBoundingBox = true;

            if (exists)
            {
                ret = effects[aName] as VisibilityNodeAnimation;
                ret.rewind(true);
            }
            else
            {
                float duration = info.duration;
                if (looped)
                {
                    ret =
                        new EffectTextureAnimation(node, unit, duration, aName,
                                                   VisibilityNodeAnimation.VisibilityType.VISIBLE,
                                                   VisibilityNodeAnimation.VisibilityType.VISIBLE);
                }
                else
                {
                    ret =
                        new EffectTextureAnimation(node, unit, duration, aName,
                                                   VisibilityNodeAnimation.VisibilityType.VISIBLE,
                                                   VisibilityNodeAnimation.VisibilityType.HIDDEN);
                }
                ret.Enabled = true;
                effects[aName] = ret;
            }
            ret.Looped = looped;

            // czy to jest ok?
            entity.RenderQueueGroup = (byte) RenderQueueGroupID.RENDER_QUEUE_OVERLAY;

            cloned = null;
            return ret;
        }

        public VisibilityNodeAnimation RectangularEffect(SceneManager sceneMgr, SceneNode parent, String localName,
                                                         EffectType type, Vector3 localPosition,
                                                         Quaternion localOrientation, bool looped)
        {
            return
                RectangularEffect(sceneMgr, parent, localName, type, localPosition, Vector2.UNIT_SCALE, localOrientation,
                                  looped);
        }


        public void WaterImpact(SceneManager sceneMgr, SceneNode parent, Vector3 localPosition, Vector2 size,
                                bool looped)
        {
            SceneNode waterImpact1Node, waterImpact2Node;

            VisibilityNodeAnimation vnAnimation, vnAnimation2;

            vnAnimation =
                RectangularEffect(sceneMgr, parent, "WaterImpact1", EffectType.WATERIMPACT1, localPosition,
                                  Quaternion.IDENTITY, looped);
            vnAnimation2 =
                RectangularEffect(sceneMgr, parent, "WaterImpact2", EffectType.WATERIMPACT1, localPosition,
                                  Quaternion.IDENTITY, looped);

            waterImpact1Node = vnAnimation.Node;
            waterImpact2Node = vnAnimation2.Node;

            waterImpact1Node.Rotate(Vector3.UNIT_X, Mogre.Math.HALF_PI);
            waterImpact1Node.Rotate(Vector3.UNIT_Z, Mogre.Math.HALF_PI/2);
            waterImpact1Node.Scale(20.0f, 20f, 20.5f);

            waterImpact2Node.Rotate(Vector3.UNIT_X, Mogre.Math.HALF_PI);
            waterImpact2Node.Rotate(Vector3.NEGATIVE_UNIT_Z, Mogre.Math.HALF_PI/2);
            waterImpact2Node.Scale(26.5f, 26.5f, 20.5f);
        }


        public void AddSeagulls(SceneManager sceneMgr, Vector3 center, Vector2 defaultSize, Degree maxRotation,
                                float speed, uint count)
        {
            VisibilityNodeAnimation vnAnimation;
            ConstMoveNodeAnimation motion;

            int sizeDevX = (int) (defaultSize.x/10.0f);
            int sizeDevY = (int) (defaultSize.y/10.0f);

            Degree rotationDev = maxRotation/2.0f;
            float tempSpeed = speed;
            int halfCount = (int) Math.Ceiling(count/2.0f);
            for (int i = -halfCount; i < halfCount; i ++)
            {
                Vector3 localPosition =
                    new Vector3(i*200, Mogre.Math.RangeRandom(-100, 100), Mogre.Math.RangeRandom(-100, 100));
                Vector2 size =
                    new Vector2(defaultSize.x + Mogre.Math.RangeRandom(-sizeDevX, sizeDevX),
                                defaultSize.y + Mogre.Math.RangeRandom(-sizeDevY, 0));

                vnAnimation =
                    RectangularEffect(sceneMgr, sceneMgr.RootSceneNode, "Seagull" + i, EffectType.SEAGULL, localPosition,
                                      size, Quaternion.IDENTITY, true);
                vnAnimation.Node.Position += center;
                vnAnimation.TimeScale = Mogre.Math.RangeRandom(0.9f, 1.1f);
                vnAnimation.rewindToRandom();
                vnAnimation.Node.GetAttachedObject(0).RenderQueueGroup =
                    (byte) RenderQueueGroupID.RENDER_QUEUE_SKIES_EARLY;
                vnAnimation.Node.Rotate(Vector3.UNIT_Z,
                                        Mogre.Math.DegreesToRadians(
                                            Mogre.Math.RangeRandom(-rotationDev.ValueDegrees,
                                                                    rotationDev.ValueDegrees)));
                vnAnimation.Node.Rotate(Vector3.UNIT_X, Mogre.Math.HALF_PI);

                if (Mogre.Math.RangeRandom(0.0f, 1.0f) >= 0.5f) // losowy kierunek lotu
                {
                    vnAnimation.Node.Rotate(Vector3.UNIT_Z, Mogre.Math.PI); // w prawo
                    tempSpeed = speed;
                }
                else
                {
                    tempSpeed = -speed;
                }
                // ruch mew
                motion =
                    new ConstMoveNodeAnimation(vnAnimation.Node, 1, tempSpeed, Vector3.UNIT_X, "Seagull" + i + "move");
                motion.Enabled = true;
                motion.onFinish = ChangeSeagullDirection; // mewy zmieniaja czasem kierunek lotu
                motion.onFinishInfo = motion;
                effects[motion.Name] = motion;
            }
        }

        /// <summary>
        /// Zmienia kierunek lotu mewy (delegat)
        /// </summary>
        /// <param name="constMoveNodeAnimation"></param>
        public static void ChangeSeagullDirection(Object constMoveNodeAnimation)
        {
            if (constMoveNodeAnimation is ConstMoveNodeAnimation)
            {
                ConstMoveNodeAnimation a = constMoveNodeAnimation as ConstMoveNodeAnimation;
                if (Mogre.Math.RangeRandom(0.0f, 1.0f) >= 0.95f)
                {
                    a.SwapDirection();
                    a.Node.Rotate(Vector3.UNIT_Z, Mogre.Math.PI);
                }
            }
        }

        public void AddClouds(SceneManager sceneMgr, Vector3 cloudsCenter, Vector2 defaultSize, Degree maxRotation,
                              uint cloudCount)
        {
            if (!sceneMgr.HasBillboardSet("Clouds1"))
            {
                cloudsBS1 = sceneMgr.CreateBillboardSet("Clouds1");
                cloudsBS1.MaterialName = "Effects/Cloud1";
            }

            if (!sceneMgr.HasBillboardSet("Clouds2"))
            {
                cloudsBS2 = sceneMgr.CreateBillboardSet("Clouds2");
                cloudsBS2.MaterialName = "Effects/Cloud2";
            }
            cloudsBS1.RenderQueueGroup = (byte) RenderQueueGroupID.RENDER_QUEUE_SKIES_EARLY;
            cloudsBS2.RenderQueueGroup = (byte) RenderQueueGroupID.RENDER_QUEUE_SKIES_EARLY;

            int sizeDevX = (int) (defaultSize.x/10.0f);
            int sizeDevY = (int) (defaultSize.y/10.0f);

            Degree rotationDev = maxRotation/2.0f;

            if (cloudsBS1.ParentSceneNode != sceneMgr.RootSceneNode) sceneMgr.RootSceneNode.AttachObject(cloudsBS1);
            if (cloudsBS2.ParentSceneNode != sceneMgr.RootSceneNode) sceneMgr.RootSceneNode.AttachObject(cloudsBS2);

            int halfCount = (int) Math.Ceiling(cloudCount/2.0f);
            for (int i = -halfCount; i < halfCount; i += 2)
            {
                Billboard cloud1 = cloudsBS1.CreateBillboard(i*25, Mogre.Math.RangeRandom(-50, 50), 0);
                cloud1.Position += cloudsCenter;
                cloud1.SetDimensions(defaultSize.x + Mogre.Math.RangeRandom(-sizeDevX, sizeDevX),
                                     defaultSize.y + Mogre.Math.RangeRandom(-sizeDevY, 0));
                cloud1.Rotation =
                    Mogre.Math.DegreesToRadians(
                        Mogre.Math.RangeRandom(-rotationDev.ValueDegrees, (float) rotationDev.ValueDegrees));
            }

            halfCount--;

            for (int i = -halfCount; i < halfCount; i += 2)
            {
                Billboard cloud2 = cloudsBS2.CreateBillboard(i*25, Mogre.Math.RangeRandom(-50, 50), 0);
                cloud2.SetDimensions(defaultSize.x + Mogre.Math.RangeRandom(-sizeDevX, sizeDevX),
                                     defaultSize.y + Mogre.Math.RangeRandom(-sizeDevY, sizeDevY));
                cloud2.Position += cloudsCenter;
                cloud2.Rotation =
                    Mogre.Math.DegreesToRadians(
                        Mogre.Math.RangeRandom(-rotationDev.ValueDegrees, rotationDev.ValueDegrees));
            }
        }
    }
}