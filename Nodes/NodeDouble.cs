using System.Collections.Generic;

namespace parser_helper
{
    class NodeDouble : INode
    {
        public double Value { get; private set; }

        public NodeDouble(double value)
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
