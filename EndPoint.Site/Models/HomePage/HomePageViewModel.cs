using Shop35_Store.Application.Service.Common.Queries.GetHomePageImages;
using Shop35_Store.Application.Service.Common.Queries.GetSlider;
using Shop35_Store.Application.Service.Products.Queries.GetProductForSite;

namespace EndPoint.Site.Models.HomePage
{
    public class HomePageViewModel
    {
        public List<sliderDto> Sliders { get; set; }
        public List<HomePageImagesDto> pageImage{get;set;}
        public List<ProductForSiteDto> Camera {  get; set; }
        public List<ProductForSiteDto> Mobile {  get; set; }
        public List<ProductForSiteDto> Laptop {  get; set; }
            

    }
}
