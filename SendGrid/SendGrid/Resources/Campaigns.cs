using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;

namespace SendGrid.Resources
{
    public class Campaigns
    {
        private string _endpoint;
        private Client _client;

        /// <summary>
        /// Constructs the SendGrid Marketing Campaign object.
        /// See https://sendgrid.com/docs/API_Reference/Web_API_v3/Marketing_Campaigns/campaigns.html
        /// </summary>
        /// <param name="client">SendGrid Web API v3 client</param>
        /// <param name="endpoint">Resource endpoint, do not prepend slash</param>
        public Campaigns(Client client, string endpoint = "v3/campaigns")
        {
            _endpoint = endpoint;
            _client = client;
        }

        /// <summary>
        /// Get all campaigns
        /// </summary>
        /// <param name="limit">The maximum number of campaigns to return Default: 10</param>
        /// <param name="offset">The index of the first campaign to return, where 0 is the first campaign Default: 10/param>
        /// <returns>https://sendgrid.com/docs/API_Reference/Web_API_v3/Marketing_Campaigns/campaigns.html</returns>
        public async Task<HttpResponseMessage> Get(int limit = 10, int offset = 0)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["limit"] = limit.ToString();
            query["offset"] = offset.ToString();
            return await _client.Get(_endpoint + "?" + query);
        }

        /// <summary>
        /// Get specific campaign.
        /// </summary>
        /// <param name="campaignId">The id of the campaign</param>
        /// <returns>https://sendgrid.com/docs/API_Reference/Web_API_v3/Marketing_Campaigns/campaigns.html</returns>
        public async Task<HttpResponseMessage> Get(int campaignId)
        {
            return await _client.Get(_endpoint + "/" + campaignId);
        }

        /// <summary>
        /// Delete a campaign.
        /// </summary>
        /// <param name="campaignId">ID of the campaign to delete</param>
        /// <returns>https://sendgrid.com/docs/API_Reference/Web_API_v3/Marketing_Campaigns/campaigns.html</returns>
        public async Task<HttpResponseMessage> Delete(string campaignId)
        {
            return await _client.Delete(_endpoint + "/" + campaignId);
        }

        /// <summary>
        /// Send a campaign.
        /// </summary>
        /// <param name="campaignId">ID of the campaign to send</param>
        /// <returns>https://sendgrid.com/docs/API_Reference/Web_API_v3/Marketing_Campaigns/campaigns.html</returns>
        public async Task<HttpResponseMessage> Send(string campaignId)
        {
            return await SendInternal(campaignId);
        }
        /// <summary>
        /// Send a campaign.
        /// </summary>
        /// <param name="campaignId">ID of the campaign to send</param>
        /// <param name="ticks">Ticks in the future to send the campaign</param>
        /// <returns>https://sendgrid.com/docs/API_Reference/Web_API_v3/Marketing_Campaigns/campaigns.html</returns>
        public async Task<HttpResponseMessage> Send(string campaignId, int ticks)
        {
            return await SendInternal(campaignId, ticks);
        }

        private async Task<HttpResponseMessage> SendInternal(string campaignId, int? ticks = null)
        {
            JObject data = null;
            var endPoint = string.Format("{0}/{1}/schedules", _endpoint, campaignId);
            if (ticks.HasValue)
            {
                data = new JObject { { "send_at", ticks.Value } };
            }
            else
            {
                endPoint += "/now";
            }
            return await _client.Post(endPoint, data);
        }
    }
}
