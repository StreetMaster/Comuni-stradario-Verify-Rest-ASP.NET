using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyRest_ASPNET
{
  /// <summary>
  /// Classe per deserializzazione della risposta JSON del servizio VERIFY
  /// </summary>
    class DetailElem
    {
        public string Regione { get; set; }
        public string ProvEstesa { get; set; }
        public int CodComune { get; set; }
        public int CodStrada { get; set; }
        public string Dug { get; set; }
        public string Toponimo { get; set; }
        public string Civico { get; set; }
        public string ComuneMultiCap { get; set; }
        public string StradaMultiCap { get; set; }

    }

    class AlterElem
    {
        public int Cap { get; set; }
        public int Comune { get; set; }
        public int Dug { get; set; }
        public int Frazione { get; set; }
        public int Prov { get; set; }
        public int Via { get; set; }
    }

    class VerifyCand
    {
        public string Prov { get; set; }
        public string Comune { get; set; }
        public string Frazione { get; set; }
        public string Cap { get; set; }
        public string Indirizzo { get; set; }
        public int ScoreComune { get; set; }
        public int ScoreStrada { get; set; }

        public DetailElem Detail { get; set; }
        public AlterElem Alter { get; set; }
    }

    class VerifyResponse
    {
      
        public int Norm { get; set; }
        public int CodErr { get; set; }
        public int NumCand { get; set; }
        public List<VerifyCand> Output { get; set; }

    }
}
