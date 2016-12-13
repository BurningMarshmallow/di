``` ini

BenchmarkDotNet=v0.10.1, OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Pentium(R) CPU N3700 1.60GHz, ProcessorCount=4
Frequency=1562449 Hz, Resolution=640.0209 ns, Timer=TSC
  [Host]        : Clr 4.0.30319.42000, 32bit LegacyJIT-v4.6.1586.0DEBUG
  FastBenchmark : Clr 4.0.30319.42000, 32bit LegacyJIT-v4.6.1586.0

Job=FastBenchmark  LaunchCount=1  TargetCount=1  
WarmupCount=1  

```
                               Method | NumberOfRectangles |           Mean |      Gen 0 | Allocated |
------------------------------------- |------------------- |--------------- |----------- |---------- |
   **PutManyRectanglesWithEndlessSpiral** |                 **10** |  **1,564.4627 ms** | **19500.0000** |  **13.91 MB** |
 PutManyRectanglesWithGeneratorSpiral |                 10 |     18.2093 ms |          - |  16.26 kB |
   **PutManyRectanglesWithEndlessSpiral** |                 **20** | **10,544.4909 ms** | **76375.0000** |  **43.87 MB** |
 PutManyRectanglesWithGeneratorSpiral |                 20 |     36.9977 ms |          - |  32.51 kB |
