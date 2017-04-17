using MongoDB.Bson;

namespace LiveElections.Domain
{
    public class Vote
    {
        public ObjectId _id { get; set; }
        public string UserId { get; set; }
        public string CandidateId { get; set; }
    }
}
