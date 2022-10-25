using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.EntityTypeConfiguration
{
    public class StreetEntityTypeConfiguration: IEntityTypeConfiguration<Street>
    {
        public void Configure(EntityTypeBuilder<Street> builder)
        {
            builder
                .ToTable("Streets");

            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Name)
                .HasColumnName("StreetName")
                .HasMaxLength(500)
                .IsRequired();

            builder
                .HasOne(x => x.ZipCode)
                .WithMany(x => x.Streets)
                .IsRequired();
        }
    }
}
