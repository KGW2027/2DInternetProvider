using System;
using System.Linq;

namespace IP.Objective
{
    public class Lotto
    {
        private byte[] _winNumbers;

        public Lotto()
        {
            _winNumbers = new byte[6];
        }

        public byte[] BoughtNumbers()
        {
            return RollNumbers();
        }

        public byte[] NewWinNumbers()
        {
            _winNumbers = RollNumbers();
            return _winNumbers;
        }
        
        private byte[] RollNumbers()
        {
            byte[] array = new byte[6];
            Random random = new Random();
            int key = 0;
            byte num;
            while (key < 6)
            {
                num = (byte) random.Next(1, 99);
                if(array.Contains(num)) continue;
                array[key++] = num;
            }
            Array.Sort(array);
            return array;
        }
    }
}