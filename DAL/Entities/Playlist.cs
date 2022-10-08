using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace DAL
{
    public class Playlist
    {
        [Key]
        public int playlistId { get; set; }
        public string PlaylistName { get; set; }
        public virtual ObservableCollection<Files> Files { get; set; }
    }
}
