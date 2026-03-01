using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security;
using System.Text.RegularExpressions;
using mRemoteNG.Resources.Language;


namespace mRemoteNG.Security.PasswordCreation
{
    public class PasswordIncludesSpecialCharactersConstraint : IPasswordConstraint
    {
        private readonly int _minimumCount;

        public IEnumerable<char> SpecialCharacters { get; } = new[] {'!', '@', '#', '$', '%', '^', '&', '*'};

        public string ConstraintHint { get; }

        public PasswordIncludesSpecialCharactersConstraint(int minimumCount = 1)
        {
            if (minimumCount < 0)
                throw new ArgumentException($"{nameof(minimumCount)} must be a positive value", nameof(minimumCount));

            _minimumCount = minimumCount;
            ConstraintHint = string.Format(CultureInfo.CurrentCulture, Language.PasswordConstainsSpecialCharactersConstraintHint, _minimumCount,
                                           string.Concat(SpecialCharacters));
        }

        public PasswordIncludesSpecialCharactersConstraint(IEnumerable<char> specialCharacters, int minimumCount = 1)
            : this(minimumCount)
        {
            ArgumentNullException.ThrowIfNull(specialCharacters);

            SpecialCharacters = specialCharacters;
            ConstraintHint = string.Format(CultureInfo.CurrentCulture, Language.PasswordConstainsSpecialCharactersConstraintHint, _minimumCount,
                                           string.Concat(SpecialCharacters));
        }

        public bool Validate(SecureString password)
        {
            Regex regex = new($"[{string.Concat(SpecialCharacters)}]");
            return regex.Count(password.ConvertToUnsecureString()) >= _minimumCount;
        }
    }
}