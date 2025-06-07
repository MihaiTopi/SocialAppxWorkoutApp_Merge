namespace ServerLibraryProject.Services
{
    using System;
    using System.Collections.Generic;
    using Workout.Core.IRepositories;
    using Workout.Core.IServices;
    using Workout.Core.Models;

    public class ReactionService : IReactionService
    {
        private readonly IReactionRepository reactionRepository;

        public ReactionService(IReactionRepository reactionRepository)
        {
            this.reactionRepository = reactionRepository;
        }

        public async Task AddReaction(Reaction reaction)
        {
            try
            {
                Reaction oldReaction = await this.reactionRepository.GetReaction(reaction.UserId, reaction.PostId);
                if (oldReaction.Type == reaction.Type)
                {
                    await this.reactionRepository.Delete(reaction.UserId, reaction.PostId);
                }
                else
                {
                    await this.reactionRepository.Update(reaction.UserId, reaction.PostId, reaction.Type);
                }
            }
            catch (Exception ex)
            {
                await this.reactionRepository.Add(reaction);
            }
        }



        //public void DeleteReaction(long userId, long postId)
        //{
        //    Reaction reaction = reactionRepository.GetReaction(userId, postId);

        //    reactionRepository.Delete(userId, postId);
        //}

        //public List<Reaction> GetAllReactions()
        //{
        //    return reactionRepository.GetAllReactions();
        //}

        public async Task<List<Reaction>> GetReactionsByPostId(long postId)
        {
            return await reactionRepository.GetReactionsByPostId(postId);
        }

        public async Task<Reaction> GetReaction(int userId, long postId)
        {
            return await this.reactionRepository.GetReaction(userId, postId);
        }

        //public void UpdateReaction(Reaction reaction)
        //{
        //    reactionRepository.GetReaction(reaction.UserId, reaction.PostId);

        //    this.reactionRepository.Update(reaction.UserId, reaction.PostId, reaction.Type);
        //}
    }
}
