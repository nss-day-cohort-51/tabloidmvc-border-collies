using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.PortableExecutable;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class TagRepository : BaseRepository, ITagRepository
    {
        public TagRepository(IConfiguration config) : base(config) { }
        public List<Tag> GetAllTags()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                      Select * from Tag
                        ORDER BY name ASC";


                    var tags = new List<Tag>();

                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        tags.Add(new Tag()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                        });
                    }

                    reader.Close();

                    return tags;
                }
            }
        }
        public void CreateTag(Tag tag)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Tag (Name)
                                                     VALUES (@Name)";
                    cmd.Parameters.AddWithValue("@Name", tag.Name);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public Tag GetTagById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id,
                                               Name
                                          FROM Tag 
                                         WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    Tag tag = null;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (tag == null)
                        {
                            tag = new Tag()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                        }
                    }
                    reader.Close();
                    return tag;
                }
            }
        }

       public void AddTagToPost(PostTag postTag)
        {

            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO PostTag (PostId, TagId) OUTPUT INSERTED.ID
                                                     VALUES (@postId, @tagId)";
                    cmd.Parameters.AddWithValue("@postId", postTag.PostId);
                    cmd.Parameters.AddWithValue("@tagId", postTag.TagId);
                    
                    int id = (int)cmd.ExecuteScalar();

                    postTag.Id = id;
                }
            }


        }


        public void Update(Tag tag)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Tag 
                                           SET Name = @name
                                         WHERE id = @id";
                    cmd.Parameters.AddWithValue("@name", tag.Name);
                    cmd.Parameters.AddWithValue("@id", tag.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Tag WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

