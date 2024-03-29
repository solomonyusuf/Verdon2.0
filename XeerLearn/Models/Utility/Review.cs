﻿using System.ComponentModel.DataAnnotations;

namespace XeerLearn.Models.Utility
{
#nullable disable
    public class Review
    {
        [Key]
        public Guid Id { get; set; }
        public Guid AccessKeyId { get; set; }
        public string TutorId { get; set; }
        public string XeerLearnUserId { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }


        public Review()
        {
            Id = Guid.Parse(Guid.NewGuid().ToString("N"));
            DateTime = DateTime.Now;
        }
    }
}
