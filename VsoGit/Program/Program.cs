using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using VsoGit;
using System.IO;

namespace Program
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.Write("User name to access:");
            string userName = ReadString();
            Console.WriteLine();

            Console.Write("Password to access:");
            SecureString password = ReadPassword();
            Console.WriteLine();

            Console.Write("VSO account:");
            string vsoAccount = ReadString();
            Console.WriteLine();

            Console.Write("VSO Project:");
            string vsoProject = ReadString();
            Console.WriteLine();

            Console.Write("File path to write result:");
            Guid g = Guid.NewGuid();
            string resultFile = ReadString(); // string.Format(@"C:\PerfLogs\pr_{0}.csv", g.ToString());
            Console.WriteLine();

            var repoTask = VsoRestClient.GetVsoRepository(
                userName,
                SecureString2PlainText(password),
                vsoAccount,
                vsoProject);

            List<VsoRepository> repos = repoTask.Result;

            using (StreamWriter sw = new StreamWriter(resultFile))
            {
                sw.WriteLine("ID,CreatedBy,Repository,URL,ReviewLink");

                foreach (var repo in repos)
                {
                    var prTask = VsoRestClient.GetVsoPullRequest(
                        userName,
                        SecureString2PlainText(password),
                        vsoAccount,
                        repo.Id,
                        "Active");

                    foreach (var pr in prTask.Result)
                    {
                        string reviewLink = string.Format(
                                "https://{0}.visualstudio.com/DefaultCollection/{1}/_git/{2}/pullrequest/{3}#view=discussion",
                                vsoAccount,
                                vsoProject,
                                repo.Name,
                                pr.PullRequestId);
                        sw.WriteLine(
                            string.Format(
                                "{0},{1},{2},{3},{4}",
                                pr.PullRequestId,
                                pr.CreatedBy.DisplayName,
                                repo.Name,
                                pr.Url,
                                reviewLink));
                        Console.WriteLine("ID:\t{0}", pr.PullRequestId);
                        Console.WriteLine("CreatedBy:\t{0}", pr.CreatedBy.DisplayName);
                        Console.WriteLine("Repository:\t{0}", repo.Name);
                        Console.WriteLine("URL:\t{0}", pr.Url);
                        Console.WriteLine(
                            "Review link:\t{0}", reviewLink);
                        Console.WriteLine();
                    }
                }

                sw.Close();
            }

            Console.ReadKey();
        }

        public static string ReadString()
        {
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                ConsoleKeyInfo info = Console.ReadKey(true);
                if (info.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (sb.Length > 0)
                    {
                        sb.Remove(sb.Length - 1, 1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    sb.Append(info.KeyChar);
                    Console.Write(info.KeyChar);
                }
            }
            return sb.ToString();
        }

        public static SecureString ReadPassword(char mask = '*')
        {
            var password = new SecureString();
            while (true)
            {
                ConsoleKeyInfo info = Console.ReadKey(true);
                if (info.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password.RemoveAt(password.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    password.AppendChar(info.KeyChar);
                    Console.Write(mask);
                }
            }
            return password;
        }

        public static string SecureString2PlainText(SecureString secureString)
        {
            IntPtr ptr = new IntPtr();
            ptr = Marshal.SecureStringToBSTR(secureString);
            string plainText = Marshal.PtrToStringBSTR(ptr);
            Marshal.ZeroFreeBSTR(ptr);
            return plainText;
        }
    }
}
