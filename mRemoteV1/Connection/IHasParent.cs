using mRemoteNG.Container;

namespace mRemoteNG.Connection
{
    public interface IHasParent
    {
        ContainerInfo Parent { get; }

        void SetParent(ContainerInfo containerInfo);

        void RemoveParent();
    }
}