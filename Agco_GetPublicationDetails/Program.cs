using Agco_GetPublicationDetails.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Agco_GetPublicationDetails
{
    class Program
    {
        public static void Main(string[] args)
        {
            const string userName = "admin";
            const string passWord = "JgJS6vs8vrePFd68aBWXgAK7yYP4yprB";

            Uri serviceUrl = new Uri(@"https://agcodev01.sdlproducts.com/ISHWS/"); // requires ending '/' character
            //https://agcodev01.sdlproducts.com/ISHWS/Wcf/API25/user.svc
            Console.WriteLine("Starting Console application for user: " + userName.ToString());
            Console.WriteLine("Autenticating the user on the specified environment...");
            InfoShareWSHelper infoShareWSHelper = new InfoShareWSHelper(serviceUrl)
            {
                Username = userName,
                Password = passWord
            };

            infoShareWSHelper.Resolve();
            //Issue a token. In other words authenticate
            infoShareWSHelper.IssueToken();

            Console.WriteLine("User " + userName.ToString() + " successfully autenticated on the specified environment.");

            try
            {
                Console.WriteLine("Starting Publication class...");
                getPubDetails.Run(infoShareWSHelper);
                Console.WriteLine("Ended Publication details...");
                Console.WriteLine("Press enter to continue...");
                Console.ReadLine();

                //await exportPublicationSpesificDetails.Run(null, infoShareWSHelper);
                //Console.WriteLine("Ended Publication details...");
                //Console.WriteLine("Press enter to continue...");
                //Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
