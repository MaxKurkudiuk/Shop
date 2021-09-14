namespace Shop.Domain.Infrastructure {
    public static class DecimalExtensions {
        // 1100.50 => 1,100.50 => $ 1,100.50
        public static string GetValueString(this decimal value) => $"$ {value:N2}";
    }
}
