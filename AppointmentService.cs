using System;
using DentalClinicSystem.Models;
using MongoDB.Driver;
using System.Collections.Generic;

namespace DentalClinicSystem.Services
{
    public class AppointmentService
    {
        private readonly IMongoCollection<Appointment> _appointments;

        public AppointmentService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _appointments = database.GetCollection<Appointment>(settings.AppointmentsCollectionName);
        }

        public List<Appointment> Get() =>
            _appointments.Find(appointment => true).ToList();

        public Appointment Get(string id) =>
            _appointments.Find<Appointment>(appointment => appointment.Id == id).FirstOrDefault();

        public Appointment Create(Appointment appointment)
        {
            _appointments.InsertOne(appointment);
            return appointment;
        }

        public void Update(string id, Appointment appointmentIn) =>
            _appointments.ReplaceOne(appointment => appointment.Id == id, appointmentIn);

        public void Remove(Appointment appointmentIn) =>
            _appointments.DeleteOne(appointment => appointment.Id == appointmentIn.Id);

        public void Remove(string id) =>
            _appointments.DeleteOne(appointment => appointment.Id == id);
    }
}

