using MongoDB.Bson;

namespace LiveElections.Domain
{
    public class Candidate
    {
        public ObjectId _id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
    }
}
