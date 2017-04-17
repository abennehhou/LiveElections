using System.Threading.Tasks;
using System.Web.Mvc;
using LiveElections.Models;
using LiveElections.Services;
using Microsoft.AspNet.Identity;

namespace LiveElections.Controllers
{
    public class VoteController : Controller
    {
        private VoteService _voteService;
        private CandidateService _candidateService;

        public VoteController()
        {
            _voteService = new VoteService();
            _candidateService = new CandidateService();
        }

        public async Task<ActionResult> Index()
        {
            var model = new VoteSummaryModel();
            var userId = GetCurrentUserId();
            var hasVoted = await _voteService.HasVoted(userId);
            model.HasVoted = hasVoted;
            var candidates = await _candidateService.GetAllCandidates();
            model.Candidates = candidates;

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Vote(string candidate)
        {
            var userId = User.Identity.GetUserId();
            var voteId = await _voteService.Vote(userId, candidate);

            return RedirectToAction("Index");
        }

        private string GetCurrentUserId()
        {
            return User.Identity.GetUserId();
        }
    }
}
