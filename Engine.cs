using System.Xml.Schema;
using System.Linq;
using System.Collections.Generic;

namespace product_recommendation
{
    public class Engine
    {
        public IDictionary<string, (IRecommendationRule rule, float weight)[]> RuleConfig { get; }
        public IDictionary<int, Product> ProductRepo { get; }
        public Engine(IDictionary<string, (IRecommendationRule rule, float weight)[]> ruleConfig, IDictionary<int, Product> productRepo)
        {
            RuleConfig = ruleConfig;
            ProductRepo = productRepo;
        }

        public IEnumerable<Recommended> Run(List<int> productIdArr)
        {
            var list = new List<Recommended>();
            foreach (var pid in productIdArr)
            {
                var rules = RuleConfig.TryGetValue(ProductRepo[pid].Category, out var r)
                ? r : RuleConfig.TryGetValue("Default", out var d)
                ? d : null;

                if (rules == null) continue;

                var productEnum = rules.SelectMany(x => x.Item1.recommend(pid, ProductRepo)
                .Select(p => new Recommended(p, new string[] { x.Item1.RuleDescription }, x.Item2)));
                list.AddRange(productEnum);
            }
            return list.Where(x => !(productIdArr.Any(y => y == x.ProductId)))
                .GroupBy(x => x.ProductId)
                .Select(g => new Recommended(g.Key, g.SelectMany(x => x.Rules).Distinct(), g.Sum(x => x.RuleWeightSum)))
                .OrderByDescending(x => x.RuleWeightSum)
                .ThenBy(x => x.ProductId);
        }
    }
}
