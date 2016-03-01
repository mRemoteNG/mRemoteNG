using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic.Devices;

// This file was created by the VB to C# converter (SharpDevelop 4.4.2.9749).
// It contains classes for supporting the VB "My" namespace in C#.
// If the VB application does not use the "My" namespace, or if you removed the usage
// after the conversion to C#, you can delete this file.

namespace mRemoteNG.My
{
	sealed partial class MyProject
	{
		[ThreadStatic] static MyApplication application;
		
		public static MyApplication Application {
			[DebuggerStepThrough]
			get {
				if (application == null)
					application = new MyApplication();
				return application;
			}
		}
		
		[ThreadStatic] static MyComputer computer;
		
		public static MyComputer Computer {
			[DebuggerStepThrough]
			get {
				if (computer == null)
					computer = new MyComputer();
				return computer;
			}
		}
		
		[ThreadStatic] static User user;
		
		public static User User {
			[DebuggerStepThrough]
			get {
				if (user == null)
					user = new User();
				return user;
			}
		}
		
		[ThreadStatic] static MyForms forms;
		
		public static MyForms Forms {
			[DebuggerStepThrough]
			get {
				if (forms == null)
					forms = new MyForms();
				return forms;
			}
		}
		
		internal sealed class MyForms
		{
			global::mRemoteNG.UI.Window.About About_instance;
			bool About_isCreating;
			public global::mRemoteNG.UI.Window.About About {
				[DebuggerStepThrough] get { return GetForm(ref About_instance, ref About_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref About_instance, value); }
			}
			
			global::mRemoteNG.UI.Window.ActiveDirectoryImport ActiveDirectoryImport_instance;
			bool ActiveDirectoryImport_isCreating;
			public global::mRemoteNG.UI.Window.ActiveDirectoryImport ActiveDirectoryImport {
				[DebuggerStepThrough] get { return GetForm(ref ActiveDirectoryImport_instance, ref ActiveDirectoryImport_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref ActiveDirectoryImport_instance, value); }
			}
			
			global::mRemoteNG.UI.Window.Announcement Announcement_instance;
			bool Announcement_isCreating;
			public global::mRemoteNG.UI.Window.Announcement Announcement {
				[DebuggerStepThrough] get { return GetForm(ref Announcement_instance, ref Announcement_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref Announcement_instance, value); }
			}
			
			global::mRemoteNG.Forms.OptionsForm OptionsForm_instance;
			bool OptionsForm_isCreating;
			public global::mRemoteNG.Forms.OptionsForm OptionsForm {
				[DebuggerStepThrough] get { return GetForm(ref OptionsForm_instance, ref OptionsForm_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref OptionsForm_instance, value); }
			}
			
			global::mRemoteNG.UI.Window.Base Base_instance;
			bool Base_isCreating;
			public global::mRemoteNG.UI.Window.Base Base {
				[DebuggerStepThrough] get { return GetForm(ref Base_instance, ref Base_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref Base_instance, value); }
			}
			
			global::mRemoteNG.UI.Window.ComponentsCheck ComponentsCheck_instance;
			bool ComponentsCheck_isCreating;
			public global::mRemoteNG.UI.Window.ComponentsCheck ComponentsCheck {
				[DebuggerStepThrough] get { return GetForm(ref ComponentsCheck_instance, ref ComponentsCheck_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref ComponentsCheck_instance, value); }
			}
			
			global::mRemoteNG.UI.Window.Config Config_instance;
			bool Config_isCreating;
			public global::mRemoteNG.UI.Window.Config Config {
				[DebuggerStepThrough] get { return GetForm(ref Config_instance, ref Config_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref Config_instance, value); }
			}
			
			global::mRemoteNG.UI.Window.Connection Connection_instance;
			bool Connection_isCreating;
			public global::mRemoteNG.UI.Window.Connection Connection {
				[DebuggerStepThrough] get { return GetForm(ref Connection_instance, ref Connection_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref Connection_instance, value); }
			}
			
			global::mRemoteNG.UI.Window.ErrorsAndInfos ErrorsAndInfos_instance;
			bool ErrorsAndInfos_isCreating;
			public global::mRemoteNG.UI.Window.ErrorsAndInfos ErrorsAndInfos {
				[DebuggerStepThrough] get { return GetForm(ref ErrorsAndInfos_instance, ref ErrorsAndInfos_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref ErrorsAndInfos_instance, value); }
			}
			
			global::mRemoteNG.Forms.ExportForm ExportForm_instance;
			bool ExportForm_isCreating;
			public global::mRemoteNG.Forms.ExportForm ExportForm {
				[DebuggerStepThrough] get { return GetForm(ref ExportForm_instance, ref ExportForm_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref ExportForm_instance, value); }
			}
			
			global::mRemoteNG.UI.Window.ExternalTools ExternalTools_instance;
			bool ExternalTools_isCreating;
			public global::mRemoteNG.UI.Window.ExternalTools ExternalTools {
				[DebuggerStepThrough] get { return GetForm(ref ExternalTools_instance, ref ExternalTools_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref ExternalTools_instance, value); }
			}
			
