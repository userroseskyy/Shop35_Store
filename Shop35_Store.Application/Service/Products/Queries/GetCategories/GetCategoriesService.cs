using Microsoft.EntityFrameworkCore;
using Shop35_Store.Application.Interfaces.Context;
using Shop35_Store.Common.Dto;

namespace Shop35_Store.Application.Service.Products.Queries.GetCategories
{
    public class GetCategoriesService : IGetCategoriesService
    {
        private readonly IDataBaseContext _context;
        public GetCategoriesService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<List<CategoriesDto>> Execute(long? ParentId)
        {
            var categories = _context.Categories
               .Include(p => p.ParentCategory)
               .Include(P => P.SubCategories)
               .Where(P => P.ParentCategoryId == ParentId)
               .ToList()
               .Select(P => new CategoriesDto
               {
                   Id = P.Id,
                   Name = P.Name,
                   Parent = P.ParentCategory != null ? new
                   ParentCategoryDto
                   {
                       Id = P.ParentCategory.Id,
                       name = P.ParentCategory.Name,
                   }
                   : null,
                   HasChild = P.SubCategories.Count() > 0 ? true : false,

               }).ToList();
            return new ResultDto<List<CategoriesDto>>()
            {
                Data = categories,
                IsSuccess = true,
                Message = "لیست باموقیت برگشت داده شد"
            };

        }
    }
}
