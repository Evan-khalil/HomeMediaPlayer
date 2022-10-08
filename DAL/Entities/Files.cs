using System.ComponentModel.DataAnnotations;

namespace DAL
{
    public class Files
    {
        [Key]
        public int filesId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Extention { get; set; }
        public string Description { get; set; }

    }
}
