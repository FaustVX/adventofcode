``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.1335/21H2)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.1 (7.0.122.56804), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.1 (7.0.122.56804), X64 RyuJIT AVX2


```
|  Method |      Mean |     Error |    StdDev |    Gen0 |    Gen1 |    Gen2 | Allocated |
|-------- |----------:|----------:|----------:|--------:|--------:|--------:|----------:|
| PartOne |  2.180 ms | 0.0431 ms | 0.1098 ms | 23.4375 |  3.9063 |       - | 311.88 KB |
| PartTwo | 24.241 ms | 0.4742 ms | 0.9360 ms | 62.5000 | 62.5000 | 62.5000 | 620.62 KB |
