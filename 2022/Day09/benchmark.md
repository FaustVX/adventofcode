``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.1219/21H2)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|  Method |     Mean |    Error |   StdDev |   Median |    Gen0 |    Gen1 |    Gen2 | Allocated |
|-------- |---------:|---------:|---------:|---------:|--------:|--------:|--------:|----------:|
| PartOne | 582.9 μs | 11.60 μs | 27.34 μs | 573.3 μs | 41.0156 | 41.0156 | 41.0156 | 346.37 KB |
| PartTwo | 979.9 μs | 16.87 μs | 42.94 μs | 964.5 μs | 13.6719 |  5.8594 |       - | 181.87 KB |
