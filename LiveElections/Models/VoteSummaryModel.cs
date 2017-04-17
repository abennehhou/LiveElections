using System.Collections.Generic;

namespace LiveElections.Models
{
    public class VoteSummaryModel
    {
        public bool HasVoted { get; set; }

        public List<CandidateModel> Candidates { get; set; }
    }
}
