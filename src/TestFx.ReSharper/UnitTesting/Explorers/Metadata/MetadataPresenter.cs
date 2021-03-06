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
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Metadata.Reader.API;
using JetBrains.ReSharper.Psi;
using JetBrains.Util;
using TestFx.Extensibility;
using TestFx.ReSharper.Utilities.Metadata;

namespace TestFx.ReSharper.UnitTesting.Explorers.Metadata
{
  public interface IMetadataPresenter
  {
    [CanBeNull]
    string Present (IMetadataTypeInfo type, string suiteAttributeType);

    [CanBeNull]
    string Present (IMetadataField field);
  }

  [PsiComponent]
  internal class MetadataPresenter : IMetadataPresenter
  {
    private readonly IntrospectionPresenter _introspectionPresenter;

    public MetadataPresenter ()
    {
      _introspectionPresenter = new IntrospectionPresenter();
    }

    [CanBeNull]
    public string Present (IMetadataTypeInfo type, string suiteAttributeType)
    {
      var subjectAttributeData = type.GetAttributeData(suiteAttributeType);
      if (subjectAttributeData == null)
        return null;

      var subjectAttribute = subjectAttributeData.ToCommon();
      var displayFormatAttribute = subjectAttributeData.UsedConstructor.NotNull().GetAttributeData<DisplayFormatAttribute>().NotNull().ToCommon();
      return _introspectionPresenter.Present(displayFormatAttribute, type.ToCommon(), subjectAttribute);
    }

    [CanBeNull]
    public string Present (IMetadataField field)
    {
      var metadataType = (field.Type as IMetadataClassType)?.Type;
      var displayFormatAttribute = metadataType?.GetAttributeData<DisplayFormatAttribute>();
      if (displayFormatAttribute == null)
        return null;

      return _introspectionPresenter.Present(displayFormatAttribute.ToCommon(), field.ToCommon());
    }
  }
}