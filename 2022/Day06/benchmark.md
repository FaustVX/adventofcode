``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.1219/21H2)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|  Method |      Mean |    Error |    StdDev |    Median |     Gen0 |  Allocated |
|-------- |----------:|---------:|----------:|----------:|---------:|-----------:|
| PartOne |  88.31 μs | 2.555 μs |  7.493 μs |  87.11 μs |  34.1797 |  419.62 KB |
| PartTwo | 214.37 μs | 7.942 μs | 22.787 μs | 206.05 μs | 101.3184 | 1241.48 KB |
