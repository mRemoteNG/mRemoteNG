using System;
using System.Security;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Security;

namespace mRemoteNG.Credential
{
    public class StaticDeserializerKeyProviderDecorator<TIn, TOut> : IDeserializer<TIn, TOut>, IHasKey
    {
        private readonly IDeserializer<TIn, TOut> _baseDeserializer;
        private readonly IHasKey _objThatNeedsKey;
        private SecureString _key = new SecureString();

        public SecureString Key
        {
            get { return _key; }
            set
            {
                if (value == null) return;
                _key = value;
            }
        }

        public StaticDeserializerKeyProviderDecorator(IHasKey objThatNeedsKey, IDeserializer<TIn, TOut> baseDeserializer)
        {
            if (objThatNeedsKey == null)
                throw new ArgumentNullException(nameof(objThatNeedsKey));
            if (baseDeserializer == null)
                throw new ArgumentNullException(nameof(baseDeserializer));

            _objThatNeedsKey = objThatNeedsKey;
            _baseDeserializer = baseDeserializer;
        }

        public TOut Deserialize(TIn serializedData)
        {
            _objThatNeedsKey.Key = Key;
            return _baseDeserializer.Deserialize(serializedData);
        }
    }
}
