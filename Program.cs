using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Text.Json;

namespace product_recommendation
{
    class Program
    {
        private static Dictionary<int, Product> repo = new Repo().productRepo;
        private static Dictionary<string, (IRecommendationRule rule, float weight)[]> ruleConfig = new Dictionary<string, (IRecommendationRule rule, float weight)[]>();
        private static void AddRuleToConfig(string category, params (IRecommendationRule rule, float weight)[] rules)
        {
            ruleConfig.Add(category, rules);
        }
        static void Main(string[] args)
        {
            Console.WriteLine("輸入商品 ID: ");
            int id;
            while (!Int32.TryParse(Console.ReadLine(), out id) || !repo.ContainsKey(id))
            {
                Console.WriteLine("商品不存在，請重新輸入 ID: ");
            }
            Console.WriteLine($"輸入商品: {repo[id].Name}");
            Console.WriteLine("推薦商品 TOP 10: ");

            var rule1 = new TopSales();
            var rule2 = new TopScore();
            var rule3 = new SameCategory();
            AddRuleToConfig("FMCG", (rule1, 0.5f), (rule2, 0.2f), (rule3, 0.3f));
            AddRuleToConfig("Electronics", (rule1, 0.1f), (rule2, 0.3f), (rule3, 0.6f));
            AddRuleToConfig("Fashion", (rule1, 0), (rule2, 0.1f), (rule3, 0.9f));

            var engine = new Engine(ruleConfig, repo);
            IEnumerable<Recommended> resultList = engine.applyRules(id);
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
                byte[] nameInBytes = JsonSerializer.SerializeToUtf8Bytes<string>(repo[pid].Name);
                var name = JsonSerializer.Deserialize<string>(nameInBytes);
                var score = item.WeightedScore;
                var rules = JsonSerializer.Serialize(item.Rules);
                Console.WriteLine($"ID {pid} {name}, 類別 {repo[pid].Category}, 價格 {repo[pid].Price}, 推薦分數 {score}, 推薦規則 {rules}");
            }
        }
    }
}
