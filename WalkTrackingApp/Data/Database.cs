using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace WalkTrackingApp.Data
{
    public class Database
    {
        readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<runData>().Wait();
        }

        public Task<List<runData>> GetRunAsync()
        {
            return _database.Table<runData>().ToListAsync();
        }

        public Task<int> SaveRunAsync(runData run)
        {
            return _database.InsertAsync(run);
        }

        public Task<int> DeleteRunAsync(runData item)
        {
            return _database.DeleteAsync(item);
        }
        public Task<List<runData>> getRows()
        {
            // SQL queries are also possible
            return _database.QueryAsync<runData>("SELECT * FROM [runData]");
        }


    }
}
