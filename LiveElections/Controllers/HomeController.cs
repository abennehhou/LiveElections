using LiveElections.Models;
using LiveElections.Services;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;

namespace LiveElections.Controllers
{
    public class HomeController : Controller
    {
        private VoteService _voteService;
        private CandidateService _candidateService;

        public HomeController()
        {
            _voteService = new VoteService();
            _candidateService = new CandidateService();
        }

        public async Task<ActionResult> Index()
        {
            var model = new ElectionSummaryModel();
            model.NumberOfVotes = await _voteService.CountVotes();
            model.NumberOfCandidates = await _candidateService.CountCandidates();
            var nbVotesByCandidate = await _voteService.GetNbVotesByCandidate();

            model.TopCandidates = new List<CandidateModel>();
            if (nbVotesByCandidate.Any(x => x.Value != 0))
            {
                var topCandidateIds = nbVotesByCandidate
                    .Where(x => x.Value == nbVotesByCandidate.Values.Max())
                    .Select(x => x.Key)
                    .ToList();

                foreach (var topCandidateId in topCandidateIds)
                {
                    var candidate = await _candidateService.GetCandidateById(topCandidateId);
                    model.TopCandidates.Add(candidate);
                }
            }

            return View(model);
        }
    }
}
