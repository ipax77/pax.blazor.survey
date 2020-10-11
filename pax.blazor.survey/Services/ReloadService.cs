using Microsoft.Extensions.Caching.Memory;
using Microsoft.JSInterop;
using pax.blazor.survey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pax.blazor.survey.Services
{
    /// <summary>
    /// Saves User/Survey Object in memory to catch connection lost szenarios
    /// </summary>
    public class ReloadService
    {
        private IMemoryCache cache;
        MemoryCacheEntryOptions options;

        public ReloadService(IMemoryCache cache)
        {
            this.cache = cache;
            options = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30));
        }

        public void SetUser(User user)
        {
            cache.Set(user.Name, user);
        }

        public User GetUser(string name)
        {
            User user = null;
            cache.TryGetValue(name, out user);
            return user;
        }

        public void ClearUser(string name)
        {
            if (cache.TryGetValue(name, out _))
                cache.Remove(name);
        }

        public void SetSurvey(Survey survey, string name)
        {
            cache.Set(name + "survey", survey);
        }

        public Survey GetSurvey(string name)
        {
            Survey survey = null;
            cache.TryGetValue(name + "survey", out survey);
            return survey;
        }

        public void ClearSurvey(string name)
        {
            if (cache.TryGetValue(name + "survey", out _))
                cache.Remove(name + "survey");
        }
    }
}
