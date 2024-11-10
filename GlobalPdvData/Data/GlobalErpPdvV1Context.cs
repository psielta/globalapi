using System;
using System.Collections.Generic;
using GlobalPdvData.Models;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace GlobalPdvData.Data;

public partial class GlobalErpPdvV1Context : DbContext
{
    public GlobalErpPdvV1Context()
    {
    }

    public GlobalErpPdvV1Context(DbContextOptions<GlobalErpPdvV1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Cidade> Cidades { get; set; }

    public virtual DbSet<Empresa> Empresas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(IniFile.GetConnectionStringPDV());
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pg_catalog", "adminpack");

        modelBuilder.Entity<Cidade>(entity =>
        {
            entity.HasKey(e => e.CdCidade).HasName("cidade_pkey");

            entity.Property(e => e.Uf).HasDefaultValueSql("'XX'::character varying");
        });

        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.HasKey(e => e.CdEmpresa).HasName("empresa_pkey");

            entity.Property(e => e.AutorizoXml).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.CdCnpj).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CpfcnpfAutorizado).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.EMail).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NmBairro).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NrInscrEstadual).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NrInscrMunicipal).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Telefone).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.TipoRegime).HasDefaultValue(1);
            entity.Property(e => e.TxtObs).HasDefaultValueSql("''::character varying");

            entity.HasOne(d => d.CdCidadeNavigation).WithMany(p => p.Empresas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("empresa_fk");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.NmUsuario).HasName("usuario_pkey");

            entity.Property(e => e.Ativo).HasDefaultValueSql("'S'::character varying");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.Usuarios).HasConstraintName("usuario_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
