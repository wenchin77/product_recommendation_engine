using System.Collections;
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
            try
            {
                var rule1 = new TopSales();
                var rule2 = new TopScore();
                var rule3 = new SameCategory();
                var ruleConfig = new Dictionary<string, Dictionary<IRecommendationRule, float>>()
                {
                    {
                        "FMCG", new Dictionary<IRecommendationRule, float>()
                        {
                            { rule1, 0.5f },
                            { rule2, 0.2f },
                            { rule3, 0.3f }
                        }
                    },
                    {
                        "Electronics", new Dictionary<IRecommendationRule, float>()
                        {
                            { rule1, 0.1f },
                            { rule2, 0.3f },
                            { rule3, 0.6f }
                        }
                    },
                    {
                        "Fashion", new Dictionary<IRecommendationRule, float>()
                        {
                            { rule1, 0 },
                            { rule2, 0.1f },
                            { rule3, 0.9f }
                        }
                    },
                };

                // to be updated -> DI
                var repo = new Repo().productRepo;

                Console.WriteLine("輸入商品 ID: ");
                int id = Int32.Parse(Console.ReadLine());
                if (!repo.ContainsKey(id))
                {
                    Console.WriteLine("商品不存在");
                    return;
                }

                Console.WriteLine($"輸入商品: {repo[id].Name}");
                Console.WriteLine("推薦商品: ");

                Engine engine = new Engine(id, ruleConfig, repo);
                IEnumerable<Recommended> resultList = engine.applyRules();

                var recommended = (from result in resultList
                                   group result by new { result.ProductId } into g
                                   select new
                                   {
                                       Id = g.Key.ProductId,
                                       WeightedScore = g.Sum(result => result.RuleWeight),
                                       Rules = new ArrayList() { g.Select(x => x.Rule.RuleDescription) },
                                   });
                foreach (var item in recommended.OrderByDescending(x => x.WeightedScore).ThenBy(x => x.Id).Take(10))
                {
                    var pid = item.Id;
                    byte[] nameInBytes = JsonSerializer.SerializeToUtf8Bytes<string>(repo[item.Id].Name);
                    var name = JsonSerializer.Deserialize<string>(nameInBytes);
                    var score = item.WeightedScore;
                    var rules = JsonSerializer.Serialize(item.Rules);
                    Console.WriteLine($"ID {pid} {name}, 推薦分數 {score}, 推薦規則 {rules}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
