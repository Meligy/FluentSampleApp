using System;
using FluentNHSampleApp.Domain;
using FluentNHibernate.Automapping;

namespace FluentNHSampleApp.Mapping
{
    public class StoreConfiguration : DefaultAutomappingConfiguration
    {
        public override bool IsConcreteBaseType(Type type)
        {
            return type == typeof (EntityBase);
        }

        public override bool ShouldMap(Type type)
        {
            return type.Namespace == typeof (EntityBase).Namespace
                   && type.IsEnum == false
                   && type != typeof (EntityBase);
        }
    }
}