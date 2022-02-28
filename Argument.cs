using System;
using System.Threading.Tasks;

namespace Prometheum
{
    public class Argument {
        public string Name { get; set; }
        public string Description { get; set; }

        public Action<string> Execute;

        public Argument(string name, string description, Action<string> executionTask) {
            Name = name;
            Description = description;
            Execute = executionTask;
        }
    }
}