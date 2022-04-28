using System;
using System.Collections.Generic;
using System.Text;
using lexer;

namespace parser
{
    public class Node
    {
        public Node[] Children { get; private set; }

        public (TokenType, dynamic) Value { get; private set; }

        public Node((TokenType, dynamic) value, Node[] children = null)
        {
            Value = value;
            Children = children;
        }

        /// <summary>
        /// Вычислить значение выражения (дерева)
        /// </summary>
        /// <param name="vars">Возможные переменные</param>
        /// <returns>Вычисленное значение</returns>
        public dynamic SolveTree(IDictionary<string, dynamic> vars = null)
        {
            // Число или переменная
            if (Children == null)
            {
                if (Value.Item1 == TokenType.Tok_id)
                {
                    dynamic num = 0;
                    if (vars != null && vars.TryGetValue(Value.Item2, out num))
                    {
                        return num;
                    }
                    return ParserHelper.GetNumFromVar(Value.Item2);
                }
                else
                {
                    return Value.Item2;
                }
            }
            else
            {
                // Функция
                if (Value.Item1 == TokenType.Tok_id)
                {
                    dynamic[] values = new dynamic[Children.Length];
                    for (int i = 0; i < Children.Length; ++i)
                    {
                        values[i] = Children[i].SolveTree(vars);
                    }
                    return ParserHelper.ActivateFunc(Value.Item2, values);
                }

                // Бинарная операция
                else
                {
                    dynamic num1 = Children[0].SolveTree(vars);
                    dynamic num2 = Children[1].SolveTree(vars);
                    switch (Value.Item1)
                    {
                        case TokenType.Tok_plus:
                            return num1 + num2;
                        case TokenType.Tok_minus:
                            return num1 - num2;
                        case TokenType.Tok_mult:
                            return num1 * num2;
                        case TokenType.Tok_divide:
                            return num1 / num2;
                        case TokenType.Tok_power:
                            return (num1 is int && num2 is int) ? num1 ^ num2 : Math.Pow(num1, num2);
                    }
                }
            }
            throw new ApplicationException($"Unknown error while solving tree");
        }

        /// <summary>
        /// Создание строкового представления дерева
        /// </summary>
        /// <param name="vars">Возможные переменные</param>
        /// <returns></returns>
        public string GetTree(IDictionary<string, double> vars = null)
        {
            // Число или переменная
            if (Children == null)
            {
                return Value.Item2.ToString();
            }
            else
            {
                // Функция
                if (Value.Item1 == TokenType.Tok_id)
                {
                    int childrenLength = Children.Length;
                    StringBuilder result = new StringBuilder($"{Value.Item2}({Children[childrenLength - 1].GetTree(vars)}");
                    for (int i = childrenLength - 2; i >= 0; --i)
                    {
                        result.Append($", {Children[i].GetTree(vars)}");
                    }
                    result.Append(')');
                    return result.ToString();
                }

                // Бинарная операция
                else
                {
                    return $"{Value.Item2}({Children[0].GetTree(vars)}, {Children[1].GetTree(vars)})";
                }
            }
            throw new ApplicationException($"Unknown error while getting tree");
        }

        /// <summary>
        /// Проверить дерево на возможность подставить значения и соответствие типов переменных (double -/> int)
        /// </summary>
        /// <param name="vars">Возможные названия переменных</param>
        public void CheckTree(string[] vars = null)
        {
            // Переменная
            if (Children == null)
            {
                if (Value.Item2 is string)
                {
                    if (vars != null && Array.IndexOf(vars, Value.Item2) == -1)
                    {
                        ParserHelper.GetNumFromVar(Value.Item2);
                    }
                }
            }
            else
            {
                // Функция
                if (Value.Item1 == TokenType.Tok_id)
                {
                    Func func = ParserHelper.GetFuncFromName(Value.Item2);
                    if (func.Vars.Length != Children.Length)
                    {
                        throw new ApplicationException($"Function {Value.Item2} has {func.Vars.Length} arguments, not {Children.Length}");
                    }
                    foreach (Node node in Children)
                    {
                        node.CheckTree(vars);
                    }
                }

                // Бинарная операция
                else
                {
                    Children[0].CheckTree(vars);
                    Children[1].CheckTree(vars);
                }
            }
        }
    }

    public class Func
    {
        public Node Root { get; private set; }

        public (Type, string)[] Vars { get; private set; }

        public Func(Node root, List<(Type, string)> vars)
        {
            Root = root;
            Vars = vars.ToArray();
        }
    }

    public class Var
    {
        public dynamic Value { get; private set; }

        public Var(dynamic value)
        {
            Value = value;
        }
    }

    public static class ParserHelper
    {
        private static IDictionary<string, Var> _vars = new Dictionary<string, Var>()
        {
            { "pi", new Var(Math.PI) },
            { "e", new Var(Math.E) }
        };

        private static IDictionary<string, Func> _funcs = new Dictionary<string, Func>();

