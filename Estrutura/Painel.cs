using System;
using System.Collections.Generic;
using System.Text;

namespace PainelMesasLivres.Estrutura
{
    public class Painel
    {
        public int Disponivel { get; set; }
        public int Reserva { get; set; }
        public int Ocupacao { get; set; }
        public int Atendindos { get; set; }
        public string MesasLivres { get; set; }
        public string MesasOcupadas { get; set; }
        public string ListaEsperaFila { get; set; }
 
    }
}
