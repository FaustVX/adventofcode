``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.1219/21H2)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|  Method |     Mean |    Error |   StdDev |    Gen0 | Allocated |
|-------- |---------:|---------:|---------:|--------:|----------:|
| PartOne | 709.2 μs | 14.05 μs | 21.45 μs | 12.6953 | 157.25 KB |
| PartTwo | 714.9 μs | 13.78 μs | 26.55 μs | 12.6953 | 157.25 KB |
