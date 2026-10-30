using College_DB.Data;




var builder = WebApplication.CreateBuilder(args);

string connectyion_string = "Data Source=COMP11A1\\SQLEXPRESS;" +
            "Initial Catalog=College_DB;" +
            "Integrated Security=True;" +
            "TrustServerCertificate=True;";
StudentRepository stud_repo = new StudentRepository(connectyion_string);
GroupRepository group_repo = new GroupRepository(connectyion_string);
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton(stud_repo);

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapGet("students/all", () =>
{
    var students = stud_repo.GetAllStudent2();
    return Results.Ok(students);
});

app.MapGet("groups/all", () =>
{
    var group = group_repo.GetAllGroups();
    return Results.Ok(group);
});

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
