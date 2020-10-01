using System;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;

namespace GalloUtils {
    public class IntOperator {

        public static IntOperator Constant(int value) {
            return new IntOperator() {
                type = Type.Constant,
                constantValue = value
            };
        }
        public static IntOperator Term(Func<int> func) {
            return new IntOperator() {
                type = Type.Variable,
                func = func
            };
        }
        public static IntOperator Adition(List<IntOperator> values) {
            return new IntOperator() {
                type = Type.Adition,
                children = new List<IntOperator>(values)
            };
        }
        public static IntOperator Subtraction(List<IntOperator> values) {
            return new IntOperator() {
                type = Type.Subtraction,
                children = new List<IntOperator>(values)
            };
        }
        public static IntOperator Multiplication(List<IntOperator> values) {
            return new IntOperator() {
                type = Type.Multiplication,
                children = new List<IntOperator>(values)
            };
        }
        public static IntOperator Division(List<IntOperator> values) {
            return new IntOperator() {
                type = Type.Division,
                children = new List<IntOperator>(values)
            };
        }
        public static IntOperator Remainder(List<IntOperator> values) {
            return new IntOperator() {
                type = Type.Remainder,
                children = new List<IntOperator>(values)
            };
        }
        public static IntOperator Min(List<IntOperator> values) {
            return new IntOperator() {
                type = Type.Min,
                children = new List<IntOperator>(values)
            };
        }
        public static IntOperator Max(List<IntOperator> values) {
            return new IntOperator() {
                type = Type.Max,
                children = new List<IntOperator>(values)
            };
        }
        public static IntOperator Clamp(IntOperator min, IntOperator max, IntOperator value) {
            return new IntOperator() {
                type = Type.Clamp,
                children = new List<IntOperator>() { min, max, value }
            };
        }

        public static IntOperator FromFormula(string formula, Dictionary<string, Func<int>> context = null) {
            ParseResult parse = ParseFormula(formula, context);
            if (parse == null) {
                return null;
            }
            return FromParse(parse, context);
        }

        public int Value {
            get {
                return CalculateValue();
            }
        }
        public enum Type {
            Constant,// 1
            Variable,// a
            Adition,// a+b
            Subtraction, // a-b
            Multiplication, // a*b
            Division,// a/b
            Remainder,// a%b
            Min,// min(a,b)
            Max,// max(a,b)
            Clamp,// clamp(a,min,max)
        }

        private IntOperator() { }
        private Type type = Type.Constant;
        private List<IntOperator> children = new List<IntOperator>();
        private int constantValue = 0;
        private Func<int> func = null;

        private static readonly Dictionary<char, Type> operators = new Dictionary<char, Type>() {
            ['+'] = Type.Adition,
            ['-'] = Type.Subtraction,
            ['*'] = Type.Multiplication,
            ['/'] = Type.Division,
            ['%'] = Type.Remainder
        };
        private static readonly Dictionary<Type, int> operatorPrecedences = new Dictionary<Type, int>() {
            [Type.Adition] = 1,
            [Type.Subtraction] = 1,
            [Type.Multiplication] = 2,
            [Type.Division] = 2,
            [Type.Remainder] = 2
        };
        private static bool IsOperator(char c) {
            return operators.ContainsKey(c);
        }
        private static readonly Dictionary<string, Type> functionTypes = new Dictionary<string, Type>() {
            ["min"] = Type.Min,
            ["max"] = Type.Max,
            ["clamp"] = Type.Clamp
        };
        private static bool IsFunction(string s) {
            return functionTypes.ContainsKey(s);
        }
        private static readonly List<char> ignoredChars = new List<char>() {
            ' ',
            '\t',
            '\n'
        };

        private class ParseResult {
            public Type type = Type.Constant;
            public int constantValue = 0;
            public List<string> childrenFormula = new List<string>();
            public Func<int> func = null;

