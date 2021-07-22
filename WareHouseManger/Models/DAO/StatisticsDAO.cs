using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WareHouseManger.Models.EF;

namespace WareHouseManger.Models.DAO
{
    public class StatisticsDAO
    {
        private readonly DB_WareHouseMangerContext _context;

        public StatisticsDAO(DB_WareHouseMangerContext context)
        {
            _context = context;
        }

        public int GetCountShop_Goods_Receipt(DateTime startDate, DateTime endDate)
        {
            endDate = EndDate(endDate);

            int count = 0;

            count = _context.Shop_Goods_Receipts.Where(t => t.DateCreated >= startDate && t.DateCreated <= endDate).Count();

            return count;
        }

        public int GetCountShop_Goods_Issues(DateTime startDate, DateTime endDate)
        {
            endDate = EndDate(endDate);

            int count = 0;

            count = _context.Shop_Goods_Issues.Where(t => t.DateCreated >= startDate && t.DateCreated <= endDate).Count();

            return count;
        }

        public decimal GetCountShop_Goods_Revenue(DateTime startDate, DateTime endDate)
        {
            endDate = EndDate(endDate);

            decimal count = 0;

            count = (decimal)_context.Shop_Goods_Issues
                .Where(t => t.DateCreated >= startDate && t.DateCreated <= endDate)
                .Select(t => t.Total)
                .Sum();

            return count;
        }

        public decimal GetCountShop_Goods_Cost(DateTime startDate, DateTime endDate)
        {
            endDate = EndDate(endDate);

            decimal count = 0;

            count = (decimal)_context.Shop_Goods_Receipts
                .Where(t => t.DateCreated >= startDate && t.DateCreated <= endDate)
                .Select(t => t.Total)
                .Sum();

            return count;
        }

        private DateTime EndDate(DateTime endDate)
        {
            endDate.AddHours(12);

            return endDate;
        }
    }
}
