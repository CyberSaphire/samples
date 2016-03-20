using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace VsoGit
{
    public class VsoRestClient
    {
        public static async Task<List<VsoRepository>> GetVsoRepository(string username, string password, string vsoAccount, string vsoProject)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(
                        Encoding.ASCII.GetBytes(
                            string.Format("{0}:{1}", username, password))));

                string requestUri = string.Format(
                    "https://{0}.visualstudio.com/defaultcollection/{1}/_apis/git/repositories?api-version=1.0",
                    vsoAccount,
                    vsoProject);
                using (HttpResponseMessage response = client.GetAsync(
                            requestUri).Result)
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    var vsoRepoProperty = JsonConvert.DeserializeObject<VsoRepositoryProperty>(responseBody);
                    return vsoRepoProperty.Value;

                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="vsoAccount"></param>
        /// <param name="repository"></param>
        /// <param name="status">enum{Active, Abandoned, Completed}</param>
        /// <returns></returns>
        public static async Task<List<VsoPullRequest>> GetVsoPullRequest(string username, string password, string vsoAccount, string repository, string status)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(
                        Encoding.ASCII.GetBytes(
                            string.Format("{0}:{1}", username, password))));

                // https://{instance}/defaultcollection/_apis/git/repositories/{repository}/pullrequests?api-version={version}[&status={string}&creatorid={guid}&reviewerid={guid}&sourcerefname={string}&targetrefname={string}&$top={integer}&$skip={integer}]
                string requestUri =
                    string.Format(
                        "https://{0}.visualstudio.com/defaultcollection/_apis/git/repositories/{1}/pullrequests?api-version=1.0&status={2}",
                        vsoAccount,
                        repository,
                        status);

                using (HttpResponseMessage response = client.GetAsync(requestUri).Result)
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    var vsoPullRequest = JsonConvert.DeserializeObject<VsoPullRequestProperty>(responseBody);
                    return vsoPullRequest.Value;
                }
            }
        }
    }
}
