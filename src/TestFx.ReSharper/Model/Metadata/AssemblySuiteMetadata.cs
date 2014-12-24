// Copyright 2014, 2013 Matthias Koch
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Metadata.Reader.API;
using JetBrains.ProjectModel;
using TestFx.ReSharper.Model.Metadata.Wrapper;
using TestFx.Utilities;

namespace TestFx.ReSharper.Model.Metadata
{
  [DebuggerDisplay (Identifiable.DebuggerDisplay)]
  public class AssemblySuiteMetadata : MetadataAssemblyBase, ISuiteMetadata
  {
    private readonly IIdentity _identity;
    private readonly IProject _project;
    private readonly string _text;
    private readonly IEnumerable<ISuiteMetadata> _suiteMetadatas;

    public AssemblySuiteMetadata (
        IIdentity identity,
        IProject project,
        string text,
        IEnumerable<ISuiteMetadata> suiteMetadatas,
        IMetadataAssembly metadataAssembly)
        : base(metadataAssembly)
    {
      _identity = identity;
      _project = project;
      _text = text;
      _suiteMetadatas = suiteMetadatas;
    }

    public IIdentity Identity
    {
      get { return _identity; }
    }

    public IProject Project
    {
      get { return _project; }
    }

    public string Text
    {
      get { return _text; }
    }

    public IEnumerable<ISuiteMetadata> SuiteMetadatas
    {
      get { return _suiteMetadatas; }
    }

    public IEnumerable<ISuiteEntity> SuiteEntities
    {
      get { return _suiteMetadatas.Cast<ISuiteEntity>(); }
    }

    public IEnumerable<ITestEntity> TestEntities
    {
      get { yield break; }
    }
  }
}