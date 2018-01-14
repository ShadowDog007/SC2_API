﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using SandwichClub.Api.Repositories;
using System;

namespace SandwichClub.Api.Migrations
{
    [DbContext(typeof(ScContext))]
    [Migration("20171211080845_NewPaymentSystem")]
    partial class NewPaymentSystem
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("SandwichClub.Api.Repositories.Models.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("Payment");
                });

            modelBuilder.Entity("SandwichClub.Api.Repositories.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AvatarUrl");

                    b.Property<string>("BankDetails");

                    b.Property<string>("BankName");

                    b.Property<string>("Email");

                    b.Property<string>("FacebookId");

                    b.Property<bool>("FirstLogin");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("Shopper");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SandwichClub.Api.Repositories.Models.Week", b =>
                {
                    b.Property<int>("WeekId")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Cost");

                    b.Property<int?>("ShopperUserId");

                    b.HasKey("WeekId");

                    b.ToTable("Weeks");
                });

            modelBuilder.Entity("SandwichClub.Api.Repositories.Models.WeekUserLink", b =>
                {
                    b.Property<int>("WeekId");

                    b.Property<int>("UserId");

                    b.Property<double>("Paid");

                    b.Property<int>("Slices");

                    b.HasKey("WeekId", "UserId");

                    b.ToTable("WeekUserLinks");
                });
#pragma warning restore 612, 618
        }
    }
}
