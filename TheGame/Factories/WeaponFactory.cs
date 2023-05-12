using Newtonsoft.Json;
using TheGame.Items;

namespace TheGame.Factories
{
    public static class WeaponFactory
    {
        private static readonly string WeaponsFolderPath = Path.Combine(Common.GetDataDir(), "Weapons");

        private static readonly string WeaponsFilePath = Path.Combine(Common.GetDataDir(), WeaponsFolderPath, "Weapons.json");
        private static List<Weapon> Weapons { get; set; } = new();

        private static List<Weapon> ReadOrInitializeWeapons()
        {
            if (Weapons.Any())
            {
                return Weapons;
            }
            var weapons = ReadWeaponsFromFile();
            if (!weapons.Any())
            {
                weapons = GenerateStarterWeapons();
            }
            Weapons = weapons;
            return weapons;
        }

        public static List<Weapon> GetWeapons() => ReadOrInitializeWeapons();

        private static void GenerateWeapon(int id, string name, int amountDice, Dice damageDice, string description = "")
        {
            Weapon weapon = new(id, name, amountDice, damageDice, description);
            AddWeaponToList(weapon);
        }

        private static void AddWeaponToList(Weapon weapon)
        {
            Weapons.Add(weapon);
            ZipWeapons();
        }

        private static List<Weapon> ReadWeaponsFromFile()
        {
            try
            {
                string json = File.ReadAllText(Path.Combine(WeaponsFilePath));
                List<Weapon>? weapons = JsonConvert.DeserializeObject<List<Weapon>>(json);
                if (weapons is null)
                {
                    throw new NullReferenceException($"Weapons list from file was null.");
                }
                return weapons;
            }
            catch (Exception exc)
            {
                Logger.LogError(exc);
                return new List<Weapon>();
            }
        }

        private static void ZipWeapons()
        {
            string json = JsonConvert.SerializeObject(Weapons, Formatting.Indented);
            if (!Directory.Exists(WeaponsFolderPath))
            {
                Directory.CreateDirectory(WeaponsFolderPath);
            }

            var path = Path.Combine(WeaponsFilePath);
            File.WriteAllText(path, json);
        }

        private static List<Weapon> GenerateStarterWeapons()
        {
            GenerateWeapon(1, "Knife", 1, Dice.d4, "Is very sharp...");
            GenerateWeapon(2, "Shortsword", 1, Dice.d6, "Is even sharper...");
            GenerateWeapon(3, "Longsword", 1, Dice.d8, "The sharpest...");
            return Weapons;
        }

        public static Weapon GetRandomWeapon()
        {
            Random rnd = new();
            return Weapons.ToArray()[rnd.Next(Weapons.Count)];
        }
    }
}