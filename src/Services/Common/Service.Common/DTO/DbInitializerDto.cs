namespace Service.Common.DTO;

public class DbInitializerDto
{
    public bool Recreate { get; set; }
    public bool AddTestData { get; set; }
    public bool IsFunctionalTest { get; set; }
    public string Type { get; set; }
}