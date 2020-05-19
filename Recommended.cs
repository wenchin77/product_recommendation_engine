using System.Collections.Generic;
namespace product_recommendation
{
    public class Recommended
    {
        public int Id;
        public int RuleId;
        public float RuleWeight;
        public Recommended(int id, int ruleId, float ruleWeight)
        {
            Id = id;
            RuleId = ruleId;
            RuleWeight = ruleWeight;
        }
    }
}
