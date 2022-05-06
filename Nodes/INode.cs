using System.Collections.Generic;

namespace parser_helper
{
    public interface INode
    {
        public dynamic SolveTree(IDictionary<string, dynamic> vars = null);

        public string GetTree(IDictionary<string, dynamic> vars = null);
    }
}
