``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.1219/21H2)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|  Method |     Mean |    Error |   StdDev |   Gen0 |   Gen1 | Allocated |
|-------- |---------:|---------:|---------:|-------:|-------:|----------:|
| PartOne | 39.25 μs | 0.775 μs | 1.600 μs | 2.9907 | 0.3052 |  37.27 KB |
| PartTwo | 36.34 μs | 0.724 μs | 1.633 μs | 3.4180 | 0.3052 |  42.07 KB |
