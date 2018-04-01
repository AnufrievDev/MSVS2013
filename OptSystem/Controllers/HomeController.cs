using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OptSystem.Models;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;


namespace OptSystem.Controllers
{
    public class HomeController : Controller
    {
        // создаем контекст данных
        ProjContext db = new ProjContext();
        int[] checks = new int[2];
        double[] genSet = {5.0, 10.0, 15.0, 20.0};
        [HttpGet]
        public ActionResult Main()
        {
            // возвращаем представление
            return View();
        }

        [HttpPost]
        public ActionResult Main(HttpPostedFileBase file)
        {
            string fileName = string.Empty;
            string dir = "";
            string nameNoExt = "";
            string outFile = "";
            double[,] startarr;

            if (file != null)
            {

                dir = Path.GetDirectoryName(file.FileName);
                nameNoExt = Path.GetFileNameWithoutExtension(file.FileName);
                outFile = dir + @"\" + nameNoExt + @".out";
                string fin = "";
                fin = file.FileName;
                CmdPspice(file.FileName); //выполняет симуляцию
                NomFinder("C:\\Cadence\\Projects\\aaa\\A3.net");
                startarr = ACFinder(outFile);
                ViewBag.Params = startarr; //массив с исходным графиком
                ViewBag.Params2 = startarr; //желаемый массив
                ViewBag.Params3 = startarr; //результирующий массив
                checks[0] = 1;
                ViewBag.Params4 = checks;
                return View("flotplot");
                //return "По файлу " + fin + " будет построен график АЧХ";
            }
                
            else
                return View();
        }


        public ActionResult Gen(List<String> first, List<String> second, List<String> third, bool[] fourth, double[] fifth)
        {                
            genSet = fifth;
            List<double> changeValue = new List<double>();     
            
            var nominal = NomFinder("C:\\Cadence\\Projects\\aaa\\A3.net");
            string[,] allStrings = nominal.Item1;
            List<string> rcList = nominal.Item2;
            List<int> rcNumbers = nominal.Item3;

            if (fourth != null)
                checks = new int[fourth.Length];
            else
                checks = new int[rcList.Count];

            for (int i = 0; i < checks.Length; i++)
            {
                if (fourth != null)
                    checks[i] = Convert.ToInt32(fourth[i]);
                else
                    checks[i] = 1;
                if (checks[i] == 1) //создаем массив из номиналов элементов которые необходимо изменить
                {
                    if (allStrings[rcNumbers[i], 3].IndexOf("k", 0) != -1)
                        changeValue.Add(Convert.ToDouble(allStrings[rcNumbers[i], 3].Replace('k', ' ')) * 1000); 
                    else
                    if (allStrings[rcNumbers[i], 3].IndexOf("p", 0) != -1)
                        changeValue.Add(Convert.ToDouble(allStrings[rcNumbers[i], 3].Replace('p', ' ')) / 1000000000000); 
                    else
                    if (allStrings[rcNumbers[i], 3].IndexOf("u", 0) != -1)
                        changeValue.Add(Convert.ToDouble(allStrings[rcNumbers[i], 3].Replace('u', ' ')) / 1000000); 
                    else
                        changeValue.Add(Convert.ToDouble(allStrings[rcNumbers[i], 3]));

                }
            }

            string path = @"C:\\Cadence\\array.net";
            ToNetFile(path, allStrings, rcNumbers, checks);

            ViewBag.Params = first;
            ViewBag.Params2 = second;
            ViewBag.Params3 = ACFinder("C:\\Cadence\\Projects\\amp_orig\\A3.out");
            return PartialView("Partial");
        }


