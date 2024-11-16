namespace App.Models
{
    public class PokemonTypeInfo
    {
        public string name { get; set; } // e.g., "grass", "poison"
        public string url { get; set; }
    }

    public class PokemonType
    {
        public int slot { get; set; }
        public PokemonTypeInfo type { get; set; }
    }

    public class PokemonStat
    {
        public PokemonStatInfo stat { get; set; }
        public int base_stat { get; set; } // The base value of the stat
    }

    public class PokemonStatInfo
    {
        public string name { get; set; } // e.g., "attack", "special-attack"
    }

    public class Pokemon
    {
        public int id { get; set; } // Pokémon's name
        public string name { get; set; } // Pokémon's name
        // For this example we only have 1 type
        public string type { get => this.types.First().type.name; }
        public int base_experience { get; set; }
        public List<PokemonType> types { get; set; } // Types (for effectiveness)
        public List<PokemonStat> stats { get; set; } // Base stats for calculations
    }

}
