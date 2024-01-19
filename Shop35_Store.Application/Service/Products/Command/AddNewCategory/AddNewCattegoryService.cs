using Shop35_Store.Application.Interfaces.Context;
using Shop35_Store.Common.Dto;
using Shop35_Store.Domain.Entities.Products;

namespace Shop35_Store.Application.Service.Products.Command.AddNewCategory
{
    public class AddNewCattegoryService : IAddNewCattegoryService
    {
        private readonly IDataBaseContext _context;
        public AddNewCattegoryService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(long? ParentId, string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "نام دسته بندی را وارد کنید",
                };
            }
            Category category = new Category()
            {
                Name = Name,
                ParentCategory = GetParent(ParentId),
                
            };

            _context.Categories.Add(category);
            _context.SaveChanges();
            return new ResultDto()
            {
                IsSuccess = true,
                Message = "دسته بندی با موفقیت اضافه شد",
            };
        }


        private Category GetParent(long? parentId)
        {
            return _context.Categories.Find(parentId);
        }

    }

}


