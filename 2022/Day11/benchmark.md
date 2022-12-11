``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.1219/21H2)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|  Method |        Mean |     Error |    StdDev |   Gen0 | Allocated |
|-------- |------------:|----------:|----------:|-------:|----------:|
| PartOne |    26.21 μs |  0.171 μs |  0.151 μs | 0.4272 |   5.39 KB |
| PartTwo | 7,454.26 μs | 60.959 μs | 57.021 μs |      - |   5.63 KB |
