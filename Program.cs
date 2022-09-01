using System.Security.Cryptography.X509Certificates;
using System.Text;

const string NOME_CERTIFICADO = "INFORME AQUI SEU CERTIFICADO";

static void ConsultarGTIN()
{
    var handler = new HttpClientHandler();
    handler.ClientCertificates.Add(GetCertificate(NOME_CERTIFICADO));

    var httpClient = new HttpClient(handler);

    var soapXml = @"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""><soap:Body><ccgConsGTIN xmlns=""http://www.portalfiscal.inf.br/nfe/wsdl/ccgConsGtin""><nfeDadosMsg><consGTIN xmlns=""http://www.portalfiscal.inf.br/nfe"" versao=""1.00""><GTIN>7894900530032</GTIN></consGTIN></nfeDadosMsg></ccgConsGTIN></soap:Body></soap:Envelope>";

    var response = httpClient.PostAsync("https://dfe-servico.svrs.rs.gov.br/ws/ccgConsGTIN/ccgConsGTIN.asmx", new StringContent(soapXml, Encoding.UTF8, "text/xml")).Result;

    var content = response.Content.ReadAsStringAsync().Result;

    Console.WriteLine(content);
}

static X509Certificate2 GetCertificate(string certificateName)
{
    X509Store store = new X509Store(StoreLocation.CurrentUser);
    store.Open(OpenFlags.ReadOnly);
    X509Certificate2 certificate = store.Certificates.Find(X509FindType.FindBySubjectName, certificateName, true)[0];
    store.Close();

    return certificate;
}

Console.WriteLine("Testando WebService GTIN NF-e - SVRS");
Console.WriteLine("------------------------------------");
ConsultarGTIN();

Console.ReadKey();