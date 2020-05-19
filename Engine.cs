using System.Collections.Generic;

namespace product_recommendation
{
    public class Engine
    {
        public int Id;
        public Dictionary<string, Dictionary<IRecommendationRule, float>> RuleConfig;
        public Dictionary<int, Product> ProductRepo;
        public Engine(int id, Dictionary<string, Dictionary<IRecommendationRule, float>> ruleConfig, Dictionary<int, Product> productRepo)
        {
            Id = id;
            RuleConfig = ruleConfig;
            ProductRepo = productRepo;
        }

        public IEnumerable<Recommended> applyRules()
        {
            var category = ProductRepo[Id].Category;
            foreach (var rule in RuleConfig[category].Keys)
            {
                IEnumerable<int> recommended = rule.recommend(Id, ProductRepo);
                foreach (var item in recommended)
                {
                    var weight = RuleConfig[category][rule];
                    Recommended result = new Recommended(item, rule, weight);
                    yield return result;
                }
            }
        }
    }
}
