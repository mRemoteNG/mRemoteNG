using mRemoteNG.Container;


namespace mRemoteNG.Config.Import
{
    public interface IConnectionImporter
    {
        void Import(object source, ContainerInfo destinationContainer);
    }
}