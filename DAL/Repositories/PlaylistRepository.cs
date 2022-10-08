using DAL.Repositories;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DAL
{
    public class PlaylistRepository : IPlaylistRepository
    {
        //Add new playlist to the specified album.
        public bool AddPlaylist(string name, string albumName, ObservableCollection<Files> files)
        {
            bool exists = false;
            using (DataContext dbContext = new DataContext())
            {
                foreach (Playlist pl in dbContext.Albums.FirstOrDefault(p => p.AlbumName.Equals(albumName)).Playlists)
                {
                    if (pl.PlaylistName.Equals(name))
                    {
                        exists = true;
                    }
                }
                if (exists == false)
                {
                    if (files is null || files.Count == 0)//If there is no files or the list is null, add the playlist and create a new list of files.
                    {
                        Playlist playlist = new Playlist()
                        {
                            PlaylistName = name,
                            Files = new ObservableCollection<Files>()
                        };
                        List<Playlist> playlists = new List<Playlist>
                        {
                            playlist
                        };
                        if (dbContext.Albums.FirstOrDefault(p => p.AlbumName.Equals(albumName)).Playlists is null)//If the album was newly created and there is no list of playlists then create a new list of playlist and add the newly created playlist to it.
                        {
                            dbContext.Albums.FirstOrDefault(p => p.AlbumName.Equals(albumName)).Playlists = playlists;
                            dbContext.SaveChanges();
                        }
                        else//Else add the playlist to the specified albums list op playlist.
                        {
                            foreach (Playlist playlist1 in playlists)
                            {
                                dbContext.Albums.FirstOrDefault(p => p.AlbumName.Equals(albumName)).Playlists.Add(playlist);
                                dbContext.SaveChanges();
                            }
                        }
                    }
                    else//Else add the files in the list to the newly created playlists list of files.
                    {
                        List<Playlist> playlists = new List<Playlist>();
                        Playlist playlist = new Playlist()
                        {
                            PlaylistName = name,
                            Files = new ObservableCollection<Files>()
                        };
                        foreach (Files files2 in files)
                        {
                            playlist.Files.Add(files2);
                        }
                        files.Clear();
                        playlists.Add(playlist);

                        if (dbContext.Albums.FirstOrDefault(p => p.AlbumName.Equals(albumName)).Playlists is null)
                        {
                            dbContext.Albums.FirstOrDefault(p => p.AlbumName.Equals(albumName)).Playlists = playlists;
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            dbContext.Albums.FirstOrDefault(p => p.AlbumName.Equals(albumName)).Playlists.Add(playlist);
                            dbContext.SaveChanges();
                        }
                    }
                }
            }
            return exists;
        }

        //Get all playlist in the specified album.
        public List<string> GetPlaylists(string album)
        {
            using (DataContext dbContext = new DataContext())
            {
                List<Playlist> playlist3 = dbContext.Albums.Find(dbContext.Albums.FirstOrDefault(p => p.AlbumName.Equals(album)).albumId).Playlists;
                List<Playlist> playlist2 = dbContext.Playlists.ToList();
                List<string> names = new List<string>();
                foreach (Playlist playlist1 in playlist3)
                {
                    names.Add(playlist1.PlaylistName);
                }
                return names;
            }
        }
    }
}
