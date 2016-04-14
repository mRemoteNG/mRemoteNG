using System;
using System.Collections;
using System.Windows.Forms;



namespace mRemoteNG.UI.Window
{
	public class WindowList : CollectionBase
	{
        #region Public Properties
        public BaseWindow this[object Index]
		{
			get
			{
				this.CleanUp();
				if (Index is BaseWindow)
                    return IndexByObject(Index);
				else if (Index is Int32)
                    return IndexByNumber(Convert.ToInt32(Index));
						
				return null;
			}
		}
	
        public new int Count
		{
			get
			{
				this.CleanUp();
				return List.Count;
			}
		}
        #endregion
		
        #region Public Methods
		public void Add(BaseWindow uiWindow)
		{
			this.List.Add(uiWindow);
			//AddHandler uiWindow.FormClosing, AddressOf uiFormClosing
		}
				
		public void AddRange(BaseWindow[] uiWindow)
		{
			foreach (Form uW in uiWindow)
			{
				this.List.Add(uW);
			}
		}
				
		public void Remove(BaseWindow uiWindow)
		{
			this.List.Remove(uiWindow);
		}
				
		public BaseWindow FromString(string uiWindow)
		{
			this.CleanUp();
			for (int i = 0; i < this.List.Count; i++)
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
			for (int i = 0; i <= this.List.Count - 1; i++)
			{
				if (i > this.List.Count - 1)
				{
					CleanUp();
					return;
				}
				if ((this.List[i] as BaseWindow).IsDisposed)
				{
					this.List.RemoveAt(i);
					CleanUp();
					return;
				}
			}
		}

        private BaseWindow IndexByObject(object Index)
        {
            try
            {
                int objectIndex = this.List.IndexOf(Index);
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
                return this.List[Index] as BaseWindow;
            }
            catch (ArgumentOutOfRangeException e)
            {
                throw new ArgumentOutOfRangeException(e.ParamName, e.ActualValue, "Index was out of bounds");
            }
        }

		private void uiFormClosing(object sender, FormClosingEventArgs e)
		{
			this.List.Remove(sender);
		}
	}
}