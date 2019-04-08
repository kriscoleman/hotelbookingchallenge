namespace Models
{
    public class LineItem
    {
        public decimal Cost { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }

        /// <summary>
        /// Overriding equals to help our tests.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is LineItem lineItem)) return false;

            return Cost == lineItem.Cost && Name == lineItem.Name && Quantity == lineItem.Quantity;
        }
    }
}