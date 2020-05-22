using System.Collections.Generic;
namespace product_recommendation
{
    public class Recommended
    {
        public int ProductId { get; }
        public IEnumerable<string> Rules { get; }
        public float RuleWeightSum { get; }
        public Recommended(int ProductId, IEnumerable<string> Rules, float RuleWeightSum)
        {
            this.ProductId = ProductId;
            this.Rules = Rules;
            this.RuleWeightSum = RuleWeightSum;
        }

    }
}
