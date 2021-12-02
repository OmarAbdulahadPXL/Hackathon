using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hackathon
{
    class Lift
    {
        public int start { get; set; }
        public int destination { get; set; }
    }
    class ChallengeA2
    {
        private static List<int> listOfSteps = new List<int>();

        public static async Task startChallengeAsync(HttpClient client)
        {
            // De url om de challenge te starten
            // We voeren de call uit en wachten op de response
            // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/await
            // De start endpoint moet je 1 keer oproepen voordat je aan een challenge kan beginnen
            // Krijg je een 403 Forbidden response op je Sample of Puzzle calls? Dat betekent dat de challenge niet gestart is
            HttpResponseMessage response = await client.GetAsync("api/path/1/medium/Start");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Challenge A2 started!");
            }
            else
            {
                Console.WriteLine("Couldn't start A2", response.StatusCode);
            }
        }

        public static async Task<Lift> getAsync(HttpClient client, string url)
        {
            Lift value = null;
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                JObject val = JObject.Parse(content);
                value = val.ToObject<Lift>();
                if (content == "Puzzle has been completed") return value;
            }
            return value;
        }
        public static List<int> processChallenge(int start, int des)
        {
            List<int> stepsList = new List<int>();
            bool isNeg = true;
            int current = start;
            int step = 1;
            stepsList.Add(current);
            while (current != des)
            {
                step = step * -1;
                current += step;
                stepsList.Add(current);
                if (isNeg)
                {
                    step--;
                    isNeg = false;
                }
                else
                {
                    step++;
                    isNeg = true;
                }
                
            }
            return stepsList;
        }

        public static async Task postAsync(HttpClient client, string url, List<int> steps)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(url, steps);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
    }
}
