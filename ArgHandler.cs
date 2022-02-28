using System;
using System.Collections.Generic;

namespace Prometheum
{
    public class ArgHandler {
        private static List<Argument> argDefinitions = new List<Argument>();
        StartupOptions options;
        public StartupOptions ParseArgs(string[] args) {            
            argDefinitions.Add(new Argument("debug", "Connects using the \"TesterToken\" token providedin the config file.", (arg) => {
                options.UseDebugToken = true; // Prevents connection for some reason???
            }));
            argDefinitions.Add(new Argument("help", "Prints the help message.", (arg) => {
                PrintHelpMessage();
            }));
            argDefinitions.Add(new Argument("no-connect", "Decides whether or not to initiate a connection to Discord's API.", (arg) => {
                options.InitiateAPIConenection = false;
            }));

            for (int i = 0; i < args.Length; i++) {
                foreach (Argument argument in argDefinitions) {
                    if (args[i].Contains("--" + argument.Name) || args[i].Contains("-" + argument.Name.Substring(0, 1))) {
                        if (args[i + 1] != null)
                            argument.Execute(args[i]);
                        else argument.Execute(String.Empty);
                    }
                }
            }
            return options;
        }
        private void PrintHelpMessage() {
            Console.Write("usage: Prometheum ");

            foreach (Argument argument in argDefinitions) {
                Console.Write($"-{argument.Name.Substring(0,1)} | ");
            }
            Console.Write("\n");

            Console.WriteLine("Options:");
            
            foreach (Argument arg in argDefinitions) {
                Console.WriteLine($"    -{arg.Name.Substring(0,1)}, --{arg.Name}       {arg.Description}");
            }

            Environment.Exit(0);
        }
    }
}