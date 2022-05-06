using System.Collections.Generic;

namespace parser_helper
{
    class NodeUnary : INode
    {
        public INode Child { get; private set; }

        public char Value { get { return '-'; } }

        public NodeUnary(INode child)
        {
            Child = child;
        }

        public string GetTree(IDictionary<string, dynamic> vars = null)
        {
            return $"neg({Child.GetTree(vars)})";
        }

        public dynamic SolveTree(IDictionary<string, dynamic> vars = null)
        {
            return -Child.SolveTree(vars);
        }
    }
}
