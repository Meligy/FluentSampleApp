using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentNHSampleApp.Domain
{
	public class Order : EntityBase
	{
	    public Order()
	    {
	        Items = new HashSet<OrderItem>();
	    }

	    public virtual DateTime Date { get; set; }
        public virtual OrderStatus Status { get; set; }
	    public virtual Location Location { get; set; }
	    public virtual ISet<OrderItem> Items { get; private set; }

	    public virtual decimal Total { get { return Items.Sum(i => i.Quantity*i.UnitPrice); } }
	}

    public class OrderItem : EntityBase
    {
        public OrderItem()
        {
            Preferences = new Dictionary<string, string>();
            Quantity = 1;
        }

        public virtual Product Product { get; set; }
        public virtual int Quantity { get; set; }
        public virtual decimal UnitPrice { get; set; }
        public virtual IDictionary<string, string> Preferences { get; private set; }
    }
}