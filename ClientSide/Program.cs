// See https://aka.ms/new-console-template for more information

using ClientSide;
using ClientSide.Services;

class Program
{
    public static string Username = null;

    public static void Main(string[] args)
    {

        while (true)
        {
            Console.WriteLine("1. Sign In as User");
            Console.WriteLine("2. Sign In as Admin");
            Console.WriteLine("3. Exit");
            Console.Write("Choose an option: ");

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    TestService.testing(); 
                    break;

                case 2:
                    //signInAdmin();
                    break;

                case 3:
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;

            }


        }

    }


}

