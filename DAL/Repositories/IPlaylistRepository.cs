using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DAL.Repositories
{
    public interface IPlaylistRepository
    {
        bool AddPlaylist(string name, string albumName, ObservableCollection<Files> files);
        List<string> GetPlaylists(string album);
    }
}
