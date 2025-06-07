namespace ServerLibraryProject.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Workout.Core.Data;
    using Workout.Core.Enums;
    using Workout.Core.IRepositories;
    using Workout.Core.Models;


    /// <summary>
    /// Repository for managing reactions.
    /// </summary>
    public class ReactionRepository : IReactionRepository
    {
        private readonly WorkoutDbContext dbContext;

        public ReactionRepository(WorkoutDbContext context)
        {
            dbContext = context;
        }

        /// <summary>
        /// Deletes a reaction by a specific user for a specific post.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="postId">The ID of the post.</param>
        public async Task Delete(int userId, long postId)
        {
            try
            {
                var reactionDeleted = await (from reaction in dbContext.Reactions
                                             where reaction.PostId == postId && reaction.UserId == userId
                                             select reaction).FirstOrDefaultAsync();
                if (reactionDeleted != null)
                {
                    dbContext.Remove(reactionDeleted);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch
            {
                throw new Exception("Error deleting the reaction");
            }

        }

        /// <summary>
        /// Retrieves all reactions.
        /// </summary>
        /// <returns>A list of all reactions.</returns>
        //public List<Reaction> GetAllReactions()
        //{
        //    return this.dbContext.Reactions.ToList();
        //}

        /// <summary>
        /// Retrieves all reactions for a specific post.
        /// </summary>
        /// <param name="postId">The ID of the post.</param>
        /// <returns>A list of reactions for the specified post.</returns>
        public Task<List<Reaction>> GetReactionsByPostId(long postId)
        {
            var reactionsQuery = from reaction in dbContext.Reactions
                                 where reaction.PostId == postId
                                 select reaction;

            return reactionsQuery.ToListAsync();
        }

        /// <summary>
        /// Retrieves a reaction by a specific user for a specific post.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="postId">The ID of the post.</param>
        /// <returns>The reaction for the specified user and post.</returns>
        public async Task<Reaction> GetReaction(int userId, long postId)
        {
            try
            {
                var reactionReturned = await (from reaction in dbContext.Reactions
                                              where reaction.PostId == postId && reaction.UserId == userId
                                              select reaction).FirstOrDefaultAsync();
                return reactionReturned;
            }
            catch
            {
                throw new Exception("Error retrieving the reaction by user and post");
            }

        }

        /// <summary>
        /// Saves a new reaction to the repository.
        /// </summary>
        /// <param name="entity">The reaction entity to save.</param>
        public async Task Add(Reaction entity)
        {
            try
            {
                await dbContext.Reactions.AddAsync(entity);
                await dbContext.SaveChangesAsync();
            }
            catch
            {
                throw new Exception("Error saving the reaction");
            }
        }

        /// <summary>
        /// Updates the reaction type for a specific user and post.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="postId">The ID of the post.</param>
        /// <param name="type">The new reaction type.</param>
        public async Task Update(int userId, long postId, ReactionType type)
        {
            try
            {
                var reaction = await dbContext.Reactions.FirstOrDefaultAsync(r => r.PostId == postId && r.UserId == userId);

                if (reaction != null)
                {
                    reaction.Type = type;
                    await dbContext.SaveChangesAsync();
                }
            }
            catch
            {
                throw new Exception("Error updating the reaction type");
            }

        }
    }
}