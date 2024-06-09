using System;
using DentalClinicSystem.Models;
using MongoDB.Driver;
using System.Collections.Generic;

namespace DentalClinicSystem.Services
{
    public class TreatmentService
    {
        private readonly IMongoCollection<Treatment> _treatments;

        public TreatmentService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _treatments = database.GetCollection<Treatment>(settings.TreatmentsCollectionName);
        }

        public List<Treatment> Get() =>
            _treatments.Find(treatment => true).ToList();

        public Treatment Get(string id) =>
            _treatments.Find<Treatment>(treatment => treatment.Id == id).FirstOrDefault();

        public Treatment Create(Treatment treatment)
        {
            _treatments.InsertOne(treatment);
            return treatment;
        }

        public void Update(string id, Treatment treatmentIn) =>
            _treatments.ReplaceOne(treatment => treatment.Id == id, treatmentIn);

        public void Remove(Treatment treatmentIn) =>
            _treatments.DeleteOne(treatment => treatment.Id == treatmentIn.Id);

        public void Remove(string id) =>
            _treatments.DeleteOne(treatment => treatment.Id == id);
    }
}

