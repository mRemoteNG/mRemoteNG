using System.Collections.Generic;
using mRemoteNG.Connection;
using System.ComponentModel;

namespace mRemoteNG.Container
{
    [DefaultProperty("Name")]
    public class ContainerInfo : ConnectionInfo
	{
        [Browsable(false)]
        public List<ConnectionInfo> Children { get; set; } = new List<ConnectionInfo>();

        [Category(""), Browsable(false), ReadOnly(false), Bindable(false), DefaultValue(""), DesignOnly(false)]
        public bool IsExpanded { get; set; }

        public ContainerInfo()
        {
            SetDefaults();
            IsContainer = true;
        }

        public new ContainerInfo Copy()
		{
			return (ContainerInfo)MemberwiseClone();
		}
			
		public void SetDefaults()
		{
            IsExpanded = true;
		}
	}
}