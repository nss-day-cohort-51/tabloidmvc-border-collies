using System.Collections.Generic;

namespace TabloidMVC.Models.ViewModels
{
    public class CommentEditViewModel
    {
        public int PostId { get; set; }
        public Comment Comment { get; set; }
    }
}
