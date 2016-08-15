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

        public void Add(ConnectionInfo newChildItem)
        {
            newChildItem.Parent = this;
            Children.Add(newChildItem);
        }

        public void AddRange(IEnumerable<ConnectionInfo> newChildren)
        {
            foreach (var child in newChildren)
            {
                Add(child);
            }
        }

        public void Remove(ConnectionInfo removalTarget)
        {
            removalTarget.Parent = null;
            Children.Remove(removalTarget);
        }

        public void RemoveRange(IEnumerable<ConnectionInfo> removalTargets)
        {
            foreach (var child in removalTargets)
            {
                Remove(child);
            }
        }

        public new ContainerInfo Copy()
		{
			return (ContainerInfo)MemberwiseClone();
		}
			
		private void SetDefaults()
		{
            IsExpanded = true;
		}
	}
}