using System.Collections.Generic;

namespace product_recommendation
{
    public class Product
    {
        public int Id;
        public string Name;
        public string Category;
        public int Price;
        public int Stock;
        public int QuantitySold;
        public double Score;
        public Product(int id, string name, string category, int price, int stock, int quantitySold, double score)
        {
            Id = id;
            Name = name;
            Category = category;
            Price = price;
            Stock = stock;
            QuantitySold = quantitySold;
            Score = score;
        }
    }

    public class Repo
    {
        // to be updated: make this into an interface (w/ GetProduct method) -> DI
        public IDictionary<int, Product> productRepo = new Dictionary<int, Product>
        {
            { 1, new Product(1, "Tissue", "FMCG", 100, 1000, 100, 4.1) },
            { 2, new Product(2, "Instant noodles", "FMCG", 200, 2000, 1000, 4.2) },
            { 3, new Product(3, "Cookies", "FMCG", 300, 3000, 2000, 3.3) },
            { 4, new Product(4, "Juice", "FMCG", 400, 4000, 100, 4.4) },
            { 5, new Product(5, "Mobile", "Electronics", 30000, 100, 3500, 4.9) },
            { 6, new Product(6, "Watch", "Electronics", 20000, 200, 2500, 4.8) },
            { 7, new Product(7, "Computer", "Electronics", 10000, 300, 100, 3.7) },
            { 8, new Product(8, "Laptop", "Electronics", 70000, 400, 250, 4.6) },
            { 9, new Product(9, "Fan", "Electronics", 7000, 500, 150, 4.5) },
            { 10, new Product(10, "Earphones", "Electronics", 5000, 600, 550, 4.4) },
            { 11, new Product(11, "Dryer", "Electronics", 3000, 700, 350, 4.3) },
            { 12, new Product(12, "Shirt", "Fashion", 8000, 500, 700, 3.5) },
            { 13, new Product(13, "Vest", "Fashion", 800, 600, 600, 3.6) },
            { 14, new Product(14, "Pants", "Fashion", 1500, 700, 5800, 4.2) },
            { 15, new Product(15, "Coat", "Fashion", 2500, 800, 4800, 3.8) },
            { 16, new Product(16, "Dress", "Fashion", 3500, 900, 300, 3.9) },
            { 17, new Product(17, "Suit", "Fashion", 4500, 1000, 200, 4.0) },
            { 18, new Product(18, "Hat", "Fashion", 5500, 160, 100, 3.0) },
            { 19, new Product(19, "Puzzle", "Entertainment", 6500, 170, 500, 4.3) },
            { 20, new Product(20, "Lego", "Entertainment", 7500, 180, 400, 4.6) }
        };

    }
}
