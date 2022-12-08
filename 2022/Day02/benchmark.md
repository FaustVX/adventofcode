``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.1219/21H2)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|  Method |     Mean |   Error |   StdDev |   Median |    Gen0 |   Gen1 | Allocated |
|-------- |---------:|--------:|---------:|---------:|--------:|-------:|----------:|
| PartOne | 203.6 μs | 4.05 μs | 11.36 μs | 199.0 μs | 13.1836 | 3.4180 | 161.96 KB |
| PartTwo | 192.6 μs | 2.49 μs |  2.21 μs | 192.6 μs | 13.1836 | 3.4180 | 161.96 KB |
