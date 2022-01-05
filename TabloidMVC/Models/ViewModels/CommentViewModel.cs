using System.Collections.Generic;

namespace TabloidMVC.Models.ViewModels
{
    public class CommentViewModel
    {
        public int PostId { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
