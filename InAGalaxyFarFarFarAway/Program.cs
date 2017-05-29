using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InAGalaxyFarFarFarAway
{
    public partial class Program
    {
        public static bool ValidInput => (Regex.IsMatch(Globals.MGLT, @"^\d+$") && long.TryParse(Globals.MGLT, out long n));

        static void Main(string[] args)
        {
            do
            {
                Console.Clear();
                List<Starship> starships = GetStarshipAsync(ResultCallBack).Result;
                Console.WriteLine($"\nAll {starships.Count} starships displayed");

                Console.WriteLine($"\nPress 'Esc' to escape or any other key to try again.");
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

            Console.WriteLine("\n             ._,.");
            Console.WriteLine("           '.-..pf.");
            Console.WriteLine("           - L..#'");
            Console.WriteLine("         .+ _L.'#");
            Console.WriteLine("        , 'j'.+.j`                 -'.__..,.,p.");
            Console.WriteLine("       _~ #..<..0.                 .J-.``..._f.");
            Console.WriteLine("      .7..#_.. _f.                ..,..-..,`4'");
            Console.WriteLine("      ;` ,#j.  T'      ..         ,.~ #..,'.j`");
            Console.WriteLine("     .` ..#^.,-0.,,,,yMMMMM,.    ,-.J...+`.j@");
            Console.WriteLine("    .'.`...'.yMMMMM0M@^=`\"\"g.. .'..-,.}.'.jH");
            Console.WriteLine("    j' .'1`  q'^)@@##^#.`#='BNg_...,]_)'...0- ");
            Console.WriteLine("     .T...I.j/    .'..+,_.'3#MMM0MggCBf....F.");
            Console.WriteLine("   j /.+ '.[..+    `^~' -^ ~~-^%'\"?'\"``'1`");
            Console.WriteLine("   .... .y.}                  `.._-:`_...jf");
            Console.WriteLine("  g -. .Lg'                  ..,..'-....,'.");
            Console.WriteLine("  .'.  .Y^                   .....',].._f");
            Console.WriteLine("  ....-f.                   .-,,.,.-:--&`");
            Console.WriteLine("                            .`...'..`_J`");
            Console.WriteLine("                            .~......'#'");
            Console.WriteLine("                            '..,.,_]`7     Sienar Fleet Systems' TIE/In");
            Console.WriteLine("                            L..`..`,/      Space Superiority Starfighter(2)\n\n");

            for (int i = 3; i > 0; --i)
            {
                Console.Write($"\rThis application will exit in {i} seconds...");
                System.Threading.Thread.Sleep(1000);                
            }
            Environment.Exit(0);
        }

        private static async Task<List<Starship>> GetStarshipAsync(Action<StarshipWrapper> callBack = null)
        {
            List<Starship> starships = new List<Starship>();
            bool needsDirection = false;

            try
            {
                HttpClient httpClient = new HttpClient()
                {
                    BaseAddress = new Uri("http://swapi.co/api/starships/")
                };
                var nextUrl = httpClient.BaseAddress.ToString();

                do
                {
                    if (needsDirection)
                    {
                        Console.Clear();
                        Console.WriteLine("\nPlease ensure you input a valid positive number (valid range: 0 - 9223372036854775807).\n");
                    }

                    Console.WriteLine("\nPlease input distance to travel in MGLT: ");

                    needsDirection = true;

                    Globals.MGLT = Console.ReadLine();
                } while (!ValidInput);

                Console.WriteLine("\nCommunication channel has been established...\n");

                Console.WriteLine("\nPlease stand by while we retrieve starships details...\n");

                do
                {
                    await httpClient.GetAsync(nextUrl)
                        .ContinueWith(async (starshipSearchTask) =>
                        {
                            var response = await starshipSearchTask;
                            if (response.IsSuccessStatusCode)
                            {
                                string asyncResponse = await response.Content.ReadAsStringAsync();
                                var result = JsonConvert.DeserializeObject<StarshipWrapper>(asyncResponse);
                                if (result != null)
                                {
                                    // Build the entire list to return later after the loop.
                                    if (result.Starships.Any())
                                        starships.AddRange(result.Starships.ToList());

                                    foreach (Starship starship in starships) {
                                        starship.ResupplyFrequency = CalculateResupplyFrequency(starship);
                                    }                                    

                                    // Run the callback method, passing the current result from the API.
                                    callBack?.Invoke(result);

                                    // Get the URL for the next page, where it exists.
                                    nextUrl = result.Next ?? string.Empty;
                                }
                            }
                            else
                            {
                                // End loop if we get an error response.
                                nextUrl = string.Empty;
                            }
                        });
                    // while nextUrl contains a value, continue.
                } while (!string.IsNullOrEmpty(nextUrl));

                Console.WriteLine("\nCommunication channel disconnected...");
            }
            catch
            {
                Console.WriteLine("\nUnable to establish communication channel...");
            }

            return starships;
        }

        private static void ResultCallBack(StarshipWrapper starshipSearchResult)
        {
            if (starshipSearchResult != null && starshipSearchResult.Count > 0)
            {
                int maxLengthStarshipName = starshipSearchResult.Starships.Aggregate((max, cur) => max.Name.Length > cur.Name.Length ? max : cur).Name.Length;
                int maxLengthStarshipRange = starshipSearchResult.Starships.Aggregate((max, cur) => max.MGLT.Length > cur.MGLT.Length ? max : cur).MGLT.Length;
                    maxLengthStarshipRange = maxLengthStarshipRange > "unknown".Length ? maxLengthStarshipRange : "unknown".Length;
                int maxLengthStarshipResupplies = Globals.MGLT.Length > "unknown".Length ? Globals.MGLT.Length : "unknown".Length;
                

                foreach (var starship in starshipSearchResult.Starships)
                {
                    Console.WriteLine($"{starship.Name.PadRight(maxLengthStarshipName)} \t\tMGLT - {starship.MGLT.PadLeft(maxLengthStarshipRange)} \t\tNo. of Resupplies - { starship.ResupplyFrequency.PadLeft(maxLengthStarshipResupplies) }");
                }
            }
        }

        public static string CalculateResupplyFrequency(Starship starship)
        {
            // if "unknown", retrun "unknown"
            if (!Regex.IsMatch(starship.MGLT, @"^\d+$"))
            {
                return starship.MGLT;
            }

            DateTime JourneyStart = DateTime.Now;
            DateTime JourneyEnd = DateTime.Now;

            string[] tokenisedProvisionsOfConsumables = starship.Consumables.Split(' ');

            switch (tokenisedProvisionsOfConsumables[1].ToLower())
            {
                case "day":
                case "days":
                    JourneyEnd = JourneyEnd.AddDays(Convert.ToDouble(tokenisedProvisionsOfConsumables[0]));
                    break;
                case "week":
                case "weeks":
                    JourneyEnd = JourneyEnd.AddDays(Convert.ToDouble(tokenisedProvisionsOfConsumables[0]) * 7);
                    break;
                case "month":
                case "months":
                    JourneyEnd = JourneyEnd.AddMonths(Convert.ToInt32(tokenisedProvisionsOfConsumables[0]));
                    break;
                case "year":
                case "years":
                    JourneyEnd = JourneyEnd.AddYears(Convert.ToInt32(tokenisedProvisionsOfConsumables[0]));
                    break;
            }

            int minutes = (int)(JourneyEnd - JourneyStart).TotalMinutes;
            int numHours = Enumerable.Range(0, minutes)
                .Select(min => JourneyStart.AddMinutes(min))
                // round to hour due to MGLT being the speed per hour
                .GroupBy(dt => new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0, 0))
                .Count();

            return (Convert.ToInt64(Globals.MGLT) / (numHours * Convert.ToInt64(starship.MGLT))).ToString();
        }

    }
}