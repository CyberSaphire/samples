using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// https://www.nuget.org/packages/Microsoft.TeamFoundationServer.Client/
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

// https://www.nuget.org/packages/Microsoft.VisualStudio.Services.InteractiveClient/
using Microsoft.VisualStudio.Services.Client;

// https://www.nuget.org/packages/Microsoft.VisualStudio.Services.Client/
using Microsoft.VisualStudio.Services.Common;
using System.Net;

// https://www.visualstudio.com/en-us/integrate/get-started/client-libraries/samples
namespace VsoGit
{
    public class VsoNetSdkClient
    {
        public static void BasicAuthSample(string uri, string userName, string password)
        {
            VssConnection connection = new VssConnection(new Uri(uri), new VssCredentials(new WindowsCredential(new NetworkCredential(userName, password))));
        }
    }
}
