//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class WalkingSchedule
    {
        public int id { get; set; }
        public Nullable<int> dog_id { get; set; }
        public System.TimeSpan time { get; set; }
        public string FIO { get; set; }
    
        public virtual Dogs Dogs { get; set; }
    }
}
