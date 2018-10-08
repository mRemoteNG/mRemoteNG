using System;
using System.Linq;

namespace mRemoteNG.Tools
{
    public class DisposableOptional<T> : Optional<T>, IDisposable
        where T : IDisposable
    {
        public DisposableOptional()
            : base()
        {
        }

        public DisposableOptional(T value)
            : base(value)
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!disposing || !this.Any())
                return;

            this.First().Dispose();
        }
    }
}
