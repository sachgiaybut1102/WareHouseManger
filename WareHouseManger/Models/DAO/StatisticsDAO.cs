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

        public async Task<List<StatisticsInfo>> GetCountShop_Goods_ReceiptByMonth(int month, int year)
        {
            List<StatisticsInfo> statisticsInfos = new List<StatisticsInfo>();

            var shop_Goods_Receipts = await _context.Shop_Goods_Receipts
                .Where(t => t.DateCreated.Value.Month == month && t.DateCreated.Value.Year == year)
                .ToArrayAsync();

            int dateCount = DateTime.DaysInMonth(year, month);

            for (int i = 1; i <= dateCount; i++)
            {
                statisticsInfos.Add(new StatisticsInfo()
                {
                    Index = i,
                    Value = (decimal)shop_Goods_Receipts.Where(t => t.DateCreated.Value.Day == i).Select(t => t.Total).Sum()
                });
            }

            return statisticsInfos;
        }

        public async Task<List<StatisticsInfo>> GetCountShop_Goods_ReceiptByYear(int year)
        {
            List<StatisticsInfo> statisticsInfos = new List<StatisticsInfo>();

            var shop_Goods_Receipts = await _context.Shop_Goods_Receipts
                .Where(t => t.DateCreated.Value.Year == year)
                .ToArrayAsync();


            for (int i = 1; i <= 12; i++)
            {
                statisticsInfos.Add(new StatisticsInfo()
                {
                    Index = i,
                    Value = (decimal)shop_Goods_Receipts.Where(t => t.DateCreated.Value.Month == i).Select(t => t.Total).Sum()
                });
            }

            return statisticsInfos;
        }

        public int GetCountShop_Goods_Issues(DateTime startDate, DateTime endDate)
        {
            endDate = EndDate(endDate);

            int count = 0;

            count = _context.Shop_Goods_Issues.Where(t => t.DateCreated >= startDate && t.DateCreated <= endDate).Count();

            return count;
        }

        public async Task<List<StatisticsInfo>> GetCountShop_Goods_IssuesByMonth(int month, int year)
        {
            List<StatisticsInfo> statisticsInfos = new List<StatisticsInfo>();

            var shop_Goods_Issues = await _context.Shop_Goods_Issues
                .Where(t => t.DateCreated.Value.Month == month && t.DateCreated.Value.Year == year)
                .ToArrayAsync();

            int dateCount = DateTime.DaysInMonth(year, month);

            for (int i = 1; i <= dateCount; i++)
            {
                statisticsInfos.Add(new StatisticsInfo()
                {
                    Index = i,
                    Value = (decimal)shop_Goods_Issues.Where(t => t.DateCreated.Value.Day == i).Select(t => t.Total).Sum()
                });
            }

            return statisticsInfos;
        }

        public async Task<List<StatisticsInfo>> GetCountShop_Goods_IssuesByYear(int year)
        {
            List<StatisticsInfo> statisticsInfos = new List<StatisticsInfo>();

            var shop_Goods_Issues = await _context.Shop_Goods_Issues
                .Where(t => t.DateCreated.Value.Year == year)
                .ToArrayAsync();


            for (int i = 1; i <= 12; i++)
            {
                statisticsInfos.Add(new StatisticsInfo()
                {
                    Index = i,
                    Value = (decimal)shop_Goods_Issues.Where(t => t.DateCreated.Value.Month == i).Select(t => t.Total).Sum()
                });
            }

            return statisticsInfos;
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

        public async Task<List<StatisticsShopGoodsInfo>> GetRankShop_GoodsByMonth(int month, int year)
        {
            List<StatisticsShopGoodsInfo> statisticsShopGoodsInfos = new List<StatisticsShopGoodsInfo>();

            var shop_Goods_Issues = await _context.Shop_Goods_Issues
                .Include(t => t.Shop_Goods_Issues_Details)
                .ThenInclude(t => t.Template)
                .ThenInclude(t => t.Unit)
                .Where(t => t.DateCreated.Value.Month == month && t.DateCreated.Value.Year == year)
                .Select(t => t.Shop_Goods_Issues_Details)
                .ToArrayAsync();

            List<Shop_Goods_Issues_Detail> shop_Goods_Issues_Details = new List<Shop_Goods_Issues_Detail>();

            foreach (var list in shop_Goods_Issues)
            {
                foreach (var info in list)
                {
                    shop_Goods_Issues_Details.Add(info);
                }
            }

            var teamplates = shop_Goods_Issues_Details.GroupBy(t => t.TemplateID).ToArray();
            foreach (var teamplate in teamplates)
            {
                statisticsShopGoodsInfos.Add(new StatisticsShopGoodsInfo()
                {
                    TemplateID = teamplate.Key,
                    Name = teamplate.FirstOrDefault().Template.Name,
                    Count = (decimal)teamplate.Sum(t => t.Count),
                    Turnover = teamplate.Sum(t => (decimal)(t.Count * t.UnitPrice)),
                    Unit = teamplate.FirstOrDefault().Template.Unit.Name
                });
            }

            return statisticsShopGoodsInfos.OrderByDescending(t => t.Turnover).ToList();
        }

        public async Task<List<StatisticsShopGoodsInfo>> GetRankShop_GoodsByYear(int year)
        {
            List<StatisticsShopGoodsInfo> statisticsShopGoodsInfos = new List<StatisticsShopGoodsInfo>();

            var shop_Goods_Issues = await _context.Shop_Goods_Issues
                .Include(t => t.Shop_Goods_Issues_Details)
                .ThenInclude(t => t.Template)
                .ThenInclude(t=>t.Unit)
                .Where(t => t.DateCreated.Value.Year == year)
                .Select(t => t.Shop_Goods_Issues_Details)
                .ToArrayAsync();

            List<Shop_Goods_Issues_Detail> shop_Goods_Issues_Details = new List<Shop_Goods_Issues_Detail>();

            foreach (var list in shop_Goods_Issues)
            {
                foreach (var info in list)
                {
                    shop_Goods_Issues_Details.Add(info);
                }
            }

            var teamplates = shop_Goods_Issues_Details.GroupBy(t => t.TemplateID).ToArray();
            foreach (var teamplate in teamplates)
            {
                statisticsShopGoodsInfos.Add(new StatisticsShopGoodsInfo()
                {
                    TemplateID = teamplate.Key,
                    Name = teamplate.FirstOrDefault().Template.Name,
                    Count = (decimal)teamplate.Sum(t => t.Count),
                    Turnover = teamplate.Sum(t => (decimal)(t.Count * t.UnitPrice)),
                    Unit = teamplate.FirstOrDefault().Template.Unit.Name
                });
            }

            return statisticsShopGoodsInfos.OrderByDescending(t => t.Turnover).ToList();
        }

        private DateTime EndDate(DateTime endDate)
        {
            endDate.AddHours(12);

            return endDate;
        }
    }
}
