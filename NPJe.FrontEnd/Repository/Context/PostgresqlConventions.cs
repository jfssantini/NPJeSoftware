using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Pluralization;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text.RegularExpressions;

namespace NPJe.FrontEnd.Repository.Context
{
    public class PostgresqlConventions
    {

    static class Identifiers
    {
        public static string CreateIdentifier(string entityName)
        {
            return entityName.ToLower();
        }
    }

    public class AllCapsTableAndColumnConvention : System.Data.Entity.ModelConfiguration.Conventions.Convention
    {

        public AllCapsTableAndColumnConvention()
        {
            var ps = (IPluralizationService)DbConfiguration.DependencyResolver.GetService(typeof(IPluralizationService), null);

            this.Types().Configure(t => t.ToTable(Identifiers.CreateIdentifier(ps.Pluralize(t.ClrType.Name))));
            this.Properties().Configure(p => p.HasColumnName(Identifiers.CreateIdentifier(p.ClrPropertyInfo.Name)));

        }
    }
    public class AllCapsForeignKeyConvention : IStoreModelConvention<AssociationType>
    {

        public void Apply(AssociationType association, DbModel model)
        {
            // Identify ForeignKey properties (including IAs)  
            if (association.IsForeignKey)
            {
                // rename FK columns  
                var constraint = association.Constraint;
                foreach (var p in constraint.FromProperties.Union(constraint.ToProperties))
                {
                    p.Name = Identifiers.CreateIdentifier(p.Name);
                }

            }
        }

    }
}
}