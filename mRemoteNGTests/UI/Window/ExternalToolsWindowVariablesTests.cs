using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.UI.Window;
using mRemoteNG.UI.Controls;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Window
{
    [TestFixture]
    public class ExternalToolsWindowVariablesTests
    {
        private static void RunWithMessagePump(Action testAction)
        {
            Exception? caught = null;
            var thread = new Thread(() =>
            {
                try
                {
                    testAction();
                }
                catch (Exception ex)
                {
                    caught = ex;
                }
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            if (caught != null)
                throw caught;
        }

        [Test]
        public void VariablesButton_InsertsVariable_IntoArgumentsTextBox()
        {
            RunWithMessagePump(() =>
            {
                using var window = new ExternalToolsWindow();
                // We don't necessarily need Show() if we are just interacting with controls, 
                // but CreateControl might be needed for some internal state.
                window.CreateControl(); 

                // Find controls using reflection
                var buttonField = typeof(ExternalToolsWindow).GetField("VariablesButton", BindingFlags.NonPublic | BindingFlags.Instance);
                Assert.That(buttonField, Is.Not.Null, "VariablesButton not found");
                var button = buttonField.GetValue(window) as MrngButton;
                Assert.That(button, Is.Not.Null, "VariablesButton is null");

                var argsField = typeof(ExternalToolsWindow).GetField("ArgumentsCheckBox", BindingFlags.NonPublic | BindingFlags.Instance);
                 Assert.That(argsField, Is.Not.Null, "ArgumentsCheckBox not found");
                var argsBox = argsField.GetValue(window) as MrngTextBox;
                 Assert.That(argsBox, Is.Not.Null, "ArgumentsCheckBox is null");

                // Simulate typing
                argsBox.Text = "foo ";
                argsBox.SelectionStart = 4;

                // Verify the method logic via reflection
                var insertMethod = typeof(ExternalToolsWindow).GetMethod("InsertVariable", BindingFlags.NonPublic | BindingFlags.Instance);
                Assert.That(insertMethod, Is.Not.Null, "InsertVariable method not found");

                insertMethod.Invoke(window, new object[] { "Hostname" });

                Assert.That(argsBox.Text, Is.EqualTo("foo %Hostname%"));
                Assert.That(argsBox.SelectionStart, Is.EqualTo(14)); // 4 + 10 (%Hostname%)
                
                // Test inserting another one
                argsBox.Text = "start ";
                argsBox.SelectionStart = 6;
                insertMethod.Invoke(window, new object[] { "Password" });
                Assert.That(argsBox.Text, Is.EqualTo("start %Password%"));
                
                // Test inserting in the middle
                argsBox.Text = "echo  end";
                argsBox.SelectionStart = 5;
                insertMethod.Invoke(window, new object[] { "Username" });
                Assert.That(argsBox.Text, Is.EqualTo("echo %Username% end"));
            });
        }
    }
}