        [HttpGet]
        public ActionResult Index()
        {
            // получаем из бд все объекты Param
            //IEnumerable<Param> paramss = db.Params;

            var nominal = NomFinder("C:\\Cadence\\Projects\\aaa\\A3.net");
            string[,] paramss1 = nominal.Item1;
            List<string> paramss2 = nominal.Item2;
            List<int> paramss3 = nominal.Item3;
            // передаем все объекты в динамическое свойство Books в ViewBag
            ViewBag.Params1 = paramss1;
            ViewBag.Params2 = paramss2;
            ViewBag.Params3 = paramss3;
            checks[0] = 1;
            ViewBag.Params4 = checks;

            // возвращаем представление
            return PartialView("Index");
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            if (file != null)
            {
                string fin = "";
                string dir = "";
                fin = file.FileName;
                dir = Path.GetDirectoryName(fin);
                CmdPspice(file.FileName); //выполняет симуляцию


                //return "По файлу " + fin + " будет построен график АЧХ";
            }

            int[,] arrr = new int[20, 2];

            for (int i = 0; i < 20; i++)
            {
                arrr[i, 0] = i;
                arrr[i, 1] = i * i;

            }
            ViewBag.Params = ACFinder("C:\\Cadence\\Projects\\aaa\\A3.out");

            return View("flotplot");
        }

        public ActionResult Partial()
        {
            ViewBag.Message = "Это частичное представление.";
            return PartialView();
        }


        public static void CmdPspice(string path) //выполняет симуляцию
        {
            // создаем процесс cmd.exe с параметрами 
            //path = "C:\\Cadence\\Projects\\aaa\\A3.cir"; //debug
            string cmdstr = "/C C:\\Cadence\\SPB_17.2\\tools\\bin\\psp_cmd.exe -r " + path;
            ProcessStartInfo psiOpt = new ProcessStartInfo(@"cmd.exe", @cmdstr);
            // скрываем окно запущенного процесса
            psiOpt.WindowStyle = ProcessWindowStyle.Hidden;
            psiOpt.RedirectStandardOutput = true;
            psiOpt.UseShellExecute = false;
            psiOpt.CreateNoWindow = true;
            // запускаем процесс
            Process procCommand = Process.Start(psiOpt);
            // получаем ответ запущенного процесса
            StreamReader srIncoming = procCommand.StandardOutput;
            
            // выводим результат
            if (false)
            {
                Console.WriteLine("Result:");
                Console.WriteLine(srIncoming.ReadToEnd());
                // закрываем процесс
                procCommand.WaitForExit();
                Console.ReadKey();
            }

            procCommand.WaitForExit();
            //System.Threading.Thread.Sleep(2000); //задержка 2 сек
        }

