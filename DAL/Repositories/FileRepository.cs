using DAL.Repositories;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace DAL
{
    public class FileRepository : IFilesRepository
    {
        private ObservableCollection<Files> ListOfFiles = new ObservableCollection<Files>();
        private ObservableCollection<Files> files = new ObservableCollection<Files>();

        //Import files.
        public void ImportFiles(string name, string path, string extention, string description)
        {
            Files file = new Files()
            {
                Name = name,
                Path = path,
                Extention = extention,
                Description = description
            };
            files.Add(file);
        }

        //Return files.
        public ObservableCollection<Files> GetFiles()
        {
            return files;
        }

        //Get the files in grid.
        public ObservableCollection<Files> GetGridItems(DataGrid dataGrid)
        {
            ListOfFiles = (ObservableCollection<Files>)dataGrid.ItemsSource;
            return ListOfFiles;
        }

        //Clear the grid.
        public void clearDataGrid()
        {
            ListOfFiles = new ObservableCollection<Files>();
            files = new ObservableCollection<Files>();
        }

        //Add files to the given playlist.
        public bool AddFiles(ObservableCollection<Files> listOfFiles, string playlistName)
        {
            bool Nofiles = false;
            using (DataContext dbContext = new DataContext())
            {
                if (listOfFiles is null)
                {
                    Nofiles = true;
                }
                if (Nofiles == false)
                {
                    foreach (Files files in listOfFiles)
                    {
                        dbContext.Playlists.Find(dbContext.Playlists.FirstOrDefault(p => p.PlaylistName.Equals(playlistName)).playlistId).Files.Add(files);
                    }
                    dbContext.SaveChanges();
                }
                return Nofiles;
            }
        }

        //Return the specified playlists list of files.
        public ObservableCollection<Files> FillDataGrid(string playlistName)
        {
            using (DataContext dbContext = new DataContext())
            {
                return files = dbContext.Playlists.Find(dbContext.Playlists.FirstOrDefault(p => p.PlaylistName.Equals(playlistName)).playlistId).Files;
            }
        }

        //Return the file at the current index.
        public Files FileAtIndex(string playlistName, int index)
        {
            ObservableCollection<Files> files = new ObservableCollection<Files>();
            using (DataContext dbContext = new DataContext())
            {
                files = dbContext.Playlists.Find(dbContext.Playlists.FirstOrDefault(p => p.PlaylistName.Equals(playlistName)).playlistId).Files;
            }
            return files[index];
        }

        //Remove files.
        public void RemoveFile(string playlistName, int SelectedIndex)
        {
            using (DataContext dbContext = new DataContext())
            {
                dbContext.Playlists.Find(dbContext.Playlists.FirstOrDefault(p => p.PlaylistName.Equals(playlistName)).playlistId).Files.RemoveAt(SelectedIndex);
                dbContext.SaveChanges();
            }

        }

        //Add decription.
        public void AddDescription(string playlistName, int SelectedIndex, string Description)
        {
            using (DataContext dbContext = new DataContext())
            {
                dbContext.Playlists.Find(dbContext.Playlists.FirstOrDefault(p => p.PlaylistName.Equals(playlistName)).playlistId).Files[SelectedIndex].Description = Description;
                dbContext.SaveChanges();
            }
        }

        //Move file down.
        public void MoveFileDown(string playlistName, DataGrid grid)
        {
            using (DataContext dbContext = new DataContext())
            {

                Files files1 = dbContext.Playlists.Find(dbContext.Playlists.FirstOrDefault(p => p.PlaylistName.Equals(playlistName)).playlistId).Files[grid.SelectedIndex];//The file at the current index.
                Files files2 = dbContext.Playlists.Find(dbContext.Playlists.FirstOrDefault(p => p.PlaylistName.Equals(playlistName)).playlistId).Files[grid.SelectedIndex + 1];//The file at the next index.
                Files files3 = (Files)grid.SelectedItem;//The file at current index.

                //Change the current files records to the next files record.
                dbContext.Playlists.Find(dbContext.Playlists.FirstOrDefault(p => p.PlaylistName.Equals(playlistName)).playlistId).Files[grid.SelectedIndex].Name = files2.Name;
                dbContext.Playlists.Find(dbContext.Playlists.FirstOrDefault(p => p.PlaylistName.Equals(playlistName)).playlistId).Files[grid.SelectedIndex].Path = files2.Path;
                dbContext.Playlists.Find(dbContext.Playlists.FirstOrDefault(p => p.PlaylistName.Equals(playlistName)).playlistId).Files[grid.SelectedIndex].Extention = files2.Extention;
                dbContext.Playlists.Find(dbContext.Playlists.FirstOrDefault(p => p.PlaylistName.Equals(playlistName)).playlistId).Files[grid.SelectedIndex].Description = files2.Description;

                //Change the next file to the file at the selected index.
                dbContext.Playlists.Find(dbContext.Playlists.FirstOrDefault(p => p.PlaylistName.Equals(playlistName)).playlistId).Files[grid.SelectedIndex + 1].Name = files3.Name;
                dbContext.Playlists.Find(dbContext.Playlists.FirstOrDefault(p => p.PlaylistName.Equals(playlistName)).playlistId).Files[grid.SelectedIndex + 1].Path = files3.Path;
                dbContext.Playlists.Find(dbContext.Playlists.FirstOrDefault(p => p.PlaylistName.Equals(playlistName)).playlistId).Files[grid.SelectedIndex + 1].Extention = files3.Extention;
                dbContext.Playlists.Find(dbContext.Playlists.FirstOrDefault(p => p.PlaylistName.Equals(playlistName)).playlistId).Files[grid.SelectedIndex + 1].Description = files3.Description;
                dbContext.SaveChanges();
            }
        }

        //Move file up.
        public void MoveFileUp(string playlistName, DataGrid grid)
        {
            using (DataContext dbContext = new DataContext())
            {

                Files files1 = dbContext.Playlists.Find(dbContext.Playlists.FirstOrDefault(p => p.PlaylistName.Equals(playlistName)).playlistId).Files[grid.SelectedIndex];//The file at the current index.
                Files files2 = dbContext.Playlists.Find(dbContext.Playlists.FirstOrDefault(p => p.PlaylistName.Equals(playlistName)).playlistId).Files[grid.SelectedIndex - 1];//The file at the previous index.
                Files files3 = (Files)grid.SelectedItem;//The file at current index.

                //Change the current files records to the previous index files record.
                dbContext.Playlists.Find(dbContext.Playlists.FirstOrDefault(p => p.PlaylistName.Equals(playlistName)).playlistId).Files[grid.SelectedIndex].Name = files2.Name;
                dbContext.Playlists.Find(dbContext.Playlists.FirstOrDefault(p => p.PlaylistName.Equals(playlistName)).playlistId).Files[grid.SelectedIndex].Path = files2.Path;
                dbContext.Playlists.Find(dbContext.Playlists.FirstOrDefault(p => p.PlaylistName.Equals(playlistName)).playlistId).Files[grid.SelectedIndex].Extention = files2.Extention;
                dbContext.Playlists.Find(dbContext.Playlists.FirstOrDefault(p => p.PlaylistName.Equals(playlistName)).playlistId).Files[grid.SelectedIndex].Description = files2.Description;

                //Change the next file to the file at the selected index.
                dbContext.Playlists.Find(dbContext.Playlists.FirstOrDefault(p => p.PlaylistName.Equals(playlistName)).playlistId).Files[grid.SelectedIndex - 1].Name = files3.Name;
                dbContext.Playlists.Find(dbContext.Playlists.FirstOrDefault(p => p.PlaylistName.Equals(playlistName)).playlistId).Files[grid.SelectedIndex - 1].Path = files3.Path;
                dbContext.Playlists.Find(dbContext.Playlists.FirstOrDefault(p => p.PlaylistName.Equals(playlistName)).playlistId).Files[grid.SelectedIndex - 1].Extention = files3.Extention;
                dbContext.Playlists.Find(dbContext.Playlists.FirstOrDefault(p => p.PlaylistName.Equals(playlistName)).playlistId).Files[grid.SelectedIndex - 1].Description = files3.Description;
                dbContext.SaveChanges();
            }
        }
    }
}
