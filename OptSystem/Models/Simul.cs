using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OptSystem.Models
{
    public class Simul
    {
        // ID симуляции
        public int Id { get; set; }
        // параметры симуляции
        public int[] SimParam { get; set; }
        // результаты симуляции
        public double[,] Result { get; set; }
        // разница с исходным решением
        public int Diff { get; set; }
    }
}