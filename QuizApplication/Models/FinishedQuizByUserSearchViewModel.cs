using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace QuizApplication.Models
{
    public class FinishedQuizByUserSearchViewModel
    {
        [Display(Name = "Felhasználó azonosító")]
        public int UserId { get; set; }

        [Display(Name = "Kvíz azonosító")]
        public int QuizId { get; set; }

        [Display(Name = "Kvíz kategória")]
        public string QuizCategory { get; set; }

        [Display(Name = "Kérdés azonosító")]
        public int QuestionId { get; set; }

        [Display(Name = "Kérdés")]
        public string QuestionName { get; set; }

        [Display(Name = "Pontszám")]
        public int Score { get; set; }

        [Display(Name = "Kitöltött kvíz azonosító")]
        public int Id { get; set; }

        [Display(Name = "Kitöltés dátuma")]
        public DateTime Date { get; set; }

        [Display(Name = "Felhasználó becenév")]
        public string NickName { get; set; }

        public ReturnFilter Filter { get; set; }
    }

    public class ReturnFilter
    {
        [Display(Name = "Szűrés kitöltés dátuma szerint")]
        public DateTime FromDate { get; set; }

        [Display(Name = "Szűrés kitöltés dátuma szerint")]
        public DateTime ToDate { get; set; }

    }
}