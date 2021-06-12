using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.DTOs
{
    public class PrefeiturasConfigDTO
    {
        public PrefeiturasConfigDTO()
        { 
            Consultas = new List<Consulta>();
        }
        public List<Consulta> Consultas { get; set; }
    }

    public class Consulta
    {
        public string TipoConsulta { get; set; }
        public string Metodo { get; set; }
        public string DiretorioDownload { get; set; }
        public string TimeExecution { get; set; }
        public int PeriodoDeConsulta { get; set; }
        public bool Executar { get; set; }
    }

}
