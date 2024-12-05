using HackerNewsBestStories.Models;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNewsBestStories.Services
{
    public class HackerNewsService
    {
        private readonly HttpClient _httpClient;
        private const string BestStoriesUrl = "https://hacker-news.firebaseio.com/v0/beststories.json";
        private const string StoryDetailsUrl = "https://hacker-news.firebaseio.com/v0/item/{0}.json";
        private readonly IMemoryCache _cache;
        private const string BestStoriesCacheKey = "BestStoriesIds";
        private const string StoryCacheKeyPrefix = "StoryDetails_";

        public HackerNewsService(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
        }

        // Método para recuperar los mejores identificadores de historias, con almacenamiento en caché
        public async Task<List<int>> GetBestStoryIdsAsync()
        {
            return await _cache.GetOrCreateAsync(BestStoriesCacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10); // Cache por 10 minutos
                return await _httpClient.GetFromJsonAsync<List<int>>(BestStoriesUrl);
            });
        }

        // Método para recuperar detalles de historias individuales, con almacenamiento en caché
        public async Task<Story> GetStoryDetailsAsync(int storyId)
        {
            string cacheKey = $"{StoryCacheKeyPrefix}{storyId}";

            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1); // Cache por 1 hora
                var url = string.Format(StoryDetailsUrl, storyId);
                var story = await _httpClient.GetFromJsonAsync<HackerNewsStory>(url);

                if (story == null)
                {
                    return null;
                }

                return new Story
                {
                    Title = story.Title,
                    Uri = story.Url,
                    PostedBy = story.By,
                    Time = DateTimeOffset.FromUnixTimeSeconds(story.Time).UtcDateTime,
                    Score = story.Score,
                    CommentCount = story.Descendants
                };
            });
        }

        private class HackerNewsStory
        {
            public string Title { get; set; }
            public string Url { get; set; }
            public string By { get; set; }
            public long Time { get; set; }
            public int Score { get; set; }
            public int Descendants { get; set; }
        }
    }
}
