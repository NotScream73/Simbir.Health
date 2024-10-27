using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Simbir.Health.Document.Configurations;
using Simbir.Health.Document.Data;
using Simbir.Health.Document.Exceptions;
using Simbir.Health.Document.Models;
using Simbir.Health.Document.Services.DTO;

namespace Simbir.Health.Document.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly DataContext _context;
        private readonly ExternalClientService _externalClientService;
        private readonly ExternalServiceBaseUrlConfig _externalServicesConfig;
        private readonly IMapper _mapper;

        public HistoryService(DataContext context, ExternalClientService externalClientService, IOptions<ExternalServiceBaseUrlConfig> config, IMapper mapper)
        {
            _context = context;
            _externalClientService = externalClientService;
            _externalServicesConfig = config.Value;
            _mapper = mapper;
        }

        public async Task CreateHistoryAsync(CreateHistoryDTO historyDTO, string accessToken)
        {
            await ValidateHistoryDTO(historyDTO);

            var roomInfo = await _externalClientService.SendRequestAsync<List<RoomInfoResponse>>(accessToken, $"{_externalServicesConfig.HospitalService}/api/Hospitals/{historyDTO.HospitalId}/Rooms", HttpMethod.Get, null)
                ?? throw new ApiException("Больница или помещение не найдено");

            var roomId = roomInfo.FirstOrDefault(r => r.Name == historyDTO.Room)?.Id
                ?? throw new ApiException("Помещение не найдено");

            var doctorInfo = await _externalClientService.SendRequestAsync<DoctorInfoResponse>(accessToken, $"{_externalServicesConfig.AccountService}/api/Doctors/{historyDTO.DoctorId}", HttpMethod.Get, null)
                ?? throw new ApiException("Доктор не найден");

            var patientInfo = await _externalClientService.SendRequestAsync<string[]>(accessToken, $"{_externalServicesConfig.AccountService}/api/Accounts/{historyDTO.PatientId}/Roles", HttpMethod.Get, null)
                ?? throw new ApiException("Роли не найдены");

            if (patientInfo.Length != 1 || patientInfo[0] != "User")
            {
                throw new ApiException("Пациент не пользователь");
            }

            var history = new History
            {
                HospitalId = historyDTO.HospitalId,
                DoctorId = historyDTO.DoctorId,
                PatientId = historyDTO.PatientId,
                RoomId = roomId,
                Date = historyDTO.Date,
                Data = historyDTO.Data
            };

            await _context.Histories.AddAsync(history);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PatientHistoryInfoDTO>> GetPatientHistoryAsync(int id, string accessToken)
        {
            var userInfo = await _externalClientService.SendRequestAsync<string[]>(accessToken, $"{_externalServicesConfig.AccountService}/api/Accounts/{id}/Roles", HttpMethod.Get, null)
                ?? throw new ApiException("Пользователь не найден");

            var currentUser = await _externalClientService.SendRequestAsync<AccountMeInfoResponse>(accessToken, $"{_externalServicesConfig.AccountService}/api/Accounts/Me", HttpMethod.Get, null);

            if (!currentUser.Roles.Contains("Doctor") && userInfo.Length == 0)
            {
                throw new NotFoundException("История посещений и назначений аккаунта не найдены");
            }

            var list = await _context.Histories.Where(h => h.PatientId == id)
                                               .ProjectTo<PatientHistoryInfoDTO>(_mapper.ConfigurationProvider)
                                               .ToListAsync();

            return list;
        }

        public async Task<GetHistoryInfoDTO> GetHistoryAsync(int id, string accessToken)
        {
            var currentUser = await _externalClientService.SendRequestAsync<AccountMeInfoResponse>(accessToken, $"{_externalServicesConfig.AccountService}/api/Accounts/Me", HttpMethod.Get, null)
                ?? throw new NotFoundException("Пользователь не найден");

            var item = await _context.Histories.Where(h => h.Id == id)
                                               .ProjectTo<GetHistoryInfoDTO>(_mapper.ConfigurationProvider)
                                               .FirstOrDefaultAsync()
                                               ?? throw new NotFoundException("История посещения и назначения не найдена");

            if (!currentUser.Roles.Contains("Doctor") && item.PatientId != currentUser.Id)
            {
                throw new NotFoundException("История посещения и назначения не найдена");
            }

            return item;
        }

        public async Task UpdateHistoryAsync(int id, UpdateHistoryDTO historyDTO, string accessToken)
        {
            await ValidateHistoryDTO(historyDTO);

            var roomInfo = await _externalClientService.SendRequestAsync<List<RoomInfoResponse>>(accessToken, $"{_externalServicesConfig.HospitalService}/api/Hospitals/{historyDTO.HospitalId}/Rooms", HttpMethod.Get, null)
                ?? throw new NotFoundException("Больница или помещение не найдено");

            var roomId = roomInfo.FirstOrDefault(r => r.Name == historyDTO.Room)?.Id
                ?? throw new NotFoundException("Помещение не найдено");

            var doctorInfo = await _externalClientService.SendRequestAsync<DoctorInfoResponse>(accessToken, $"{_externalServicesConfig.AccountService}/api/Doctors/{historyDTO.DoctorId}", HttpMethod.Get, null)
                ?? throw new NotFoundException("Доктор не найден");

            var patientInfo = await _externalClientService.SendRequestAsync<string[]>(accessToken, $"{_externalServicesConfig.AccountService}/api/Accounts/{historyDTO.PatientId}/Roles", HttpMethod.Get, null)
                ?? throw new NotFoundException("Пациент не найден");

            if (patientInfo.Length != 1 || patientInfo[0] != "User")
            {
                throw new NotFoundException("Пациент не пользователь");
            }

            var history = await _context.Histories.FirstOrDefaultAsync(h => h.Id == id)
                ?? throw new NotFoundException("История посещений не найдена.");


            history.HospitalId = historyDTO.HospitalId;
            history.DoctorId = historyDTO.DoctorId;
            history.PatientId = historyDTO.PatientId;
            history.RoomId = roomId;
            history.Date = historyDTO.Date;
            history.Data = historyDTO.Data;

            await _context.SaveChangesAsync();
        }

        private async Task ValidateHistoryDTO(HistoryDTO historyDTO)
        {
            if (historyDTO == null)
            {
                throw new ApiException("Модель не может быть нуллом");
            }
            if (historyDTO.HospitalId <= 0)
            {
                throw new ApiException("Id больницы не может быть меньше или равно 0");
            }
            if (historyDTO.DoctorId <= 0)
            {
                throw new ApiException("Id доктора не может быть меньше или равно 0");
            }
            if (historyDTO.PatientId <= 0)
            {
                throw new ApiException("Id пользователя не может быть меньше или равно 0");
            }
            if (string.IsNullOrEmpty(historyDTO.Room))
            {
                throw new ApiException("Название комнаты не может быть пустым");
            }
            if (string.IsNullOrEmpty(historyDTO.Data))
            {
                throw new ApiException("Информация о посещении не может быть пустой");
            }
            if (historyDTO.Date.Minute % 30 != 0 || historyDTO.Date.Second != 0)
            {
                throw new ApiException("Количество минут всегда кратно 30, секунды всегда 0");
            }

            historyDTO.Date = historyDTO.Date.AddMilliseconds(-historyDTO.Date.Millisecond);
        }
    }
}
