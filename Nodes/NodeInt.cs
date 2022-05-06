using System.Collections.Generic;

namespace parser_helper
{
    class NodeInt : INode
    {
        public int Value { get; private set; }

        public NodeInt(int value)
        {
            Value = value;
        }

        public string GetTree(IDictionary<string, dynamic> vars = null)
        {
            return Value.ToString();
        }

        public dynamic SolveTree(IDictionary<string, dynamic> vars = null)
        {
            return Value;
        }
    }
}
