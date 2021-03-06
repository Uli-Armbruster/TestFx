﻿// Copyright 2016, 2015, 2014 Matthias Koch
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using TestFx.Evaluation.Intents;
using TestFx.Evaluation.Results;

namespace TestFx.Evaluation.Reporting
{
  internal class CrossAppDomainRunListener : MarshalByRefObject, IRunListener
  {
    public static IRunListener Create (params IRunListener[] listeners)
    {
      return new CrossAppDomainRunListener(new CompositeRunListener(listeners));
    }

    private readonly IRunListener _listener;

    public CrossAppDomainRunListener (IRunListener listener)
    {
      _listener = listener;
    }

    public void OnRunStarted (IRunIntent intent)
    {
      _listener.OnRunStarted(intent);
    }

    public void OnRunFinished (IRunResult result)
    {
      _listener.OnRunFinished(result);
    }

    public void OnSuiteStarted (IIntent intent, string text)
    {
      _listener.OnSuiteStarted(intent, text);
    }

    public void OnSuiteFinished (ISuiteResult result)
    {
      _listener.OnSuiteFinished(result);
    }

    public void OnTestStarted (IIntent intent, string text)
    {
      _listener.OnTestStarted(intent, text);
    }

    public void OnTestFinished (ITestResult result)
    {
      _listener.OnTestFinished(result);
    }

    public void OnError (IExceptionDescriptor exception)
    {
      _listener.OnError(exception);
    }
  }
}