using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using pax.blazor.survey.Components;
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
        private readonly ReloadService reload;
        public FeedbackComponent FeedbackComponent { get; set; }

        public DbService(SurveyContext context, ReloadService reload, ILogger<DbService> logger)
        {
            this.context = context;
            this.reload = reload;
            this.logger = logger;
        }

        /// <summary>
        /// Returns a simplified sort list of all surveys
        /// </summary>
        public async Task<List<SurveyListItem>> GetSurveysListAsync()
        {
            var surveys = context.Surveys.Include(i => i.Questions).Include(j => j.Users).AsNoTracking();
            return await surveys.Select(s => new SurveyListItem { ID = s.ID, Pos = s.Pos, Title = s.Title.Length > 30 ? s.Title.Substring(0, 27) + "..." : s.Title, Questions = s.Questions.Count(), Users = s.Users.Count() }).ToListAsync();
        }

        /// <summary>
        /// Returns a simplified sort list of all survey questions
        /// </summary>
        public async Task<List<QuestionListItem>> GetSurveyQuestionsAsync(Survey survey)
        {
            var questions = context.Questions.Include(i => i.Responses).Where(x => x.Survey == survey).AsNoTracking();
            return await questions.Select(s => new QuestionListItem { ID = s.ID, Pos = s.Pos, Interview = s.Interview, Type = s.Type, Responses = s.Responses.Count() }).ToListAsync();
        }

        /// <summary>
        /// Returns a tracked survey database object
        /// </summary>
        public async Task<Survey> GetSurveyAsync(int id, string authname = null)
        {
            Survey survey = null;
            
            if (authname != null)
                survey = reload.GetSurvey(authname);
            if (survey != null)
                return survey;

            survey = await context.Surveys.FindAsync(id);

            if (survey == null)
                return null;

            await context.Entry(survey)
                .Collection(b => b.Questions)
                .Query()
                .Include(i => i.Options)
                .LoadAsync();

            return survey;
        }

        /// <summary>
        /// Returns a untracked survey database object
        /// </summary>
        public async Task<Survey> GetSurveyAsync(string url)
        {
            Survey survey = null;
            if (String.IsNullOrEmpty(url))
                survey = await context.Surveys.FirstOrDefaultAsync();
            else
                survey = await context.Surveys.FirstOrDefaultAsync(f => f.SubUrl == url);

            if (survey == null)
                return null;

            await context.Entry(survey)
                .Collection(b => b.Questions)
                .Query()
                .Include(i => i.Options)
                .LoadAsync();

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
        public async Task<bool> SaveSurveyAsync(Survey survey, string authname)
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
            reload.ClearSurvey(authname);
            return true;
        }

        /// <summary>
        /// Gets or creates a User Database Object
        /// </summary>
        public async Task<User> GetUserAsync(Survey survey, string username, string userId)
        {
            if (survey == null)
                return null;
            string name = String.Empty;

            // try to avoid multiple submissions from one person
            if (!String.IsNullOrEmpty(username))
                name = username;
            else if (survey.AllowAnonymouse)
            {
                name = userId;
            }

            // Reload user from cache if available
            if (String.IsNullOrEmpty(name))
                return null;
            User user = reload.GetUser(name);
            if (user != null)
                return user;

            user = await context.Users.FirstOrDefaultAsync(x => x.Name == name);
            if (user == null)
            {
                user = new User() { Name = name };
            } else
            {
                await context.Entry(user)
                    .Collection(b => b.Surveys)
                    .LoadAsync();
                await context.Entry(user)
                    .Collection(b => b.Responses)
                    .Query()
                    .Include(i => i.Answers)
                    .LoadAsync();
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
        /// <summary>
        /// Gets or creates a Response Database Object
        /// </summary>
        public Response GetResponse(Survey survey, Question question, User user)
        {
            //logger.LogInformation($"GetResponse: {user.Name}: {survey.ID}|{question.ID}");
            Response response = user.Responses.FirstOrDefault(x => x.Survey.ID == survey.ID && x.Question.ID == question.ID);

            if (response != null)
                return response;
            //logger.LogInformation("create new");

            //response = context.Responses.FirstOrDefault(f => f.User == user && f.Survey == survey && f.Question == question);
            if (response == null)
            {
                response = new Response();
                response.Survey = survey;
                response.User = user;
                response.Question = question;
                response.Answers = question.Type switch
                {
                    (int)QuestionType.MultiSelect => new List<Answer>(question.Options.Select(s => new Answer() { Pos = s.Pos, Response = response, Option = s })),
                    _ => null
                };
                user.Responses.Add(response);
            }
            else
                context.Entry(response)
                    .Collection(c => c.Answers)
                    .Load();

            return response;
        }

        /// <summary>
        /// Text answers for result view
        /// </summary>
        public async Task<List<AnswerListItem>> GetTextAnswers(int questionId) {

            var answers = context.Responses.Where(x => x.Question.ID == questionId).Select(s => new AnswerListItem() { ID = s.ID, UserID = s.User.ID, Answer = s.Feedback.Length > 30 ? s.Feedback.Substring(0, 30) + "..." : s.Feedback });
            return await answers.ToListAsync();
        }

        /// <summary>
        /// full Text answer for result view
        /// </summary>
        public string GetFullText(int responseID)
        {
            return context.Responses.Find(responseID).Feedback;
        }


        /// <summary>
        /// Saves the User
        /// </summary>
        public async Task<bool> SaveUserAsync(User user, Survey survey)
        {
            if (user.ID == 0)
                context.Users.Add(user);
            
            // TODO ???
            lock (context)
            {
                // count questions and their options for new entires
                foreach (Response response in user.Responses.Where(x => x.ID == 0 && x.Survey == survey))
                {
                    if (response.Pos > 0)
                    {
                        response.Question.Count++;
                        response.Question.Options.First(f => f.Pos == response.Pos).Count++;
                    }
                    if (response.Answers != null)
                    {
                        foreach (Answer answer in response.Answers.Where(x => x.ID == 0 && x.Selected))
                            answer.Option.Count++;
                    }
                }

                // recount options for changed entires
                foreach (Response response in user.Responses.Where(x => x.Survey == survey && x.ID != 0))
                {
                    if (context.Entry(response).State == EntityState.Modified)
                    {
                        Response before = (Response)context.Entry(response).OriginalValues.ToObject();
                        response.Question.Options.First(f => f.Pos == before.Pos).Count--;
                        response.Question.Options.First(f => f.Pos == response.Pos).Count++;
                    }
                    if (response.Answers != null)
                    {
                        foreach (Answer answer in response.Answers.Where(x => x.ID > 0 && context.Entry(x).State == EntityState.Modified))
                        {
                            if (answer.Selected)
                                answer.Option.Count++;
                            else
                                answer.Option.Count--;
                        }
                    }
                }
            }
            await context.SaveChangesAsync();
            
            reload.ClearUser(user.Name);
            
            return true;
        }

        public async Task CreateAndSeedTestSurvey()
        {
            if (context.Surveys.FirstOrDefault(f => f.SubUrl == "test") != null)
                return;

            Survey survey = new Survey();
            survey.Title = "Test Survey";
            survey.Description = "test survey";
            survey.SubUrl = "test";
            survey.AllowAnonymouse = true;
            survey.ShowProgress = true;
            survey.ShowResult = true;
            survey.Expire = DateTime.Today.AddDays(30);
            survey.Questions = new List<Question>();

            Question question1 = new Question();
            question1.Interview = "What is the meaning of life?";
            question1.Type = (int)QuestionType.Text;
            question1.Survey = survey;
            question1.Pos = 1;
            Option option11 = new Option();
            option11.OptionValue = "42";
            option11.Questions = new List<Question>() { question1 };
            option11.Pos = 1;
            question1.Options = new List<Option>() { option11 };
            survey.Questions.Add(question1);

            Question question2 = new Question();
            question2.Interview = "How are you?";
            question2.Type = (int)QuestionType.Bool;
            question2.Pos = 2;
            Option option21 = new Option();
            option21.OptionValue = "good";
            option21.Questions = new List<Question>() { question2 };
            option21.Pos = 1;
            Option option22 = new Option();
            option22.OptionValue = "bad";
            option22.Questions = new List<Question>() { question2 };
            option22.Pos = 2;
            question2.Options = new List<Option>() { option21, option22 };
            survey.Questions.Add(question2);

            Question question3 = new Question();
            question3.Interview = "What is your favorite color?";
            question3.Type = (int)QuestionType.SingleSelect;
            question3.Pos = 3;
            Option option31 = new Option();
            option31.OptionValue = "red";
            option31.Questions = new List<Question>() { question3 };
            option31.Pos = 1;
            Option option32 = new Option();
            option32.OptionValue = "blue";
            option32.Questions = new List<Question>() { question3 };
            option32.Pos = 2;
            Option option33 = new Option();
            option33.OptionValue = "green";
            option33.Questions = new List<Question>() { question3 };
            option33.Pos = 3;
            Option option34 = new Option();
            option34.OptionValue = "yellow";
            option34.Questions = new List<Question>() { question3 };
            option34.Pos = 4;
            Option option35 = new Option();
            option35.OptionValue = "black";
            option35.Questions = new List<Question>() { question3 };
            option35.Pos = 5;
            question3.Options = new List<Option>() { option31, option32, option33, option34 };
            survey.Questions.Add(question3);

            Question question4 = new Question();
            question4.Interview = "What pets do you own?";
            question4.Type = (int)QuestionType.MultiSelect;
            question4.Pos = 4;
            Option option41 = new Option();
            option41.OptionValue = "cat";
            option41.Questions = new List<Question>() { question4 };
            option41.Pos = 1;
            Option option42 = new Option();
            option42.OptionValue = "dog";
            option42.Questions = new List<Question>() { question4 };
            option42.Pos = 2;
            Option option43 = new Option();
            option43.OptionValue = "mouse";
            option43.Questions = new List<Question>() { question4 };
            option43.Pos = 3;
            Option option44 = new Option();
            option44.OptionValue = "pig";
            option44.Questions = new List<Question>() { question4 };
            option44.Pos = 4;
            Option option45 = new Option();
            option45.OptionValue = "snake";
            option45.Questions = new List<Question>() { question4 };
            option45.Pos = 5;
            Option option46 = new Option();
            option46.OptionValue = "crocodile";
            option46.Questions = new List<Question>() { question4 };
            option46.Pos = 6;
            question4.Options = new List<Option>() { option41, option42, option43, option44, option45, option46 };
            survey.Questions.Add(question4);

            context.Surveys.Add(survey);
            await context.SaveChangesAsync();
            await SeedSurvey(survey);

        }

        public async Task SeedSurvey(Survey survey, int count = 10)
        {
            string userId = "123456";
            Random rng = new Random();

            for (int i = 0; i < count; i++)
            {
                string username = "TestUser" + i;
                User user = await GetUserAsync(survey, username, userId);

                foreach (Question question in survey.Questions)
                {
                    question.Count++;
                    Response response = GetResponse(survey, question, user);
                    if (question.Type == (int)QuestionType.Text || question.Type == (int)QuestionType.LongText)
                    {
                        response.Pos = 1;
                        response.Feedback = Guid.NewGuid().ToString();
                    }
                    else if (question.Type == (int)QuestionType.MultiSelect)
                    {
                        int selectcount = rng.Next(1, question.Options.Count);
                        List<int> options = new List<int>(question.Options.Select(s => s.Pos));
                        for (int j = 0; j < selectcount; j++)
                        {
                            int select = rng.Next(options.Count);
                            response.Answers.First(f => f.Pos == options[select]).Selected = true;
                            question.Options.First(f => f.Pos == options[select]).Count++;
                            options.Remove(options[select]);
                        }
                    }
                    else
                    {
                        int selectcount = rng.Next(1, question.Options.Count);
                        List<int> options = new List<int>(question.Options.Select(s => s.Pos));
                        int select = rng.Next(options.Count);
                        response.Pos = question.Options.First(f => f.Pos == options[select]).Pos;
                        question.Options.First(f => f.Pos == options[select]).Count++;
                    }
                    if (response.ID == 0)
                        context.Responses.Add(response);
                }
                if (user.ID == 0)
                    context.Users.Add(user);
                if (i % 100 == 0)
                    await context.SaveChangesAsync();
            }
            await context.SaveChangesAsync();
        }

        public async Task DeleteSurvey(int id)
        {
            Survey survey = await context.Surveys.Include(i => i.Questions).Include(j => j.Responses).ThenInclude(i => i.Answers).FirstOrDefaultAsync(i => i.ID == id);
            context.Surveys.Remove(survey);
            await context.SaveChangesAsync();
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
