using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace DD {
    public static class DbContextExtensions {

        public static IEnumerable<DbSet> GetTables (this DbContext db) {
            var tables = new List<DbSet> ();
            var propInfos = db.GetType ().GetProperties ();
            foreach (var propInfo in propInfos) {
                if (propInfo.PropertyType.IsGenericType
                    && propInfo.PropertyType.GetGenericTypeDefinition () == typeof (System.Data.Entity.DbSet<>)) {
                    tables.Add ((dynamic)propInfo.GetValue (db, null));
                }
            }

            return tables;
        }

        public static int GetTableCount (this DbContext db) {
            return db.GetTables().Count();
        }

        public static string GetTableName (this DbSet set) {
            // とりあえず DbSet<T> の Tの名前を返しているが
            // 将来的にどするか不明
            var args = set.GetType ().GetGenericArguments ();
            return args[0].Name;
        }
    }
}
