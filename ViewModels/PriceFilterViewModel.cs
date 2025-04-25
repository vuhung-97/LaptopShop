namespace LaptopShop.ViewModels
{
    public class PriceFilterViewModel
    {
        public List<PriceRange> PriceRanges { get; set; }
        public int? SelectedRangeId { get; set; }

        public PriceFilterViewModel()
        {
            PriceRanges = new List<PriceRange>
            {
                new PriceRange { Id = 1, Name = "Dưới 10 triệu", MinPrice = 0, MaxPrice = 10000000 },
                new PriceRange { Id = 2, Name = "10 - 15 triệu", MinPrice = 10000000, MaxPrice = 15000000 },
                new PriceRange { Id = 3, Name = "15 - 20 triệu", MinPrice = 15000000, MaxPrice = 20000000 },
                new PriceRange { Id = 4, Name = "20 - 30 triệu", MinPrice = 20000000, MaxPrice = 30000000 },
                new PriceRange { Id = 5, Name = "Trên 30 triệu", MinPrice = 30000000, MaxPrice = null }
            };
        }
    }

    public class PriceRange
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
    }
}
