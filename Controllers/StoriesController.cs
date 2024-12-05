using HackerNewsBestStories.Services;
using Microsoft.AspNetCore.Mvc;

namespace HackerNewsBestStories.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoriesController : ControllerBase
    {
        private readonly HackerNewsService _hackerNewsService;

        public StoriesController(HackerNewsService hackerNewsService)
        {
            _hackerNewsService = hackerNewsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBestStories([FromQuery] int n = 10)
        {
            if (n <= 0)
            {
                return BadRequest("The number of stories must be greater than 0.");
            }

            var storyIds = await _hackerNewsService.GetBestStoryIdsAsync();
            var tasks = storyIds.Take(n).Select(id => _hackerNewsService.GetStoryDetailsAsync(id));
            var stories = await Task.WhenAll(tasks);

            return Ok(stories.Where(s => s != null).OrderByDescending(s => s.Score).Take(n));
        }
    }
}
