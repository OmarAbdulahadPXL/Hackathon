using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Hackathon
{
    class ChallengeA1
    {
        public static async Task startChallengeAsync(HttpClient client)
        {
            // De url om de challenge te starten
            // We voeren de call uit en wachten op de response
            // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/await
            // De start endpoint moet je 1 keer oproepen voordat je aan een challenge kan beginnen
            // Krijg je een 403 Forbidden response op je Sample of Puzzle calls? Dat betekent dat de challenge niet gestart is
            HttpResponseMessage response = await client.GetAsync("api/path/1/easy/Start");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Challenge A1 started!");
            }
            else
            {
                Console.WriteLine("Couldn't start A1", response.StatusCode);
            }
        }

        public static async Task<List<int>> getAsync(HttpClient client, string url)
        {
            List<int> numbers = null;
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
                if (content == "Puzzle has been completed") return numbers;
                numbers = getNumbers(content);
            }
            return numbers;
        }

        public static int processNumber(List<int> numbers)
        {
            int som = 0;
            foreach (int i in numbers)
            {
                som += i;
            }
            Console.WriteLine(som);
            return addNumbertillOneDigit(som);
        }

        public static async Task postAsync(HttpClient client, string url, int som)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(url, som);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        private static int addNumbertillOneDigit(int som)
        {
            string somString = som.ToString();
            int length = somString.Length;
            som = 0;
            if (length > 1)
            {
                foreach (char i in somString)
                {
                    som += Int32.Parse(i.ToString());
                }
                Console.WriteLine(som);
                somString = som.ToString();
                if (somString.Length > 1)
                {
                    som = addNumbertillOneDigit(som);
                }
            }
            return som;
        }

        private static List<int> getNumbers(string content)
        {
            List<int> numbers = new List<int>();
            content = content.Remove(0, 1);
            content = content.Remove(content.Length - 1, 1);
            foreach (string number in content.Split(','))
            {
                numbers.Add(Int32.Parse(number));
            }
            return numbers;
        }
    }
}
