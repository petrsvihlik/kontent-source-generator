using System;
using KenticoKontentModels;

namespace Kentico.Kontent.SourceGeneratorDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var model = new Article() { Price = 35 };
            Console.WriteLine($"Price: {model.Price}");
            Console.ReadLine();
        }
    }
}
