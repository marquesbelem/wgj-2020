using System;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;

namespace GalloUtils {
    public class FloatOperator {

        public static FloatOperator Constant(float value) {
            return new FloatOperator() {
                type = Type.Constant,
                constantValue = value
            };
        }
        public static FloatOperator Term(Func<float> func) {
            return new FloatOperator() {
                type = Type.Variable,
                func = func
            };
        }
        public static FloatOperator Adition(List<FloatOperator> values) {
            return new FloatOperator() {
                type = Type.Adition,
                children = new List<FloatOperator>(values)
            };
        }
        public static FloatOperator Subtraction(List<FloatOperator> values) {
            return new FloatOperator() {
                type = Type.Subtraction,
                children = new List<FloatOperator>(values)
            };
        }
        public static FloatOperator Multiplication(List<FloatOperator> values) {
            return new FloatOperator() {
                type = Type.Multiplication,
                children = new List<FloatOperator>(values)
            };
        }
        public static FloatOperator Division(List<FloatOperator> values) {
            return new FloatOperator() {
                type = Type.Division,
                children = new List<FloatOperator>(values)
            };
        }
        public static FloatOperator Remainder(List<FloatOperator> values) {
            return new FloatOperator() {
                type = Type.Remainder,
                children = new List<FloatOperator>(values)
            };
        }
        public static FloatOperator Power(List<FloatOperator> values) {
            return new FloatOperator() {
                type = Type.Power,
                children = new List<FloatOperator>(values)
            };
        }
        public static FloatOperator Round(FloatOperator value) {
            return new FloatOperator() {
                type = Type.Round,
                children = new List<FloatOperator>() { value }
            };
        }
        public static FloatOperator Floor(FloatOperator value) {
            return new FloatOperator() {
                type = Type.Floor,
                children = new List<FloatOperator>() { value }
            };
        }
        public static FloatOperator Ceil(FloatOperator value) {
            return new FloatOperator() {
                type = Type.Ceil,
                children = new List<FloatOperator>() { value }
            };
        }
        public static FloatOperator Min(List<FloatOperator> values) {
            return new FloatOperator() {
                type = Type.Min,
                children = new List<FloatOperator>(values)
            };
        }
        public static FloatOperator Max(List<FloatOperator> values) {
            return new FloatOperator() {
                type = Type.Max,
                children = new List<FloatOperator>(values)
            };
        }
        public static FloatOperator Clamp(FloatOperator min, FloatOperator max, FloatOperator value) {
            return new FloatOperator() {
                type = Type.Clamp,
                children = new List<FloatOperator>() { min, max, value }
            };
        }

        public static FloatOperator FromFormula(string formula, Dictionary<string, Func<float>> context = null) {
            ParseResult parse = ParseFormula(formula, context);
            if (parse == null) {
                return null;
            }
            return FromParse(parse, context);
        }

        public float Value {
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
            Power,// a^b
            Round,// round(a)
            Floor,// floor(a)
            Ceil,// ceil(a)
            Min,// min(a,b)
            Max,// max(a,b)
            Clamp,// clamp(a,min,max)
        }

        private FloatOperator() { }
        private Type type = Type.Constant;
        private List<FloatOperator> children = new List<FloatOperator>();
        private float constantValue = 0;
        private Func<float> func = null;

        private static readonly Dictionary<char, Type> operators = new Dictionary<char, Type>() {
            ['+'] = Type.Adition,
            ['-'] = Type.Subtraction,
            ['*'] = Type.Multiplication,
            ['/'] = Type.Division,
            ['%'] = Type.Remainder,
            ['^'] = Type.Power
        };
        private static readonly Dictionary<Type, int> operatorPrecedences = new Dictionary<Type, int>() {
            [Type.Adition] = 1,
            [Type.Subtraction] = 1,
            [Type.Multiplication] = 2,
            [Type.Division] = 2,
            [Type.Remainder] = 2,
            [Type.Power] = 3
        };
        private static bool IsOperator(char c) {
            return operators.ContainsKey(c);
        }
        private static readonly Dictionary<string, Type> functionTypes = new Dictionary<string, Type>() {
            ["round"] = Type.Round,
            ["floor"] = Type.Floor,
            ["ceil"] = Type.Ceil,
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
            public float constantValue = 0;
            public List<string> childrenFormula = new List<string>();
            public Func<float> func = null;

            public ParseResult(float constantValue) {
                this.constantValue = constantValue;
            }
            public ParseResult() { }
        }
        private FloatOperator(ParseResult parse) {
            type = parse.type;
            constantValue = parse.constantValue;
            func = parse.func;
        }
        private static ParseResult ParseFormula(string formula, Dictionary<string, Func<float>> context) {
            ParseResult result = new ParseResult();

            //Clean Text
            formula = formula.Cleaned(ignoredChars);
            formula = formula.RemoveDirectlyNestedParenthesis();
            formula = formula.RemoveBorderParenthesis();
            formula = formula.RemoveFirstIf(c => IsOperator(c) && c != '-');
            formula = formula.RemoveLastIf(c => IsOperator(c));

            //Check for empty
            if (string.IsNullOrEmpty(formula)) {
                return new ParseResult(0f);
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
            List<FloatOperator> children = new List<FloatOperator>();
            float number;

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
            else if (float.TryParse(formula, NumberStyles.Any, CultureInfo.InvariantCulture, out number)) {//It's a constant
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
        public static bool IsParseable(string formula, Dictionary<string, Func<float>> context) {
            return ParseFormula(formula, context) != null;
        }
        private static FloatOperator FromParse(ParseResult parse, Dictionary<string, Func<float>> context) {
            List<ParseResult> childrenParse = parse.childrenFormula.ConvertAll(t => ParseFormula(t, context));
            if (childrenParse.Contains(null)) {
                return null;
            }
            FloatOperator result = new FloatOperator(parse);
            for (int i = 0; i < childrenParse.Count; i++) {
                result.children.Add(FromParse(childrenParse[i], context));
            }
            return result;
        }

        private float CalculateValue() {
            float result = 0f;
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
                        result = children.StartingAt(1).Reduce((e, a) => a - e.Value, children.First().Value);
                    }
                    break;
                case Type.Multiplication:
                    if (children.Count > 0) {
                        result = children.Reduce((e, a) => a * e.Value, 1f);
                    }
                    break;
                case Type.Division:
                    if (children.Count > 0) {
                        result = children.StartingAt(1).Reduce((e, a) => a / e.Value, children.First().Value);
                    }
                    break;
                case Type.Remainder:
                    if (children.Count > 0) {
                        result = children.StartingAt(1).Reduce((e, a) => a % e.Value, children.First().Value);
                    }
                    break;
                case Type.Power:
                    if (children.Count > 0) {
                        result = children.StartingAt(1).Reduce((e, a) => Mathf.Pow(a, e.Value), children.First().Value);
                    }
                    break;
                case Type.Round:
                    if (children.Count > 0) {
                        return Mathf.Round(children[0].Value);
                    }
                    break;
                case Type.Floor:
                    if (children.Count > 0) {
                        return Mathf.Floor(children[0].Value);
                    }
                    break;
                case Type.Ceil:
                    if (children.Count > 0) {
                        return Mathf.Ceil(children[0].Value);
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
