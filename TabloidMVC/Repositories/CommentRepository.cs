using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        public CommentRepository(IConfiguration config) : base(config) { }
        public void AddComment(Comment comment)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Comment (PostId, UserProfileId, Subject, Content, CreateDateTime)
                        OUTPUT INSERTED.ID
                        VALUES (@postId, @userProfileId, @subject, @content, SYSDATETIME());";
                    cmd.Parameters.AddWithValue("@postId", comment.PostId);
                    cmd.Parameters.AddWithValue("@userProfileId", comment.UserProfileId);
                    cmd.Parameters.AddWithValue("@subject", comment.Subject);
                    cmd.Parameters.AddWithValue("@content", comment.Content);
                    int id = (int)cmd.ExecuteScalar();
                    comment.Id = id;
                }
            }
        }
        public void DeleteComment(int commentId)
        {
            using (var conn = Connection)
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Comment WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", commentId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<Comment> GetAllPostComments(int postId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                      Select * from Comment
                      WHERE PostId = @postId";
                    cmd.Parameters.AddWithValue("@postId", postId);
                    var comments = new List<Comment>();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        comments.Add(new Comment()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                            UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                            Subject = reader.GetString(reader.GetOrdinal("Subject")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime"))
                        });
                    }
                    reader.Close();
                    return comments;
                }
            }
        }
        public Comment GetCommentById(int postId, int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                      Select * from Comment
                      WHERE PostId = @postId AND Id = @id";
                    cmd.Parameters.AddWithValue("@postId", postId);
                    cmd.Parameters.AddWithValue("@id", id);
                    Comment comment = null;
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        comment = new Comment()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                            UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                            Subject = reader.GetString(reader.GetOrdinal("Subject")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime"))
                        };
                    }
                    reader.Close();
                    return comment;
                }
            }
        }
        public List<Comment> GetAllPostComments(int postId, IPostRepository repo)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                      Select * from Comment
                      WHERE PostId = @postId";
                    cmd.Parameters.AddWithValue("@postId", postId);
                    var comments = new List<Comment>();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Comment cmt = new Comment()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),                           
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                            UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                            Subject = reader.GetString(reader.GetOrdinal("Subject")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime"))                           
                        };
                        cmt.Post = repo.GetUserPostById(postId, cmt.UserProfileId);
                        comments.Add(cmt);
                    }
                    reader.Close();
                    return comments;
                }
            }
        }

        public void UpdateComment(Comment comment)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Comment
                            SET 
                                Subject = @subject, 
                                Content = @content
                            WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@subject", comment.Subject);
                    cmd.Parameters.AddWithValue("@content", comment.Content);
                    cmd.Parameters.AddWithValue("@id", comment.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
