using System.Collections.Generic;
namespace product_recommendation
{
    public class Recommended
    {
        public int ProductId { get; }
        public IRecommendationRule Rule { get; }
        public float RuleWeight { get; }
        public Recommended(int ProductId, IRecommendationRule Rule, float RuleWeight)
        {
            this.ProductId = ProductId;
            this.Rule = Rule;
            this.RuleWeight = RuleWeight;
        }

    }
}
