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
    
    public partial class VideoType
    {
        public VideoType()
        {
            this.Videos = new HashSet<Video>();
        }
    
        public int VideoType_Id { get; set; }
        public string VideoType1 { get; set; }
    
        public virtual ICollection<Video> Videos { get; set; }
    }
}
