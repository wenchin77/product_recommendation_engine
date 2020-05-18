using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace product_recommendation
{
    public interface IRecommendationRule
    {
        int RuleId { get; set; }
        IEnumerable<int> recommend(int id, Dictionary<int, Product> productRepo);
    }

    // 實作推薦方法
    public class TopSales : IRecommendationRule
    {
        public int RuleId { get; set; }
        public TopSales(int ruleId)
        {
            RuleId = ruleId;
        }

        public IEnumerable<int> recommend(int id, Dictionary<int, Product> productRepo)
        {
            var exList = new List<int> { id };
            var sorted = productRepo.Select(x => x).Where(x => !exList.Contains(x.Key) && x.Value.QuantitySold >= 500).OrderByDescending(x => x.Value.QuantitySold);
            return sorted.Select(x => x.Key);
        }
    };

    public class TopScore : IRecommendationRule
    {
        public int RuleId { get; set; }
        public TopScore(int ruleId)
        {
            RuleId = ruleId;
        }
        public IEnumerable<int> recommend(int id, Dictionary<int, Product> productRepo)
        {
            var exList = new List<int> { id };
            var sorted = productRepo.Select(x => x).Where(x => !exList.Contains(x.Key) && x.Value.Score >= 4.0).OrderByDescending(x => x.Value.Score);
            return sorted.Select(x => x.Key);
        }
    };

    public class SameCategory : IRecommendationRule
    {
        public int RuleId { get; set; }
        public SameCategory(int ruleId)
        {
            RuleId = ruleId;
        }
        public IEnumerable<int> recommend(int id, Dictionary<int, Product> productRepo)
        {
            var exList = new List<int> { id };
            var category = productRepo[id].Category;
            var result = productRepo.Select(x => x).Where(x => !exList.Contains(x.Key) && x.Value.Category == category);
            return result.Select(x => x.Key);
        }
    };
}
