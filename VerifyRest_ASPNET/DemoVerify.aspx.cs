using RestSharp;
using System;

namespace VerifyRest_ASPNET
{
    /// <summary>
    /// Esempio di utilizzo del servizio WS VERIFY per la verifica e la normalizzazione degli indirizzi italiani 
    /// realizzato da StreetMaster Italia
    /// 
    /// L'end point del servizio è 
    ///     https://streetmaster.streetmaster.it/smrest/webresources/verify
    ///     
    /// Per l'utilizzo registrarsi sul sito http://streetmaster.it e richiedere la chiave per il servizio VERIFY 
    /// Il protocollo di comunicazione e' in formato JSON
    /// Per le comunicazioni REST è utilizzata la libreria opensource RestSharp (http://restsharp.org/)
    /// 
    ///  2016 - Software by StreetMaster (c)
    /// </summary>
    public partial class DemoVerify : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCallVerify_Click(object sender, EventArgs e)
        {

            outArea.Style["Border"] = "none";
            outArea.Style["Border-color"] = "#336600";

            // inizializzazione client del servizio VERIFY
            var clientVerify = new RestSharp.RestClient
            {
                BaseUrl = new Uri("https://streetmaster.streetmaster.it")
            };

            var request = new RestRequest("smrest/webresources/verify", Method.GET)
            {
                RequestFormat = DataFormat.Json
            };

            // valorizzazione input
            // per l'esempio viene valorizzato un insieme minimo dei parametri
            request.AddParameter("Key", txtKey.Text);
            request.AddParameter("Localita", txtComune.Text);
            request.AddParameter("Cap", txtCap.Text);
            request.AddParameter("Provincia", txtProv.Text);
            request.AddParameter("Indirizzo", txtIndirizzo.Text);
            request.AddParameter("Localita2", txtFrazione.Text);
            request.AddParameter("Dug", String.Empty);
            request.AddParameter("Civico", String.Empty);


            // chiamata al servizio
            var response = clientVerify.Execute<VerifyResponse>(request);

            // recupero result
            var outCall = response.Data;

            if (outCall.Norm==1)
            {
                // verifica OK
                txtCap.Text = outCall.Output[0].Cap;
                txtProv.Text= outCall.Output[0].Prov;
                txtComune.Text = outCall.Output[0].Comune;
                txtFrazione.Text = outCall.Output[0].Frazione;
                txtIndirizzo.Text = outCall.Output[0].Indirizzo;
                outArea.InnerHtml = "<p><font color=\"green\">INDIRIZZO VALIDO</font></p>";
            }
            else
            {
                // verifica KO, gestione errore

                // errore di licenza
                if (outCall.CodErr == 997)
                    outArea.InnerHtml = "<p><font color=\"red\">LICENSE KEY NON RICONOSCIUTA</font></p>";
                else if (outCall.CodErr == 123)
                    outArea.InnerHtml = "<p><font color=\"red\">NON E' STATO VALORIZZATO IL COMUNE</font></p>";
                else if (outCall.CodErr == 124)
                    outArea.InnerHtml = "<p><font color=\"red\">COMUNE\\FRAZIONE NON RICONOSCIUTO</font></p>";
                else if (outCall.CodErr == 125)
                {
                    var htmlOut= "<p><font color=\"red\">COMUNE\\FRAZIONE AMBIGUO</font></p>";

                    htmlOut += "<table>";
                    foreach (VerifyCand outElem in outCall.Output)
                    {
                        htmlOut += "<tr><td>";

                        htmlOut += outElem.Cap + " "+ outElem.Comune+ " " + outElem.Prov;
                        if (outElem.Frazione != string.Empty)
                            htmlOut += " - " + outElem.Frazione;
                        htmlOut += "</td></tr>";
                    }
                    htmlOut += "</table>";
                    outArea.InnerHtml = htmlOut;
                }
                else if (outCall.CodErr == 466)
                {
                    txtCap.Text = outCall.Output[0].Cap;
                    txtProv.Text = outCall.Output[0].Prov;
                    txtComune.Text = outCall.Output[0].Comune;
                    txtFrazione.Text = outCall.Output[0].Frazione;
                    outArea.InnerHtml = "<p><font color=\"red\">NON E' STATO VALORIZZATO L'INDIRIZZO</font></p>";
                }
                else if (outCall.CodErr == 467)
                {
                    txtCap.Text = outCall.Output[0].Cap;
                    txtProv.Text = outCall.Output[0].Prov;
                    txtComune.Text = outCall.Output[0].Comune;
                    txtFrazione.Text = outCall.Output[0].Frazione;
                    outArea.InnerHtml = "<p><font color=\"red\">INDIRIZZO NON RICONOSCIUTO</font></p>";
                }
                else if (outCall.CodErr == 468)
                {
                    txtCap.Text = outCall.Output[0].Cap;
                    txtProv.Text = outCall.Output[0].Prov;
                    txtComune.Text = outCall.Output[0].Comune;
                
                    var htmlOut = "<p><font color=\"red\">INDIRIZZO AMBIGUO</font></p>";
                    htmlOut += "<table>";
                    foreach (VerifyCand outElem in outCall.Output)
                    {
                        htmlOut += "<tr><td>";

                        htmlOut += outElem.Cap + " " + outElem.Indirizzo;
                        if (outElem.Frazione != string.Empty)
                            htmlOut += " (" + outElem.Frazione + ")";
                        htmlOut += "</td></tr>";
                    }
                    htmlOut += "</table>";
                    outArea.InnerHtml = htmlOut;
                }
                else if (outCall.CodErr == 455)
                {
                    txtCap.Text = outCall.Output[0].Cap;
                    txtProv.Text = outCall.Output[0].Prov;
                    txtComune.Text = outCall.Output[0].Comune;
                    txtFrazione.Text = outCall.Output[0].Frazione;
                    txtIndirizzo.Text = outCall.Output[0].Indirizzo;
                    outArea.InnerHtml = "<p><font color=\"red\">CAP INCONGRUENTE SU VIA MULTICAP</font></p>";
                }
            }
            outArea.Style["Border"] = "groove";
        }
    }
}