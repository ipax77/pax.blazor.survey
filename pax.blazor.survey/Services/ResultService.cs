using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using pax.blazor.survey.Db;
using pax.blazor.survey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pax.blazor.survey.Services
{
    public class ResultService
    {
        private readonly SurveyContext context;
        private readonly ILogger logger;

        public ResultService(SurveyContext context, ILogger<ResultService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task CreateResult(Survey survey)
        {
            DateTime t = DateTime.UtcNow;
            foreach (Question question in survey.Questions)
            {
                var responses = context.Responses.Where(x => x.Survey == survey && x.Question == question);
                float count = await responses.CountAsync();
                if (question.Type == (int)QuestionType.Bool)
                {
                    var option1 = from r in responses
                                  from a in r.Answers.Where(x => x.AnswerValue == question.Options.First().OptionValue)
                                  select (a);
                    var option2 = from r in responses
                                  from a in r.Answers.Where(x => x.AnswerValue == question.Options.Last().OptionValue)
                                  select (a);
                    
                    float opt1 = await option1.CountAsync();
                    float opt2 = await option2.CountAsync();

                    float o1 = opt1 * 100f / count;
                    float o2 = opt2 * 100f / count;
                    Console.WriteLine($"{question.Interview}: {question.Options.First().OptionValue}: {MathF.Round(o1, 2)} %; {question.Options.Last().OptionValue}: {MathF.Round(o2, 2)} %");
                }

                if (question.Type == (int)QuestionType.SingleSelect)
                {
                    Dictionary<string, float> results = new Dictionary<string, float>();
                    var answers = from r in responses
                                  from a in r.Answers
                                  select (a.AnswerValue);

                    foreach (Option option in question.Options.OrderBy(o => o.Pos))
                    {
                        float o = await answers.CountAsync(c => c == option.OptionValue);
                        results.Add(option.OptionValue, MathF.Round(o * 100f / count, 2));
                    }
                    Console.WriteLine(question.Interview);
                    foreach (var ent in results)
                        Console.WriteLine($"{ent.Key}: {ent.Value} %");
                }

                if (question.Type == (int)QuestionType.MultiSelect)
                {
                    Dictionary<string, float> results = new Dictionary<string, float>();
                    var answers = from r in responses
                                  from a in r.Answers
                                  select new { a.Pos, a.AnswerBool };
                    foreach (Option option in question.Options.OrderBy(o => o.Pos))
                    {
                        float o = await answers.CountAsync(c => c.Pos == option.Pos && c.AnswerBool);
                        results.Add(option.OptionValue, MathF.Round(o * 100f / count, 2));
                    }
                    Console.WriteLine(question.Interview);
                    foreach (var ent in results)
                        Console.WriteLine($"{ent.Key}: {ent.Value} %");
                }
            }
            logger.LogInformation("Result created in (ms)" + (DateTime.UtcNow - t).TotalMilliseconds);
        }
    }
}
