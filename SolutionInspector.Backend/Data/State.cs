using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolutionInspector.Data
{
    public class State
    {
        public static string SolutionPath { get; set; } = "";
        public static Solution? Solution { get; set; }
    }
}
