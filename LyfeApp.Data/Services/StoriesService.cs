using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LyfeApp.Data.Services;
using LyfeApp.Data.Models;

namespace LyfeApp.Data.Services
{
    public class StoriesService : IStoriesService
    {
        private readonly ApplicationDbContext _context;
        public StoriesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StoryModel> CreateStoryAsync(StoryModel story)
        {
            await _context.Stories.AddAsync(story);
            await _context.SaveChangesAsync();
            return story;
        }
        
    }
}