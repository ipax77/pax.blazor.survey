﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using pax.blazor.survey.Db;
using pax.blazor.survey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pax.blazor.survey.Services
{
    public class DbService
    {
        private readonly SurveyContext context;
        private readonly ILogger<DbService> logger;

        public DbService(SurveyContext context, ILogger<DbService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// Returns a simplified sort list of all surveys
        /// </summary>
        public async Task<List<SurveyListItem>> GetSurveysListAsync()
        {
            var surveys = context.Surveys.Include(i => i.Questions).Include(j => j.Users).AsNoTracking();
            return await surveys.Select(s => new SurveyListItem { ID = s.ID, Pos = s.Pos, Title = s.Title, Questions = s.Questions.Count(), Users = s.Users.Count() }).ToListAsync();
        }

        /// <summary>
        /// Returns a simplified sort list of all survey questions
        /// </summary>
        public async Task<List<QuestionListItem>> GetSurveyQuestionsAsync(Survey survey)
        {
            var questions = context.Questions.Include(i => i.Responses).Where(x => x.Surveys.Contains(survey)).AsNoTracking();
            return await questions.Select(s => new QuestionListItem { ID = s.ID, Pos = s.Pos, Interview = s.Interview, Type = s.Type, Responses = s.Responses.Count() }).ToListAsync();
        }

        /// <summary>
        /// Returns a tracked survey database object
        /// </summary>
        public async Task<Survey> GetSurveyAsync(int id)
        {
            Survey survey = await context.Surveys.Include(i => i.Questions).ThenInclude(k => k.Options).Include(j => j.Users).FirstOrDefaultAsync(f => f.ID == id);
            return survey;
        }

        /// <summary>
        /// Returns a tracked survey database object
        /// </summary>
        public async Task<Survey> GetSurveyAsync(string url)
        {
            Survey survey = null;
            if (String.IsNullOrEmpty(url))
                survey = await context.Surveys.Include(i => i.Questions).ThenInclude(k => k.Options).Include(j => j.Users).FirstOrDefaultAsync();
            else
                survey = await context.Surveys.Include(i => i.Questions).ThenInclude(k => k.Options).Include(j => j.Users).FirstOrDefaultAsync(f => f.SubUrl == url);
            return survey;
        }

        /// <summary>
        /// Returns a tracked question database object
        /// </summary>
        public async Task<Question> GetQuestionAsync(int id)
        {
            return await context.Questions.Include(i => i.Options).FirstOrDefaultAsync(f => f.ID == id);
        }

        /// <summary>
        /// Saves the modified or created Survey
        /// </summary>
        public async Task<bool> SaveSurveyAsync(Survey survey)
        {
            foreach (var question in survey.Questions.ToArray())
            {
                if (String.IsNullOrEmpty(question.Interview))
                    survey.Questions.Remove(question);
            }
            if (!survey.Questions.Any())
                return false;

            if (survey.ID == 0)
                context.Surveys.Add(survey);
            await context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Gets or creates a User Database Object
        /// </summary>
        public async Task<User> GetUserAsync(Survey survey, string username, string ip, string agent)
        {
            string name = String.Empty;
            if (!String.IsNullOrEmpty(username))
                name = username;
            else if (survey.AllowAnonymouse)
            {
                // try to avoid multiple submissions from one person without privacy conflicts
                name = CreateMD5(ip + agent);
            }

            if (String.IsNullOrEmpty(name))
                return null;

            User user = null;
            user = await context.Users.Include(s => s.Surveys).Include(r => r.Responses).ThenInclude(t => t.Answers).FirstOrDefaultAsync(x => x.Name == name);
            if (user == null)
            {
                user = new User() { Name = name };
            }
            if (user.Surveys == null)
                user.Surveys = new List<Survey>() { survey };
            else if (!user.Surveys.Contains(survey))
                user.Surveys.Add(survey);
            else
                logger.LogInformation($"User {name} again on Survey {survey.Title}");

            if (user.Responses == null)
                user.Responses = new List<Response>();

            return user;
        }

        public async Task<bool> SaveUserAsync(User user)
        {
            if (user.ID == 0)
                context.Users.Add(user);

            await context.SaveChangesAsync();
            return true;
        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
