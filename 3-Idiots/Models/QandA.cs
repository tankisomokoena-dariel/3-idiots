using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3_Idiots.Models
{
    public class QandA
    {
        public int questionID  { get; set; }
        public int userID { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}