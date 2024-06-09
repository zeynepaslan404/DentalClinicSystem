using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicSystem.Models
{
    public class Appointment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string PatientId { get; set; }
        public string DentistId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string TreatmentPlan { get; set; }
        public List<string> TreatmentIds { get; set; }
    }
}

