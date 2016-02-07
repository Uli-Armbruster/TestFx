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
using JetBrains.Annotations;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using TestFx.ReSharper.Model.Tree;
using TestFx.ReSharper.Utilities.Psi.Tree;

namespace TestFx.ReSharper.Daemon
{
  public abstract class SimpleTestDeclarationHighlightingBase : IHighlighting
  {
    private readonly ITestDeclaration _treeNode;
    private readonly string _toolTipText;

    protected SimpleTestDeclarationHighlightingBase(ITestDeclaration treeNode, [NotNull] string toolTipText)
    {
      _treeNode = treeNode;
      _toolTipText = toolTipText;
    }

    public string ToolTip
    {
      get { return _toolTipText; }
    }

    public string ErrorStripeToolTip
    {
      get { return _toolTipText; }
    }

    public int NavigationOffsetPatch
    {
      get { return 0; }
    }

    public bool IsValid ()
    {
      return _treeNode.IsValid();
    }

    public virtual DocumentRange CalculateRange ()
    {
      return _treeNode.GetRanges().NavigationRange;
    }
  }
}