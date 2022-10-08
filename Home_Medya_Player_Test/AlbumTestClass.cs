using DAL;
using DAL.Repositories;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Home_Medya_Player_Test
{
    public class AlbumTestClass
    {
        //Add a new album, then retrieve a list of albums to compare the first elements name at the retrieved list against the added album name, and it should be true. 
        [TestCase("Add_Album_With_Existing_Name_Test_Album")]
        public void Add_Album_With_Existing_Name(string albumName)
        {
            IAlbumRepository _albumRepository = new AlbumRepository();
            _albumRepository.AddAlbum(albumName);
            using (DataContext dataContext = new DataContext())
            {
                List<Albums> Album_List = dataContext.Albums.ToList();
                string Album_At_First_Index_Name = Album_List.ElementAt(0).AlbumName;
                //Assert
                Assert.IsTrue(Album_At_First_Index_Name.Equals("Add_Album_With_Existing_Name_Test_Album"));
            }
        }

        //Add a new album then retrieve a list of albums to compare the first elements name against the list returned by GetAll method, result should be true.
        [TestCase("Get_All_Albums_Test_Album")]
        public void Get_All_Albums_Test(string albumName)
        {
            IAlbumRepository _albumRepository = new AlbumRepository();
            Add_Album_With_Existing_Name(albumName);
            List<Albums> listOFAlbums = new List<Albums>();
            using (DataContext dataContext = new DataContext())
            {
                listOFAlbums = dataContext.Albums.ToList();
            }
            List<Albums> Retrieved_Album_List = _albumRepository.GetAll();
            Assert.IsTrue(listOFAlbums[0].AlbumName.Equals(Retrieved_Album_List[0].AlbumName));
        }
    }
}
