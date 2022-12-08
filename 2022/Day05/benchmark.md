``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.1219/21H2)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|  Method |     Mean |   Error |   StdDev |   Gen0 | Allocated |
|-------- |---------:|--------:|---------:|-------:|----------:|
| PartOne | 390.6 μs | 3.54 μs |  2.95 μs | 5.3711 |  71.52 KB |
| PartTwo | 418.8 μs | 8.28 μs | 12.13 μs | 7.3242 |  94.84 KB |
