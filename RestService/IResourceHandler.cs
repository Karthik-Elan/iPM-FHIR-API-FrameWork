using System;
using System.Net;
using Hl7.Fhir.Model;

namespace RestService
{
    /// <summary>
    /// This interface will have operation that are allowed on a FHIR resource
    /// </summary>
    public interface IResourceHandler
    {
        String Type();

        Conformance . ResourceComponent Metadata();

        String Read(string patid);
        String VRead(string patid, string version);
        String List();

        HttpStatusCode Create(string format, string data);
        HttpStatusCode Update(string patid, string format, string data);
        HttpStatusCode Delete(String patid);
    }
}