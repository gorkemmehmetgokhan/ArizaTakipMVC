﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DB_ArizaTakipEntities : DbContext
    {
        public DB_ArizaTakipEntities()
            : base("name=DB_ArizaTakipEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<tbl_Ariza> tbl_Ariza { get; set; }
        public virtual DbSet<tbl_ArizaDurumTur> tbl_ArizaDurumTur { get; set; }
        public virtual DbSet<tbl_ArizaTur> tbl_ArizaTur { get; set; }
        public virtual DbSet<tbl_Departman> tbl_Departman { get; set; }
        public virtual DbSet<tbl_Gorev> tbl_Gorev { get; set; }
        public virtual DbSet<tbl_GorevDurumTur> tbl_GorevDurumTur { get; set; }
        public virtual DbSet<tbl_Kullanici> tbl_Kullanici { get; set; }
        public virtual DbSet<tbl_KullaniciTur> tbl_KullaniciTur { get; set; }
    }
}
