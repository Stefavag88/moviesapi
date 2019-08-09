using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public partial class MoviesDBContext : DbContext
    {
        public MoviesDBContext()
        {
        }

        public MoviesDBContext(DbContextOptions<MoviesDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Contrib> Contrib { get; set; }
        public virtual DbSet<Contribtype> Contribtype { get; set; }
        public virtual DbSet<ContribTypeMovie> ContribTypeMovie { get; set; }
        public virtual DbSet<Genre> Genre { get; set; }
        public virtual DbSet<Lang> Lang { get; set; }
        public virtual DbSet<Langtext> Langtext { get; set; }
        public virtual DbSet<Movie> Movie { get; set; }
        public virtual DbSet<MovieGenre> MovieGenre { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
        }

        public DbSet<T> GetDbSet<T>() where T : class
        {
            return Set<T>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contrib>(entity =>
            {
                entity.ToTable("CONTRIB");

                entity.Property(e => e.ContribId).HasColumnName("Contrib_Id");

                entity.Property(e => e.LangTextCode)
                    .IsRequired()
                    .HasColumnName("LangText_Code")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Contribtype>(entity =>
            {
                entity.ToTable("CONTRIBTYPE");

                entity.Property(e => e.ContribTypeId).HasColumnName("ContribType_Id");

                entity.Property(e => e.LangTextCode)
                    .IsRequired()
                    .HasColumnName("LangText_Code")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ContribTypeMovie>(entity =>
            {
                entity.HasKey(e => e.RecordId);

                entity.ToTable("CONTRIB_TYPE_MOVIE");

                entity.Property(e => e.RecordId).HasColumnName("Record_Id");

                entity.Property(e => e.ContribId).HasColumnName("Contrib_Id");

                entity.Property(e => e.ContribTypeId).HasColumnName("ContribType_Id");

                entity.Property(e => e.MovieId).HasColumnName("Movie_Id");

                entity.HasOne(d => d.Contrib)
                    .WithMany(p => p.ContribTypeMovie)
                    .HasForeignKey(d => d.ContribId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CONTRIBTYPEMOVIE_CONTRIB_ID");

                entity.HasOne(d => d.ContribType)
                    .WithMany(p => p.ContribTypeMovie)
                    .HasForeignKey(d => d.ContribTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CONTRIBTYPEMOVIE_CONTRIBTYPE_ID");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.ContribTypeMovie)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CONTRIBTYPEMOVIE_MOVIE_ID");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("GENRE");

                entity.Property(e => e.GenreId).HasColumnName("Genre_Id");

                entity.Property(e => e.LangTextCode)
                    .IsRequired()
                    .HasColumnName("LangText_Code")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Lang>(entity =>
            {
                entity.ToTable("LANG");

                entity.Property(e => e.LangId).HasColumnName("Lang_Id");

                entity.Property(e => e.LangCode)
                    .IsRequired()
                    .HasColumnName("Lang_Code")
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Langtext>(entity =>
            {
                entity.ToTable("LANGTEXT");

                entity.Property(e => e.LangTextId).HasColumnName("LangText_Id");

                entity.Property(e => e.ContribId).HasColumnName("Contrib_Id");

                entity.Property(e => e.ContribTypeId).HasColumnName("ContribType_Id");

                entity.Property(e => e.GenreId).HasColumnName("Genre_Id");

                entity.Property(e => e.LangId).HasColumnName("Lang_Id");

                entity.Property(e => e.MovieId).HasColumnName("Movie_Id");

                entity.Property(e => e.TextCode)
                    .IsRequired()
                    .HasColumnName("Text_Code")
                    .HasMaxLength(70)
                    .IsUnicode(false);

                entity.Property(e => e.TextDescription).HasColumnName("Text_Description");

                entity.Property(e => e.TextName)
                    .HasColumnName("Text_Name")
                    .HasMaxLength(50);

                entity.Property(e => e.TextTitle)
                    .HasColumnName("Text_Title")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Contrib)
                    .WithMany(p => p.Langtext)
                    .HasForeignKey(d => d.ContribId)
                    .HasConstraintName("FK_LANGTEXT_CONTRIB_ID");

                entity.HasOne(d => d.ContribType)
                    .WithMany(p => p.Langtext)
                    .HasForeignKey(d => d.ContribTypeId)
                    .HasConstraintName("FK_LANGTEXT_CONTRIBTYPE_ID");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.Langtext)
                    .HasForeignKey(d => d.GenreId)
                    .HasConstraintName("FK_LANGTEXT_GENRE_ID");

                entity.HasOne(d => d.Lang)
                    .WithMany(p => p.Langtext)
                    .HasForeignKey(d => d.LangId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LANGTEXT_LANG_ID");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.Langtext)
                    .HasForeignKey(d => d.MovieId)
                    .HasConstraintName("FK_LANGTEXT_MOVIE_ID");
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.ToTable("MOVIE");

                entity.Property(e => e.MovieId).HasColumnName("Movie_Id");

                entity.Property(e => e.LangTextCode)
                    .IsRequired()
                    .HasColumnName("LangText_Code")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MovieGenre>(entity =>
            {
                entity.HasKey(e => e.RecordId);

                entity.ToTable("MOVIE_GENRE");

                entity.Property(e => e.RecordId).HasColumnName("Record_Id");

                entity.Property(e => e.GenreId).HasColumnName("Genre_Id");

                entity.Property(e => e.MovieId).HasColumnName("Movie_Id");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.MovieGenre)
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MOVIEGENRE_GENRE_ID");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.MovieGenre)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MOVIEGENRE_MOVIE_ID");
            });
        }
    }
}
