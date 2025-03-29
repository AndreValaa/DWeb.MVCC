using DWeb_MVC.Models;

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

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
	    base.OnModelCreating(modelBuilder);
      }

	}
}