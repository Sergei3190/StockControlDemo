namespace FileStorage.API.Infrastructure.Settings;

public class MongoDbSettings
{
    public const string SectionName = "MongoDB";

    public string Type { get; set; }

    public string Local { get; set; }

    public string DockerDb { get; set; }

    public string Catalog { get; set; }

    public string DefaultCollection { get; set; }
}
