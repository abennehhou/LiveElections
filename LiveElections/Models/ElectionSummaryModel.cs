using System.Collections.Generic;

namespace LiveElections.Models
{
    public class ElectionSummaryModel
    {
        public int NumberOfVotes { get; set; }
        public int NumberOfCandidates { get; set; }
        public List<CandidateModel> TopCandidates { get; set; }
    }
}
