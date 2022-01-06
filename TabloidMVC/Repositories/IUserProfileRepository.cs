using System.Collections.Generic;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Repositories
{
    public interface IUserProfileRepository
    {
        UserProfile GetByEmail(string email);
        List<UserProfile> GetAll();
         void AddUser(UserProfile userProfile);
         void DeactivateUser(UserProfile user);
         UserProfile GetById(int Id);

    }
}