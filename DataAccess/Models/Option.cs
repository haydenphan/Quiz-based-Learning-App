using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Option : BaseDomain
    {
        [Required]
        public int QuestionID { get; set; }

        [Required]
        public string OptionContent { get; set; }

        [Required]
        public bool IsCorrect { get; set; }

        [Required]
        public int SortOrder { get; set; }

        [ForeignKey("QuestionID")]
        public virtual Question Question { get; set; }
    }
}
