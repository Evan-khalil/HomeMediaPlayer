using System.Collections.Generic;

namespace DAL.Repositories
{
    public interface IAlbumRepository
    {
        bool AddAlbum(string name);
        List<Albums> GetAll();
        List<string> GetNames();
    }
}
