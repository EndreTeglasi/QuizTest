using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizApplication.Models
{
    public class UserFinishedQuizViewModel
    {
        public int Id { get; set; }
        public string QuizName { get; set; }
        public string NickName { get; set; }
        public List<string> QuestionName { get; set; }
        public List<int> ScorePerQuestion { get; set; }
        public int TotalScore { get; set; }
        public DateTime Date { get; set; }
    }
}