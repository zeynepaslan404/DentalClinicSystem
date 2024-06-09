using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DentalClinicSystem.Models;
using DentalClinicSystem.Services;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System;

namespace DentalClinicSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            RunDentalClinicSystem(host.Services);
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    services.Configure<DatabaseSettings>(
                        context.Configuration.GetSection(nameof(DatabaseSettings)));

                    services.AddSingleton<IDatabaseSettings>(sp =>
                        sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

                    services.AddSingleton<PatientService>();
                    services.AddSingleton<DentistService>();
                    services.AddSingleton<AppointmentService>();
                    services.AddSingleton<TreatmentService>();
                });

        static void RunDentalClinicSystem(IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var patientService = scope.ServiceProvider.GetRequiredService<PatientService>();
                var dentistService = scope.ServiceProvider.GetRequiredService<DentistService>();
                var appointmentService = scope.ServiceProvider.GetRequiredService<AppointmentService>();
                var treatmentService = scope.ServiceProvider.GetRequiredService<TreatmentService>();

                // Veritabanını oluştur ve veri ekle
                var patient1 = new Patient
                {
                    Name = "Ahmet Yılmaz",
                    Email = "ahmet.yilmaz@example.com",
                    Phone = "1234567890",
                    MedicalHistory = "Alerji yok",
                    AppointmentIds = new List<string>()
                };

                var dentist1 = new Dentist
                {
                    Name = "Dr. Mehmet Öz",
                    Specialty = "Ortodonti",
                    AppointmentIds = new List<string>()
                };

                patientService.Create(patient1);
                dentistService.Create(dentist1);

                var appointment1 = new Appointment
                {
                    PatientId = patient1.Id,
                    DentistId = dentist1.Id,
                    AppointmentDate = DateTime.Now.AddDays(1),
                    TreatmentPlan = "Diş teli takılması",
                    TreatmentIds = new List<string>()
                };

                appointmentService.Create(appointment1);

                var treatment1 = new Treatment
                {
                    Name = "Diş Teli",
                    Description = "Ortodontik diş teli takılması",
                    Cost = 5000,
                    AppointmentId = appointment1.Id
                };

                treatmentService.Create(treatment1);

                // Randevuya tedavi ekle
                appointment1.TreatmentIds.Add(treatment1.Id);
                appointmentService.Update(appointment1.Id, appointment1);

                // Hastaya ve diş hekimine randevu ekle
                patient1.AppointmentIds.Add(appointment1.Id);
                dentist1.AppointmentIds.Add(appointment1.Id);
                patientService.Update(patient1.Id, patient1);
                dentistService.Update(dentist1.Id, dentist1);

                var appointments = appointmentService.Get();

                foreach (var appointment in appointments)
                {
                    PrintAppointmentDetails(appointment, patientService, dentistService, treatmentService);
                }
            }
        }

        static void PrintAppointmentDetails(Appointment appointment, PatientService patientService, DentistService dentistService, TreatmentService treatmentService)
        {
            var patient = patientService.Get(appointment.PatientId);
            var dentist = dentistService.Get(appointment.DentistId);

            Console.WriteLine($"Randevu ID: {appointment.Id}");
            Console.WriteLine($"Hasta: {patient.Name}");
            Console.WriteLine($"Diş Hekimi: {dentist.Name}");
            Console.WriteLine($"Uzmanlık: {dentist.Specialty}");
            Console.WriteLine($"Randevu Tarihi: {appointment.AppointmentDate}");
            Console.WriteLine($"Tedavi Planı: {appointment.TreatmentPlan}");

            foreach (var treatmentId in appointment.TreatmentIds)
            {
                var treatment = treatmentService.Get(treatmentId);
                Console.WriteLine($"  - Tedavi: {treatment.Name}, Açıklama: {treatment.Description}, Maliyet: {treatment.Cost}");
            }
        }
    }
}
