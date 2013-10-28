using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace DD {
    /// <summary>
    /// DbContextの拡張
    /// </summary>
    /// <remarks>
    /// <see cref="DbContext"/> の機能はかなり少ない。
    /// せめて表（DbSet）の一覧ぐらいは標準で提供して欲しいのだが・・・
    /// </remarks>
    public static class DbContextExtensions {

        /// <summary>
        /// 全ての表 <see cref="DbSet"/> の取得
        /// </summary>
        /// <remarks>
        /// このデータベースコンテキストで定義されている表（<see cref="DbSet"/>）を
        /// 全て取得します。
        /// </remarks>
        /// <param name="db">データベース コンテキスト</param>
        /// <returns></returns>
        public static IEnumerable<DbSet> GetDbSets (this DbContext db) {
            if (db == null) {
                throw new ArgumentNullException ("Database context is null");
            }
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

        /// <summary>
        /// 表 <see cref="DbSet"/> の個数の取得
        /// </summary>
        /// <param name="db">データベースコンテキスト</param>
        /// <returns></returns>
        public static int GetDbSetCount (this DbContext db) {
            return db.GetDbSets().Count();
        }
        
        /// <summary>
        /// 表 <see cref="DbSet"/> の名前の取得 
        /// </summary>
        /// <remarks>
        /// Entity Frameworkで「表の名前」として明確に定義された物はない。
        /// とりあえず <see cref="DbSet{T}"/> の Tの名前を返しているが将来的にどうするか不明。
        /// </remarks>
        /// <param name="set">表 <see cref="DbSet"/></param>
        /// <returns></returns>
        public static string GetDbSetName (this DbSet set) {
            var args = set.GetType ().GetGenericArguments ();
            return args[0].Name;
        }
        
    }
}
