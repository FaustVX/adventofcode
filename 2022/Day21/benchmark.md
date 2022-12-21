``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.1335/21H2)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.1 (7.0.122.56804), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.1 (7.0.122.56804), X64 RyuJIT AVX2


```
|  Method |       Mean |    Error |   StdDev |    Gen0 |    Gen1 | Allocated |
|-------- |-----------:|---------:|---------:|--------:|--------:|----------:|
| PartOne |   623.9 μs | 11.55 μs | 10.80 μs | 33.2031 | 17.5781 | 409.92 KB |
| PartTwo | 1,051.8 μs | 20.89 μs | 57.54 μs | 33.2031 | 17.5781 | 409.99 KB |
