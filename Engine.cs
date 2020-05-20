using System.Linq;
using System.Collections.Generic;

namespace product_recommendation
{
    public class Engine
    {
        public int ProductId { get; }
        public Dictionary<string, (IRecommendationRule rule, float weight)[]> RuleConfig { get; }
        public Dictionary<int, Product> ProductRepo { get; }
        public Engine(int productId, Dictionary<string, (IRecommendationRule rule, float weight)[]> ruleConfig, Dictionary<int, Product> productRepo)
        {
            ProductId = productId;
            RuleConfig = ruleConfig;
            ProductRepo = productRepo;
        }

        public IEnumerable<Recommended> applyRules()
        {
            var category = ProductRepo[ProductId].Category;
            foreach (var ruleTuple in RuleConfig[category])
            {
                var rule = ruleTuple.Item1;
                var weight = ruleTuple.Item2;
                return rule.recommend(ProductId, ProductRepo).Select(x => new Recommended(x, rule, weight));
            }
        }
    }
}
