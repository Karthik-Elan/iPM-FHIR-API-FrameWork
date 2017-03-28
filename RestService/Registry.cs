using Hl7.Fhir.Model;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestService
{
    public class Registry
    {
        private readonly Dictionary<string, IResourceHandler> _servants;
        private readonly IRepository _repository;


        public Registry(IRepository repository)
        {
            _repository = repository;

            // Wire up all available handlers
            _servants = new Dictionary<string, IResourceHandler>();

            // Add all the type of FHIR we need to implement 
            var patientHandler = new Handler.PatientResourceHandler(repository);
            _servants.Add(patientHandler.Type(), patientHandler);

        }


        public IResourceHandler GetHandler(String type)
        {
            if (_servants.ContainsKey(type))
            {
                return _servants[type];
            }
            return null;
        }

        public Conformance MetaData()
        {
            Conformance thisConformance = new Conformance();

            thisConformance.Description = "This is an test implementation of FHIR server which wrappes around the existing iPM APIs Using DSTU 2";
            thisConformance.Date = DateTime.UtcNow.ToString("s");
            thisConformance.Experimental = true;
            thisConformance.AcceptUnknown = Conformance.UnknownContentCode.No;
            thisConformance.FhirVersion = "Fhir.DSTU2 0.91.1.1";
            thisConformance.Name = "FHIR Server for Patient Resources";
            thisConformance.Rest = new List<Conformance.RestComponent>();


            Conformance.RestComponent rc = new Conformance.RestComponent()
            {
                Mode = Conformance.RestfulConformanceMode.Server,
                Resource = new List<Conformance.ResourceComponent>()
            };

            foreach (var servant in _servants.Values)
            {
                rc.Resource.Add(servant.Metadata());
            }
            thisConformance.Rest.Add(rc);

            return thisConformance;

        }
    }
}