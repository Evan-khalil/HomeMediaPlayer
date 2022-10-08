using DAL;
using DAL.Repositories;
using NUnit.Framework;
using System.Collections.ObjectModel;
using System.Linq;

namespace Home_Medya_Player_Test
{
    public class PlaylistTestClass
    {
        //Add a new playlist with an empty list of files then retrieve a list of playlists to compare the first elements name against the playlist name, result should be true.
        [TestCase("Add_New_Playlist_With_List_Of_Files_That_Contains_No_Values_Test_Playlist",
                  "Add_New_Playlist_With_List_Of_Files_That_Contains_No_Values_Test_Album")]
        public void Add_New_Playlist_With_List_Of_Files_That_Contains_No_Values(string playlistName, string albumName)
        {
            IPlaylistRepository _playlistRepository = new PlaylistRepository();
            AlbumTestClass AlbumTestClass = new AlbumTestClass();
            AlbumTestClass.Add_Album_With_Existing_Name(albumName);
            ObservableCollection<Files> listOfFiles = new ObservableCollection<Files>();
            _playlistRepository.AddPlaylist(playlistName, albumName, listOfFiles);

            using (DataContext dataContext = new DataContext())
            {
                string Playlist_At_First_Index_Name = dataContext.Albums.FirstOrDefault(p => p.AlbumName == albumName).Playlists.ElementAt(0).PlaylistName;
                //Assert
                Assert.IsTrue(Playlist_At_First_Index_Name.Equals("Add_New_Playlist_With_List_Of_Files_That_Contains_No_Values_Test_Playlist"));
            }

        }

        //Add a new playlist with a list of files that contains values, then retrieve list of files and compare the first element against the file name, result should be true.
        [TestCase("Add_New_Playlist_With_list_Of_Files_That_Contains_Values_Test_Playlist",
                  "Add_New_Playlist_With_list_Of_Files_That_Contains_Values_Test_Album")]
        public void Add_New_Playlist_With_list_Of_Files_That_Contains_Values(string playlistName, string albumName)
        {
            IPlaylistRepository _playlistRepository = new PlaylistRepository();
            AlbumTestClass AlbumTestClass = new AlbumTestClass();
            ObservableCollection<Files> listOfFiles = new();
            Files files = new Files
            {
                Name = "Test_Picture_Name",
                Path = "Test_Path",
                Extention = "Test_Extention",
                Description = "Test_Description"
            };
            listOfFiles.Add(files);
            AlbumTestClass.Add_Album_With_Existing_Name(albumName);
            _playlistRepository.AddPlaylist(playlistName, albumName, listOfFiles);
            using (DataContext dataContext = new DataContext())
            {
                string File_At_First_Index_Name = dataContext.Playlists.Find(dataContext.Playlists.FirstOrDefault(p => p.PlaylistName.Equals(playlistName)).playlistId).Files.ElementAt(0).Name;
                Assert.IsTrue(File_At_First_Index_Name.Equals("Test_Picture_Name"));
            }
        }

        //Add a new playlist to a new album then add another playlist to the same album, retrieve a list of playlists to check that the second added playlist hasn't replaced the existing playlist, the count of the retrieved list should be 2 and the result should be true.
        [TestCase("Add_New_Created_Playlist_To_Existing_Album_Playlists_Test_Palylist",
                  "Add_New_Created_Playlist_To_Existing_Album_Playlists_Test_Album")]
        public void Add_New_Created_Playlist_To_Existing_Album_Playlists(string playlistName, string albumName)
        {
            AlbumTestClass AlbumTestClass = new AlbumTestClass();
            PlaylistTestClass PlaylistTestClass = new PlaylistTestClass();
            AlbumTestClass.Add_Album_With_Existing_Name(albumName);
            PlaylistTestClass.Add_New_Playlist_With_list_Of_Files_That_Contains_Values(playlistName, albumName);
            PlaylistTestClass.Add_New_Playlist_With_list_Of_Files_That_Contains_Values("Add_New_Created_Playlist_To_Existing_Album_Playlists_Test_Palylist_Number_two", albumName);
            using (DataContext dataContext = new DataContext())
            {
                int Playlists_Count = dataContext.Albums.FirstOrDefault(p => p.AlbumName == albumName).Playlists.Count;
                Assert.AreEqual(Playlists_Count, 2);
            }
        }

        //Add a new playlist then retrieve a list of playlists to compare the first elements name against the list returned by GetAll method, result should be true.
        [TestCase("Get_All_Playlists_Test_Method_Test_Album",
                  "Get_All_Playlists_Test_Method_Test_Playlist")]
        public void Get_All_Playlists_Test_Method(string albumName, string playlistName)
        {
            AlbumTestClass albumTest = new AlbumTestClass();
            PlaylistTestClass playlistTestClass = new PlaylistTestClass();
            albumTest.Add_Album_With_Existing_Name(albumName);
            playlistTestClass.Add_New_Playlist_With_list_Of_Files_That_Contains_Values(playlistName, albumName);
            IPlaylistRepository _playlistRepository = new PlaylistRepository();
            using (DataContext dataContext = new DataContext())
            {
                System.Collections.Generic.List<string> Retrieved_Playlists = _playlistRepository.GetPlaylists(albumName);
                string Playlist_Name = dataContext.Albums.FirstOrDefault(p => p.AlbumName == albumName).Playlists.ElementAt(0).PlaylistName;
                Assert.IsTrue(Playlist_Name.Equals(Retrieved_Playlists[0]));
            }

        }
    }
}
