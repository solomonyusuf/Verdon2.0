﻿using System.ComponentModel.DataAnnotations;

namespace XeerLearn.Models.Utility
{
#nullable disable
    public class Credential
    {
        [Key]
        public Guid Id { get; set; }
        public string XeerLearnUserId { get; set; }
        public string Title { get; set; }
        public string File { get; set; }
        public DateTime DateTime { get; set; }


        public Credential()
        {
            Id = Guid.Parse(Guid.NewGuid().ToString("N"));
            DateTime = DateTime.Now;
        }
    }
}
