using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LyfeApp.Data.Models;

namespace LyfeApp.Data.Services
{
    public interface IStoriesService
    {
        Task<StoryModel> CreateStoryAsync(StoryModel story);
    }
}