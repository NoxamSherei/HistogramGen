// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using System.Reflection;

Console.WriteLine("Hello, World!");
BenchmarkSwitcher.FromAssembly(Assembly.GetExecutingAssembly()).Run();
