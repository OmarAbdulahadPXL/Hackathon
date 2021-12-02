using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Hackathon
{
    class Time
    {
        public string date1 { get; set; }
        public string date2 { get; set; }
    }
    class ChallengeB1
    {
        public static async Task startChallengeAsync(HttpClient client)
        {
            // De url om de challenge te starten
            // We voeren de call uit en wachten op de response
            // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/await
            // De start endpoint moet je 1 keer oproepen voordat je aan een challenge kan beginnen
            // Krijg je een 403 Forbidden response op je Sample of Puzzle calls? Dat betekent dat de challenge niet gestart is
            HttpResponseMessage response = await client.GetAsync("api/path/2/easy/Start");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Challenge B1 started!");
            }
            else
            {
                Console.WriteLine("Couldn't start B1", response.StatusCode);
            }
        }

        public static async Task<Time> getAsync(HttpClient client, string url)
        {
            Time value = null;
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                JObject val = JObject.Parse(content);
                value = val.ToObject<Time>();
                if (content == "Puzzle has been completed") return value;
            }
            return value;
        }

        public static double processChallenge(Time time)
        {
            int positionD = time.date1.IndexOf('D');
            int day1 = Int32.Parse(time.date1[positionD - 2].ToString() + time.date1[positionD - 1].ToString());

            int positionM = time.date1.IndexOf('M');
            int month1 = Int32.Parse(time.date1[positionM - 2].ToString() + time.date1[positionM - 1].ToString());

            int positionY = time.date1.IndexOf('Y');
            int year1 = Int32.Parse(time.date1[positionY - 4].ToString() +
                time.date1[positionY - 3].ToString() + 
                time.date1[positionY - 2].ToString() + 
                time.date1[positionY - 1].ToString());

            positionD = time.date2.IndexOf('D');
            int day2 = Int32.Parse(time.date2[positionD - 2].ToString() + time.date2[positionD - 1].ToString());

            positionM = time.date2.IndexOf('M');
            int month2 = Int32.Parse(time.date2[positionM - 2].ToString() + time.date2[positionM - 1].ToString());

            positionY = time.date2.IndexOf('Y');
            int year2 = Int32.Parse(time.date2[positionY - 4].ToString() +
                time.date2[positionY - 3].ToString() +
                time.date2[positionY - 2].ToString() +
                time.date2[positionY - 1].ToString());

            int positionh = time.date1.IndexOf('h');

            if (positionh >= 0)
            {
                int hour1 = Int32.Parse(time.date1[positionh - 2].ToString() + time.date1[positionh - 1].ToString());
                int positionm = time.date1.IndexOf('m');
                int min1 = Int32.Parse(time.date1[positionm - 2].ToString() + time.date1[positionm - 1].ToString());

                int positions = time.date1.IndexOf('s');
                int sec1 = Int32.Parse(time.date1[positions - 2].ToString() + time.date1[positions - 1].ToString());

                positionh = time.date2.IndexOf('h');
                int hour2 = Int32.Parse(time.date2[positionh - 2].ToString() + time.date2[positionh - 1].ToString());

                positionm = time.date2.IndexOf('m');
                int min2 = Int32.Parse(time.date2[positionm - 2].ToString() + time.date2[positionm - 1].ToString());

                positions = time.date2.IndexOf('s');
                int sec2 = Int32.Parse(time.date2[positions - 2].ToString() + time.date2[positions - 1].ToString());

                DateTime datehour1 = new DateTime(year1, month1, day1, hour1, min1, sec1);
                DateTime datehour2 = new DateTime(year2, month2, day2, hour2, min2, sec2);
                return (datehour2 - datehour1).TotalSeconds;
            }

            DateTime date1 = new DateTime(year1, month1, day1);
            DateTime date2 = new DateTime(year2, month2, day2);

            return (date2 - date1).TotalSeconds;
        }

        public static async Task postAsync(HttpClient client, string url, double sec)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(url, sec);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
    }
}
