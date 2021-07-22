using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WareHouseManger.Models.EF;

namespace WareHouseManger.Reports
{
    public class rtpHoaDonNhap
    {
        private List<Shop_Goods_Receipt> shop_Goods_Receipts_;

        public rtpHoaDonNhap(List<Shop_Goods_Receipt> shop_Goods_Receipts)
        {
            shop_Goods_Receipts_ = shop_Goods_Receipts;
        }


    }
}
