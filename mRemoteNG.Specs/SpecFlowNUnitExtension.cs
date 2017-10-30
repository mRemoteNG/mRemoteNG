// ------------------------------------------------------------------------------
//
//   SpecFlow NUnit Runner AddIn 
// 
//   This AddIn ensures the [AfterTestRun] hook to be fired when the tests are
//   executed with *nunit-console.exe* or *nunit-console-x86.exe*.  
//   For executing the tests with other runner, this code is not necessary.
//
//   See also: https://github.com/techtalk/SpecFlow/wiki/Unit-test-providers
//   Copyright © SpecFlow 2012, http://www.specflow.org
//
// ------------------------------------------------------------------------------
using System;
using System.Reflection;
using NUnit.Core;
using NUnit.Core.Extensibility;

namespace TechTalk.SpecFlow
{
    [NUnitAddin(Type = ExtensionType.Core, Name = "SpecFlow")]
    public class SpecFlowNUnitExtension : IAddin, EventListener
    {
        public bool Install(IExtensionHost host)
        {
            IExtensionPoint listeners = host.GetExtensionPoint("EventListeners");

            listeners.Install(this);

            return true;
        }

        private void TriggerTestRunEnd()
        {
            ITestRunner testRunner = (ITestRunner)typeof(ScenarioContext).GetProperty("TestRunner", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(ScenarioContext.Current, null);
            testRunner.OnTestRunEnd();
        }

        public void RunFinished(TestResult result)
        {
            TriggerTestRunEnd();
        }

        public void RunFinished(Exception exception)
        {
            TriggerTestRunEnd();
        }

        #region Not-affected listener methods
        public void RunStarted(string name, int testCount)
        {
        }

        public void TestStarted(TestName testName)
        {
        }

        public void TestFinished(TestResult result)
        {
        }

        public void SuiteStarted(TestName testName)
        {
        }

        public void SuiteFinished(TestResult result)
        {
        }

        public void UnhandledException(Exception exception)
        {
        }

        public void TestOutput(TestOutput testOutput)
        {
        }
        #endregion
    }
}
