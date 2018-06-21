using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace QuizApplication.Models
{
    public class FinishedQuizByUserSearchListViewModel
    {
        public List<FinishedQuizByUserSearchViewModel> FinishedQuizByUserSearchViewModel { get; set; }

        [Display(Name = "Szűrés kategóriák szerint")]
        public List<string> QuizCategoryList { get; set; }

        public int? UserIdForPassing { get; set; }
    }
}