using System;
using System.Threading;
using FlaUI.Core.AutomationElements;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace mRemoteNGSpecs.StepDefinitions
{
    [Binding]
    public class SmokeTestSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public SmokeTestSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"the application is running")]
        public void GivenTheApplicationIsRunning()
        {
            var mainWindow = _scenarioContext.Get<Window>();
            Assert.That(mainWindow, Is.Not.Null, "Main window should be available after launch.");
        }

        [Then(@"the main window title contains ""(.*)""")]
        public void ThenTheMainWindowTitleContains(string expectedTitle)
        {
            var mainWindow = _scenarioContext.Get<Window>();

            // FlaUI may return the window before the title is fully set.
            // Poll for up to 10 seconds waiting for the title to populate.
            var deadline = DateTime.UtcNow.AddSeconds(10);
            string title = mainWindow.Title;
            while (string.IsNullOrWhiteSpace(title) || !title.Contains(expectedTitle, StringComparison.OrdinalIgnoreCase))
            {
                if (DateTime.UtcNow >= deadline)
                    break;
                Thread.Sleep(500);
                title = mainWindow.Title;
            }

            Assert.That(title, Does.Contain(expectedTitle),
                $"Expected main window title to contain '{expectedTitle}', but was '{title}'.");
        }
    }
}
