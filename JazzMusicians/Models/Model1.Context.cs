namespace JazzMusicians.Models
{

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


public partial class JazzMusicianEntities : DbContext
{
    public JazzMusicianEntities()
        : base("name=JazzMusicianEntities")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public virtual DbSet<Musician> Musicians { get; set; }

}

}

