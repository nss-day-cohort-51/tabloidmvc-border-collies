using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class TagViewModel
    {
        public Post Post { get; set; }
        public List<Tag> Tags { get; set; }
        public List<int> SelectedTags { get; set; }
    }
}
