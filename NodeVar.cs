using System.Collections.Generic;

namespace parser_helper
{
    class NodeVar : INode
    {
        public string Value { get; private set; }

        public NodeVar(string value)
        {
            Value = value;
        }

        public string GetTree(IDictionary<string, dynamic> vars = null)
        {
            return Value;
        }

        public dynamic SolveTree(IDictionary<string, dynamic> vars = null)
        {
            if (vars != null)
            {
                dynamic num;
                if (vars.TryGetValue(Value, out num))
                {
                    return num;
                }
            }
            return ParserHelper.GetNumFromVar(Value);
        }
    }
}
