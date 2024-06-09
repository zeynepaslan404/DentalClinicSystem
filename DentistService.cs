using System;
using DentalClinicSystem.Models;
using MongoDB.Driver;
using System.Collections.Generic;

namespace DentalClinicSystem.Services
{
    public class DentistService
    {
        private readonly IMongoCollection<Dentist> _dentists;

        public DentistService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _dentists = database.GetCollection<Dentist>(settings.DentistsCollectionName);
        }

        public List<Dentist> Get() =>
            _dentists.Find(dentist => true).ToList();

        public Dentist Get(string id) =>
            _dentists.Find<Dentist>(dentist => dentist.Id == id).FirstOrDefault();

        public Dentist Create(Dentist dentist)
        {
            _dentists.InsertOne(dentist);
            return dentist;
        }

        public void Update(string id, Dentist dentistIn) =>
            _dentists.ReplaceOne(dentist => dentist.Id == id, dentistIn);

        public void Remove(Dentist dentistIn) =>
            _dentists.DeleteOne(dentist => dentist.Id == dentistIn.Id);

        public void Remove(string id) =>
            _dentists.DeleteOne(dentist => dentist.Id == id);
    }
}

