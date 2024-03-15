using Microsoft.EntityFrameworkCore;
using 普通的一对多配置;

using MySqlContext ct = new MySqlContext();

//Article article = new Article() { title = "路飞五档！！", content = "详情:人人果实太阳神形态！！" };
//Comment comment = new Comment() { message = "666"};
//Comment comment2 = new Comment() { message = "真好！！"};
//article.comments.Add(comment);
//article.comments.Add(comment2);
//ct.Add(article);
//await ct.SaveChangesAsync();

//var articles = ct.Articles.Where(r => r.title.Contains("路飞")).Include(r=>r.comments);
//foreach (var article in articles)
//{
//    Console.WriteLine($"标题:{article.title},内容:{article.content}," +
//        $"评论:[{string.Join(",",article.comments.Select(t=>t.message))}]");
//}

var Articles = ct.Articles.Include(r=>r.comments).Where(r => r.comments.Any(t => t.message.Contains("棒")));

foreach (var a in Articles)
{
    Console.WriteLine(a.title + ":" + a.content);
    foreach (var c in a.comments)
    {
        Console.WriteLine(c.message);
    }
}