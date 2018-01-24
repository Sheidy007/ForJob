using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FigureArea
{
    public static class Figure
    {
        static int _iParsePos = 0;
        public static double GetArea(string Params, string Name = "Null")
        {
            List<string> _LFigureParams = new List<string>();
            string _sParam = "";
            //Считываем все параметры с введенной строки
            for (int i = 0; i < Params.Count(); i++)
            {
                if (Params[i] != ',') _sParam += Params[i];
                else
                {
                    _LFigureParams.Add(Convert.ToString(_sParam));
                    _sParam = "";
                }

                if (i == Params.Count() - 1)
                {
                    _LFigureParams.Add(Convert.ToString(_sParam));
                    _sParam = "";
                }
            }

            string _sThisString;
            //Подключаем файл формул
            System.IO.StreamReader _sisFile = new System.IO.StreamReader(@"Expression.conf");

            string _sExpression = "";
            //Ищем подходящие формулы и заменяем переменные типа [i] на значения
            if (Name != "Null") //Если фигура определена
            {
                string FigureName = "";

                while ((_sThisString = _sisFile.ReadLine()) != null)
                {
                    for (int i = 0; i < _sThisString.Count(); i++)
                    {
                        if (_sThisString[i] == '=')
                        {
                            i++;
                            if (FigureName == Name)
                            {
                                for (int j = i; j < _sThisString.Count(); j++)
                                    _sExpression += _sThisString[j];
                                for (int j = 0; j < _LFigureParams.Count(); j++)
                                    _sExpression = _sExpression.Replace("[" + (j + 1) + "]", _LFigureParams[j]);
                                break;
                            }
                            FigureName = "";
                        }
                        else FigureName += _sThisString[i];
                    }
                    FigureName = "";
                }
            }
            else //Если фигура не определена
            {
                while ((_sThisString = _sisFile.ReadLine()) != null)
                {
                    for (int i = 0; i < _sThisString.Count(); i++)
                    {
                        if (_sThisString[i] != '=') _sExpression += _sThisString[i];
                        else _sExpression = "";
                    }
                    bool contain = false;
                    for (int i = 0; i < _LFigureParams.Count(); i++)
                    {
                        if (_sExpression.Contains("[" + (i + 1) + "]"))
                        {
                            _sExpression = _sExpression.Replace("[" + (i + 1) + "]", _LFigureParams[i]);
                            contain = true;
                        }
                        else contain = false;
                        if (i == _LFigureParams.Count() - 1 && _sExpression.Contains("[" + (i + 2) + "]"))
                            contain = false;

                    }
                    if (contain) break; else _sExpression = "";
                }
            }
            //Обнулим если формула не найдена
            if (_sExpression == "") _sExpression = "0";
            else//Заменим Pi
            {
                _sExpression = _sExpression.Replace("Pi", "" + Math.PI);
                _sExpression = _sExpression.Replace("pi", "" + Math.PI);
            }
            _sisFile.Close();
            _iParsePos = 0;
            double result;
            //Для предотвращения ошибок формул
            try
            {
                result = Evaluate(EvaluateExpression(_sExpression));
            }
            catch
            {
                result = 0;
            }
            return result;
        }

        private static string EvaluateExpression(string Expression) //Парсим формулу
        {
            string _sResult = "";
            string _sExp = "";
            while (_iParsePos < Expression.Count())
            {
                if (Expression[_iParsePos] == '(')
                {
                    if (_iParsePos > 3)
                        _sExp = Expression[_iParsePos - 4] + "" + Expression[_iParsePos - 3] + "" + Expression[_iParsePos - 2] + "" + Expression[_iParsePos - 1];

                    switch (_sExp.ToLower())
                    {
                        case "sqrt":
                            {
                                _iParsePos++;
                                _sResult += "" + Math.Sqrt(Convert.ToDouble(Evaluate(EvaluateExpression(Expression))));
                            }
                            break;
                        default:
                            {
                                _iParsePos++;
                                _sResult += "" + Evaluate(EvaluateExpression(Expression));
                            }
                            break;
                    }
                }
                else if (Expression[_iParsePos] == ')')
                {
                    return _sResult;
                }
                else if (!Char.IsLetter(Expression[_iParsePos]))
                    _sResult += Expression[_iParsePos] + "";
                _iParsePos++;
            }
            return _sResult;
        }
        private static double Evaluate(string Expression)//Парсим знаки
        {
            DataTable _dTable = new DataTable();
            _dTable.Columns.Add("Expression", typeof(string), Expression.Replace(',', '.'));
            DataRow _dRow = _dTable.NewRow();
            _dTable.Rows.Add(_dRow);
            return double.Parse((string)_dRow["Expression"]);
        }
    }
}
