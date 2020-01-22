using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Matrix = Matrices.Matrix;


namespace Macierze_v2
{
    public sealed partial class MainPage : Page
    {
        public ObservableCollection<MatrixElement> matrixList = new ObservableCollection<MatrixElement>();

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void UpdateLetters()
        {
            for(int i = 0; i < matrixList.Count; i++)
            {
                matrixList[i].L = (char)(i + 'A');
            }
        }

        public void ReadMatrix(TextBox inputBox, ObservableCollection<MatrixElement> list)
        {
            Matrix temp;
            try
            {
                temp = Matrix.FromString(inputBox.Text);
            }
            catch
            {
                InputErrorMessagebox.Visibility = Visibility.Visible;
                return;
            }
            MatrixInputButton.Flyout.Hide();

            list.Add(new MatrixElement());
            list[list.Count - 1].M = temp;
            list[list.Count - 1].S = $"{list[list.Count - 1].M.Height}x{list[list.Count - 1].M.Width}";

            try
            {
                list[list.Count - 1].Det = list[list.Count - 1].M.Determinant.ToString();
            }
            catch (InvalidOperationException)
            {
                list[list.Count - 1].Det = "Determinant does not exist";
            }

            UpdateLetters();
        }

        public void ReadMatrix(TextBlock inputBox, ObservableCollection<MatrixElement> list)
        {
            Matrix temp;
            try
            {
                temp = Matrix.FromString(inputBox.Text);
            }
            catch
            {
                InputErrorMessagebox.Visibility = Visibility.Visible;
                return;
            }
            MatrixInputButton.Flyout.Hide();

            list.Add(new MatrixElement());
            list[list.Count - 1].M = temp;
            list[list.Count - 1].S = $"{list[list.Count - 1].M.Height}x{list[list.Count - 1].M.Width}";

            try
            {
                list[list.Count - 1].Det = list[list.Count - 1].M.Determinant.ToString();
            }
            catch (InvalidOperationException)
            {
                list[list.Count - 1].Det = "Determinant does not exist";
            }

            UpdateLetters();
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            matrixList.RemoveAt(MatrixListview.SelectedIndex);
            UpdateLetters();
            DeleteButton.IsEnabled = false;
        }

        private void ReadButton_Click(object sender, RoutedEventArgs e)
        {
            ReadMatrix(MatrixInputTextbox, matrixList);
        }

        private void InputButton_Click(object sender, RoutedEventArgs e)
        {
            InputErrorMessagebox.Visibility = Visibility.Collapsed;
            MatrixInputTextbox.Text = "";
        }

        private void MatrixListview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(MatrixListview.SelectedIndex != -1)
            {
                DeleteButton.IsEnabled = true;
            }
        }

        private Matrix Evaluate(string input)
        {
            if (input.Length > 1)
            {
                Regex rx = new Regex(@"[\d,]+|[it]?\((?>\((?<DEPTH>)|\)(?<-DEPTH>)|[^()]+)*\)(?(DEPTH)(?!))|\w+|[+*]");
                MatchCollection matches = rx.Matches(input);
                Matrix a;
                if(matches[0].Value[0] == 't')
                {
                    return Matrix.Transpose(Evaluate(matches[0].Value.Substring(2, matches[0].Value.Length - 3)));
                }
                else if(matches[0].Value[0] == 'i')
                {
                    return Evaluate(matches[0].Value.Substring(2, matches[0].Value.Length - 3)).Inverse();
                }
                else if(matches[0].Value[0] == '(')
                {
                    a = Evaluate(matches[0].Value.Substring(1, matches[0].Value.Length - 2));
                }
                else
                {
                    a = Evaluate(matches[0].Value);
                }

                int pos = 1;
                while(pos < matches.Count)
                {
                    if(matches[pos].Value == "+")
                    { 
                        Matrix b = Evaluate(matches[pos + 1].Value);
                        a = a + b;
                    }
                    else if(matches[pos].Value == "*")
                    {
                        if (matches[pos + 1].Value[0] >= '0' && matches[pos + 1].Value[0] <= '9')
                        {
                            a *= double.Parse(matches[pos + 1].Value);
                        }
                        else
                        { 
                            Matrix b = Evaluate(matches[pos + 1].Value);
                            a = a * b;
                        }
                    }
                    pos += 2;
                }
                return a;

            }
            else
            {
                if(input[0] >= 'A' && input[0] <= 'Z')
                {
                    return matrixList[input[0] - 'A'].M;
                }
                return null;
            }
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CalculateOutputBox.Text = Evaluate(FormulaTextBox.Text).ToString();
                AddMatrixButton.IsEnabled = true;
            }
            catch
            {
                CalculateOutputBox.Text = "Invalid formula";
                AddMatrixButton.IsEnabled = false;
            }
        }

        private void AddOutputButton_Click(object sender, RoutedEventArgs e)
        {
            ReadMatrix(CalculateOutputBox, matrixList);
            AddMatrixButton.IsEnabled = false;
        }
    }
}