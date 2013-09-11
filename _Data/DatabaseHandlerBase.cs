using System;
using System.Data;
using System.Diagnostics.Contracts;
using Mono.Data.Sqlite;
using MySql.Data.MySqlClient;

using TShockAPI;
using TShockAPI.DB;

namespace Terraria.Plugins.Common {
  public abstract class DatabaseHandlerBase: IDisposable {
    #region [Property: DbConnection, SqlType]
    private IDbConnection dbConnection;

    protected IDbConnection DbConnection {
      get { return this.dbConnection; }
    }

    protected SqlType SqlType {
      get { return this.DbConnection.GetSqlType(); }
    }
    #endregion

    private readonly string sqliteDatabaseFilePath;


    #region [Method: Constructor]
    protected DatabaseHandlerBase(string sqliteDatabaseFilePath) {
      Contract.Requires<ArgumentNullException>(sqliteDatabaseFilePath != null);
      Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(sqliteDatabaseFilePath));

      this.sqliteDatabaseFilePath = sqliteDatabaseFilePath;
    }
    #endregion

    #region [Methods: EnsureDataStructure, GetQueryBuilder]
    public void EstablishConnection() {
      if (this.dbConnection != null)
        throw new InvalidOperationException("Database connection already established.");

      switch (TShock.Config.StorageType.ToLower()) {
        case "mysql":
          string[] host = TShock.Config.MySqlHost.Split(':');
          this.dbConnection = new MySqlConnection(string.Format(
            "Server={0}; Port={1}; Database={2}; Uid={3}; Pwd={4};",
            host[0],
            host.Length == 1 ? "3306" : host[1],
            TShock.Config.MySqlDbName,
            TShock.Config.MySqlUsername,
            TShock.Config.MySqlPassword
          ));

          break;
        case "sqlite":
          this.dbConnection = new SqliteConnection(
            string.Format("uri=file://{0},Version=3", sqliteDatabaseFilePath)
          );

          break;
        default:
          throw new NotSupportedException("Not supported storage type.");
      }
    }

    public virtual void EnsureDataStructure() {
      if (this.SqlType == SqlType.Sqlite) {
        this.DbConnection.Query(string.Concat(
          @"CREATE TABLE IF NOT EXISTS EntityVersion (",
          @"  Name varchar(64) NOT NULL,",
          @"  Version tinyint UNSIGNED NOT NULL DEFAULT 1,",
          @"  PRIMARY KEY (Name)",
          @");"
        ));
      } else if (this.SqlType == SqlType.Mysql) {
        this.DbConnection.Query(string.Concat(
          @"CREATE TABLE IF NOT EXISTS EntityVersion (",
          @"  Name varchar(64) NOT NULL,",
          @"  Version tinyint UNSIGNED NOT NULL DEFAULT 1,",
          @"  PRIMARY KEY (Name)",
          @") ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;"
        ));
      } else {
        throw new NotSupportedException("Not supported storage type.");
      }

      this.AddOrUpdateEntityVersion("EntityVersion", 1);
    }

    protected IQueryBuilder GetQueryBuilder() {
      if (this.SqlType == SqlType.Sqlite)
        return new SqliteQueryCreator();
      else
        return new MysqlQueryCreator();
    }
    #endregion

    #region [Utility Methods]
    protected bool CheckTableExists(string tableName) {
      if (this.SqlType == SqlType.Sqlite) {
        const string QueryString = @"SELECT name FROM sqlite_master WHERE type='table' AND name=@0";
        using (QueryResult reader = this.DbConnection.QueryReader(QueryString, tableName))
          return reader.Read();
      } else if (this.SqlType == SqlType.Mysql) {
        const string QueryString = @"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA=@0 AND TABLE_NAME=@1";
        using (QueryResult reader = this.DbConnection.QueryReader(QueryString, tableName, this.DbConnection.Database))
          return reader.Read();
      } else {
        throw new NotSupportedException("Not supported storage type.");
      }
    }

    protected bool TryGetEntityVersion(string name, out byte version) {
      const string QueryString = @"SELECT Version FROM EntityVersion WHERE Name = @0;";
      using (QueryResult reader = this.DbConnection.QueryReader(QueryString, name)) {
        if (!reader.Read()) {
          version = 0;
          return false;
        }

        version = reader.Get<byte>("Version");
        return true;
      }
    }

    protected void AddOrUpdateEntityVersion(string name, byte version) {
      const string QueryString = 
        @"  INSERT OR IGNORE INTO EntityVersion (Name, Version) VALUES (@0, @1);" + 
        @"  UPDATE EntityVersion SET Version = @1 WHERE Name = @0 AND Version < @1;";

      this.DbConnection.Query(QueryString, name, version);
    }
    #endregion

    #region [IDisposable Implementation]
    private bool isDisposed;

    public bool IsDisposed {
      get { return this.isDisposed; } 
    }

    protected virtual void Dispose(bool isDisposing) {
      if (this.isDisposed)
        return;
    
      if (isDisposing) {
        if (this.dbConnection != null)
          this.dbConnection.Dispose();
      }

      this.isDisposed = true;
    }

    public void Dispose() {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    ~DatabaseHandlerBase() {
      this.Dispose(false);
    }
    #endregion
  }
}
