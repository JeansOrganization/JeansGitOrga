
Console.WriteLine("开始下载网页");
int length = await DownLoadHtmlAsync("http://www.baidu.com","d:/2.html");
Console.WriteLine("获取网页内容长度:" + length);


async Task<int> DownLoadHtmlAsync(string url,string path)
{
    using HttpClient client = new HttpClient();
    string htmlString = await client.GetStringAsync(url);
    await File.WriteAllTextAsync(path, htmlString);
    return htmlString.Length;
}