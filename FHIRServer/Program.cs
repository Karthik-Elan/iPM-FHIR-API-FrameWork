
using System;
using Model;
using System.Reflection;
using RestService;
using iPM_API_Resource = iSOFT.ANZ.PatientManagerServiceLibrary;
using System.Data.Linq;
using System.ServiceModel.Web;
using System.ServiceModel;

namespace FHIRServer
{
    class Program
    {
        internal static ServiceHost myServiceHost = null;
        static void Main(string[] args)
        {

            string url = String.Format("http://localhost:8733/KarthikServers/fhir");

            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
            var name = Assembly.GetExecutingAssembly().GetName().Name;

            var header = String.Format("FHIR Server - {0} version {1}", name, version);
            var line = new string('-', header.Length);
            Console.WriteLine(header);
            Console.WriteLine(line);
            Console.WriteLine();

            // Describe the Data context
           var dataContext = new DataContext(""); // ENTITY FRAME WORK goes there
           var repository = new Model.Repository();
           var service = new RestServiceImpl( repository);

            WebServiceHost server = new WebServiceHost(service, new Uri(url));

            server.Opened += server_Opened;
            try
            {
                server.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            Console.WriteLine();
            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();


            //  

            //  var service = new RestServiceImpl(;


        }

        /// <summary>
        /// Handles the Opened event of the server control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <remarks>N/A</remarks>
        static void server_Opened(object sender, EventArgs e)
        {
            var server = (WebServiceHost)sender;
            Console.WriteLine("Listening on {0}", server.BaseAddresses[0].OriginalString);
        }


    }
}
