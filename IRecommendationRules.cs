using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace product_recommendation
{
    public interface IRecommendationRule
    {
        int RuleId { get; }
        string RuleDescription { get; }
        IEnumerable<int> recommend(int id, IDictionary<int, Product> productRepo);
    }
    // 實作推薦方法
    public class TopSales : IRecommendationRule
    {
        public int RuleId => 1;
        public string RuleDescription => "Quantity sold over 500";
        public IEnumerable<int> recommend(int id, IDictionary<int, Product> productRepo)
        {
            var exList = new List<int> { id };
            var sorted = productRepo.Select(x => x).Where(x => !exList.Contains(x.Key) && x.Value.QuantitySold >= 500).OrderByDescending(x => x.Value.QuantitySold);
            return sorted.Select(x => x.Key);
        }
    };

    public class TopScore : IRecommendationRule
    {
        public int RuleId => 2;
        public string RuleDescription => "Score over 4.0";
        public IEnumerable<int> recommend(int id, IDictionary<int, Product> productRepo)
        {
            var exList = new List<int> { id };
            var sorted = productRepo.Select(x => x).Where(x => !exList.Contains(x.Key) && x.Value.Score >= 4.0).OrderByDescending(x => x.Value.Score);
            return sorted.Select(x => x.Key);
        }
    };

    public class SameCategory : IRecommendationRule
    {
        public int RuleId => 3;
        public string RuleDescription => "Products within the same category";
        public IEnumerable<int> recommend(int id, IDictionary<int, Product> productRepo)
        {
            var exList = new List<int> { id };
            var category = productRepo[id].Category;
            var result = productRepo.Select(x => x).Where(x => !exList.Contains(x.Key) && x.Value.Category == category);
            return result.Select(x => x.Key);
        }
    };
}
