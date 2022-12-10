``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.1219/21H2)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|  Method |     Mean |    Error |   StdDev |   Gen0 |   Gen1 | Allocated |
|-------- |---------:|---------:|---------:|-------:|-------:|----------:|
| PartOne | 46.85 μs | 0.459 μs | 0.407 μs | 1.7700 |      - |  22.13 KB |
| PartTwo | 51.37 μs | 0.616 μs | 0.576 μs | 2.5024 | 0.0610 |  30.83 KB |
