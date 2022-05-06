using System;
using System.Collections.Generic;

namespace parser_helper
{
    class NodeBinary : INode
    {
        public INode[] Children { get; private set; }

        public char Value { get; private set; }

        public NodeBinary(char value, INode[] children)
        {
            Value = value;
            Children = children;
        }

        public string GetTree(IDictionary<string, dynamic> vars = null)
        {
            return $"{Value}({Children[0].GetTree(vars)}, {Children[1].GetTree(vars)})";
        }

        public dynamic SolveTree(IDictionary<string, dynamic> vars = null)
        {
            dynamic num1 = Children[0].SolveTree(vars);
            dynamic num2 = Children[1].SolveTree(vars);

            switch (Value)
            {
                case '+':
                    return num1 + num2;
                case '-':
                    return num1 - num2;
                case '*':
                    return num1 * num2;
                case '/':
                    return num1 / num2;
                case '^':
                    return (num1 is double || num2 is double) ? Math.Pow(num1, num2) : ParserHelper.IntPow(num1, num2);
            }

            throw new ApplicationException("Unknown error while solving tree.");
        }
    }
}
