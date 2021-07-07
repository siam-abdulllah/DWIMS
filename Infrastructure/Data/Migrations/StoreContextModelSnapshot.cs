﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Data.Migrations
{
    [DbContext(typeof(StoreContext))]
    partial class StoreContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Core.Entities.ApprAuthConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ApprovalAuthorityId")
                        .HasColumnType("int");

                    b.Property<int>("DataStatus")
                        .HasColumnType("int");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("SetOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ApprovalAuthorityId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("ApprAuthConfig");
                });

            modelBuilder.Entity("Core.Entities.ApprovalAuthority", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ApprovalAuthorityName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DataStatus")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<string>("Remarks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("SetOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ApprovalAuthority");
                });

            modelBuilder.Entity("Core.Entities.ApprovalCeiling", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Additional")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ApprovalAuthorityId")
                        .HasColumnType("int");

                    b.Property<int>("DataStatus")
                        .HasColumnType("int");

                    b.Property<DateTime?>("InvestmentFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("InvestmentTo")
                        .HasColumnType("datetime2");

                    b.Property<int>("InvestmentTypeId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Remarks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("SetOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("TransacionAmount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ApprovalAuthorityId");

                    b.HasIndex("InvestmentTypeId");

                    b.ToTable("ApprovalCeiling");
                });

            modelBuilder.Entity("Core.Entities.Bcds", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BcdsAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BcdsName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DataStatus")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NoOfMember")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("SetOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Bcds");
                });

            modelBuilder.Entity("Core.Entities.BrandInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BrandName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DataStatus")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("SetOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("BrandInfo");
                });

            modelBuilder.Entity("Core.Entities.CampaignDtl", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("Budget")
                        .HasColumnType("bigint");

                    b.Property<int>("DataStatus")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("MstId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("SetOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTime>("SubCampEndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("SubCampStartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("SubCampaignId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MstId");

                    b.HasIndex("SubCampaignId");

                    b.ToTable("CampaignDtl");
                });

            modelBuilder.Entity("Core.Entities.CampaignDtlProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DataStatus")
                        .HasColumnType("int");

                    b.Property<int>("DtlId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("SetOn")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("DtlId");

                    b.HasIndex("ProductId");

                    b.ToTable("CampaignDtlProduct");
                });

            modelBuilder.Entity("Core.Entities.CampaignMst", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BrandId")
                        .HasColumnType("int");

                    b.Property<string>("CampaignName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CampaignNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DataStatus")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("SBU")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("SetOn")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.ToTable("CampaignMst");
                });

            modelBuilder.Entity("Core.Entities.Donation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DataStatus")
                        .HasColumnType("int");

                    b.Property<string>("DonationTypeName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Remarks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("SetOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Donation");
                });

            modelBuilder.Entity("Core.Entities.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int>("DataStatus")
                        .HasColumnType("int");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("DepartmentName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DesignationId")
                        .HasColumnType("int");

                    b.Property<string>("DesignationName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DivisionCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DivisionName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmployeeName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmployeeSAPCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("JoiningDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("JoiningPlace")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MarketCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MarketName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostingType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RegionCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RegionName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SBU")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("SetOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("TerritoryCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TerritoryName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ZoneCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ZoneName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("Core.Entities.InvestmentType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DataStatus")
                        .HasColumnType("int");

                    b.Property<string>("InvesetmentTypeName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<string>("Remarks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("SetOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("InvestmentType");
                });

            modelBuilder.Entity("Core.Entities.MarketGroupDtl", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DataStatus")
                        .HasColumnType("int");

                    b.Property<string>("MarketCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MarketName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("MstId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("SetOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MstId");

                    b.ToTable("MarketGroupDtl");
                });

            modelBuilder.Entity("Core.Entities.MarketGroupMst", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DataStatus")
                        .HasColumnType("int");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<string>("GroupName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("SetOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("MarketGroupMst");
                });

            modelBuilder.Entity("Core.Entities.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DataStatus")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("PostDescription")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<string>("PostTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("PostedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("SetOn")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("Post");
                });

            modelBuilder.Entity("Core.Entities.PostComments", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CommentBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CommentText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DataStatus")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("NoOfDisLike")
                        .HasColumnType("int");

                    b.Property<int>("NoOfLike")
                        .HasColumnType("int");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("SetOn")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.ToTable("PostComments");
                });

            modelBuilder.Entity("Core.Entities.ProductInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DataStatus")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ProductCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("SetOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ProductInfo");
                });

            modelBuilder.Entity("Core.Entities.SBU", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DataStatus")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Remarks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SBUName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("SetOn")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("SBU");
                });

            modelBuilder.Entity("Core.Entities.SBUWiseBudget", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("Amount")
                        .HasColumnType("bigint");

                    b.Property<int>("DataStatus")
                        .HasColumnType("int");

                    b.Property<DateTime?>("FromDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Remarks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SBUId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("SetOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTime?>("ToDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("SBUId");

                    b.ToTable("SBUWiseBudget");
                });

            modelBuilder.Entity("Core.Entities.Society", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DataStatus")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NoOfMember")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("SetOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("SocietyAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SocietyName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Society");
                });

            modelBuilder.Entity("Core.Entities.SubCampaign", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DataStatus")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Remarks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("SetOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubCampaignName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SubCampaign");
                });

            modelBuilder.Entity("Core.Entities.ApprAuthConfig", b =>
                {
                    b.HasOne("Core.Entities.ApprovalAuthority", "ApprovalAuthority")
                        .WithMany()
                        .HasForeignKey("ApprovalAuthorityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Entities.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Entities.ApprovalCeiling", b =>
                {
                    b.HasOne("Core.Entities.ApprovalAuthority", "ApprovalAuthority")
                        .WithMany()
                        .HasForeignKey("ApprovalAuthorityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Entities.InvestmentType", "InvestmentType")
                        .WithMany()
                        .HasForeignKey("InvestmentTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Entities.CampaignDtl", b =>
                {
                    b.HasOne("Core.Entities.CampaignMst", null)
                        .WithMany("CampaignDtls")
                        .HasForeignKey("MstId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Entities.SubCampaign", "SubCampaign")
                        .WithMany()
                        .HasForeignKey("SubCampaignId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Entities.CampaignDtlProduct", b =>
                {
                    b.HasOne("Core.Entities.CampaignDtl", null)
                        .WithMany("CampaignDtlProducts")
                        .HasForeignKey("DtlId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Entities.ProductInfo", "ProductInfo")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Entities.CampaignMst", b =>
                {
                    b.HasOne("Core.Entities.BrandInfo", "BrandInfo")
                        .WithMany()
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Entities.MarketGroupDtl", b =>
                {
                    b.HasOne("Core.Entities.MarketGroupMst", null)
                        .WithMany("MarketGroupDtls")
                        .HasForeignKey("MstId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Entities.MarketGroupMst", b =>
                {
                    b.HasOne("Core.Entities.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Entities.PostComments", b =>
                {
                    b.HasOne("Core.Entities.Post", "Post")
                        .WithMany("PostComments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Entities.SBUWiseBudget", b =>
                {
                    b.HasOne("Core.Entities.SBU", "SBU")
                        .WithMany()
                        .HasForeignKey("SBUId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
