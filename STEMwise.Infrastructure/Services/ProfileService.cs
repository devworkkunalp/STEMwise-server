using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using STEMwise.Application.DTOs;
using STEMwise.Application.Interfaces;
using STEMwise.Domain.Entities;
using STEMwise.Infrastructure.Data;

namespace STEMwise.Infrastructure.Services;

public class ProfileService : IProfileService
{
    private readonly AppDbContext _context;

    public ProfileService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Profile?> GetProfileByIdAsync(Guid id)
    {
        return await _context.Profiles
            .Include(p => p.UserUniversities)
                .ThenInclude(uu => uu.University)
            .Include(p => p.LoanConfigs)
            .Include(p => p.VisaConfigs)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Profile?> GetProfileByUserIdAsync(Guid userId)
    {
        // T20: Removing AsSplitQuery to prevent connection hanging in pooler
        return await _context.Profiles
            .Include(p => p.UserUniversities)
                .ThenInclude(uu => uu.University)
            .Include(p => p.LoanConfigs)
            .Include(p => p.VisaConfigs)
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    public async Task<Profile> UpsertProfileAsync(Guid userId, ProfileDto profileDto)
    {
        var profile = await _context.Profiles
            .Include(p => p.UserUniversities)
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (profile == null)
        {
            profile = new Profile
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };
            _context.Profiles.Add(profile);
        }

        // Map DTO to Entity
        profile.DisplayName = profileDto.DisplayName;
        profile.Nationality = profileDto.Nationality;
        profile.HomeCurrency = profileDto.HomeCurrency;
        profile.StemField = profileDto.StemField;
        profile.DegreeLevel = profileDto.DegreeLevel;
        profile.IntakeTerm = profileDto.IntakeTerm;
        
        // Map Career Targets
        profile.TargetCity = profileDto.TargetCity;
        profile.TargetSalary = profileDto.TargetSalary;
        profile.Specialization = profileDto.Specialization;

        // Map ROI Baseline Details
        profile.TargetUniversity = profileDto.TargetUniversity;
        profile.DegreeName = profileDto.DegreeName;
        profile.AnnualTuition = profileDto.AnnualTuition;
        profile.AnnualLivingCost = profileDto.AnnualLivingCost;
        profile.ProgramDurationYears = profileDto.ProgramDurationYears;
        profile.LoanAmount = profileDto.LoanAmount;
        profile.LoanInterestRate = profileDto.LoanInterestRate;

        profile.UpdatedAt = DateTime.UtcNow;

        // Handle University Selections
        // Simple approach: clear and re-add for onboarding
        _context.UserUniversities.RemoveRange(profile.UserUniversities);
        
        foreach (var uni in profileDto.SelectedUniversities)
        {
            profile.UserUniversities.Add(new UserUniversity
            {
                ProfileId = profile.Id,
                UniversityId = uni.UniversityId,
                Status = uni.Status,
                CreatedAt = DateTime.UtcNow
            });
        }

        await _context.SaveChangesAsync();
        return profile;
    }

    public async Task<bool> DeleteProfileAsync(Guid userId)
    {
        var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == userId);
        if (profile == null) return false;

        _context.Profiles.Remove(profile);
        await _context.SaveChangesAsync();
        return true;
    }
}
