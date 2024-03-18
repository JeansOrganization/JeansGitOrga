using EFCore原理;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MySqlConnector;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Xml.Linq;

using MySqlContext context = new MySqlContext();

#region 数据保存

//var list = new List<Book>
//{
//    new Book() { Title="第一本书", PubTime = DateTime.Now.AddDays(-50), Price=80, AuthorName="Jean" },
//    new Book() { Title="第二本书", PubTime = DateTime.Now.AddDays(-49), Price=95, AuthorName="Jean" },
//    new Book() { Title="第三本书", PubTime = DateTime.Now.AddDays(-48), Price=95, AuthorName="Ting" },
//    new Book() { Title="第四本书", PubTime = DateTime.Now.AddDays(-47), Price=95, AuthorName="Jean" },
//    new Book() { Title="第五本书", PubTime = DateTime.Now.AddDays(-46), Price=85, AuthorName="Jean" },
//    new Book() { Title="第六本书", PubTime = DateTime.Now.AddDays(-45), Price=70, AuthorName="Ting" },
//    new Book() { Title="第七本书", PubTime = DateTime.Now.AddDays(-50), Price=80, AuthorName="Jean" },
//    new Book() { Title="第八本书", PubTime = DateTime.Now.AddDays(-49), Price=80, AuthorName="Ting" },
//    new Book() { Title="第九本书", PubTime = DateTime.Now.AddDays(-48), Price=80, AuthorName="Jean" },
//    new Book() { Title="第十本书", PubTime = DateTime.Now.AddDays(-47), Price=80, AuthorName="Ting" },
//    new Book() { Title="第十一本书", PubTime = DateTime.Now.AddDays(-46), Price=85, AuthorName="Jean" },
//    new Book() { Title="第十二本书", PubTime = DateTime.Now.AddDays(-45), Price=70, AuthorName="Jean" }
//};

//await context.Books.AddRangeAsync(list);
//await context.SaveChangesAsync();

#endregion

/* IEnumerable加where条件 */
/*IEnumerable<Book> books1 = context.Books;
foreach (Book book in books1.Where(r=>r.AuthorName=="Jean"))
{
    Console.WriteLine($"ID:{book},书名:{book.Title},作者:{book.AuthorName},价格:{book.Price}元");
}*/


/* IQueryable加where条件 */
/*IQueryable<Book> books2 =  context.Books;
foreach (Book book in books2.Where(r => r.AuthorName == "Jean"))
{
    Console.WriteLine($"ID:{book},书名:{book.Title},作者:{book.AuthorName},价格:{book.Price}元");
}*/


/*遍历查询*/
/*Console.WriteLine("1、Where之前");
IQueryable<Book> books = context.Books.Where(b => b.Price > 1.1);
Console.WriteLine("2、遍历IQueryable之前");
foreach (var b in books)
{
    Console.WriteLine(b.Title + ":" + b.PubTime);
}
Console.WriteLine("3、遍历IQueryable之后");*/


/*拼接复杂的查询条件*/
/*QueryBooks("Jean", true, true, 100);
QueryBooks("Ting", false, false, 18);
void QueryBooks(string searchWords, bool searchAll, bool orderByPrice, double upperPrice)
{
	IQueryable<Book> books = context.Books.Where(b => b.Price <= upperPrice);
	if (searchAll)//匹配书名或、作者名
	{
		books = books.Where(b => b.Title.Contains(searchWords) || b.AuthorName.Contains(searchWords));
	}
	else//只匹配书名
	{
		books = books.Where(b => b.Title.Contains(searchWords));
	}
	if (orderByPrice)//按照价格排序
	{
		books = books.OrderBy(b => b.Price);
	}
	foreach (Book b in books)
	{
		Console.WriteLine($"{b.Id},{b.Title},{b.Price},{b.AuthorName}");
	}
}*/


/*IQueryable的复用*/
/*IQueryable<Book> books = context.Books.Where(b => b.Price >= 8);
Console.WriteLine(books.Count());
Console.WriteLine(books.Max(b => b.Price));
foreach (Book b in books.Where(b => b.PubTime.Year > 2000))
{
	Console.WriteLine(b.Title);
}*/


/*分页查询*/
/*IQueryable<Book> books = context.Books;
OutputPage(books,3,5);
void OutputPage(IQueryable<Book> books,int page,int count=3)
{
	var bookPages = books.Skip((page - 1) * count).Take(count);
	foreach (var bookPage in bookPages)
	{
        Console.WriteLine($"ID:{bookPage},书名:{bookPage.Title},作者:{bookPage.AuthorName},价格:{bookPage.Price}元");
    }
}*/


/*关于context作用域对IQueryable数据的影响*/
/*foreach (var b in QueryBooksQ())
{
	Console.WriteLine(b.Title);
}
*//*有错误的返回IQueryable查询结果*//*
IQueryable<Book> QueryBooksQ()
{
	using MySqlContext ctx = new MySqlContext();
	return ctx.Books.Where(b => b.AuthorName=="Jean");
}
*//*正确的返回IEnumerable的数据*//*
IEnumerable<Book> QueryBooksE()
{
	using MySqlContext ctx = new MySqlContext();
	return ctx.Books.Where(b => b.AuthorName == "Jean").ToArray();
}*/

#region ExecuteSqlInterpolatedAsync(控制台插入程序)
//while (true)
//{
//	string name="";
//    while (name.Equals(""))
//	{
//        Console.WriteLine("请输入您的姓名(按Enter确认):");
//        name = Console.ReadLine()?.Trim();
//		if(name.Equals("")) Console.WriteLine("姓名不能为空！！");
//    }

