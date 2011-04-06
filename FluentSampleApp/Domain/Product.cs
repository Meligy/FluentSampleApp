using System.Collections.Generic;

namespace FluentNHSampleApp.Domain
{
    public class Product : EntityBase
    {
        private readonly ISet<Customization> customizations;

        public Product()
        {
            customizations = new HashSet<Customization>();
        }

        public virtual string Name { get; set; }
        public virtual decimal Price { get; set; }

        public virtual ISet<Customization> Customizations
        {
            get { return customizations; }
        }
    }

    public class Customization : EntityBase
    {
        private readonly ISet<string> possibleValues;

        public Customization()
        {
            possibleValues = new HashSet<string>();
        }

        public virtual string Name { get; set; }

        public virtual ISet<string> PossibleValues
        {
            get { return possibleValues; }
        }
    }
}