using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHSampleApp.Domain;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Steps;
using FluentNHibernate.Conventions;

namespace FluentNHSampleApp.Mapping
{
    public class StoreConfiguration : DefaultAutomappingConfiguration
    {
        public override bool IsConcreteBaseType(Type type)
        {
            return type == typeof(EntityBase);
        }

        public override bool ShouldMap(Type type)
        {
            return type.Namespace == typeof (EntityBase).Namespace
                   && type.IsEnum == false
                   && type != typeof (EntityBase);
        }
    }
}
