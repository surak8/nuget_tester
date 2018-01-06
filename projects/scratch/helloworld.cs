// https://msdn.microsoft.com/en-us/library/dd576348.aspx
using System;

class HelloWorld {
    static void Main() {
#if DebugConfig
        Console.WriteLine("WE ARE IN THE DEBUG CONFIGURATION");  
#endif

        Console.WriteLine("Hello, world!");
    }
}