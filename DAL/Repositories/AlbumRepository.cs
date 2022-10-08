using DAL.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class AlbumRepository : IAlbumRepository
    {
        //Add a new album with the given name
        public bool AddAlbum(string name)
        {
            bool exists = false;
            using (DataContext dbContext = new DataContext())
            {
                List<Albums> albums = dbContext.Albums.ToList();
                foreach (Albums album in albums)
                {
                    if (album.AlbumName.Equals(name))
                    {
                        exists = true;
                    }
                }
                if (exists == false)
                {
                    Albums Album = new Albums()
                    {
                        AlbumName = name,
                        Playlists = new List<Playlist>()
                    };
                    dbContext.Albums.Add(Album);
                    dbContext.SaveChanges();
                }
            }
            return exists;
        }

        //Get all albums
        public List<Albums> GetAll()
        {
            using (DataContext dbContext = new DataContext())
            {
                return dbContext.Albums.ToList();
            }
        }

        //Get a list of album names
        public List<string> GetNames()
        {
            List<string> names = new List<string>();
            foreach (Albums albums in GetAll())
            {
                names.Add(albums.AlbumName);
            }
            return names;
        }

    }
}
