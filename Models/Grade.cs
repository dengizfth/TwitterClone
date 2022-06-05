using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IskurTwitterApp.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public string Rank { get; set; }
        public bool Status { get; set; }
        public bool Delete { get; set; }

    }
}