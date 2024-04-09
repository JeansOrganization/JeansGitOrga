

int difference = int.MaxValue;
List<int> mainList = new List<int>();
List<int> retList = new List<int>();

//List<int> numlist = new List<int>() {1,8, 5, 3,6,9,4};
//int global = 10;
List<int> numlist = new List<int>() {1,8, 5, 3,6,9,4};
int global = 10;
MinAbsDifference(numlist,global);
#region 递归循环

void MinAbsDifference(List<int> numList, int global)
{
    while (numlist.Count > 0)
    {
        dfs(numList, global, 0, 0);
        foreach (var num in mainList)
        {
            Console.Write(num + ",");
        }
        Console.WriteLine();
        numList.RemoveAll(r => mainList.Contains(r));
        mainList.Clear();
        retList.Clear();
        difference = int.MaxValue;
    }
}
bool dfs(List<int> numList, int global,int index,int sum)
{
    if (global - sum < 0) return false;
    if (difference > global - sum)
    {
        difference = global - sum;
        mainList.Clear();
        retList.ForEach(r => mainList.Add(r));
    }
    if (global - sum == 0) return true;

    for(int i = index; i < numList.Count; i++)
    {
        retList.Add(numList[i]);
        bool isok = dfs(numList, global, i + 1, sum + numList[i]);
        if (!isok)
        {
            retList.Remove(numList[i]);
            continue;
        }
        else
        {
            return true;
        }
    }
    return false;
}

#endregion

#region 枚举+双指针

bool MinAbsDifference2(List<int> numList, int global)
{
    int mid = numlist.Count / 2;

    List<int> leftNumList = new List<int>();
    List<int> rightNumList = new List<int>();

    dfs2(numlist, 0, mid - 1, 0, leftNumList);
    dfs2(numlist, mid, numlist.Count-1, 0, rightNumList);
    leftNumList = leftNumList.OrderBy(r => r).ToList();
    rightNumList = rightNumList.OrderBy(r => r).ToList();

    int ans = int.MaxValue;
    int left = 0;
    int right = rightNumList.Count-1;
    while(left<leftNumList.Count && right >= 0)
    {
        int t = leftNumList[left] + rightNumList[right];
        if (global < t)
        {
            right--;
            continue;
        }
        ans = Math.Min(ans, t);
        left++;
    }
}

void dfs2(List<int> numList,int left,int right,int sum,List<int> leftNumList)
{
    leftNumList.Add(sum);
    for(int i = left; i <= right; i++)
    {
        dfs2(numlist, i + 1, right, sum + numlist[i],leftNumList);
    }
}

#endregion

Console.WriteLine("Hello, World!");
