using CalcLib;
using Sprache;

Console.Clear();

const string QUIT = "q";
const double EPSILON = 1e-8;

var line = String.Empty;

var roundToZero = (double d) => Math.Abs(d) < EPSILON ? 0 : d;

do
{
    Console.Write($"{Environment.NewLine}Expression or '{QUIT}' to quit > ");

    line = Console.ReadLine() ?? QUIT;
    if (line != QUIT)
    {
        try
        {
            var parsed = ExpressionParser.ParseExpression(line);

            var visitor = new LambdaVisitor(parsed, new List<string>());
            Console.WriteLine("Details:");
            visitor.Visit("> ");
            Console.WriteLine(visitor.ToString());
            Console.WriteLine();

            var result = roundToZero(parsed.Compile()());

            Console.WriteLine("Result:");
            if (result != Math.Floor(result))
            {
                Console.WriteLine("({0})() = {1:G}", parsed, result);
            }
            else
            {
                Console.WriteLine("({0})() = {1}", parsed, result);
            }
        }
        catch (ParseException ex)
        {
            Console.WriteLine("ERROR: {0}", ex.Message);
        }
    }
} while (line != "q");
