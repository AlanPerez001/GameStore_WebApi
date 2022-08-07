using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore_WebApi.Utility
{
    [Serializable]

    public class MiExcepcion : Exception
    {
        public MiExcepcion()
        {

        }

        public MiExcepcion(string mensaje)
       : base(mensaje)
        {

        }
    }
}