        /// <summary>
        /// Конвертирование строки в число. По умолчанию парсит в Int, но при невозможности это сделать парсит в Double
        /// </summary>
        /// <param name="strNum">Строчное представление числа</param>
        /// <returns>Число</returns>
        public static dynamic ConvertToNum(string strNum)
        {
            try
            {
                return Convert.ToInt32(strNum);
            }
            catch
            {
                return Convert.ToDouble(strNum.Replace('.', ','));
            }
        }

        /// <summary>
        /// Возвращает строчное представление типов данных в необходимом формате (пример: int вместо System.Int)
        /// </summary>
        /// <param name="type">Тип данных</param>
        /// <returns>Строчное представление типа данных</returns>
        public static string ConvertTypeToString(Type type)
        {
            if (type == typeof(double))
            {
                return "double";
            }
            else
            {
                return "int";
            }
        }

        /// <summary>
        /// Возвращает тип данных из строчного представления типа данных (пример: int -> System.Int)
        /// </summary>
        /// <param name="typeStr">Строчное представление типа данных</param>
        /// <returns>Тип данных</returns>
        public static Type GetTypeFromString(string typeStr)
        {
            if (typeStr.Equals("double"))
            {
                return typeof(double);
            }
            else
            {
                return typeof(int);
            }
        }

        /// <summary>
        /// Возвращает строку с типами данных параметров функции
        /// </summary>
        /// <param name="funcName">Название функции</param>
        /// <returns>Типы данных параметров</returns>
        public static string GetFuncTypes(string funcName)
        {
            Func func = GetFuncFromName(funcName);
            int varCount = func.Vars.Length;
            StringBuilder result = new StringBuilder($"({ConvertTypeToString(func.Vars[varCount - 1].Item1)}");
            for (int i = varCount - 2; i >= 0; --i)
            {
                result.Append($", {ConvertTypeToString(func.Vars[i].Item1)}");
            }
            result.Append(')');
            return result.ToString();
        }

        /// <summary>
        /// Возвращает значение переменной
        /// </summary>
        /// <param name="id">Название переменной</param>
        /// <returns>Значение переменной</returns>
        public static dynamic GetNumFromVar(string id)
        {
            Var result;
            if (_vars.TryGetValue(id, out result))
            {
                return result.Value;
            }
            throw new ApplicationException($"Unknown id {id}");
        }

        /// <summary>
        /// Возвращает объект функции по её наименованию
        /// </summary>
        /// <param name="id">Наименование</param>
        /// <returns>Объект функции</returns>
        public static Func GetFuncFromName(string id)
        {
            Func result;
            if (_funcs.TryGetValue(id, out result))
            {
                return result;
            }
            throw new ApplicationException($"Unknown function {id}");
        }

        /// <summary>
        /// Добавляет переменную в хранилище
        /// </summary>
        /// <param name="id">Наименование переменной</param>
        /// <param name="num">Значение переменной</param>
        /// <returns>Строка с результатом</returns>
        public static string AddVar(string id, dynamic num)
        {
            if (!_vars.TryAdd(id, new Var(num)))
            {
                _vars.Remove(id);
                _vars.Add(id, new Var(num));
                return $"Changed var {id} to {num}";
            }
            return $"Added new var {id} = {num}";
        }

        /// <summary>
        /// Добавляет функцию в хранилище
        /// </summary>
        /// <param name="id">Наименование функции</param>
        /// <param name="func">Объект функции</param>
        /// <returns>Строка с результатом</returns>
        public static string AddFunc(string id, Func func)
        {
            string[] vars = new string[func.Vars.Length];
            for (int i = 0; i < vars.Length; ++i)
            {
                vars[i] = func.Vars[i].Item2;
            }

            // Проверка наличия объявленных переменных и добавление функции
            func.Root.CheckTree(vars);
            if (!_funcs.TryAdd(id, func))
            {
                _funcs.Remove(id);
                _funcs.Add(id, func);
                return $"Changed function {id}";
            }
            return $"Added new function {id}";
        }

        /// <summary>
        /// Выполнить функцию
        /// </summary>
        /// <param name="funcName">Наименование функции</param>
        /// <param name="values">Аргументы</param>
        /// <returns>Возвращённое функцией значение</returns>
        public static dynamic ActivateFunc(string funcName, dynamic[] values)
        {
            Func func = GetFuncFromName(funcName);
            if (func.Vars.Length == values.Length)
            {
                Dictionary<string, dynamic> varsInfo = new Dictionary<string, dynamic>();
                for (int i = 0; i < func.Vars.Length; ++i)
                {
                    if (func.Vars[i].Item1 == typeof(int))
                    {
                        if (values[i] is int)
                        {
                            varsInfo.Add(func.Vars[i].Item2, values[i]);
                        }
                        else
                        {
                            throw new ApplicationException($"Tried casting double to int in function '{funcName}', " +
                                $"argument #{i + 1}");
                        }
                    }
                    else
                    {
                        varsInfo.Add(func.Vars[i].Item2, (double)values[i]);
                    }
                }
                return func.Root.SolveTree(varsInfo);
            }
            throw new ApplicationException($"Function {funcName} has {func.Vars.Length} arguments, not {values.Length}");
        }
    }
}