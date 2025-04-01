using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Question : BaseDomain
    {
        [Required]
        public int QuizID { get; set; }

        [Required]
        public string QuestionContent { get; set; }

        [Required]
        public int TimeLimitID { get; set; }

        [Required]
        public int SortOrder { get; set; }

        [ForeignKey("QuizID")]
        public virtual Quiz Quiz { get; set; }

        [ForeignKey("TimeLimitID")]
        public virtual TimeLimit TimeLimit { get; set; }

        public virtual ICollection<Option> Options { get; set; }
    }
}
