namespace AngularBffSample.Bff;

public class ToDo
{
    static int _nextId = 1;
    public static int NewId()
    {
        return _nextId++;
    }

    public int Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public string? Name { get; set; }
    public string? User { get; set; }
}