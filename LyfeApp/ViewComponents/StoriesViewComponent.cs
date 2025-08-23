using LyfeApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LyfeApp.ViewComponents
{
    public class StoriesViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public StoriesViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var allStories = await _context.Stories
            .Where(s => s.DateCreated >= DateTime.UtcNow.AddHours(-24))
            .OrderByDescending(s => s.DateCreated)
            .Include(s => s.User)
            .ToListAsync();
            return View(allStories);
        }
    }
}