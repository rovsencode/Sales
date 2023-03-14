namespace FirelloProject.Models.demo
{
    public class Genre
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public List<BookGenre>? Genres { get;set; }

    }
}
