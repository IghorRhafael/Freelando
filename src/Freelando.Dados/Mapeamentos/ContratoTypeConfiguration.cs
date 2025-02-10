using Freelando.Modelo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelando.Dados.Mapeamentos;

internal class ContratoTypeConfiguration : IEntityTypeConfiguration<Contrato>
{
    public void Configure(EntityTypeBuilder<Contrato> entity)
    {
        entity.ToTable("TB_Contratos");
        entity.Property(e => e.Id).HasColumnName("ID_Contrato");
        
        entity.Property(e => e.DataInicio).HasColumnName("Data_Inicio");
        entity.Property(e => e.DataEncerramento).HasColumnName("Data_Encerramento");
    }
}