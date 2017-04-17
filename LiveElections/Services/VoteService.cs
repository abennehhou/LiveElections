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
    public class VoteService
    {
        private ILog _logger = LogManager.GetLogger(typeof(CandidateService));

        public async Task<string> Vote(string userId, string candidateId)
        {
            if (await HasVoted(userId))
                throw new Exception($"The user {userId} already voted.");

            var dbVote = new Vote
            {
                UserId = userId,
                CandidateId = candidateId
            };

            await MvcApplication.ElectionsContext.Votes.InsertOneAsync(dbVote);
            var id = dbVote._id.ToString();
            _logger.Debug($"Vote {id} created.");

            return id;
        }

        public async Task<bool> HasVoted(string userId)
        {
            var existingVote = await MvcApplication.ElectionsContext.Votes
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.UserId == userId);

            return existingVote != null;
        }

        public async Task<int> CountVotes(string candidateId)
        {
            return await MvcApplication.ElectionsContext.Votes
                .AsQueryable()
                .CountAsync(x => x.CandidateId == candidateId);
        }

        public async Task<int> CountVotes()
        {
            return await MvcApplication.ElectionsContext.Votes
                .AsQueryable()
                .CountAsync();
        }

        public async Task<Dictionary<string, int>> GetNbVotesByCandidate()
        {
            var candidateIdWithCountVotes = await MvcApplication.ElectionsContext.Votes.AsQueryable()
                .GroupBy(x => x.CandidateId)
                .Select(g => new { g.Key, Count = g.Count() })
                .ToListAsync();

            return candidateIdWithCountVotes.ToDictionary(x => x.Key, y => y.Count);

        }

    }
}
