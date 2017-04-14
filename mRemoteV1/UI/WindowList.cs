using System;
using System.Collections;
using mRemoteNG.UI.Window;

namespace mRemoteNG.UI
{
	public class WindowList : CollectionBase
	{
        #region Public Properties
        public BaseWindow this[object Index]
		{
			get
			{
				CleanUp();
				if (Index is BaseWindow)
                    return IndexByObject(Index);
			    if (Index is int)
			        return IndexByNumber(Convert.ToInt32(Index));

                return null;
			}
		}
	
        public new int Count
		{
			get
			{
				CleanUp();
				return List.Count;
			}
		}
        #endregion
		
        #region Public Methods
		public void Add(BaseWindow uiWindow)
		{
			List.Add(uiWindow);
			//AddHandler uiWindow.FormClosing, AddressOf uiFormClosing
		}
				
		public void AddRange(BaseWindow[] uiWindow)
		{
			foreach (var uW in uiWindow)
			{
				List.Add(uW);
			}
		}
				
		public void Remove(BaseWindow uiWindow)
		{
			List.Remove(uiWindow);
		}
				
		public BaseWindow FromString(string uiWindow)
		{
			CleanUp();
			for (var i = 0; i < List.Count; i++)
			{
				if (this[i].Text == uiWindow.Replace("&", "&&"))
				{
					return this[i];
				}
			}
					
			return null;
		}
        #endregion
		

		private void CleanUp()
		{
			for (var i = 0; i <= List.Count - 1; i++)
			{
				if (i > List.Count - 1)
				{
					CleanUp();
					return;
				}
			    var baseWindow = List[i] as BaseWindow;
			    if (baseWindow != null && !baseWindow.IsDisposed) continue;
			    List.RemoveAt(i);
			    CleanUp();
			    return;
			}
		}

        private BaseWindow IndexByObject(object Index)
        {
            try
            {
                var objectIndex = List.IndexOf(Index);
                return IndexByNumber(objectIndex);
            }
            catch (ArgumentOutOfRangeException e)
            {
                throw new ArgumentOutOfRangeException(e.ParamName, "Object was not present in the collection.");
            }
        }

        private BaseWindow IndexByNumber(int Index)
        {
            try
            {
                return List[Index] as BaseWindow;
            }
            catch (ArgumentOutOfRangeException e)
            {
                throw new ArgumentOutOfRangeException(e.ParamName, e.ActualValue, "Index was out of bounds");
            }
        }
        
        /*
		private void uiFormClosing(object sender, FormClosingEventArgs e)
		{
			List.Remove(sender);
		}
        */
	}
}