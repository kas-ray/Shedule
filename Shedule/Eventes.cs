//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Shedule
{
    using System;
    using System.Collections.Generic;
    
    public partial class Eventes
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public System.DateTime Date { get; set; }
        public int Lessons_Id { get; set; }
    
        public virtual Lessons Lessons { get; set; }
    }
}
