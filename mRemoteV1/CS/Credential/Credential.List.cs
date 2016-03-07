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


namespace mRemoteNG.Credential
{
	public class List : CollectionBase
	{
        #region Public Properties
        public Credential.Info this[object Index]
		{
			get
			{
				if (Index is Credential.Info)
				{
                    return (Credential.Info)Index;
				}
				else
				{
					return ((Credential.Info) (List[System.Convert.ToInt32(Index)]));
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
		public Credential.Info Add(Credential.Info cInfo)
		{
			List.Add(cInfo);
			return cInfo;
		}
			
		public void AddRange(Credential.Info[] cInfo)
		{
			foreach (Credential.Info cI in cInfo)
			{
				List.Add(cI);
			}
		}
			
		public Credential.List Copy()
		{
			try
			{
                return (Credential.List)this.MemberwiseClone();
			}
			catch (Exception)
			{
			}
				
			return null;
		}
			
		public new void Clear()
		{
			List.Clear();
		}
        #endregion
	}
}
