//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Diagnostics;

//namespace Gen
//{
//    public class Main
//    {
//        int numbChrom; //Количество хромосом
//        int numbVariables = 2;
//        double chMutation;
//        double chCrossing;
//        List<Individual> listIndivid;
//        Random rnd;

//        public Main()
//        {

//        }

//        private void Enter_Click(object sender, EventArgs e)
//        {
//            Reset();

//            numbChrom = int.Parse(numbChromosomes.Text);


//            chMutation = double.Parse(chanceMutation.Text);

//            chCrossing = double.Parse(chanceCrossing.Text);


//            this.numbVariables = formFunction.numbVariables;
//            double[] x = new double[numbVariables];

//            CreateIndividum();

//        }
//        Individual ind;

//        private void Algoritm()
//        {
//            Crossover();
//            Mutation();
//            Sorting();
//            Delete();

//        }

//        private void CreateIndividum()
//        {

//            rnd = new Random();
//            for (int i = 0; i != numbChrom; i++)
//            {
//                double[] x = new double[numbVariables];
//                Individual indiv = new Individual();
//                //ind.setX();
//                for (int j = 0; j != x.Count(); j++)
//                {
//                    x[j] = rnd.Next(0, 100);
//                }
//                indiv.setX(x);
//                indiv.setResultFunc(formFunction.resFunction(x));
//                listIndivid.Add(indiv);
//            }
//        }

//        private void Crossover()
//        {
//            Individual Parent1;
//            Individual Parent2;
//            double[] xChild;
//            Random rnd1 = new Random();
//            Random rnd2 = new Random();
//            int Irnd1 = 0, Irnd2 = 0;
//            for (int i = 0; i < numbChrom * (chCrossing * 0.01); i++)
//            {
//                Parent1 = new Individual();
//                Parent2 = new Individual();
//                Irnd1 = rnd1.Next(0, numbChrom);
//                Irnd2 = rnd2.Next(0, numbChrom);
//                if (Irnd1 == Irnd2)
//                {
//                    if (Irnd2 == numbChrom - 1)
//                        Irnd2 -= 1;
//                    else
//                        Irnd2 += 1;
//                }
//                Parent1.setX(listIndivid[Irnd1].getX());
//                Parent2.setX(listIndivid[Irnd2].getX());
//                //Parent2 = new Individual(listIndivid[rnd2.Next(0, numbChrom)].getX());
//                ind = new Individual();
//                xChild = CreateChild(Parent1, Parent2); //Координаты потомка 
//                ind.setX(xChild); // Назначаем
//                ind.setResultFunc(formFunction.resFunction(xChild)); // Назначаем значение функции в данных координатах
//                listIndivid.Add(ind);
//            }
//        }

//        private void Sorting()
//        {
//            double min;
//            int indexMin = 0;
//            for (int i = 0; i < listIndivid.Count() - 1; i++)
//            {
//                min = formFunction.resFunction(listIndivid[i].getX());
//                Individual indiv = new Individual();
//                //indiv = listIndivid[i];
//                indexMin = i;
//                for (int j = i + 1; j < listIndivid.Count(); j++)
//                {
//                    if (listIndivid[j].getResultFunc() <= min)
//                    {
//                        min = listIndivid[j].getResultFunc();
//                        indiv = listIndivid[j];
//                        indexMin = j;
//                    }
//                }
//                if (indexMin != i)
//                {
//                    listIndivid[indexMin] = listIndivid[i];
//                    listIndivid[i] = indiv;
//                }
//            }
//            //listIndivid.RemoveRange(numbChrom, (listIndivid.Count() - 1));

//        }
//        private double CalcFunction(Individual individ)
//        {
//            double[] koord = individ.getX();
//            //double result = pars.resultFunction(koord);
//            double result = formFunction.resFunction(koord);
//            individ.setResultFunc(result);
//            return (result);
//        }
//        private double[] CreateChild(Individual Parent1, Individual Parent2)
//        {
//            rnd = new Random();
//            double[] _X = new double[numbVariables];
//            double[] xP1 = Parent1.getX();
//            double[] xP2 = Parent2.getX();
//            double w = rnd.NextDouble();
//            for (int i = 0; i != _X.Count(); i++)
//            {
//                _X[i] = w * xP1[i] + (1 - w) * xP2[i];
//            }
//            return _X;
//        }

//        private void Mutation()
//        {
//            Random rndVariables = new Random();
//            int numbMut;
//            double[] xMut = new double[numbVariables];
//            int numbVar;
//            //rnd = new Random();
//            for (int i = 0; i < numbChrom * (chMutation * 0.01); i++)
//            {
//                rnd = new Random();
//                numbMut = rnd.Next(0, numbChrom); //Выбираем индивид,который будет мутировать
//                numbVar = rndVariables.Next(0, numbVariables); //Выбираем хромосому этого индивида
//                xMut = listIndivid[numbMut].getX();
//                xMut[numbVar] += rnd.NextDouble() * rnd.Next(-1, 1); //Производим мутацию этого гена. Прибавляем или отнимаем значение от 0 до 1
//                listIndivid[numbMut].setX(xMut);//Записываем мутированный индивид
//                listIndivid[numbMut].setResultFunc(formFunction.resFunction(xMut));
//            }
//        }

//        private void Conclusion()
//        {

//            double[] averageX = new double[numbVariables];
//            for (int i = 0; i < listIndivid.Count(); i++)
//            {
//                for (int j = 0; j < numbVariables; j++)
//                    averageX[j] += listIndivid[i].getX()[j];
//            }

//            for (int j = 0; j < numbVariables; j++)
//            {
//                averageX[j] /= listIndivid.Count();
//                richTextBox1.Text += averageX[j] + "\r\n";
//            }
//        }
//        private void Delete()
//        {
//            /*for (int i = 0; i < listIndivid.Count() - 1; i++)
//            {
//                for (int j = i+1; j < listIndivid.Count(); j++)
//                {
//                    if (listIndivid[i].getResultFunc() == listIndivid[j].getResultFunc())
//                    {
//                        listIndivid.Remove(listIndivid[j]);
//                    }
//                }
//            }*/
//            listIndivid.RemoveRange(numbChrom, (listIndivid.Count()) - numbChrom);
//        }


//        private void Iteration(int sizePop)
//        {
//            for (int i = 0; i != sizePop; i++)
//            {
//                Algoritm();
//                Conclusion();
//            }
//        }


//        private void Reset()
//        {
//            numbChrom = 0;
//            chMutation = 0.0;
//            chCrossing = 0.0;
//            listIndivid = new List<Individual>();
//        }

//    }
//}
