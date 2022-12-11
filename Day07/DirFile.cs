namespace Day07
{
    public class FileClass
    {
        public string FileName { get; set; }
        public int FileSize { get; set;}

        public FileClass(string name, int size)
        { 
            FileName = name;
            FileSize = size;
        }
    }

    public class DirClass
    {
        public string DirName;
        public string FullPath;
        public List<DirClass> SubDirs;
        public List<FileClass> Files;
        public int SpaceUsed;

        public DirClass(string Name)
        {
            DirName = Name; 
            FullPath = Name;
            SubDirs = new();
            Files = new();
            SpaceUsed = 0;
        }
    }
}
