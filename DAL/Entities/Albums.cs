using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL
{
    public class Albums
    {
        [Key]
        public int albumId { get; set; }
        public string AlbumName { get; set; }
        public virtual List<Playlist> Playlists { get; set; }
    }
}
