using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace WynajemAut
{
    public class User
    {
        public string id { get; set; }
        public string Login { get; set; }
        public string Admin { get; set; }

        public string Email { get; set; }

        public string Imie { get; set; }

        public string Nazwisko { get; set; }

        public string NumerTele { get; set; }

        public string PanPani { get; set; }

        public int Ile_aut { get; set; }

    }
}


