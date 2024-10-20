using MeterReaderAPI.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace MeterReaderAPI
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
        }

        public DbSet<Notebook> Notebooks { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Street> Streets { get; set; }
        public DbSet<Error> Errors { get; set; }
    }
}
