using mRemoteNG.Container;

namespace mRemoteNG.Config.Import
{
    public interface IConnectionImporter<in TSource>
        where TSource : class
    {
        void Import(TSource source, ContainerInfo destinationContainer);
    }
}