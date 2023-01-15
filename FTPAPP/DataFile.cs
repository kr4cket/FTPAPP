namespace FTPAPP
{
    public class DataFile
    {
        public string file_size { get; set; }
        public string file_name { get; set; }
        public string file_type { get; set; }

        public DataFile(string file_size, string file_name, string file_type)
        {
            this.file_size = file_size;
            this.file_name = file_name;
            this.file_type = file_type;
        }
    }
}