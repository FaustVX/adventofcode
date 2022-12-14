``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.1335/21H2)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.1 (7.0.122.56804), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.1 (7.0.122.56804), X64 RyuJIT AVX2


```
|  Method |       Mean |    Error |   StdDev |     Gen0 |     Gen1 |  Allocated |
|-------- |-----------:|---------:|---------:|---------:|---------:|-----------:|
| PartOne |   519.0 μs |  6.46 μs |  5.73 μs |  55.6641 |  45.8984 |  683.05 KB |
| PartTwo | 1,239.0 μs | 14.42 μs | 13.49 μs | 208.9844 | 183.5938 | 2574.38 KB |
