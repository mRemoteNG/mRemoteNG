using System;
using System.Security;
using System.Text.RegularExpressions;

namespace mRemoteNG.Security.PasswordCreation
{
    public class PasswordIncludesUpperCaseConstraint : IPasswordConstraint
    {
        private readonly int _minimumCount;

        public string ConstraintHint { get; }

        public PasswordIncludesUpperCaseConstraint(int minimumCount = 1)
        {
            if (minimumCount < 0)
                throw new ArgumentException($"{nameof(minimumCount)} must be a positive value");

            _minimumCount = minimumCount;
            ConstraintHint = string.Format(Language.strPasswordContainsUpperCaseConstraintHint, _minimumCount);
        }

        public bool Validate(SecureString password)
        {
            var regex = new Regex(@"[A-Z]");
            return regex.Matches(password.ConvertToUnsecureString()).Count >= _minimumCount;
        }
    }
}