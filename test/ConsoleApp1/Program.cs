// See https://aka.ms/new-console-template for more information

using ConsoleApp1;
using SystemModule;

Console.WriteLine("Hello, World!");

BytePool pool = BytePool.Create(10000, 50);

var rr = pool.Obtain(68);


Console.ReadLine();