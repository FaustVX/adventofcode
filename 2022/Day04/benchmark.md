``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.1219/21H2)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|  Method |     Mean |    Error |   StdDev |    Gen0 | Allocated |
|-------- |---------:|---------:|---------:|--------:|----------:|
| PartOne | 733.6 μs | 11.40 μs | 10.10 μs | 10.7422 | 140.67 KB |
| PartTwo | 731.6 μs | 12.87 μs | 12.04 μs | 10.7422 | 140.67 KB |
