using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.VisualBasic.CompilerServices;
using System.Reflection.Metadata;

namespace Rpn
{
	public class Calculate
	{
		public double ReadNumber(double x,string input,out bool haveError)
		{
			double result;
			result = 0;			

			var numArray = new List<double>();
			var letterArray = new List<char>();
			var functionArray = new List<string>();
			var numOfFunction = new List<int>();			

			bool haveNotPriority;
			haveError = false;
			int closeLetter, openLetter, startLength = input.Length - 1;

			if (input.Contains("x")) 
			input = SearchX(input, x);

			do
			{				
				SearchPriority(input, out closeLetter, out openLetter, out  haveError,out haveNotPriority);
				if (!haveError)
				{
					string priorityText = input[openLetter..(closeLetter + 1)];
					numArray = NumbParser(priorityText, input, out priorityText, out numOfFunction, out input);
					letterArray = LetterParser(priorityText);
					functionArray = FunctionParser(priorityText);
					numArray = CountMath(functionArray, numArray, numOfFunction,priorityText,out priorityText);
					if (numArray.Count > 1)
						result = CountAnswer(letterArray, numArray);
					if (numArray.Count == 1)
						result = numArray[0];

					input = ReplaceFunction(input, priorityText.Trim(),result.ToString());
				}				
			} while ((openLetter!=-1&&closeLetter!=-1)&& !haveNotPriority);
			return result;
		}
		public string DeleteEmptyLetter(string text)
		{
			while(text.Contains(" "))
			{
				text = text.Replace(" ", "");
			}
			return text;
		}
		public bool IsCorrectFunction(string text)
		{
			bool IsCorrect = IsCorrectText(text);
			if(IsCorrect)
			{
				for (int i = 0; i < text.Length; i++)
				{
					if(i<text.Length-2)
					if (text[i..(i + 3)]=="sin"|| text[i..(i + 3)] == "cos" || text[i..(i + 3)] == "ctg")
						i += 3;
					else if (text[i..(i + 2)] == "tg")
						i += 2;
					if (!IsLetter(text[i]) && !Int32.TryParse(text[i].ToString(), out _)) 
					{						
						IsCorrect = false;
						//MakeError(text[i],i);
						break;
					}
						
				}
			}
			return IsCorrect;
		}
		private static bool IsLetter(char Letter)
		{
			switch(Letter)
			{
				case '+': return true;
				case '-': return true;
				case '*': return true;
				case '/': return true;
				case ',': return true;
				case 'x': return true;
				case '(': return true;
				case ')': return true;
				case ' ': return true;
				default: return false;
			}
		}
		private static void MakeError(char letter,int i)
		{
			Console.SetCursorPosition(i, 2);
			Console.WriteLine("^");
			if (letter == '/')
				Console.WriteLine("Деление пишеться в другую сторону");
			else if (letter == '.')
				Console.WriteLine("Нужно использовать запятую");
			else if(!Int32.TryParse(letter.ToString(),out _))
				Console.WriteLine("Неопознанная функция");
			
		}
		private static string SearchX(string text,double x)
		{
			text=text.Replace("x",x.ToString());
			return text;
		}
		private static string ReplaceFunction(string text,string shortText,string num)
		{
			text = text.Replace(shortText, num);
			return text;
		}
		private static List<double> CountMath(List<string>functionArray,List<double>numArray,List<int>numOfFuncthion,string text,out string text1)
		{
			text1 = text;
			double function=0;
			if (functionArray.Count > 0) 
			{
				for (int i = 0; i < functionArray.Count; i++)
				{
					switch (functionArray[i])
					{						
						case "sin":
							function= Math.Sin(Math.PI * numArray[numOfFuncthion[i]]/180);
							text1 = ReplaceFunction(text, $"sin{numArray[numOfFuncthion[i]]}", function.ToString());
							break;
						case "cos": function = Math.Cos(Math.PI * numArray[numOfFuncthion[i]/180]);
							text1 = ReplaceFunction(text, $"cos{numArray[numOfFuncthion[i]]}", function.ToString());
							break;
						case "tg": function = Math.Tan(Math.PI * numArray[numOfFuncthion[i]]/180);
							text1 = ReplaceFunction(text, $"tg{numArray[numOfFuncthion[i]]}", function.ToString());
							break;
						case "ctg": function = 1/Math.Tan(Math.PI * numArray[numOfFuncthion[i]]/180);
							text1 = ReplaceFunction(text, $"ctg{numArray[numOfFuncthion[i]]}", function.ToString());
							break;
					}
					numArray[numOfFuncthion[i]] = function;				
				}
			}
			return numArray;
		}
		private static void SearchPriority(string text,out int closeZnak,out int openZnak,out bool haveError,out bool haveNotPriority)                                                  
		{
			closeZnak = -1;
			openZnak = -1;
			haveNotPriority = true;
			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] == '(')  
					openZnak = i;
				if (text[i] == ')')
				{
					closeZnak = i;
					break;
				}
			}

			if (closeZnak != -1 && openZnak == -1)
			{
				haveError = false;
				//ReadError("закрывающая", closeZnak, out haveError, out openZnak, out closeZnak);
			}
			else if (closeZnak == -1 && openZnak != -1)
			{
				haveError = false;
				//ReadError("открывающая", openZnak, out haveError, out openZnak, out closeZnak);
			}
			else if (closeZnak == -1 && openZnak == -1)
			{
				haveError = false;
				openZnak = 0;
				closeZnak = text.Length - 1;
			}				
			else
			{
				haveError = false;
				haveNotPriority = false;
			}
				
		}
		/*private static void ReadError(string error,int cursor,out bool haveError,out int openZnak,out int closeZnak)
		{
			Console.SetCursorPosition(cursor, 2);
			Console.WriteLine("^");
			Console.WriteLine(@"Ошибка лишняя "+error+@" скобка");
			haveError = true;
			closeZnak = -1;
			openZnak = -1;
		}*/
		private static double CountAnswer(List<char> mathLetter,List<double> numArray)
		{
			double num=0;
			for (int i = 0; i < mathLetter.Count; i++)
			{
				if (mathLetter[i] == '^')
				{
					num = Math.Pow(numArray[i],numArray[i + 1]);
					numArray.Remove(numArray[i]);
					numArray.Remove(numArray[i]);
					mathLetter.Remove('^');
					numArray.Insert(i, num);
					i--;
				}
			}
			for (int i = 0; i < mathLetter.Count; i++)
			{
				if (mathLetter[i] == '*')
				{
					num = numArray[i] * numArray[i + 1];
					numArray.Remove(numArray[i]);
					numArray.Remove(numArray[i]);
					mathLetter.Remove('*');
					numArray.Insert(i, num);
					i--;
				}					
				else if (mathLetter[i] == '/')
				{
					num = numArray[i] / numArray[i + 1];
					numArray.Remove(numArray[i]);
					numArray.Remove(numArray[i]);
					mathLetter.Remove('/');
					numArray.Insert(i, num);
					i--;
				}				
			}
			for (int i = 0; i < mathLetter.Count; i++)
			{
				if (mathLetter[i] == '-')
				{
					num = numArray[i] - numArray[i + 1];
					numArray.Remove(numArray[i]);
					numArray.Remove(numArray[i]);
					mathLetter.Remove('-');
					numArray.Insert(i, num);
					i--;
				}
				else if (mathLetter[i] == '+')
				{
					num = numArray[i] + numArray[i + 1];
					numArray.Remove(numArray[i]);
					numArray.Remove(numArray[i]);
					mathLetter.Remove('+');
					numArray.Insert(i, num);
					i--;
				}
			}
			return num;			
		}
		private static List<double> NumbParser(string text,string input,out string newtext, out List<int> numOfFunction,out string newInput)
		{
			text += " ";
			string num="";
			numOfFunction = new List<int>();
			var numbArray = new List<double>();
			if (text[0] == '-')
			{
				num = "-";
				input = ReplaceFunction(input, text.Trim(), text.Substring(1));
				text = text.Substring(1);
			}
			if (text[1] == '-'&& text[0] == '(')
			{
				num = "-";
				input = ReplaceFunction(input, text.Trim(), "(" + text.Substring(2));
				text = "("+text.Substring(2);
			}
			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] == '+' || text[i] == '-' || text[i] == '*' || text[i] == '/') 
				{
					while(text[i] == ' ')
						i++;
					if (text[i] == '-')
					{
						num = "-";
						text = text[0..i]+text[(i+1)..text.Length];
					}										
				}
				while (double.TryParse(text[i].ToString(), out _)||text[i]==',')
				{
					
					num += text[i];
					i++;
				}
				if(num!=""&&num!="-")
				{
					numbArray.Add(Convert.ToDouble(num));
					num = "";
				}
				if (text[i]=='s'|| text[i] == 'c' || text[i] == 't')
					numOfFunction.Add(numOfFunction.Count);

			}
			newInput = input;
			newtext = text;
			return numbArray;
		}
		private static List<char> LetterParser(string text)
		{
			var letterArray = new List<char>();
			for (int i = 0; i < text.Length; i++)
			{
				switch(text[i])
				{
					case '+':
						letterArray.Add('+'); break;
					case '-':
						letterArray.Add('-'); break;
					case '*':
						letterArray.Add('*'); break;
					case '/':
						letterArray.Add('/'); break;
					case '^':
						letterArray.Add('^'); break;
				}
				
			}
			return letterArray;
		}
		private static List<string> FunctionParser(string text)
		{
			var functionArray = new List<string>();
			for (int i = 0; i < text.Length-2; i++)
			{
				if (text[i] == 's' && text[i + 1] == 'i' && text[i + 2] == 'n')
					functionArray.Add("sin");
				if (text[i] == 'c' && text[i + 1] == 'o' && text[i + 2] == 's')
					functionArray.Add("cos");
				if (text[i] == 't' && text[i + 1] == 'g')
					functionArray.Add("tg");
				if (text[i] == 'c' && text[i + 1] == 't' && text[i + 2] == 'g')
					functionArray.Add("ctg");				
			}
			return functionArray;
		}
		private static bool IsCorrectText(string text)
		{
			if (string.IsNullOrEmpty(text))
				return false;
			return true;
		}
	}
}
