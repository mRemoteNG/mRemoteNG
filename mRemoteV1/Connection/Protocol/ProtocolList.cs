using System;
using System.Collections;


namespace mRemoteNG.Connection.Protocol
{
	public class ProtocolList : CollectionBase
	{
        public ProtocolBase this[object index]
		{
			get
			{
			    var @base = index as ProtocolBase;
			    if (@base != null)
                    return @base;
			    return ((ProtocolBase) (List[Convert.ToInt32(index)]));
			}
		}
				
        public new int Count => List.Count;

				
		public void Add(ProtocolBase cProt)
		{
			List.Add(cProt);
		}
				
		public void AddRange(ProtocolBase[] cProt)
		{
			foreach (var cP in cProt)
			{
				List.Add(cP);
			}
		}
				
		public void Remove(ProtocolBase cProt)
		{
			try
			{
				List.Remove(cProt);
			}
			catch (Exception)
			{
			}
		}
				
		public new void Clear()
		{
			List.Clear();
		}
	}
}