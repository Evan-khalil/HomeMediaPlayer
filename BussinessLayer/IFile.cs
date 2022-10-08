using DAL;
using DAL.Repositories;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace BussinessLayer
{
    public class IFile
    {
        private readonly IFilesRepository _fileRepository = new FileRepository();
        public IFile()
        {

        }

        //Import files to grid.
        public void AddNewFile(OpenFileDialog fileDialog, DataGrid dataGrid)
        {
            fileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Image files (*.JPG;*.PNG)|*.JPG;*.PNG|" +
                   "Video files (*.WMV;*.MP4)|*.WMV;*.MP4|" +
                   "All supported files|*.JPG;*.PNG;*.WMV;*.MP4"//Only these files are supported to upload.
            };

            if (fileDialog.ShowDialog() == true)
            {
                foreach (string file in fileDialog.FileNames)
                {
                    string fileName = Path.GetFileName(file);
                    string fileExtention = Path.GetExtension(file);
                    string filePath = file;
                    string fileDescription = "";
                    _fileRepository.ImportFiles(fileName, filePath, fileExtention, fileDescription);
                }
                dataGrid.ItemsSource = _fileRepository.GetFiles();
            }
        }

        //Remove a file at the specified index.
        public void RemoveFiles(DataGrid FileDataGrid, ListBox PlaylistDataGrid)
        {
            _fileRepository.RemoveFile(PlaylistDataGrid.SelectedItems[0].ToString(), FileDataGrid.SelectedIndex);
        }

        //Add a description to the specified file.
        public void AddDescription(DataGrid FileDataGrid, TextBox DescriptionTextBox, ListBox playlistgrid)
        {
            _fileRepository.AddDescription(playlistgrid.SelectedItems[0].ToString(), FileDataGrid.SelectedIndex, DescriptionTextBox.Text);
        }

        //Move the file on the specified index down.
        public void MoveFileDown(DataGrid grid, ListBox playlistListBox)
        {
            _fileRepository.MoveFileDown(playlistListBox.SelectedItems[0].ToString(), grid);
        }

        //Move the file on the specified index up.
        public void MoveFileUp(DataGrid grid, ListBox playlistListBox)
        {
            _fileRepository.MoveFileUp(playlistListBox.SelectedItems[0].ToString(), grid);
        }

        //Enable and disable controls based on selection.
        public void OnFilesSelectionChanged(Button up, Button down, Button remove, Button addDes, TextBox des, DataGrid filesDataGrid)
        {
            if (filesDataGrid.SelectedItems.Count != 0)
            {
                up.IsEnabled = true;
                down.IsEnabled = true;
                remove.IsEnabled = true;
                addDes.IsEnabled = true;
                des.IsEnabled = true;
            }
            else
            {
                up.IsEnabled = false;
                down.IsEnabled = false;
                remove.IsEnabled = false;
                addDes.IsEnabled = false;
                des.IsEnabled = false;
            }

        }

        //Add the files in the grid to an existing playlist.
        public void AddFileToSelectedPlaylist(DataGrid FilesDataGrid, ListBox PlaylistDataGrid, DataGrid filesDataGrid2)
        {
            if (_fileRepository.AddFiles(_fileRepository.GetGridItems(FilesDataGrid), PlaylistDataGrid.SelectedItems[0].ToString()) == true)
            {
                MessageBox.Show("No files to add");
            }
            else
            {
                filesDataGrid2.ItemsSource = _fileRepository.FillDataGrid(PlaylistDataGrid.SelectedItems[0].ToString());
                _fileRepository.clearDataGrid();
                FilesDataGrid.ItemsSource = null;
            }
        }
    }
}
