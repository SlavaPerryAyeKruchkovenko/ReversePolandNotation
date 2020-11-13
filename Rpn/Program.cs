using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace Rpn
{
	class Program
	{
		static void Main(string[] args)
		{
			var yArray = new List<string>();
			var xArray = new List<double>();
			bool haveError;
			double answer,x1=0,x2=0;
			string function="";
			StreamWriter output = new StreamWriter("C:\\proga\\text.txt");
			Calculate y = new Calculate();
			string text = Environment.CurrentDirectory + "\\Input.txt";
			string[] inputs = File.ReadAllLines(text);
			if(inputs.Length>=2)
			{
				Console.WriteLine("Деапозон значений");
				ParseX(inputs[1], out  x1, out  x2);
				InputCoordinate(x1, x2);
				Console.Clear();

				Console.WriteLine("Функция");
				function = inputs[0];
				Console.WriteLine(function);
				
				haveError = y.IsCorrectFunction(function);
				function = y.DeleteEmptyLetter(function);
			}
			else
			{
				Console.WriteLine("Неправельно задан текстовый файл");
				haveError = false;
			}

			if (haveError)
			for (double i = x1; i <= x2; i++)
			{
				answer=y.ReadNumber(i,function, out haveError);
					if (!haveError)
					{
						yArray.Add(answer.ToString());
						xArray.Add(i);
						Console.WriteLine(answer);
					}
						
			}
			if(!haveError)
			{
				string longestNum = FoundMaximus(yArray);
				output=DrawAllTablet(longestNum, yArray, xArray, output);
				output.Close();
			}			
		}		
		private static void ParseX(string x,out double x1,out double x2)
		{
			string[] input = x.Split(';');
			x1 = Convert.ToDouble(input[0].Trim());
			x2 = Convert.ToDouble(input[1].Trim());
		}
		private static void InputCoordinate(double x1,double x2)
		{
			Console.WriteLine("Начальное значние");
			Console.WriteLine(x1);
			Console.WriteLine("Конечное значние");
			Console.WriteLine(x2);			
			
			CheckCoordinate(x1, x2);
			Thread.Sleep(1500);
		}
		private static bool CheckCoordinate(double x1,double x2)
		{
			if (x1 < x2)
				return true;
			else
			{
				Console.WriteLine("Конечная кордината должна быть больше начальной \nПоробуйте снова");
				return false;
			}
		}

		private static string DrawingTable(string longestNumb)
		{
			string str="";
			str+="|";
			for (int i = 1; i < 6; i++)
			{
				str+=" ";
			}
			str+="|";
			for (int i = 0; i < longestNumb.Length+3; i++)
			{
				str+=" ";
			}
			str+="|";
			return str;
		}
		private static string DrawingClose(string longestNumb)
		{
			string str="";
			str+="+";
			for (int i = 1; i <= 6; i++)
			{
				str+="-";
			}
			for (int i = 0; i < longestNumb.Length+3; i++)
			{
				str+="-";
			}
			str+='+';
			return str;
		}
		private static string DrawTabletWithNumb(string y, double x, string longestText)
		{
			string str="";
			str+="|";
			str+=" ";
			int i = 2;
			str+=x;
			i += x.ToString().Length - 1;
			while (i < 5)
			{
				str+=" ";
				i++;
			}				
			str+="|";
			str+=" ";
			i = 1;
			str+=y;
			i += y.Length - 1;
			while (i < longestText.Length+2)
			{
				str+=" ";
				i++;
			}			
			str+="|";
			return str;
		}
		private static string FoundMaximus(List<string> yArray)
		{
			string longestText="";
			foreach (string num in yArray)
			{
				if (longestText.Length < num.Length)
					longestText = num;
			}
			return longestText;
		}
		private static StreamWriter DrawAllTablet(string longestNum,List<string>yArray,List<double> xArray,StreamWriter output)
		{
			output.WriteLine(DrawingClose(longestNum));
			for (int i = 0; i < xArray.Count; i++)
			{
				output.WriteLine(DrawingTable(longestNum));
				output.WriteLine(DrawTabletWithNumb(yArray[i], xArray[i], longestNum));
			}
			output.WriteLine(DrawingTable(longestNum));
			output.WriteLine(DrawingClose(longestNum));
			return output;
		}
	}
}
