namespace LiveElections.Models
{
    public class CandidateModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }

        public int? NumberOfVotes { get; set; }
    }
}