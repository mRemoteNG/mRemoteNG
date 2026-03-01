using FlaUI.Core.AutomationElements;
using mRemoteNGSpecs.Drivers;
using TechTalk.SpecFlow;

namespace mRemoteNGSpecs.Hooks
{
    /// <summary>
    /// SpecFlow hooks that manage the AppDriver lifecycle for UI automation scenarios.
    /// Tagged with @ui so only UI scenarios pay the startup cost.
    /// </summary>
    [Binding]
    public sealed class AutomationHooks
    {
        private readonly ScenarioContext _scenarioContext;

        public AutomationHooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario("ui")]
        public void BeforeUiScenario()
        {
            var driver = new AppDriver();
            var mainWindow = driver.Start();
            _scenarioContext.Set(driver);
            _scenarioContext.Set(mainWindow);
        }

        [AfterScenario("ui")]
        public void AfterUiScenario()
        {
            if (_scenarioContext.TryGetValue<AppDriver>(out var driver))
            {
                driver.Dispose();
            }
        }
    }
}
