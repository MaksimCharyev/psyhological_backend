namespace BackendPsychSite.UseCases.Utils
{
    public class DataSetArgs
    {
        public string Name;
        public string UserEmail { get; set; }
        public string ProjectName { get; set; }
        public string Path { get; set; } //Combination of UserEmail part and ProjectName part

    }
}
