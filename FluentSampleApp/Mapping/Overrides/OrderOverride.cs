using FluentNHSampleApp.Domain;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace FluentNHSampleApp.Mapping.Overrides
{
    public class OrderOverride : IAutoMappingOverride<Order>
    {
        #region IAutoMappingOverride<Order> Members

        public void Override(AutoMapping<Order> mapping)
        {
            mapping.Map(o => o.Total).Access.ReadOnly();
        }

        #endregion
    }
}