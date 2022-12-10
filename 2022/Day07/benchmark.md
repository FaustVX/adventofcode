``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.1219/21H2)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|  Method |     Mean |   Error |  StdDev |   Median |   Gen0 |   Gen1 | Allocated |
|-------- |---------:|--------:|--------:|---------:|-------:|-------:|----------:|
| PartOne | 142.9 μs | 3.15 μs | 9.08 μs | 141.1 μs | 4.8828 | 0.2441 |   60.8 KB |
| PartTwo | 135.3 μs | 2.70 μs | 6.15 μs | 132.9 μs | 5.1270 | 0.2441 |  63.12 KB |
