using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace WcfServiceTravel
{
    public class Posted
    {
        public int id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        [DataType(DataType.Date)]
        public DateTime ? CreatedAt { get; set; }
        public int ? UserId { get; set; }
    }
}