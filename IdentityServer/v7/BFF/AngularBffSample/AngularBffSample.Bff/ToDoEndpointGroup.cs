using System.Security.Claims;

namespace AngularBffSample.Bff;

public static class TodoEndpointGroup
{

    private static readonly List<ToDo> data = new List<ToDo>()
        {
            new ToDo { Id = ToDo.NewId(), Date = DateTimeOffset.UtcNow, Name = "Demo ToDo API", User = "bob" },
            new ToDo { Id = ToDo.NewId(), Date = DateTimeOffset.UtcNow.AddHours(1), Name = "Stop Demo", User = "bob" },
            new ToDo { Id = ToDo.NewId(), Date = DateTimeOffset.UtcNow.AddHours(4), Name = "Have Dinner", User = "alice" },
        };

    public static RouteGroupBuilder ToDoGroup(this RouteGroupBuilder group)
    {
        // GET
        group.MapGet("/", () => data);
        group.MapGet("/{id}", (int id) =>
        {
            var item = data.FirstOrDefault(x => x.Id == id);
        });

        // POST
        group.MapPost("/", (ToDo model, ClaimsPrincipal User) =>
        {
            model.Id = ToDo.NewId();
            model.User = $"{User.FindFirst("sub")?.Value} ({User.FindFirst("name")?.Value})";
            
            data.Add(model);
        });

        // PUT
        group.MapPut("/{id}", (int id, ToDo model, ClaimsPrincipal User) =>
        {
            var item = data.FirstOrDefault(x => x.Id == id);
            if (item == null) return Results.NotFound();

            item.Date = model.Date;
            item.Name = model.Name;

            return Results.NoContent();
        });

        // DELETE
        group.MapDelete("/{id}", (int id) =>
        {
            var item = data.FirstOrDefault(x => x.Id == id);
            if (item == null) return Results.NotFound();

            data.Remove(item);

            return Results.NoContent();
        });

        return group;
    }
}
