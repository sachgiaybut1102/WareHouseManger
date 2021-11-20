using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WareHouseManger.Models.EF;

namespace WareHouseManger.Controllers.Client
{
    public class ProductDetailsController : Controller
    {
        private readonly DB_WareHouseMangerContext _context;
        private Shop_Good shop_GoodDetails_;

        public ProductDetailsController(DB_WareHouseMangerContext context)
        {
            _context = context;
        }

        public IActionResult Index(string name, string id)
        {
            shop_GoodDetails_ = _context.Shop_Goods.Where(x => x.Name == name && x.TemplateID == id).FirstOrDefault();
            if (shop_GoodDetails_ == null)
            {
                string error = "Sản phẩn không còn tồn tại! Mời kiểm tra lại!";
            }
            ViewBag.ProductDetails = shop_GoodDetails_;
            return View(shop_GoodDetails_);
        }
    }
}
