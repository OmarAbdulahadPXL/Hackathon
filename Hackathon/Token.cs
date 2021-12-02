using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Hackathon
{
    class Token
    {

        public static async Task<string> getTokenAsync(HttpClient client)
        {
            // De token die je gebruikt om je team te authenticeren, deze kan je via de swagger ophalen met je teamname + passwor
            string token = null;
            HttpResponseMessage response = await client.GetAsync("/api/team/Token?teamname=awesomesink&password=natuurproduct");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Got the token!");
                token = await response.Content.ReadAsStringAsync();
            }
            return token;
        }
        
    }
}
