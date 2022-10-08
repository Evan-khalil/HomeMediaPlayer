using DAL;
using DAL.Repositories;
using System;
using System.Windows;
using System.Windows.Controls;

namespace BussinessLayer
{
    public class IPlaylist
    {
        private readonly IPlaylistRepository _playlistRepository = new PlaylistRepository();
        private readonly IFilesRepository _fileRepository = new FileRepository();
        public IPlaylist()
        {

        }

        //Add a new playlist
        public void AddNewPlaylist(TextBox playlistname, ListBox album, ListBox PlaylistDataGrid, DataGrid ImportedFilesGrid)
        {
            if (_playlistRepository.AddPlaylist(playlistname.Text, album.SelectedItems[0].ToString(), _fileRepository.GetGridItems(ImportedFilesGrid)) == true)
            {
                MessageBox.Show("Playlist already exists");
            }
            else
            {
                _playlistRepository.AddPlaylist(playlistname.Text, album.SelectedItems[0].ToString(), _fileRepository.GetGridItems(ImportedFilesGrid));
                PlaylistDataGrid.ItemsSource = _playlistRepository.GetPlaylists(album.SelectedItems[0].ToString());
                _fileRepository.clearDataGrid();
            }
        }

        //Get list of files on datagrid selection changed
        public void OnSelectionChanged(ListBox PlaylistDataGrid, DataGrid FilesDataGrid2)
        {
            try
            {
                FilesDataGrid2.ItemsSource = _fileRepository.FillDataGrid(PlaylistDataGrid.SelectedItems[0].ToString());
            }
            catch (ArgumentOutOfRangeException)
            {

            }
        }

        //If no playlist selected disable play and add files else enable.
        public void OnSelection(Button button, Button button1, ListBox listBox)
        {
            if (listBox.SelectedItems.Count != 0)
            {
                button.IsEnabled = true;
                button1.IsEnabled = true;
            }
            else
            {
                button.IsEnabled = false;
                button1.IsEnabled = false;
            }
        }
    }
}
