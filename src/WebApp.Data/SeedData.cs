using WebApp.Common.Enums;
using WebApp.Data.Entities;

namespace WebApp.Data
{
    public static class SeedData
    {
        public static readonly Category[] CategoryData =
        {
            // Expense - Food
            new() { Id = 1, Name = "Groceries", CategoryTypeId = (int)CategoryTypeEnum.Food },
            new() { Id = 2, Name = "Dining Out", CategoryTypeId = (int)CategoryTypeEnum.Food },

            // Expense - Travel
            new() { Id = 3, Name = "Fuel", CategoryTypeId = (int)CategoryTypeEnum.Travel },
            new() { Id = 4, Name = "Public Transport", CategoryTypeId = (int)CategoryTypeEnum.Travel },

            // Expense - Bills
            new() { Id = 5, Name = "Electricity", CategoryTypeId = (int)CategoryTypeEnum.Bills },
            new() { Id = 6, Name = "Internet", CategoryTypeId = (int)CategoryTypeEnum.Bills },
            // Income
            new() { Id = 7, Name = "Monthly Salary", CategoryTypeId = (int)CategoryTypeEnum.Salary },
            new() { Id = 8, Name = "Freelance Income", CategoryTypeId = (int)CategoryTypeEnum.Salary },

            // Other
            new() { Id = 9, Name = "Medical", CategoryTypeId = (int)CategoryTypeEnum.Other },
            new() { Id = 10, Name = "Shopping", CategoryTypeId = (int)CategoryTypeEnum.Other }
        };

        public static readonly CategoryType[] CategoryTypeData =
        {
            new() { Id = 1, Name = CategoryTypeEnum.Food.ToString() },
            new() { Id = 2, Name = CategoryTypeEnum.Travel.ToString() },
            new() { Id = 3, Name = CategoryTypeEnum.Bills.ToString() },
            new() { Id = 4, Name = CategoryTypeEnum.Salary.ToString() },
            new() { Id = 5, Name = CategoryTypeEnum.Other.ToString() }
        };
    }
}
