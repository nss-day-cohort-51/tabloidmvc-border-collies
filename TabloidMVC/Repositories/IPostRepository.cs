using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IPostRepository
    {
        void Add(Post postId);
        void UpdatePost(Post post);
        void DeletePost(int post);
        List<Post> GetAllPublishedPosts();
        Post GetPublishedPostById(int id);
        Post GetUserPostById(int id, int userProfileId);
        List<PostTag> GetAllPostTagsForPost(int id);
         List<int> GetPostTagsByPostId(int id);
        void AddPostTag(PostTag postTag);
    }
}