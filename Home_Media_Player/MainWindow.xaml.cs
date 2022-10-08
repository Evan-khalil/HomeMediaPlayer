using BussinessLayer;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Home_Media_Player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int CounterIndex;//Index to compare to the list of files count
        private int SlideShowIndex;//Index to toggle the slide show
        private DispatcherTimer SlideshowTimer;
        private readonly IFile IFile = new IFile();
        private readonly IAlbums IAlbums = new IAlbums();
        private readonly IPlaylist IPlaylist = new IPlaylist();
        private readonly ISlideshow ISlideshow = new ISlideshow();

        public MainWindow()
        {
            InitializeComponent();
            AlbumTextBox.Focus();
            IAlbums.GetAllAlbumNames(AlbumListBox);
        }

        //Add a new album
        private void AddAlbum(object sender, RoutedEventArgs e)
        {
            IAlbums.AddNewAlbum(AlbumTextBox);
            IAlbums.GetAllAlbumNames(AlbumListBox);
        }

        //Get playlists from selected album
        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PlaylistListBox.ItemsSource = null;
            IAlbums.OnSelectionChanged(AlbumListBox, PlaylistListBox);
            IAlbums.OnSelection(playlistTextBox, AlbumListBox);
        }

        //Add new playlist
        private void AddPlaylist(object sender, RoutedEventArgs e)
        {
            IPlaylist.AddNewPlaylist(playlistTextBox, AlbumListBox, PlaylistListBox, FilesDataGrid);
            FilesDataGrid.ItemsSource = null;
        }

        //Start slide
        private void PlaySlideShow(object sender, RoutedEventArgs e)
        {
            if (IntervalTextBox.Text == "")
            {
                MessageBox.Show("give an interval to start your slide show");
                IntervalTextBox.Focus();
            }
            else if (FilesDataGrid2.Items.Count == 0)
            {
                MessageBox.Show("No files to play, add some files to play your slideshow");
            }
            else
            {
                SlideShowIndex = 0;
                CounterIndex = 1;
                SetTimer();//Start this method if condition is true
            }
        }

        //Event to be called when mediaplay ended
        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            SlideShowIndex++;
            CounterIndex++;
            SetTimer();
        }

        //Event to be called at timer tick
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (CounterIndex <= ISlideshow.GetFilesCount(PlaylistListBox))//Check if the slideshow index is not out of range
            {
                ISlideshow.OnPlay(StopBtn, PlaySlideShowBtn, UpBtn, DownBtn, RemoveBtn, DescriptionBtn, IntervalTextBox);

                if (ISlideshow.FileAtIndex(PlaylistListBox, SlideShowIndex) == true)//Check if the file is a video file
                {
                    ISlideshow.PlayVideo(video, image, SlideshowTimer, DescriptionTextBlock, MediaElement_MediaEnded, PlaylistListBox, SlideShowIndex);
                }

                else //Check if the file is a image file
                {
                    ISlideshow.PlayImage(video, image, IntervalTextBox, SlideshowTimer, DescriptionTextBlock, PlaylistListBox, SlideShowIndex);
                    SlideShowIndex++;//Move to next index of the list of files
                    CounterIndex++;
                }
            }
            else//If the slideshow index reaches the end of listOfFiles 
            {
                ISlideshow.OnStop(StopBtn, PlaySlideShowBtn, UpBtn, DownBtn, RemoveBtn, DescriptionBtn, IntervalTextBox);
                ISlideshow.StopSlideShow(video, image, SlideshowTimer, SlideShowIndex, CounterIndex, DescriptionTextBlock, PlaySlideShowBtn, StopBtn, FilesDataGrid2);//Stop the slide show
            }
        }

        //Set a timer and start it without a specified interval
        private void SetTimer()
        {
            SlideshowTimer = new DispatcherTimer();
            SlideshowTimer.Tick += dispatcherTimer_Tick;
            SlideshowTimer.Start();
        }

        //Move the specified file one index down and refresh datagrid
        private void MoveFileDown(object sender, RoutedEventArgs e)
        {
            IFile.MoveFileDown(FilesDataGrid2, PlaylistListBox);
            IPlaylist.OnSelectionChanged(PlaylistListBox, FilesDataGrid2);
        }

        //Move the specified file one index up and refresh datagrid
        private void MoveFileUp(object sender, RoutedEventArgs e)
        {
            IFile.MoveFileUp(FilesDataGrid2, PlaylistListBox);
            IPlaylist.OnSelectionChanged(PlaylistListBox, FilesDataGrid2);
        }

        //Remove file from playlist
        private void RemoveFile(object sender, RoutedEventArgs e)
        {
            IFile.RemoveFiles(FilesDataGrid2, PlaylistListBox);
            IPlaylist.OnSelectionChanged(PlaylistListBox, FilesDataGrid2);
        }

        //Add description to file
        private void AddDescription(object sender, RoutedEventArgs e)
        {
            IFile.AddDescription(FilesDataGrid2, DescriptionTextBox, PlaylistListBox);
            IPlaylist.OnSelectionChanged(PlaylistListBox, FilesDataGrid2);
        }

        //Stop slideshow
        private void StopSlideShow(object sender, RoutedEventArgs e)
        {
            ISlideshow.OnStop(StopBtn, PlaySlideShowBtn, UpBtn, DownBtn, RemoveBtn, DescriptionBtn, IntervalTextBox);
            ISlideshow.StopSlideShow(video, image, SlideshowTimer, SlideShowIndex, CounterIndex, DescriptionTextBlock, PlaySlideShowBtn, StopBtn, FilesDataGrid2);
        }

        //Add new files
        private void AddFiles(object sender, RoutedEventArgs e)
        {
            IFile.AddNewFile(new OpenFileDialog(), FilesDataGrid);
        }

        //Add the files in grid to the selected playlist
        private void AddFileToSelectedPlaylist(object sender, RoutedEventArgs e)
        {
            IFile.AddFileToSelectedPlaylist(FilesDataGrid, PlaylistListBox, FilesDataGrid2);
            FilesDataGrid.ItemsSource = null;
        }

        //On files selection changed 
        private void OnFilesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IFile.OnFilesSelectionChanged(UpBtn, DownBtn, RemoveBtn, DescriptionBtn, DescriptionTextBox, FilesDataGrid2);
        }

        //On playlistlistbox selection changed.
        private void OnPlaylistSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IPlaylist.OnSelectionChanged(PlaylistListBox, FilesDataGrid2);
            IPlaylist.OnSelection(PlaySlideShowBtn, AddFileToSelectedPlaylistBtn, PlaylistListBox);
        }

        //On album textbox text changed.
        private void OnAlbumTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            IAlbums.CanAdd(AlbumTextBox, AlbumBtn);
        }

        //On playlist textbox text changed.
        private void OnPlaylistTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            IAlbums.CanAdd(playlistTextBox, PlaylistBtn);
        }
    }
}
