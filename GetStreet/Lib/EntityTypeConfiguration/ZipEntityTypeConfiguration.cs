using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Lib.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.EntityTypeConfiguration
{
    public class ZipEntityTypeConfiguration: IEntityTypeConfiguration<ZipCode>
    {
        public void Configure(EntityTypeBuilder<ZipCode> builder)
        {
            builder
                .ToTable("ZipCodes");

            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Zip)
                .HasColumnName("Code")
                .HasMaxLength(10)
                .IsRequired();

            builder.HasAlternateKey(x => x.Zip)
                .HasName("UXC_ZipCodes_Code");
        }
    }
}
