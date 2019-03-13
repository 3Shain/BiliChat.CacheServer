using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SanShain.Bilichat.Models
{
    public class Entry
    {
        public int id { get; set; }
        public DateTime entry_time { get; set; }
        public long user_id { get; set; }
        public string user_name { get; set; }
        public string user_intro { get; set; }
        public int room_id { get; set; }
    }

    public class EntryMap : IEntityTypeConfiguration<Entry>
    {
        void IEntityTypeConfiguration<Entry>.Configure(EntityTypeBuilder<Entry> builder)
        {
            builder.ToTable("entrys");

            builder.HasKey(x => x.id);
            builder.Property(x => x.id).ValueGeneratedOnAdd();
        }
    }
}
