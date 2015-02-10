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
using System.Text;

namespace TestFx.Utilities.Introspection
{
  public class CommonExpressionProvider
  {
    private readonly Lazy<CommonExpression> _lazyExpression;
    private readonly Lazy<string> _toString;

    public CommonExpressionProvider (Func<CommonExpression> factory, IEnumerable<CommonType> strippedTypes)
    {
      _lazyExpression = new Lazy<CommonExpression>(factory);
      _toString = new Lazy<string>(() => new Parser(Expression, strippedTypes.Select(x => x.Fullname)).ToString());
    }

    public CommonExpression Expression
    {
      get { return _lazyExpression.Value; }
    }

    public override string ToString ()
    {
      return _toString.Value;
    }

    private class Parser
    {
      private readonly CommonExpression _expression;
      private readonly IEnumerable<string> _strippedTypeFullNames;
      private readonly StringBuilder _builder;

      public Parser (CommonExpression expression, IEnumerable<string> strippedTypeFullNames)
      {
        _expression = expression;
        _strippedTypeFullNames = strippedTypeFullNames;
        _builder = new StringBuilder();
      }

      private void Visit (CommonExpression expression)
      {
        if (expression is CommonInvocationExpression)
          VisitInvocation(expression.To<CommonInvocationExpression>());
        else if (expression is CommonConstantExpression)
          VisitConstant(expression.To<CommonConstantExpression>());
        else if (expression is CommonMemberAccessExpression)
          VisitMemberAccess(expression.To<CommonMemberAccessExpression>());
        else if (expression is CommonParameterExpression)
          VisitParameter(expression.To<CommonParameterExpression>());
        else if (expression is CommonThisExpression)
          VisitThis(expression.To<CommonThisExpression>());
        else if (expression is CommonArrayItemsExpression)
          VisitArrayItems(expression.To<CommonArrayItemsExpression>());
        else if (expression is CommonNewObjectExpression)
          VisitNewObject(expression.To<CommonNewObjectExpression>());
        else
          throw new Exception(string.Format("Expressions of type {0} are not supported.", expression.GetType()));
      }

      private void VisitNewObject (CommonNewObjectExpression expression)
      {
        _builder.Append("new ").Append(expression.Type.Name);

        _builder.Append("(");
        VisitEnumerable(expression.Arguments);
        _builder.Append(")");
      }

      private void VisitThis (CommonThisExpression expression)
      {
        _builder.Append(expression.Type.Name);
      }

      private void VisitInvocation (CommonInvocationExpression expression)
      {
        var instance = expression.Instance;
        var arguments = expression.Arguments.ToList();
        var method = expression.Method;
        if (method.IsExtension)
        {
          instance = arguments.First();
          arguments = arguments.Skip(1).ToList();
        }

        VisitMember(instance, method);

        _builder.Append("(");
        VisitEnumerable(arguments);
        _builder.Append(")");
      }

      private void VisitMemberAccess (CommonMemberAccessExpression expression)
      {
        VisitMember(expression.Instance, expression.Member);
      }

      private void VisitMember (CommonExpression instance, CommonMemberInfo member)
      {
        if (instance != null)
        {
          if (!_strippedTypeFullNames.Any(x => instance.Type.IsAssignableTo(x)))
          {
            Visit(instance);
            _builder.Append(".");
          }
        }
        else
        {
          Trace.Assert(member.IsStatic);
          _builder.Append(member.DeclaringType.Name);
          _builder.Append(".");
        }

        _builder.Append(member.Name);
      }

      private void VisitConstant (CommonConstantExpression expression)
      {
        if (expression.Type.Fullname != typeof (string).FullName)
          _builder.Append(expression.Value);
        else
          _builder.Append("\"").Append(expression.Value).Append("\"");
      }

      private void VisitArrayItems (CommonArrayItemsExpression expression)
      {
        _builder.Append("[ ");
        VisitEnumerable(expression.Items);
        _builder.Append(" ]");
      }

      private void VisitParameter (CommonParameterExpression expression)
      {
        _builder.Append(expression.Type.Name);
      }

      private void VisitEnumerable (IEnumerable<CommonExpression> expressions, string value = ", ")
      {
        var expressionList = expressions.ToList();
        for (var i = 0; i < expressionList.Count; i++)
        {
          Visit(expressionList[i]);
          if (i < expressionList.Count - 1)
            _builder.Append(value);
        }
      }

      public override string ToString ()
      {
        if (_builder.Length == 0)
          Visit(_expression);

        return _builder.ToString();
      }
    }
  }
}