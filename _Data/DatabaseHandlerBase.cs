using System;
using System.Data;

using Mono.Data.Sqlite;
using MySql.Data.MySqlClient;

using TShockAPI;
using TShockAPI.DB;

namespace Terraria.Plugins.CoderCow {
  public abstract class DatabaseHandlerBase: IDisposable {
    #region [Property: DbConnection, SqlType]
    private readonly IDbConnection dbConnection;

    protected IDbConnection DbConnection {
      get { return this.dbConnection; }
    }

    protected SqlType SqlType {
      get { return this.DbConnection.GetSqlType(); }
    }
    #endregion

    #region [Property: WorkQueue]
    private readonly AsyncWorkQueue workQueue;

    protected AsyncWorkQueue WorkQueue {
      get { return this.workQueue; }
    }
    #endregion


    #region [Method: Constructor]
    protected DatabaseHandlerBase(string sqliteDatabaseFilePath) {
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
          throw new NotImplementedException();
      }

      this.workQueue = new AsyncWorkQueue();
    }
    #endregion

    #region [Methods: EnsureDataStructure, GetQueryBuilder]
    public abstract void EnsureDataStructure();

    protected IQueryBuilder GetQueryBuilder() {
      if (this.SqlType == SqlType.Sqlite)
        return new SqliteQueryCreator();
      else
        return new MysqlQueryCreator();
    }
    #endregion

    #region [Method: HandleGameUpdate]
    public void HandleGameUpdate() {
      this.WorkQueue.HandleGameUpdate();
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
        if (this.workQueue != null) 
          this.workQueue.Dispose();
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
