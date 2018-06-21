using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizApplication.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PlainPassword { get; set; }
        public int AvatarId { get; set; }
        public byte[] AvatarImage { get; set; }
        public int UserTypeId { get; set; }
        public UserType UserType { get; set; }
        public string NickName { get; set; }
        public int NumberOfFinishedQuiz { get; set; }
        public int TotalScore { get; set; }
    }
}