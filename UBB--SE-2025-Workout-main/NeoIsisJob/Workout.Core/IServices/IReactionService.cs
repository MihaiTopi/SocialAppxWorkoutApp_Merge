using Workout.Core.Models;

namespace Workout.Core.IServices
{
    public interface IReactionService
    {
        //List<Reaction> GetAllReactions();

        Task<List<Reaction>> GetReactionsByPostId(long postId);

        Task AddReaction(Reaction reaction);

        Task<Reaction> GetReaction(int userId, long postId);
    }
}