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

            // Workaround for FNH problem when mapping collections of simple 
            //  .NET types, not collections of entities.
            if (instance.Key.EntityType.Assembly
                            == typeof(string).Assembly) // BCL type
                instance.Name("Value");
        }

        #endregion
    }
}