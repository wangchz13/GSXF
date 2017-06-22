using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace GSXF.Model
{
    public class Evaluation
    {
        [Key]
        public int ID { get; set; }

        public virtual Project Project { get; set; }

        public EvaluationSource Source { get; set; }

        public string Note { get; set; }

        public int Result { get; set; }

    }
}
