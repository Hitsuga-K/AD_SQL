using College_DB.Data;
using College_DB.Models;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CollegeDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddRazorPages();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CollegeDBContext>();
    db.Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.MapGet("students/all", async (CollegeDBContext db) =>
{
    return Results.Ok(await db.Students.AsNoTracking().ToListAsync());
});

app.MapGet("groups/all", async (CollegeDBContext db) =>
{
    return Results.Ok(await db.Groups.AsNoTracking().ToListAsync());
});
app.MapGet("/students/{id:int}", async (int id, CollegeDBContext db) =>
{
    var student = await db.Students
        .AsNoTracking()
        .SingleOrDefaultAsync(s => s.StudentId == id);

    return student == null
        ? Results.NotFound()
        : Results.Ok(student);
});

app.MapGet("/groups/{id:int}", async (int id, CollegeDBContext db) =>
{
    var group = await db.Groups
        .AsNoTracking()
        .SingleOrDefaultAsync(g => g.GroupId == id);

    return group == null
        ? Results.NotFound()
        : Results.Ok(group);
});

app.MapPost("/students", async (Student student, CollegeDBContext db) =>
{
    db.Students.Add(student);
    await db.SaveChangesAsync();

    return Results.Created($"/students/add",
                           new { studentId = student.StudentId });
});

app.MapPost("/groups", async (Group group, CollegeDBContext db) =>
{
    db.Groups.Add(group);
    await db.SaveChangesAsync();

    return Results.Created($"/groups/add",
                           new { groupId = group.GroupId });
});

app.MapPut("/students/{id:int}", async (int id, Student student, CollegeDBContext db) =>
{
    var existing = await db.Students.FindAsync(id);

    if (existing == null)
        return Results.NotFound();

    existing.FirstName = student.FirstName;
    existing.LastName = student.LastName;
    existing.Age = student.Age;
    existing.GroupId = student.GroupId;

    await db.SaveChangesAsync();

    return Results.NoContent();
});
app.MapPut("/groups/{id:int}", async (int id, Group group, CollegeDBContext db) =>
{
    var existing = await db.Groups.FindAsync(id);

    if (existing == null)
        return Results.NotFound();

    existing.GroupName = group.GroupName;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/students/{id:int}", async (int id, CollegeDBContext db) =>
{
    var existing = await db.Students.FindAsync(id);

    if (existing != null)
    {
        db.Remove(existing);
        await db.SaveChangesAsync();
    }

    return Results.NoContent();
});
app.MapDelete("/groups/{id:int}", async (int id, CollegeDBContext db) =>
{
    var existing = await db.Groups.FindAsync(id);

    if (existing == null)
        return Results.NotFound();

    db.Groups.Remove(existing);

    await db.SaveChangesAsync();

    return Results.NoContent();
});
app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapGet("/debug/database", async (CollegeDBContext db) =>
{
    var connection = db.Database.GetDbConnection();

    var count = await db.Students.CountAsync();

    return Results.Ok(new
    {
        Server = connection.DataSource,
        Database = connection.Database,
        StudentsCount = count
    });
});

app.Run();