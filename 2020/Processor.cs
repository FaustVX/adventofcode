using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2020
{
    public class Processor
    {
        public Processor(OpCode[] program)
        {
            Program = program;
        }

        public OpCode[] Program { get; }

        public int Accumulator { get; set; }
        public int InstructionPointer { get; set; }
        public ref OpCode CurrentOpCode => ref Program[InstructionPointer];

        public bool Run()
        {
            do
            {
                if (!Program[InstructionPointer].Run(this))
                    return false;
            } while (InstructionPointer < Program.Length);
            return true;
        }
    }
}