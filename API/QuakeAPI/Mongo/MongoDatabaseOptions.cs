namespace QuakeAPI.Mongo
{
    public class MongoDatabaseOptions
    {
        public const string MongoDB = "MongoDB";

        public string ConnectionString {get;set;} = null!;
        public string DatabaseName {get;set;} = null!;
        public string LogsCollectionName {get;set;} = null!;
    }
}