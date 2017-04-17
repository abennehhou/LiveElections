using System.Configuration;
using LiveElections.Domain;
using log4net;
using MongoDB.Driver;

namespace LiveElections
{
    public class ElectionsContext
    {
        private ILog _logger = LogManager.GetLogger(typeof(ElectionsContext));

        public IMongoDatabase ElectionsDatabase;

        private const string AppSettingsKeyConnectionString     = "MongoDBConnectionString";
        private const string AppSettingsKeyDatabase             = "MongoDBLiveElectionsDatabase";
        private const string AppSettingsKeyCollectionCandidates = "MongoDBCollectionCandidates";

        private readonly string CandidatesCollectionName;

        public ElectionsContext()
        {
            // The context is thread safe and can be injected with IOC, can be static.
            var connectionString = ConfigurationManager.AppSettings[AppSettingsKeyConnectionString];
            var settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            var client = new MongoClient(settings);
            _logger.Debug(string.Format("Mongo Client initialized with connection string {0}.", connectionString));

            var databaseName = ConfigurationManager.AppSettings[AppSettingsKeyDatabase];
            ElectionsDatabase = client.GetDatabase(databaseName);
            CandidatesCollectionName = ConfigurationManager.AppSettings[AppSettingsKeyCollectionCandidates];

            _logger.Debug(string.Format("Connection to database '{0}' initialized.", databaseName));
        }

        public IMongoCollection<Candidate> Candidates => ElectionsDatabase.GetCollection<Candidate>(CandidatesCollectionName);
    }
}
