using Freelando.Modelo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelando.Dados.Mapeamentos;

internal class CandidaturaTypeConfiguration : IEntityTypeConfiguration<Candidatura>
{
    public void Configure(EntityTypeBuilder<Candidatura> entity)
    {
        entity.ToTable("TB_Candidaturas");
        entity.Property(e => e.Id).HasColumnName("ID_Candidatura");
        entity.Property(e => e.ValorProposto).HasColumnName("Valor_Proposto");
        entity.Property(e => e.DescricaoProposta).HasColumnName("DS_Proposta");
        entity.Property(e => e.DuracaoProposta).HasConversion(
                          v => v.ToString(), // Converte o enum para string ao salvar no banco de dados
                          v => (DuracaoEmDias)Enum.Parse(typeof(DuracaoEmDias), v) // Converte a string para enum ao ler do banco de dados
                      )
                      .HasColumnName("Duracao_Proposta");
        entity.Property(e => e.Status)
                     .HasConversion(
                         v => v.ToString(), // Converte o enum para string ao salvar no banco de dados
                         v => (StatusCandidatura)Enum.Parse(typeof(StatusCandidatura), v) // Converte a string para enum ao ler do banco de dados
                     )
                     .HasColumnName("Status");
    }
}
