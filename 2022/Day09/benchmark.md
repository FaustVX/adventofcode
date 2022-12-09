``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.1219/21H2)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|  Method |     Mean |    Error |   StdDev |     Gen0 |     Gen1 |    Gen2 | Allocated |
|-------- |---------:|---------:|---------:|---------:|---------:|--------:|----------:|
| PartOne | 476.9 μs |  9.28 μs |  9.12 μs | 205.0781 | 205.0781 | 34.1797 | 379.34 KB |
| PartTwo | 903.9 μs | 17.98 μs | 29.04 μs |  16.6016 |   4.8828 |       - | 214.84 KB |