        public static double[,] ACFinder(string parth) //выполняет поиск таблицы АЧХ по выходному файлу
        {
            string strTarget = "FREQ"; //искомый текст
            string strTarget2 = "CONCLUDED"; //искомый текст 2 
            int startPos = 0; //начальная позиция таблицы АЧХ 
            int finPos = 0; //конечная позиция ачх
            //parth = "C:\\Cadence\\Projects\\aaa\\A3.out"; //debug
            FileInfo fileInf = new FileInfo(parth);

            string[] lines = System.IO.File.ReadAllLines(@parth).Take(System.IO.File.ReadAllLines(@parth).Length).ToArray(); //переписывает файл в массив по строкам
            //ищем по строкам где начинается и заканчивается таблица ачх
            double a = lines.Length;
            double b = System.IO.File.ReadAllLines(@parth).Length;
            for (int i = 0; i < System.IO.File.ReadAllLines(@parth).Length; i++)
            {
                if (lines[i].IndexOf(strTarget, 0) != -1)
                {
                    startPos = i + 3; //сдвигаем на 3 строки вниз т.к. значения находятся ниже шапки
                }
                if (lines[i].IndexOf(strTarget2, 0) != -1)
                {
                    finPos = i - 3; //сдвигаем на 3 строки вверх т.к. значения заканчиваются вверх по файлу
                    break;
                }
            }

            double[,] arr = new double[finPos - startPos + 1, 2]; //создаем массив под извстный размер раблицы

            //переписываем таблицу в двумерный массив
            int num = 0;
            for (int i = startPos; i <= finPos; i++)
            {
                string[] split = lines[i].Split(new Char[] { ' ', ' ', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    arr[num, 0] = Convert.ToDouble(split[0]);
                    arr[num, 1] = Convert.ToDouble(split[3]);

                num++;
            }
            return arr;
        }

        public static Tuple <string[,], List<string>, List<int>> NomFinder(string parth) //выполняет поиск номиналов по файлу .net 
        {
            string strTarget = "Netlist"; //искомый текст
            string strTarget2 = "PRINT"; //искомый текст 2 
            int startPos = 0; //начальная позиция таблицы АЧХ 
            int finPos = 0; //конечная позиция ачх
            //string parth = "C:\\Cadence\\Projects\\aaa\\A3.out";
            FileInfo fileInf = new FileInfo(parth);

            string[] lines = System.IO.File.ReadAllLines(@parth).Take(System.IO.File.ReadAllLines(@parth).Length).ToArray(); //переписывает файл в массив по строкам
            //ищем по строкам где начинается и заканчивается таблица ачх
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].IndexOf(strTarget, 0) != -1)
                {
                    startPos = i + 4; //сдвигаем на 3 строки вниз т.к. значения находятся ниже шапки
                }
                if (lines[i].IndexOf(strTarget2, 0) != -1)
                {
                    finPos = i - 2; //сдвигаем на 3 строки вверх т.к. значения заканчиваются вверх по файлу
                    break;
                }
                else
                if (i == lines.Length-1) finPos = lines.Length - 1;
            }

            string[,] arr = new string[finPos - startPos + 1, 7]; //создаем массив под извстный размер раблицы
            
            List<string> nameList = new List<string>(); //список имен R C компонентов по порядку
            List<int> numbList = new List<int>();  //список номеров позициий R C компонентов в таблице
            //переписываем таблицу в двумерный массив
            int num = 0;
            for (int i = startPos; i <= finPos; i++)
            {
                string[] split = lines[i].Split(new Char[] { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < 7; j++)
                {
                    if (j > split.Length - 1)
                        arr[num, j] = " ";
                    else arr[num, j] = split[j];
                }
                if ((split[0].IndexOf("R_R", 0) != -1) || (split[0].IndexOf("C_C", 0) != -1))
                {
                    nameList.Add(split[0]);
                    numbList.Add(num);
                }

                num++;
            }

            //тестовый вывод в файл таблицы номиналов

            return Tuple.Create(arr, nameList, numbList);
        }

        public static void ToNetFile(string path, string[,] allStrings, List<int> rcNumbers, int[] checks)
        {
            for (int i = 0; i < checks.Length; i++)
            {
                if (checks[i] == 1)
                {
                    allStrings[rcNumbers[i], 3] = "7000007";
                }
            }


                using (StreamWriter sw = new StreamWriter(path))
                {
                    for (int i = 0; i < allStrings.GetLength(0); i++)
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            sw.Write(String.Format("{0} ", allStrings[i, j]));
                        }
                        sw.WriteLine();
                    }
                    sw.Close();
                }
        }


        [HttpPost]
        public JsonResult Config(bool[] first, double[] second)
        {
            checks = new int[first.Length];
            genSet = second;
            for (int i = 0; i < first.Length; i++)
                checks[i] = Convert.ToInt32(first[i]);
            //string fin = "<h1>TEST</h1>";
            //for (int i = 0; i < checks.Count; i++)
            //{
            //    fin += checks[i] + "  -  ";
            //}
            //return View("Main");
            return Json("OK");
        }

        public ActionResult About()
        {

            // передаем все объекты в динамическое свойство Books в ViewBag
            ViewBag.Title = "123";
            ViewBag.Message = "456";

            // возвращаем представление
            return View();
        }

    }
}