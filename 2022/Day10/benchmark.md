``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.1219/21H2)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|  Method |     Mean |    Error |   StdDev |   Gen0 |   Gen1 | Allocated |
|-------- |---------:|---------:|---------:|-------:|-------:|----------:|
| PartOne | 34.80 μs | 0.693 μs | 1.607 μs | 3.1738 | 0.0610 |  39.17 KB |
| PartTwo | 38.76 μs | 0.629 μs | 0.749 μs | 3.8452 | 0.0610 |  47.35 KB |
