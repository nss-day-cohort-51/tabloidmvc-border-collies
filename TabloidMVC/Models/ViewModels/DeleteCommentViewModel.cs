using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class DeleteCommentViewModel
    {
        public int PostId { get; set; }

        public Comment CommentId { get; set; }
    }
}
