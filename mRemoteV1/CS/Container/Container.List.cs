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


namespace mRemoteNG.Container
{
	public class List : CollectionBase
		{
			
#region Public Properties
public Container.Info this[object Index]
			{
				get
				{
					if (Index is Container.Info)
					{
						return Index;
					}
					else
					{
						return ((Container.Info) (List[System.Convert.ToInt32(Index)]));
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
			public Container.Info Add(Container.Info cInfo)
			{
				this.List.Add(cInfo);
				return cInfo;
			}
			
			public void AddRange(Container.Info[] cInfo)
			{
				foreach (Container.Info cI in cInfo)
				{
					List.Add(cI);
				}
			}
			
			public Container.Info FindByConstantID(string id)
			{
				foreach (Container.Info contI in List)
				{
					if (contI.ConnectionInfo.ConstantID == id)
					{
						return contI;
					}
				}
				
				return null;
			}
			
			public Container.List Copy()
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
