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
        private const string AppSettingsKeyCollectionVotes      = "MongoDBCollectionVotes";

        private readonly string CandidatesCollectionName;
        private readonly string VotesCollectionName;

        public ElectionsContext()
        {
            // The context is thread safe and can be injected with IOC, can be static.
            var connectionString = ConfigurationManager.AppSettings[AppSettingsKeyConnectionString];
            var settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            var client = new MongoClient(settings);
            _logger.Debug($"Mongo Client initialized with connection string {connectionString}.");

            var databaseName = ConfigurationManager.AppSettings[AppSettingsKeyDatabase];
            ElectionsDatabase = client.GetDatabase(databaseName);
            CandidatesCollectionName = ConfigurationManager.AppSettings[AppSettingsKeyCollectionCandidates];
            VotesCollectionName = ConfigurationManager.AppSettings[AppSettingsKeyCollectionVotes];

            _logger.Debug($"Connection to database '{databaseName}' initialized.");
        }

        public IMongoCollection<Candidate> Candidates => ElectionsDatabase.GetCollection<Candidate>(CandidatesCollectionName);
        public IMongoCollection<Vote> Votes => ElectionsDatabase.GetCollection<Vote>(VotesCollectionName);
    }
}
