using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Quiz
    {
        [Key]
        public int QuizID { get; set; }

        [Required]
        public string UserID { get; set; }

        [Required]
        public string QuizTitle { get; set; }

        [Required]
        public string QuizDescription { get; set; }

        [Required]
        public DateTime CreateAt { get; set; }

        [ForeignKey("UserID")]
        public virtual IdentityUser User { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
