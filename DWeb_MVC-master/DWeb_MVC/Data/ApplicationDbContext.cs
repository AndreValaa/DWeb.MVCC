using DWeb_MVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DWeb_MVC.Data {

   /// <summary>
   /// esta classe representa a Base de Dados do nosso projeto
   /// </summary>
   public class ApplicationDbContext : IdentityDbContext {

      public ApplicationDbContext(
         DbContextOptions<ApplicationDbContext> options)
          : base(options) {
      }

      /* *********************************************
       * Criação das Tabelas
       * ********************************************* */
      public DbSet<Clientes> Clientes { get; set; }
      public DbSet<Compras> Compras { get; set; }
      public DbSet<Produtos> Produtos { get; set; }
      public DbSet<Categorias> Categorias { get; set; }
      public DbSet<Fotografias> Fotografias { get; set; }
      public DbSet<DetalhesCompras> DetalhesCompras { get; set; }
      public DbSet<Cores> Cores { get; set; }
      public DbSet<Grupos> Grupos { get; set; }

        public DbSet<Tamanhos> Tamanhos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
	    base.OnModelCreating(modelBuilder);

            //Adicionar o Role de admin
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "admin", NormalizedName = "ADMIN", Name = "admin"}
            );

            //Adicioar o User admin
            modelBuilder.Entity<IdentityUser>().HasData(
                new IdentityUser
                {
                    Id = "028f5626-60cd-473b-9713-8ffdad5b47f3",
                    UserName = "admin@gmail.com",
                    NormalizedUserName = "ADMIN@GMAIL.COM",
                    Email = "admin@gmail.com",
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAIAAYagAAAAECTsx4WD4Iz/v5MUj88/B3qeU2l+B3ssQ8HbnMDwUFWcAGgbhQlZfbs6Bw8HcIZD2A==",
                    SecurityStamp = "WFKPRDAUOFEZQNIZ7NKO4R6O3EGGIBCM",
                    ConcurrencyStamp = "944a1ad5-6a5b-4d20-b556-ac2017faa24a",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0
                }

            );

            //Adicionar o role "admin" ao user "admin@gmail.com"
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = "028f5626-60cd-473b-9713-8ffdad5b47f3", RoleId = "admin" }
            );
        }
	}
}