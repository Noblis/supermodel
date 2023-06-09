﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Supersonic.Linq
{
    internal static class Evaluator
    {
        #region Embedded Types
        class SubtreeEvaluator : ExpressionVisitor
        {
            #region Methods
            internal SubtreeEvaluator(HashSet<Expression> candidates)
            {
                _candidates = candidates;
            }
            internal Expression Eval(Expression exp)
            {
                return Visit(exp);
            }
            public override Expression Visit(Expression exp)
            {
                if (exp == null) return null;
                if (_candidates.Contains(exp)) return Evaluate(exp);
                return base.Visit(exp);
            }
            private Expression Evaluate(Expression e)
            {
                if (e.NodeType == ExpressionType.Constant) return e;
                var lambda = Expression.Lambda(e);
                var fn = lambda.Compile();
                return Expression.Constant(fn.DynamicInvoke(null), e.Type);
            }
            #endregion

            #region Fields
            readonly HashSet<Expression> _candidates;
            #endregion

        }
        class Nominator : ExpressionVisitor
        {
            #region Methods
            internal Nominator(Func<Expression, bool> fnCanBeEvaluated)
            {
                _fnCanBeEvaluated = fnCanBeEvaluated;
            }
            internal HashSet<Expression> Nominate(Expression expression)
            {
                _candidates = new HashSet<Expression>();
                Visit(expression);
                return _candidates;
            }
            public override Expression Visit(Expression expression)
            {
                if (expression != null)
                {
                    var saveCannotBeEvaluated = _cannotBeEvaluated;
                    _cannotBeEvaluated = false;
                    base.Visit(expression);
                    if (!_cannotBeEvaluated)
                    {
                        if (_fnCanBeEvaluated(expression)) _candidates.Add(expression);
                        else _cannotBeEvaluated = true;
                    }
                    _cannotBeEvaluated |= saveCannotBeEvaluated;
                }
                return expression;
            }
            #endregion

            #region Fields
            readonly Func<Expression, bool> _fnCanBeEvaluated;
            HashSet<Expression> _candidates;
            bool _cannotBeEvaluated;
            #endregion
        }
        #endregion

        #region Methods
        public static Expression PartialEval(Expression expression, Func<Expression, bool> fnCanBeEvaluated)
        {
            return new SubtreeEvaluator(new Nominator(fnCanBeEvaluated).Nominate(expression)).Eval(expression);
        }
        public static Expression PartialEval(Expression expression)
        {
            return PartialEval(expression, CanBeEvaluatedLocally);
        }
        private static bool CanBeEvaluatedLocally(Expression expression)
        {
            return expression.NodeType != ExpressionType.Parameter;
        }
        #endregion
    }
}