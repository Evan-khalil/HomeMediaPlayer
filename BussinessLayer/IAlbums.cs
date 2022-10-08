using DAL;
using DAL.Repositories;
using System.Windows;
using System.Windows.Controls;

namespace BussinessLayer
{
    public class IAlbums
    {
        private readonly IAlbumRepository _albumRepository = new AlbumRepository();
        private readonly IPlaylistRepository _playlistRepository = new PlaylistRepository();
        public IAlbums()
        {

        }

        //Add a new album.
        public void AddNewAlbum(TextBox Name)
        {
            if (_albumRepository.AddAlbum(Name.Text) == true)
            {
                MessageBox.Show("Album already exists");
            }
            else
            {
                _albumRepository.AddAlbum(Name.Text);
            }
        }

        //Set listbox item source to the specified list of names.
        public void GetAllAlbumNames(ListBox listBox)
        {
            listBox.ItemsSource = _albumRepository.GetNames();
        }

        //Get playlists from selected album.
        public void OnSelectionChanged(ListBox AlbumListBox, ListBox PlaylistDataGrid)
        {
            PlaylistDataGrid.ItemsSource = _playlistRepository.GetPlaylists(AlbumListBox.SelectedItems[0].ToString());
        }

        //If album selected enable adding playlist
        public void OnSelection(TextBox PlaylistTextBox, ListBox AlbumListBox)
        {
            if (AlbumListBox.SelectedItems.Count != 0)
            {
                PlaylistTextBox.IsEnabled = true;
            }
            else
            {
                PlaylistTextBox.IsEnabled = false;
            }
        }

        //If album textbox is empty disable adding albums else enable adding.
        public void CanAdd(TextBox textBox, Button button)
        {
            if (textBox.Text == "")
            {
                button.IsEnabled = false;
            }
            else
            {
                button.IsEnabled = true;
            }
        }
    }
}
