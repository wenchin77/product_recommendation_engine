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
        private IEnumerable<Recommended> applyRules(int productId, (IRecommendationRule rule, float weight)[] rules)
        {
            return rules.SelectMany(x => x.Item1.recommend(productId, ProductRepo).Select(y => new Recommended(y, x.Item1, x.Item2)));
        }
        public IEnumerable<Recommended> run(int productId)
        {   
            return RuleConfig.TryGetValue(ProductRepo[productId].Category, out var rule)
            ? applyRules(productId, rule) : RuleConfig.TryGetValue("Default", out var defaultRule) 
            ? applyRules(productId, defaultRule) : Enumerable.Empty<Recommended>();
        }

        // public IEnumerable<Recommended> applyRules(int productId)
        // {   
            // (IRecommendationRule rule, float weight)[] rules;
            // if (!RuleConfig.TryGetValue(ProductRepo[productId].Category, out rules))
            // {
            //     if (!RuleConfig.TryGetValue("Default", out rules))
            //     {
            //         return Enumerable.Empty<Recommended>();
            //     }
            // }
            // return rules.SelectMany(x => x.Item1.recommend(productId, ProductRepo).Select(y => new Recommended(y, x.Item1, x.Item2)));
        // }
    }
}
