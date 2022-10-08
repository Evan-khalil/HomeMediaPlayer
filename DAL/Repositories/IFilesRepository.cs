using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace DAL.Repositories
{
    public interface IFilesRepository
    {
        void ImportFiles(string name, string path, string extention, string description);
        bool AddFiles(ObservableCollection<Files> listOfFiles, string playlistName);
        void RemoveFile(string playlistName, int SelectedIndex);
        void AddDescription(string playlistName, int SelectedIndex, string Description);
        void MoveFileDown(string playlistName, DataGrid grid);
        void MoveFileUp(string playlistName, DataGrid grid);
        Files FileAtIndex(string playlistName, int index);
        ObservableCollection<Files> FillDataGrid(string playlistName);
        ObservableCollection<Files> GetGridItems(DataGrid dataGrid);
        ObservableCollection<Files> GetFiles();
        void clearDataGrid();
    }
}
