using System.Collections.Generic;

namespace product_recommendation
{
    public class Engine
    {
        public int Id;
        public Dictionary<IRecommendationRule, float> RuleDict;
        public Dictionary<int, Product> ProductRepo;
        public Engine(int id, Dictionary<IRecommendationRule, float> ruleDict, Dictionary<int, Product> productRepo)
        {
            Id = id;
            RuleDict = ruleDict;
            ProductRepo = productRepo;
        }

        public IEnumerable<Recommended> applyRules()
        {
            // + 不同類別規則套用的邏輯
            foreach (var rule in RuleDict.Keys)
            {
                IEnumerable<int> recommended = rule.recommend(Id, ProductRepo);
                foreach (var item in recommended)
                {
                    // 要回傳的商品格式為 Recommended
                    Recommended result = new Recommended(item, new Dictionary<int, float>(){
                        { rule.RuleId, RuleDict[rule] }
                    });
                    yield return result;
                }
            }
        }
    }
}
