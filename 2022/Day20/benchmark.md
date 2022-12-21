``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.1335/21H2)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.1 (7.0.122.56804), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.1 (7.0.122.56804), X64 RyuJIT AVX2


```
|  Method |      Mean |    Error |   StdDev | Allocated |
|-------- |----------:|---------:|---------:|----------:|
| PartOne |  44.98 ms | 0.895 ms | 1.065 ms | 507.95 KB |
| PartTwo | 511.68 ms | 8.965 ms | 8.386 ms |  508.3 KB |
