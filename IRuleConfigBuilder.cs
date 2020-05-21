using System.Collections.Generic;

namespace product_recommendation
{
    // Builder pattern
    public interface IRuleConfigBuilder
    {
        IRuleConfigBuilder AddRuleToConfig(string category, params (IRecommendationRule rule, float weight)[] rules);
        IDictionary<string, (IRecommendationRule rule, float weight)[]> GetConfig();
    }
    public class RuleConfigBuilder : IRuleConfigBuilder
    {
        private IDictionary<string, (IRecommendationRule rule, float weight)[]> _ruleConfig = new Dictionary<string, (IRecommendationRule rule, float weight)[]>();
        public RuleConfigBuilder()
        {
            this.Reset();
        }
        private void Reset()
        {
            this._ruleConfig = new Dictionary<string, (IRecommendationRule rule, float weight)[]>();
            AddRuleToConfig("Default", (new TopSales(), 0.4f), (new SameCategory(), 0.6f));
        }
        public IRuleConfigBuilder AddRuleToConfig(string category, params (IRecommendationRule rule, float weight)[] rules)
        {
            this._ruleConfig.Add(category, rules);
            return this;
        }
        public IDictionary<string, (IRecommendationRule rule, float weight)[]> GetConfig()
        {
            return this._ruleConfig;
        }
    }
}