using Shop35_Store.Application.Interfaces.Context;
using Shop35_Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop35_Store.Application.Service.Common.Queries.GetSlider
{
    public interface IGetSliderService
    {
        ResultDto<List<sliderDto>> Execute();

    }

    public class GetSliderService : IGetSliderService
    {
        private readonly IDataBaseContext _context;
        public GetSliderService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<List<sliderDto>> Execute()
        {
           var sliders=_context.sliders.OrderByDescending(p=> p.Id).ToList().Select(
          p=> new sliderDto
          {
              link=p.link,
              Src=p.Src,
          }).ToList();
            return new ResultDto<List<sliderDto>>()
                { Data=sliders,
             IsSuccess=true,
            };

        }
    }
    public class sliderDto
    {
        public string  Src { get; set; }
        public string link { get; set; }
    }
}
