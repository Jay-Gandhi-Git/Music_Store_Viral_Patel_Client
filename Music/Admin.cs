//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Music
{
    using System;
    using System.Collections.Generic;
    
    public partial class Admin
    {
        public Admin()
        {
            this.Audios = new HashSet<Audio>();
            this.Videos = new HashSet<Video>();
        }
    
        public int Admin_Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string MobileNumber { get; set; }
    
        public virtual ICollection<Audio> Audios { get; set; }
        public virtual ICollection<Video> Videos { get; set; }
    }
}