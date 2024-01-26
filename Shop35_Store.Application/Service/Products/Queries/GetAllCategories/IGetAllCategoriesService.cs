using Shop35_Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop35_Store.Application.Service.Products.Queries.GetAllCategories
{
    public interface IGetAllCategoriesService
    {
        
        ResultDto<List<AllCategoriesDto>> Execute();
    }


}
