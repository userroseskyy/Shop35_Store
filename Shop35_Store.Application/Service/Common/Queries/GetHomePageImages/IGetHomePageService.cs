using Shop35_Store.Application.Interfaces.Context;
using Shop35_Store.Common.Dto;
using Shop35_Store.Domain.Entities.HomePages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop35_Store.Application.Service.Common.Queries.GetHomePageImages
{
    public interface IGetHomePageService
    {
        ResultDto<List<HomePageImagesDto>> Execute();

    }
    public class GetHomePageService : IGetHomePageService
    {
        private readonly IDataBaseContext _context;
        public GetHomePageService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<List<HomePageImagesDto>> Execute()
        {
           var image=_context.HomePageImages.OrderByDescending(p=> p.Id).
                Select(p=> new HomePageImagesDto
           {
                    Id = p.Id,
                    ImageLocation= p.ImageLocation,
                    link=p.link,
                    Src= p.Src,
           }).ToList();
            return new ResultDto<List<HomePageImagesDto>>()
            {
                Data = image,
                IsSuccess = true,


            };
        }
    }


    public class HomePageImagesDto
    {
        public long Id { get; set; }
        public string Src { get; set; }
        public string link { get; set; }
        public ImageLocation ImageLocation { get; set; }

    }
}
