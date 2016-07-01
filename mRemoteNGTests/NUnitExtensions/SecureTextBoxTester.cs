using System.Windows.Forms;
using mRemoteNG.UI.Controls;

// ReSharper disable once CheckNamespace
namespace NUnit.Extensions.Forms
{
    public class SecureTextBoxTester : ControlTester
    {
        public SecureTextBoxTester(string name, string formName) : base(name, formName)
        {
        }

        public SecureTextBoxTester(string name, Form form) : base(name, form)
        {
        }

        public SecureTextBoxTester(string name) : base(name)
        {
        }

        public SecureTextBox Properties => (SecureTextBox)Control;
    }
}