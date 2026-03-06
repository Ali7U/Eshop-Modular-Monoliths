namespace Catalog.Data;

public static class InitialData
{
    public static IEnumerable<Product> Products =>
        new List<Product>
        {
            Product.Create(new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61"), "IPhone X", ["category1"],
                "Long description", "Imagefile", 500),
            Product.Create(
                new Guid("6a4422d4-9f76-4e49-9036-bf360b3c1234"),
                "Samsung Galaxy S22",
                ["category1"],
                "Samsung Galaxy S22 with Dynamic AMOLED 120Hz display, 128GB storage.",
                "galaxy-s22.jpg",
                449
            ),

            Product.Create(
                new Guid("77a3ed72-eaa9-4a5b-8d5f-bd3e7f3fd888"),
                "MacBook Air M2",
                ["category2"],
                "MacBook Air with Apple M2 chip, 256GB SSD, 8GB RAM. Lightweight and powerful.",
                "macbook-air-m2.jpg",
                899
            ),

            Product.Create(
                new Guid("9b2e6d1e-4dcb-4f7a-8fc9-7b0b017a4cce"),
                "Sony WH-1000XM5",
                ["category2"],
                "Sony's latest noise-canceling wireless headphones with 30-hour battery life.",
                "sony-wh1000xm5.jpg",
                328
            )
        };
}