//    string bookName = "";
//    while (bookName.Equals(""))
//    {
//        Console.WriteLine("请输入您要售卖的书(按Enter确认):");
//        bookName = Console.ReadLine()?.Trim();
//        if (bookName.Equals("")) Console.WriteLine("书名不能为空！！");
//    }

//    string priceStr = "";
//    double price = 0;
//    while (!double.TryParse(priceStr, out price))
//    {
//        Console.WriteLine("请输入您要售卖的价格(按Enter确认):");
//        priceStr = Console.ReadLine()?.Trim(); 
//        if (!double.TryParse(priceStr, out price))
//        {
//            Console.WriteLine("价格输入不规范，请重新输入!!!");
//        }
//    }

//	ConsoleKeyInfo? keyInfo = null;
//	while(keyInfo?.Key != ConsoleKey.F && keyInfo?.Key != ConsoleKey.Enter)
//    {
//        Console.WriteLine($"请确认您的售卖信息是否无误:[书名:{bookName},作者名:{name},售卖价格:{price}],无误请按Enter确认,否则按F重新输入");
//        keyInfo = Console.ReadKey();
//		if(keyInfo?.Key != ConsoleKey.F && keyInfo?.Key != ConsoleKey.Enter)
//		{
//            Console.WriteLine("请输入正确的按键进行选择！！");
//        }
//    }

//	if(keyInfo?.Key == ConsoleKey.F)
//	{
//        Console.WriteLine();
//        continue;
//	}
//	if(keyInfo?.Key == ConsoleKey.Enter)
//	{
//		int times = await context.Database.ExecuteSqlInterpolatedAsync
//			($"INSERT INTO t_book (ID, Title, PubTime, Price, AuthorName) VALUES ({Guid.NewGuid()},{bookName}, SYSDATE(), {price}, {name})");
//        if(times == 1) Console.WriteLine("插入成功！！"); else { Console.WriteLine("插入失败！！"); }
//		break;
//    }
//}
#endregion

#region FromSqlInterpolated(控制台查询程序)

//Console.WriteLine("请输入作者姓名搜索书籍(支持模糊搜索):");
//string name = Console.ReadLine();
//name = $"%{name}%";
//IQueryable<Book> books = context.Books.FromSqlInterpolated($"select* from t_book a where a.AuthorName like {name}");
//foreach (var book in books)
///* FromSqlInterpolated数据再加工 */
////foreach (var book in books.Take(2))
//{
//    Console.WriteLine($"ID:{book},书名:{book.Title},作者:{book.AuthorName},价格:{book.Price}元");
//}

#endregion

#region 获取connect连接(执行任意语句)
//using MySqlConnection conn = (MySqlConnection)context.Database.GetDbConnection();
//if(conn.State != System.Data.ConnectionState.Open) conn.Open();
//using var command = conn.CreateCommand();
//command.CommandText = "select * from t_book";
#region DataTable
//DataTable dt = new DataTable();
//MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
//dataAdapter.Fill(dt);
//foreach(DataRow dr in dt.Rows)
//{
//    foreach (DataColumn dc in dt.Columns)
//    {
//        Console.Write(dc.ColumnName + ":" + dr[dc.ColumnName] + ",");
//    }
//    Console.WriteLine();
//}
#endregion

#region DataReader
//using var reader = command.ExecuteReader();
//while (reader.Read())
//{
//    Console.WriteLine($"{reader.GetValue(0)}:{reader.GetValue(1)}");
//}
#endregion

#endregion


/*对象的跟踪状态*/
//Book[] books = context.Books.Take(3).ToArray();
//Book b1 = books[0];
//Book b2 = books[1];
//Book b3 = books[2];
//Book b4 = new Book { Title = "零基础趣学C语言", AuthorName = "杨中科" };
//Book b5 = new Book { Title = "百年孤独", AuthorName = "马尔克斯" };
//b1.Title = "abc";
//context.Remove(b3);
//context.Add(b4);
//EntityEntry entry1 = context.Entry(b1);
//EntityEntry entry2 = context.Entry(b2);
//EntityEntry entry3 = context.Entry(b3);
//EntityEntry entry4 = context.Entry(b4);
//EntityEntry entry5 = context.Entry(b5);
//Console.WriteLine("b1.State:" + entry1.State);
//Console.WriteLine("b1.DebugView:" + entry1.DebugView.LongView);
//Console.WriteLine("b2.State:" + entry2.State);
//Console.WriteLine("b3.State:" + entry3.State);
//Console.WriteLine("b4.State:" + entry4.State);
//Console.WriteLine("b5.State:" + entry5.State);



/*AsNoTracking*/
/*Book[] books = context.Books.AsNoTracking().Take(3).ToArray();
Book b1 = books[0];
b1.Title = "abc";
EntityEntry entry1 = context.Entry(b1);
Console.WriteLine(entry1.State);*/


/*Book b1 = context.Books.First(b => b.AuthorName=="Jean");
b1.Title = "Jean2333";
EntityEntry entry1 = context.Entry(b1);
Console.WriteLine(entry1.State);
context.SaveChanges();
Console.WriteLine(entry1.State);*/


/* 通过改变EntityState.Detached数据的state值,实现不查询就能操作数据(不推荐) */
/*Book b1 = new Book { Id = Guid.Parse("08dc44b6-cea9-4f9f-8c47-ce4bcde76dc7") };
b1.Title = "新书名";
var entry1 = context.Entry(b1);
entry1.Property("Title").IsModified = true;
Console.WriteLine(entry1.DebugView.LongView);
context.SaveChanges();*/
/*Book b1 = new Book { Id = Guid.Parse("08dc44b6-cea9-4f9f-8c47-ce4bcde76dc7") };
context.Entry(b1).State = EntityState.Deleted;
context.SaveChanges();*/


