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
    
    public partial class PhotoAlbum
    {
        public PhotoAlbum()
        {
            this.PhotoAlbumDetails = new HashSet<PhotoAlbumDetail>();
        }
    
        public int PhotoAlbum_Id { get; set; }
        public string CoverPhoto { get; set; }
        public string Title { get; set; }
        public Nullable<System.DateTime> Created_On { get; set; }
        public Nullable<System.DateTime> Modified_On { get; set; }
    
        public virtual ICollection<PhotoAlbumDetail> PhotoAlbumDetails { get; set; }
    }
}
