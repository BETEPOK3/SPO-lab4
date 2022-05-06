using System;
using System.Collections.Generic;
using System.Text;

namespace parser_helper
{
    public class Func
    {
        public INode Root { get; private set; }

        public (Type, string)[] Vars { get; private set; }

        public Func(INode root, List<(Type, string)> vars)
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
        /// Фукнция возведения в степень для целых чисел
        /// </summary>
        /// <param name="num">Основание</param>
        /// <param name="exp">Степень</param>
        /// <returns>Результат возведения в степень</returns>
        public static dynamic IntPow(int num, int exp)
        {
            if (exp >= 0)
            {
                int result = 1;
                for (; exp > 0; --exp)
                {
                    result *= num;
                }
                return result;
            }
            else
            {
                return Math.Pow(num, exp);
            }
        }

        /// <summary>
        /// Конвертирование строкового представления числа в NodeInt или NodeDouble (зависит от типа числа)
        /// </summary>
        /// <param name="strNum">Строчное представление числа</param>
        /// <returns>Узел</returns>
        public static dynamic ConvertToNode(string strNum)
        {
            try
            {
                return new NodeInt(Convert.ToInt32(strNum));
            }
            catch
            {
                return new NodeDouble(Convert.ToDouble(strNum.Replace('.', ',')));
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
            else if (typeStr.Equals("int"))
            {
                return typeof(int);
            }
            else
            {
                throw new ApplicationException($"Unknown type '{typeStr}'");
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
            throw new ApplicationException($"Unknown var '{id}'.");
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
            throw new ApplicationException($"Unknown function '{id}'.");
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
                return $"Changed var '{id}' to {num}";
            }
            return $"Added new var '{id}' = {num}";
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
                                $"argument #{i + 1}.");
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