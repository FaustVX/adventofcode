``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.1219/21H2)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|  Method |     Mean |   Error |  StdDev |   Gen0 | Allocated |
|-------- |---------:|--------:|--------:|-------:|----------:|
| PartOne | 440.9 μs | 4.30 μs | 4.02 μs | 4.8828 |  62.48 KB |
| PartTwo | 447.1 μs | 4.85 μs | 4.05 μs | 6.8359 |  85.81 KB |
