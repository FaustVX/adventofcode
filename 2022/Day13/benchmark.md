``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.1219/21H2)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|  Method |     Mean |    Error |  StdDev |    Gen0 |    Gen1 | Allocated |
|-------- |---------:|---------:|--------:|--------:|--------:|----------:|
| PartOne | 475.8 μs |  6.89 μs | 6.44 μs | 46.8750 | 35.1563 | 585.09 KB |
| PartTwo | 681.9 μs | 11.61 μs | 9.69 μs | 70.3125 | 50.7813 | 862.05 KB |
