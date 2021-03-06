﻿using NPJe.FrontEnd.Models;
using System.Data.Entity.ModelConfiguration;

namespace NPJe.FrontEnd.Repository.Mapping
{
    public class GrupoMap : EntityTypeConfiguration<Grupo>
    {
        public GrupoMap()
        {
            ToTable("grupo");

            HasOptional(x => x.UsuarioExclusao)
                .WithMany()
                .HasForeignKey(x => x.IdUsuarioExclusao);
        }
    }
}