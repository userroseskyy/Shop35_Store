using Shop35_Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop35_Store.Application.Service.Products.Command.AddNewCategory
{
    public interface IAddNewCattegoryService
    {
        ResultDto Execute(long? ParentId, string Name);
    }

}


