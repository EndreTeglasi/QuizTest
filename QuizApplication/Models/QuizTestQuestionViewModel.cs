using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizApplication.Models
{
    public class QuizTestQuestionViewModel
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public int Point { get; set; }
        public List<Answer> AnswerList { get; set; }
        public int? SelectedAnswer { get; set; }
        public int QuizId { get; set; }
        public int QuestionType { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
    }
}