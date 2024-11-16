using Microsoft.AspNetCore.Mvc;
using App.Models;
using App.Services;

namespace App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {

        private readonly ILogger<PokemonController> _logger;
        private readonly IPokemonService _pokemonService;
        private readonly HashSet<string> SORT_BY_TYPES = new HashSet<string>()
        {
            "wins",
            "losses",
            "ties",
            "name",
            "id"
        };
        private readonly HashSet<string> SORT_DIRECTION_TYPES = new HashSet<string>()
        {
            "asc",
            "desc"
        };

        public PokemonController(ILogger<PokemonController> logger, IPokemonService pokemonService)
        {
            _logger = logger;
            _pokemonService = pokemonService;
        }

        [HttpGet("tournament/statistics")]
        public async Task<IActionResult> Statistics(string sortBy, string? sortDirection = "desc")
        {
            try
            {
                if (sortBy == null) throw new Exception("sortBy parameter is required");
                if (!SORT_BY_TYPES.Contains(sortBy)) throw new Exception("sortBy parameter is invalid");
                if (!SORT_DIRECTION_TYPES.Contains(sortDirection)) throw new Exception("sortDirection parameter is invalid");

                // Get 8 distinct random ints between 1 and 151
                var random = new Random();
                var pokemonIds = Enumerable.Range(1, 151).OrderBy(_ => random.Next()).Take(8).ToList();

                var pokemons = new List<Pokemon>();

                // Fetch Pok�mon data
                foreach (var id in pokemonIds)
                {
                    pokemons.Add(await _pokemonService.GetPokemon(id));
                }

                // Initialize statistics
                var stats = pokemons.ToDictionary(
                    p => p.name,
                    p => new { p.id, p.name, type = p.type, wins = 0, losses = 0, ties = 0 });

                // Simulate battles
                for (int i = 0; i < pokemons.Count; i++)
                {
                    for (int j = i + 1; j < pokemons.Count; j++)
                    {
                        var challenger = pokemons[i];
                        var defender = pokemons[j];

                        var result = _pokemonService.Fight(challenger, defender);

                        if (result == BattleResult.WIN)
                        {
                            stats[challenger.name] = stats[challenger.name] with { wins = stats[challenger.name].wins + 1 };
                            stats[defender.name] = stats[defender.name] with { losses = stats[defender.name].losses + 1 };
                        }
                        else if (result == BattleResult.LOSS)
                        {
                            stats[challenger.name] = stats[challenger.name] with { losses = stats[challenger.name].losses + 1 };
                            stats[defender.name] = stats[defender.name] with { wins = stats[defender.name].wins + 1 };
                        }
                        else
                        {
                            stats[challenger.name] = stats[challenger.name] with { ties = stats[challenger.name].ties + 1 };
                            stats[defender.name] = stats[defender.name] with { ties = stats[defender.name].ties + 1 };
                        }
                    }
                }

                // Sort results
                var sortedResults = stats
                    .Select(x => x.Value)
                    .OrderByDescending(s => s.GetType().GetProperty(sortBy)?.GetValue(s, null));

                // Return JSON result
                return Ok(sortDirection == "desc" ? sortedResults : sortedResults.Reverse());

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in Tournament Statistics function");

                // Return error response
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
