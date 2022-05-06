using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parser_helper
{
    class NodeFunction : INode
    {
        public INode[] Children { get; private set; }

        public string Value { get; private set; }

        public NodeFunction(string value, INode[] children)
        {
            Value = value;
            Children = children;
        }

        public string GetTree(IDictionary<string, dynamic> vars = null)
        {
            StringBuilder str = new StringBuilder($"{Value}({(Children.Length > 0 ? Children.Last().GetTree(vars) : "")}");
            for (int i = Children.Length - 2; i >=0; --i)
            {
                str.Append($", {Children[i].GetTree(vars)}");
            }
            str.Append(')');
            return str.ToString();
        }

        public dynamic SolveTree(IDictionary<string, dynamic> vars = null)
        {
            dynamic[] values = new dynamic[Children.Length];
            for (int i = Children.Length - 1; i >= 0; --i)
            {
                values[i] = Children[i].SolveTree(vars);
            }
            return ParserHelper.ActivateFunc(Value, values);
        }
    }
}
