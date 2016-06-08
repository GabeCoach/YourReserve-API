using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YourReservation
{
    public class KeySingleton
    {
        private static KeySingleton _instance;

        protected KeySingleton()
        {

        }

        public static KeySingleton Instance()
        {
            if (_instance == null)
            {
                _instance = new KeySingleton();
            }

            return _instance;
        }

        public string key { get; set; }
    }
}