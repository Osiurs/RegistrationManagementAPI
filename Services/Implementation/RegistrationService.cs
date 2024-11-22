using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Repositories.Interface;
using RegistrationManagementAPI.Services.Interface;

namespace RegistrationManagementAPI.Services.Implementation
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRegistrationRepository _registrationRepository;

        public RegistrationService(IRegistrationRepository registrationRepository)
        {
            _registrationRepository = registrationRepository;
        }

        public async Task<IEnumerable<Registration>> GetAllRegistrationsAsync()
        {
            return await _registrationRepository.GetAllRegistrationsAsync();
        }

        public async Task<Registration> GetRegistrationByIdAsync(int id)
        {
            var registration = await _registrationRepository.GetRegistrationByIdAsync(id);
            if (registration == null)
            {
                throw new InvalidOperationException("Registration not found.");
            }
            return registration;
        }

        public async Task<IEnumerable<Registration>> GetRegistrationsByStudentIdAsync(int studentId)
        {
            return await _registrationRepository.GetRegistrationsByStudentIdAsync(studentId);
        }

        public async Task<IEnumerable<Registration>> GetRegistrationsByCourseIdAsync(int courseId)
        {
            return await _registrationRepository.GetRegistrationsByCourseIdAsync(courseId);
        }

        public async Task<Registration> AddRegistrationAsync(Registration registration)
        {
            if (registration.RegistrationDate < DateTime.UtcNow)
            {
                throw new ArgumentException("Registration date cannot be in the past.");
            }

            return await _registrationRepository.AddRegistrationAsync(registration);
        }

        public async Task UpdateRegistrationAsync(int id, Registration registration)
        {
            var existingRegistration = await _registrationRepository.GetRegistrationByIdAsync(id);
            if (existingRegistration == null)
            {
                throw new InvalidOperationException("Registration not found.");
            }

            existingRegistration.CourseId = registration.CourseId;
            existingRegistration.Status = registration.Status;
            existingRegistration.RegistrationDate = registration.RegistrationDate;

            await _registrationRepository.UpdateRegistrationAsync(existingRegistration);
        }

        public async Task DeleteRegistrationAsync(int id)
        {
            var registration = await _registrationRepository.GetRegistrationByIdAsync(id);
            if (registration == null)
            {
                throw new InvalidOperationException("Registration not found.");
            }

            await _registrationRepository.DeleteRegistrationAsync(id);
        }
    }
}
