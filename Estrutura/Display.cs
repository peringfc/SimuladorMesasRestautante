using System;
using System.Collections.Generic;
using System.Text;

namespace PainelMesasLivres.Estrutura
{
    public class Display
    {


        public Painel Letreiro()
        {
            int Max_Mesas = 20;
            int Max_Pessoas_Mesas = 4;
            int Mesas_Salao = Max_Mesas / Max_Pessoas_Mesas;

            Random rnd = new Random();
            

            Painel oPainel = new Painel();
            oPainel.Disponivel = 5;
            oPainel.Reserva = 23;
            oPainel.Ocupacao = rnd.Next(0, 20);
            oPainel.Atendindos = 32;
            int xMesasOcupada = 0;
            int xMesasLivre = 0; 

            try
            {
               xMesasOcupada = Mesas_Salao - (Max_Mesas / oPainel.Ocupacao);
            }
            catch (Exception)
            {
                xMesasOcupada = 0;
            }

            try
            {
               xMesasLivre = Mesas_Salao - xMesasOcupada;
            }
            catch (Exception)
            {
               xMesasLivre = 0;
            }
           

            GeradorNome geradorNome = new GeradorNome();


            List<string> Nomes = geradorNome.GeradorNomes(oPainel.Ocupacao);

            string xMesasOcupadas = string.Empty;
            string xMesasLivres = string.Empty;
            string xListaEsperaFila = string.Empty;

            /// Carregando Mesas Ocupadas
            for (int i = 0; i < xMesasOcupada; i++)
            {
                xMesasOcupadas = xMesasOcupadas + " <img src=\"https://cdn-icons-png.flaticon.com/512/1535/1535044.png\" class=\"float-end\"  alt=\"Free\" width=\"100\" height=\"100\">";
            }

            /// Carregando Mesas Livres
            for (int i = 0; i < xMesasLivre; i++)
            {
                xMesasLivres = xMesasLivres +  "<img src =\"https://cdn-icons-png.flaticon.com/512/1535/1535041.png\" class=\"float-start\"  alt=\"Free\" width=\"100\" height=\"100\"> ";
            }



            int cliente = 0;

           foreach (var item in Nomes)
            {
                cliente++; 
               xListaEsperaFila = xListaEsperaFila + $" <tr><td>{cliente.ToString()}</td><td>{item}</td><td>*</td></tr>"; 
            }


            oPainel.MesasOcupadas = xMesasOcupadas;
            oPainel.MesasLivres = xMesasLivres;
            oPainel.ListaEsperaFila = xListaEsperaFila;

            return oPainel;
        }
    }
}
