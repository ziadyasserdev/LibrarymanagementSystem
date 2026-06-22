using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Common.Caching
{
    public static class CacheKeys
    {
        public static class Books
        {
            public const string AllBooks = "all_books";
            public static string ByCategoryName(string name)
      => $"BooksByCategory_{name}";
            public static string ByAuthorName(string name)
      => $"BooksByAuthor_{name}";
            public static string BookById(int id) => $"book_{id}";
        }

        public static class Categories
        {
            public const string All = "AllCategories";
            public static string ById(int id) => $"CategoryById{id}";
        }
        public static class Authors
        {
            public const string All = "AllAuthors";

            public static string ById(int id)
                => $"AuthorById_{id}";

            public static string ByName(string name)
                => $"AuthorByName_{name}";

            public static string ByCategory(string category)
                => $"AuthorsByCategory_{category}";
        }
    }
}
