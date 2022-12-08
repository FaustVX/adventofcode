``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.1219/21H2)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|  Method |     Mean |   Error |  StdDev |    Gen0 |   Gen1 | Allocated |
|-------- |---------:|--------:|--------:|--------:|-------:|----------:|
| PartOne | 174.3 μs | 3.41 μs | 4.78 μs | 13.6719 | 4.3945 | 168.88 KB |
| PartTwo | 173.7 μs | 3.27 μs | 3.21 μs | 13.6719 | 4.3945 | 168.88 KB |
