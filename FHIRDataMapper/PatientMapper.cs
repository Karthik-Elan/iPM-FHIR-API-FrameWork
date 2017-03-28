using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iPM_API_Resource = iSOFT.ANZ.PatientManagerServiceLibrary;
using Hl7.Fhir.Model;




namespace FHIRDataMapper
{
   public class PatientMapper
    {
        public static iPM_API_Resource.Patient  MapResource(Resource resource)
        {
            var source = resource as Hl7.Fhir.Model.Patient;
            if (source == null)
            {
                throw new ArgumentException("Resource in not a HL7 FHIR Patient resouce");
            }
            var iPMpatient = new iPM_API_Resource.Patient();

            iPMpatient.ArchvFlag = (source.Active ?? true).ToString();
            var deceased = source.Deceased as FhirBoolean;
            if (deceased != null)
                iPMpatient.DecsdFlag = (deceased.Value ?? false).ToString();

            iPMpatient.Forename = source.Name[0].Given.FirstOrDefault();
            iPMpatient.Surname = source.Name[0].Family.FirstOrDefault();
            
            iPMpatient.HomeTelephone = new iPM_API_Resource.Address();
            var phone = source.Telecom.FirstOrDefault(t => t.System == ContactPoint.ContactPointSystem.Phone && t.Use == ContactPoint.ContactPointUse.Home);
            if (phone != null)
            {
                decimal PhNo=0;
                decimal.TryParse(phone.Value, out PhNo);
                iPMpatient.HomeTelephone.TlandOid = PhNo;

            }

            //var mobile = source.Telecom.FirstOrDefault(t => t.System == ContactPoint.ContactPointSystem.Phone && t.Use == ContactPoint.ContactPointUse.Mobile);
            //if (mobile != null)
            //    iPMpatient.Mobile = mobile.Value;

            //var email = source.Telecom.FirstOrDefault(t => t.System == ContactPoint.ContactPointSystem.Email && t.Use == ContactPoint.ContactPointUse.Home);
            //if (email != null)
            //    iPMpatient.EMail = email.Value;
            iPMpatient.Sex = new iPM_API_Resource.ReferenceValue();
            // 

          

            var birthday = source.BirthDate;
            iPMpatient.DttmOfBirth = DateTime.Parse(birthday);

            return iPMpatient;


        }


        public static Hl7.Fhir.Model.Patient MapModel(iPM_API_Resource.Patient iPMpatient)
        {
            if (iPMpatient == null)
            {
                throw new ArgumentNullException("patient");
            }

            var resource = new Hl7.Fhir.Model.Patient();

            resource.Id = iPMpatient.Pasid;

            resource.Active =  (iPMpatient.ArchvFlag =="Y") ?true : false;
            resource.Deceased = new FhirBoolean((iPMpatient.DecsdFlag == "Y") ? true : false);

            resource.Name = new List<HumanName>();
            var name = new HumanName()
            {
                Family = new[] { iPMpatient.Surname },
                Given = new[] { iPMpatient.Forename },
                Use = HumanName.NameUse.Official
            };
            resource.Name.Add(name);

            resource.BirthDate = iPMpatient.DttmOfBirth.ToString();

            //switch (iPMpatient.Gender)
            //{
            //    case GenderCode.Female:
            //        resource.Gender = new CodeableConcept("http://hl7.org/fhir/v3/AdministrativeGender", "F", "Female");
            //        break;

            //    case GenderCode.Male:
            //        resource.Gender = new CodeableConcept("http://hl7.org/fhir/v3/AdministrativeGender", "M", "Male");
            //        break;

            //    case GenderCode.Undetermined:
            //        resource.Gender = new CodeableConcept("http://hl7.org/fhir/v3/AdministrativeGender", "U", "Undetermined");
            //        break;

            //    default:
            //        resource.Gender = new CodeableConcept("http://hl7.org/fhir/v3/NullFlavor", "UNK", "Unknown");
            //        break;
            //}

            //resource.Telecom = new List<ContactPoint>
            //{
            //    new ContactPoint() {
            //        Value = iPMpatient.HomeTelephone.TlandOid.ToString(),
            //        System = ContactPoint.ContactPointSystem.Phone,
            //        Use = ContactPoint.ContactPointUse.Home
            //    },
            //    new ContactPoint() {
            //        Value = "0411445547878",
            //        System =  ContactPoint.ContactPointSystem.Phone,
            //        Use = ContactPoint.ContactPointUse.Mobile
            //    },
              
            //};

            //resource.Address = new List<Address>
            //{
            //    new Address()
            //    {
            //        Country = iPMpatient.HomeAddress.County,
            //        City = iPMpatient.HomeAddress.Line1,
            //        PostalCode = iPMpatient.HomeAddress.Pcode,
            //        Line = new[]
            //        {
            //            iPMpatient.HomeAddress.Line1,
            //            iPMpatient.HomeAddress.Line2
            //        }
            //    }
            //};

            // Make use of extensions ...
            //
            resource.Extension = new List<Extension>(1);
            //resource.Extension.Add(new Extension(new Uri("http://www.englishclub.com/vocabulary/world-countries-nationality.htm"),
            //                                       new FhirString("Australia")
            //                                     ));

            return resource;
        }


    }
}
