using Microsoft.AspNetCore.Identity;
using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Models.Identity;
using SpecificSolutions.Endowment.Application.Models.Identity.Entities;
using SpecificSolutions.Endowment.Core.Entities.Cities;
using SpecificSolutions.Endowment.Core.Entities.Offices;
using SpecificSolutions.Endowment.Core.Entities.Products;
using SpecificSolutions.Endowment.Core.Entities.Regions;
using SpecificSolutions.Endowment.Infrastructure.Persistence;

namespace SpecificSolutions.Endowment.Infrastructure.Seeders;

public sealed class Seeder
{
    private readonly AppDbContext _context;
    //implementing IPasswordHasher 
    //TODO use build in
    private readonly IPasswordHasher _passwordHasher;
    //private readonly ISerializerService _serializerService;

    public Seeder(AppDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public static async Task SeedAsync(AppDbContext context, IPasswordHasher passwordHasher)
        => await new Seeder(context, passwordHasher).SeedDataAsync();

    public async Task SeedDataAsync()
    {
        var hasher = new PasswordHasher<ApplicationUser>();

        // Seed Cities
        if (!await _context.Cities.AnyAsync())
        {
            var cities = new[]
            {
                City.Seed(
                    id: new Guid("C658312F-1519-443F-8E1F-12307F01B77D"),
                    name: "Tripoli",
                    country: "Libya")
            };
            await _context.Cities.AddRangeAsync(cities);
            await _context.SaveChangesAsync();
        }

        // Seed Regions
        if (!await _context.Regions.AnyAsync())
        {
            var regions = new[]
            {
                Region.Seed(
                    id: new Guid("DDEC6E9E-7698-4623-9A84-4E5EFC02187C"),
                    cityId: new Guid("C658312F-1519-443F-8E1F-12307F01B77D"),
                    name: "Tripoli",
                    country: "Libya")
            };
            await _context.Regions.AddRangeAsync(regions);
            await _context.SaveChangesAsync();
        }

        // Seed Offices
        if (!await _context.Offices.AnyAsync())
        {
            var office = Office.Seed(
                id: new Guid("DDEC6E9E-7628-4623-9A94-4E4EFC02187C"),
                userId: new Guid("a2d890d8-01d1-494b-9f62-6336b937e6fc"),
                name: "Main Office",
                location: "Tripoli",
                phoneNumber: "09284746974",
                regionId: new Guid("DDEC6E9E-7698-4623-9A84-4E5EFC02187C")
            );
            await _context.Offices.AddAsync(office);
            await _context.SaveChangesAsync();
        }

        // Seed Users
        if (!await _context.Users.AnyAsync())
        {
            var users = new[]
            {
                ApplicationUser.Seed(
                    id: new Guid("a2d890d8-01d1-494b-9f62-6336b937e6fc").ToString(),
                    email: "1",
                    firstName: "Khaled",
                    lastName: "Alnefati",
                    officeId: new Guid("DDEC6E9E-7628-4623-9A94-4E4EFC02187C"),
                    userName: "1",
                    passwordHash: hasher.HashPassword(null, "1"),
                    emailConfirmed: true) // Add SecurityStamp

            };

            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();
        }

        if (!await _context.Users.AnyAsync(c => c.Email == "A@gmail.com"))
        {
            var users = new[]
            {
                ApplicationUser.Seed(
                    id: new Guid("a3d890d8-01d1-444b-9f62-6336b937e5fc").ToString(),
                    email: "A@gmail.com",
                    firstName: "Ragid",
                    lastName: "Dev",
                    officeId: new Guid("DDEC6E9E-7628-4623-9A94-4E4EFC02187C"),
                    userName: "A@gmail.com",
                    passwordHash: hasher.HashPassword(null, "12345678"),
                    emailConfirmed: true)

            };

            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();
        }

        if (!await _context.Users.AnyAsync(c => c.Email == "employee@gmail.com"))
        {
            var employeeUser = ApplicationUser.Seed(
                id: new Guid("c2d890d8-01d1-494b-9f62-6336b937e6fe").ToString(),
                email: "employee@gmail.com",
                firstName: "Ahmed",
                lastName: "Employee",
                officeId: new Guid("DDEC6E9E-7628-4623-9A94-4E4EFC02187C"),
                userName: "employee@gmail.com",
                passwordHash: hasher.HashPassword(null, "12345678"),
                emailConfirmed: true);

            await _context.Users.AddAsync(employeeUser);
            await _context.SaveChangesAsync();
        }

        if (!await _context.Products.AnyAsync())
        {
            var products = new[]
            {
                Product.Seed(
                    id: new Guid("DDEC9E9E-6698-7623-9A84-4E5EFC02187C"),
                    name: "Product 1",
                    description: "Description 1" ),

                Product.Seed(
                    id: new Guid("DDEC8E9E-4698-4623-9A84-4E7EFC02187C"),
                    name: "Product 2",
                    description: "Description 2" ),
            };

            await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();
        }

        // Seed Roles (Role Groups)
        // Each role represents a group of users with similar responsibilities
        if (!await _context.Roles.AnyAsync())
        {
            var roles = new[]
            {
                new ApplicationRole
                {
                    Id = new Guid("5cda4a3e-7a57-4b0e-87fe-48ee9c1d3eaa").ToString(),
                    Name = "Admin",           // مدراء - Managers Group
                    NormalizedName = "ADMIN"
                },
                new ApplicationRole
                {
                    Id = new Guid("6cda4a3e-7a57-4b0e-87fe-48ee9c1d3ebb").ToString(),
                    Name = "Customer",        // مبيعات - Sales Group  
                    NormalizedName = "CUSTOMER"
                },
                new ApplicationRole
                {
                    Id = new Guid("7cda4a3e-7a57-4b0e-87fe-48ee9c1d3ccc").ToString(),
                    Name = "Employee",        // موظفين - Staff Group
                    NormalizedName = "EMPLOYEE"
                }
            };
            await _context.ApplicationRole.AddRangeAsync(roles);
            await _context.SaveChangesAsync();
        }

        // Seed UserRoles (User-Role-Permissions Mapping)
        // Each UserRole contains specific permissions for a role group
        if (!await _context.UserRoles.AnyAsync())
        {
            var userRoles = new[]
            {
                // Admin Group - Full permissions (استخدام المستخدم الموجود)
                ApplicationUserRole.Create(
                    userId: new Guid("a2d890d8-01d1-494b-9f62-6336b937e6fc").ToString(),
                    roleId: new Guid("5cda4a3e-7a57-4b0e-87fe-48ee9c1d3eaa").ToString(),
                    permissions: Permission.Seed()), // All permissions = true
                    
                // Customer Group - Limited permissions (استخدام المستخدم الثاني الموجود)
                ApplicationUserRole.Create(
                    userId: new Guid("a3d890d8-01d1-444b-9f62-6336b937e5fc").ToString(),
                    roleId: new Guid("6cda4a3e-7a57-4b0e-87fe-48ee9c1d3ebb").ToString(),
                    permissions: Permission.Create(
                        accountView: true, accountAdd: false, accountEdit: false, accountDelete: false,
                        userView: false, userAdd: false, userEdit: false, userDelete: false,
                        roleView: false, roleAdd: false, roleEdit: false, roleDelete: false,
                        decisionView: false, decisionAdd: false, decisionEdit: false, decisionDelete: false,
                        requestView: true, requestAdd: true, requestEdit: true, requestDelete: false,
                        officeView: true, officeAdd: false, officeEdit: false, officeDelete: false,
                        endowmentView: true, endowmentAdd: false, endowmentEdit: false, endowmentDelete: false,
                        cityView: false, cityAdd: false, cityEdit: false, cityDelete: false,
                        regionView: true, regionAdd: false, regionEdit: false, regionDelete: false,
                        buildingView: true, buildingAdd: false, buildingEdit: false, buildingDelete: false
                    )),
                    
                // Employee Group - Basic permissions (استخدام المستخدم الثالث الموجود)
                ApplicationUserRole.Create(
                    userId: new Guid("c2d890d8-01d1-494b-9f62-6336b937e6fe").ToString(),
                    roleId: new Guid("7cda4a3e-7a57-4b0e-87fe-48ee9c1d3ccc").ToString(),
                    permissions: Permission.Create(
                        accountView: true, accountAdd: false, accountEdit: false, accountDelete: false,
                        userView: false, userAdd: false, userEdit: false, userDelete: false,
                        roleView: false, roleAdd: false, roleEdit: false, roleDelete: false,
                        decisionView: false, decisionAdd: false, decisionEdit: false, decisionDelete: false,
                        requestView: true, requestAdd: false, requestEdit: false, requestDelete: false,
                        officeView: true, officeAdd: false, officeEdit: false, officeDelete: false,
                        endowmentView: true, endowmentAdd: false, endowmentEdit: false, endowmentDelete: false,
                        cityView: true, cityAdd: false, cityEdit: false, cityDelete: false,
                        regionView: true, regionAdd: false, regionEdit: false, regionDelete: false,
                        buildingView: true, buildingAdd: false, buildingEdit: false, buildingDelete: false
                    ))
            };
            await _context.ApplicationUserRole.AddRangeAsync(userRoles);
            await _context.SaveChangesAsync();
        }
    }
}