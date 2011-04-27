using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHSampleApp.Mapping
{
    public class CollectionConvention : ICollectionConvention
    {
        #region ICollectionConvention Members

        public void Apply(ICollectionInstance instance)
        {
            instance.AsSet();
            instance.Cascade.All();
        }

        #endregion
    }
}