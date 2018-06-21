using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizApplication.Models
{
    public class CompletedQuiz
    {
        public int UserId { get; set; }
        public string UserNickName { get; set; }
        public int QuizId { get; set; }
        public string QuizName { get; set; }
        public int QuestionId { get; set; }
        public int Score { get; set; }
        public int TotalScore { get; set; }
        public int Id { get; set; }
        public DateTime Date { get; set; }
    }
}