``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.19044.2251/21H2/November2021Update)
11th Gen Intel Core i7-1185G7 3.00GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.1 (7.0.122.56804), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.1 (7.0.122.56804), X64 RyuJIT AVX2


```
|  Method |     Mean |     Error |    StdDev |   Gen0 | Allocated |
|-------- |---------:|----------:|----------:|-------:|----------:|
| PartOne |       NA |        NA |        NA |      - |         - |
| PartTwo | 4.372 ns | 0.1347 ns | 0.2429 ns | 0.0038 |      24 B |

Benchmarks with issues:
  Bench<Solution>.PartOne: DefaultJob
