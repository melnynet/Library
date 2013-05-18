using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValueOfFunction
{
    public class Function
    {
        #region Поля  и Свойства класса
        //Содержит строку со стартовой функцией
        private string startFunction;
        //Флаг, который указывает, является ли стартовая функция, результатом(true - является)
        private bool startEndValue;
        //Содержит динамическую строку, которая преобразуется в результат(изначально - это стартовая функция)
        private StringBuilder function;
        //Содержит динамическую строку, которая отвечает за левый операнд текущей операции(lop - left operand)
        private StringBuilder lop;
        //Содержит динамическую строку, которая отвечает за правый операнд текущей операции(rop - right operand)
        private StringBuilder rop;
        //Флаг, который указывает, готов ли левый операнд к операции над ним (true - готов)
        private bool left_flag = false;
        //Флаг, который указывает, готов ли правый операнд к операции над ним (true - готов)
        private bool right_flag = false;
        //Значение результата
        private decimal result = 0;
        //Свойство значения результата(только чтение)
        /// <summary>
        /// Получение результата расчета функции
        /// </summary>
        public decimal Result
        {
            get { return result; }
        }
        //Свойство значения стартовой строки(только чтение)
        /// <summary>
        /// Получение стартовой функции. read-only
        /// </summary>
        public string StartFunction
        {
            get { return startFunction; }
        }
        //Массив всех операторов
        private char[] allOperators = { '(', ')', '*', '/', '+', '-', '^' };

        private decimal currentVar;

        private string[] standartFunctions = {"abs", "arccos", "arcsin", "cos", "sin", "tg", "sqrt", "e", "ln", "lg", "arctg", "ch", "sh", "th" };    //Сделать распознование разного регистра LN = ln = Ln
        #endregion                                                                                                                       //можно заменить будет sqrt на чтото другое , придумать чтото для логарифмов с произвольным основанием


        #region Конструктор
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="function">Строка функции, которую необходимо расчитать</param>
        public Function(string function)
        {
            this.function = new StringBuilder(function);
            this.startFunction = function;
            lop = new StringBuilder();
            rop = new StringBuilder();
            startEndValue = true;
        }
        #endregion

        #region Метод проверки окончания расчета
        /// <summary>
        /// Проверка окончания расчета функции
        /// </summary>
        /// <returns>Возвращает true, если расчет окончен</returns>
        private bool IsEnd()
        {
            int countOfOperators = 0;
            for (int i = 0; i < function.Length; i++)
            {
                foreach (char ch in allOperators)
                {
                    if (function[i].Equals(ch))
                        countOfOperators++;
                    else
                        continue;
                }
            }
            switch (countOfOperators)
            {
                case 0:
                    this.result = Convert.ToDecimal(function.ToString());
                    return true;
                case 1:
                    if ((function[0] == '-') || (function[0] == '{'))
                    {
                        if (function[0] == '{')
                        {
                            function.Remove(function.Length - 1, 1);
                            function.Remove(0, 1);
                        }
                        this.result = Convert.ToDecimal(function.ToString());
                        return true;
                    }
                    else
                        return false;
                case 2:
                    if ((function[0] == '-') && (function[2] == '-'))
                    {
                        function.Remove(0, 3);
                        function.Remove(function.Length - 1, 1);
                        this.result = Convert.ToDecimal(function.ToString());
                        return true;
                    }
                    else
                        return false;                
                default:
                    return false;
            }
        }
        #endregion
        #region Замена стандартных функций
        /// <summary>
        /// Заменяет все стандартные функции на чисельные значения
        /// </summary>
        /// <param name="standartFunctions">Список всех стандартных функций</param>
        private void ReplaceStandartFunction(params string[] standartFunctions)
        {
            int startSquareScobe = 0;
            int endSquareScobe = 0;
            int tempStartScobes = 0;
            foreach (string tempStr in standartFunctions)    //необходимо при найденой функции пересчитывать сначала все функции
            {
                int indexOfFunction = function.ToString().IndexOf(tempStr);
                while ((indexOfFunction != -1))
                {

                    startSquareScobe = indexOfFunction + tempStr.Length;
                    for (int i = startSquareScobe + 1; i < function.Length; i++)
                    {
                        if (function[i].Equals('('))
                            tempStartScobes++;
                        if (function[i].Equals(')') && (tempStartScobes == 0))
                        {
                            endSquareScobe = i;
                            break;
                        }
                        else if (function[i].Equals(')') && (tempStartScobes != 0))
                            tempStartScobes--;
                    }
                    Function tempFunc = new Function(function.ToString(startSquareScobe + 1, endSquareScobe - startSquareScobe - 1));
                    tempFunc.Eval(currentVar);
                    decimal tempArgumentResult = tempFunc.Result;
                    decimal tempResult = 0;
                    switch (tempStr)
                    {
                        case "abs":
                            tempResult = Convert.ToDecimal(Math.Abs(Convert.ToDouble(tempArgumentResult)));
                            break;
                        case "sin":
                            tempResult = Convert.ToDecimal(Math.Sin(Convert.ToDouble(tempArgumentResult)));
                            break;
                        case "cos":
                            tempResult = Convert.ToDecimal(Math.Cos(Convert.ToDouble(tempArgumentResult)));
                            break;
                        case "tg":
                            tempResult = Convert.ToDecimal(Math.Tan(Convert.ToDouble(tempArgumentResult)));
                            break;
                        case "sqrt":
                            tempResult = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(tempArgumentResult)));
                            break;
                        case "e":
                            tempResult = Convert.ToDecimal(Math.Exp(Convert.ToDouble(tempArgumentResult)));
                            break;
                        case "ln":
                            tempResult = Convert.ToDecimal(Math.Log(Convert.ToDouble(tempArgumentResult)));
                            break;
                        case "lg":
                            tempResult = Convert.ToDecimal(Math.Log10(Convert.ToDouble(tempArgumentResult)));
                            break;
                        case "arccos":
                            tempResult = Convert.ToDecimal(Math.Acos(Convert.ToDouble(tempArgumentResult)));
                            break;
                        case "arcsin":
                            tempResult = Convert.ToDecimal(Math.Asin(Convert.ToDouble(tempArgumentResult)));
                            break;
                        case "arctg":
                            tempResult = Convert.ToDecimal(Math.Atan(Convert.ToDouble(tempArgumentResult)));
                            break;
                        case "ch":
                            tempResult = Convert.ToDecimal(Math.Cosh(Convert.ToDouble(tempArgumentResult)));
                            break;
                        case "sh":
                            tempResult = Convert.ToDecimal(Math.Sinh(Convert.ToDouble(tempArgumentResult)));
                            break;
                        case "th":
                            tempResult = Convert.ToDecimal(Math.Tanh(Convert.ToDouble(tempArgumentResult)));
                            break;
                        default:
                            throw new Exception("Неверная функция");
                    }
                    function.Remove(indexOfFunction, tempStr.Length + tempFunc.StartFunction.Length + 2);
                    if (tempResult >= 0)
                    {
                        function.Insert(indexOfFunction, tempResult);
                    }
                    else
                    {
                        function.Insert(indexOfFunction, '}');
                        function.Insert(indexOfFunction, '{');
                        function.Insert(indexOfFunction + 1, tempResult);
                    }
                    indexOfFunction = function.ToString().IndexOf(tempStr);
                }
            }
        }
        #endregion

        private void ReplaceVariable(decimal variable)
        {
            for (int i = 0; i < function.Length; i++)
            {
                int indexOfVar = function.ToString().IndexOf('x');
                if (indexOfVar != -1)
                {
                    if (variable >= 0)
                    {
                        function.Remove(indexOfVar, 1);
                        function.Insert(indexOfVar, variable);
                    }
                    else
                    {
                        if ((indexOfVar == 0) || (function[indexOfVar - 1] == '('))
                        {
                            function.Remove(indexOfVar, 1);
                            function.Insert(indexOfVar, variable);
                        }
                        else
                        {
                            function.Remove(indexOfVar, 1);
                            function.Insert(indexOfVar, '}');
                            function.Insert(indexOfVar, '{');
                            function.Insert(indexOfVar + 1, variable);
                        }
                    }

                }
                else
                    return;
            }

        }
        #region Метод начала расчета
        /// <summary>
        /// Начало расчета
        /// </summary>
        /// <param name="variable">Значение Х.Если аргумента Х нету - укажите 0</param>
        public decimal Eval(decimal variable)
        {
            this.currentVar = variable;
            ReplaceVariable(currentVar);
            ReplaceStandartFunction(standartFunctions);
            while (IsEnd() == false)
            {
                startEndValue = false;
                int temp;
                if (FindSymbol(1,out temp) != -1)             
                {
                    PartialEvaluate(temp);
                }
                else if (FindSymbol(2,out temp) != -1)
                {
                    PartialEvaluate(temp);
                }
                else if (FindSymbol(3,out temp) != -1)
                {
                    PartialEvaluate(temp);
                }
                else if (FindSymbol(4,out temp) != -1)
                {
                    PartialEvaluate(temp);
                }
            }
            if (startEndValue == true)
            {
                this.result = Convert.ToDecimal(function.ToString());  //в начале было вместо function, startFunction..Если будет сбой, обратить на это внимание

            }
            function.Remove(0, function.ToString().Length);
            function.Insert(0, StartFunction);
            this.startEndValue = true;
            return result;
        }
        #endregion

        #region Методы нахождения символов в строке
        /// <summary>
        /// Находит в строке из функцией символ, который первый попадется в массиве операторов
        /// </summary>
        /// <param name="operators">Массив операторов в зависимости от приоритетности</param>
        /// <returns>Возвращает индекс в строке из функцией</returns>
        private int equal(params char[] operators)
        {
            for (int i = 0; i < function.Length; i++)
            {
                foreach (char ch in operators)
                {
                    if ((function[i].Equals(ch)) && ((i != 0) || (ch == '(')))
                        if ((ch == '-') && (function[i - 1].Equals('{')))
                            continue;
                        else
                            return i;
                }
                continue;
            }
            
            return -1;

        }
        /// <summary>
        /// Ищет символ в строке функции в зависимости от номера приоритета
        /// </summary>
        /// <param name="prioritet">Номер приоритета</param>
        /// <returns>Возвращает номер символа в строке функции</returns>
        private int FindSymbol(int prioritet, out int temp)
        {
            int tempEqual = -1;
            switch (prioritet)
            {
                case 1:
                    tempEqual = equal('(');
                    break;
                case 2:
                    tempEqual = equal('^');
                    break;
                case 3:
                    tempEqual = equal('*','/');
                    break;
                case 4:
                    tempEqual = equal('+','-');
                    break;
                default:
                    break;
            }        
                    if (tempEqual != -1)
                    {
                        temp = tempEqual;
                        return temp;
                    }
                    else
                    {
                        temp = -1;
                        return temp;
                    }

        }
        #endregion

        #region Метод расчета частичных функций
        /// <summary>
        /// Расчет частичных функций в зависимости от приоритета операции
        /// </summary>
        /// <param name="indexOfOperator">Индекс оператора в строке</param>
        private void PartialEvaluate(int indexOfOperator)
        {
            int temp = indexOfOperator;
            char operatorSymbol = function[indexOfOperator];
            bool wereLeftScobes = false;
            bool wereRightScobes = false;
            if (operatorSymbol == '(')
            {
                int startPosScobe = temp;
                int endPosScobe = 0;
                int tempBackScobe = 0;
                for (int i = startPosScobe + 1; i < function.Length; i++)
                {
                    if (function[i].Equals('('))
                        tempBackScobe++;
                    if (function[i].Equals(')') && (tempBackScobe == 0))
                    {
                        endPosScobe = i;
                        break;
                    }
                    else if (function[i].Equals(')') && (tempBackScobe != 0))
                        tempBackScobe--;
                }
                Function tempFunc = new Function(function.ToString(startPosScobe + 1, endPosScobe - startPosScobe - 1));
                tempFunc.Eval(currentVar);
                decimal tempResult = tempFunc.Result;
                function.Insert(startPosScobe, tempResult);
                if (tempResult < 0)
                {
                    function.Insert(startPosScobe, '{');
                    function.Insert(startPosScobe + tempResult.ToString().Length + 1, '}');
                    function.Remove(startPosScobe + Convert.ToString(tempResult).Length + 2, endPosScobe - startPosScobe + 1);
                }
                else
                    function.Remove(startPosScobe + Convert.ToString(tempResult).Length, endPosScobe - startPosScobe + 1);
            }
            else
            {
                left_flag = right_flag = false;
                lop.Clear();
                rop.Clear();
                bool inScobe = false;
                while (left_flag == false)
                {
                    if ((temp - 1 < 0) || (function[temp - 1] == '*') || (function[temp - 1] == '/') || (function[temp - 1] == '+') || (function[temp - 1] == '+') || ((function[temp - 1] == '-') && (temp - 1 != 0) && (inScobe == false)))   //нужно все знаки проверить
                    {
                        left_flag = true;
                    }
                    else if ((function[temp - 1] != '}') && (function[temp - 1] != '{'))
                    {
                        lop.Insert(0, function[temp - 1]);
                        temp--;
                    }
                    else
                    {
                        if (function[temp - 1] == '{')
                        {
                            left_flag = true;
                            inScobe = false;
                            continue;
                        }
                        inScobe = true;
                        wereLeftScobes = true;
                        temp--;
                    }
                }
                temp = indexOfOperator;
                while (right_flag == false)
                {
                    if ((temp + 1 > function.Length - 1) || (function[temp + 1] == '*') || (function[temp + 1] == '/') || (function[temp + 1] == '+') || (function[temp + 1] == '^') || ((function[temp + 1] == '-') && (inScobe == false)))
                    {
                        right_flag = true;
                    }
                    else if ((function[temp + 1] != '{') && (function[temp + 1] != '}'))
                    {
                        rop.Append(function[temp + 1]);
                        temp++;
                    }
                    else
                    {
                        if (function[temp + 1] == '}')
                        {
                            right_flag = true;
                            inScobe = false;
                            continue;
                        }
                        inScobe = true;
                        wereRightScobes = true;
                        temp++;
                    }
                }
                switch (operatorSymbol)
                {
                    case '*':
                        result = Convert.ToDecimal(lop.ToString()) * Convert.ToDecimal(rop.ToString());
                        break;
                    case '/':
                        result = Convert.ToDecimal(lop.ToString()) / Convert.ToDecimal(rop.ToString());
                        break;
                    case '+':
                        result = Convert.ToDecimal(lop.ToString()) + Convert.ToDecimal(rop.ToString());
                        break;
                    case '-':
                        result = Convert.ToDecimal(lop.ToString()) - Convert.ToDecimal(rop.ToString());
                        break;
                    case '^':
                        result = Convert.ToDecimal(Math.Pow(Convert.ToDouble(lop.ToString()), Convert.ToDouble(rop.ToString())));
                        break;
                    default:
                        throw new Exception("Неверная операция");
                }
                bool min_res = false;
                if (result < 0)
                    min_res = true;
                if (!wereLeftScobes)
                {
                    if (!wereRightScobes)
                        function.Remove(indexOfOperator - lop.Length, lop.Length + rop.Length + 1);
                    else
                        function.Remove(indexOfOperator - lop.Length, lop.Length + rop.Length + 3);
                    if (!min_res)
                    {
                        function.Insert(indexOfOperator - lop.Length, result);
                    }
                    else
                    {
                        function.Insert(indexOfOperator - lop.Length, '}');
                        function.Insert(indexOfOperator - lop.Length, '{');
                        function.Insert(indexOfOperator - lop.Length + 1, result);
                    }
                }
                else
                {
                    if (!wereRightScobes)
                        function.Remove(indexOfOperator - lop.Length - 2, lop.Length + rop.Length + 3);
                    else
                        function.Remove(indexOfOperator - lop.Length - 2, lop.Length + rop.Length + 5);
                    if (!min_res)
                    {
                        function.Insert(indexOfOperator - lop.Length - 2, result);
                    }
                    else
                    {
                        function.Insert(indexOfOperator - lop.Length - 2, '}');
                        function.Insert(indexOfOperator - lop.Length - 2, '{');
                        function.Insert(indexOfOperator - lop.Length - 1, result);
                    }
                }
            }
        }
        #endregion
    }
}

