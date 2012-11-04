// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Terraria.Plugins.CoderCow.Test {
  public abstract class TestRunnerBase {
    private readonly Dictionary<string,TestRunData> testRunData;

    #region [Property: Trace]
    private readonly PluginTrace trace;

    protected PluginTrace Trace {
      get { return this.trace; }
    }
    #endregion


    #region [Method: Constructor]
    protected TestRunnerBase(PluginTrace trace) {
      this.testRunData = new Dictionary<string,TestRunData>();
      this.trace = trace;
    }
    #endregion

    #region [Methods: RegisterTest, RunAllTests, TestInit, TestCleanup]
    protected void RegisterTest(string testName, Action<TestContext> testAction) {
      if (this.testRunIsRunning)
        throw new InvalidOperationException("Registering new tests is not possible while a test run is in progress.");

      this.testRunData.Add(testName, new TestRunData(testAction));
    }

    private bool testRunIsRunning;
    private IEnumerator<KeyValuePair<string,TestRunData>> testRunEnumerator;
    private List<string> testRunSucceededTests;
    private List<string> testRunFailedTests;
    public void RunAllTests() {
      if (this.testRunIsRunning)
        throw new InvalidOperationException("A testrun is already in progress.");

      this.testRunEnumerator = this.testRunData.GetEnumerator();
      this.testRunSucceededTests = new List<string>();
      this.testRunFailedTests = new List<string>();

      this.testRunIsRunning = true;

      this.Trace.WriteLineInfo("------------------------------------------");
      this.Trace.WriteLineInfo("Test running with {0} tests...", this.testRunData.Count);
    }

    protected abstract void TestInit();
    protected abstract void TestCleanup();
    #endregion

    #region [Method: HandleGameUpdate]
    private int frameCounter;
    public void HandleGameUpdate() {
      if (!this.testRunIsRunning)
        return;

      if (this.frameCounter < 10) {
        this.frameCounter++;
        return;
      }

      this.frameCounter = 0;
      KeyValuePair<string,TestRunData> testPair = this.testRunEnumerator.Current;
      bool isIteration = (testPair.Value == null);
      if (!isIteration) {
        if (testPair.Value.Context.DelayedActions.Count > 0) {
          TestDelay delayedAction = testPair.Value.Context.DelayedActions[0];

          if (delayedAction.FramesLeft <= 0) {
            try {
              if (delayedAction.Action != null)
                delayedAction.Action(testPair.Value.Context);

              testPair.Value.Context.DelayedActions.RemoveAt(0);

              return;
            } catch (Exception ex) {
              testPair.Value.FailException = ex;
              this.testRunFailedTests.Add(testPair.Key);

              testPair.Value.Context.DelayedActions.Clear();
            }
          } else {
            delayedAction.FramesLeft -= 10;
          }
        } else {
          try {
            this.TestCleanup();
          } catch (Exception ex) {
            testPair.Value.FailException = new InvalidOperationException(
              "TestCleanup threw an exception. See inner exception for details.", ex
            );
            this.testRunFailedTests.Add(testPair.Key);
          }
        }
      }

      if (this.testRunEnumerator.MoveNext()) {
        testPair = this.testRunEnumerator.Current;

        try {
          this.TestInit();
        } catch (Exception ex) {
          testPair.Value.FailException = new InvalidOperationException(
            "TestInit threw an exception. See inner exception for details.", ex
          );
          this.testRunFailedTests.Add(testPair.Key);
          return;
        }

        try {
          testPair.Value.TestAction(testPair.Value.Context);
          this.testRunSucceededTests.Add(testPair.Key);
        } catch (Exception ex) {
          testPair.Value.FailException = ex;
          this.testRunFailedTests.Add(testPair.Key);
          return;
        }
      } else {
        this.Trace.WriteLineInfo("{0} tests passed.", this.testRunSucceededTests.Count);
        this.Trace.WriteLineInfo("{0} tests failed to pass.", this.testRunFailedTests.Count);

        if (this.testRunFailedTests.Count > 0) {
          this.Trace.WriteLineInfo("");
          this.Trace.WriteLineInfo("Failed tests:");

          foreach (string failedTestName in this.testRunFailedTests) {
            TestRunData testRunData = this.testRunData[failedTestName];

            if (!string.IsNullOrWhiteSpace(testRunData.Context.Phase))
              this.Trace.WriteLineInfo("\"{0}\" at phase \"{1}\"", failedTestName, testRunData.Context.Phase);
            else 
              this.Trace.WriteLineInfo("\"{0}\"", failedTestName);

            this.Trace.WriteLineInfo(testRunData.FailException.Message);
              
            testRunData.Reset();
          }
        }

        this.Trace.WriteLineInfo("------------------------------------------");

        this.testRunIsRunning = false;
      }
    }
    #endregion

    #region [Method: ToString]
    public override string ToString() {
      return string.Format("{0} registered tests.", this.testRunData.Count);
    }
    #endregion
  }
}
