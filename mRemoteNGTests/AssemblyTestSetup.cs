using System.Windows.Forms;
using NUnit.Framework;


// Dont put this in a namespace. Leaving it by itself tells NUnit
// to run it on assembly load
[SetUpFixture]
public class AssemblyTestSetup
{
    [OneTimeSetUp]
    public void AssemblySetup()
    {
        // ensure window options set before any test window created
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
    }
}
