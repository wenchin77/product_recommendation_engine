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
        // public IEnumerable<Recommended> Run(int productId)
        // {
        //     var rules = RuleConfig.TryGetValue(ProductRepo[productId].Category, out var r)
        //     ? r : RuleConfig.TryGetValue("Default", out var d)
        //     ? d : null;
        //     if (rules == null) return Enumerable.Empty<Recommended>();
        //     return rules.SelectMany(x => x.Item1.recommend(productId, ProductRepo).Select(y => new Recommended(y, x.Item1, x.Item2)));
        // }

        public IEnumerable<Recommended> Run(int[] productIdArr)
        {
            var list = new List<Recommended>();
            foreach (var pid in productIdArr)
            {
                // 拿出該商品的規則們
                var rules = RuleConfig.TryGetValue(ProductRepo[pid].Category, out var r)
                ? r : RuleConfig.TryGetValue("Default", out var d)
                ? d : null;

                // 用商品規則去篩選商品，產出 Recommended 回傳到 list
                if (rules != null)
                {
                    var p = rules.SelectMany(x => x.Item1.recommend(pid, ProductRepo).Select(y => new Recommended(y, x.Item1, x.Item2)));
                    list.AddRange(p);
                }
            }

            // 篩掉是 productIdArr 裡面的商品 ++

            var recommended = from p in list
                        group p by p.ProductId into g
                        select new
                        {
                            ProductId = g.Key,
                            RuleWeight = g.Sum(x => x.RuleWeight),
                            rules = g.Select(x => x.Rule.RuleDescription)
                        };
            foreach (var item in recommended)
            {
                yield return item;
            }
        }
    }
}
