int i = 5, j = 2;
int w = add(i, j);
Task t = File.WriteAllTextAsync(@"d:/1.txt","hello" + w);

Console.WriteLine("111");

int add(int x,int y)
{
    return x + y;
}