using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nodule4_02
{
    class Program
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        static string line = new string('-', 50);

        static void Main(string[] args)
        {
            Exmpl9();

        }

        static void Exmpl1()
        {
            DataSet ds = new DataSet();
            DataTable tbl = ds.Tables.Add("newEquipment");

            //Готовим объект DataTable 
            OleDbCommand cmd = new OleDbCommand();

            //Готовим объект Command 
            OleDbDataReader rdr = cmd.ExecuteReader();
            DataRow row;

            while (rdr.Read())
            {
                row = tbl.NewRow();
                row["CustomerID"] = rdr["intEquipmentID"];
                //Выбираем данные из других столбцов 
                tbl.Rows.Add(row);
            }

            rdr.Close();
        }

        static void Exmpl2()
        {
            DataSet ds = new DataSet();
            DataTable tbl = ds.Tables.Add("newEquipment");

            
            //Инициализируем объект DataAdapter 
            DataTableMapping TblМар;
            DataColumnMapping ColMap = new DataColumnMapping();
            SqlDataAdapter da = new SqlDataAdapter();





            TblМар = da.TableMappings.Add("Table", "newEquipment");
            ColMap = TblМар.ColumnMappings.Add("EquipmentID", "intEquipmentID");
            ColMap = TblМар.ColumnMappings.Add("GarageRoom", "intGarageRoom");
            ColMap = TblМар.ColumnMappings.Add("SerialNo", "strSerialNo");

            //2-oi variant
            DataColumnMapping[] ColMapArray;
            ColMapArray = new DataColumnMapping[]
            {
                new DataColumnMapping("EquipmentID", "intEquipmentID"),
                new DataColumnMapping("GarageRoom", "intGarageRoom"),
                new DataColumnMapping("SerialNo", "strSerialNo")
            };
            TblМар.ColumnMappings.AddRange(ColMapArray);

        }

        //Конструкторы DataAdapter 
        static void Exmpl3()
        {
            //SqlConnection conn = new SqlConnection("");

            //string strSql = "select * from newEquipment";
            //SqlCommand cmd = new SqlCommand();
            //cmd.CommandText = strSql;
            //cmd.Connection = conn;

            //SqlDataAdapter da = new SqlDataAdapter();
            //da.SelectCommand = cmd;
            

            SqlConnection conn = new SqlConnection(connectionString);

            //1
            string strSQL = "SELECT *  FROM newEquipment";
            SqlDataAdapter da = new SqlDataAdapter(strSQL, conn);

            //2
            SqlDataAdapter daEquipment, daManufacturer;

            daEquipment = new SqlDataAdapter("SELECT * FROM newEquipment", conn);
            daManufacturer = new SqlDataAdapter("SELECT * FROM TablesManufacturer", conn);

            //3
            string strSOL = "SELECT * FROM newEquipment";

            SqlCommand cmd = new SqlCommand(strSQL, conn);
            SqlDataAdapter da2 = new SqlDataAdapter(cmd);

        }

        static void Exmpl4()
        {
            SqlConnection conn = new SqlConnection(connectionString);

            string strSOL = "SELECT top 3  intEquipmentID, intGarageRoom, strSerialNo FROM newEquipment";
            SqlDataAdapter da = new SqlDataAdapter(strSOL, conn);
            DataSet ds = new DataSet();

            da.Fill(ds);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                // получаем все ячейки строки
                var cells = row.ItemArray;
                foreach (object cell in cells)
                    Console.Write("\t{0}", cell);
                Console.WriteLine();
            }
        }

        static void Exmpl5()
        {
            SqlConnection conn = new SqlConnection(connectionString);

            string strSOL = "SELECT  top 3  intEquipmentID, intGarageRoom, strSerialNo  FROM newEquipment";
            SqlDataAdapter da = new SqlDataAdapter(strSOL, conn);

            da.TableMappings.Add("newEquipment", "Equipment");
            DataSet ds = new DataSet();
            da.Fill(ds);

            foreach (DataTable dt in ds.Tables)
            {
                Console.WriteLine(dt.TableName); // название таблицы
                                                 // перебор всех столбцов
                foreach (DataColumn column in dt.Columns)
                    Console.Write("\t{0} - dataType: {1}", column.ColumnName, column.DataType.ToString());
                Console.WriteLine();
                // перебор всех строк таблицы
                foreach (DataRow row in dt.Rows)
                {
                    // получаем все ячейки строки
                    var cells = row.ItemArray;
                    foreach (object cell in cells)
                        Console.Write("\t{0}", cell);
                    Console.WriteLine();
                }
            }
        }

        static void Exmpl6()
        {
            SqlConnection conn = new SqlConnection(connectionString);

            DataSet ds = new DataSet();
            string strSOL = "SELECT * FROM newEquipment";
            SqlDataAdapter da = new SqlDataAdapter(strSOL, conn);
            da.Fill(ds, "newEquipment");

            //Кроме того, вместо объекта DataSet можно указать объект DataTable
            //Этот вариант полезен, когда у вас есть уже созданный и ожидающий заполнения объект DataTable
            DataTable tbl = new DataTable();
            da.Fill(tbl);

            int intStartRecord = 0;
            int intNumRecorbs = 3;
            //da.Fill(ds, intStartRecord, intNumRecorbs, "newEquipment");

          
    

        intStartRecord = 4;
            intNumRecorbs = 10;
            da.Fill(ds, intStartRecord, intNumRecorbs, "newEquipment");
  //foreach (DataRow row in ds.Tables[0].Rows)
  //          {
  //              // получаем все ячейки строки
  //              var cells = row.ItemArray;
  //              foreach (object cell in cells)
  //                  Console.Write("\t{0}", cell);
  //              Console.WriteLine();
  //          }

        }

        static void Exmpl7()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlDataAdapter daEquipment, daManufacturer;

            daEquipment = new SqlDataAdapter("SELECT * FROM newEquipment", conn);
            daManufacturer = new SqlDataAdapter("SELECT * FROM TablesManufacturer", conn);

            DataSet ds = new DataSet();

            daEquipment.Fill(ds);
            daManufacturer.Fill(ds);

            //ЛУЧШЕ
            conn.Open();
            daEquipment.Fill(ds);
            daManufacturer.Fill(ds);
            conn.Close();

        }

        static void Exmpl8()
        {
            SqlConnection conn = new SqlConnection(connectionString);

            string strSOL = "SELECT top 10 intEquipmentID, intGarageRoom, strSerialNo FROM newEquipment;";
            SqlDataAdapter da = new SqlDataAdapter(strSOL, conn);

            DataSet ds = new DataSet();

            da.Fill(ds, "newEquipment");

            foreach (DataRow r in ds.Tables[0].Rows)
            {
                foreach (var cell in r.ItemArray)
                    Console.Write("\t{0}", cell);
                Console.WriteLine();
            }

            Console.WriteLine(line);

            da.Fill(ds, "newEquipment");
            foreach (DataRow r in ds.Tables[0].Rows)
            {
                foreach (var cell in r.ItemArray)
                    Console.Write("\t{0}", cell);
                Console.WriteLine();
            }


            Console.Read();
        }

        static void Exmpl9()
        {
            string strSQL = "SELECT intEquipmentID, intGarageRoom, strSerialNo FROM newEquipment;" +
                            "select intManufacturerID, strName  from TablesManufacturer";

            SqlConnection conn = new SqlConnection(connectionString);
            SqlDataAdapter da = new SqlDataAdapter(strSQL, conn);

            DataSet ds = new DataSet();

            da.TableMappings.Add("Table", "newEquipment");
            da.TableMappings.Add("Table1", "TablesManufacturer");
           // da.MissingMappingAction = MissingMappingAction.Ignore;
            da.MissingSchemaAction = MissingSchemaAction.Ignore;
            da.Fill(ds);

            
            

        }

        static void Exmpl10()
        {
            string strSQL = "SELECT intEquipmentID, intGarageRoom, strSerialNo FROM newEquipment";

            SqlConnection conn = new SqlConnection(connectionString);
            SqlDataAdapter da = new SqlDataAdapter(strSQL, conn);
            //Указывает TM
            da.TableMappings.Add("Table", "newEquipment");

            DataColumnMappingCollection cm = da.TableMappings[0].ColumnMappings;
            cm.Add("EquipmentID", "intEquipmentID");
            cm.Add("GarageRoom", "intGarageRoom");
            cm.Add("SerialNo", "strSerialNo");

            DataTable tbl = new DataTable("newEquipment");
            da.Fill(tbl);

            //Можно также воспользоваться методом AddRange объекта DataTableMappingCoUection 
            //или DataColumnfrlappingCollection и добавить в набор группу элементов за один вызов

            //DataColumnMappingCollection cm2 = da.TableMappings[0].ColumnMappings;
            //cm2.AddRange(new DataColumnMapping[]
            //{
            //    new DataColumnMapping("intEquipmentID", "intEquipmentID"),
            //    new DataColumnMapping("intGarageRoom", "intGarageRoom"),
            //    new DataColumnMapping("strSerialNo", "strSerialNo")
            //});

            /*
             * А что, если выполняемый объектом DataAdapter запрос содержит информацию об одной из таблиц объекта DataSet, отсутствующую в наборе TableMappings? 
             * По умолчанию DataAdapter предполагает, что вам требуется получить эту информацию и записать ее в таблицу
             */
            SqlDataAdapter da2 = new SqlDataAdapter(strSQL, conn);
            DataSet ds = new DataSet();
            da2.Fill(ds);

            Console.WriteLine(ds.Tables["Table"].Rows[0]["intEquipmentID"].ToString());

        }

        static void Exmpl11()
        {
            string sql = "SELECT * FROM TablesModel";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                DataTable dt = ds.Tables[0];
                // добавим новую строку
                DataRow newRow = dt.NewRow();
                newRow["strName"] = "Outback";
                newRow["intManufacturerID"] = 1;
                newRow["intSMCSFamilyID"] = 1;
                newRow["strImage"] = "19.png";
                dt.Rows.Add(newRow);

                // создаем объект SqlCommandBuilder
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
                adapter.Update(ds);
                // альтернативный способ - обновление только одной таблицы
                //adapter.Update(dt);
                // заново получаем данные из бд
                // очищаем полностью DataSet
                ds.Clear();
                // перезагружаем данные
                adapter.Fill(ds);

                foreach (DataColumn column in dt.Columns)
                    Console.Write("\t{0}", column.ColumnName);
                Console.WriteLine();
                // перебор всех строк таблицы
                foreach (DataRow row in dt.Rows)
                {
                    // получаем все ячейки строки
                    var cells = row.ItemArray;
                    foreach (object cell in cells)
                        Console.Write("\t{0}", cell);
                    Console.WriteLine();
                }
            }
            Console.Read();
        }

        static void Exmpl12()
        {
            string sql = "SELECT * FROM TablesModel";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                // устанавливаем команду на вставку
                adapter.InsertCommand = new SqlCommand("CreateTablesModel", connection);
                // это будет зранимая процедура
                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;

                adapter.InsertCommand.Parameters.Add(new SqlParameter("@strName", SqlDbType.NVarChar, 50, "strName"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@intManufacturerID", SqlDbType.Int, 0,
                    "intManufacturerID"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@intSMCSFamilyID", SqlDbType.Int, 0,
                    "intSMCSFamilyID"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@strImage", SqlDbType.NVarChar, 50, "strImage"));

                // добавляем выходной параметр для id
                SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@intModelID", SqlDbType.Int, 0,
                    "intModelID");
                parameter.Direction = ParameterDirection.Output;

                DataSet ds = new DataSet();
                adapter.Fill(ds);

                DataTable dt = ds.Tables[0];
                // добавим новую строку
                DataRow newRow = dt.NewRow();
                newRow["strName"] = "Outback";
                newRow["intManufacturerID"] = 1;
                newRow["intSMCSFamilyID"] = 1;
                newRow["strImage"] = "19.png";
                dt.Rows.Add(newRow);

                adapter.Update(ds);
                ds.AcceptChanges();


                foreach (DataColumn column in dt.Columns)
                    Console.Write("\t{0}", column.ColumnName);
                Console.WriteLine();
                // перебор всех строк таблицы
                foreach (DataRow row in dt.Rows)
                {
                    // получаем все ячейки строки
                    var cells = row.ItemArray;
                    foreach (object cell in cells)
                        Console.Write("\t{0}", cell);
                    Console.WriteLine();
                }
            }
            Console.Read();
        }
    }
}
