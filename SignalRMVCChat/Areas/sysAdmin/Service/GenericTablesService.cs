using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;

namespace TelegramBotsWebApplication.Areas.Admin.Service
{
    public class GenericTablesService
    {
        private readonly DbContext db;

        public GenericTablesService(DbContext context)
        {
            db = context;
        }
        public List<string> GetAllTableNames()
        {
            List<string> tableNameList = new List<string>();
                var metadata = ((IObjectContextAdapter)db).ObjectContext.MetadataWorkspace;

                var tables = metadata.GetItemCollection(DataSpace.SSpace)
                    .GetItems<EntityContainer>()
                    .Single()
                    .BaseEntitySets
                    .OfType<EntitySet>()
                    .Where(s => !s.MetadataProperties.Contains("Type")
                                || s.MetadataProperties["Type"].ToString() == "Tables");

                foreach (var table in tables)
                {
                    var tableName = table.MetadataProperties.Contains("Table")
                                    && table.MetadataProperties["Table"].Value != null
                        ? table.MetadataProperties["Table"].Value.ToString()
                        : table.Name;

               //     var tableSchema = table.MetadataProperties["Schema"].Value.ToString();

                    tableNameList.Add( tableName);
                }

                return tableNameList;
        }

        public Dictionary<string, string> SetNames(List<string> list, Dictionary<string, string> tableNamesDictionary)
        {
            Dictionary<string, string> namesDictionary = new Dictionary<string, string>();
            foreach (var table in list)
            {
                var _key= tableNamesDictionary.Keys.FirstOrDefault(k => k == table);

                if (string.IsNullOrEmpty(_key))
                {
                    continue;
                }

                namesDictionary.Add(_key,tableNamesDictionary[_key]);

            }

            return namesDictionary;
        }

        public TableViewModel ReadAllDataWithStruture(string table)
        {

                var sqlText = string.Format("SELECT * FROM {0}", table);

                DataTable dt = new DataTable();

                // Use DataTables to extract the whole table in one hit
                using (SqlDataAdapter da = new SqlDataAdapter(sqlText, db.Database.Connection.ConnectionString))
                {
                    da.Fill(dt);
                }

                /*var tableData = new Dictionary<DataColumn, EntityList<object>>();

                // Go through all columns, retrieving their names and populating the rows
                foreach (DataColumn dc in dt.Columns)
                {
                    string columnName = dc.ColumnName;
                   var rowData = new EntityList<object>();

                   tableData.Add(dc, rowData);

                    foreach (DataRow dr in dt.Rows)
                    {
                        rowData.Add(dr[columnName]);
                    }
                }*/

                return new TableViewModel
                {
                    DataTable = dt
                };

                /*//remove s
                var _table = table.Remove(table.Length - 1);
    
    
                    var objectContext = ((IObjectContextAdapter)db).ObjectContext;
                    var storageMetadata = ((EntityConnection)objectContext.Connection).GetMetadataWorkspace().GetItems(DataSpace.SSpace);
                    var entityProps = (from s in storageMetadata where s.BuiltInTypeKind == BuiltInTypeKind.EntityType select s as EntityType);
                    var personRightStorageMetadata = (from m in entityProps where m.Name == _table select m).Single();
    
                   var cols= personRightStorageMetadata.Properties.Select(p =>
                        new ColumnNameViewModel
                        {
                            EdmProperty = p
                        });
    
    
                    return new TableViewModel{
                        Data=result, Columns=cols.ToList()};*/


        }

        public class TableViewModel
        {
            public string TableName { get; set; }
            public string TableFaName { get; set; }
            public DataTable DataTable { get; set; }
        }

        public class MyGenericTableGlobal
        {
            public static object GetPropValue(object src, string propName)
            {
                return src.GetType().GetProperty(propName).GetValue(src, null);
            }
        }
      
    }
    
}