using System;
using System.Collections.Generic;

namespace Prometheum
{
    public class ArgHandler {
        private static Dictionary<string, Action<string>> argDefinitions = new Dictionary<string, Action<string>>();
        StartupOptions options;
        public StartupOptions ParseArgs(string[] args) {            
            argDefinitions.Add("debug", arg => {
                if (arg != String.Empty)
                options.UseDebugToken = true;
            });
            argDefinitions.Add("connect", arg => {
                if (args[1].Equals("false")) {
                    options.InitiateAPIConenection = false;
                } else if (!args[1].Equals("true")) {
                    Console.WriteLine("Invalid Argument Passed.  Proceeding as normal...");
                }
            });
            argDefinitions.Add("help", arg => {
                PrintHelpMessage();
            });

            for (int i = 0; i < args.Length; i++) {
                foreach (KeyValuePair<string, Action<String>> pair in argDefinitions) {
                    if (args[i].Contains("--" + pair.Key) || args[i].Contains("-" + pair.Key.Substring(0, 1))) {
                        pair.Value(args[i + 1]);
                    }
                }
            }



            // switch (args[0]) {
            //     case "-d":
            //     case "--debug":
            //         options.UseDebugToken = true;
            //         break;
            //     case "--connect":
            //         if (args[1].Equals("false"))
            //             options.InitiateAPIConenection = false;
            //         break;
            //     case "-h":
            //     case "--help":
            //         PrintHelpMessage();
            //         break;
            //     default:
            //         PrintHelpMessage();
            //         break;
            // }

            return options;
        }
        private void PrintHelpMessage() {
            Console.WriteLine("usage: Prometheum -d | [--connect initiate connection] ");

            foreach (KeyValuePair<string, Action<string>> pair in argDefinitions) {

            }

            Console.WriteLine("Options:");

            Environment.Exit(0);
        }
    }
}