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
    
    public partial class PickUpPoint
    {
        public PickUpPoint()
        {
            this.Order = new HashSet<Order>();
        }
    
        public int PickUpPointID { get; set; }
        public string PickUpPointIndex { get; set; }
        public string PickUpCity { get; set; }
        public string PickUpStreet { get; set; }
        public string PickUpHouseNumber { get; set; }
    
        public virtual ICollection<Order> Order { get; set; }
    }
}
