//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Khabirova41Size
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        public Order()
        {
            this.OrderProduct = new HashSet<OrderProduct>();
        }
    
        public int OrderID { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public Nullable<System.DateTime> OrderDeliveryDate { get; set; }
        public Nullable<int> OrderPickupPoint { get; set; }
        public Nullable<int> OrderUserID { get; set; }
        public string OrderReceiptCode { get; set; }
        public string OrderStatus { get; set; }
    
        public virtual ICollection<OrderProduct> OrderProduct { get; set; }
        public virtual PickUpPoint PickUpPoint { get; set; }
        public virtual User User { get; set; }
    }
}
