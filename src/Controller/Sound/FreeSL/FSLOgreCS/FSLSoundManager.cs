using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using Mogre;
using Wof.Controller;

namespace FSLOgreCS
{
    public abstract class FSLSoundManager
    {
        #region Variables

        private List<FSLSoundObject> _soundObjectVector = new List<FSLSoundObject>();
        private bool _initSound;
        private FSLListener _listener;

        private object lockSync = new object();

        protected Thread updaterThread;

        protected bool updaterRunning = false;

        public bool UpdaterRunning
        {
            get { return updaterRunning; }
            set {

                lock (LockSync)
                {
                    this.updaterRunning = value;
                }

            }
        }

        protected bool killUpdater = false;

        #endregion

        #region Singleton Stuff

        protected FSLSoundManager() // changed private -> protected
        {
            _initSound = false;
            _listener = null;
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
       /* public static FSLSoundManager Instance
        {
            get { return SingletonCreator.instance; }
        }

        private class SingletonCreator
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static SingletonCreator()
            {
            }

            internal static readonly FSLSoundManager instance = new FSLSoundManager();
        }*/

        #endregion

        #region Private Members

        private FSLSoundObject AddSound(FSLSoundObject sound)
        {
            lock(this)
            {
                SoundObjectVector.Add(sound);
                return sound;
            }
           
        }

        #endregion

        public void ErrorCallback(string s, bool b)
        {
            //Console.WriteLine("Error: " + s);   
        }
        public bool InitializeSound(CameraListenerBase listener, FreeSL.FSL_SOUND_SYSTEM soundSystem)
        {
            lock (LockSync)
            {
               
                _listener = new FSLListener(listener);
                if (_initSound)
                    return true;

                if (!FreeSL.fslInit(soundSystem)) //Change if desire
                    return false;

                _initSound = true;

                /*FreeSL.ErrorCallbackDelegate ErrorDelegate = new FreeSL.ErrorCallbackDelegate(ErrorCallback);
                GCHandle AllocatedDelegate = GCHandle.Alloc(ErrorDelegate);
                FreeSL.fslSetErrorCallback(ErrorDelegate);*/
              
               
                updaterThread = new Thread(new ThreadStart((UpdateSoundObjects)));
                updaterThread.Name = "Wof - sound updater thread " + updaterThread.GetHashCode();
                updaterThread.Start();
                return true;
                
            }
           
        }

        public void SetUserPlane(Wof.Model.Level.Planes.Plane plane)
        {
            SetListener(_listener.Listener, plane);
        }

        public void ShutDown()
        {
            lock (LockSync)
            {
                FreeSL.fslShutDown();
                _initSound = false;
            }
           // this.updaterThread = new Thread(new ThreadStart(UpdateSoundObjects));
           
        }

        public float Volume
        {
            set { FreeSL.fslSetVolume(value); }
        }

        public bool Initialized
        {
            get { return _initSound; }
        }

        public List<FSLSoundObject> SoundObjectVector
        {
            get { return _soundObjectVector; }
        }

        public object LockSync
        {
            get { return lockSync; }
        }

        public void RemoveSound(string name)
        {
            lock (LockSync)
            {
                FSLSoundObject sound = GetSound(name);
                if (sound == null)
                    return;
                else
                {
                    SoundObjectVector.Remove(sound); // zmiana
                    sound.Destroy();
                    sound = null;
                }
            }

        }

        public FSLSoundObject GetSound(string name)
        {
           // lock (LockSync)
            {
                if (SoundObjectVector.Count == 0)
                    return null;

                for (int i = 0; i < SoundObjectVector.Count; i++)
                {
                    if (SoundObjectVector[i].Name == name) return SoundObjectVector[i];
                }
            }
            return null;
        }

        public void UpdateSoundObjects()
        {
        	int k = 0;
            while(true)
            {
            	 try
                 {
                        lock (LockSync)
		                {
		                   
		                    if (killUpdater)
		                    {
		                        updaterRunning = false;
		                        killUpdater = false;
		                        return;
		                    }
		                }

                        lock (LockSync)
                        {
                            if (updaterRunning)
	                        {
	
	                            //updaterRunning = true;
	                            if (!_initSound)
	                                return;
	                            _listener.Update();
	
	                            try
	                            {
	                                for (int i = 0; i < SoundObjectVector.Count; i++)
	                                {
	                                    if (SoundObjectVector[i] != null)
	                                    {
	                                        SoundObjectVector[i].Update();
	                                    }
	                                }
	                            }
	                            catch(Exception e)
	                            {
	                            	//LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Error in sound loop:"+ e.Message+ e.StackTrace);
	                              
	
	                            }
	                            try
	                            {
                                    if (SoundObjectVector.Count > 0)
                                    {
                                        FreeSL.fslUpdate();
                                    }
                                    
	                            }
	                            catch (Exception)
	                            {
                                   // LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "YEAH THATS RIGHT");
	                               // throw;
	                            }
	                            
	
	                            // Console.WriteLine("Running");

	                        }
                           
		                }
                        FreeSL.fslSleep(0.01f);
                       /* k++;
                        if(k == 2000) {
                        	k = 0;
                        	LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "SLEEEEEEP");
                        	Thread.Sleep(10000);
                        }
	                  */
	                    
                }                
            	catch (Exception ex)
                {
            	//	LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Exception in sound updater: "+ex);
                }
                 
               
              
            }
            
           
        }

        public void SetListener(CameraListenerBase listener)
        {
            _listener = new FSLListener(listener);
        }
        public void SetListener(CameraListenerBase listener, Wof.Model.Level.Planes.Plane plane)
        {
            _listener = new FSLListener(listener, plane);
        }

        public FSLListener GetListener()
        {
            return _listener;
        }



        public FSLAmbientSoundMusic CreateAmbientSoundMusic(string musicFile, string name, bool loop, bool streaming)
        {
          //  lock (LockSync)
            {
                int before = Environment.TickCount;
               FSLAmbientSoundMusic music =  new FSLAmbientSoundMusic(musicFile, name, loop, streaming);
                music.IsOnPlaylist = true;
                AddSound(music);
                if(!streaming && LogManager.Singleton != null)
                {
                    LogManager.Singleton.LogMessage(LogMessageLevel.LML_NORMAL, "Ambient sound music '"+name+"' preloaded within "+ (Environment.TickCount - before)+ "ms");
                }
                return music;
            }
        }
        public FSLSoundObject CreateAmbientSound(string soundFile, string name, bool loop, bool streaming)
        {
         //   lock (LockSync)
            {
                int before = Environment.TickCount;
                FSLSoundObject obj = AddSound(new FSLAmbientSound(soundFile, name, loop, streaming));
                if(!streaming && LogManager.Singleton != null)
                {
                    LogManager.Singleton.LogMessage(LogMessageLevel.LML_NORMAL, "Ambient sound '" + name + "' preloaded within " + (Environment.TickCount - before) + "ms");
                }
                return obj;
            }
        }

        public FSLSoundObject CreateSoundEntity(string soundFile, SceneNode renderable, string name, bool loop,
                                                bool streaming)
        {
           //  lock (LockSync)
             {
                 return AddSound(new FSLSoundEntity(soundFile, renderable, name, loop, streaming));
             }
        }

      
        public FSLSoundObject CreateSoundEntity(string package, string soundFile, SceneNode renderable, string name,
                                                bool loop)
        {
          //  lock (LockSync)
            {
                return AddSound(new FSLSoundEntity(package, soundFile, renderable, name, loop));
            }
            
        }

        public bool FrameStarted(FrameEvent evt)
        {
          /*  lock (this)
            {
                updaterRunning = true;
            }*/
            return true;
        }

        public virtual void Destroy()
        {
            lock (LockSync)
            {
                updaterRunning = false;
                killUpdater = true;

            }
           
            if(updaterThread != null)
            {
                while (updaterThread.ThreadState != ThreadState.Stopped && updaterThread.ThreadState != ThreadState.Unstarted)
                {
                    Thread.Sleep(100);
                }
                
            }
           

            
           
            killUpdater = false;
            updaterThread = new Thread(UpdateSoundObjects);

            if (SoundObjectVector.Count != 0)
            {

                for (int i = 0; i < SoundObjectVector.Count; i++)
                {
                    LogManager.Singleton.LogMessage(LogMessageLevel.LML_NORMAL, "Disposing sound object: " + SoundObjectVector[i].Name);
                  
                    SoundObjectVector[i].Destroy();
                }
                SoundObjectVector.Clear();
            }
            if (_listener != null)
                _listener = null;
            if (_initSound)
                ShutDown();
            _initSound = false;
            
        }

        #region Environment Functions

        /// <summary>
        /// Sets the listener environment.
        /// </summary>
        /// <param name="prop">The prop.</param>
        public void SetListenerEnvironment(FreeSL.FSL_EAX_LISTENER_PROPERTIES prop)
        {
            FreeSL.fslSetListenerEnvironment(prop);
        }

        /// <summary>
        /// Sets the listener environment preset.
        /// </summary>
        /// <param name="type">The type.</param>
        public void SetListenerEnvironmentPreset(FreeSL.FSL_LISTENER_ENVIRONMENT type)
        {
            FreeSL.fslSetListenerEnvironmentPreset(type);
        }

        /// <summary>
        /// Sets the listener default environment.
        /// </summary>
        public void SetListenerDefaultEnvironment()
        {
            FreeSL.fslSetListenerDefaultEnvironment();
        }

        /// <summary>
        /// Gets the current listener environment.
        /// </summary>
        /// <returns></returns>
        public FreeSL.FSL_EAX_LISTENER_PROPERTIES GetCurrentListenerEnvironment()
        {
            return FreeSL.fslGetCurrentListenerEnvironment();
        }

        /// <summary>
        /// Loads the listener environment.
        /// </summary>
        /// <param name="strFile">The STR file.</param>
        /// <returns></returns>
        public FreeSL.FSL_EAX_LISTENER_PROPERTIES LoadListenerEnvironment(string strFile)
        {
            return FreeSL.fslLoadListenerEnvironment(strFile);
        }

        /// <summary>
        /// Loads the listener environment from zip.
        /// </summary>
        /// <param name="strFile">The STR file.</param>
        /// <param name="strPackage">The STR package.</param>
        /// <returns></returns>
        public FreeSL.FSL_EAX_LISTENER_PROPERTIES LoadListenerEnvironmentFromZip(string strFile, string strPackage)
        {
            return FreeSL.fslLoadListenerEnvironmentFromZip(strPackage, strFile);
        }

        /// <summary>
        /// Creates the listener environment.
        /// </summary>
        /// <param name="strData">The STR data.</param>
        /// <param name="Size">The size.</param>
        /// <returns></returns>
        public FreeSL.FSL_EAX_LISTENER_PROPERTIES CreateListenerEnvironment(string strData, uint Size)
        {
            return FreeSL.fslCreateListenerEnvironment(strData, Size);
        }

        #endregion
    }
}