// See https://aka.ms/new-console-template for more information

using ConsoleApp1;

Console.WriteLine("Hello, World!");

var array = new HighPerfArray<byte>(1024);

array.Memory.Span[0] = 1;
array.Memory.Span[1] = 3;

array.Dispose();

ByteWave wave = new ByteWave(4);

var span = wave.Memory.Span;
span[0] = 1;
span[1] = 2;
var tmpSpan = wave.Memory;
wave.Dispose();

Console.WriteLine(123);