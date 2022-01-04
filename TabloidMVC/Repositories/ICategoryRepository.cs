using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        Category GetById(int id);
        void DeleteCategory(int id);
    }
}