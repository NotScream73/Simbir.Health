using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Simbir.Health.Timetable.Configurations;
using Simbir.Health.Timetable.Controllers.DTO;
using Simbir.Health.Timetable.Data;
using Simbir.Health.Timetable.Exceptions;
using Simbir.Health.Timetable.Models;
using Simbir.Health.Timetable.Services.DTO;
using System.Globalization;

namespace Simbir.Health.Timetable.Services
{
    public class TimeTableService : ITimeTableService
    {
        private readonly DataContext _context;
        private readonly ExternalClientService _externalClientService;
        private readonly ExternalServiceBaseUrlConfig _externalServicesConfig;

        public TimeTableService(DataContext context, ExternalClientService externalClientService, IOptions<ExternalServiceBaseUrlConfig> config)
        {
            _context = context;
            _externalClientService = externalClientService;
            _externalServicesConfig = config.Value;
        }

        public async Task CreateTimeTableAsync(TimeTableCreateRequest request, string accessToken)
        {
            if (request.From.Minute % 30 != 0 || request.From.Second != 0
                || request.To.Minute % 30 != 0 || request.To.Second != 0)
            {
                throw new ApiException("Количество минут всегда кратно 30, секунды всегда 0.");
            }

            if (request.To <= request.From)
            {
                throw new ApiException("Дата начала приёма должна быть раньше даты окончания.");
            }

            if ((request.To - request.From).TotalHours > 12)
            {
                throw new ApiException("Пожалейте врача.");
            }

            request.To = request.To.AddMilliseconds(-request.To.Millisecond);
            request.From = request.From.AddMilliseconds(-request.From.Millisecond);

            var doctorUrl = $"{_externalServicesConfig.AccountService}/api/Doctors/{request.DoctorId}";

            var doctor = await _externalClientService
                .SendRequestAsync<DoctorInformationDTO>(accessToken, doctorUrl, HttpMethod.Get, null)
                ?? throw new NotFoundException($"Доктор с ID {request.DoctorId} не найден.");

            var roomUrl = $"{_externalServicesConfig.HospitalService}/api/Hospitals/{request.HospitalId}/Rooms";

            var hospitalInfo = await _externalClientService
                .SendRequestAsync<List<RoomInfoDTO>>(accessToken, roomUrl, HttpMethod.Get, null)
                ?? throw new NotFoundException($"Больница с ID {request.HospitalId} не найдена.");

            if (!hospitalInfo.Any(r => r.Name == request.Room))
            {
                throw new ApiException($"Комната \"{request.Room}\" в больнице с ID {request.HospitalId} не найдена.");
            }

            var timetable = new TimeTableEntity
            {
                HospitalId = request.HospitalId,
                DoctorId = request.DoctorId,
                StartTime = request.From,
                EndTime = request.To,
                RoomId = hospitalInfo.First(r => r.Name.Equals(request.Room)).Id
            };

            await _context.TimeTable.AddAsync(timetable);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTimeTableAsync(int timeTableId, TimeTableCreateRequest request, string accessToken)
        {
            if (request.From.Minute % 30 != 0 || request.From.Second != 0
                || request.To.Minute % 30 != 0 || request.To.Second != 0)
            {
                throw new ApiException("Количество минут всегда кратно 30, секунды всегда 0.");
            }

            if (request.To <= request.From)
            {
                throw new ApiException("Дата начала приёма должна быть раньше даты окончания.");
            }

            if ((request.To - request.From).TotalHours > 12)
            {
                throw new ApiException("Пожалейте врача.");
            }

            if (await _context.Appointments.AnyAsync(i => i.TimetableId == timeTableId))
            {
                throw new ApiException("Есть записавшиеся на приём.");
            }

            request.To = request.To.AddMilliseconds(-request.To.Millisecond);
            request.From = request.From.AddMilliseconds(-request.From.Millisecond);

            var timetable = await _context.TimeTable.FirstOrDefaultAsync(t => t.Id == timeTableId)
                ?? throw new NotFoundException("Запись не найдена.");

            var doctorUrl = $"{_externalServicesConfig.AccountService}/api/Doctors/{request.DoctorId}";

            var doctor = await _externalClientService
                .SendRequestAsync<DoctorInformationDTO>(accessToken, doctorUrl, HttpMethod.Get, null)
                ?? throw new NotFoundException($"Доктор с ID {request.DoctorId} не найден.");

            var roomUrl = $"{_externalServicesConfig.HospitalService}/api/Hospitals/{request.HospitalId}/Rooms";

            var hospitalInfo = await _externalClientService
                .SendRequestAsync<List<RoomInfoDTO>>(accessToken, roomUrl, HttpMethod.Get, null)
                ?? throw new NotFoundException($"Больница с ID {request.HospitalId} не найдена.");

            if (!hospitalInfo.Any(r => r.Name == request.Room))
            {
                throw new ApiException($"Комната \"{request.Room}\" в больнице с ID {request.HospitalId} не найдена.");
            }

            timetable.HospitalId = request.HospitalId;
            timetable.DoctorId = request.DoctorId;
            timetable.StartTime = request.From;
            timetable.EndTime = request.To;
            timetable.RoomId = hospitalInfo.First(r => r.Name.Equals(request.Room)).Id;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteTimeTableAsync(int timeTableId)
        {
            var timeTable = await _context.TimeTable
                .FirstOrDefaultAsync(t => t.Id == timeTableId)
                ?? throw new NotFoundException($"Расписание с ID {timeTableId} не найдено.");

            _context.TimeTable.Remove(timeTable);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDoctorTimeTableAsync(int doctorId)
        {
            var timeTables = await _context.TimeTable
                .Where(t => t.Id == doctorId)
                .ToListAsync();

            if (timeTables.Count == 0)
            {
                throw new NotFoundException($"У доктора с ID {doctorId} нет расписания.");
            }

            _context.TimeTable.RemoveRange(timeTables);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteHospitalTimeTableAsync(int hospitalId)
        {
            var timeTables = await _context.TimeTable
                .Where(t => t.HospitalId == hospitalId)
                .ToListAsync();

            if (timeTables.Count == 0)
            {
                throw new NotFoundException($"В больнице с ID {hospitalId} нет расписания.");
            }

            _context.TimeTable.RemoveRange(timeTables);
            await _context.SaveChangesAsync();
        }

        public async Task<List<HospitalTimeTableInfoDTO>> GetHospitalTimeTableListAsync(int hospitalId, TimeTableFilterDTO filter, string accessToken)
        {
            var query = _context.TimeTable.Where(t => t.HospitalId == hospitalId);

            if (!string.IsNullOrEmpty(filter.From) && DateTimeOffset.TryParseExact(filter.From, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateStart))
            {
                query = query.Where(t => t.StartTime >= dateStart);
            }

            if (!string.IsNullOrEmpty(filter.To) && DateTimeOffset.TryParseExact(filter.To, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateEnd))
            {
                query = query.Where(t => t.EndTime <= dateEnd);
            }

            var list = await query
                .Select(t => new HospitalTimeTableInfoDTO
                {
                    Id = t.Id,
                    DoctorId = t.DoctorId,
                    From = t.StartTime,
                    To = t.EndTime,
                    RoomId = t.RoomId
                }).ToListAsync();

            var roomUrl = $"{_externalServicesConfig.HospitalService}/api/Hospitals/{hospitalId}/Rooms";

            var hospitalInfo = await _externalClientService
                .SendRequestAsync<List<RoomInfoDTO>>(accessToken, roomUrl, HttpMethod.Get, null)
                ?? throw new NotFoundException($"Больница с ID {hospitalId} не найдена.");

            foreach (var item in list)
            {
                item.RoomNumber = hospitalInfo.FirstOrDefault(h => h.Id == item.RoomId)?.Name ?? throw new ApiException($"Комната с ID {item.RoomId} не найдена.");
            }

            foreach (var item in list)
            {
                var doctorUrl = $"{_externalServicesConfig.AccountService}/api/Doctors/{item.DoctorId}";

                var doctor = await _externalClientService
                    .SendRequestAsync<DoctorInformationDTO>(accessToken, doctorUrl, HttpMethod.Get, null)
                    ?? throw new NotFoundException($"Доктор с ID {item.DoctorId} не найден.");

                item.DoctorFullName = doctor.LastName + " " + doctor.FirstName;
            }

            return list;
        }

        public async Task<List<DoctorTimeTableInfoDTO>> GetDoctorTimeTableListAsync(int doctorId, TimeTableFilterDTO filter, string accessToken)
        {
            var query = _context.TimeTable.Where(t => t.DoctorId == doctorId);

            if (!string.IsNullOrEmpty(filter.From) && DateTimeOffset.TryParseExact(filter.From, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateStart))
            {
                query = query.Where(t => t.StartTime >= dateStart);
            }

            if (!string.IsNullOrEmpty(filter.To) && DateTimeOffset.TryParseExact(filter.To, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateEnd))
            {
                query = query.Where(t => t.EndTime <= dateEnd);
            }

            var list = await query
                .Select(t => new DoctorTimeTableInfoDTO
                {
                    Id = t.Id,
                    HospitalId = t.HospitalId,
                    From = t.StartTime,
                    To = t.EndTime,
                    RoomId = t.RoomId
                }).ToListAsync();

            var hospitalIdList = list.Select(i => i.HospitalId).Distinct().ToList();

            foreach (var hospitalId in hospitalIdList)
            {
                var hospitalInfoUrl = $"{_externalServicesConfig.HospitalService}/api/Hospitals/{hospitalId}";

                var hospitalInfo = await _externalClientService
                    .SendRequestAsync<HospitalDetailedInfoDTO>(accessToken, hospitalInfoUrl, HttpMethod.Get, null)
                    ?? throw new NotFoundException($"Больница с ID {hospitalId} не найдена.");

                list.ForEach(l =>
                {
                    if (l.HospitalId == hospitalId)
                    {
                        l.HospitalName = hospitalInfo.Name;
                        l.ContactPhone = hospitalInfo.ContactPhone;
                        l.Address = hospitalInfo.Address;

                        l.RoomNumber = hospitalInfo.RoomList.FirstOrDefault(h => h.Id == l.RoomId)?.Name ?? throw new ApiException($"Комната с ID {l.RoomId} не найдена.");
                    }
                });
            }

            return list;
        }

        public async Task<List<RoomTimeTableInfoDTO>> GetRoomTimeTableListAsync(RoomTimeTableFilterDTO filter, string accessToken)
        {
            var query = _context.TimeTable.Where(t => t.HospitalId == filter.HospitalId);

            if (!string.IsNullOrEmpty(filter.From) && DateTimeOffset.TryParseExact(filter.From, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateStart))
            {
                query = query.Where(t => t.StartTime >= dateStart);
            }

            if (!string.IsNullOrEmpty(filter.To) && DateTimeOffset.TryParseExact(filter.To, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateEnd))
            {
                query = query.Where(t => t.EndTime <= dateEnd);
            }

            var hospitalInfoUrl = $"{_externalServicesConfig.HospitalService}/api/Hospitals/{filter.HospitalId}/Rooms";

            var hospitalInfo = await _externalClientService
                .SendRequestAsync<List<RoomInfoDTO>>(accessToken, hospitalInfoUrl, HttpMethod.Get, null)
                ?? throw new NotFoundException($"Больница с ID {filter.HospitalId} не найдена.");

            if (!hospitalInfo.Any(h => h.Name == filter.RoomNumber))
            {
                throw new ApiException($"Комната {filter.RoomNumber} не найден.");
            }

            var roomId = hospitalInfo.First(h => h.Name == filter.RoomNumber).Id;

            var list = await query
                .Where(t => t.RoomId == roomId)
                .Select(t => new RoomTimeTableInfoDTO
                {
                    Id = t.Id,
                    DoctorId = t.DoctorId,
                    From = t.StartTime,
                    To = t.EndTime,
                }).ToListAsync();

            var doctorIdList = list.Select(l => l.DoctorId).Distinct().ToList();

            foreach (var doctorId in doctorIdList)
            {
                var doctorInfoUrl = $"{_externalServicesConfig.AccountService}/api/Doctors/{doctorId}";

                var doctorUrl = $"{_externalServicesConfig.AccountService}/api/Doctors/{doctorId}";

                var doctor = await _externalClientService
                    .SendRequestAsync<DoctorInformationDTO>(accessToken, doctorUrl, HttpMethod.Get, null)
                    ?? throw new NotFoundException($"Доктор с ID {doctorId} не найден.");

                list.ForEach(l =>
                {
                    if (l.DoctorId == doctorId)
                    {
                        l.DoctorFullName = doctor.LastName + " " + doctor.FirstName;
                    }
                });
            }

            return list;
        }

        public async Task<List<DateTimeOffset>> GetAvailableAppointmentsByTimeTableId(int id)
        {
            var timetable = await _context.TimeTable
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    t.StartTime,
                    t.EndTime
                }).FirstOrDefaultAsync()
                ?? throw new NotFoundException($"Расписание с ID {id} не найдено.");

            var existingAppointments = await _context.Appointments
                .Where(a => a.TimetableId == id)
                .Select(a => a.ReceptionTime)
                .ToListAsync();

            var availableSlots = new List<DateTimeOffset>();
            var slotTime = timetable.StartTime;

            while (slotTime < timetable.EndTime)
            {
                if (!existingAppointments.Contains(slotTime))
                {
                    availableSlots.Add(slotTime);
                }

                slotTime = slotTime.AddMinutes(30);
            }

            return availableSlots;
        }

        public async Task CreateAppointmentAsync(int id, AppointmentCreateRequest request, string accessToken)
        {
            if (request.Time.Minute % 30 != 0 || request.Time.Second != 0)
            {
                throw new ApiException("Количество минут всегда кратно 30, секунды всегда 0.");
            }

            request.Time = request.Time.AddMilliseconds(-request.Time.Millisecond);

            var timetable = await _context.TimeTable
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    t.StartTime,
                    t.EndTime
                }).FirstOrDefaultAsync()
                ?? throw new NotFoundException($"Расписание с ID {id} не найдено.");

            if (request.Time < timetable.StartTime || request.Time >= timetable.EndTime)
            {
                throw new ApiException("Время записи вне диапазона времени приёма врача.");
            }

            var existingAppointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.TimetableId == id && a.ReceptionTime == request.Time);

            if (existingAppointment != null)
            {
                throw new ApiException("Выбранное время приёма уже занято.");
            }

            var meInfoUrl = $"{_externalServicesConfig.AccountService}/api/Accounts/Me";

            var meInfo = await _externalClientService
                .SendRequestAsync<AccountMeInfoResponse>(accessToken, meInfoUrl, HttpMethod.Get, null)
                ?? throw new ApiException("Действие запрещено.");

            var appointment = new Appointment
            {
                TimetableId = id,
                UserId = meInfo.Id,
                ReceptionTime = request.Time
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAppointmentAsync(int id, string accessToken)
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == id)
                ?? throw new NotFoundException($"Запись на приём с ID {id} не найдена.");

            var meInfoUrl = $"{_externalServicesConfig.AccountService}/api/Accounts/Me";

            var meInfo = await _externalClientService
                .SendRequestAsync<AccountMeInfoResponse>(accessToken, meInfoUrl, HttpMethod.Get, null)
                ?? throw new ApiException("Действие запрещено.");

            if (!meInfo.Roles.Any(r => r == "Admin" || r == "Manager") && meInfo.Id != appointment.UserId)
            {
                throw new NotFoundException("Запись не найдена.");
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
        }
    }
}
