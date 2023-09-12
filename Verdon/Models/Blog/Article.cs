using System.ComponentModel.DataAnnotations;

namespace Verdon.Models.Blog
{
#nullable disable
    public class Article
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ArticleCategoryId { get; set; }
        public string VerdonUserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime DateTime { get; set; }


        public Article()
        {
            DateTime = DateTime.Now;
        }
    }
}
