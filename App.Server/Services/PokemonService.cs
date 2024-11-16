using System.Text.Json;
using App.Models;

namespace App.Services
{
    public interface IPokemonService
    {
        Task<Pokemon> GetPokemon(int id);
        BattleResult Fight(Pokemon challenger, Pokemon defender);
    }
    public class PokemonService : IPokemonService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<IPokemonService> _logger;
        private readonly static Dictionary<string, string> TYPE_ADVANTAGES = new Dictionary<string, string>
            {
                { "water", "fire" },
                { "fire", "grass" },
                { "grass", "electric" },
                { "electric", "water" },
                { "ghost", "psychic" },
                { "psychic", "fighting" },
                { "fighting", "dark" },
                { "dark", "ghost" }
            };

        public PokemonService(ILogger<IPokemonService> logger)
        {
            _httpClient = new HttpClient();
            _logger = logger;
        }

        public async Task<Pokemon> GetPokemon(int id)
        {
            // Construct the API URL
            var apiUrl = $"https://pokeapi.co/api/v2/pokemon/{id}";

            try
            {
                // Send GET request to the API
                var response = await _httpClient.GetAsync(apiUrl);

                // Ensure the request was successful
                response.EnsureSuccessStatusCode();

                // Read and deserialize the JSON response
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var pokemon = JsonSerializer.Deserialize<Pokemon>(jsonResponse);

                return pokemon ?? throw new InvalidOperationException("Failed to deserialize the Pokemon data.");
            }
            catch (Exception ex)
            {
                // Handle errors (e.g., log them)
                Console.WriteLine($"Error fetching Pokémon data: {ex.Message}");
                throw;
            }
        }

        public BattleResult Fight(Pokemon challenger, Pokemon defender)
        {
            // Log the types and base experiences
            _logger.LogInformation($"{challenger.name} vs {defender.name}");
            _logger.LogInformation($"{challenger.name}: Type = {challenger.type}, Base Experience = {challenger.base_experience}");
            _logger.LogInformation($"{defender.name}: Type = {defender.type}, Base Experience = {defender.base_experience}");

            // Check if type advantage exists
            if (TYPE_ADVANTAGES.TryGetValue(challenger.type, out var advantageousAgainst) && advantageousAgainst.Equals(defender.type))
            {
                _logger.LogInformation($"{challenger.name}: WINS BY TYPE ADVANTAGE!");
                return BattleResult.WIN;
            }

            if (TYPE_ADVANTAGES.TryGetValue(defender.type, out advantageousAgainst) && advantageousAgainst.Equals(challenger.type))
            {
                _logger.LogInformation($"{defender.name}: WINS BY TYPE ADVANTAGE!");
                return BattleResult.LOSS;
            }

            // If no type advantage, decide based on base experience
            if (challenger.base_experience > defender.base_experience)
            {
                _logger.LogInformation($"{challenger.name}: WINS BY XP!");
                return BattleResult.WIN;
            }

            if (defender.base_experience > challenger.base_experience)
            {
                _logger.LogInformation($"{defender.name}: WINS BY XP!");
                return BattleResult.LOSS;
            }

            _logger.LogInformation($"Its a Tie!");
            // If all else fails, it's a tie
            return BattleResult.TIE;
        }
    }

}
