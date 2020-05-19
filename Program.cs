using System.Linq;
using System.Collections.Generic;
using System;
using System.Text.Json;

namespace product_recommendation
{
    class Program
    {
        static void Main(string[] args)
        {
            var ruleDict = new Dictionary<IRecommendationRule, float>()
            {
                { new TopSales(1), 0.5f },
                { new TopScore(2), 0.2f },
                { new SameCategory(3), 0.3f }
            };

            // to be updated -> DI
            var repo = new Repo().productRepo;

            try
            {
                Console.WriteLine("輸入商品 id: ");
                int id = Int32.Parse(Console.ReadLine());
                if (!repo.ContainsKey(id))
                {
                    Console.WriteLine("商品不存在");
                    return;
                }

                Engine engine = new Engine(id, ruleDict, repo);
                IEnumerable<Recommended> resultList = engine.applyRules();

                var recommended = (from result in resultList group result by new { result.Id } into g 
                select new { Id = g.Key.Id, WeightedScore = g.Sum(result => result.RuleWeight) }).ToArray();
                
                foreach (var item in recommended.OrderByDescending(x => x.WeightedScore).ThenBy(x => x.Id).Take(10))
                {
                    byte[] nameInBytes = JsonSerializer.SerializeToUtf8Bytes<string>(repo[item.Id].Name);
                    Console.WriteLine($"商品 ID {item.Id}, 名稱 {JsonSerializer.Deserialize<string>(nameInBytes)}, 推薦分數 {item.WeightedScore}");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
