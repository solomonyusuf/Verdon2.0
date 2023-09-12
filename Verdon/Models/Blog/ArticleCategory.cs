using System.ComponentModel.DataAnnotations;

namespace Verdon.Models.Blog
{
#nullable disable
    public class ArticleCategory
    {
        [Key]
        public Guid Id { get; set; }
        public string Image { get; set; }
        public string Icon { get; set; }
        public string Title { get; set; }
        public DateTime DateTime { get; set; }


        public ArticleCategory()
        {
            DateTime = DateTime.Now;
        }
    }
}
