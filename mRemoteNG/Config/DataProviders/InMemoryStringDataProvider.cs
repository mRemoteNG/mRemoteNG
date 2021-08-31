namespace mRemoteNG.Config.DataProviders
{
    public class InMemoryStringDataProvider : IDataProvider<string>
    {
        private string _contents;

        public InMemoryStringDataProvider(string initialContents = "")
        {
            _contents = initialContents;
        }

        public string Load()
        {
            return _contents;
        }

        public void Save(string contents)
        {
            _contents = contents;
        }
    }
}