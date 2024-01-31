using Categoryservice.Data;
using Categoryservice.Models;
using Categoryservice.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace Categoryservice.Service
{
    public class CategoryService : ICategory
    {
        private readonly ApplicationDbContext _context;
        public CategoryService(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }
        public async Task<string> CreateCategory(Category category)
        {
           _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return "Category Created";
        }

        public async Task<string> DeleteCategory(Category category)
        {
            /* var category= await _context.Categories.Where(x => x. Id==categoryId).FirstOrDefaultAsync();
              if (category!=null)
              {
                   _context.Categories.Remove(category);
                  await _context.SaveChangesAsync();
                  return "category deleted";
              }*/
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return "category deleted";
        }

      

        public async Task<List<Category>> GetAll()
        {
           var art=await _context.Categories.ToListAsync();
            return art;
        }

        public async Task<Category> GetCategoryById(Guid categoryId)
        {
          return await _context.Categories.Where(x=>x.Id==categoryId).FirstOrDefaultAsync();
           
        }

        public async Task saveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<string> UpdateCategory(Category category)
        {
            await _context.SaveChangesAsync();
            return "Category Updated";
        }
    }
}