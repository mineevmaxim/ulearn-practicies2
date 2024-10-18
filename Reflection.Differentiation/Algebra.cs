﻿using System.Linq.Expressions;

namespace Reflection.Differentiation;

public class Algebra
{
    public static Expression<Func<double, double>> Differentiate(Expression<Func<double, double>> function)
        => Expression.Lambda<Func<double, double>>(Differentiate(function.Body), function.Parameters);

    private static Expression Differentiate(Expression expression)
    {
        return expression switch
        {
            ConstantExpression => Expression.Constant(0.0),
            ParameterExpression => Expression.Constant(1.0),
            BinaryExpression binaryExpression => DifferentiateBinaryExpression(binaryExpression),
            MethodCallExpression methodCallExpression when methodCallExpression.Method.Name.Contains("Sin")
                => DifferentiateSin(methodCallExpression),
            MethodCallExpression methodCallExpression when methodCallExpression.Method.Name.Contains("Cos")
                => DifferentiateCos(methodCallExpression),
            MethodCallExpression methodCallExpression => throw new ArgumentException(
                $"Function {methodCallExpression} is not supported"),
            _ => throw new ArgumentException($"Function {expression} is not supported")
        };
    }

    private static Expression DifferentiateBinaryExpression(BinaryExpression binaryExpression)
    {
        return binaryExpression.NodeType switch
        {
            ExpressionType.Add => Expression.MakeBinary(binaryExpression.NodeType, Differentiate(binaryExpression.Left),
                Differentiate(binaryExpression.Right)),
            ExpressionType.Multiply => Expression.Add(
                Expression.Multiply(Differentiate(binaryExpression.Left), binaryExpression.Right),
                Expression.Multiply(binaryExpression.Left, Differentiate(binaryExpression.Right))),
            _ => throw new ArgumentException($"Function {binaryExpression} is not supported")
        };
    }

    private static Expression DifferentiateCos(MethodCallExpression methodCallExpression)
    {
        var sin = Expression.Call(typeof(Math).GetMethod("Sin"), methodCallExpression.Arguments);
        var minusSin = Expression.MakeBinary(ExpressionType.Multiply, sin, Expression.Constant(-1.0));
        return IsComplexFunction(methodCallExpression) 
            ? Expression.Multiply(minusSin, Differentiate(methodCallExpression.Arguments[0])) 
            : minusSin;
    }

    private static Expression DifferentiateSin(MethodCallExpression methodCallExpression)
    {
        var cos = Expression.Call(typeof(Math).GetMethod("Cos"), methodCallExpression.Arguments);
        return IsComplexFunction(methodCallExpression)
            ? Expression.Multiply(cos, Differentiate(methodCallExpression.Arguments[0]))
            : cos;
    }

    private static bool IsComplexFunction(MethodCallExpression methodCallExpression)
        => methodCallExpression.Arguments[0] is BinaryExpression ||
           methodCallExpression.Arguments[0] is MethodCallExpression;
}