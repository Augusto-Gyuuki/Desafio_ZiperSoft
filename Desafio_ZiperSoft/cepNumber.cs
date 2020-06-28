using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio_ZiperSoft
{
    //objeto para converter json 
    class cepNumber
    {
        string logradouro;
        string bairro;
        string localidade;
        string uf;

        public string Logradouro { get => logradouro; set => logradouro = value; }
        public string Bairro { get => bairro; set => bairro = value; }
        public string Localidade { get => localidade; set => localidade = value; }
        public string Uf { get => uf; set => uf = value; }
    }
}

