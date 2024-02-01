namespace SeedData.models;

public class Country
{
    public Guid Id { get; set; }
    
    public string? Name { get; set; }
    
    public Location? Location { get; set; }
    
    public Currency? Currency { get; set; }
    
    public Language? Language { get; set; }
}