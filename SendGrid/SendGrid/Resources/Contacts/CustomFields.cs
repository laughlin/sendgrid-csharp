using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SendGrid.Resources.Contacts
{
    public class CustomFields
    {
        private string _endpoint;
        private Client _client;

        /// <summary>
        /// Constructs the SendGrid Marketing Campaign object.
        /// See https://sendgrid.com/docs/API_Reference/Web_API_v3/Marketing_Campaigns/contactdb.html
        /// </summary>
        /// <param name="client">SendGrid Web API v3 client</param>
        /// <param name="endpoint">Resource endpoint, do not prepend slash</param>
        public CustomFields(Client client, string endpoint = "v3/contactdb/custom_fields")
        {
            _endpoint = endpoint;
            _client = client;
        }

        public async Task<HttpResponseMessage> Get()        {
            return await _client.Get(_endpoint);
        }
        public async Task<HttpResponseMessage> Get(int customFieldId)
        {
            return await _client.Get(string.Concat(_endpoint, "/", customFieldId));
        }

        public async Task<HttpResponseMessage> Post(string name, string type)
        {
            var data = new JObject(
                    new JProperty("name", name),
                    new JProperty("type", type)
            );
            return await _client.Post(_endpoint, data);
        }
        
        public async Task<HttpResponseMessage> Delete(string customFieldId)
        {
            return await _client.Delete(string.Concat(_endpoint, "/", customFieldId));
        }
    }
}
