using System.Collections.Generic;
namespace product_recommendation
{
    public class Recommended
    {
        public int Id;
        // { rule no, rule weight }
        public Dictionary<int, float> RuleWeight;
        public Recommended(int id, Dictionary<int, float> ruleWeight)
        {
            Id = id;
            RuleWeight = ruleWeight;
        }
    }
}
