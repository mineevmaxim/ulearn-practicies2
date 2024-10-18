using System.Linq.Expressions;

namespace Reflection.Differentiation;

public class Algebra
{
    public static Expression<Func<double, double>> Differentiate(Expression<Func<double, double>> function)
    {
        if (function.Body is ConstantExpression constantExpression)
            return function;
        else if (function.Body is ParameterExpression parameterExpression)
        {
            return Expression.Constant(1);
        }
        else if (function.Body is BinaryExpression binaryExpression)
            return function;
        else throw new NotImplementedException();
    }
}