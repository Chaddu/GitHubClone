using Application.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class OrganizationMemberService(IUnitOfWork unitOfWork, IMapper mapper) : IOrganizationMemberService
{
    public async Task<Result<OrganizationMemberResponse>> AddMemberAsync(int organizationId, AddOrganizationMemberRequest request, int currentUserId)
    {
        var currentMembership = await unitOfWork.OrganizationMembers.GetMembershipAsync(organizationId, currentUserId);

        if (currentMembership == null)
            return Result<OrganizationMemberResponse>.Failure("You are not a member of this organization.", ErrorType.Forbidden);

        if (currentMembership.Role != OrganizationRole.Admin &&
            currentMembership.Role != OrganizationRole.Owner)
            return Result<OrganizationMemberResponse>.Failure("You do not have permission to add members.", ErrorType.Forbidden);

        var alreadyMember = await unitOfWork.OrganizationMembers.IsMemberAsync(organizationId, request.UserId);

        if (alreadyMember)
            return Result<OrganizationMemberResponse>.Failure("User is already a member of this organization.", ErrorType.Conflict);

        var member = mapper.Map<OrganizationMember>(request);

        member.OrganizationId = organizationId;
        member.Role = OrganizationRole.Member;

        await unitOfWork.OrganizationMembers.AddASync(member);
        await unitOfWork.SaveChangesAsync();

        var response = mapper.Map<OrganizationMemberResponse>(member);

        return Result<OrganizationMemberResponse>.Success(response);

    }

    public async Task<Result<List<OrganizationMemberResponse>>> GetMembersAsync(int organizationId)
    {
        var memeber = await unitOfWork.OrganizationMembers.GetByOrganizationIdAsync(organizationId);

        var response = mapper.Map<List<OrganizationMemberResponse>>(memeber);
        return Result<List<OrganizationMemberResponse>>.Success(response);
    }

    public async Task<Result> RemoveMemberAsync(int organizationId, int memberId, int currentUserId)
    {
        var currentMembership = await unitOfWork.OrganizationMembers.GetMembershipAsync(organizationId, currentUserId);

        if(currentMembership == null)
            return Result.Failure("You are not a member of this organization.", ErrorType.Forbidden);

        if(currentMembership.Role != OrganizationRole.Owner && currentMembership.Role != OrganizationRole.Admin)
            return Result.Failure("You do not have permission to remove members.", ErrorType.Forbidden);

        var member = await unitOfWork.OrganizationMembers.GetByIdAsync(memberId);

        if(member == null || member.OrganizationId != organizationId)
            return Result.Failure("Member not found in this organization.", ErrorType.NotFound);

        if(member.Role == OrganizationRole.Owner)
            return Result.Failure("You cannot remove the organization owner.", ErrorType.Forbidden);

        unitOfWork.OrganizationMembers.Delete(member);
        await unitOfWork.SaveChangesAsync();

        return Result.Success("Member was removed successfully.");
    }

    public async Task<Result> UpdateRoleAsync(int organizationId, int memberId, UpdateOrganizationMemberRoleRequest request, int currentUserId)
    {
        var currentMembership = await unitOfWork.OrganizationMembers.GetMembershipAsync(organizationId, currentUserId);

        if (currentMembership == null)
            return Result.Failure("You are not a member of this organization.", ErrorType.Forbidden);

        if (currentMembership.Role != OrganizationRole.Owner)
            return Result.Failure("You do not have permission to update member roles.", ErrorType.Forbidden);


        var member = await unitOfWork.OrganizationMembers.GetByIdAsync(memberId);

        if (member == null || member.OrganizationId != organizationId)
            return Result.Failure("Member not found in this organization.", ErrorType.NotFound);

        if (member.Role == OrganizationRole.Owner)
            return Result.Failure("You cannot update the role of the organization owner.", ErrorType.Forbidden);

        if (request.Role == OrganizationRole.Owner)
            return Result.Failure("Owner role cannot be assigned this way.", ErrorType.BadRequest);


        member.Role = request.Role;

        unitOfWork.OrganizationMembers.Update(member);
        await unitOfWork.SaveChangesAsync();

        return Result.Success("Member role was updated successfully.");
    }

   
}
