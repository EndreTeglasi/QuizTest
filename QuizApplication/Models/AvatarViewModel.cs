using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuizApplication.Models
{
    public class AvatarViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Avatárnév")]
        public string Name { get; set; }

        
        [DisplayName("Fájlnév")]
        public HttpPostedFileBase DataInHttpPostedFileBase { get; set; }

        [DisplayName("Kép")]
        public string SourceString { get; set; }
    }
}