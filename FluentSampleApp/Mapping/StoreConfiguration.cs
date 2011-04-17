using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Steps;
using FluentNHibernate.Conventions;

namespace FluentNHSampleApp.Mapping
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
}
