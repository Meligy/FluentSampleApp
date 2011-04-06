using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Automapping.Steps;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHSampleApp.Domain;
using FluentNHibernate.Conventions.Instances;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Tool.hbm2ddl;
using Environment = NHibernate.Cfg.Environment;

namespace FluentNHSampleApp
{
    public class StoreConfiguration : DefaultAutomappingConfiguration
    {
        public override IEnumerable<IAutomappingStep> GetMappingSteps(AutoMapper mapper,
                                                                      IConventionFinder conventionFinder)
        {
            var automappingSteps = base.GetMappingSteps(mapper, conventionFinder);
            return automappingSteps;
        }

        public override bool ShouldMap(Type type)
        {
            return type.Namespace == "FluentNHSampleApp.Domain" && !type.IsEnum;
        }
    }

    public class TotalPropertyOverride : IAutoMappingOverride<Order>
    {   
        public void Override(AutoMapping<Order> mapping)
        {
            mapping.Map(o => o.Total).Access.ReadOnly();
        }
    }

    //public class ProductCustomizationOverride : IAutoMappingOverride<Product>
    //{
    //    public void Override(AutoMapping<Product> mapping)
    //    {
    //        //mapping.HasMany(p => p.Customizations)
    //        //    .AsSet()
    //        //    .Cascade.All();
    //    }
    //}

    //public class CustomizationOverride : IAutoMappingOverride<Customization>
    //{
    //    public void Override(AutoMapping<Customization> mapping)
    //    {
            
    //        //mapping.HasMany(p => p.PossibleValues)
    //        //    .AsSet()
    //        //    .Element("value")
    //        //    .Cascade.All();
    //    }
    //}


    public class SetConvention : ICollectionConvention
    {
        public void Apply(ICollectionInstance instance)
        {
            instance.AsSet();
            instance.Cascade.All();

            if (instance.Key.EntityType.Assembly == typeof(string).Assembly) // BCL type
                instance.Name("Value");
        }
    }

    //It didn't work..
    //public class SetConvention : ICollectionConvention, ICollectionConventionAcceptance
    //{
    //    public void Apply(ICollectionInstance instance)
    //    {
    //        instance.AsSet();
    //    }

    //    public void Accept(IAcceptanceCriteria<ICollectionInspector> criteria)
    //    {
    //        criteria.Expect(c => c.Generic && c.CollectionType.GenericTypeDefinition == typeof (ISet<>));
    //    }
    //}

    //public class StringElementConvention : ICollectionConvention, ICollectionConventionAcceptance
    //{
    //    public void Apply(ICollectionInstance instance)
    //    {
    //        instance.as;
    //    }

    //    public void Accept(IAcceptanceCriteria<ICollectionInspector> criteria)
    //    {
    //        criteria.Expect(c => c.CollectionType.IsGenericTypeDefinition 
    //                          && c.CollectionType.GetGenericArguments()[0] == typeof(string));
    //    }
    //}


    internal class Program
    {
        private static void Main()
        {
            Configuration configuration = ConfigureNHibernate();

            Console.WriteLine("Generating the schema");
            new SchemaExport(configuration).Create(true, true);

            Console.WriteLine("Persiting some objects");
            ISessionFactory sf = configuration.BuildSessionFactory();
            //Console.ReadKey();
            using (ISession s = sf.OpenSession())
            using (ITransaction tx = s.BeginTransaction())
            {
                var product = new Product
                                  {
                                      Name = "Fideos"
                                  };

                product.Customizations.Add(
                    new Customization
                        {
                            Name = "Tuco",
                            PossibleValues = {"Pocon", "Medio", "Sopa"}
                        });

                s.Save(product);

                tx.Commit();
            }
            Console.ReadLine();
        }

        private static Configuration ConfigureNHibernate()
        {
            var storeConfiguration = new StoreConfiguration();
            var fluentConfig = Fluently.Configure()
                .Mappings(
                    m => m.AutoMappings.Add(
                        AutoMap.AssemblyOf<Program>(storeConfiguration)
                            .Conventions.Add<SetConvention>()
                            .UseOverridesFromAssembly(typeof(TotalPropertyOverride).Assembly)
                            )
                            .ExportTo(Console.Out))
                             .Database(() => MsSqlConfiguration
                                                 .MsSql2008.ConnectionString(
                                                     c => c.FromConnectionStringWithKey("NHibernateTest"))
                                                 .ShowSql().FormatSql()
                                                 .Raw(Environment.Hbm2ddlKeyWords, Hbm2DDLKeyWords.AutoQuote.ToString()))
                             .CollectionTypeFactory<Net4CollectionTypeFactory>();


            Configuration config = fluentConfig.BuildConfiguration();
            return config;
        }

        //This method is only used to show you in the console the nhibernate mappings in XML 
        protected static string Serialize(HbmMapping hbmElement)
        {
            var setting = new XmlWriterSettings {Indent = true};
            var serializer = new XmlSerializer(typeof (HbmMapping));
            using (var memStream = new MemoryStream())
            using (XmlWriter xmlWriter = XmlWriter.Create(memStream, setting))
            {
                serializer.Serialize(xmlWriter, hbmElement);
                memStream.Flush();
                memStream.Position = 0;

                var sr = new StreamReader(memStream);
                return sr.ReadToEnd();
            }
        }
    }
}