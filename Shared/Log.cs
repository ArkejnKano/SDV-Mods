﻿using System;
using System.Collections.Generic;
using System.Text;
using StardewModdingAPI;

namespace Shared
{
    internal class Class1
    {using StardewModdingAPI;

    namespace Shared
    {
        internal class Log
        {
            public static IMonitor Monitor;

            public static void Verbose(string str)
            {
                Log.Monitor.VerboseLog(str);
            }

            public static void Trace(string str)
            {
                Log.Monitor.Log(str, LogLevel.Trace);
            }

            public static void Debug(string str)
            {
                Log.Monitor.Log(str, LogLevel.Debug);
            }

            public static void Info(string str)
            {
                Log.Monitor.Log(str, LogLevel.Info);
            }

            public static void Warn(string str)
            {
                Log.Monitor.Log(str, LogLevel.Warn);
            }

            public static void Error(string str)
            {
                Log.Monitor.Log(str, LogLevel.Error);
            }
        }
    }
}
}
