using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VsoGit
{
    public class VsoRepositoryProperty
    {
        [JsonProperty("count")]
        public uint Count { get; set; }
        [JsonProperty("value")]
        public List<VsoRepository> Value { get; set; }
    }

    public class VsoRepository
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("project")]
        public VsoProject Project { get; set; }
        [JsonProperty("defaultBranch")]
        public string DefaultBranch { get; set; }
        [JsonProperty("remoteUrl")]
        public string RemoteUrl { get; set; }
    }

    public class VsoProject
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
    }

    public class VsoPullRequestProperty
    {
        [JsonProperty("count")]
        public uint Count { get; set; }
        [JsonProperty("value")]
        public List<VsoPullRequest> Value { get; set; }
    }

    public class VsoPullRequest
    {
        [JsonProperty("pullRequestId")]
        public string PullRequestId { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("createdBy")]
        public VsoUser CreatedBy { get; set; }
    }

    public class VsoUser
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
        [JsonProperty("uniqueName")]
        public string UniqueName { get; set; }
    }
}
