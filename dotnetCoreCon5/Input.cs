using System;

public static class Input
{
    public static int GetNumber()
    {
        return Convert.ToInt32(Console.ReadLine());
    }

    public static int GetNumberHidden()
    {
        string password = "";
        while (true)
        {
            var key = System.Console.ReadKey(true);
            if (key.Key == ConsoleKey.Enter)
                break;
            password += key.KeyChar;
        }

        return Convert.ToInt32(password);
    }
}