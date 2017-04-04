using System;
using System.Security;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Security;

namespace mRemoteNG.Credential
{
    public class StaticSerializerKeyProviderDecorator<TIn, TOut> : ISerializer<TIn, TOut>, IHasKey
    {
        private readonly ISerializer<TIn, TOut> _baseSerializer;
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

        public StaticSerializerKeyProviderDecorator(IHasKey objThatNeedsKey, ISerializer<TIn, TOut> baseSerializer)
        {
            if (objThatNeedsKey == null)
                throw new ArgumentNullException(nameof(objThatNeedsKey));
            if (baseSerializer == null)
                throw new ArgumentNullException(nameof(baseSerializer));

            _objThatNeedsKey = objThatNeedsKey;
            _baseSerializer = baseSerializer;
        }

        public TOut Serialize(TIn model)
        {
            _objThatNeedsKey.Key = Key;
            return _baseSerializer.Serialize(model);
        }
    }
}