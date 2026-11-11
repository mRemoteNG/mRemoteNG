using System.Collections.Generic;
using System.Linq;

namespace mRemoteNG.Security.PasswordCreation
{
    public enum PasswordStrength
    {
        None,
        NotEstimated,
        Weak,
        Fair,
        Good,
        Strong
    }

    public sealed record PasswordStrengthResult(PasswordStrength Strength, string Advice);

    /// <summary>
    /// Rates a password with zxcvbn, which looks for common passwords, words,
    /// names, keyboard patterns, dates and repeats rather than only counting
    /// character types.
    /// </summary>
    public static class PasswordStrengthMeter
    {
        public const int MaximumPasswordLength = 256;

        public static PasswordStrengthResult Evaluate(string password, IEnumerable<string> userInputs = null)
        {
            if (string.IsNullOrEmpty(password))
                return new PasswordStrengthResult(PasswordStrength.None, string.Empty);
            if (password.Length > MaximumPasswordLength)
                return new PasswordStrengthResult(PasswordStrength.NotEstimated, string.Empty);

            Zxcvbn.Result result = Zxcvbn.Core.EvaluatePassword(password, userInputs);
            PasswordStrength strength = result.Score switch
            {
                <= 1 => PasswordStrength.Weak,
                2 => PasswordStrength.Fair,
                3 => PasswordStrength.Good,
                _ => PasswordStrength.Strong
            };

            IEnumerable<string> advice = new[] { result.Feedback?.Warning }
                .Concat(result.Feedback?.Suggestions ?? Enumerable.Empty<string>())
                .Where(text => !string.IsNullOrWhiteSpace(text));
            return new PasswordStrengthResult(strength, string.Join(" ", advice));
        }
    }
}
