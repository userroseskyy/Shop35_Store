using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Shop35_Store.Application.Interfaces.Context;
using Shop35_Store.Application.Service.Products.Command.AddNewProduct;
using Shop35_Store.Common.Dto;
using Shop35_Store.Domain.Entities.HomePages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop35_Store.Application.Service.HomePage.AddNewSlider
{
    public interface IAddNewSliderService
    {
        ResultDto Execute(IFormFile file, string link);
    }

    public class AddNewSliderService : IAddNewSliderService
    {
        private readonly IDataBaseContext _context;
        private readonly IHostingEnvironment _environment;
        public AddNewSliderService(IDataBaseContext context,IHostingEnvironment environment)
        {
            _context = context;
            _environment= environment;
        }
        public ResultDto Execute(IFormFile file,string link)
        {
           var resultUpload=UploadFile(file);
            Slider slider=new Slider()
            {
                link= link,
                Src=resultUpload.FileNameAddress

            };
            _context.sliders.Add(slider);
            _context.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true,

            };
        }



        private UploadDto UploadFile(IFormFile file)
        
        {
            if (file != null)
            {
                string folder = $@"images\HomePages\Sliders\";
                var uploadsRootFolder = Path.Combine(_environment.WebRootPath, folder);
                if (!Directory.Exists(uploadsRootFolder))
                {
                    Directory.CreateDirectory(uploadsRootFolder);
                }


                if (file == null || file.Length == 0)
                {
                    return new UploadDto()
                    {
                        Status = false,
                        FileNameAddress = "",
                    };
                }

                string fileName = DateTime.Now.Ticks.ToString() + file.FileName;
                var filePath = Path.Combine(uploadsRootFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                return new UploadDto()
                {
                    FileNameAddress = folder + fileName,
                    Status = true,
                };
            }
            return null;
        }
    }
}
