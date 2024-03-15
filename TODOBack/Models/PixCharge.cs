using Gerencianet.NETCore.SDK;
using Newtonsoft.Json.Linq;

namespace TODOBack.Models
{
    public class PixCharge
    {
        dynamic Endpoints = new Endpoints(JObject.Parse(File.ReadAllText("credentials.json")));

        public dynamic Execute(string cpf, string fullName)
        {
            var body = new
            {
                calendario = new
                {
                    expiracao = 60
                },
                devedor = new
                {
                    cpf = $"{cpf}",
                    nome = $"{fullName}"
                },
                valor = new
                {
                    original = "0.01"
                },
                chave = "a25d7cf8-cfeb-412a-bd8a-6f87e562dd7c"
            };

            try
            {
                var response = Endpoints.PixCreateImmediateCharge(null, body);

                return response;
            }
            catch (GnException e)
            {
                return e.Message;
            }
        }

        public dynamic Execute(string txid)
        {
            var param = new
            {
                txid
            };

            try
            {
                var response = Endpoints.PixDetailCharge(param);

                return response;
            }
            catch (GnException e)
            {
                return e.Message;
            }
        }

        public dynamic GetQRCode(int locationId)
        {
            var param = new
            {
                id = locationId
            };

            try
            {
                var response = Endpoints.PixGenerateQRCode(param);

                return response;
            }
            catch (GnException e)
            {
                return e.Message;
            }
        }
    }
}
