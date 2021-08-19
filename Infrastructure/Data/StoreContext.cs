using System;
using System.Linq;
using System.Reflection;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }
        public DbSet<Post> Post { get; set; }
        public DbSet<PostComments> PostComments { get; set; }
        public DbSet<Donation> Donation { get; set; }
        public DbSet<SubCampaign> SubCampaign { get; set; }
        public DbSet<CampaignMst> CampaignMst { get; set; }
        public DbSet<CampaignDtl> CampaignDtl { get; set; }
        public DbSet<MarketGroupMst> MarketGroupMst { get; set; }
        public DbSet<MarketGroupDtl> MarketGroupDtl { get; set; }
        public DbSet<CampaignDtlProduct> CampaignDtlProduct { get; set; }
        public DbSet<ProductInfo> ProductInfo { get; set; }
        public DbSet<BrandInfo> BrandInfo { get; set; }
        public DbSet<Bcds> Bcds { get; set; }
        public DbSet<Society> Society { get; set; }
        public DbSet<ApprovalTimeLimit> ApprovalTimeLimit { get; set; }
        public DbSet<ApprovalAuthority> ApprovalAuthority { get; set; }
        public DbSet<ApprAuthConfig> ApprAuthConfig { get; set; }
        public DbSet<ApprovalCeiling> ApprovalCeiling { get; set; }
        public DbSet<InvestmentType> InvestmentType { get; set; }
        public DbSet<SBU> SBU { get; set; }
        public DbSet<SBUWiseBudget> SBUWiseBudget { get; set; }
        public DbSet<DoctorInfo> DoctorInfo { get; set; }
        public DbSet<DoctorMarket> DoctorMarket { get; set; }
        public DbSet<DoctorHonAppr> DoctorHonAppr { get; set; }
        public DbSet<InstitutionInfo> InstitutionInfo { get; set; }
        public DbSet<InvestmentBcds> InvestmentBcds { get; set; }
        public DbSet<InvestmentCampaign> InvestmentCampaign { get; set; }
        public DbSet<InvestmentDetail> InvestmentDetail { get; set; }
        public DbSet<InvestmentDoctor> InvestmentDoctor { get; set; }
        public DbSet<InvestmentInit> InvestmentInit { get; set; }
        public DbSet<InvestmentInstitution> InvestmentInstitution { get; set; }
        public DbSet<InvestmentSociety> InvestmentSociety { get; set; }
        public DbSet<InvestmentTargetedGroup> InvestmentTargetedGroup { get; set; }
        public DbSet<InvestmentTargetedProd> InvestmentTargetedProd { get; set; }
        public DbSet<InvestmentApr> InvestmentApr { get; set; }
        public DbSet<InvestmentAprComment> InvestmentAprComment { get; set; }
        public DbSet<InvestmentAprProducts> InvestmentAprProducts { get; set; }
        public DbSet<InvestmentRec> InvestmentRec { get; set; }
        public DbSet<InvestmentRecComment> InvestmentRecComment { get; set; }
        public DbSet<InvestmentRecProducts> InvestmentRecProducts { get; set; }
        public DbSet<ReportProductInfo> ReportProductInfo { get; set; }
        public DbSet<ReportInvestmentInfo> ReportInvestmentInfo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(decimal));
                    var dateTimeProperties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(DateTimeOffset));

                    foreach (var property in properties)
                    {
                        modelBuilder.Entity(entityType.Name).Property(property.Name).HasConversion<double>();
                    }

                    foreach (var property in dateTimeProperties)
                    {
                        modelBuilder.Entity(entityType.Name).Property(property.Name)
                            .HasConversion(new DateTimeOffsetToBinaryConverter());
                    }
                }
            }
        }
    }
}