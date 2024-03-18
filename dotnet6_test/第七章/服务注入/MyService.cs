using Hispital;

namespace 服务注入
{
    public class MyService
    {
        int FilesCount = 0;

        public MyService()
        {
            var fileNameArr = Directory.GetFiles(@"D:\program", "*.exe", SearchOption.AllDirectories);
            FilesCount = fileNameArr.Count();
            Console.WriteLine(fileNameArr.Count());
        }

        public int GetFilesCount()
        {
            return FilesCount;
        }


        public IEnumerable<string> GetNames()
        {
            return new string[] { "Tom", "Zack", "Jack" };
        }


    }
}
