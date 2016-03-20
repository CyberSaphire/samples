using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace VsoGit
{
    public class VsoRestClient
    {
        public static async void GetRepos(string username, string password, string requestUri)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(
                        System.Text.ASCIIEncoding.ASCII.GetBytes(
                            string.Format("{0}:{1}", username, password))));

                using (HttpResponseMessage response = client.GetAsync(
                            requestUri).Result)
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseBody);
                }
            }
        }
    }
}
