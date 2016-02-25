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
    public class Lists
    {
        private string _endpoint;
        private Client _client;

        /// <summary>
        /// Constructs the SendGrid Marketing Campaign object.
        /// See https://sendgrid.com/docs/API_Reference/Web_API_v3/Marketing_Campaigns/contactdb.html
        /// </summary>
        /// <param name="client">SendGrid Web API v3 client</param>
        /// <param name="endpoint">Resource endpoint, do not prepend slash</param>
        public Lists(Client client, string endpoint = "v3/contactdb/lists")
        {
            _endpoint = endpoint;
            _client = client;
        }

        public async Task<HttpResponseMessage> Get()
        {
            return await _client.Get(_endpoint);
        }
        public async Task<HttpResponseMessage> Get(int listId)
        {
            return await _client.Get(string.Concat(_endpoint, "/", listId));
        }
        public async Task<HttpResponseMessage> Recipients(int listId, int page = 1, int pageSize = 100)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["page"] = page.ToString();
            query["pageSize"] = pageSize.ToString();
            return await _client.Get(string.Concat(_endpoint, "/", listId, "?", query));
        }

        public async Task<HttpResponseMessage> Post(string name)
        {
            var data = new JObject(
                    new JProperty("name", name)
            );
            return await _client.Post(_endpoint, data);
        }

        public async Task<HttpResponseMessage> Delete(string listId, bool deleteContacts = true)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["delete_contacts"] = deleteContacts.ToString();

            return await _client.Delete(string.Concat(_endpoint, "/", listId, "?", query));
        }

        public async Task<HttpResponseMessage> Patch(string listId, string name)
        {
            var data = new JObject(
                    new JProperty("name", name)
            );
            return await _client.Patch(string.Concat(_endpoint, "/", listId), data);
        }

        public async Task<HttpResponseMessage> AddToList(int listId, string recipientId)
        {
            return await _client.Post(string.Concat(_endpoint, "/", listId, "/recipient/", recipientId), (JObject)null);
        }
        public async Task<HttpResponseMessage> DeleteFromList(int listId, string recipientId)
        {
            return await _client.Delete(string.Concat(_endpoint, "/", listId, "/recipient/", recipientId));
        }
    }
}
