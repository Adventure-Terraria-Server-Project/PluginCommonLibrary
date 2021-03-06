﻿using System;
using System.Collections.Generic;

namespace Terraria.Plugins.Common.Test {
  public abstract class TestRunnerBase {
    private readonly Dictionary<string,TestRunData> testRunData;
    private IEnumerator<KeyValuePair<string,TestRunData>> testRunEnumerator;
    private List<string> testRunSucceededTests;
    private List<string> testRunFailedTests;

    public bool IsRunning { get; private set; }
    protected PluginTrace PluginTrace { get; private set; }

    #region [Event: TestRunCompleted]
    public event EventHandler TestRunCompleted;

    protected virtual void OnTestRunCompleted() {
      if (TestRunCompleted != null)
        this.TestRunCompleted(this, EventArgs.Empty);
    }
    #endregion


    protected TestRunnerBase(PluginTrace pluginTrace) {
      this.testRunData = new Dictionary<string,TestRunData>();
      this.PluginTrace = pluginTrace;
    }

    protected void RegisterTest(string testName, Action<TestContext> testAction) {
      if (this.IsRunning)
        throw new InvalidOperationException("Registering new tests is not possible while a test run is in progress.");

      this.testRunData.Add(testName, new TestRunData(testAction));
    }

    public void RunAllTests() {
      if (this.IsRunning)
        throw new InvalidOperationException("A testrun is already in progress.");

      this.testRunEnumerator = this.testRunData.GetEnumerator();
      this.testRunSucceededTests = new List<string>();
      this.testRunFailedTests = new List<string>();

      this.IsRunning = true;

      this.PluginTrace.WriteLineInfo("------------------------------------------");
      this.PluginTrace.WriteLineInfo("Test running with {0} tests...", this.testRunData.Count);
    }

    protected abstract void TestInit();
    protected abstract void TestCleanup();

    private int frameCounter;
    public virtual void HandleGameUpdate() {
      if (!this.IsRunning)
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

          return;
        } else if (testPair.Value.FailException == null) {
          try {
            this.TestCleanup();
            this.testRunSucceededTests.Add(testPair.Key);
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
        } catch (Exception ex) {
          testPair.Value.FailException = ex;
          this.testRunFailedTests.Add(testPair.Key);
          return;
        }
      } else {
        this.PluginTrace.WriteLineInfo("{0} tests passed.", this.testRunSucceededTests.Count);
        this.PluginTrace.WriteLineInfo("{0} tests failed to pass.", this.testRunFailedTests.Count);

        if (this.testRunFailedTests.Count > 0) {
          this.PluginTrace.WriteLineInfo("");
          this.PluginTrace.WriteLineInfo("Failed tests:");

          foreach (string failedTestName in this.testRunFailedTests) {
            TestRunData testRunData = this.testRunData[failedTestName];

            if (!string.IsNullOrWhiteSpace(testRunData.Context.Phase))
              this.PluginTrace.WriteLineInfo("\"{0}\" at phase \"{1}\"", failedTestName, testRunData.Context.Phase);
            else 
              this.PluginTrace.WriteLineInfo("\"{0}\"", failedTestName);

            this.PluginTrace.WriteLineInfo(testRunData.FailException.Message);
              
            testRunData.Reset();
          }
        }

        this.PluginTrace.WriteLineInfo("------------------------------------------");

        this.IsRunning = false;
        this.OnTestRunCompleted();
      }
    }

    public override string ToString() {
      return $"{this.testRunData.Count} registered tests.";
    }
  }
}
