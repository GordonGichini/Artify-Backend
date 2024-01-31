using Categoryservice.Models;

namespace Categoryservice.Service.IService
{
    public interface ICategory
    {
        Task<List<Category>> GetAll();
        Task<string> CreateCategory(Category category);
        Task<string> UpdateCategory(Category category);
        Task<string> DeleteCategory(Category category);
        Task<Category> GetCategoryById(Guid categoryId);
        Task saveChanges();
    }
}