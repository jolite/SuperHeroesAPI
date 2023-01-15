using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace SuperHeroesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public SuperHeroController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<SuperHero>>> Get()
        {
            return Ok(await _dataContext.SuperHeroes.ToListAsync());
        }
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(SuperHero), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        public async Task<ActionResult<SuperHero>> Get(int id)
        {
            var hero = await _dataContext.SuperHeroes.FindAsync(id);
            if(hero == null) { return BadRequest("Hero Not Found"); }
            return Ok(hero);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(SuperHero), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        public async Task<ActionResult<List<SuperHero>>> AddHero(SuperHero hero)
        {
            _dataContext.SuperHeroes.Add(hero);
            await _dataContext.SaveChangesAsync();
            return Ok(await _dataContext.SuperHeroes.ToListAsync()); 
        }
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<SuperHero>>> UpdateHero(SuperHero request)
        {
            var hero = await _dataContext.SuperHeroes.FindAsync(request.Id);
            if (hero == null) { return BadRequest("Hero Not Found"); }
            hero.Name = request.Name;
            hero.FirstName = request.FirstName;
            hero.LastName = request.LastName;
            hero.Location = request.Location;
            await _dataContext.SaveChangesAsync();
            return Ok(await _dataContext.SuperHeroes.ToListAsync());
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<SuperHero>> Delete(int id)
        {
            var hero = await _dataContext.SuperHeroes.FindAsync(id);
            if (hero == null) { return BadRequest("Hero Not Found"); }
            _dataContext.SuperHeroes.Remove(hero);
            await _dataContext.SaveChangesAsync();  
            return Ok(await _dataContext.SuperHeroes.ToListAsync());
        }
    }
}
