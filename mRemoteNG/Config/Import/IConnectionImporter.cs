using mRemoteNG.Container;
using System.Threading;

namespace mRemoteNG.Config.Import
{
    public interface IConnectionImporter<in TSource>
        where TSource : class
    {
        void Import(TSource source, ContainerInfo destinationContainer);

        void Import(TSource source, ContainerInfo destinationContainer, CancellationToken cancellationToken)
        {
            ImportStager.Run(destinationContainer, cancellationToken, staged => Import(source, staged));
        }
    }
}