using Freelando.Modelo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelando.Dados.Mapeamentos;

internal class ServicoTypeConfiguration : IEntityTypeConfiguration<Servico>
{
    public void Configure(EntityTypeBuilder<Servico> entity)
    {
        entity.ToTable("TB_Servicos");
        entity.Property(e => e.Id).HasColumnName("ID_Servico");
        entity.Property(e => e.Titulo).HasColumnName("Titulo");
        entity.Property(e => e.Descricao).HasColumnName("DS_Projeto");
        entity.Property(e => e.Status)
                     .HasConversion(
                         v => v.ToString(), // Converte o enum para string ao salvar no banco de dados
                         v => (StatusServico)Enum.Parse(typeof(StatusServico), v) // Converte a string para enum ao ler do banco de dados
                     )
                     .HasColumnName("Status");
    }
}
