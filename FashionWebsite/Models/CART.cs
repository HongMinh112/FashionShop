//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FashionWebsite.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CART
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CART()
        {
            this.PRODUCTs = new HashSet<PRODUCT>();
        }
    
        public int ID { get; set; }
        public Nullable<int> MEMBERID { get; set; }
        public Nullable<int> PRODUCTID { get; set; }
        public string NAME { get; set; }
        public Nullable<int> QUANTITY { get; set; }
        public Nullable<decimal> PRICE { get; set; }
        public Nullable<decimal> TOTAL { get; set; }
        public string IMAGE { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRODUCT> PRODUCTs { get; set; }
    }
}
