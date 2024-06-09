using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicSystem.Models
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string PatientsCollectionName { get; set; }
        public string DentistsCollectionName { get; set; }
        public string AppointmentsCollectionName { get; set; }
        public string TreatmentsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IDatabaseSettings
    {
        string PatientsCollectionName { get; set; }
        string DentistsCollectionName { get; set; }
        string AppointmentsCollectionName { get; set; }
        string TreatmentsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
