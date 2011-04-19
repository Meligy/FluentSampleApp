using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHSampleApp.Domain;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHSampleApp.Mapping
{
    public class HiLoIdConvention : IIdConvention
    {
        public void Apply(IIdentityInstance instance)
        {
            instance.GeneratedBy.HiLo("1000");
        }
    }
}
