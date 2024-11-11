namespace CowsayForWindows
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Cowsay.SayMessage(String.Join(" ", args));
        }
    }
}
