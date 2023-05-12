namespace TheGame
{
    public static class Common
    {
        public static string GetWorkingDirectory() => Environment.CurrentDirectory;

        public static string GetDataDir() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TheGame");
    }
}