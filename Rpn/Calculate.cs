using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.VisualBasic.CompilerServices;

namespace Rpn
{
	public class Calculate
	{
		public double ReadNumber(int x,string input)
		{
			var numArray = new List<double>();
			var letterArray = new List<char>();
			var functionArray = new List<string>();
			string text = Environment.CurrentDirectory + "\\Input.txt";
			//string input = File.ReadAllText(text).Trim().ToLower();
			if(IsCorrectText(input))
			{
				numArray = NumbParser(input);
				letterArray = LetterParser(input);
				functionArray = FunctionParser(input);
			}
			return CountAnswer(letterArray,numArray);
		}
		private static double CountAnswer(List<char> mathLetter,List<double> numArray)
		{
			double num=0;
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
		private static List<double> NumbParser(string text)
		{
			text += " ";
			string num="";
			var numbArray = new List<double>();
			for (int i = 0; i < text.Length; i++)
			{
				while (double.TryParse(text[i].ToString(), out _))
				{
					num += text[i];
					i++;
				}
				if(num!="")
				{
					numbArray.Add(Convert.ToDouble(num));
					num = "";
				}				
			}
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
				if (text[i] == 'l' && text[i + 1] == 'o' && text[i + 2] == 'g')
					functionArray.Add("log");
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
