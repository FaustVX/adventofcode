``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.1219/21H2)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|  Method |      Mean |    Error |   StdDev |   Gen0 |   Gen1 | Allocated |
|-------- |----------:|---------:|---------:|-------:|-------:|----------:|
| PartOne | 102.18 μs | 2.016 μs | 1.886 μs | 6.2256 | 0.3662 |  77.02 KB |
| PartTwo |  99.99 μs | 1.822 μs | 4.294 μs | 6.4697 | 0.3662 |  79.34 KB |
