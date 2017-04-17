using System;
using LiveElections.Models;
using MongoDB.Driver;
using System.Configuration;

namespace LiveElections
{
    public class ApplicationIdentityContext : IDisposable
    {
        private const string AppSettingsKeyConnectionString = "MongoDBConnectionString";
        private const string AppSettingsKeyDatabase         = "MongoDBLiveElectionsDatabase";
        private const string AppSettingsKeyCollectionUsers  = "MongoDBCollectionUsers";

        public static ApplicationIdentityContext Create()
        {
            var connectionString    = ConfigurationManager.AppSettings[AppSettingsKeyConnectionString];
            var databaseName        = ConfigurationManager.AppSettings[AppSettingsKeyDatabase];
            var usersCollectionName = ConfigurationManager.AppSettings[AppSettingsKeyCollectionUsers];

            var client   = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            var users    = database.GetCollection<ApplicationUser>(usersCollectionName);

            return new ApplicationIdentityContext(users);
        }

        private ApplicationIdentityContext(IMongoCollection<ApplicationUser> users)
        {
            Users = users;
        }

        public IMongoCollection<ApplicationUser> Users { get; set; }

        public void Dispose()
        {
        }
    }
}
