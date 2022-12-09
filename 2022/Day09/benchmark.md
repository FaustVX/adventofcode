``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.1219/21H2)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|  Method |     Mean |    Error |   StdDev |     Gen0 |     Gen1 |    Gen2 | Allocated |
|-------- |---------:|---------:|---------:|---------:|---------:|--------:|----------:|
| PartOne | 711.8 μs |  4.20 μs |  3.72 μs | 440.4297 | 440.4297 | 74.2188 |  496.4 KB |
| PartTwo | 877.5 μs | 17.00 μs | 26.47 μs |  11.7188 |   2.9297 |       - | 145.06 KB |
