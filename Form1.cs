using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class mainContainer : Form
    {
        private string resScreen = "";
        private StringBuilder sb = new StringBuilder();
        public mainContainer()
        {
            InitializeComponent();
            result.Text = resScreen;
            sb.Append(resScreen);
        }

        private void label10_Click_1(object sender, EventArgs e)
        {
            sb.Append("^");
            result.Text = sb.ToString();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            sb.Append(1);
            result.Text = sb.ToString();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            sb.Append(2);
            result.Text = sb.ToString();

        }

        private void label3_Click(object sender, EventArgs e)
        {
            sb.Append(3);
            result.Text = sb.ToString();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            sb.Append(4);
            result.Text = sb.ToString();
        }
        private void label5_Click(object sender, EventArgs e)
        {
            sb.Append(5);
            result.Text = sb.ToString();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            sb.Append(6);
            result.Text = sb.ToString();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            sb.Append(7);
            result.Text = sb.ToString();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            sb.Append(8);
            result.Text = sb.ToString();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            sb.Append(9);
            result.Text = sb.ToString();
        }

        private void label0_Click(object sender, EventArgs e)
        {
            sb.Append(0);
            result.Text = sb.ToString();
        }

        private void addition_Click(object sender, EventArgs e)
        {
            sb.Append("+");
            result.Text = sb.ToString();
        }

        private void subtraction_Click(object sender, EventArgs e)
        {
            sb.Append("-");
            result.Text = sb.ToString();
        }

        private void multiply_Click(object sender, EventArgs e)
        {
            sb.Append("*");
            result.Text = sb.ToString();
        }

        private void division_Click(object sender, EventArgs e)
        {
            sb.Append("/");
            result.Text = sb.ToString();
        }

        private void clear_Click(object sender, EventArgs e) {
            sb.Clear();
            resScreen = sb.ToString();
            result.Text = sb.ToString();
        }

        private void calculate_Click(object sender, EventArgs e)
        {
            resScreen = sb.ToString();
            double result = EvaluateExpression(resScreen);
            resScreen = result.ToString();
            sb.Clear();
            sb.Append(resScreen);
            this.result.Text = resScreen;
           
        }

        private static double EvaluateExpression(string expression)
        {
            try
            {
                // Özel işlem önceliklerini ve operatörleri belirleyin
                var operatorPrecedence = new Dictionary<string, int>
        {
            {"√", 4},
            {"sin", 4},
            {"cos", 4},
            {"tan", 4},
            {"cot", 4},
            {"^", 3},
            {"*", 2},
            {"/", 2},
            {"+", 1},
            {"-", 1}
        };

                // İfadeyi işlem önceliklerine göre diziye çevirin
                var tokens = TokenizeExpression(expression);

                // Operatörler ve operatörlerin öncelikleri için iki yardımcı yığın kullanın
                var operatorStack = new Stack<string>();
                var outputQueue = new Queue<string>();

                foreach (var token in tokens)
                {
                    if (double.TryParse(token, out double number))
                    {
                        outputQueue.Enqueue(token); // Sayılar doğrudan çıktı kuyruğuna eklenir
                    }
                    else if (operatorPrecedence.ContainsKey(token))
                    {
                        while (operatorStack.Count > 0 &&
                               operatorPrecedence.ContainsKey(operatorStack.Peek()) &&
                               operatorPrecedence[token] <= operatorPrecedence[operatorStack.Peek()])
                        {
                            outputQueue.Enqueue(operatorStack.Pop());
                        }
                        operatorStack.Push(token);
                    }
                }

                while (operatorStack.Count > 0)
                {
                    outputQueue.Enqueue(operatorStack.Pop());
                }

                var resultStack = new Stack<double>();
                foreach (var token in outputQueue)
                {
                    if (double.TryParse(token, out double number))
                    {
                        resultStack.Push(number);
                    }
                    else if (operatorPrecedence.ContainsKey(token))
                    {
                        double operand2 = resultStack.Pop();
                        double operand1 = 0;

                        operand1 = resultStack.Pop();

                        double result = PerformOperation(token, operand1, operand2);
                        resultStack.Push(result);
                    }
                }

                if (resultStack.Count == 1)
                {
                    return resultStack.Pop();
                }
            }
            catch (Exception e)
            {
                return 0;
            }

            return 0;
        }

        private static List<string> TokenizeExpression(string expression)
        {
            var tokens = new List<string>();
            int i = 0;
            while (i < expression.Length)
            {
                if (Char.IsDigit(expression[i]))
                {
                    StringBuilder number = new StringBuilder();
                    while (i < expression.Length && (Char.IsDigit(expression[i]) || expression[i] == '.'))
                    {
                        number.Append(expression[i]);
                        i++;
                    }
                    tokens.Add(number.ToString());
                }
                else
                {
                    tokens.Add(expression[i].ToString());
                    i++;
                }
            }
            return tokens;
        }

        private static double PerformOperation(string operatorToken, double operand1, double operand2)
        {
            // MessageBox.Show(String.Format("o1 : {0} token : {1} o2 : {2}", operand1, operatorToken, operand2)
            switch (operatorToken)
            {
                case "+":
                    return operand1 + operand2;
                case "-":
                    return operand1 - operand2;
                case "*":
                    return operand1 * operand2;
                case "/":
                    if (operand2 != 0)
                    {
                        return operand1 / operand2;
                    }
                    else
                    {
                        MessageBox.Show("Cannot be divide 0");
                        throw new DivideByZeroException();
                    }
                case "^":
                    return Math.Pow(operand1, operand2);
                case "√":
                    if(operand1!=0)
                    {
                        return operand1 * Math.Sqrt(operand2);
                    }
                    return Math.Sqrt(operand2);
                case "sin":
                    return Math.Sin(operand2);
                case "cos":
                    return Math.Cos(operand2);
                case "tan":
                    return Math.Tan(operand2);
                case "cot":
                    return 1 / Math.Tan(operand2);
                default:
                    throw new ArgumentException("Geçersiz operatör: " + operatorToken);
            }
        }


        private void Sqrt_Click(object sender, EventArgs e)
        {
            sb.Append("√");
            result.Text = sb.ToString();
        }

        private void sin_Click(object sender, EventArgs e)
        {
            sb.Append("sin");
            result.Text = sb.ToString();

        }

        private void cos_Click(object sender, EventArgs e)
        {
            sb.Append("cos");
            result.Text = sb.ToString();
        }

        private void tan_Click(object sender, EventArgs e)
        {
            sb.Append("tan");
            result.Text = sb.ToString();
        }

        private void cot_Click(object sender, EventArgs e)
        {
            sb.Append("cot");
            result.Text = sb.ToString();
        }
    }
}
