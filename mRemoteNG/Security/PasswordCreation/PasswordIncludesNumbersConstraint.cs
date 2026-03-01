using System;
using System.Globalization;
using System.Security;
using System.Text.RegularExpressions;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.Security.PasswordCreation
{
    public class PasswordIncludesNumbersConstraint : IPasswordConstraint
    {
        private readonly int _minimumCount;

        public string ConstraintHint { get; }

        public PasswordIncludesNumbersConstraint(int minimumCount = 1)
        {
            if (minimumCount < 0)
                throw new ArgumentException($"{nameof(minimumCount)} must be a positive value", nameof(minimumCount));

            _minimumCount = minimumCount;
            ConstraintHint = string.Format(CultureInfo.CurrentCulture, Language.PasswordContainsNumbersConstraint, _minimumCount);
        }

        public bool Validate(SecureString password)
        {
            Regex regex = new(@"\d");
            return regex.Count(password.ConvertToUnsecureString()) >= _minimumCount;
        }
    }
}