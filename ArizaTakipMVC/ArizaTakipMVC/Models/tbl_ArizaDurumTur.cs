//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ArizaTakipMVC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_ArizaDurumTur
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_ArizaDurumTur()
        {
            this.tbl_Ariza = new HashSet<tbl_Ariza>();
        }
    
        public byte id_ArizaDurumTur { get; set; }
        public string ArizaDurumTurAd { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Ariza> tbl_Ariza { get; set; }
    }
}