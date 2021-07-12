using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class DB_WareHouseMangerContext : DbContext
    {
        public DB_WareHouseMangerContext()
        {
        }

        public DB_WareHouseMangerContext(DbContextOptions<DB_WareHouseMangerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Account_Roles_Detail> Account_Roles_Details { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Customer_Category> Customer_Categories { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<FinalSettlement_Customer> FinalSettlement_Customers { get; set; }
        public virtual DbSet<FinalSettlement_Suplier> FinalSettlement_Supliers { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Producer> Producers { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Roles_Detail> Roles_Details { get; set; }
        public virtual DbSet<Shop_Good> Shop_Goods { get; set; }
        public virtual DbSet<Shop_Goods_Category> Shop_Goods_Categories { get; set; }
        public virtual DbSet<Shop_Goods_Issue> Shop_Goods_Issues { get; set; }
        public virtual DbSet<Shop_Goods_Issues_Detail> Shop_Goods_Issues_Details { get; set; }
        public virtual DbSet<Shop_Goods_Receipt> Shop_Goods_Receipts { get; set; }
        public virtual DbSet<Shop_Goods_Receipt_Detail> Shop_Goods_Receipt_Details { get; set; }
        public virtual DbSet<Shop_Goods_StockTake> Shop_Goods_StockTakes { get; set; }
        public virtual DbSet<Shop_Goods_StockTake_Detail> Shop_Goods_StockTake_Details { get; set; }
        public virtual DbSet<Shop_Goods_Unit> Shop_Goods_Units { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<WareHouse> WareHouses { get; set; }
        public virtual DbSet<WareHouse_Goods_Detail> WareHouse_Goods_Details { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-Q7IUOUM;Initial Catalog=DB_WareHouseManger;User ID=sa;Password=123456;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.DateCreated).HasColumnType("date");

                entity.Property(e => e.Password)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.EmployeeID)
                    .HasConstraintName("FK_Account_Employee");
            });

            modelBuilder.Entity<Account_Roles_Detail>(entity =>
            {
                entity.HasKey(e => new { e.AccountID, e.ID });

                entity.ToTable("Account_Roles_Detail");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Account_Roles_Details)
                    .HasForeignKey(d => d.AccountID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_Roles_Detail_Account");

                entity.HasOne(d => d.IDNavigation)
                    .WithMany(p => p.Account_Roles_Details)
                    .HasForeignKey(d => d.ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_Roles_Detail_Roles_Detail");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.Address).HasMaxLength(1000);

                entity.Property(e => e.EMail)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.HasOne(d => d.CustomerCategory)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.CustomerCategoryID)
                    .HasConstraintName("FK_Customer_Customer_Category");
            });

            modelBuilder.Entity<Customer_Category>(entity =>
            {
                entity.HasKey(e => e.CustomerCategoryID);

                entity.ToTable("Customer_Category");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");

                entity.Property(e => e.Address).HasMaxLength(1000);

                entity.Property(e => e.EMail)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.PositionID)
                    .HasConstraintName("FK_Employee_Position");
            });

            modelBuilder.Entity<FinalSettlement_Customer>(entity =>
            {
                entity.ToTable("FinalSettlement_Customer");

                entity.Property(e => e.ID).ValueGeneratedNever();

                entity.Property(e => e.DateCreated).HasColumnType("date");

                entity.Property(e => e.GoodsIssuesID)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Payment).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Remainder).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.FinalSettlement_Customers)
                    .HasForeignKey(d => d.CustomerID)
                    .HasConstraintName("FK_FinalSettlement_Customer_Customer");

                entity.HasOne(d => d.GoodsIssues)
                    .WithMany(p => p.FinalSettlement_Customers)
                    .HasForeignKey(d => d.GoodsIssuesID)
                    .HasConstraintName("FK_FinalSettlement_Customer_Shop_Goods_Issues");
            });

            modelBuilder.Entity<FinalSettlement_Suplier>(entity =>
            {
                entity.ToTable("FinalSettlement_Suplier");

                entity.Property(e => e.ID).ValueGeneratedNever();

                entity.Property(e => e.DateCreated).HasColumnType("date");

                entity.Property(e => e.GoodsReceiptID)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Payment).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Remainder).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.GoodsReceipt)
                    .WithMany(p => p.FinalSettlement_Supliers)
                    .HasForeignKey(d => d.GoodsReceiptID)
                    .HasConstraintName("FK_FinalSettlement_Suplier_Shop_Goods_Receipt");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.FinalSettlement_Supliers)
                    .HasForeignKey(d => d.SupplierID)
                    .HasConstraintName("FK_FinalSettlement_Suplier_Supplier");
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.ToTable("Position");

                entity.Property(e => e.PositionID).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Remark).HasMaxLength(1000);
            });

            modelBuilder.Entity<Producer>(entity =>
            {
                entity.ToTable("Producer");

                entity.Property(e => e.Address).HasMaxLength(1000);

                entity.Property(e => e.EMail)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.SortName)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Roles_Detail>(entity =>
            {
                entity.ToTable("Roles_Detail");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Roles_Details)
                    .HasForeignKey(d => d.RoleID)
                    .HasConstraintName("FK_Roles_Detail_Roles");
            });

            modelBuilder.Entity<Shop_Good>(entity =>
            {
                entity.HasKey(e => e.TemplateID);

                entity.Property(e => e.TemplateID)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Shop_Goods)
                    .HasForeignKey(d => d.CategoryID)
                    .HasConstraintName("FK_Shop_Goods_Shop_Goods_Category");

                entity.HasOne(d => d.Producer)
                    .WithMany(p => p.Shop_Goods)
                    .HasForeignKey(d => d.ProducerID)
                    .HasConstraintName("FK_Shop_Goods_Producer");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.Shop_Goods)
                    .HasForeignKey(d => d.UnitID)
                    .HasConstraintName("FK_Shop_Goods_Shop_Goods_Unit");
            });

            modelBuilder.Entity<Shop_Goods_Category>(entity =>
            {
                entity.HasKey(e => e.CategoryID)
                    .HasName("PK_Shop_Goods_Categorys");

                entity.ToTable("Shop_Goods_Category");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.SortName)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Shop_Goods_Issue>(entity =>
            {
                entity.HasKey(e => e.GoodsIssueID);

                entity.Property(e => e.GoodsIssueID)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.DateCreated).HasColumnType("date");

                entity.Property(e => e.Remark).HasMaxLength(1000);

                entity.Property(e => e.Total).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Shop_Goods_Issues)
                    .HasForeignKey(d => d.CustomerID)
                    .HasConstraintName("FK_Shop_Goods_Issues_Customer");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Shop_Goods_Issues)
                    .HasForeignKey(d => d.EmployeeID)
                    .HasConstraintName("FK_Shop_Goods_Issues_Employee");
            });

            modelBuilder.Entity<Shop_Goods_Issues_Detail>(entity =>
            {
                entity.HasKey(e => new { e.GoodsIssueID, e.TemplateID })
                    .HasName("PK_Shop_Goods_Issues_Details");

                entity.ToTable("Shop_Goods_Issues_Detail");

                entity.Property(e => e.GoodsIssueID)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.TemplateID)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.HasOne(d => d.GoodsIssue)
                    .WithMany(p => p.Shop_Goods_Issues_Details)
                    .HasForeignKey(d => d.GoodsIssueID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Shop_Goods_Issues_Detail_Shop_Goods_Issues");

                entity.HasOne(d => d.Template)
                    .WithMany(p => p.Shop_Goods_Issues_Details)
                    .HasForeignKey(d => d.TemplateID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Shop_Goods_Issues_Detail_Shop_Goods");
            });

            modelBuilder.Entity<Shop_Goods_Receipt>(entity =>
            {
                entity.HasKey(e => e.GoodsReceiptID)
                    .HasName("PK_Shop_Goods_Received");

                entity.ToTable("Shop_Goods_Receipt");

                entity.Property(e => e.GoodsReceiptID)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.DateCreated).HasColumnType("date");

                entity.Property(e => e.Remark).HasMaxLength(1000);

                entity.Property(e => e.Total).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Shop_Goods_Receipts)
                    .HasForeignKey(d => d.EmployeeID)
                    .HasConstraintName("FK_Shop_Goods_Receipt_Employee");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Shop_Goods_Receipts)
                    .HasForeignKey(d => d.SupplierID)
                    .HasConstraintName("FK_Shop_Goods_Receipt_Supplier");
            });

            modelBuilder.Entity<Shop_Goods_Receipt_Detail>(entity =>
            {
                entity.HasKey(e => new { e.GoodsReceiptID, e.TemplateID })
                    .HasName("PK_Shop_Goods_Receipt_Details");

                entity.ToTable("Shop_Goods_Receipt_Detail");

                entity.Property(e => e.GoodsReceiptID)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.TemplateID)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.HasOne(d => d.GoodsReceipt)
                    .WithMany(p => p.Shop_Goods_Receipt_Details)
                    .HasForeignKey(d => d.GoodsReceiptID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Shop_Goods_Receipt_Detail_Shop_Goods_Receipt");

                entity.HasOne(d => d.Template)
                    .WithMany(p => p.Shop_Goods_Receipt_Details)
                    .HasForeignKey(d => d.TemplateID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Shop_Goods_Receipt_Detail_Shop_Goods");
            });

            modelBuilder.Entity<Shop_Goods_StockTake>(entity =>
            {
                entity.HasKey(e => e.StockTakeID)
                    .HasName("PK_Shop_Goods_StockTakes");

                entity.ToTable("Shop_Goods_StockTake");

                entity.Property(e => e.StockTakeID)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.DateCreated).HasColumnType("date");

                entity.Property(e => e.Remark).HasMaxLength(1000);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Shop_Goods_StockTakes)
                    .HasForeignKey(d => d.EmployeeID)
                    .HasConstraintName("FK_Shop_Goods_StockTake_Employee");
            });

            modelBuilder.Entity<Shop_Goods_StockTake_Detail>(entity =>
            {
                entity.HasKey(e => new { e.StockTakeID, e.TemplateID })
                    .HasName("PK_Shop_Goods_StockTakes_Details");

                entity.ToTable("Shop_Goods_StockTake_Detail");

                entity.Property(e => e.StockTakeID)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.TemplateID)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Remark).HasMaxLength(1000);

                entity.HasOne(d => d.StockTake)
                    .WithMany(p => p.Shop_Goods_StockTake_Details)
                    .HasForeignKey(d => d.StockTakeID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Shop_Goods_StockTake_Detail_Shop_Goods_StockTake");

                entity.HasOne(d => d.Template)
                    .WithMany(p => p.Shop_Goods_StockTake_Details)
                    .HasForeignKey(d => d.TemplateID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Shop_Goods_StockTake_Detail_Shop_Goods");
            });

            modelBuilder.Entity<Shop_Goods_Unit>(entity =>
            {
                entity.HasKey(e => e.UnitID)
                    .HasName("PK_Shop_Goods_Units");

                entity.ToTable("Shop_Goods_Unit");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.ToTable("Supplier");

                entity.Property(e => e.Address).HasMaxLength(1000);

                entity.Property(e => e.EMail)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<WareHouse>(entity =>
            {
                entity.ToTable("WareHouse");

                entity.Property(e => e.ID).ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(1000);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<WareHouse_Goods_Detail>(entity =>
            {
                entity.HasKey(e => new { e.WareHouseID, e.TemplateID });

                entity.ToTable("WareHouse_Goods_Detail");

                entity.Property(e => e.TemplateID)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
