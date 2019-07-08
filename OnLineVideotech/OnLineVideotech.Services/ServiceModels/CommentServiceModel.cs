using System;

namespace OnLineVideotech.Services.ServiceModels
{
    public class CommentServiceModel
    {
        public Guid Id { get; set; }

        public string Comment { get; set; }

        public string UserName { get; set; }

        public DateTime Date { get; set; }

        public Guid MovieId { get; set; }
    }
}