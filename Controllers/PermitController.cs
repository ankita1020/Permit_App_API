using System.Diagnostics;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Permit_App.Models;
using Permit_App.Services;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Permit_App.Controllers;

[ApiController]
[Route("api/permit")]
public class PermitController : ControllerBase
{
    private readonly AppDbContext _context;

    public PermitController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("counties")]
    public async Task<IActionResult> GetCounties()
    {
        var counties = await _context.Counties.ToListAsync();

        return Ok(counties);
    }

    [HttpGet("permittypes")]
    public async Task<IActionResult> GetPermitTypes()
    {
        var permitTypes = await _context.PermitTypes.ToListAsync();
        return Ok(permitTypes);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _context.Users.Include(u => u.Address).ToListAsync();
        return Ok(users);
    }

    [HttpPost("submit")]
    public async Task<IActionResult> CreateUser([FromBody] User user)
    {
        if (user == null || user.Address == null)
            return BadRequest("User and Address are required.");

        // Check if the address already exists
        var existingAddress = await _context.Addresses
            .FirstOrDefaultAsync(a =>
                a.AddressLine1 == user.Address.AddressLine1 &&
                a.AddressLine2 == user.Address.AddressLine2 &&
                a.City == user.Address.City &&
                a.State == user.Address.State &&
                a.ZipCode == user.Address.ZipCode &&
                a.Country == user.Address.Country);

        if (existingAddress != null)
            return BadRequest("Address already registered");

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _context.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.UserId == id);
        if (user == null)
            return NotFound();

        return Ok(user);
    }
}