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
        Tag GetPostByTag(int PostId);
        //Post GetSubsPost (int)
        List<int> GetTagsByPostId(int id);
        void AddPostTag(PostTag postTag);
    }
}