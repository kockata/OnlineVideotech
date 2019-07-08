using System;

namespace OnLineVideotech.Web.Models
{
    public class CommentViewModel
    {
        public Guid MovieId { get; set; }

        public Guid CommentId { get; set; }

        public string Comment { get; set; }

        public string UserName { get; set; }

        public DateTime Date { get; set; }
    }
}