using Microsoft.EntityFrameworkCore;
using Shop35_Store.Application.Interfaces.Context;
using Shop35_Store.Common.Dto;
using Shop35_Store.Domain.Entities.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop35_Store.Application.Service.Carts
{
   public interface ICartService
    {
        ResultDto AddToCart(long ProductId, Guid BrowserId);
        ResultDto removeFromCart(long ProductId, Guid BrowserId);
        ResultDto<CartDto> GetMyCart(Guid BrowserId);
        ResultDto Add(long CartItemId);
        ResultDto LowOff(long CartItemId);
    }


    public class CartService : ICartService
    {
        private readonly IDataBaseContext _context;
        public CartService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Add(long CartItemId)
        {
            var cartItem = _context.CartItem.Find(CartItemId);
            
            cartItem.count++;
            _context.SaveChanges();
            return new ResultDto()
            {
                IsSuccess = true,
            };
        }

        public ResultDto AddToCart(long ProductId, Guid BrowserId)
        {
           var cart=_context.Cart.Where(p=> p.BrowserId == BrowserId&& p.Finished==false).FirstOrDefault();
            if (cart == null)
            {
                Cart NewCart = new Cart()
                {
                    Finished = false,
                    BrowserId = BrowserId,
                };
                _context.Cart.Add(NewCart);
                _context.SaveChanges();
                cart = NewCart;



            }
            var product = _context.Products.Find(ProductId);
            var CartItem=_context.CartItem.Where(p=> p.ProductId == ProductId&&p.CartId==cart.UserId).FirstOrDefault();
            if(CartItem != null)
            {
                CartItem.count++;
            }
            else
            {
                CartItem newCartItem = new CartItem()
                {
                    Cart=cart,
                    count=1,
                    Price=product.Price,
                    Product=product,

                };
                _context.CartItem.Add(newCartItem);
                _context.SaveChanges();

            }
            return new ResultDto
            {
                IsSuccess = true,
                Message = $"محصول {product.Name}با موفقیت به سبد خرید شما اضافه شد"
            };
        }

        public ResultDto<CartDto> GetMyCart(Guid BrowserId)
        {
           var cart = _context.Cart
                .Include(p=> p.CartItems)
                .ThenInclude(p=>p.Product)
                .ThenInclude(p=> p.ProductImages)
                .Where(p=> p.BrowserId== BrowserId&&p.Finished==false)
                .OrderByDescending(p=>p.UserId)
                .FirstOrDefault();

            if (cart == null)
            {
             
                return new ResultDto<CartDto>()
                {
                    IsSuccess = false,
                   Message = "سبد خریدی یافت نشد."
                };
            }

           
       

            return new ResultDto<CartDto>()
            {
                Data = new CartDto()
                {
                    ProductCount=cart.CartItems.Count,
                    SumAmount=cart.CartItems.Sum(p=>p.Price * p.count),
                    CartItems = cart.CartItems.Select(p => new CartItemDto
                    {
                        Id=p.Id,

                        Count = p.count,
                        Price = p.Price,
                        Product = p.Product.Name,
                        Images=p.Product?.ProductImages?.FirstOrDefault().Src??"",


                    }).ToList(),
                },
                IsSuccess = true,
            };
            
        }

        public ResultDto LowOff(long CartItemId)
        {
            var cartItem = _context.CartItem.Find(CartItemId);
            if (cartItem.count <= 1)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = ""
                };

            }

            cartItem.count--;

            _context.SaveChanges();
            return new ResultDto()
            {
                IsSuccess = true,
            };
           
        }

        public ResultDto removeFromCart(long ProductId, Guid BrowserId)
        {
            var cartItem=_context.CartItem.Where(p=> p.Cart.BrowserId== BrowserId).FirstOrDefault();
            if(cartItem != null)
            {
                cartItem.IsRemoved = true;
                cartItem.RemoveTime = DateTime.Now;
                _context.SaveChanges();
                return new ResultDto
                {
                    IsSuccess = true,
                    Message = "محصول از سبد خرید شما حذف شد",
                };
            }
            else
            {
              return new ResultDto
                {
                    IsSuccess = false,
                    Message = "محصول یافت نشد",
                };
            }
        }
    }





    public class CartDto
    {
        public int ProductCount { get; set; }
        public int SumAmount { get; set; }
        public List<CartItemDto> CartItems { get;set;}
    }
    public class CartItemDto
    {
        public long Id{get; set; }
    public string Product { get; set; }
        public string Images {  get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
    }
}
