﻿using System;
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
            int som = ChallengeA1.processNumber(numbers);
            await ChallengeA1.postAsync(client, sampleURL, som);
            //Solve the legit challenge
            Console.WriteLine("Solving puzzle of challenge A1");
            string puzzleURL = "/api/path/1/easy/Puzzle";
            numbers = await ChallengeA1.getAsync(client, puzzleURL);
            if (numbers != null)
            {
                som = ChallengeA1.processNumber(numbers);
                await ChallengeA1.postAsync(client, puzzleURL, som);
            }


        }
    }
}
