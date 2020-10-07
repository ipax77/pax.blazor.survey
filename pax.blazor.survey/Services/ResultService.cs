﻿using Microsoft.EntityFrameworkCore;
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
                var responses = context.Responses.AsNoTracking().Where(x => x.Survey == survey && x.Question == question);
                float count = await responses.CountAsync();

                Dictionary<string, float> results = new Dictionary<string, float>();
                var dbanswers = question.Type switch
                {
                    (int)QuestionType.MultiSelect => from r in responses
                                                     from a in r.Answers
                                                     where a.Selected
                                                     select a.Pos,
                    _ => from r in responses
                         select r.Pos
                }; 
                var answers = await dbanswers.ToListAsync();
                foreach (Option option in question.Options.OrderBy(o => o.Pos))
                {
                    float o = answers.Count(c => c == option.Pos);
                    results.Add(option.OptionValue, MathF.Round(o * 100f / count, 2));
                }
                Console.WriteLine(question.Interview);
                foreach (var ent in results)
                    Console.WriteLine($"{ent.Key}: {ent.Value} %");
            }
            logger.LogInformation("Result created in (ms)" + (DateTime.UtcNow - t).TotalMilliseconds);
        }
    }
}
