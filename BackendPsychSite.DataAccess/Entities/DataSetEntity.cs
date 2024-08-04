namespace BackendPsychSite.DataAccess.Entities
{
    public class DataSetEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DataSetBucketEntity Bucket { get; set; }
        public string FilePath { get; set; }
        public Stream Stream { get; set; }
        public DataSetEntity()
        {
            
        }

    }
}