			global::mRemoteNG.frmMain frmMain_instance;
			bool frmMain_isCreating;
			public global::mRemoteNG.frmMain frmMain {
				[DebuggerStepThrough] get { return GetForm(ref frmMain_instance, ref frmMain_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref frmMain_instance, value); }
			}
			
			global::mRemoteNG.Forms.PasswordForm PasswordForm_instance;
			bool PasswordForm_isCreating;
			public global::mRemoteNG.Forms.PasswordForm PasswordForm {
				[DebuggerStepThrough] get { return GetForm(ref PasswordForm_instance, ref PasswordForm_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref PasswordForm_instance, value); }
			}
			
			global::mRemoteNG.UI.Window.Help Help_instance;
			bool Help_isCreating;
			public global::mRemoteNG.UI.Window.Help Help {
				[DebuggerStepThrough] get { return GetForm(ref Help_instance, ref Help_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref Help_instance, value); }
			}
			
			global::mRemoteNG.UI.Window.PortScan PortScan_instance;
			bool PortScan_isCreating;
			public global::mRemoteNG.UI.Window.PortScan PortScan {
				[DebuggerStepThrough] get { return GetForm(ref PortScan_instance, ref PortScan_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref PortScan_instance, value); }
			}
			
			global::mRemoteNG.UI.Window.ScreenshotManager ScreenshotManager_instance;
			bool ScreenshotManager_isCreating;
			public global::mRemoteNG.UI.Window.ScreenshotManager ScreenshotManager {
				[DebuggerStepThrough] get { return GetForm(ref ScreenshotManager_instance, ref ScreenshotManager_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref ScreenshotManager_instance, value); }
			}
			
			global::mRemoteNG.UI.Window.Sessions Sessions_instance;
			bool Sessions_isCreating;
			public global::mRemoteNG.UI.Window.Sessions Sessions {
				[DebuggerStepThrough] get { return GetForm(ref Sessions_instance, ref Sessions_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref Sessions_instance, value); }
			}
			
			global::mRemoteNG.UI.Window.SSHTransfer SSHTransfer_instance;
			bool SSHTransfer_isCreating;
			public global::mRemoteNG.UI.Window.SSHTransfer SSHTransfer {
				[DebuggerStepThrough] get { return GetForm(ref SSHTransfer_instance, ref SSHTransfer_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref SSHTransfer_instance, value); }
			}
			
			global::mRemoteNG.UI.Window.Tree Tree_instance;
			bool Tree_isCreating;
			public global::mRemoteNG.UI.Window.Tree Tree {
				[DebuggerStepThrough] get { return GetForm(ref Tree_instance, ref Tree_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref Tree_instance, value); }
			}
			
			global::mRemoteNG.UI.Window.UltraVNCSC UltraVNCSC_instance;
			bool UltraVNCSC_isCreating;
			public global::mRemoteNG.UI.Window.UltraVNCSC UltraVNCSC {
				[DebuggerStepThrough] get { return GetForm(ref UltraVNCSC_instance, ref UltraVNCSC_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref UltraVNCSC_instance, value); }
			}
			
			global::mRemoteNG.UI.Window.Update Update_instance;
			bool Update_isCreating;
			public global::mRemoteNG.UI.Window.Update Update {
				[DebuggerStepThrough] get { return GetForm(ref Update_instance, ref Update_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref Update_instance, value); }
			}
			
			global::mRemoteNG.frmChoosePanel frmChoosePanel_instance;
			bool frmChoosePanel_isCreating;
			public global::mRemoteNG.frmChoosePanel frmChoosePanel {
				[DebuggerStepThrough] get { return GetForm(ref frmChoosePanel_instance, ref frmChoosePanel_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref frmChoosePanel_instance, value); }
			}
			
			[DebuggerStepThrough]
			static T GetForm<T>(ref T instance, ref bool isCreating) where T : Form, new()
			{
				if (instance == null || instance.IsDisposed) {
					if (isCreating) {
						throw new InvalidOperationException(Utils.GetResourceString("WinForms_RecursiveFormCreate", new string[0]));
					}
					isCreating = true;
					try {
						instance = new T();
					} catch (System.Reflection.TargetInvocationException ex) {
						throw new InvalidOperationException(Utils.GetResourceString("WinForms_SeeInnerException", new string[] { ex.InnerException.Message }), ex.InnerException);
					} finally {
						isCreating = false;
					}
				}
				return instance;
			}
			
			[DebuggerStepThrough]
			static void SetForm<T>(ref T instance, T value) where T : Form
			{
				if (instance != value) {
					if (value == null) {
						instance.Dispose();
						instance = null;
					} else {
						throw new ArgumentException("Property can only be set to null");
					}
				}
			}
		}
	}
	
	partial class MyApplication : WindowsFormsApplicationBase
	{
		[STAThread]
		public static void Main(string[] args)
		{
			Application.SetCompatibleTextRenderingDefault(UseCompatibleTextRendering);
			MyProject.Application.Run(args);
		}
	}
	
	partial class MyComputer : Computer
	{
	}
}
