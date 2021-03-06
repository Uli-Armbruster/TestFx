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
using System.Runtime.CompilerServices;
using TestFx.Extensibility;
using TestFx.Extensibility.Containers;
using TestFx.SpecK.Implementation.Controllers;
using TestFx.SpecK.InferredApi;

namespace TestFx.SpecK.Implementation.Containers
{
  internal class SpecializedSuiteContainer<TSubject, TResult> : Container, IIgnoreOrCase<TSubject, TResult>
  {
    private readonly ISpecializedSuiteController<TSubject, TResult> _controller;

    public SpecializedSuiteContainer (ISpecializedSuiteController<TSubject, TResult> controller)
        : base(controller)
    {
      _controller = controller;
    }

    public ICase<TSubject, TResult> Skip (string reason)
    {
      _controller.IgnoreNext(reason);
      return this;
    }

    [DisplayFormat ("{0}")]
    public IIgnoreOrCase<TSubject, TResult> Case (
        string description,
        Func<ICombineOrArrangeOrAssert<TSubject, TResult, Dummy, Dummy>, IAssert> succession,
        [CallerFilePath] string filePath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
      var testController = _controller.CreateTestController(description, filePath, lineNumber);
      succession(TestContainer.Create(testController));
      return this;
    }
  }
}