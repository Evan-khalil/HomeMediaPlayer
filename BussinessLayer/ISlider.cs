using DAL;
using DAL.Repositories;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace BussinessLayer
{
    public class ISlideshow
    {
        private readonly IFilesRepository _fileRepository = new FileRepository();
        public ISlideshow()
        {

        }

        //Get total file count in list
        public int GetFilesCount(ListBox playlistDataGrid)
        {
            return _fileRepository.FillDataGrid(playlistDataGrid.SelectedItems[0].ToString()).Count;
        }

        //Check type of file
        public bool FileAtIndex(ListBox playlistListBox, int index)
        {
            bool isVideo = false;
            if (_fileRepository.FileAtIndex(playlistListBox.SelectedItems[0].ToString(), index).Extention.Equals(".mp4") || _fileRepository.FileAtIndex(playlistListBox.SelectedItems[0].ToString(), index).Extention.Equals("wmv"))
            {
                isVideo = true;
            }
            return isVideo;
        }

        //Stop slide show method
        public void StopSlideShow(MediaElement video, Image image, DispatcherTimer dispatcherTimer, int atIndex, int atIndex2, TextBlock descriptionTextBlock, Button PlayBtn, Button StopBtn, DataGrid FilesDataGrid)
        {
            if (video != null)
            {
                video.LoadedBehavior = MediaState.Manual;
                video.Stop();
            }
            dispatcherTimer.Stop();
            video.Visibility = Visibility.Hidden;
            image.Visibility = Visibility.Hidden;
            descriptionTextBlock.Text = "";
            atIndex = 0;
            atIndex2 = 1;
            FilesDataGrid.SelectedIndex = 0;
        }

        //Play image for the given interval
        public void PlayImage(MediaElement video, Image image, TextBox IntervalTextBox, DispatcherTimer dispatcherTimer, TextBlock DescriptionTextBlock, ListBox PlaylistListBox, int index)
        {
            try
            {
                video.Visibility = Visibility.Hidden;
                image.Visibility = Visibility.Visible;
                string text = IntervalTextBox.Text;
                int duration = int.Parse(text);
                dispatcherTimer.Interval = new TimeSpan(0, 0, duration);//Set the interval for the running timer
                image.Source = new BitmapImage(new Uri(_fileRepository.FileAtIndex(PlaylistListBox.SelectedItems[0].ToString(), index).Path, UriKind.RelativeOrAbsolute));//Show image
                DescriptionTextBlock.Text = _fileRepository.FileAtIndex(PlaylistListBox.SelectedItems[0].ToString(), index).Description;

            }
            catch (FormatException)
            {

            }

        }

        //Play video 
        public void PlayVideo(MediaElement video, Image image, DispatcherTimer dispatcherTimer, TextBlock DescriptionTextBlock, RoutedEventHandler MediaElement_MediaEnded, ListBox playlistListBox, int index)
        {
            dispatcherTimer.Stop();
            image.Visibility = Visibility.Hidden;
            video.Visibility = Visibility.Visible;
            video.Source = new Uri(_fileRepository.FileAtIndex(playlistListBox.SelectedItems[0].ToString(), index).Path);
            video.LoadedBehavior = MediaState.Manual;
            video.Play();//Start the video
            DescriptionTextBlock.Text = _fileRepository.FileAtIndex(playlistListBox.SelectedItems[0].ToString(), index).Description;
            video.MediaEnded += MediaElement_MediaEnded;//Check if media play has ended
        }

        //On slideshow is playing
        public void OnPlay(Button StopBtn, Button PlaySlideShowBtn, Button UpBtn, Button DownBtn, Button RemoveBtn, Button DescriptionBtn, TextBox IntervalTextBox)
        {
            StopBtn.IsEnabled = true;
            PlaySlideShowBtn.IsEnabled = false;
            UpBtn.IsEnabled = false;
            DownBtn.IsEnabled = false;
            RemoveBtn.IsEnabled = false;
            DescriptionBtn.IsEnabled = false;
            IntervalTextBox.IsReadOnly = true;
        }

        //On slideshow stopped
        public void OnStop(Button StopBtn, Button PlaySlideShowBtn, Button UpBtn, Button DownBtn, Button RemoveBtn, Button DescriptionBtn, TextBox IntervalTextBox)
        {
            StopBtn.IsEnabled = false;
            PlaySlideShowBtn.IsEnabled = true;
            UpBtn.IsEnabled = true;
            DownBtn.IsEnabled = true;
            RemoveBtn.IsEnabled = true;
            DescriptionBtn.IsEnabled = true;
            IntervalTextBox.IsReadOnly = false;
        }
    }
}
