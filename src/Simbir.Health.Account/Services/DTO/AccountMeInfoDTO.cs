﻿namespace Simbir.Health.Account.Services.DTO;

public class AccountMeInfoDTO
{
    public int Id { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string UserName { get; set; }
    public string[] Roles { get; set; }
}
