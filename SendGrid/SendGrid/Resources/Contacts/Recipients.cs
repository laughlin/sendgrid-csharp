using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;

namespace SendGrid.Resources.Contacts
{
    public class Recipients
    {
        private string _endpoint;
        private Client _client;

        /// <summary>
        /// Constructs the SendGrid Marketing Campaign object.
        /// See https://sendgrid.com/docs/API_Reference/Web_API_v3/Marketing_Campaigns/contactdb.html
        /// </summary>
        /// <param name="client">SendGrid Web API v3 client</param>
        /// <param name="endpoint">Resource endpoint, do not prepend slash</param>
        public Recipients(Client client, string endpoint = "v3/contactdb/recipients")
        {
            _endpoint = endpoint;
            _client = client;
        }
        public async Task<HttpResponseMessage> Post(Models.Contacts.Recipient recipient)
        {
            return await Post(new[] { recipient });
        }
        public async Task<HttpResponseMessage> Post(IEnumerable<Models.Contacts.Recipient> recipients)
        {
            // todo: use serilize?
            var data = new JObject();
            var innerArray = new JArray();
            foreach (var recipient in recipients)
            {
                var innerData = new JObject(
                        new JProperty("first_name", recipient.FirstName),
                        new JProperty("last_name", recipient.LastName),
                        new JProperty("email", recipient.Email));
                foreach (var field in recipient.CustomFields)
                {
                    innerData.Add(new JProperty(field.Name, field.Value));
                }
                innerArray.Add(innerData);
            }
            data.Add(innerArray);

            return await _client.Post(_endpoint, data);
        }

        public async Task<HttpResponseMessage> Get(int page = 1, int pageSize = 100)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["page"] = page.ToString();
            query["pageSize"] = pageSize.ToString();
            return await _client.Get(string.Concat(_endpoint, "?", query));
        }
        public async Task<HttpResponseMessage> Get(int recipientId)
        {
            return await _client.Get(string.Concat(_endpoint, "/", recipientId));
        }
        public async Task<HttpResponseMessage> Delete(int recipientId)
        {
            return await _client.Delete(string.Concat(_endpoint, "/", recipientId));
        }
        public async Task<HttpResponseMessage> Find(string field, string value)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query[field] = value;
            return await _client.Get(string.Concat(_endpoint, "?", query));
        }
        public async Task<HttpResponseMessage> Find(string email)
        {
            return await Find("email", email);
        }
    }
}
