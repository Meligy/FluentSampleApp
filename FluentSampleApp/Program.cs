using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using FluentNHSampleApp.Domain;
using FluentNHSampleApp.Mapping;
using FluentNHSampleApp.Mapping.Overrides;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Tool.hbm2ddl;
using Environment = NHibernate.Cfg.Environment;

namespace FluentNHSampleApp
{
    internal class Program
    {
        private static void Main()
        {
            Configuration configuration = ConfigureNHibernate();

            Console.WriteLine("Generating the schema");
            new SchemaExport(configuration).Create(true, true);

            Console.WriteLine("Persiting some objects");
            using (ISessionFactory sf = configuration.BuildSessionFactory())
            using (ISession s = sf.OpenSession())
            using (ITransaction tx = s.BeginTransaction())
            {
                var product = new Product
                                  {
                                      Name = "Fideos",
                                      Customizations =
                                          {
                                              new Customization
                                                  {
                                                      Name = "Tuco",
                                                      PossibleValues = {"Pocon", "Medio", "Sopa"}
                                                  }
                                          }
                                  };

                var order = new Order() {Date = DateTime.Now};
                order.Items.Add(new OrderItem {Product = product});

                s.Save(product);
                s.Save(order);

                tx.Commit();
            }
        }

        private static Configuration ConfigureNHibernate()
        {
            var storeConfiguration = new StoreConfiguration();
            FluentConfiguration fluentConfig = Fluently.Configure()
                .Mappings(m => new DefaultAutomappingConfiguration())
                .Mappings(
                    m => m.AutoMappings.Add(
                        AutoMap.AssemblyOf<Program>(storeConfiguration)
                            .Conventions.Add<EnumConvention>()
                            .Conventions.Add<CollectionConvention>()
                            .Conventions.Add<HiLoIdConvention>()
                            .UseOverridesFromAssembly(typeof (OrderOverride).Assembly))
                             .ExportTo(Console.Out)
                )
                .Database(
                    () => MsSqlConfiguration.MsSql2008
                              .ConnectionString(
                                  c => c.FromConnectionStringWithKey("NHibernateTest"))
                              .ShowSql().FormatSql()
                              .Raw(Environment.Hbm2ddlKeyWords,
                                   Hbm2DDLKeyWords.AutoQuote.ToString())
                )
                .CollectionTypeFactory<Net4CollectionTypeFactory>();

            return fluentConfig.BuildConfiguration();
        }
    }
}