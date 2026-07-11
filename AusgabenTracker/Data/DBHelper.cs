using AusgabenTracker.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace AusgabenTracker.Data
{
    public class DBHelper(string connectionString)
    {
        private readonly string _connectionString = connectionString;

        public async Task<List<Category>> GetCategoriesAsync()
        {
            using SqlConnection connection = new(_connectionString);

            string sql = "SELECT Id, Name FROM Category";

            var categories = await connection.QueryAsync<Category>(sql);
            return [.. categories];
        }

        public async Task<List<Expense>> GetAllExpensesAsync()
        {
            using SqlConnection connection = new(_connectionString);

            string sql = @"
                SELECT e.Id, Description, Amount, Date, c.Name as CategoryName 
                FROM Expense as e 
                INNER JOIN Category as c on c.id = e.categoryId";

            var expenses = await connection.QueryAsync<Expense>(sql);
            return [.. expenses];
        }

        public async Task AddExpenseAsync(Expense expense)
        {
            using SqlConnection connection = new(_connectionString);

            string sql = @"
                INSERT INTO Expense (Description, Amount, Date, CategoryId) 
                VALUES (@Description, @Amount, @Date, @CategoryId)";

            await connection.ExecuteAsync(sql, expense);
        }

        public async Task DeleteExpenseAsync(Expense expense)
        {
            using SqlConnection connection = new(_connectionString);

            string sql = @"
                DELETE FROM Expense WHERE id = @Id";

            await connection.ExecuteAsync(sql, expense);
        }
    }
}

