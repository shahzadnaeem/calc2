namespace CalcLib;

public class Calc
{
    public string Name { get; set; }
    public Guid Id { get; }
    public bool AutoId { get; } = false;

    public Calc(string name)
    {
        Name = name;
        Id = System.Guid.NewGuid();
        AutoId = true;
    }

    public Calc(string name, Guid id)
    {
        Name = name;
        Id = id;
        AutoId = false;
    }

    public override string ToString()
    {
        var className = typeof(Calc).Name;

        return String.Format($"class {className}: (Name={Name},Id={Id},AutoId={AutoId})");
    }
}
