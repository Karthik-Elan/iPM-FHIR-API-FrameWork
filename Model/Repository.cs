using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using iPMServiceClient.iPMDataService;

namespace Model
{
    public class Repository: IRepository
    {
        void IRepository.CreatePatient(Patient patient)
        {
            throw new NotImplementedException();
        }

        bool IRepository.DeletePatient(int patid)
        {
            throw new NotImplementedException();
        }

        List<Patient> IRepository.GetPatientHistory(Patient patient)
        {
            throw new NotImplementedException();
        }

        List<Patient> IRepository.GetPatientHistory(int patid)
        {
            throw new NotImplementedException();
        }

        Patient IRepository.ReadPatient(int id)
        {
            throw new NotImplementedException();
        }

        Patient IRepository.ReadPatientHistory(int patid, int version)
        {
            throw new NotImplementedException();
        }

        List<Patient> IRepository.SearchByName(string name)
        {
            throw new NotImplementedException();
        }

        void IRepository.UpdatePatient(Patient patient)
        {
            throw new NotImplementedException();
        }
    }
}
