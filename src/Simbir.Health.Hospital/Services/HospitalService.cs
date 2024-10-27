using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Simbir.Health.Hospital.Controllers.DTO;
using Simbir.Health.Hospital.Data;
using Simbir.Health.Hospital.Exceptions;
using Simbir.Health.Hospital.Models;
using Simbir.Health.Hospital.Services.DTO;

namespace Simbir.Health.Hospital.Services
{
    public class HospitalService : IHospitalService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public HospitalService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<HospitalInfoListDTO> GetHospitalsAsync(int from, int count)
        {
            var query = _context.Hospitals.Where(h => !h.IsDeleted);

            var totalCount = query.Count();

            var hospitalList = await
                query.Skip(from)
                     .Take(count)
                     .Select(h => new HospitalInfoDTO
                     {
                         Id = h.Id,
                         Name = h.Name,
                         Address = h.Address,
                         ContactNumber = h.ContactPhone
                     }).ToListAsync();

            var result = new HospitalInfoListDTO
            {
                Hospitals = hospitalList,
                TotalCount = totalCount
            };

            return result;
        }

        public async Task<HospitalDetailedInfoDTO> GetHospitalByIdAsync(int id)
        {
            var hospital = await
                (
                    from h in _context.Hospitals
                    where !h.IsDeleted && h.Id == id
                    select new HospitalDetailedInfoDTO
                    {
                        Id = h.Id,
                        Name = h.Name,
                        Address = h.Address,
                        ContactPhone = h.ContactPhone,
                        RoomList =
                            (
                                from r in _context.Rooms
                                where r.HospitalId == h.Id
                                select new RoomInfoDTO
                                {
                                    Id = r.Id,
                                    Name = r.Name,
                                }
                            ).ToList()
                    }
                ).FirstOrDefaultAsync()
                ?? throw new NotFoundException($"Больница с ID {id} не найдена.");

            return hospital;
        }

        public async Task<List<RoomInfoDTO>> GetHospitalRoomsAsync(int hospitalId)
        {
            var hospitalExists =
                await _context.Hospitals.Where(h => h.Id == hospitalId && !h.IsDeleted)
                                        .FirstOrDefaultAsync()
                                        ?? throw new NotFoundException($"Больница с ID {hospitalId} не найдена.");

            var rooms =
                await _context.Rooms.Where(r => r.HospitalId == hospitalId)
                                    .Select(r => new RoomInfoDTO
                                    {
                                        Id = r.Id,
                                        Name = r.Name
                                    }).ToListAsync();

            return rooms;
        }

        public async Task<int> CreateHospitalAsync(HospitalCreateRequest request)
        {
            var hospital = _mapper.Map<HospitalEntity>(request);

            using var tr = _context.Database.BeginTransaction();

            await _context.Hospitals.AddAsync(hospital);

            await _context.SaveChangesAsync();

            var rooms =
                request.Rooms.Select(r => new Room
                {
                    HospitalId = hospital.Id,
                    Name = r
                });

            await _context.Rooms.AddRangeAsync(rooms);
            await _context.SaveChangesAsync();

            await tr.CommitAsync();

            return hospital.Id;
        }

        public async Task UpdateHospitalAsync(int id, HospitalUpdateRequest request)
        {
            var hospital =
                await _context.Hospitals.Where(h => h.Id == id && !h.IsDeleted)
                                        .FirstOrDefaultAsync()
                                        ?? throw new NotFoundException($"Больница с ID {id} не найдена.");

            _mapper.Map(request, hospital);

            var currentRooms = await _context.Rooms
                .Where(r => r.HospitalId == id)
                .ToListAsync();

            var roomsToDelete = currentRooms
                .Where(r => !request.Rooms.Contains(r.Name))
                .ToList();

            var roomsToAdd = request.Rooms
                .Where(name => !currentRooms.Any(r => r.Name == name))
                .Select(name => new Room
                {
                    HospitalId = id,
                    Name = name
                })
                .ToList();

            _context.Rooms.RemoveRange(roomsToDelete);

            await _context.Rooms.AddRangeAsync(roomsToAdd);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteHospitalByIdAsync(int hospitalId)
        {
            var hospital = await _context.Hospitals
                .FirstOrDefaultAsync(h => h.Id == hospitalId)
                ?? throw new NotFoundException($"Больница с ID {hospitalId} не найдена.");

            hospital.IsDeleted = true;

            await _context.SaveChangesAsync();
        }

    }
}
