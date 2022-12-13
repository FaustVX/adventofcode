``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.1219/21H2)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|  Method |     Mean |   Error |  StdDev |    Gen0 |    Gen1 | Allocated |
|-------- |---------:|--------:|--------:|--------:|--------:|----------:|
| PartOne | 453.3 μs | 8.11 μs | 8.68 μs | 48.3398 | 31.2500 | 593.27 KB |
| PartTwo | 644.5 μs | 2.03 μs | 1.69 μs | 65.4297 | 50.7813 | 810.24 KB |
