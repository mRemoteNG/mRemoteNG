using System.Security;


namespace mRemoteNG.Security.PasswordCreation
{
    public interface IPasswordConstraint
    {
        string ConstraintHint { get; }

        bool Validate(SecureString password);
    }
}