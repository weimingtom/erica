using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace DD {
    public class DatabaseManager : Component, IDisposable {
        #region Field
        Dictionary<string, DbContext> dbs;
        #endregion

        #region Constructor
        public DatabaseManager () {
            this.dbs = new Dictionary<string, DbContext> ();
        }
        #endregion

        #region Property
        public int DataBaseCount  {
            get { return dbs.Count(); }
        }

        public IEnumerable<KeyValuePair<string, DbContext>> DataBases {
            get { return dbs; }
        }

        #endregion


        #region Method
        public void AddDataBase (string name, DbContext db) {
            if (name == null || name == "") {
                throw new ArgumentNullException ("Name is null or empty");
            }
            if (db == null) {
                throw new ArgumentNullException ("Database is null");
            }
            this.dbs.Add (name, db);
        }

        public void RemoveDataBase (string name) {
            this.dbs.Remove (name);
        }

        public IEnumerable<string> GetDataBaseNames () {
            return dbs.Keys;
        }

        public DbContext GetDataBase (string name) {
            return dbs[name];
        }


        public T GetDataBase<T> () where T : DbContext {
            return (from db in dbs.Values
                    where db is T
                    select db as T).FirstOrDefault ();
        }

        public void SaveAllChanges () {
            foreach (var db in dbs.Values) {
                db.SaveChanges ();
            }
        }

        public void Dispose () {
            if (dbs != null) {
                foreach (var db in dbs.Values) {
                    db.SaveChanges ();
                    db.Dispose ();
                }
                this.dbs = null;
            }
        }

        void IDisposable.Dispose () {
            Dispose ();
        }

        #endregion

    }
}
