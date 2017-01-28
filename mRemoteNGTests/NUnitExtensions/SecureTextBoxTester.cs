using System.Windows.Forms;
using mRemoteNG.UI.Controls;
using NUnit.Extensions.Forms;


namespace mRemoteNGTests.NUnitExtensions
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

        public SecureTextBoxTester(ControlTester tester, int index) : base(tester, index)
        {
        }

        public SecureTextBox Properties => (SecureTextBox) Control;
    }
}