using System.Windows.Forms;
using NUnit.Framework;

// Outside any namespace, so it runs once before every test in the assembly.
[SetUpFixture]
public class TestRunSetup
{
    // As the application does at startup. Visual styles only take effect when they are enabled
    // before the first control is created, and the text box cue banner needs them, so this
    // cannot wait for whichever test happens to create a form first.
    [OneTimeSetUp]
    public void EnableVisualStyles()
    {
        Application.EnableVisualStyles();
    }
}
