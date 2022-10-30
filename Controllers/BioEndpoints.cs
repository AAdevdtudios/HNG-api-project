using HNG_api_project.Models;
namespace HNG_api_project.Controllers;

public static class BioEndpoints
{
    public static void MapBioEndpoints (this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/Bio", () =>
        {
            return new Bio()
            {
                backend = true,
                age = 23,
                bio = "My name is Oladele Joseph, am a software developer",
                slackUsername = "DevTherapy",
            };
        })
        .WithName("GetAllBios")
        .Produces<Bio[]>(StatusCodes.Status200OK);
/*
        routes.MapGet("/api/Bio/{id}", (int id) =>
        {
            //return new Bio { ID = id };
        })
        .WithName("GetBioById")
        .Produces<Bio>(StatusCodes.Status200OK);

        routes.MapPut("/api/Bio/{id}", (int id, Bio input) =>
        {
            return Results.NoContent();
        })
        .WithName("UpdateBio")
        .Produces(StatusCodes.Status204NoContent);

        routes.MapPost("/api/Bio/", (Bio model) =>
        {
            //return Results.Created($"/Bios/{model.ID}", model);
        })
        .WithName("CreateBio")
        .Produces<Bio>(StatusCodes.Status201Created);

        routes.MapDelete("/api/Bio/{id}", (int id) =>
        {
            //return Results.Ok(new Bio { ID = id });
        })
        .WithName("DeleteBio")
        .Produces<Bio>(StatusCodes.Status200OK);*/
    }
}
