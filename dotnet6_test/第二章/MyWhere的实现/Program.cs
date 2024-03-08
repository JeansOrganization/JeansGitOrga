
int[] nums = new int[] { 1, 5, 6, 8, 10, 110, 12, 2, 3, 9,51,98,96 };
OutputList(nums);
var num1 = nums.Where(r => r > 50);
OutputList(num1);
var num2 = nums.MyWhere(r => r > 50);
OutputList(num2);
var num3 = nums.Remove(r => r > 50);
OutputList(num3);



void OutputList<T>(IEnumerable<T> obj)
{
    Console.WriteLine();
    foreach (T o in obj)
    {
        Console.WriteLine(o);
    }
}

public static class Extension
{
    public static IEnumerable<T> MyWhere<T>(this IEnumerable<T> obj, Func<T,bool> func)
    {
        List<T> list = new List<T>();
        foreach (T o in obj)
        {
            if (func(o))
            {
                //list.Add(o);
                yield return o;
            }
        }
        //return list;
    }

    public static IEnumerable<T> Remove<T>(this IEnumerable<T> obj, Func<T, bool> func)
    {
        List<T> list = new List<T>();
        foreach (T o in obj)
        {
            if (!func(o))
            {
                //list.Add(o);
                yield return o;
            }
        }
        //return list; 返回整个list
    }
}
