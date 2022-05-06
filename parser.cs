using lexer;
using System;
using System.Collections.Generic;

using parser_helper;

namespace parser {
public class Parser {
  private readonly bool debug;
  private Stack<(uint state, dynamic value)> stack = new Stack<(uint state, dynamic value)>();
  private static uint[,] Action = new uint[,] {
    {48,22,48,48,48,48,46,48,48,48,42,21,7,13},
    {49,48,48,48,48,48,48,48,48,48,48,48,48,48},
    {1,48,48,48,48,48,48,48,48,48,48,48,48,48},
    {50,48,48,48,36,48,37,48,48,48,48,48,48,48},
    {48,22,48,48,48,48,46,48,48,48,42,21,48,48},
    {48,6,48,48,48,48,48,48,11,48,48,48,48,48},
    {48,48,48,48,48,48,48,48,48,48,25,48,48,48},
    {48,48,48,48,48,48,48,48,48,48,5,48,48,48},
    {48,48,48,48,48,48,48,48,4,48,48,48,48,48},
    {48,48,8,48,48,48,48,48,48,48,48,48,48,48},
    {51,48,48,48,36,48,37,48,48,48,48,48,48,48},
    {48,22,48,48,48,48,46,48,48,48,42,21,48,48},
    {52,48,48,48,36,48,37,48,48,48,48,48,48,48},
    {48,22,48,48,48,48,46,48,48,48,15,21,48,48},
    {53,48,48,48,36,48,37,48,48,48,48,48,48,48},
    {54,16,54,54,54,54,54,54,48,54,48,48,48,48},
    {48,22,17,48,48,48,46,48,48,48,42,21,48,48},
    {55,48,48,48,48,48,48,48,48,48,48,48,48,48},
    {56,48,56,32,56,56,56,38,48,48,48,48,48,48},
    {57,48,57,57,57,57,57,57,48,40,48,48,48,48},
    {58,48,58,58,58,58,58,58,48,48,48,48,48,48},
    {59,48,59,59,59,59,59,59,48,59,48,48,48,48},
    {48,22,48,48,48,48,46,48,48,48,42,21,48,48},
    {60,48,60,60,60,60,60,60,48,60,48,48,48,48},
    {48,48,23,48,36,48,37,48,48,48,48,48,48,48},
    {48,48,48,48,48,48,48,48,48,48,26,48,48,48},
    {48,48,61,48,48,27,48,48,48,48,48,48,48,48},
    {48,48,48,48,48,48,48,48,48,48,25,48,48,48},
    {48,48,62,48,48,48,48,48,48,48,48,48,48,48},
    {48,48,63,48,36,30,37,48,48,48,48,48,48,48},
    {48,22,48,48,48,48,46,48,48,48,42,21,48,48},
    {48,48,64,48,48,48,48,48,48,48,48,48,48,48},
    {48,22,48,48,48,48,46,48,48,48,42,21,48,48},
    {65,48,65,65,65,65,65,65,48,48,48,48,48,48},
    {66,48,66,32,66,66,66,38,48,48,48,48,48,48},
    {67,48,67,32,67,67,67,38,48,48,48,48,48,48},
    {48,22,48,48,48,48,46,48,48,48,42,21,48,48},
    {48,22,48,48,48,48,46,48,48,48,42,21,48,48},
    {48,22,48,48,48,48,46,48,48,48,42,21,48,48},
    {68,48,68,68,68,68,68,68,48,48,48,48,48,48},
    {48,22,48,48,48,48,46,48,48,48,42,21,48,48},
    {69,48,69,69,69,69,69,69,48,48,48,48,48,48},
    {54,43,54,54,54,54,54,54,48,54,48,48,48,48},
    {48,22,48,48,48,48,46,48,48,48,42,21,48,48},
    {70,48,70,70,70,70,70,70,48,70,48,48,48,48},
    {48,48,44,48,48,48,48,48,48,48,48,48,48,48},
    {48,22,48,48,48,48,46,48,48,48,42,21,48,48},
    {71,48,71,71,71,71,71,71,48,71,48,48,48,48}
  };
  private static uint[,] GOTO = new uint[,] {
    {3,20,2,18,19,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {10,20,0,18,19,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,9,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {12,20,0,18,19,0,0},
    {0,0,0,0,0,0,0},
    {14,20,0,18,19,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {29,20,0,18,19,0,45},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {24,20,0,18,19,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,28,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {29,20,0,18,19,0,31},
    {0,0,0,0,0,0,0},
    {0,33,0,0,19,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,20,0,34,19,0,0},
    {0,20,0,35,19,0,0},
    {0,39,0,0,19,0,0},
    {0,0,0,0,0,0,0},
    {0,41,0,0,19,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {29,20,0,18,19,0,45},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,47,0,0},
    {0,0,0,0,0,0,0}
  };
  private uint top() {
    return stack.Count == 0 ? 0 : stack.Peek().state;
  }
  static string[] stateNames = new string[] {".","%eof","S","E","equal","id","lparen","parami","rparen","D1","E","equal","E","paramt","E","id","lparen","rparen","T","V","F","num","lparen","rparen","E","id","id","comma","D1","E","comma","D2","mult","F","T","T","plus","minus","divide","F","power","F","id","lparen","rparen","D2","minus","V"};
  static string[] expectedSyms = new string[] {"S","%eof","%eof","%eof/plus/minus","E","lparen/equal","D1","id/id","equal","rparen","%eof/plus/minus","E","%eof/plus/minus","E/id","%eof/plus/minus","lparen/lparen/%eof/comma/divide/minus/mult/plus/power/rparen","rparen/D2","%eof","%eof/comma/minus/plus/rparen/mult/divide","%eof/comma/divide/minus/mult/plus/rparen/power","%eof/comma/divide/minus/mult/plus/rparen","%eof/comma/divide/minus/mult/plus/power/rparen","E","%eof/comma/divide/minus/mult/plus/power/rparen","rparen/plus/minus","id/id","rparen/comma","D1","rparen","rparen/comma/plus/minus","D2","rparen","F","%eof/comma/divide/minus/mult/plus/rparen","mult/%eof/comma/minus/plus/rparen/divide","mult/%eof/comma/minus/plus/rparen/divide","T","T","F","%eof/comma/divide/minus/mult/plus/rparen","F","%eof/comma/divide/minus/mult/plus/rparen","lparen/%eof/comma/divide/minus/mult/plus/power/rparen","D2","%eof/comma/divide/minus/mult/plus/power/rparen","rparen","V","%eof/comma/divide/minus/mult/plus/power/rparen"};

  public Parser(bool debug = false) {
    this.debug = debug;
  }
  public dynamic parse(IEnumerable<(TokenType type, dynamic attr)> tokens) {
    stack.Clear();
    var iter = tokens.GetEnumerator();
    iter.MoveNext();
    var a = iter.Current;
    while (true) {
      var action = Action[top(), (int)a.type];
      switch (action) {
      case 49: {
          stack.Pop();
          return stack.Pop().value;
        }
      case 61: {
          if(debug) Console.Error.WriteLine("Reduce using D1 -> id id");
          var _2=stack.Pop().value.Item2;
          var _1=stack.Pop().value.Item2;
          var gt = GOTO[top(), 5 /*D1*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(new List<(Type, string)>(){ (ParserHelper.GetTypeFromString(_1), _2) })));
          break;
        }
      case 62: {
          if(debug) Console.Error.WriteLine("Reduce using D1 -> id id comma D1");
          dynamic _4=stack.Pop().value;
          var _3=stack.Pop().value.Item2;
          var _2=stack.Pop().value.Item2;
          var _1=stack.Pop().value.Item2;
          var gt = GOTO[top(), 5 /*D1*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(new List<(Type, string)>(_4){ (ParserHelper.GetTypeFromString(_1), _2) })));
          break;
        }
      case 63: {
          if(debug) Console.Error.WriteLine("Reduce using D2 -> E");
          dynamic _1=stack.Pop().value;
          var gt = GOTO[top(), 6 /*D2*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(new List<INode>(){ _1 })));
          break;
        }
      case 64: {
          if(debug) Console.Error.WriteLine("Reduce using D2 -> E comma D2");
          dynamic _3=stack.Pop().value;
          var _2=stack.Pop().value.Item2;
          dynamic _1=stack.Pop().value;
          var gt = GOTO[top(), 6 /*D2*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(new List<INode>(_3){ _1 })));
          break;
        }
      case 67: {
          if(debug) Console.Error.WriteLine("Reduce using E -> E minus T");
          dynamic _3=stack.Pop().value;
          var _2=stack.Pop().value.Item2;
          dynamic _1=stack.Pop().value;
          var gt = GOTO[top(), 0 /*E*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(new NodeBinary('-', new INode[] { _1, _3 }))));
          break;
        }
      case 66: {
          if(debug) Console.Error.WriteLine("Reduce using E -> E plus T");
          dynamic _3=stack.Pop().value;
          var _2=stack.Pop().value.Item2;
          dynamic _1=stack.Pop().value;
          var gt = GOTO[top(), 0 /*E*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(new NodeBinary('+', new INode[] { _1, _3 }))));
          break;
        }
      case 56: {
          if(debug) Console.Error.WriteLine("Reduce using E -> T");
          dynamic _1=stack.Pop().value;
          var gt = GOTO[top(), 0 /*E*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(_1)));
          break;
        }
      case 57: {
          if(debug) Console.Error.WriteLine("Reduce using F -> V");
          dynamic _1=stack.Pop().value;
          var gt = GOTO[top(), 1 /*F*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(_1)));
          break;
        }
      case 69: {
          if(debug) Console.Error.WriteLine("Reduce using F -> V power F");
          dynamic _3=stack.Pop().value;
          var _2=stack.Pop().value.Item2;
          dynamic _1=stack.Pop().value;
          var gt = GOTO[top(), 1 /*F*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(new NodeBinary('^', new INode[] { _1, _3 }))));
          break;
        }
      case 52: {
          if(debug) Console.Error.WriteLine("Reduce using S -> parami id equal E");
          dynamic _4=stack.Pop().value;
          var _3=stack.Pop().value.Item2;
          var _2=stack.Pop().value.Item2;
          var _1=stack.Pop().value.Item2;
          var gt = GOTO[top(), 2 /*S*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(ParserHelper.AddVar(_2, _4.SolveTree()))));
          break;
        }
      case 51: {
          if(debug) Console.Error.WriteLine("Reduce using S -> parami id lparen D1 rparen equal E");
          dynamic _7=stack.Pop().value;
          var _6=stack.Pop().value.Item2;
          var _5=stack.Pop().value.Item2;
          dynamic _4=stack.Pop().value;
          var _3=stack.Pop().value.Item2;
          var _2=stack.Pop().value.Item2;
          var _1=stack.Pop().value.Item2;
          var gt = GOTO[top(), 2 /*S*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(ParserHelper.AddFunc(_2, new Func(_7, _4)))));
          break;
        }
      case 55: {
          if(debug) Console.Error.WriteLine("Reduce using S -> paramt id lparen rparen");
          var _4=stack.Pop().value.Item2;
          var _3=stack.Pop().value.Item2;
          var _2=stack.Pop().value.Item2;
          var _1=stack.Pop().value.Item2;
          var gt = GOTO[top(), 2 /*S*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(ParserHelper.GetFuncTypes(_2))));
          break;
        }
      case 53: {
          if(debug) Console.Error.WriteLine("Reduce using S -> paramt E");
          dynamic _2=stack.Pop().value;
          var _1=stack.Pop().value.Item2;
          var gt = GOTO[top(), 2 /*S*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(ParserHelper.ConvertTypeToString(_2.SolveTree().GetType()))));
          break;
        }
      case 50: {
          if(debug) Console.Error.WriteLine("Reduce using S -> E");
          dynamic _1=stack.Pop().value;
          var gt = GOTO[top(), 2 /*S*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,($"Tree: {_1.GetTree()}\nValue: {_1.SolveTree()}")));
          break;
        }
      case 58: {
          if(debug) Console.Error.WriteLine("Reduce using T -> F");
          dynamic _1=stack.Pop().value;
          var gt = GOTO[top(), 3 /*T*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(_1)));
          break;
        }
      case 68: {
          if(debug) Console.Error.WriteLine("Reduce using T -> T divide F");
          dynamic _3=stack.Pop().value;
          var _2=stack.Pop().value.Item2;
          dynamic _1=stack.Pop().value;
          var gt = GOTO[top(), 3 /*T*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(new NodeBinary('/', new INode[] { _1, _3 }))));
          break;
        }
      case 65: {
          if(debug) Console.Error.WriteLine("Reduce using T -> T mult F");
          dynamic _3=stack.Pop().value;
          var _2=stack.Pop().value.Item2;
          dynamic _1=stack.Pop().value;
          var gt = GOTO[top(), 3 /*T*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(new NodeBinary('*', new INode[] { _1, _3 }))));
          break;
        }
      case 54: {
          if(debug) Console.Error.WriteLine("Reduce using V -> id");
          var _1=stack.Pop().value.Item2;
          var gt = GOTO[top(), 4 /*V*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(new NodeVar(_1))));
          break;
        }
      case 70: {
          if(debug) Console.Error.WriteLine("Reduce using V -> id lparen D2 rparen");
          var _4=stack.Pop().value.Item2;
          dynamic _3=stack.Pop().value;
          var _2=stack.Pop().value.Item2;
          var _1=stack.Pop().value.Item2;
          var gt = GOTO[top(), 4 /*V*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(new NodeFunction(_1, _3.ToArray()))));
          break;
        }
      case 60: {
          if(debug) Console.Error.WriteLine("Reduce using V -> lparen E rparen");
          var _3=stack.Pop().value.Item2;
          dynamic _2=stack.Pop().value;
          var _1=stack.Pop().value.Item2;
          var gt = GOTO[top(), 4 /*V*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(_2)));
          break;
        }
      case 71: {
          if(debug) Console.Error.WriteLine("Reduce using V -> minus V");
          dynamic _2=stack.Pop().value;
          var _1=stack.Pop().value.Item2;
          var gt = GOTO[top(), 4 /*V*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(new NodeUnary(_2))));
          break;
        }
      case 59: {
          if(debug) Console.Error.WriteLine("Reduce using V -> num");
          var _1=stack.Pop().value.Item2;
          var gt = GOTO[top(), 4 /*V*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(_1)));
          break;
        }
      case 48: {
          string parsed=stateNames[top()];
          var lastSt = top();
          while(stack.Count > 0) { stack.Pop(); parsed = stateNames[top()] + " " + parsed; }
          throw new ApplicationException(
            $"Rejection state reached after parsing \"{parsed}\", when encoutered symbol \""
            + $"\"{a.type}\" in state {lastSt}. Expected \"{expectedSyms[lastSt]}\"");
        }
      default:
        if(debug) Console.Error.WriteLine($"Shift to {action}");
        stack.Push((action, a));
        iter.MoveNext();
        a=iter.Current;
        break;
      }
    }
  }
}
}