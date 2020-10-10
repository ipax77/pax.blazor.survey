using pax.blazor.chartjs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pax.blazor.survey.Models
{
    public class Survey
    {
        public int ID { get; set; }
        public int Pos { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string SubUrl { get; set; }
        public bool AllowAnonymouse { get; set; }
        public bool ShowProgress { get; set; }
        public bool ShowResult { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expire { get; set; } = DateTime.Today.AddDays(30);
        public ICollection<Question> Questions { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Response> Responses { get; set; }
        [NotMapped]
        public List<QuestionListItem> QuestionListItems { get; set; } = new List<QuestionListItem>();
    }

    public class Question
    {

        public int ID { get; set; }
        public int Pos { get; set; }
        [Required]
        public string Interview { get; set; }
        public int Type { get; set; }
        public int Count { get; set; }
        public ICollection<Option> Options { get; set; }
        public ICollection<Response> Responses { get; set; }
        public Survey Survey { get; set; }
        [NotMapped]
        public Chart Chart { get; set; }
    }

    public class Option
    {
        public int ID { get; set; }
        public int Pos { get; set; }
        public string OptionValue { get; set; }
        public int Count { get; set; }
        public ICollection<Answer> Answers { get; set; }
        public ICollection<Question> Questions { get; set; }
    }

    public class Response
    {
        public int ID { get; set; }
        public int Pos { get; set; }
        public bool Selected { get; set; }
        public string Feedback { get; set; }
        public User User { get; set; }
        public Question Question { get; set; }
        public Survey Survey { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }

    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<Survey> Surveys { get; set; }
        public ICollection<Response> Responses { get; set; }
    }

    public class Answer
    {
        public int ID { get; set; }
        public int Pos { get; set; }
        public string AnswerValue { get; set; }
        public bool Selected { get; set; }
        public Option Option { get; set; }
        public Response Response { get; set; }
    }

    public enum QuestionType
    {
        Text,
        LongText,
        Bool,
        SingleSelect,
        MultiSelect
    }

    public enum AnswerType
    {
        Text,
        Number,
        Date
    }

    public class SurveyListItem
    {
        public int ID { get; set; }
        public int Pos { get; set; }
        public string Title { get; set; }
        public int Questions { get; set; }
        public int Users { get; set; }
    }

    public class QuestionListItem
    {
        public int ID { get; set; }
        public int Pos { get; set; }
        public string Interview { get; set; }
        public int Type { get; set; }
        public int Responses { get; set; }
    }

    public class AnswerListItem
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Answer { get; set; }
    }
}
