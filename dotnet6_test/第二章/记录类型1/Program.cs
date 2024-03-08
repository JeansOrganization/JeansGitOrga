
using 记录类型;

Person p1 = new Person("jeeen", "lee") { firstName = "jean" };
Person p2 = new Person("jean", "lee");
Person p3 = new Person("deqiang", "wang");

p1.Age = 20;

Console.WriteLine(p1.GetDisplayName());
Console.WriteLine(p1.firstName, p1.lastName);
Console.WriteLine(p1 == p2);
Console.WriteLine(p1 == p3);
Console.WriteLine(p3.lastName);

var (firstName, lastName) = p1;

Console.WriteLine("firstName:" + firstName);
Console.WriteLine("lastName:" + lastName);



//Car c1 = new Car("奔驰","2000年", 40);
//Car c2 = new Car("奔驰","1997年", 40);
//Car c3 = c2 with { year = "2000年" };
//Console.WriteLine(c3);
//Console.WriteLine(c1 == c2);
//Console.WriteLine(c1 == c3);

//Car c4 = new Car("北京现代", 60);
//Car c5 = c4 with { year = "2000年" };
//Console.WriteLine(c4);
//Console.WriteLine(c5);



//using 记录类型1;

/////
//Test ts = new Test("1111", "2222", "3333");