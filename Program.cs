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

                var resultList = engine.applyRules();
                // Console.WriteLine(JsonSerializer.Serialize(resultList));

                resultList.GroupBy(a => a.Id).Select(a => new { Weight = a.Sum(b => b.RuleWeight.Values), Name = a.Key}).

                foreach (var item in resultList)
                {
                    byte[] nameInBytes = JsonSerializer.SerializeToUtf8Bytes<string>(repo[item.Id].Name);
                    Console.Write($"商品 ID {item.Id}, 名稱 {JsonSerializer.Deserialize<string>(nameInBytes)} - ");
                    foreach (var ruleWeight in item.RuleWeight)
                    {
                        Console.WriteLine($"規則 #{ruleWeight.Key.ToString()}, 權重 {ruleWeight.Value}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
