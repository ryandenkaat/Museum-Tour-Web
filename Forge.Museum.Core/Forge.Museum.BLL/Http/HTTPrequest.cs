using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace Forge.Museum.BLL.Http
{
    public class HTTPrequest
    {
        private string APIurl = WebConfigurationManager.AppSettings["APIurl"];
        private static HttpClient httpClient;

        HttpClient GetClient()
        {
            HttpClient client = null;

            if (httpClient == null)
            {
                client = new HttpClient()
                {
                    BaseAddress = new Uri(APIurl)
                };

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
            else
            {
                client = httpClient;
            }

            return client;
        }

        public async Task<T> Get<T>(string endPoint)
        {
            try
            {
                HttpClient client = GetClient();

                HttpResponseMessage response = await client.GetAsync(endPoint);

                return await ParseResponse<T>(response);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<T> Post<T>(string endPoint, object body)
        {
            try
            {
                HttpClient client = GetClient();

                string json = JsonConvert.SerializeObject(body);

                HttpResponseMessage response = await client.PostAsync(endPoint, new StringContent(json, Encoding.Default, "text/json"));

                return await ParseResponse<T>(response);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<T> Put<T>(string endPoint, object body)
        {
            try
            {
                HttpClient client = GetClient();

                string json = JsonConvert.SerializeObject(body);

                HttpResponseMessage response = await client.PutAsync(endPoint, new StringContent(json, Encoding.Default, "text/json"));

                return await ParseResponse<T>(response);

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<bool> Delete(string endPoint)
        {
            try
            {
                HttpClient client = GetClient();

                HttpResponseMessage response = await client.DeleteAsync(endPoint);

                return response.IsSuccessStatusCode;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        #region Helper Methods
        async Task<T> ParseResponse<T>(HttpResponseMessage httpResponse)
        {
            if (httpResponse.IsSuccessStatusCode)
            {
                try
                {
                    var data = await httpResponse.Content.ReadAsStringAsync();
                    var settings = new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    };

                    var response = JsonConvert.DeserializeObject<T>(data, settings);

                    return response;
                }
                catch (Exception exception)
                {
                    return default(T);
                }
            }
            else if (httpResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new HttpRequestException();
            }
            else
            {
                string contentMsg = "";

                try
                {
                    contentMsg = await httpResponse.Content.ReadAsStringAsync();
                }
                catch (Exception e)
                {
                    throw new HttpException($"{contentMsg}: {httpResponse.StatusCode} {httpResponse.ReasonPhrase}", e);
                }

                throw new HttpException($"{contentMsg}: {httpResponse.StatusCode} {httpResponse.ReasonPhrase}");

            }
        }
        #endregion
    }
}
