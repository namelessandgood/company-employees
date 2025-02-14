﻿using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service;

internal sealed class EmployeesService : IEmployeeService
{
    private readonly ILoggerManager _logger;
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    public EmployeesService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges)
    {

        var company = _repository.Company.GetCompany(companyId, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var employeesFromDb = _repository.Employee.GetEmployees(companyId, trackChanges);
        var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);
        return employeesDto;

    }

    public EmployeeDto GetEmployee(Guid companyId, Guid id, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges);

        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var employeeDb = _repository.Employee.GetEmployee(companyId, id, trackChanges);

        if (employeeDb is null)
            throw new EmployeeNotFoundException(id);

        var employee = _mapper.Map<EmployeeDto>(employeeDb);

        return employee;
    }

    public EmployeeDto CreateEmployeeForCompany(
        Guid companyId,
        EmployeeForCreationDto employeeForCreation,
        bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges);

        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        var employeeEntity = _mapper.Map<Employee>(employeeForCreation);
        _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
        _repository.Save();

        var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);
        return employeeToReturn;
    }

    public void DeleteEmployee(Guid companyId, Guid id, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges);

        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var employeeForCompany =
            _repository.Employee.GetEmployee(companyId, id, trackChanges);

        if (employeeForCompany is null)
            throw new EmployeeNotFoundException(id);

        _repository.Employee.DeleteEmployee(employeeForCompany);
        _repository.Save();
    }

    public void UpdateEmployeeForCompany(
        Guid companyId,
        Guid id,
        EmployeeForUpdateDto employeeForUpdateDto,
        bool compTrackChanges,
        bool empTrackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, compTrackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);
        var employeeEntity = _repository.Employee.GetEmployee(companyId, id,
            empTrackChanges);
        if (employeeEntity is null)
            throw new EmployeeNotFoundException(id);
        _mapper.Map(employeeForUpdateDto, employeeEntity);
        _repository.Save();
    }

    public (EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
        GetEmployeeForPatch
        (Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, compTrackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);
        var employeeEntity = _repository.Employee.GetEmployee(companyId, id,
            empTrackChanges);
        if (employeeEntity is null)
            throw new EmployeeNotFoundException(companyId);
        var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);
        return (employeeToPatch, employeeEntity);
    }

    public void SaveChangesForPatch(
        EmployeeForUpdateDto employeeToPatch,
        Employee employeeEntity)
    {
        _mapper.Map(employeeToPatch, employeeEntity);
        _repository.Save();
    }
}