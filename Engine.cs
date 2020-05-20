using System.Linq;
using System.Collections.Generic;

namespace product_recommendation
{
    public class Engine
    {
        public Dictionary<string, (IRecommendationRule rule, float weight)[]> RuleConfig { get; }
        public Dictionary<int, Product> ProductRepo { get; }
        public Engine(Dictionary<string, (IRecommendationRule rule, float weight)[]> ruleConfig, Dictionary<int, Product> productRepo)
        {
            RuleConfig = ruleConfig;
            ProductRepo = productRepo;
        }

        public IEnumerable<Recommended> applyRules(int productId)
        {
            var category = (RuleConfig.ContainsKey(ProductRepo[productId].Category)) ? ProductRepo[productId].Category : "Default";
            return RuleConfig[category].SelectMany(x => x.Item1.recommend(productId, ProductRepo).Select(y => new Recommended(y, x.Item1, x.Item2)));
        }
    }
}
