﻿using Microsoft.EntityFrameworkCore;
using ArtworkService.Models;

namespace ArtworkService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Artwork> Artwork { get; set; }

    }
}
