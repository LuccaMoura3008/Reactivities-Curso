using System;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers;

public class ActivitiesController(AppDbContext context) : BaseApiController //Injeção de dependencia do contexto do banco de dados!! Esse AppDbContext é o construtor primario da classe
{
    [HttpGet]
    public async Task<ActionResult<List<Activity>>> GetActivities() //Usa ActionResult para retornar respostas HTTP / GetActivities é o nome do método/endpoint
    {
        return await context.Activities.ToListAsync(); //Retorna a lista de atividades do banco de dados de forma assincrona
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Activity>> GetActivityDetail(string id)
    {
        var activity = await context.Activities.FindAsync(id);

        if (activity == null) return NotFound();

        return activity;
    }
}
