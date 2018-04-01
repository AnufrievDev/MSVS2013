using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gen
{
    class Individual
    {
        
        double[] _x;
        double _resFunc;
        Random rnd = new Random();
        /*public Individual(double[] x)
        {
            _x = x;
        }*/

        public void setX(double[] x)
        {
            _x = x;
            /*for (int i = 0; i != _x.Count(); i++)
            {
                _x[i] = rnd.Next(0, 100);
            }*/
        }

        
        public double[] getX()
        {
            return _x;
        }
        public double getResultFunc()
        {
            return (_resFunc);
        }
        public void setResultFunc(double result)
        {
            _resFunc = result;
        }
    }
}
