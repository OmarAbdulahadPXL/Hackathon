using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Hackathon
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Swagger
            // https://involved-htf-challenge.azurewebsites.net/swagger/index.html

            // De httpclient die we gebruiken om http calls te maken
            var client = new HttpClient();
            // De base url die voor alle calls hetzelfde is
            client.BaseAddress = new Uri("http://involved-htf-challenge.azurewebsites.net");
            // De token die je gebruikt om je team te authenticeren, deze kan je via de swagger ophalen met je teamname + passwor
            string token = await Token.getTokenAsync(client);
            // We stellen de token in zodat die wordt meegestuurd bij alle calls, anders krijgen we een 401 Unauthorized response op onze calls
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            //start challenge A1
            await ChallengeA1.startChallengeAsync(client);
            //Solve the sample challenge
            string sampleURL = "/api/path/1/easy/Sample";
            Console.WriteLine("Solving sample of challeng A1");
            List<int> numbers = await ChallengeA1.getAsync(client, sampleURL);
            int som = ChallengeA1.processChallenge(numbers);
            await ChallengeA1.postAsync(client, sampleURL, som);
            //Solve the legit challenge
            Console.WriteLine("Solving puzzle of challenge A1");
            string puzzleURL = "/api/path/1/easy/Puzzle";
            numbers = await ChallengeA1.getAsync(client, puzzleURL);
            if (numbers != null)
            {
                som = ChallengeA1.processChallenge(numbers);
                await ChallengeA1.postAsync(client, puzzleURL, som);
            }

            //start challenge A2
            await ChallengeA2.startChallengeAsync(client);
            //Challenge sample
            sampleURL = "/api/path/1/medium/Sample";
            Lift value = await ChallengeA2.getAsync(client, sampleURL);
            List<int> steps = ChallengeA2.processChallenge(value.start, value.destination);
            await ChallengeA2.postAsync(client, sampleURL, steps);
            //Challenge puzzle
            puzzleURL = "/api/path/1/medium/Puzzle";
            value = await ChallengeA2.getAsync(client, puzzleURL);
            if (value != null)
            {
                steps = ChallengeA2.processChallenge(value.start, value.destination);
                await ChallengeA2.postAsync(client, puzzleURL, steps);
            }

            //start challenge B1
            await ChallengeB1.startChallengeAsync(client);
            //challenge sample
            sampleURL = "/api/path/2/easy/Sample";
            Time valueTime = await ChallengeB1.getAsync(client, sampleURL);
            Console.WriteLine(valueTime.date1);
            Console.WriteLine(valueTime.date2);
            double sec = ChallengeB1.processChallenge(valueTime);
            await ChallengeB1.postAsync(client, sampleURL, sec);

            puzzleURL = "/api/path/2/easy/Puzzle";
            valueTime = await ChallengeB1.getAsync(client, puzzleURL);
            if (valueTime != null)
            {
                Console.WriteLine(valueTime.date1);
                Console.WriteLine(valueTime.date2);
                sec = ChallengeB1.processChallenge(valueTime);
                await ChallengeB1.postAsync(client, puzzleURL, sec);
            }
        }
    }
}
