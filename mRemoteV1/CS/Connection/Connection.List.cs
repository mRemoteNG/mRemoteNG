// VBConversions Note: VB project level imports
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
// End of VB project level imports


namespace mRemoteNG.Connection
{
	public class List : CollectionBase
	{
			
#region Public Properties
public Connection.Info this[object Index]
		{
			get
			{
				if (Index is Connection.Info)
				{
					return Index;
				}
				else
				{
					return ((Connection.Info) (List[System.Convert.ToInt32(Index)]));
				}
			}
		}
			
public new int Count
		{
			get
			{
				return List.Count;
			}
		}
#endregion
			
#region Public Methods
		public Connection.Info Add(Connection.Info cInfo)
		{
			this.List.Add(cInfo);
			return cInfo;
		}
			
		public void AddRange(Connection.Info[] cInfo)
		{
			foreach (Connection.Info cI in cInfo)
			{
				List.Add(cI);
			}
		}
			
		public Connection.Info FindByConstantID(string id)
		{
			foreach (Connection.Info conI in List)
			{
				if (conI.ConstantID == id)
				{
					return conI;
				}
			}
				
			return null;
		}
			
		//Public Function Find(ByVal cInfo As Connection.Info)
		//    For Each cI As Connection.Info In List
			
		//    Next
		//End Function
			
		public Connection.List Copy()
		{
			try
			{
				return this.MemberwiseClone();
			}
			catch (Exception)
			{
			}
				
			return null;
		}
			
		public new void Clear()
		{
			this.List.Clear();
		}
#endregion
	}
}
