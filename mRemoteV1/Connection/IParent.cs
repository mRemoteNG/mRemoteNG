using mRemoteNG.Container;

namespace mRemoteNG.Connection
{
    public interface IParent
    {
        ContainerInfo Parent { get; set; }
    }
}