            public ParseResult(int constantValue) {
                this.constantValue = constantValue;
            }
            public ParseResult() { }
        }
        private IntOperator(ParseResult parse) {
            type = parse.type;
            constantValue = parse.constantValue;
        }
        private static ParseResult ParseFormula(string formula, Dictionary<string, Func<int>> context) {
            ParseResult result = new ParseResult();

            //Clean Text
            formula = formula.Cleaned(ignoredChars);
            formula = formula.RemoveDirectlyNestedParenthesis();
            formula = formula.RemoveBorderParenthesis();
            formula = formula.RemoveFirstIf(c => IsOperator(c) && c != '-');
            formula = formula.RemoveLastIf(c => IsOperator(c));

            //Check for empty
            if (string.IsNullOrEmpty(formula)) {
                return new ParseResult(0);
            }

            //Check for errors
            int openParenthesisCount = formula.CountAll('(');
            int closeParenthesisCount = formula.CountAll(')');
            if (openParenthesisCount != closeParenthesisCount) {
                return null;
            }

            //Declare auxiliar variables
            List<int> operatorIndices = new List<int>();
            List<string> childrenText = new List<string>();
            List<Type> operatorTypes = new List<Type>();
            List<IntOperator> children = new List<IntOperator>();
            int number;

            operatorIndices = formula.FindAllIndex(c => IsOperator(c));//Find operators
            operatorIndices.RemoveAll(o => formula.ParenthesisDepthAt(o) != 0);//Remove inner operators
            operatorIndices.RemoveAt(operatorIndices.FindAllIndex(i => operatorIndices.Exists(j => i + 1 == j)));//Remove neighbor operators

            if (operatorIndices.Count > 0) {//It is an operator
                operatorTypes = formula.SelectChars(operatorIndices).ConvertAll(c => operators[c]);
                int leastPrecedence = operatorTypes.Min(o => operatorPrecedences[o]);
                int chosenIndex = operatorTypes.FindAllIndex(o => operatorPrecedences[o] == leastPrecedence).Last();
                result.type = operatorTypes[chosenIndex];
                List<int> chosenIndices = new List<int>() { chosenIndex };
                for (int i = chosenIndex - 1; i >= 0; i--) {
                    if (operatorTypes[i] != operatorTypes[chosenIndex]) {
                        break;
                    }
                    chosenIndices.Insert(0, i);
                }
                result.childrenFormula = formula.Split(operatorIndices.SortedBy(chosenIndices));
            }
            else if (formula.Last() == ')') {
                int openParenthesisIndex = formula.FindIndex('(');
                string name = formula.Substring(0, openParenthesisIndex).ToLowerInvariant();
                string innerText = formula.Substring(openParenthesisIndex + 1, formula.Length - (openParenthesisIndex + 2));
                if (IsFunction(name)) {//It's a function
                    result.type = functionTypes[name];
                    List<int> comaIndices = innerText.FindAllIndex(c => c == ',');
                    comaIndices.RemoveAll(o => innerText.ParenthesisDepthAt(o) != 0);
                    result.childrenFormula = new List<string>(innerText.Split(comaIndices, StringSplitOptions.RemoveEmptyEntries));
                }
                else {
                    return null;
                }
            }
            else if (int.TryParse(formula, NumberStyles.Any, CultureInfo.InvariantCulture, out number)) {//It's a constant
                result.type = Type.Constant;
                result.constantValue = number;
            }
            else if (context != null && context.ContainsKey(formula)) {//It's a variable
                result.type = Type.Variable;
                result.func = context[formula];
            }
            else {
                return null;
            }

            return result;
        }
        public static bool IsParseable(string formula, Dictionary<string, Func<int>> context) {
            return ParseFormula(formula,context) != null;
        }
        private static IntOperator FromParse(ParseResult parse, Dictionary<string, Func<int>> context) {
            List<ParseResult> childrenParse = parse.childrenFormula.ConvertAll(t => ParseFormula(t, context));
            if (childrenParse.Contains(null)) {
                return null;
            }
            IntOperator result = new IntOperator(parse);
            for (int i = 0; i < childrenParse.Count; i++) {
                result.children.Add(FromParse(childrenParse[i], context));
            }
            return result;
        }

        private int CalculateValue() {
            int result = 0;
            switch (type) {
                case Type.Constant:
                    result = constantValue;
                    break;
                case Type.Variable:
                    result = func.Invoke();
                    break;
                case Type.Adition:
                    if (children.Count > 0) {
                        result = children.Sum(c => c.Value);
                    }
                    break;
                case Type.Subtraction:
                    if (children.Count > 0) {
                        result = children.Reduce<IntOperator, int>((e, a) => a - e.Value);
                    }
                    break;
                case Type.Multiplication:
                    if (children.Count > 0) {
                        result = children.Reduce<IntOperator, int>((e, a) => a * e.Value);
                    }
                    break;
                case Type.Division:
                    if (children.Count > 0) {
                        result = children.Reduce<IntOperator, int>((e, a) => a / e.Value);
                    }
                    break;
                case Type.Remainder:
                    if (children.Count > 0) {
                        result = children.Reduce<IntOperator, int>((e, a) => a % e.Value);
                    }
                    break;
                case Type.Min:
                    if (children.Count > 0) {
                        return children.Min(c => c.Value);
                    }
                    break;
                case Type.Max:
                    if (children.Count > 0) {
                        return children.Max(c => c.Value);
                    }
                    break;
                case Type.Clamp:
                    if (children.Count > 2) {
                        return Mathf.Clamp(children[0].Value, children[1].Value, children[2].Value);
                    }
                    break;
            }
            return result;
        }

    }

}
