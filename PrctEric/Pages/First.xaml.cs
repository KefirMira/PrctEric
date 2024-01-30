using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using PrctEric.Models;

namespace PrctEric.Pages;

public partial class First : Page
{
    private int rows = 0;
    private int columns = 0;
    private MainFunc _func;
    private double[] _func1;
    private List<ModelFunc> _funcs;
    private double[,] matr;
    private double[,] matri;
    private double[,] matrix;
    private List<Save> _left;
    private List<Save> _top;
    private double[,] matrixe;
    private double[,] matrixes;
    public First()
    {
        InitializeComponent();
        _func = new MainFunc();
        _funcs = new List<ModelFunc>();
        
    }

    private void CreateButton_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            _funcs.Clear();
            
            FuncPanel.Children.Clear();
            MatrixPanel.Children.Clear();
            columns = Convert.ToInt32(PerTextBox.Text);
            rows = Convert.ToInt32(OgrTextBox.Text);
            StackPanel stackPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal
                ,Margin = new Thickness(2)
            };
            TextBlock text = new TextBlock()
            {
                Text = "f(x) = "
                ,Margin = new Thickness(2)
            };
            stackPanel.Children.Add(text);
            for (int i = 0; i < columns; i++)
            {
                TextBlock newtext = new TextBlock()
                {
                    Text = "x" + (i + 1)
                    ,Margin = new Thickness(2)
                };
                TextBox textBox = new TextBox()
                {
                    Width = 20
                    ,Margin = new Thickness(2)
                };
                TextBlock newtext1 = new TextBlock()
                {
                    Text = " + "
                    ,Margin = new Thickness(2)
                };
                stackPanel.Children.Add(textBox);
                stackPanel.Children.Add(newtext);
                stackPanel.Children.Add(newtext1);
            }

            
            TextBlock newtex4t = new TextBlock()
            {
                Text = "--> max"
                ,Margin = new Thickness(2)
            };
            stackPanel.Children.Add(newtex4t);
            FuncPanel.Children.Add(stackPanel);
            StackPanel ogrStackPanelFirst = new StackPanel()
            {
                Orientation = Orientation.Horizontal
                ,Margin = new Thickness(2)
            };
            for (int i = 0; i < rows; i++)
            {
                StackPanel orgSt = new StackPanel()
                {
                    Orientation = Orientation.Horizontal
                    ,Margin = new Thickness(2)
                };
                for (int j = 0; j < columns; j++)
                {
                    TextBox textBox = new TextBox()
                    {
                        Width = 20
                        ,Margin = new Thickness(2)
                    };
                    TextBlock newtext = new TextBlock()
                    {
                        Text = "x" + (i + 1)
                        ,Margin = new Thickness(2)
                    };
                    orgSt.Children.Add(textBox);
                    orgSt.Children.Add(newtext);
                    TextBlock newtext1 = new TextBlock()
                    {
                        Text = " + "
                        ,Margin = new Thickness(2)
                    };
                    orgSt.Children.Add(newtext1);
                }

                ComboBox comboBox = new ComboBox();
                comboBox.Items.Add(">=");
                comboBox.Items.Add("<=");
                
                TextBox newtextBox = new TextBox()
                {
                    Width = 20
                    ,Margin = new Thickness(2)
                };
                orgSt.Children.Add(comboBox);
                orgSt.Children.Add(newtextBox);
                MatrixPanel.Children.Add(orgSt);
                CalcButton.Visibility = Visibility.Visible;
            }
        }
        catch
        {
            MessageBox.Show("Ошибка формирования функции! Провербте правильность введённых данных!");
        }
    }

    public void ReadMatrix()
    {
        List<TextBox> textBoxes = new List<TextBox>();
        foreach (UIElement item in FuncPanel.Children)
        {
            if (item is StackPanel)
            {
                foreach (UIElement item1 in (item as StackPanel).Children)
                {
                    if (item1.GetType() == typeof(TextBox))
                        textBoxes.Add(item1 as TextBox);
                }
            }
        } 
        double[] mas = new double[textBoxes.Count];
        _func1 = new double[textBoxes.Count];
        for (int i = 0; i < textBoxes.Count; i++)
        {
            mas[i] = Convert.ToDouble(textBoxes[i].Text);
        }

        _func1 = mas;
        _func.Mas = mas;
        List<TextBox> textBoxes1 = new List<TextBox>();
        foreach (UIElement item in MatrixPanel.Children)
        {
            if (item is StackPanel)
            {
                foreach (UIElement item1 in (item as StackPanel).Children)
                {
                    if (item1.GetType() == typeof(TextBox))
                        textBoxes1.Add(item1 as TextBox);
                }
            }
        } 
        int g = 0;
        for (int i = 0; i < rows; i++)
        {
            ModelFunc model = new ModelFunc();
            double[] matrix = new double[columns];
            //g++;
            for (int j = 0; j < matrix.Length; j++)
            {
                matrix[j] = Convert.ToDouble(textBoxes1[g].Text);
                g++;
            }

            model.Mas = matrix;
            model.Rez = Convert.ToDouble(textBoxes1[g].Text);
            _funcs.Add(model);
            g++;
        }
        
        List<ComboBox> comboBoxes = new List<ComboBox>();
        foreach (UIElement item in MatrixPanel.Children)
        {
            if (item is StackPanel)
            {
                foreach (UIElement item1 in (item as StackPanel).Children)
                {
                    if (item1.GetType() == typeof(ComboBox))
                        comboBoxes.Add(item1 as ComboBox);
                }
            }
        }
        for (int i = 0; i < _funcs.Count; i++)
        {
            _funcs[i].Ravno = comboBoxes[i].SelectedIndex;
        }
    }
    
    private void CalcButton_OnClick(object sender, RoutedEventArgs e)
    {
        ReadMatrix();
        matr = new double[rows,columns+2];
        int r = 0;
        //преобразуем к матрице с u, но без базисной переменной
        foreach (var item in _funcs)
        {
            if (item.Rez<0)
            {
                for (int i = 0; i < item.Mas.Length; i++)
                {
                    matr[r, i] = item.Mas[i]*-1;
                }
                matr[r,columns+1] =-1* item.Rez;
                if (item.Ravno == 0)
                {
                    matr[r, columns] = 1;
                }
                else
                {
                    matr[r, columns ] = -1;
                }
            }
            else
            {
                for (int i = 0; i < item.Mas.Length; i++)
                {
                    matr[r, i] = item.Mas[i];
                }
                matr[r,columns+1] = item.Rez;
                if (item.Ravno == 0)
                {
                    matr[r, columns ] = -1;
                }
                else
                {
                    matr[r, columns ] = 1;
                }
            }

           
            r++;
        }

        // for (int i = 0; i < matr.GetUpperBound(0)+1; i++)
        // {
        //     for (int j = 0; j < matr.GetUpperBound(1)+1; j++)
        //     {
        //         Console.Write(matr[i,j] + "\t");
        //     }
        //     Console.WriteLine();
        // }

        matri = new double[rows, columns + rows];
        for (int i = 0; i < matri.GetUpperBound(0)+1; i++)
        {
            for (int j = 0; j < matri.GetUpperBound(1)+1; j++)
            {
                if (j < columns)
                {
                    matri[i, j] = matr[i, j];
                }
                else if (j - columns == i)
                {
                    matri[i, j] = matr[i, matr.GetUpperBound(1)-1];
                }
                else
                {
                    matri[i, j] = 0;
                }
            }
        }
        
        //ДОБАВЛЯЕМ W
        int colvo = 0;
        for (int i = 0; i < matri.GetUpperBound(0)+1; i++)
        {
            for (int j = columns; j < matri.GetUpperBound(1)+1; j++)
            {
                if (j - columns == i)
                {
                    if (matri[i, j] != 1)
                    {
                        colvo++;
                    }
                }
            }
        }
        int[] index = new int[colvo];
        int u = 0;
        //ищем индексы неподходящих нам строк
        for (int i = 0; i < matri.GetUpperBound(0)+1; i++)
        {
            for (int j = columns; j < matri.GetUpperBound(1)+1; j++)
            {
                if (j - columns == i)
                {
                    if (matri[i, j] != 1)
                    {
                        index[u] = i;
                        u++;
                    }
                }
            }
        }
        
        //создаём матрицу с w
        matrix = new double[rows,rows+columns+colvo];
        u = 0;
        for (int i = 0; i < matrix.GetUpperBound(0)+1; i++)
        {
            for (int j = 0; j < matrix.GetUpperBound(1)+1; j++)
            {
                if (j < (columns + rows))
                {
                    matrix[i, j] = matri[i, j];
                }
                else
                {
                    for (int k = 0; k < index.Length; k++)
                    {
                        if (i == index[k])
                        {
                            matrix[i, j] = 1;
                            index[k] = -1;
                            break;
                        }
                    }
                    
                }
            }
        }


        _left = new List<Save>();
        _top = new List<Save>();
        matrixe = new double[rows+1,matrix.GetUpperBound(1)+1-rows+1];
        
        for (int i = 0; i < matri.GetUpperBound(0)+1; i++)
        {
            for (int j = columns; j < matri.GetUpperBound(1)+1; j++)
            {
                if (j - columns == i)
                {
                    if (matri[i, j] != 1)
                    {
                        index[u] = i;
                        u++;
                    }
                }
            }
        }

        int[] ind = new int[rows];
         
        for (int i = 0; i < matrix.GetUpperBound(0)+1; i++)
        {
            for (int j = columns; j < matrix.GetUpperBound(1)+1; j++)
            {
                if (matrix[i, j] == 1)
                    ind[i] = j;
            }
        }
        
        
        
        //составляем таблицу для решения F
        int h = columns;
        for (int i = 0; i < matrix.GetUpperBound(0)+1; i++)
        {
            for (int j = 0; j < matrix.GetUpperBound(1)+1; j++)
            {
                if (j < columns)
                {
                    Save s = new Save()
                    {
                        Name = "x",
                        Num = j+1,
                        IndexInMas = i
                    };
                    matrixe[i, j] = matrix[i, j];
                    //h = j;
                    if(_top.FirstOrDefault(c=>c.Name==s.Name&&c.Num==s.Num)==null)
                        _top.Add(s);
                }
                else
                {
                    if (matrix[i, j] == -1)
                    {
                        Save ux = new Save()
                        {
                            Name = "u",
                            Num = j-columns+1,
                            IndexInMas = i
                        };
                        for (int k = 0; k < matrix.GetUpperBound(0)+1; k++)
                        {
                            matrixe[k, h] = matrix[k, j];
                        }
                        h++;
                        if(_top.FirstOrDefault(c=>c.Name==ux.Name&&c.Num==ux.Num)==null)
                            _top.Add(ux);
                    }
                    else
                    {
                        if (j >= rows + columns)
                        {
                            Save s = new Save()
                            {
                                Name = "w",
                                Num = j+1 -columns-rows,
                                IndexInMas = j
                            };
                            if(_left.FirstOrDefault(c=>c.Name==s.Name&&c.Num==s.Num)==null&&_top.FirstOrDefault(c=>c.Name==s.Name&&c.Num==s.Num)==null)
                                _left.Add(s);    
                        }
                        else
                        {
                            Save s = new Save()
                            {
                                Name = "u",
                                Num = j+1-columns,
                                IndexInMas = j
                            };
                            if(_left.FirstOrDefault(c=>c.Name==s.Name&&c.Num==s.Num)==null&&_top.FirstOrDefault(c=>c.Name==s.Name&&c.Num==s.Num)==null)
                                _left.Add(s);
                        }
                        
                    }
                }
            }
        }

        foreach (var item in _top)
        {
            Save s = _left.FirstOrDefault(c => c.Name == item.Name && c.Num == item.Num);
            if (s!=null)
                _left.RemoveAt(_left.FindIndex(c => c.Name == item.Name && c.Num == item.Num));
        }
        
        for (int write = 0; write < _left.Count; write++) {
            for (int sort = 0; sort < _left.Count - 1; sort++) {
                if (_left[sort].IndexInMas == ind[sort + 1]) {
                    Save temp = _left[sort ];
                    _left[sort] = _left[sort + 1];
                    _left[sort+1] = temp;
                }
            }
        }
        
        for (int i = 0; i < matrixe.GetUpperBound(0); i++)
        {
            matrixe[i, matrixe.GetUpperBound(1)] = matr[i,matr.GetUpperBound(1)];
        }


        
        
       for (int i = 0; i < _top.Count; i++)    
        {
            Console.WriteLine(_top[i].Name+_top[i].Num);
        }

        for (int i = 0; i < _left.Count; i++)
        {
            Console.WriteLine(_left[i].Name+_left[i].Num);
        }


        for (int i = 0; i < matrixe.GetUpperBound(1) + 1; i++)
        {
            double p = 0;
            for (int j = 0; j < index.Length; j++)
            {
                p += matrixe[index[j], i]*-1;
            }
            matrixe[matrixe.GetUpperBound(0), i] = p;
        }

        for (int i = 0; i < matrixe.GetUpperBound(0)+1; i++)
        {
            for (int j = 0; j < matrixe.GetUpperBound(1)+1; j++)
            {
                Console.Write(matrixe[i,j]+$"\t");
            }
            Console.WriteLine();
        }

        Simplex();



        int d = 0;
        foreach (var item in _top)
        {
            if (item.Name != "w")
                d++;
        }
        
        int v = 0;
        foreach (var item in _left)
        {
            if (item.Name != "w")
                v++;
        }

        matrixes = new double[v+1, d+1];

        v = 0;
        d = 0;

        List<Save> _forDel = new List<Save>();
        for (int i = 0; i < matrixe.GetUpperBound(0); i++)
        {
            
            if (_left.Exists(c => c.IndexInMas == i&&c.Name=="w"))
            {
                Save l = _left.FirstOrDefault(c => c.IndexInMas == i && c.Name == "w");
                _forDel.Add(l);
                continue;
            }
            else
            {
                v = 0;
                for (int j = 0; j < matrixe.GetUpperBound(1)+1; j++) 
                {
                    if (_top.Exists(c=>c.IndexInMas==j&&c.Name=="w"))
                    {
                        Save l = _top.FirstOrDefault(c => c.IndexInMas == i && c.Name == "w");
                        _forDel.Add(l);
                        continue;
                    }
                    else
                    {
                        matrixes[d, v] = matrixe[i, j];
                        v++;
                    }
                }
                d++;    
            }
        }

        foreach (var item in _forDel)
        {
            _top.Remove(item);
            _left.Remove(item);
        }

        for (int i = 0; i < _left.Count(); i++)
        {
            _left[i].IndexInMas = i;
        }

        for (int i = 0; i < _top.Count; i++)
        {
            _top[i].IndexInMas = i;
        }
        
        for (int i = 0; i < matrixes.GetUpperBound(0)+1; i++)
        {
            for (int j = 0; j < matrixes.GetUpperBound(1)+1; j++)
            {
                Console.Write(matrixes[i,j]+$"\t");
            }
            Console.WriteLine();
        }
        
            Console.WriteLine();
            for (int j = 0; j < matrixes.GetUpperBound(1); j++)
            {
                double k = 0;
                for (int i = 0; i < matrixes.GetUpperBound(0) ; i++)
                {
                    if (_left[i].Name == "x" && _left[i].IndexInMas == i)
                    {
                        k += matrixes[i, j] * _func1[_left[i].Num - 1];
                    }
                    else
                    {
                        k += 0;
                    }
                }

                if (_top[j].Name == "x" && _top[j].IndexInMas == j)
                {
                    k -= _func1[_top[j].Num - 1];
                }
                matrixes[matrixes.GetUpperBound(0),j] = k;
            }

            
            
            
            double m = 0;
            for (int i = 0; i < matrixes.GetUpperBound(0); i++)
            {
                if (_left[i].Name == "x" && _left[i].IndexInMas == i)
                {
                    m += matrixes[i, matrixes.GetUpperBound(1)] * _func1[_left[i].Num - 1];
                }
            }

            matrixes[matrixes.GetUpperBound(0), matrixes.GetUpperBound(1)] = m;
            for (int i = 0; i < matrixes.GetUpperBound(0)+1; i++)
            {
                for (int j = 0; j < matrixes.GetUpperBound(1)+1; j++)
                {
                    Console.Write(matrixes[i,j]+$"\t");
                }
                Console.WriteLine();
            }
        
        SimplexSecond();


        string res = "x*=(";
        for (int i = 0; i < _left.Count; i++)
        {
            if (_left[i].Name == "x")
            {
                res += "x" + i+1+"=";
                res += matrixes[i, matrixes.GetUpperBound(1)];
                res += ";";
            }
        }
        
        for (int i = 0; i < _top.Count; i++)
        {
            if (_top[i].Name == "x")
            {
                res += "x" + i+1+"=";
                res += 0;
                res += ";";
            }
        }

        res += ")";

        res += $"\nf*={matrixes[matrixes.GetUpperBound(0),matrixes.GetUpperBound(1)]}";

        FullResTextBlock.Text = res;

    }

    public bool CheckForMax(double[,] newmatr)
    {
        try
        {
            for (int i = 0; i < newmatr.GetUpperBound(1)+1; i++)
            {
                if (newmatr[newmatr.GetUpperBound(0) , i]<0)
                {
                    return true;
                }
            }
        }
        catch
        {
            MessageBox.Show("Ошибка");
        }
            
        return false;
    }
    
    public int SearchIndexMinColumn(double[,] newmatr)
    {
        int index = 0;
        try
        {
            double min = newmatr[newmatr.GetUpperBound(0) , 0];
            
            for (int i = 0; i < newmatr.GetUpperBound(1); i++)
            {
                if (newmatr[newmatr.GetUpperBound(0) , i] < min)
                {
                    min = newmatr[newmatr.GetUpperBound(0) , i];
                    index = i;
                }
            }
        }
        catch
        {
            MessageBox.Show("Ошибка");
        }
            
        return index;
    }

    public int SearchIndexMinRow(double[,] newmatr,int j)
    {
        int index = 0;
        try
        {
            double[] min = new double[newmatr.GetUpperBound(0)];

            for (int i = 0; i < newmatr.GetUpperBound(0); i++)
            {
                if (newmatr[i, j] < 0)
                {
                    min[i] = 100000000;
                }
                else
                {
                    min[i] = newmatr[i, newmatr.GetUpperBound(1) ] / newmatr[i, j];
                }
            }
            for (int i = 0; i < min.Length; i++)
            {
                if (min[index] > min[i])
                    index = i;
            }


        }
        catch
        {
            MessageBox.Show("Ошибка");
        }
        return index;
    }

    
    public double[,] NullRow(double[,] newmatr,int indexColumn,int indexRow)
    {
        double[,] newnewmatr = new double[newmatr.GetUpperBound(0)+1,newmatr.GetUpperBound(1)+1];
        try
        {
            for (int i = 0; i < newmatr.GetUpperBound(1)+1; i++)
            {
                newnewmatr[indexRow, i] = newmatr[indexRow, i] / newmatr[indexRow, indexColumn];
            }

            for (int i = 0; i < newnewmatr.GetUpperBound(0)+1; i++)
            {
                if(i==indexRow)
                    continue;
                newnewmatr[i, indexColumn] = newmatr[i, indexColumn]/newmatr[newmatr.GetUpperBound(0),indexColumn];
            }

        }
        catch
        {
            MessageBox.Show("Ошибка преобразования");
        }
         return newnewmatr;
    }
    
    public double[,] NullRowSecond(double[,] newmatr,int indexColumn,int indexRow)
    {
        double[,] newnewmatr = new double[newmatr.GetUpperBound(0)+1,newmatr.GetUpperBound(1)+1];
        newnewmatr[indexRow, indexColumn] = newmatr[indexRow, indexColumn];
        try
        {
            for (int i = 0; i < newmatr.GetUpperBound(1)+1; i++)
            {
                if(i==indexColumn)
                    continue;
                newnewmatr[indexRow, i] = newmatr[indexRow, i] / newmatr[indexRow, indexColumn] ;
            }

            for (int i = 0; i < newnewmatr.GetUpperBound(0)+1; i++)
            {
                if(i==indexRow)
                    continue;
                newnewmatr[i, indexColumn] = newmatr[i, indexColumn]/newmatr[indexRow, indexColumn]*-1;
            }

        }
        catch
        {
            MessageBox.Show("Ошибка преобразования");
        }
        return newnewmatr;
    }
    
    public double[,] Res(double[,] newmatr, int indexColumn, int indexRow, double[,] newnewmatr)
    {
        try
        {
            for (int i = 0; i < newnewmatr.GetUpperBound(0)+1; i++)
            {
                for (int j = 0; j < newnewmatr.GetUpperBound(1)+1; j++)
                {
                    if(i==indexRow||j==indexColumn)
                        continue;
                    newnewmatr[i, j] = ((newmatr[i, j]* newmatr[indexRow, indexColumn]- (newmatr[i,indexColumn]*newmatr[indexRow,j]) )/newmatr[indexRow, indexColumn]) ;
                }
            }

        }
        catch
        {
            MessageBox.Show("Ошибка вычислений");
        }
            
        return newnewmatr;
    }

    public void Simplex()
    {
        while (CheckForMax(matrixe))
        {
            int indexColumn = SearchIndexMinColumn(matrixe);
            int indexRow = SearchIndexMinRow(matrixe, indexColumn);
            Save s = new Save()
            {
                Name =  _top[indexColumn].Name,
                Num =   _top[indexColumn].Num 
            };
            _top[indexColumn].Name = _left[indexRow].Name;
            _top[indexColumn].Num = _left[indexRow].Num;
            _left[indexRow] = s;
            
            double[,] newnewmatr = NullRow(matrixe, indexColumn, indexRow);
            matrixe = Res(matrixe, indexColumn, indexRow, newnewmatr);
                Console.WriteLine();
                Console.WriteLine();

            for (int i = 0; i < matrixe.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < matrixe.GetUpperBound(1) + 1; j++)
                {
                    Console.Write($"{matrixe[i, j]}\t");
                }

                Console.WriteLine();
            }


            foreach (var item in _left)
            {
                Console.WriteLine(item.Name+item.Num);
            }

            if (matrixe[matrixe.GetUpperBound(0), matrixe.GetUpperBound(1)] == 0)
            {
                
            }
            else
            {
                MessageBox.Show("Продолжить вычисления невозможно");
            }

        }
    }
    
    public void SimplexSecond()
    {
        while (CheckForMax(matrixes))
        {
            int indexColumn = SearchIndexMinColumn(matrixes);
            int indexRow = SearchIndexMinRow(matrixes, indexColumn);
            Save s = new Save()
            {
                Name =  _top[indexColumn].Name,
                Num =   _top[indexColumn].Num 
            };
            _top[indexColumn].Name = _left[indexRow].Name;
            _top[indexColumn].Num = _left[indexRow].Num;
            _left[indexRow] = s;
            
            double[,] newnewmatr = NullRowSecond(matrixes, indexColumn, indexRow);
            matrixes = Res(matrixes, indexColumn, indexRow, newnewmatr);
            Console.WriteLine();
            Console.WriteLine();

            for (int i = 0; i < matrixes.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < matrixes.GetUpperBound(1) + 1; j++)
                {
                    Console.Write($"{matrixes[i, j]}\t");
                }

                Console.WriteLine();
            }


            foreach (var item in _left)
            {
                Console.WriteLine(item.Name+item.Num);
            }

            

        }

        
    }
}
