namespace 配置系统集成
{
    public class BookService
    {
        public async Task<Book?> GetBookByIdAsync(int i)
        {
            return await Task.FromResult(GetBookById(i));
        }

        public Book? GetBookById (int id)
        {
            switch (id)
            {
                case 0:
                    return new Book() { Id = 0, BookName = "钢铁是怎么练成的", Price = 88 };
                case 1:
                    return new Book() { Id = 1, BookName = "富爸爸穷爸爸", Price = 66 };
                case 2:
                    return new Book() { Id = 2, BookName = "数据结构与算法", Price = 77 };
                default:
                    return null;
            }
        }

        public IEnumerable<Book> GetAllBooks()
        {
            yield return new Book() { Id = 0, BookName = "钢铁是怎么练成的", Price = 88 };
            yield return new Book() { Id = 1, BookName = "富爸爸穷爸爸", Price = 66 };
            yield return new Book() { Id = 2, BookName = "数据结构与算法", Price = 77 };
        }
    }

    public record Book
    {
        public int Id { get; set; }

        public string? BookName { get; set; }

        public double Price { get; set; }
    }
}
