using System.Collections.Generic;
using System;
using AxWFICALib;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using AxMSTSCLib;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;


namespace mRemoteNG.UI.Window
{
	public class Base : DockContent
	{
		public Base()
		{
			//InitializeComponent();
					
			//Added to support default instance behavour in C#
			if (defaultInstance == null)
				defaultInstance = this;
		}
				
        #region Default Instance
		private static Base defaultInstance;
				
		/// <summary>
		/// Added by the VB.Net to C# Converter to support default instance behavour in C#
		/// </summary>
        public static Base Default
		{
			get
			{
				if (defaultInstance == null)
				{
					defaultInstance = new Base();
					defaultInstance.FormClosed += new FormClosedEventHandler(defaultInstance_FormClosed);
				}
						
				return defaultInstance;
			}
			set
			{
				defaultInstance = value;
			}
		}
				
		static void defaultInstance_FormClosed(object sender, FormClosedEventArgs e)
		{
			defaultInstance = null;
		}	
        #endregion
				
        #region Public Properties
		private Type _WindowType;
        public Type WindowType
		{
			get
			{
				return this._WindowType;
			}
			set
			{
				this._WindowType = value;
			}
		}
				
		private DockContent _DockPnl;
        public DockContent DockPnl
		{
			get
			{
				return this._DockPnl;
			}
			set
			{
				this._DockPnl = value;
			}
		}
        #endregion
				
        #region Public Methods
		public void SetFormText(string Text)
		{
			this.Text = Text;
			this.TabText = Text;
		}
        #endregion
				
        #region Private Methods
		private void Base_Load(System.Object sender, System.EventArgs e)
		{
			frmMain.Default.ShowHidePanelTabs();
		}
				
		private void Base_FormClosed(System.Object sender, System.Windows.Forms.FormClosedEventArgs e)
		{
			frmMain.Default.ShowHidePanelTabs(this);
		}
        #endregion
	}
}
