``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.1219/21H2)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|  Method |     Mean |   Error |  StdDev |    Gen0 | Allocated |
|-------- |---------:|--------:|--------:|--------:|----------:|
| PartOne | 605.8 μs | 2.01 μs | 1.78 μs | 10.7422 | 140.67 KB |
| PartTwo | 597.0 μs | 4.22 μs | 3.53 μs | 10.7422 | 140.67 KB |
