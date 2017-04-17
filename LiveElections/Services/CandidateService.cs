using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveElections.Domain;
using LiveElections.Models;
using log4net;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace LiveElections.Services
{
    public class CandidateService
    {
        private ILog _logger = LogManager.GetLogger(typeof(CandidateService));

        public async Task<string> CreateCandidate(CreateCandidateModel candidate)
        {
            var dbCandidate = new Candidate
            {
                FirstName = candidate.FirstName,
                LastName = candidate.LastName,
                Description = candidate.Description
            };

            await MvcApplication.ElectionsContext.Candidates.InsertOneAsync(dbCandidate);
            var id = dbCandidate._id.ToString();
            _logger.Debug($"Candidate {id} created.");

            return id;
        }

        public async Task<bool> UpdateCandidate(string id, UpdateCandidateModel candidate)
        {
            _logger.Debug($"Updating candidate {id}");
            var objectId = ObjectId.Parse(id);

            if (candidate == null)
                throw new Exception($"Candidate not provided for id={id}");

            if (id != candidate.Id)
                throw new Exception($"Id {id} is different from candidate's id {candidate.Id}");

            var modificationUpdate = Builders<Candidate>.Update
                .Set(x => x.FirstName, candidate.FirstName)
                .Set(x => x.LastName, candidate.LastName)
                .Set(x => x.Description, candidate.Description);

            var result = await MvcApplication.ElectionsContext.Candidates.UpdateOneAsync(x => x._id == objectId, modificationUpdate);
            _logger.Debug($"Candidate {id} updated. isAcknownledged?: {result.IsAcknowledged}.");

            return result.IsAcknowledged;
        }



        public async Task<bool> DeleteCandidate(string id)
        {
            _logger.Debug($"Deleting candidate {id}");
            var objectId = new ObjectId(id);
            var result = await MvcApplication.ElectionsContext.Candidates.DeleteOneAsync(x => x._id == objectId);
            _logger.Debug($"Candidate {id} deleted. isAcknownledged?: {result.IsAcknowledged}.");
            return result.IsAcknowledged;
        }

        public async Task<CandidateModel> GetCandidateById(string id)
        {
            var objectId = ObjectId.Parse(id);
            var dbCandidate = await MvcApplication.ElectionsContext.Candidates
                .AsQueryable()
                .Where(x => x._id == objectId)
                .FirstOrDefaultAsync();

            var candidate = new CandidateModel
            {
                Id = dbCandidate._id.ToString(),
                FirstName = dbCandidate.FirstName,
                LastName = dbCandidate.LastName,
                Description = dbCandidate.Description
            };

            return candidate;
        }

        public async Task<List<CandidateModel>> GetAllCandidates()
        {
            var dbCandidates = await MvcApplication.ElectionsContext.Candidates
                .AsQueryable()
                .ToListAsync();

            var candidates = dbCandidates.Select(dbCandidate => new CandidateModel
            {
                Id = dbCandidate._id.ToString(),
                FirstName = dbCandidate.FirstName,
                LastName = dbCandidate.LastName,
                Description = dbCandidate.Description
            }).ToList();

            return candidates;
        }
    }
}
