namespace mRemoteNG.Config
{
    public interface ISaver<in T>
    {
        void Save(T model, string propertyNameTrigger = "");
    }
}