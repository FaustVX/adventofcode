using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2020
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public abstract class OpCode
    {
        private static readonly Regex _opCodeIntRegex = new(@"(.*) ([+-]\d*)");
        public bool Visited { get; protected set; }

        public bool Run(Processor processor)
        {
            if (Visited)
                return false;

            RunImpl(processor);
            return Visited = true;
        }

        protected abstract void RunImpl(Processor processor);

        public static OpCode Parse(string input)
        {
            var group = _opCodeIntRegex.Match(input).Groups;
            return (group[1].Value, int.Parse(group[2].Value)) switch
            {
                ("nop", var v) => new Nop(v),
                ("acc", var v) => new Acc(v),
                ("jmp", var o) => new Jmp(o),
            };
        }

        protected virtual string GetDebuggerDisplay()
            => (Visited ? "!" : "") + this.GetType().Name;

        public sealed class Nop : OpCode
        {
            public Nop(int value)
            {
                Value = value;
            }

            public int Value { get; }

            protected override void RunImpl(Processor processor)
            { }
        }

        public sealed class Acc : OpCode
        {
            public Acc(int value)
            {
                Value = value;
            }

            public int Value { get; }

            protected override void RunImpl(Processor processor)
            {
                processor.Accumulator += Value;
            }

            protected override string GetDebuggerDisplay()
                => $"{base.GetDebuggerDisplay()} ({Value})";
        }

        public sealed class Jmp : OpCode
        {
            public Jmp(int offset)
            {
                Offset = offset;
            }

            public int Offset { get; }

            protected override void RunImpl(Processor processor)
            {
                processor.InstructionPointer += Offset - 1;
            }

            protected override string GetDebuggerDisplay()
                => $"{base.GetDebuggerDisplay()} ({Offset})";
        }
    }
}