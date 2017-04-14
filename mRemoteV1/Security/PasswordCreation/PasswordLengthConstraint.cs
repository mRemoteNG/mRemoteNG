using System;
using System.Security;


namespace mRemoteNG.Security.PasswordCreation
{
    public class PasswordLengthConstraint : IPasswordConstraint
    {
        private readonly int _minLength;
        private readonly int _maxLength;

        public string ConstraintHint { get; }

        public PasswordLengthConstraint(int minimumLength, int maxLength = int.MaxValue)
        {
            if (minimumLength < 0)
                throw new ArgumentException($"{nameof(minimumLength)} must be a positive value.");
            if (maxLength <= 0)
                throw new ArgumentException($"{nameof(maxLength)} must be a positive, non-zero value.");
            if (maxLength < minimumLength)
                throw new ArgumentException($"{nameof(maxLength)} must be greater than or equal to {nameof(minimumLength)}.");

            _minLength = minimumLength;
            _maxLength = maxLength;
            ConstraintHint = string.Format(Language.strPasswordLengthConstraintHint, _minLength, _maxLength);
        }

        public bool Validate(SecureString password)
        {
            if (password.Length < _minLength)
                return false;
            if (password.Length > _maxLength)
                return false;

            return true;
        }
    }
}