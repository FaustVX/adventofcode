``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.1335/21H2)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.1 (7.0.122.56804), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.1 (7.0.122.56804), X64 RyuJIT AVX2


```
|  Method |    Mean |    Error |   StdDev | Allocated |
|-------- |--------:|---------:|---------:|----------:|
| PartOne | 1.124 s | 0.0222 s | 0.0228 s |   8.24 KB |
| PartTwo | 1.810 s | 0.0123 s | 0.0103 s |   8.66 KB |
