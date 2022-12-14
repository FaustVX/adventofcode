``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.1335/21H2)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.1 (7.0.122.56804), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.1 (7.0.122.56804), X64 RyuJIT AVX2


```
|  Method |        Mean |     Error |    StdDev |    Gen0 |   Gen1 | Allocated |
|-------- |------------:|----------:|----------:|--------:|-------:|----------:|
| PartOne |    891.9 μs |  17.26 μs |  16.15 μs | 13.6719 | 0.9766 | 173.82 KB |
| PartTwo | 13,327.5 μs | 109.50 μs | 102.43 μs | 15.6250 |      - | 209.62 KB |
