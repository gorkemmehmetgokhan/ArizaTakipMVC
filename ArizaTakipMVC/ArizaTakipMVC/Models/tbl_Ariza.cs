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
    
    public partial class tbl_Ariza
    {
        public int id_Ariza { get; set; }
        public short id_ArizaTur { get; set; }
        public byte id_ArizaDurumTur { get; set; }
        public int id_Kullanici { get; set; }
        public string Aciklama { get; set; }
        public System.DateTime ArizaGirisTarihi { get; set; }
    
        public virtual tbl_ArizaDurumTur tbl_ArizaDurumTur { get; set; }
        public virtual tbl_ArizaTur tbl_ArizaTur { get; set; }
        public virtual tbl_Kullanici tbl_Kullanici { get; set; }
    }
}