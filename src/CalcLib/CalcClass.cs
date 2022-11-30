namespace CalcLib;

public class CalcClass
{
    public string Name { get; set; }
    public Guid Id { get; }
    public bool AutoId { get; } = false;

    public CalcClass(string name)
    {
        Name = name;
        Id = System.Guid.NewGuid();
        AutoId = true;
    }

    public CalcClass(string name, Guid id)
    {
        Name = name;
        Id = id;
        AutoId = false;
    }
}
