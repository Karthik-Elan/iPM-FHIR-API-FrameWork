using Hl7.Fhir.Serialization;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace RestService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RestServiceImpl" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RestServiceImpl.svc or RestServiceImpl.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class RestServiceImpl : IRestServiceImpl
    {
        private static Registry _registry;
        public RestServiceImpl(IRepository repository)
        {
            _registry = new Registry(repository);
        }

        public string JsonData(string id)
        {
            return " You have requested xml data for " + id;
        }

        public string XmlData(string id)
        {
            return " You have requested Json data for " + id;
        }
        public Stream Conformance()
        {
            var conformance = _registry.MetaData();
            if (WebOperationContext.Current != null)
            {
                string accept = WebOperationContext.Current.IncomingRequest.Accept;
                string payload;
                if (!String.IsNullOrEmpty(accept) && accept == "application/json")
                {
                    payload = FhirSerializer.SerializeResourceToJson(conformance);
                }
                else
                {
                    payload = FhirSerializer.SerializeResourceToXml(conformance);
                }
                return new MemoryStream(Encoding.UTF8.GetBytes(payload));
            }
            return new MemoryStream();



        }

        public Stream Create(string type, string format, Stream data)
        {

           // Stream test;
            

            throw new NotImplementedException();
        }

        public Stream Delete(string type, string id)
        {
            throw new NotImplementedException();
        }

        public Stream History(string type, string id)
        {
            throw new NotImplementedException();
        }

        public Stream HistoryAll()
        {
            throw new NotImplementedException();
        }

        public Stream HistoryType(string type)
        {
            throw new NotImplementedException();
        }

     
        public Stream Read(string type, string id)
        {
            IResourceHandler handler = _registry.GetHandler(type);
            if (handler == null)
            {
                return ErrorMessage(HttpStatusCode.NotImplemented, String.Format("Get resource of type {0} not implemented.", type));
            }

            try
            {
                string payload = handler.Read(id);
                if (String.IsNullOrEmpty(payload))
                {
                    return ErrorMessage(HttpStatusCode.NotFound, String.Format("{1} resource {0} not found", id, type));
                }
                return new MemoryStream(Encoding.UTF8.GetBytes(payload));
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(NotImplementedException))
                {
                    return ErrorMessage(HttpStatusCode.NotImplemented, e.Message);
                }
                return ErrorMessage(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        public Stream Search(string type)
        {
            throw new NotImplementedException();
        }

        public Stream SearchAll()
        {
            throw new NotImplementedException();
        }

        public Stream Transaction()
        {
            throw new NotImplementedException();
        }

        public Stream Update(string type, string id, string format, Stream data)
        {
            throw new NotImplementedException();
        }

        public Stream Validate(string type, string id, string format, Stream data)
        {
            throw new NotImplementedException();
        }

        public Stream ValidateType(string type, string format, Stream data)
        {
            throw new NotImplementedException();
        }

        public Stream Vread(string type, string id, string vid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Errors the message.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="message">The message.</param>
        /// <returns>Stream.</returns>
        /// <remarks>N/A</remarks>
        private Stream ErrorMessage(HttpStatusCode statusCode, string message)
        {
            if (WebOperationContext.Current != null)
            {
                WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
                WebOperationContext.Current.OutgoingResponse.StatusCode = statusCode;
            }
            return new MemoryStream(Encoding.UTF8.GetBytes(message));
        }

    }
}
