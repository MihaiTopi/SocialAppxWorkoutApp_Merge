namespace NeoIsisJob.Proxy
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using Workout.Core.IServices;
    using Workout.Core.Models;

    public class ReactionServiceProxy : IReactionService
    {
        private readonly HttpClient httpClient;

        public ReactionServiceProxy()
        {
            this.httpClient = new HttpClient();
            this.httpClient.BaseAddress = new Uri("http://localhost:5261/api/reactions/");
        }

        public async Task<List<Reaction>> GetReactionsByPostId(long postId)
        {
            var client = new HttpClient();
            var response = await client.GetAsync($"http://localhost:5261/api/posts/{postId}/reactions");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Reaction>>() ?? new List<Reaction>();
            }

            return new List<Reaction>();
        }

        public async Task<Reaction?> GetReaction(int userId, long postId)
        {
            var response = await this.httpClient.GetAsync($"{userId}/{postId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(content))
                {
                    var reaction = await response.Content.ReadFromJsonAsync<Reaction>();
                    return reaction;
                }
            }

            return null;
        }

        public async Task AddReaction(Reaction reaction)
        {
            var response = await this.httpClient.PostAsJsonAsync(string.Empty, reaction);
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            throw new Exception($"Failed to add reaction: {response.StatusCode}");
        }
    }
}
