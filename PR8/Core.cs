using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PR8;

namespace MarketPlace
{
    internal class Core  // подключение бд
    {
        public static MarketPlaceNagievEntities Context = new MarketPlaceNagievEntities();  
    }
}
