/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Utils
{
    public sealed class NotifierFindingExpressionVisitor : ExpressionVisitor
    {
        public HashSet<string> PropNames { get; set; }
        public HashSet<INotifyPropertyChanged> Notifiers { get; set; }

        public NotifierFindingExpressionVisitor(Expression exp)
        {
            PropNames = new HashSet<string>();
            Notifiers = new HashSet<INotifyPropertyChanged>();
            Visit(exp);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (IsPropertyChain(node.Expression))
            {
                if (node.NodeType == ExpressionType.MemberAccess)
                {
                    var expression = node.Expression as ConstantExpression;
                    if (expression != null)
                    {
                        AddNotifier(expression.Value);
                    }
                    else if (node.Expression is MemberExpression)
                    {
                        AddNotifier(GetValue((MemberExpression)node.Expression));
                    }
                    PropNames.Add(node.Member.Name);
                }
            }
            else if (node.NodeType == ExpressionType.Constant)
            {
                AddNotifier(((ConstantExpression)node.Expression).Value);
            }

            return base.VisitMember(node);
        }

        private void AddNotifier(object notifier)
        {
            if (notifier != null)
            {
                var item = notifier as INotifyPropertyChanged;
                if (item != null)
                {
                    Notifiers.Add(item);
                }
            }
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Object != null) // Is this a static method call?
            {
                RegisterNotifyingCollection(node.Object);
            }

            return base.VisitMethodCall(node);
        }

        private void RegisterNotifyingCollection(Expression node)
        {
            if (IsPropertyChain(node))
            {
                var expression = node as ConstantExpression;
                if (expression != null)
                {
                    AddNotifier(expression.Value);
                }
                else if (node is MemberExpression)
                {
                    MemberInfo member = ((MemberExpression)node).Member;
                    var info = member as PropertyInfo;
                    if (info != null && !PropNames.Contains(info.Name))
                    {
                        var prop = info;
                        PropNames.Add(prop.Name);
                    }
                }
            }
        }

        private static bool IsPropertyChain(Expression node)
        {
            while (true)
            {
                switch (node.NodeType)
                {
                    case ExpressionType.Constant:
                        return true;

                    case ExpressionType.MemberAccess:
                        var memberExpr = (MemberExpression)node;
                        node = memberExpr.Expression;
                        break;

                    default:
                        return false;
                }
            }
        }

        private static object GetValue(MemberExpression member)
        {
            Stack<MemberExpression> expressions = new Stack<MemberExpression>();
            var tmpMember = member;
            expressions.Push(tmpMember);
            while (tmpMember.Expression is MemberExpression)
            {
                var innerExpression = (MemberExpression)tmpMember.Expression;
                expressions.Push(innerExpression);
                tmpMember = innerExpression;
            }

            object ret = null;
            while (expressions.Count > 0)
            {
                var currMember = expressions.Pop();
                var objectMember = Expression.Convert(currMember, typeof(object));
                var getterLambda = Expression.Lambda<Func<object>>(objectMember);
                var getter = getterLambda.Compile();
                ret = getter();
                if (ret == null)
                {
                    break;
                }
            }
            return ret;
        }
    }
}