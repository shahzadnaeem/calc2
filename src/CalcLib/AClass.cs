namespace CalcLib;

public class AClass
{
    public string Name { get; set; }
    public Guid Id { get; }
    public bool AutoId { get; } = false;

    public AClass(string name)
    {
        Name = name;
        Id = System.Guid.NewGuid();
        AutoId = true;
    }

    public AClass(string name, Guid id)
    {
        Name = name;
        Id = id;
        AutoId = false;
    }

    public override string ToString()
    {
        var className = typeof(AClass).Name;

        return String.Format($"class {className}: (Name={Name},Id={Id},AutoId={AutoId})");
    }
}
