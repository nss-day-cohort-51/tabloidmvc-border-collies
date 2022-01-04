using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IReactionRepository
    {
        void AddReaction(Reaction category);
        List<Reaction> GetAll();
        void DeleteReaction(int id);
        Reaction GetById(int id);
    }
}
