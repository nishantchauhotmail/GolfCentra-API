//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GolfCentra.Core.DataBase
{
    using System;
    using System.Collections.Generic;
    
    public partial class ErrorLog
    {
        public long ErrorLogId { get; set; }
        public long ErrorTypeId { get; set; }
        public string ClassName { get; set; }
        public string FunctionName { get; set; }
        public string LineNumber { get; set; }
        public string StackInformation { get; set; }
        public Nullable<System.DateTime> IssueTime { get; set; }
        public string Message { get; set; }
        public Nullable<long> UserId { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    
        public virtual ErrorType ErrorType { get; set; }
    }
}
