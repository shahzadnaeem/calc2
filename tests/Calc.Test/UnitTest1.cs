using CalcLib;
using Xunit;

namespace Calc.Test;

public class UnitTest1
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
}
