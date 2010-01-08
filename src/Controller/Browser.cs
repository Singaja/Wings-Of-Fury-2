﻿/*
 * WiiMote - Zastosowanie zaawansowanych kontrolerów gier do stworzenia naturalnych
interfejsów użytkownika.
*/
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using Mogre;
using MOIS;
using Wof.Controller.Screens;

namespace Wof.Controller
{
	/// <summary>
	/// Description of Browser.
	/// </summary>
	public partial class Browser : Form
	{
		private Point mousePos = new Point(0, 0);
		private bool eventsWired = false;

		protected bool canActivate = false;
	    private bool isActivated = false;		
		protected bool mouseOver = true;

       
        protected Vector2 origin;
        protected Vector2 dimensions;

		
		public bool MouseOver {
			get { return mouseOver; }
		}
	    protected Point lastScreenPos = new Point(-1,-1);
	    
	    protected bool isInitialState = true;
	    
	
	    public bool IsInitialState {
			get { return isInitialState; }
		}

	    public bool IsActivated
	    {
	        get 
	        { 
	        	return isActivated;
	        }
	        set 
	        {
                if(value)
                {
                    mouseOver = true;                   
                }
                else
                {
                    mouseOver = false;        
                }               
                isActivated = value;            
            }
	    }

	    public Point MousePos
	    {
	        get { return mousePos; }
	    }

	    public bool IsMouseOver()
        {           
	        return mouseOver;    
        }	

	    public bool IsMouseOver(Point screen)
		{            
            Point client = this.wofBrowser.PointToClient(screen);
            lastScreenPos = screen;         
            return this.wofBrowser.Bounds.Contains(client);
		}		
		
	    private Form gameForm;
		
		public Browser(Form gameForm, AbstractScreen currentScreen)
		{

            Vector2 m = currentScreen.GetMargin();

            origin = currentScreen.ViewportToScreen(new Vector2(m.x + currentScreen.Viewport.ActualWidth * 0.51f, (int)(m.y)));
            dimensions = currentScreen.ViewportToScreen(new Vector2(currentScreen.Viewport.ActualWidth * 0.48f, (int)currentScreen.Viewport.ActualHeight - m.y - currentScreen.GetTextVSpacing()));


            this.gameForm = gameForm;			
			InitializeComponent();
            this.wofBrowser.Url = new System.Uri(EngineConfig.C_WOF_HOME_PAGE, System.UriKind.Absolute);
		}


        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // turn on WS_EX_TOOLWINDOW style bit
                cp.ExStyle |= 0x80;
                return cp;
            }
        }

        public void SetPosition()
        {
          
            SetBounds((int)(origin.x), (int)(origin.y), (int)dimensions.x, (int)dimensions.y);
        }



	    private void Document_MouseLeave(object sender, HtmlElementEventArgs e)
	    {
	        LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "browser LEAVE");
	        mouseOver = false;           
	    }

	    private void Document_MouseOver(object sender, HtmlElementEventArgs e)
	    {
            mouseOver = true;
	        mousePos = e.MousePosition;         
	    }

       

	    void Document_MouseMove(object sender, HtmlElementEventArgs e)
        {
	        mousePos = e.MousePosition;  
	    }

	   
	
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
        	
        	switch(wofBrowser.Document.Url.LocalPath)
        	{
        		case  "/navcancl.htm":
        			
        		case "/dnserrordiagoff_webOC.htm":        			
        			wofBrowser.DocumentText = File.ReadAllText("none.dat");
        			return;
        		break;                       			  	
        	}
        		
   
    
          //  LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "document loaded");
            if(!wofBrowser.ReadyState.Equals(WebBrowserReadyState.Complete))
            {
                return;
            }
           
            //  this.wofBrowser.
            this.wofBrowser.Document.MouseMove += new HtmlElementEventHandler(Document_MouseMove);
            this.wofBrowser.Document.MouseOver += new HtmlElementEventHandler(Document_MouseOver);
            this.wofBrowser.Document.MouseLeave += new HtmlElementEventHandler(Document_MouseLeave);
            this.wofBrowser.Document.Click += new HtmlElementEventHandler(Document_Click);

            this.wofBrowser.Navigating += wofBrowser_Navigating;
            eventsWired = true;
           
        }

        void wofBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (eventsWired)
            {
                this.wofBrowser.Navigating -= wofBrowser_Navigating;
                eventsWired = false;
            }
           
            if (isInitialState)
            {
                this.WindowState = FormWindowState.Maximized;
                this.FormBorderStyle = FormBorderStyle.Fixed3D;
                // this.Activate();
            }

            isInitialState = false;
        }

        void Document_Click(object sender, HtmlElementEventArgs e)
        {
            
        }

        void element_Click(object sender, HtmlElementEventArgs e)
        {
           
        }

	   

	    public new void Activate()
        {
        	canActivate = true;
        	base.Activate();
	        base.Focus();
        }

        
        private void Browser_Activated(object sender, EventArgs e)
        {
        	if(!canActivate)
        	{
        		gameForm.Activate();
        		return;
        	}
           // Console.WriteLine(" !!!!!!!!!!!!!! ACTIVATED !!!!!!!!!!!!!!");
            if(lastScreenPos.X >= 0)
            {
                Cursor.Position = lastScreenPos;
            }     
            canActivate = false;            
        }

        public new void Hide()
        {
        	isInitialState = false;
        	base.Hide();
        }
		
	
		
        public void ReturnToInitialState()
        {
            if(!isInitialState)
            {
             
            	this.Visible = true;
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.None;
                SetPosition();
                if (eventsWired)
                {
                    this.wofBrowser.Navigating -= wofBrowser_Navigating;
                    eventsWired = false;
                }
                this.wofBrowser.Navigate(new Uri(EngineConfig.C_WOF_NEWS_PAGE, UriKind.Absolute));
            }
            isInitialState = true;
        }
		void BrowserFormClosing(object sender, FormClosingEventArgs e)
		{
		    ReturnToInitialState();
			e.Cancel = true;
			

		}
		
		void WofBrowserNavigated(object sender, WebBrowserNavigatedEventArgs e)
		{
			
		}
	}
}