using Shop35_Store.Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop35_Store.Domain.Entities.Products
{
    public class Category:BaseEntity
    {
        [Required(ErrorMessage = "نام دسته بندی الزامی است")]
        [Display(Name = "نام دسته بندی")]
        public string Name { get; set; }

        public virtual Category ParentCategory { get; set; }
        public long? ParentCategoryId { get; set; }


        //برای نمایش زیر دسته های هر گروه
        public virtual ICollection<Category> SubCategories { get; set; }
    }
}
