using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace product_recommendation
{
    class Program
    {
        private static IDictionary<int, Product> repo = new Repo().productRepo;
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Product ID: ");
                var inputIdArray = new List<int>();
                string input = Console.ReadLine();
                string[] inputIdStrArray = Regex.Split(input, " ");
                foreach (var st in inputIdStrArray)
                {
                    int id;
                    if (!Int32.TryParse(st, out id) || !repo.ContainsKey(id))
                    {
                        Console.WriteLine($"Product {st} doesn't exist.");
                        continue;
                    }
                    Console.WriteLine($"Input product: {st} {repo[id].Name}");
                    inputIdArray.Add(id);
                };

                Console.WriteLine("Recommended products (top 10): ");

                var rule1 = new TopSales();
                var rule2 = new TopScore();
                var rule3 = new SameCategory();
                var ruleConfig = new RuleConfigBuilder().AddRuleToConfig("FMCG", (rule1, 0.5f), (rule2, 0.2f), (rule3, 0.3f))
                .AddRuleToConfig("Electronics", (rule1, 0.1f), (rule2, 0.3f), (rule3, 0.6f))
                .AddRuleToConfig("Fashion", (rule2, 0.1f), (rule3, 0.9f));

                var engine = new Engine(ruleConfig.GetConfig(), repo);
                var recommended = engine.Run(inputIdArray);

                foreach (var item in recommended.Take(10))
                {
                    var pid = item.ProductId;
                    byte[] nameInBytes = JsonSerializer.SerializeToUtf8Bytes<string>(repo[pid].Name);
                    var name = JsonSerializer.Deserialize<string>(nameInBytes);
                    var score = item.RuleWeightSum;
                    var rules = JsonSerializer.Serialize(item.Rules);
                    Console.WriteLine($"ID {pid} {name}, {repo[pid].Category}, ${repo[pid].Price}, recommendation score {score}, recommendation rules {rules}");
                }
                Console.WriteLine("-------");
            }
        }
    }
}
