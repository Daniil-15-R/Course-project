﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Course_project
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
        private static Entities _context;
        public static Entities GetContext()
        {
            if (_context == null)
            {
                _context = new Entities();
            }
            return _context;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Accounting> Accounting { get; set; }
        public virtual DbSet<Dogs> Dogs { get; set; }
        public virtual DbSet<FoodProducts> FoodProducts { get; set; }
        public virtual DbSet<Medicines> Medicines { get; set; }
        public virtual DbSet<ParasiteTreatmentSchedule> ParasiteTreatmentSchedule { get; set; }
        public virtual DbSet<PlannedEvents> PlannedEvents { get; set; }
        public virtual DbSet<ShelterEmployees> ShelterEmployees { get; set; }
        public virtual DbSet<ShelterNeeds> ShelterNeeds { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<VaccinationSchedule> VaccinationSchedule { get; set; }
        public virtual DbSet<WalkingSchedule> WalkingSchedule { get; set; }
    }
}
