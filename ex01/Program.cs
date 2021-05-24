using System;
using System.IO;

string name;

static int MinAmongFreeNumbers(int x, int y, int z)
{
    if (x > y)
        x = y;
    if (x < z)
        return (x);
    return (z);
}

static int DistanceLevenshtein(string namecons, string namedict)
{
    var n = namecons.Length + 1;
    var m = namedict.Length + 1;
    var matrixDistLev = new int[n, m];

    for (var i = 0; i < n; i++)
        matrixDistLev[i, 0] = i;

    for (var j = 0; j < m; j++)
        matrixDistLev[0, j] = j;

    for (var i = 1; i < n; i++)
    {
        for (var j = 1; j < m; j++)
        {
            var changeel = 0;
            if (namecons[i - 1] == namedict[j - 1])
                changeel = 0;
            else 
                changeel = 1;

            matrixDistLev[i, j] = MinAmongFreeNumbers(matrixDistLev[i - 1, j] + 1,
                matrixDistLev[i, j - 1] + 1,
                matrixDistLev[i - 1, j - 1] + changeel);
        }
    }
    return matrixDistLev[n - 1, m - 1];
}

Console.Write("Enter name: ");
name = Console.ReadLine();
if (name == "")
    Console.WriteLine("Your name was not found.");
else
{
    StreamReader file = new StreamReader($"us.txt");
    int nowlevdist;
    string ln;

    while ((ln = file.ReadLine()) != null)
    {
        nowlevdist = DistanceLevenshtein(name, ln);
        if (nowlevdist == 0)
        {
            Console.WriteLine("Hello, {0}!", name);
            file.Close();
            return ;
        }
    }
    file.Close();
 
    file = new StreamReader($"us.txt");
    while ((ln = file.ReadLine()) != null)
    {
        nowlevdist = DistanceLevenshtein(name, ln);
        if (nowlevdist <= 2)
        {
            Console.WriteLine("Did you mean “{0}”? Y/N", ln);
            string yn = Console.ReadLine();
            while (yn != "Y" && yn != "N")
            {
                Console.WriteLine("Did you mean “{0}”? Y/N", ln);
                yn = Console.ReadLine();
            }
            if (yn == "Y")
            {
                Console.WriteLine("Hello, {0}!", ln);
                file.Close();
                return ;
            }
        }
    }
 
    Console.WriteLine("Your name was not found.");
    file.Close();
}
  