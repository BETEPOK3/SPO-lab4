using lexer;
using System;
using System.Collections.Generic;

namespace parser {
public class Parser {
  private readonly bool debug;
  private Stack<(uint state, dynamic value)> stack = new Stack<(uint state, dynamic value)>();
  private static uint[,] Action = new uint[,] {
    {46,18,46,46,46,46,40,46,46,46,33,42,7},
    {47,46,46,46,46,46,46,46,46,46,46,46,46},
    {1,46,46,46,46,46,46,46,46,46,46,46,46},
    {48,46,46,46,43,46,37,46,46,46,46,46,46},
    {46,18,46,46,46,46,40,46,46,46,33,42,46},
    {49,6,49,49,49,49,49,49,11,49,46,46,46},
    {46,18,14,46,46,46,40,46,46,46,22,42,46},
    {46,18,46,46,46,46,40,46,46,46,5,42,46},
    {46,46,46,46,46,46,46,46,4,46,46,46,46},
    {46,46,8,46,46,46,46,46,46,46,46,46,46},
    {50,46,46,46,43,46,37,46,46,46,46,46,46},
    {46,18,46,46,46,46,40,46,46,46,33,42,46},
    {51,46,46,46,43,46,37,46,46,46,46,46,46},
    {52,46,46,46,43,46,37,46,46,46,46,46,46},
    {53,46,46,46,46,46,46,46,46,46,46,46,46},
    {54,46,54,38,54,54,54,29,46,46,46,46,46},
    {55,46,55,55,55,55,55,55,46,44,46,46,46},
    {56,46,56,56,56,56,56,56,46,46,46,46,46},
    {46,18,46,46,46,46,40,46,46,46,33,42,46},
    {57,46,57,57,57,57,57,57,46,57,46,46,46},
    {46,46,19,46,43,46,37,46,46,46,46,46,46},
    {46,46,46,46,46,46,46,46,46,46,23,46,46},
    {49,34,49,49,49,49,49,49,46,49,23,46,46},
    {46,46,58,46,46,24,46,46,46,46,46,46,46},
    {46,46,46,46,46,46,46,46,46,46,21,46,46},
    {46,46,59,46,46,46,46,46,46,46,46,46,46},
    {46,46,60,46,43,27,37,46,46,46,46,46,46},
    {46,18,46,46,46,46,40,46,46,46,33,42,46},
    {46,46,61,46,46,46,46,46,46,46,46,46,46},
    {46,18,46,46,46,46,40,46,46,46,33,42,46},
    {62,46,62,62,62,62,62,62,46,46,46,46,46},
    {63,46,63,38,63,63,63,29,46,46,46,46,46},
    {64,46,64,38,64,64,64,29,46,46,46,46,46},
    {49,34,49,49,49,49,49,49,46,49,46,46,46},
    {46,18,46,46,46,46,40,46,46,46,33,42,46},
    {65,46,65,65,65,65,65,65,46,65,46,46,46},
    {46,46,35,46,46,46,46,46,46,46,46,46,46},
    {46,18,46,46,46,46,40,46,46,46,33,42,46},
    {46,18,46,46,46,46,40,46,46,46,33,42,46},
    {66,46,66,66,66,66,66,66,46,46,46,46,46},
    {46,18,46,46,46,46,40,46,46,46,33,42,46},
    {67,46,67,67,67,67,67,67,46,67,46,46,46},
    {68,46,68,68,68,68,68,68,46,68,46,46,46},
    {46,18,46,46,46,46,40,46,46,46,33,42,46},
    {46,18,46,46,46,46,40,46,46,46,33,42,46},
    {69,46,69,69,69,69,69,69,46,46,46,46,46}
  };
  private static uint[,] GOTO = new uint[,] {
    {3,17,2,15,16,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {10,17,0,15,16,0,0},
    {0,0,0,0,0,0,0},
    {26,17,0,15,16,9,36},
    {13,17,0,15,16,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {12,17,0,15,16,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {20,17,0,15,16,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,25,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {26,17,0,15,16,0,28},
    {0,0,0,0,0,0,0},
    {0,30,0,0,16,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {26,17,0,15,16,0,36},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,17,0,31,16,0,0},
    {0,39,0,0,16,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,41,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,17,0,32,16,0,0},
    {0,45,0,0,16,0,0},
    {0,0,0,0,0,0,0}
  };
  private uint top() {
    return stack.Count == 0 ? 0 : stack.Peek().state;
  }
  static string[] stateNames = new string[] {".","%eof","S","E","equal","id","lparen","param","rparen","D1","E","equal","E","E","rparen","T","V","F","lparen","rparen","E","id","id","id","comma","D1","E","comma","D2","divide","F","T","T","id","lparen","rparen","D2","minus","mult","F","minus","V","num","plus","power","F"};
  static string[] expectedSyms = new string[] {"S","%eof","%eof","%eof/minus/plus","E","lparen/equal/lparen/%eof/comma/divide/minus/mult/plus/power/rparen/lparen","D1/rparen/D2","id/id/E/id","equal","rparen","%eof/minus/plus","E","%eof/minus/plus","%eof/minus/plus","%eof","%eof/comma/minus/plus/rparen/divide/mult","%eof/comma/divide/minus/mult/plus/rparen/power","%eof/comma/divide/minus/mult/plus/rparen","E","%eof/comma/divide/minus/mult/plus/power/rparen","rparen/minus/plus","id/id","id/id/%eof/comma/divide/minus/mult/plus/power/rparen/lparen","rparen/comma","D1","rparen","rparen/comma/minus/plus","D2","rparen","F","%eof/comma/divide/minus/mult/plus/rparen","divide/%eof/comma/minus/plus/rparen/mult","divide/mult/%eof/comma/minus/plus/rparen","%eof/comma/divide/minus/mult/plus/power/rparen/lparen","D2","%eof/comma/divide/minus/mult/plus/power/rparen","rparen","T","F","%eof/comma/divide/minus/mult/plus/rparen","V","%eof/comma/divide/minus/mult/plus/power/rparen","%eof/comma/divide/minus/mult/plus/power/rparen","T","F","%eof/comma/divide/minus/mult/plus/rparen"};

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
      case 47: {
          stack.Pop();
          return stack.Pop().value;
        }
      case 58: {
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
      case 59: {
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
      case 60: {
          if(debug) Console.Error.WriteLine("Reduce using D2 -> E");
          dynamic _1=stack.Pop().value;
          var gt = GOTO[top(), 6 /*D2*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(new List<Node>(){ _1 })));
          break;
        }
      case 61: {
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
          stack.Push((gt,(new List<Node>(_3){ _1 })));
          break;
        }
      case 63: {
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
          stack.Push((gt,(new Node((TokenType.Tok_minus, _2), new Node[]{ _1, _3 }))));
          break;
        }
      case 64: {
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
          stack.Push((gt,(new Node((TokenType.Tok_plus, _2), new Node[]{ _1, _3 }))));
          break;
        }
      case 54: {
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
      case 55: {
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
          stack.Push((gt,(new Node((TokenType.Tok_power, _2), new Node[]{ _1, _3 }))));
          break;
        }
      case 51: {
          if(debug) Console.Error.WriteLine("Reduce using S -> param id equal E");
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
          stack.Push((gt,((_1[1] == 'i' && _1.Length == 2) ? ParserHelper.AddVar(_2, _4.SolveTree()) : throw new ApplicationException($"Parameter should be ':i', not '{_1}'"))));
          break;
        }
      case 53: {
          if(debug) Console.Error.WriteLine("Reduce using S -> param id lparen rparen");
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
          stack.Push((gt,((_1[1] == 't' && _1.Length == 2) ? ParserHelper.GetFuncTypes(_2) : throw new ApplicationException($"Parameter should be ':t', not '{_1}'"))));
          break;
        }
      case 50: {
          if(debug) Console.Error.WriteLine("Reduce using S -> param id lparen D1 rparen equal E");
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
          stack.Push((gt,((_1[1] == 'i' && _1.Length == 2) ? ParserHelper.AddFunc(_2, new Func(_7, _4)) : throw new ApplicationException($"Parameter should be ':i', not '{_1}'"))));
          break;
        }
      case 52: {
          if(debug) Console.Error.WriteLine("Reduce using S -> param E");
          dynamic _2=stack.Pop().value;
          var _1=stack.Pop().value.Item2;
          var gt = GOTO[top(), 2 /*S*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,((_1[1] == 't' && _1.Length == 2) ? ParserHelper.ConvertTypeToString(_2.SolveTree().GetType()) : throw new ApplicationException($"Parameter should be ':t', not '{_1}'"))));
          break;
        }
      case 48: {
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
      case 56: {
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
      case 62: {
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
          stack.Push((gt,(new Node((TokenType.Tok_divide, _2), new Node[]{ _1, _3}))));
          break;
        }
      case 66: {
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
          stack.Push((gt,(new Node((TokenType.Tok_mult, _2), new Node[]{ _1, _3 }))));
          break;
        }
      case 49: {
          if(debug) Console.Error.WriteLine("Reduce using V -> id");
          var _1=stack.Pop().value.Item2;
          var gt = GOTO[top(), 4 /*V*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(new Node((TokenType.Tok_id, _1)))));
          break;
        }
      case 65: {
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
          stack.Push((gt,(new Node((TokenType.Tok_id, _1), _3.ToArray()))));
          break;
        }
      case 57: {
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
      case 67: {
          if(debug) Console.Error.WriteLine("Reduce using V -> minus V");
          dynamic _2=stack.Pop().value;
          var _1=stack.Pop().value.Item2;
          var gt = GOTO[top(), 4 /*V*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(new Node((TokenType.Tok_num, -_2)))));
          break;
        }
      case 68: {
          if(debug) Console.Error.WriteLine("Reduce using V -> num");
          var _1=stack.Pop().value.Item2;
          var gt = GOTO[top(), 4 /*V*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(new Node((TokenType.Tok_num, _1)))));
          break;
        }
      case 46: {
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