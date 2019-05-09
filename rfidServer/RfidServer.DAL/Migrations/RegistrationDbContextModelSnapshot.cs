﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RfidServer.DAL;

namespace RfidServer.DAL.Migrations
{
    [DbContext(typeof(RegistrationDbContext))]
    partial class RegistrationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity("RfidServer.DAL.Entity.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Date");

                    b.Property<string>("Email");

                    b.Property<string>("Login");

                    b.Property<string>("Name");

                    b.Property<int>("Points");

                    b.Property<string>("RegTime");

                    b.Property<string>("RegType");

                    b.Property<bool>("Registered");

                    b.Property<string>("Update");

                    b.Property<int>("VariantId");

                    b.Property<string>("Who");

                    b.Property<int>("WisId");

                    b.Property<int>("WisPersonId");

                    b.HasKey("Id");

                    b.HasIndex("VariantId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("RfidServer.DAL.Entity.Variant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CourseAbbrv");

                    b.Property<int>("Limit");

                    b.Property<int>("Points");

                    b.Property<string>("Sem");

                    b.Property<string>("Title");

                    b.Property<int>("WisCourseId");

                    b.Property<int>("WisId");

                    b.Property<int>("WisItemId");

                    b.Property<int>("Year");

                    b.HasKey("Id");

                    b.ToTable("Variants");
                });

            modelBuilder.Entity("RfidServer.DAL.Entity.Student", b =>
                {
                    b.HasOne("RfidServer.DAL.Entity.Variant", "RegisteredVariant")
                        .WithMany("Students")
                        .HasForeignKey("VariantId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
