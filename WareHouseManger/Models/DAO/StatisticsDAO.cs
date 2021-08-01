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

        public async Task<int> GetCountShop_Goods_Receipt(DateTime startDate, DateTime endDate)
        {
            endDate = EndDate(endDate);

            int count = 0;

            count = await _context.Shop_Goods_Receipts.Where(t => t.DateCreated >= startDate && t.DateCreated <= endDate).CountAsync();

            return count;
        }

        public async Task<List<StatisticsInfo>> GetCountShop_Goods_ReceiptByMonth(int month, int year)
        {
            List<StatisticsInfo> statisticsInfos = new();

            var shop_Goods_Receipts = await _context.FinalSettlement_Supliers
                .Where(t => t.DateCreated.Value.Month == month && t.DateCreated.Value.Year == year)
                .ToArrayAsync();

            int dateCount = DateTime.DaysInMonth(year, month);

            for (int i = 1; i <= dateCount; i++)
            {
                statisticsInfos.Add(new StatisticsInfo()
                {
                    Index = i,
                    Value = (decimal)shop_Goods_Receipts.Where(t => t.DateCreated.Value.Day == i).Select(t => t.Payment).Sum()
                });
            }

            return statisticsInfos;
        }

        public async Task<List<StatisticsInfo>> GetCountShop_Goods_ReceiptByYear(int year)
        {
            List<StatisticsInfo> statisticsInfos = new();

            var shop_Goods_Receipts = await _context.FinalSettlement_Supliers
                .Where(t => t.DateCreated.Value.Year == year)
                .ToArrayAsync();


            for (int i = 1; i <= 12; i++)
            {
                statisticsInfos.Add(new StatisticsInfo()
                {
                    Index = i,
                    Value = (decimal)shop_Goods_Receipts.Where(t => t.DateCreated.Value.Month == i).Select(t => t.Payment).Sum()
                });
            }

            return statisticsInfos;
        }

        public async Task<int> GetCountShop_Goods_Issues(DateTime startDate, DateTime endDate)
        {
            endDate = EndDate(endDate);

            int count = 0;

            count = await _context.Shop_Goods_Issues.Where(t => t.DateCreated >= startDate && t.DateCreated <= endDate).CountAsync();

            return count;
        }

        public async Task<List<StatisticsInfo>> GetCountShop_Goods_IssuesByMonth(int month, int year)
        {
            List<StatisticsInfo> statisticsInfos = new();

            var shop_Goods_Issues = await _context.FinalSettlement_Customers
                .Where(t => t.DateCreated.Value.Month == month && t.DateCreated.Value.Year == year)
                .ToArrayAsync();

            int dateCount = DateTime.DaysInMonth(year, month);

            for (int i = 1; i <= dateCount; i++)
            {
                statisticsInfos.Add(new StatisticsInfo()
                {
                    Index = i,
                    Value = (decimal)shop_Goods_Issues.Where(t => t.DateCreated.Value.Day == i).Select(t => t.Payment).Sum()
                });
            }

            return statisticsInfos;
        }

        public async Task<List<StatisticsInfo>> GetCountShop_Goods_IssuesByYear(int year)
        {
            List<StatisticsInfo> statisticsInfos = new();

            var shop_Goods_Issues = await _context.FinalSettlement_Customers
                .Where(t => t.DateCreated.Value.Year == year)
                .ToArrayAsync();


            for (int i = 1; i <= 12; i++)
            {
                statisticsInfos.Add(new StatisticsInfo()
                {
                    Index = i,
                    Value = (decimal)shop_Goods_Issues.Where(t => t.DateCreated.Value.Month == i).Select(t => t.Payment).Sum()
                });
            }

            return statisticsInfos;
        }

        public async Task<decimal> GetCountShop_Goods_Revenue(DateTime startDate, DateTime endDate)
        {
            endDate = EndDate(endDate);

            decimal count = 0;

            count = (decimal)await _context.Shop_Goods_Issues
                .Where(t => t.DateCreated >= startDate && t.DateCreated <= endDate)
                .Select(t => t.Total)
                .SumAsync();

            return count;
        }

        public async Task<decimal> GetCountShop_Goods_Cost(DateTime startDate, DateTime endDate)
        {
            endDate = EndDate(endDate);

            decimal count = 0;

            count = (decimal)await _context.Shop_Goods_Receipts
                .Where(t => t.DateCreated >= startDate && t.DateCreated <= endDate)
                .Select(t => t.Total)
                .SumAsync();

            return count;
        }

        public async Task<decimal> GetCountShop_Goods_RealRevenue(DateTime startDate, DateTime endDate)
        {
            endDate = EndDate(endDate);

            decimal count = 0;

            count = (decimal)await _context.FinalSettlement_Customers
                .Where(t => t.DateCreated >= startDate && t.DateCreated <= endDate)
                .Select(t => t.Payment)
                .SumAsync();

            return count;
        }

        public async Task<decimal> GetCountShop_Goods_RealCost(DateTime startDate, DateTime endDate)
        {
            endDate = EndDate(endDate);

            decimal count = 0;

            count = (decimal)await _context.FinalSettlement_Supliers
                .Where(t => t.DateCreated >= startDate && t.DateCreated <= endDate)
                .Select(t => t.Payment)
                .SumAsync();

            return count;
        }

        public async Task<List<StatisticsShopGoodsInfo>> GetRankShop_GoodsByMonth(int month, int year)
        {
            List<StatisticsShopGoodsInfo> statisticsShopGoodsInfos = new();

            var shop_Goods_Issues = await _context.Shop_Goods_Issues
                .Include(t => t.Shop_Goods_Issues_Details)
                .ThenInclude(t => t.Template)
                .ThenInclude(t => t.Unit)
                .Where(t => t.DateCreated.Value.Month == month && t.DateCreated.Value.Year == year)
                .Select(t => t.Shop_Goods_Issues_Details)
                .ToArrayAsync();

            List<Shop_Goods_Issues_Detail> shop_Goods_Issues_Details = new();

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
            List<StatisticsShopGoodsInfo> statisticsShopGoodsInfos = new();

            var shop_Goods_Issues = await _context.Shop_Goods_Issues
                .Include(t => t.Shop_Goods_Issues_Details)
                .ThenInclude(t => t.Template)
                .ThenInclude(t => t.Unit)
                .Where(t => t.DateCreated.Value.Year == year)
                .Select(t => t.Shop_Goods_Issues_Details)
                .ToArrayAsync();

            List<Shop_Goods_Issues_Detail> shop_Goods_Issues_Details = new();

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

        public async Task<List<RankingPersonInfo>> GetRevenueGroupByEmployee(DateTime startDate, DateTime endDate)
        {
            endDate = EndDate(endDate);

            var list = await _context.Shop_Goods_Issues
                .Include(t => t.Employee)
                .Where(t => t.DateCreated >= startDate && t.DateCreated <= endDate)
                .ToListAsync();

            List<RankingPersonInfo> rankingPersonInfos = new();

            if (list.Count > 0)
            {
                var employees = list.GroupBy(t => t.Employee).ToList();

                foreach (var employee in employees)
                {
                    RankingPersonInfo rankingPersonInfo = new RankingPersonInfo()
                    {
                        ID = employee.Key.EmployeeID,
                        Name = employee.Key.Name,
                        Price = (decimal)employee.Select(t => t.Total).Sum(),
                        TotalBill = employee.Count()
                    };

                    rankingPersonInfos.Add(rankingPersonInfo);
                }
            }

            return rankingPersonInfos.OrderByDescending(t => t.Price).ToList();
        }

        public async Task<List<RankingPersonInfo>> GetRevenueGroupByCustomer(DateTime startDate, DateTime endDate)
        {
            endDate = EndDate(endDate);

            var list = await _context.Shop_Goods_Issues
               .Include(t => t.Customer)
               .Where(t => t.DateCreated >= startDate && t.DateCreated <= endDate)

               .ToListAsync();

            List<RankingPersonInfo> rankingPersonInfos = new();

            if (list.Count > 0)
            {
                var customers = list.GroupBy(t => t.Customer).ToList();

                foreach (var customer in customers)
                {
                    RankingPersonInfo rankingPersonInfo = new RankingPersonInfo()
                    {
                        ID = customer.Key.CustomerID,
                        Name = customer.Key.Name,
                        Price = (decimal)customer.Select(t => t.Total).Sum(),
                        TotalBill = customer.Count()
                    };

                    rankingPersonInfos.Add(rankingPersonInfo);
                }
            }

            return rankingPersonInfos.OrderByDescending(t => t.Price).ToList();
        }

        public async Task<List<RankingPersonInfo>> GetCostGroupBySuplier(DateTime startDate, DateTime endDate)
        {
            endDate = EndDate(endDate);

            var list = await _context.Shop_Goods_Receipts
                .Include(t => t.Supplier)
                .Where(t => t.DateCreated >= startDate && t.DateCreated <= endDate)

                .ToListAsync();

            List<RankingPersonInfo> rankingPersonInfos = new();

            if (list.Count() > 0)
            {
                var supliers = list.GroupBy(t => t.Supplier).ToList();
                foreach (var suplier in supliers)
                {
                    RankingPersonInfo rankingPersonInfo = new RankingPersonInfo()
                    {
                        ID = suplier.Key.SupplierID,
                        Name = suplier.Key.Name,
                        Price = (decimal)suplier.Select(t => t.Total).Sum(),
                        TotalBill = suplier.Count()
                    };

                    rankingPersonInfos.Add(rankingPersonInfo);
                }
            }


            return rankingPersonInfos.OrderByDescending(t => t.Price).ToList();
        }

        public async Task<List<object>> GetCountRecepitShopGoods(int month, int year, string templateID)
        {
            List<StatisticsInfo> statisticsInfos = new();

            int days = DateTime.DaysInMonth(year, month);

            var goodsReceiptIDs = await _context.Shop_Goods_Receipts
                .Where(t => t.DateCreated.Value.Month == month && t.DateCreated.Value.Year == year)
                .Select(t => t.GoodsReceiptID)
                .ToArrayAsync();

            var Shop_Goods_Receipt_Details = await _context.Shop_Goods_Receipt_Details
                .Include(t => t.GoodsReceipt)
                .Where(t => goodsReceiptIDs.Contains(t.GoodsReceiptID) && t.TemplateID == templateID)
                .ToListAsync();

            var goodsIssues = await _context.Shop_Goods_Issues
                .Where(t => t.DateCreated.Value.Month == month && t.DateCreated.Value.Year == year)
                .Select(t => t.GoodsIssueID)
                .ToArrayAsync();

            var shop_Goods_Issues_Details = await _context.Shop_Goods_Issues_Details
                 .Include(t => t.GoodsIssue)
                .Where(t => goodsIssues.Contains(t.GoodsIssueID) && t.TemplateID == templateID)
                .ToListAsync();


            List<object> data = new List<object>();

            for (int i = 1; i <= days; i++)
            {
                var childShop_Goods_Receipt_Details = Shop_Goods_Receipt_Details.Where(t => t.GoodsReceipt.DateCreated.Value.Day == i).ToArray();
                var childshop_Goods_Issues_Details = shop_Goods_Issues_Details.Where(t => t.GoodsIssue.DateCreated.Value.Day == i).ToArray();

                var turnover = childshop_Goods_Issues_Details.Select(t => t.UnitPrice * t.Count).Sum();
                var cost = childShop_Goods_Receipt_Details.Select(t => t.UnitPrice * t.Count).Sum();

                var countIssues = (int)childshop_Goods_Issues_Details.Select(t => t.Count).Sum();
                var countRecepit = (int)childShop_Goods_Receipt_Details.Select(t => t.Count).Sum();

                data.Add(new
                {
                    index = i,
                    countIssues = countIssues,
                    countRecepit = countRecepit,
                    turnover = turnover,
                    cost = cost,
                });
            }

            return data;
        }

        public async Task<List<object>> GetCountRecepitShopGoods(int year, string templateID)
        {
            List<StatisticsInfo> statisticsInfos = new();

            var goodsReceiptIDs = await _context.Shop_Goods_Receipts
                .Where(t => t.DateCreated.Value.Year == year)
                .Select(t => t.GoodsReceiptID)
                .ToArrayAsync();

            var Shop_Goods_Receipt_Details = await _context.Shop_Goods_Receipt_Details
                .Include(t => t.GoodsReceipt)
                .Where(t => goodsReceiptIDs.Contains(t.GoodsReceiptID) && t.TemplateID == templateID)
                .ToListAsync();

            var goodsIssues = await _context.Shop_Goods_Issues
                .Where(t => t.DateCreated.Value.Year == year)
                .Select(t => t.GoodsIssueID)
                .ToArrayAsync();

            var shop_Goods_Issues_Details = await _context.Shop_Goods_Issues_Details
                 .Include(t => t.GoodsIssue)
                .Where(t => goodsIssues.Contains(t.GoodsIssueID) && t.TemplateID == templateID)
                .ToListAsync();


            List<object> data = new List<object>();

            for (int i = 1; i <= 12; i++)
            {
                var childShop_Goods_Receipt_Details = Shop_Goods_Receipt_Details.Where(t => t.GoodsReceipt.DateCreated.Value.Month == i).ToArray();
                var childshop_Goods_Issues_Details = shop_Goods_Issues_Details.Where(t => t.GoodsIssue.DateCreated.Value.Month == i).ToArray();

                var turnover = childshop_Goods_Issues_Details.Select(t => t.UnitPrice * t.Count).Sum();
                var cost = childShop_Goods_Receipt_Details.Select(t => t.UnitPrice * t.Count).Sum();

                var countIssues = (int)childshop_Goods_Issues_Details.Select(t => t.Count).Sum();
                var countRecepit = (int)childShop_Goods_Receipt_Details.Select(t => t.Count).Sum();

                data.Add(new
                {
                    index = i,
                    countIssues = countIssues,
                    countRecepit = countRecepit,
                    turnover = countIssues,
                    cost = countRecepit,
                });
            }

            return data;
        }

        public async Task<List<CountRecepitShopGoodsGroupByCustomer>> GetCountRecepitShopGoodsGroupByCustomer(int month, int year, string templateID)
        {
            List<StatisticsInfo> statisticsInfos = new();

            int days = DateTime.DaysInMonth(year, month);


            var goodsIssues = await _context.Shop_Goods_Issues
                .Include(t => t.Customer)
                .Where(t => t.DateCreated.Value.Month == month && t.DateCreated.Value.Year == year).
                ToArrayAsync();


            var goodsIssuesID = goodsIssues.Select(t => t.GoodsIssueID).ToArray();

            var shop_Goods_Issues_Details = await _context.Shop_Goods_Issues_Details
                 .Include(t => t.GoodsIssue)
                .Where(t => goodsIssuesID.Contains(t.GoodsIssueID) && t.TemplateID == templateID)
                .ToListAsync();


            List<CountRecepitShopGoodsGroupByCustomer> data = new();

            var goodsIssuesGroup = goodsIssues.GroupBy(t => t.Customer).ToArray();


            foreach (var customer in goodsIssuesGroup)
            {
                int id = customer.Key.CustomerID;
                string address = customer.Key.Address;
                string name = customer.Key.Name;
                string phoneNuber = customer.Key.PhoneNumber;

                goodsIssuesID = customer.Select(t => t.GoodsIssueID).ToArray();
                var list = shop_Goods_Issues_Details.Where(t => goodsIssuesID.Contains(t.GoodsIssueID)).ToArray();

                var count = (int)list.Select(t => t.Count).Sum();
                var turnover = (decimal)list.Select(t => (decimal)t.Count * t.UnitPrice).Sum();

                data.Add(new CountRecepitShopGoodsGroupByCustomer()
                {
                    ID = id,
                    Name = name,
                    Address = address,
                    PhoneNumber = phoneNuber,
                    Count = count,
                    Turnover = turnover,
                });
            }

            return data;
        }

        public async Task<List<CountRecepitShopGoodsGroupByCustomer>> GetCountRecepitShopGoodsGroupByCustomer(int year, string templateID)
        {
            List<StatisticsInfo> statisticsInfos = new();

            var goodsIssues = await _context.Shop_Goods_Issues
                .Include(t => t.Customer)
                .Where(t => t.DateCreated.Value.Year == year).
                ToArrayAsync();


            var goodsIssuesID = goodsIssues.Select(t => t.GoodsIssueID).ToArray();

            var shop_Goods_Issues_Details = await _context.Shop_Goods_Issues_Details
                 .Include(t => t.GoodsIssue)
                .Where(t => goodsIssuesID.Contains(t.GoodsIssueID) && t.TemplateID == templateID)
                .ToListAsync();


            List<CountRecepitShopGoodsGroupByCustomer> data = new();

            var goodsIssuesGroup = goodsIssues.GroupBy(t => t.Customer).ToArray();


            foreach (var customer in goodsIssuesGroup)
            {
                int id = customer.Key.CustomerID;
                string address = customer.Key.Address;
                string name = customer.Key.Name;
                string phoneNuber = customer.Key.PhoneNumber;

                goodsIssuesID = customer.Select(t => t.GoodsIssueID).ToArray();
                var list = shop_Goods_Issues_Details.Where(t => goodsIssuesID.Contains(t.GoodsIssueID)).ToArray();

                var count = (int)list.Select(t => t.Count).Sum();
                var turnover = (decimal)list.Select(t => (decimal)t.Count * t.UnitPrice).Sum();

                data.Add(new CountRecepitShopGoodsGroupByCustomer()
                {
                    ID = id,
                    Name = name,
                    Address = address,
                    PhoneNumber = phoneNuber,
                    Count = count,
                    Turnover = turnover,
                });
            }

            return data.OrderByDescending(t => t.Turnover).ToList();
        }


        public async Task<List<object>> GetCountIssuesShopGoodsGroupByEmployee(int month, int year, int employeeID)
        {
            List<StatisticsInfo> statisticsInfos = new();

            int days = DateTime.DaysInMonth(year, month);

            //var Shop_Goods_Receipts = await _context.Shop_Goods_Receipts
            //    .Where(t => t.DateCreated.Value.Month == month && t.DateCreated.Value.Year == year && t.EmployeeID == employeeID)
            //    .ToArrayAsync();

            var shop_Goods_Issues = await _context.Shop_Goods_Issues
                .Where(t => t.DateCreated.Value.Month == month && t.DateCreated.Value.Year == year && t.EmployeeID == employeeID)
                .ToArrayAsync();



            List<object> data = new List<object>();

            for (int i = 1; i <= days; i++)
            {
                //var childShop_Goods_Receipt_Details = Shop_Goods_Receipts.Where(t => t.DateCreated.Value.Day == i).ToArray();
                var childshop_Goods_Issues_Details = shop_Goods_Issues.Where(t => t.DateCreated.Value.Day == i).ToArray();

                data.Add(new
                {
                    index = i,
                    value = childshop_Goods_Issues_Details.Select(t => t.Total).Sum()
                }); ;
            }

            return data;
        }

        public async Task<List<object>> GetCountIssuesShopGoodsGroupByEmployee(int year, int employeeID)
        {
            List<StatisticsInfo> statisticsInfos = new();

            //var Shop_Goods_Receipts = await _context.Shop_Goods_Receipts
            //    .Where(t => t.DateCreated.Value.Month == month && t.DateCreated.Value.Year == year && t.EmployeeID == employeeID)
            //    .ToArrayAsync();

            var shop_Goods_Issues = await _context.Shop_Goods_Issues
                .Where(t => t.DateCreated.Value.Year == year && t.EmployeeID == employeeID)
                .ToArrayAsync();



            List<object> data = new List<object>();

            for (int i = 1; i <= 12; i++)
            {
                //var childShop_Goods_Receipt_Details = Shop_Goods_Receipts.Where(t => t.DateCreated.Value.Day == i).ToArray();
                var childshop_Goods_Issues_Details = shop_Goods_Issues.Where(t => t.DateCreated.Value.Day == i).ToArray();

                data.Add(new
                {
                    index = i,
                    value = childshop_Goods_Issues_Details.Select(t => t.Total).Sum()
                }); ;
            }

            return data;
        }

        private DateTime EndDate(DateTime endDate)
        {
            endDate.AddHours(12);

            return endDate;
        }
    }
}
