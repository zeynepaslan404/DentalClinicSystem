using System;
using DentalClinicSystem.Models;
using MongoDB.Driver;
using System.Collections.Generic;

namespace DentalClinicSystem.Services
{
    public class PatientService
    {
        private readonly IMongoCollection<Patient> _patients;

        public PatientService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _patients = database.GetCollection<Patient>(settings.PatientsCollectionName);
        }

        public List<Patient> Get() =>
            _patients.Find(patient => true).ToList();

        public Patient Get(string id) =>
            _patients.Find<Patient>(patient => patient.Id == id).FirstOrDefault();

        public Patient Create(Patient patient)
        {
            _patients.InsertOne(patient);
            return patient;
        }

        public void Update(string id, Patient patientIn) =>
            _patients.ReplaceOne(patient => patient.Id == id, patientIn);

        public void Remove(Patient patientIn) =>
            _patients.DeleteOne(patient => patient.Id == patientIn.Id);

        public void Remove(string id) =>
            _patients.DeleteOne(patient => patient.Id == id);
    }
}

