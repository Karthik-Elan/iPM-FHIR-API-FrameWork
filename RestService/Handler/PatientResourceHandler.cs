using System;
using System.Net;
using Hl7.Fhir.Model;
using FHIRDataMapper;
using System.ServiceModel.Web;
using Hl7.Fhir.Serialization;
using iPM_API_Resource = iSOFT.ANZ.PatientManagerServiceLibrary;
using Model;

namespace RestService.Handler
{
    public class PatientResourceHandler : IResourceHandler
    {
        private const string PATIENT = "Patient";
        private readonly IRepository _repository;
        

        public PatientResourceHandler(IRepository repository)
        {
            _repository = repository;
        }


        private String Serialize(iSOFT.ANZ.PatientManagerServiceLibrary.Patient iPMpatient)
        {
            var resource = PatientMapper.MapModel(iPMpatient);

            String payload = String.Empty;
            if (WebOperationContext.Current != null)
            {
                var response = WebOperationContext.Current.OutgoingResponse;

                response.LastModified = iPMpatient.ModifDttm;
                string accept = WebOperationContext.Current.IncomingRequest.Accept;
                if (!String.IsNullOrEmpty(accept) && accept == "application/json")
                {
                    payload = FhirSerializer.SerializeResourceToJson(resource);
                    response.ContentType = "application/json+fhir";
                }
                else
                {
                    payload = FhirSerializer.SerializeResourceToXml(resource);
                    response.ContentType = "application/xml+fhir";
                }
            }
            return payload;
        }



        public HttpStatusCode Create(string format, string data)
        {
            throw new NotImplementedException();
        }

        public HttpStatusCode Delete(string patid)
        {
            throw new NotImplementedException();
        }

        public string List()
        {
            throw new NotImplementedException();
        }

        public Conformance.ResourceComponent Metadata()
        {

            var patientConformance = new Conformance.ResourceComponent();
            patientConformance.Type = ResourceType.Patient;
            patientConformance.ReadHistory = false;
            patientConformance.Interaction = new System.Collections.Generic.List<Conformance.ResourceInteractionComponent>
            {
                //  supported functions 
                new Conformance.ResourceInteractionComponent
                {
                    Code = Conformance.TypeRestfulInteraction.Read
                },
                new Conformance.ResourceInteractionComponent
                {
                    Code = Conformance.TypeRestfulInteraction.SearchType
                }
            };

            return patientConformance;

        }

        public string Read(string patid)
        {
            String payload = String.Empty;
            //  iPM_API_Resource.Patient iPmPatient;
            try
            {
                int id = Int32.Parse(patid);

                // Call the iPM pat Service to get the patient 
                iPM_API_Resource.PatientService ps = new iPM_API_Resource.PatientService();
                iPM_API_Resource.Patient pat = ps.GetByPatientIdentifier(patid, iPM_API_Resource.PatientLoadDepth.PatientOnly);

                if (pat != null )
                {
                    var resource = FHIRDataMapper.PatientMapper.MapModel(pat);

                    if (WebOperationContext.Current != null)
                    {
                        var response = WebOperationContext.Current.OutgoingResponse;

                        response.LastModified = DateTime.Now;
                        string accept = WebOperationContext.Current.IncomingRequest.Accept;
                        if (!String.IsNullOrEmpty(accept) && accept == "application/json")
                        {
                            payload = FhirSerializer.SerializeResourceToJson(resource);
                            response.ContentType = "application/json+fhir";
                        }
                        else
                        {
                            payload = FhirSerializer.SerializeResourceToXml(resource);
                            response.ContentType = "application/xml+fhir";
                        }
                    }
                    return payload;
                }
                return payload;
            }
            catch(Exception exp)
            {
                throw exp;
            }
        }

        public string Type()
        {
            return PATIENT;
        }

        public HttpStatusCode Update(string patid, string format, string data)
        {
            throw new NotImplementedException();
        }

        public string VRead(string patid, string version)
        {
            throw new NotImplementedException();
        }
    }
}