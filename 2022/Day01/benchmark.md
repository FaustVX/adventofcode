``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.1219/21H2)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|  Method |     Mean |   Error |   StdDev |   Gen0 | Allocated |
|-------- |---------:|--------:|---------:|-------:|----------:|
| PartOne | 307.6 μs | 7.18 μs | 20.73 μs | 4.3945 |  59.71 KB |
| PartTwo | 273.3 μs | 5.32 μs |  8.44 μs | 4.3945 |  59.71 KB |
