using System.Linq.Expressions;
using System.Reflection;
using Sprache;


namespace CalcLib;
public static class ExpressionParser
{
    public static Expression<Func<double>> ParseExpression(string text)
    {
        return Lambda.Parse(text);
    }

    // ========================================================================

    static Parser<ExpressionType> Operator(string op, ExpressionType opType)
    {
        return Parse.String(op).Token().Return(opType);
    }

    static readonly Parser<ExpressionType> Add = Operator("+", ExpressionType.AddChecked);
    static readonly Parser<ExpressionType> Subtract = Operator("-", ExpressionType.SubtractChecked);
    static readonly Parser<ExpressionType> Multiply = Operator("*", ExpressionType.MultiplyChecked);
    static readonly Parser<ExpressionType> Divide = Operator("/", ExpressionType.Divide);
    static readonly Parser<ExpressionType> Modulo = Operator("%", ExpressionType.Modulo);
    static readonly Parser<ExpressionType> Power = Operator("^", ExpressionType.Power);

    // ========================================================================

    static readonly Parser<Expression> Function =
        from name in Parse.Letter.AtLeastOnce().Text()
        from lparen in Parse.Char('(')
        from expr in Parse.Ref(() => Expr).DelimitedBy(Parse.Char(',').Token())
        from rparen in Parse.Char(')')
        select CallFunction(name, expr.ToArray());

    static Expression CallFunction(string name, Expression[] parameters)
    {
        var methodInfo = typeof(Math).GetTypeInfo().GetMethod(name, parameters.Select(e => e.Type).ToArray());
        if (methodInfo == null)
            throw new ParseException(string.Format("Function '{0}({1})' does not exist.", name,
                                                   string.Join(",", parameters.Select(e => e.Type.Name))));

        var returnType = methodInfo.ReturnType;

        if (returnType == typeof(int) || returnType == typeof(bool))
        {
            return Expression.Convert(Expression.Call(methodInfo, parameters), typeof(double));
        }
        else
        {
            return Expression.Call(methodInfo, parameters);
        }
    }

    // ========================================================================

    static Dictionary<string, bool> KnownMathConstants = new Dictionary<string, bool>();

    static bool isMathConstant(string name)
    {
        if (!KnownMathConstants.ContainsKey(name))
        {
            var c = typeof(Math)
                        .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                        .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.Name == name);


            KnownMathConstants[name] = c.Count() == 1;
        }

        return KnownMathConstants[name];
    }

    static readonly Parser<Expression> MathConstant =
        from name in Parse.Letter.AtLeastOnce().Token().Text()
        where isMathConstant(name)
        select GetMathConstant(name);

    static Expression GetMathConstant(string name)
    {
        var c = typeof(Math)
                    .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                    .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.Name == name);

        if (c.Count() == 0)
            throw new ParseException(string.Format("Math Constant '{0}' does not exist.", name));

        var it = c.First();
        var theConst = it.GetRawConstantValue() ?? 0;

        // Return the Field to that we can see the Constant being used
        return Expression.Field(null, it);
    }

    // ========================================================================

    static readonly Parser<Expression> Constant =
         Parse.Decimal
         .Select(x => Expression.Constant(double.Parse(x)))
         .Named("number");

    static readonly Parser<Expression> Factor =
        (from lparen in Parse.Char('(')
         from expr in Parse.Ref(() => Expr)
         from rparen in Parse.Char(')')
         select expr).Named("expression")
        .XOr(Constant)
        .XOr(MathConstant)
        .XOr(Function);


    static readonly Parser<Expression> Operand =
        ((from sign in Parse.Char('-')
          from factor in Factor
          select Expression.Negate(factor)
         ).XOr(Factor)).Token();

    static readonly Parser<Expression> InnerTerm = Parse.ChainRightOperator(Power, Operand, Expression.MakeBinary);

    static readonly Parser<Expression> Term = Parse.ChainOperator(Multiply.Or(Divide).Or(Modulo), InnerTerm, Expression.MakeBinary);

    static readonly Parser<Expression> Expr = Parse.ChainOperator(Add.Or(Subtract), Term, Expression.MakeBinary);

    // ========================================================================

    static readonly Parser<Expression<Func<double>>> Lambda =
        Expr.Token().End().Select(body => Expression.Lambda<Func<double>>(body));
}
