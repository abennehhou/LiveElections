using System.Threading.Tasks;
using System.Web.Mvc;
using LiveElections.Models;
using LiveElections.Services;
using System;

namespace LiveElections.Controllers
{
    public class CandidateController : Controller
    {
        private CandidateService _candidateService;
        private VoteService _voteService;

        public CandidateController()
        {
            _candidateService = new CandidateService();
            _voteService = new VoteService();
        }

        public async Task<ActionResult> Index()
        {
            var candidates = await _candidateService.GetAllCandidates();
            foreach (var candidate in candidates)
            {
                candidate.NumberOfVotes = await _voteService.CountVotes(candidate.Id);
            }

            return View(candidates);
        }

        public async Task<ActionResult> Details(string id)
        {
            var details = await _candidateService.GetCandidateById(id);

            return View(details);
        }

        public ActionResult Create()
        {
            var accountModel = new CreateCandidateModel();

            return View(accountModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateCandidateModel account)
        {
            if (ModelState.IsValid)
            {
                var id = await _candidateService.CreateCandidate(account);
                return RedirectToAction("Details", "Candidate", new { id });
            }

            return View(account);
        }

        public async Task<ActionResult> Edit(string id)
        {
            var candidate = await _candidateService.GetCandidateById(id);
            var editCandidateModel = new UpdateCandidateModel
            {
                Id = candidate.Id,
                FirstName = candidate.FirstName,
                LastName = candidate.LastName
            };
            return View(editCandidateModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, UpdateCandidateModel candidate)
        {
            if (candidate == null)
                throw new Exception($"Candidate not provided for id={id}");
            if (id != candidate.Id)
                throw new Exception($"Id {id} is different from candidate's id {candidate.Id}");

            if (ModelState.IsValid)
            {
                var result = await _candidateService.UpdateCandidate(id, candidate);
                return RedirectToAction("Details", "Candidate", new { id });
            }
            return View(candidate);
        }

        public async Task<ActionResult> Delete(string id)
        {
            var account = await _candidateService.GetCandidateById(id);
            return View(account);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var result = await _candidateService.DeleteCandidate(id);
            return RedirectToAction("Index", "Candidate");
        }

    }
}
