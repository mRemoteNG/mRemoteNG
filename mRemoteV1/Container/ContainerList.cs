using System;
using System.Collections;


namespace mRemoteNG.Container
{
	public class ContainerList : CollectionBase
	{
        public ContainerInfo this[object Index]
		{
			get
			{
				if (Index is ContainerInfo)
                    return (ContainerInfo)Index;
				else
					return ((ContainerInfo) (List[System.Convert.ToInt32(Index)]));
			}
		}
			
        public new int Count
		{
			get { return List.Count; }
		}

        public ContainerList() : base()
        { }
			
        #region Public Methods
		public ContainerInfo Add(ContainerInfo containerInfo)
		{
			this.List.Add(containerInfo);
			return containerInfo;
		}
			
		public void AddRange(ContainerInfo[] cInfo)
		{
			foreach (ContainerInfo containerInfo in cInfo)
			{
				List.Add(containerInfo);
			}
		}
			
		public ContainerInfo FindByConstantID(string id)
		{
			foreach (ContainerInfo containerInfo in List)
			{
				if (containerInfo.ConnectionInfo.ConstantID == id)
				{
					return containerInfo;
				}
			}
				
			return null;
		}
			
		public ContainerList Copy()
		{
			try
			{
                return (ContainerList)this.MemberwiseClone();
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
