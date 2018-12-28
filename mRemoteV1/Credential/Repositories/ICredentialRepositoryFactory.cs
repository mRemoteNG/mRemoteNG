namespace mRemoteNG.Credential.Repositories
{
    public interface ICredentialRepositoryFactory
    {
        /// <summary>
        /// The <see cref="ICredentialRepositoryConfig.TypeName"/> that this factory can build.
        /// </summary>
        string SupportsConfigType { get; }

        /// <summary>
        /// Builds a new <see cref="ICredentialRepositoryFactory"/> given the <see cref="ICredentialRepositoryConfig"/>
        /// that describes it. The <see cref="ICredentialRepositoryConfig.TypeName"/> must match this factory's
        /// <see cref="SupportsConfigType"/> property.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="isLoaded"></param>
        /// <returns></returns>
        ICredentialRepository Build(ICredentialRepositoryConfig config, bool isLoaded = false);
    }
}