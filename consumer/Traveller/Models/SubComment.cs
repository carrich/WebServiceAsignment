using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Traveller.Models
{
    public class SubComment
    {
        public int Id { get; set; }
        public int? CommentId { get; set; }
        public string Subcomment { get; set; }

        public DateTime? CreateAt { get; set; }
    }
}