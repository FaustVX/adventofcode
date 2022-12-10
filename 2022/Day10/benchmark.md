``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.1219/21H2)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|  Method |        Mean |     Error |    StdDev |      Median |   Gen0 | Allocated |
|-------- |------------:|----------:|----------:|------------:|-------:|----------:|
| PartOne |    50.55 μs |  0.919 μs |  1.836 μs |    49.83 μs | 1.8921 |  23.67 KB |
| PartTwo | 1,098.18 μs | 21.396 μs | 20.014 μs | 1,102.29 μs | 1.9531 |  31.85 KB |
