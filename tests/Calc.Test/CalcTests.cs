using CalcLib;
using Xunit;

namespace Calc.Test;

public class CalcTests
{
    [Fact]
    public void CanConstuctWithJustName()
    {
        const string Name = "Meee!";

        var calc = new CalcLib.Calc(Name);

        Assert.Equal(Name, calc.Name);
        Assert.True(calc.AutoId);

        var guid = calc.Id;
        Assert.NotEqual(Guid.Empty, guid);
    }

    [Fact]
    public void CanConstructWithId()
    {
        const string Name = "Meee!";
        Guid AGuid = Guid.Empty;

        var calc = new CalcLib.Calc(Name, AGuid);

        Assert.Equal(Name, calc.Name);
        Assert.False(calc.AutoId);

        var guid = calc.Id;
        Assert.Equal(AGuid, guid);
    }

    [Fact]
    public void ToStringTest()
    {
        const string Name = "Meee!";

        var calc = new CalcLib.Calc(Name);

        var toString = calc.ToString();

        Assert.Contains($"Name={Name}", toString);
        Assert.Contains("AutoId=True", toString);
    }
}
