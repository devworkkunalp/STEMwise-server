using System;
using System.Threading.Tasks;
using STEMwise.Application.DTOs;
using STEMwise.Domain.Entities;

namespace STEMwise.Application.Interfaces;

public interface IProfileService
{
    Task<Profile?> GetProfileByUserIdAsync(Guid userId);
    Task<Profile> UpsertProfileAsync(Guid userId, ProfileDto profileDto);
    Task<bool> DeleteProfileAsync(Guid userId);
}
