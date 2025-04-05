using Dapper;
using Lab9Startup.Models;
using MySqlConnector;

namespace Lab9Startup.Services
{
    public class BookDbAccessor
    {
        protected MySqlConnection connection;

        public BookDbAccessor()
        {
            // get environemnt variable
            //string dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            //string dbUser = Environment.GetEnvironmentVariable("DB_USER");
            //string dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
            string dbHost = "localhost";
            string dbUser = "root";
            string dbPassword = "password";

            var builder = new MySqlConnectionStringBuilder
            {
                Server = dbHost,
                UserID = dbUser,
                Password = dbPassword,
                Database = "library", // Use maria db to create a database called library
            };

            connection = new MySqlConnection(builder.ConnectionString);
        }

        /// <summary>
        /// Initialize the database and create the books table
        /// </summary>
        public void InitializeDatabase()
        {
            connection.Open();

            var sqlString = @"CREATE TABLE IF NOT EXISTS books 
                            (
                                BookId VARCHAR(36) PRIMARY KEY,
                                Title VARCHAR(255) NOT NULL,
                                Author VARCHAR(255) NOT NULL,
                                Description TEXT,
                                Category VARCHAR(255)
                            )";

            connection.Execute(sqlString);

            connection.Close();
        }

        /// <summary>
        /// Implement the AddBook method to add a book to the database
        /// </summary>
        /// <param name="book"></param>
        public void AddBook(Book book)
        {
			try
			{
				connection.Open();
				var sqlString = @"INSERT INTO books (BookId, Title, Author, Description, Category) 
                                  VALUES (@BookId, @Title, @Author, @Description, @Category)";
				connection.Execute(sqlString, book);
			}
			finally
			{
				connection.Close();
			}
        }

        /// <summary>
        /// Implement the GetBooks method to get all books from the database
        /// </summary>
        /// <returns></returns>
        public List<Book> GetBooks()
        {
			try
			{
				connection.Open();
				var sqlString = "SELECT * FROM books";
				var books = connection.Query<Book>(sqlString).ToList();
				return books;
			}
			finally
			{
				connection.Close();
			}
		}

        /// <summary>
        /// Implement the GetBook method to get a book from the database
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public Book GetBook(string bookId)
        {
			try
			{
				connection.Open();
				var sqlString = "SELECT * FROM books WHERE BookId = @BookId";
				var book = connection.QuerySingleOrDefault<Book>(sqlString, new { BookId = bookId });
				return book;
			}
			finally
			{
				connection.Close();
			}
		}

        /// <summary>
        /// Implement the UpdateBook method to update a book in the database
        /// </summary>
        /// <param name="book"></param>
        public void UpdateBook(Book book)
        {
			try
			{
				connection.Open();
				var sql = @"UPDATE books SET 
                            Title = @Title, 
                            Author = @Author, 
                            Description = @Description, 
                            Category = @Category
                            WHERE BookId = @BookId";
				connection.Execute(sql, book);
			}
			finally
			{
				connection.Close();
			}
		}

        /// <summary>
        /// Implement the DeleteBook method to delete a book from the database
        /// </summary>
        /// <param name="bookId"></param>
        public void DeleteBook(string bookId)
        {
			try
			{
				connection.Open();
				var sql = "DELETE FROM books WHERE BookId = @BookId";
				connection.Execute(sql, new { BookId = bookId });
			}
			finally
			{
				connection.Close();
			}
		}
    }
}
