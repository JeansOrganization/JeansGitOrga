
//对可空类型的成员进行检查
/*Student student = GetData();
//student.PhoneNumber = "152121112";
Console.WriteLine(student.Name.ToUpper());
if(student.PhoneNumber is null)
{
    Console.WriteLine("PhoneNumber为null");
}
else
{
    Console.WriteLine(student.PhoneNumber.ToUpper());
}*/

Student student = GetData();
Console.WriteLine(student.Name.ToUpper());
//Console.WriteLine(student.PhoneNumber!.ToUpper() ?? "PhoneNumber为null");
Console.WriteLine(student.PhoneNumber?.ToUpper() ?? "PhoneNumber为null");

Student GetData()
{
    return new Student("SirLee");
}

