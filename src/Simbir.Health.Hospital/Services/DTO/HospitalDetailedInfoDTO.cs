﻿namespace Simbir.Health.Hospital.Services.DTO
{
    public class HospitalDetailedInfoDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactPhone { get; set; }
        public List<RoomInfoDTO> RoomList { get; set; }
    }
}
