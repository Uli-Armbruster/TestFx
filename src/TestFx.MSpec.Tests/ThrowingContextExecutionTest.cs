// Copyright 2016, 2015, 2014 Matthias Koch
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
using FakeItEasy;
using FakeItEasy.Core;
using TestFx.Evaluation.Results;

namespace TestFx.MSpec.Tests
{
  public class ThrowingContextExecutionTest : ContextTestBase
  {
    public override void SetUp ()
    {
      base.SetUp();

      BaseContextBaseEstablish = ThrowingAction;
    }

    protected override void AssertResults (IRunResult runResult, IFakeScope scope)
    {
      using (scope.OrderedAssertions())
      {
        A.CallTo(() => OuterContextBaseEstablish()).MustHaveHappened();
        A.CallTo(() => OuterContextEstablish()).MustHaveHappened();

        A.CallTo(() => BaseContextEstablish()).MustNotHaveHappened();
        A.CallTo(() => Establish()).MustNotHaveHappened();
        A.CallTo(() => Action()).MustNotHaveHappened();
        A.CallTo(() => Assertion()).MustNotHaveHappened();
        A.CallTo(() => Cleanup()).MustNotHaveHappened();
        A.CallTo(() => BaseContextCleanup()).MustNotHaveHappened();
        A.CallTo(() => BaseContextBaseCleanup()).MustNotHaveHappened();

        A.CallTo(() => OuterContextCleanup()).MustHaveHappened();
        A.CallTo(() => OuterContextBaseCleanup()).MustHaveHappened();
      }
    }
  }
}