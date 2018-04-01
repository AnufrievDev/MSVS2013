using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OptSystem.Models
{
    public class Param
    {
        // ID элемента
        public int Id { get; set; }
        // название элемента
        public string Name { get; set; }
        // атрибут  изменения
        public bool Change { get; set; }
        // значение элемента
        public int Value { get; set; }
    }
}

