using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace pos
{
    public class product_info
    {
        public string barcode { get; set; }
        public string product_name { get; set; }
        public productTypeInfo productType { get; set; }
        public ulong money { get; set; }
        public string product_money
        {
            get
            {
                return String.Format("{0:#,0}", money);
            }
            set
            {
                money = Convert.ToUInt64(value);
            }
        }
        public sale_info sale { get; set; }
        public string product_sale
        {
            get
            {
                return sale.sale + "%";
            }
        }
        public int product_count { get; set; }
        public string product_total
        {
            get
            {
                return String.Format("{0:#,0}", (Convert.ToDouble(money * Convert.ToUInt64(product_count)) * (1.0 - (Convert.ToDouble(sale.sale) / 100.0))));
            }
        }
    }

    public class productTypeInfo
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class sale_info
    {
        public int id { get; set; }
        public string name { get; set; }
        public int sale { get; set; }
        public int productType { get; set; }
    }

    class Product_manager
    {
        public bool is_correct_DB(string path)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection("data source=" + path))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        string command = @"select name from sqlite_master where type='table' and name='goods';";
                        cmd.CommandText = command;
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                return false;
                            else
                            {
                                while (reader.Read())
                                {
                                    if (reader["name"].ToString() != "goods")
                                        return false;
                                }
                            }
                        }
                    }
                    conn.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool createDB()
        {
            try
            {
                SQLiteConnection.CreateFile("product_list.db");
                using (SQLiteConnection conn = new SQLiteConnection("data source=product_list.db"))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        string command = @"create table goods(barcode_id text, name text, money integer, sale interger, count interger, type interger);";
                        cmd.CommandText = command;
                        cmd.ExecuteNonQuery();
                        command = @"create table sales(id interger, PRIMARY KEY(id AUTOINCREMENT), name text, saleper interger, productType interger);";
                        cmd.CommandText = command;
                        cmd.ExecuteNonQuery();
                        command = @"create table productType(id interger, PRIMARY KEY(id AUTOINCREMENT), name text);";
                        cmd.CommandText = command;
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public sale_info[] GetAllSaleInfos()
        {
            using (SQLiteConnection conn = new SQLiteConnection("data source=product_list.db"))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = $@"select * from sales";
                    int RowCount = Convert.ToInt32(cmd.ExecuteScalar());
                    sale_info[] sale_infos = new sale_info[RowCount];
                    cmd.CommandText = $@"select * from sales";
                    int idx = 0;
                    using (SQLiteDataReader read = cmd.ExecuteReader())
                    {
                        if (read.HasRows)
                        {
                            while (read.Read())
                            {
                                sale_infos[idx] = new sale_info()
                                {
                                    id = Convert.ToInt32(read["id"]),
                                    name = read["name"].ToString(),
                                    sale = Convert.ToInt32(read["saleper"]),
                                    productType = Convert.ToInt32(read["productType"])
                                };
                                idx++;
                            }
                            if (sale_infos.Length != 0) return sale_infos;
                            return null;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public sale_info GetSaleInfo(int id) 
        { 
            using (SQLiteConnection conn = new SQLiteConnection("data source=product_list.db"))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = $@"select * from sales where id = {id}";
                    using(SQLiteDataReader read = cmd.ExecuteReader())
                    {
                        if(read.HasRows)
                        {
                            while(read.Read())
                            {
                                return new sale_info()
                                {
                                    id = Convert.ToInt32(read["id"]),
                                    name = read["name"].ToString(),
                                    sale = Convert.ToInt32(read["saleper"]),
                                    productType = Convert.ToInt32(read["productType"])
                                };
                            }
                            return null;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public productTypeInfo[] GetAllproductTypeInfos()
        {
            using (SQLiteConnection conn = new SQLiteConnection("data source=product_list.db"))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = $@"select * from sales where id = {id}";
                    using (SQLiteDataReader read = cmd.ExecuteReader())
                    {
                        if (read.HasRows)
                        {
                            while (read.Read())
                            {
                                return new productTypeInfo()
                                {
                                    id = Convert.ToInt32(read["id"]),
                                    name = read["name"].ToString(),
                                };
                            }
                            return null;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public productTypeInfo GetProductTypeInfo(int id)
        {
            using (SQLiteConnection conn = new SQLiteConnection("data source=product_list.db"))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = $@"select * from sales where id = {id}";
                    using (SQLiteDataReader read = cmd.ExecuteReader())
                    {
                        if (read.HasRows)
                        {
                            while (read.Read())
                            {
                                return new productTypeInfo()
                                {
                                    id = Convert.ToInt32(read["id"]),
                                    name = read["name"].ToString(),
                                };
                            }
                            return null;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
        public product_info GetProductInfo(string barcodeNumber)
        {
            using (SQLiteConnection conn = new SQLiteConnection("data source=product_list.db"))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = $@"select * from goods where barcode_id = '{barcodeNumber}';";
                    using (SQLiteDataReader read = cmd.ExecuteReader())
                    {
                        if (read.HasRows)
                        {
                            while (read.Read())
                            {
                                string barcode = read["barcode_id"].ToString(), name = read["name"].ToString(), money = read["money"].ToString();
                                return new product_info() { 
                                    barcode = barcode, 
                                    product_name = name, 
                                    product_money = money, 
                                    sale = GetSaleInfo(Convert.ToInt32(read["sale"])), 
                                    product_count = Convert.ToInt32(read["count"]),
                                    productType = GetProductTypeInfo(Convert.ToInt32(read["type"]))
                                };
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                conn.Close();
            }
            return null;
        }

        public List<product_info> GetAllProductInfo()
        {
            List<product_info> list = new List<product_info>();
            using (SQLiteConnection conn = new SQLiteConnection("data source=product_list.db"))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = $@"select * from goods;";
                    using (SQLiteDataReader read = cmd.ExecuteReader())
                    {
                        if (read.HasRows)
                        {
                            while (read.Read())
                            {
                                string barcode = read["barcode_id"].ToString(), name = read["name"].ToString(), money = read["money"].ToString();
                                list.Add(new product_info() { 
                                    barcode = barcode, 
                                    product_name = name, 
                                    product_money = money, 
                                    sale = GetSaleInfo(Convert.ToInt32(read["sale"])), 
                                    product_count = Convert.ToInt32(read["count"]),
                                    productType = GetProductTypeInfo(Convert.ToInt32(read["type"]))
                                });
                            }
                        }
                    }
                }
                conn.Close();
            }
            if (list.Count != 0)
                return list;
            else
                return null;
        }

        public void AddProductInfo(product_info info)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection("data source=product_list.db"))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        string command = $@"insert into goods values('{info.barcode}', '{info.product_name}', {info.money}, {info.sale}, {info.product_count});";
                        cmd.CommandText = command;
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
            }
            catch
            {
            }
        }

        public void RemoveProductInfo(string barcodeNumber)
        {
            using (SQLiteConnection conn = new SQLiteConnection("data source=product_list.db"))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = $@"delete from goods where barcode_id = '{barcodeNumber}';";
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        public void EditProductInfo(string barcodeNumber, product_info newProductInfo)
        {
            using (SQLiteConnection conn = new SQLiteConnection("data source=product_list.db"))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    //barcode_id text, name text, money integer, sale interger, count interger
                    cmd.CommandText = $@"update goods set barcode_id = '{newProductInfo.barcode}', sale={newProductInfo.sale.id} name = '{newProductInfo.product_name}', money = {newProductInfo.money}, productType = {newProductInfo.productType}, count = {newProductInfo.product_count} where barcode_id = '{barcodeNumber}'";
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }
    }
}
