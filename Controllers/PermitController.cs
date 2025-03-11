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
    private static List<FormData> SubmittedForms = new List<FormData>();
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

    [HttpPost("submit")]
    public async Task<IActionResult> SubmitForm([FromBody] FormData formData)
    {
        if (formData == null)
            return BadRequest("Invalid form data.");

        _context.FormSubmissions.Add(formData);
        await _context.SaveChangesAsync();

        // Query the saved values to check what has been saved
        var savedFormData = await _context.FormSubmissions
                                           .Where(f => f.Id == formData.Id)  // Fetch the specific saved record
                                           .FirstOrDefaultAsync();

        // Log or print to console for debugging
        if (!string.IsNullOrEmpty(savedFormData.UserName))
            Console.WriteLine($"Saved FormData: Username = {savedFormData.UserName},Address = {savedFormData.AddressLine1}, " +
                        $"City = {savedFormData.City}, State = {savedFormData.State}, " +
                        $"ZipCode = {savedFormData.ZipCode}, Country = {savedFormData.Country}, " +
                        $"County = {savedFormData.CountyId}, PermitType = {savedFormData.PermitTypeId}");
        else
            Console.WriteLine("Form does not have valid data");

            return Ok(new { message = "Form submitted successfully!", data = formData });
    }
}