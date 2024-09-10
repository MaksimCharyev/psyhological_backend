namespace BackendPsychSite.Core.Models
{
    public class DataSet
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
        public string Prefix { get; set; }
        public string Name { get; set; }
        public Stream Stream { get; set; }
        public DataSet(Guid id, string path, string name, string prefix, Stream stream)
        {
            Id = id;
            Path = path;
            Name = name;
            Prefix = prefix;
            Stream = stream;
        }
        public static DataSet Create(Guid id, string path, string name, string prefix, Stream stream)
        {
            // Можно добавить какие-то проверки
            return new DataSet(id, path, name, prefix, stream);
        }
    }
}
