using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace GoldenPOS
{
    public static class DataManager
    {
        private static readonly string conString = ConfigurationManager.ConnectionStrings["GoldenEagleConString"].ConnectionString;

        #region Customers
        public static DataTable GetCustomersBasicInfo(Dictionary<string, string> Filters)
        {
            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand cmd = new SqlCommand())
            using (DataTable Customers = new DataTable())
            {
                cmd.Connection = con;
                string queryP1 = " select C.ID,C.Name,C.Phone1,C.Phone2,C.Email,CT.TypeName as Type from tblCustomers C "
                               + " left join tblCustomersTypes CT on C.ID = CT.ID ";
                string queryP2 = " where 1=1 ";
                //filtering the query by function params.
                if (!string.IsNullOrEmpty(Filters["nameOrPhones"].ToString()))
                {
                    queryP2 += " AND C.Name LIKE '%' + @namephone + '%' OR C.Phone1 LIKE '%' + @namephone + '%' OR C.Phone2 LIKE '%' + @namephone + '%' ";
                    cmd.Parameters.AddWithValue("@namephone", Filters["nameOrPhones"]);
                }
                if (!string.IsNullOrEmpty(Filters["type"].ToString()))
                {
                    queryP2 += " AND CT.ID = @type ";
                    cmd.Parameters.AddWithValue("@type", Convert.ToInt32(Filters["type"]));
                }
                if (!string.IsNullOrEmpty(Filters["deleted"].ToString()))
                {
                    queryP2 += " AND C.Deleted = @deleted ";
                    cmd.Parameters.AddWithValue("@deleted", Convert.ToInt32(Filters["deleted"]));
                }
                cmd.CommandText = "" + queryP1 + queryP2;
                try
                {
                    con.Open();
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                    {
                        dataAdapter.Fill(Customers);
                    }
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                }
                finally
                {
                    con.Close();
                }
                return Customers;
            }
        }

        public static Dictionary<string, string> GetOneCustomerAllInfoByID(int customerID)
        {
            Dictionary<string, string> Customer = new Dictionary<string, string>();
            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = " select C.ID,C.Name,C.Phone1,C.Phone2,C.TaxCard,C.WebSite,C.Address,C.CommercialRegistry,C.Email,CT.TypeName as Type  "
                                + " from tblCustomers C "
                                + " left join tblCustomersTypes CT on C.ID = CT.ID "
                                + " where C.ID = @customerid and C.Deleted = 0 ";
                cmd.Parameters.AddWithValue("@customerid", customerID);
                try
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Customer.Add("id", reader["ID"].ToString());
                            Customer.Add("name", reader["Name"].ToString());
                            Customer.Add("phone1", reader["Phone1"].ToString());
                            Customer.Add("phone2", reader["Phone2"].ToString());
                            Customer.Add("taxCard", reader["TaxCard"].ToString());
                            Customer.Add("webSite", reader["WebSite"].ToString());
                            Customer.Add("address", reader["Address"].ToString());
                            Customer.Add("commercialRegistry", reader["CommercialRegistry"].ToString());
                            Customer.Add("email", reader["Email"].ToString());
                            Customer.Add("type", reader["Type"].ToString());
                        }
                        reader.Close();
                    }
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            return Customer;
        }

        public static Dictionary<string, int> GetCustomersNames()
        {
            Dictionary<string, int> customers = new Dictionary<string, int>();
            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "select ID,Name from tblCustomers where Deleted = 0 ";
                try
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                        while (reader.Read())
                        {
                            customers.Add(reader["Name"].ToString(), Convert.ToInt32(reader["ID"]));
                        }
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            return customers;
        }

        public static bool InsertCustomer(Dictionary<string, string> customer)
        {
            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                string queryP1 = " insert into tblCustomers ( Name,CustomerType,Deleted ";
                string queryP2 = " values ( @name,@ctype,@deleted ";

                try
                {
                    con.Open();

                    #region Nullable Values
                    if (!string.IsNullOrEmpty(customer["address"]))
                    {
                        queryP1 += ",Address ";
                        queryP2 += ",@address ";
                        cmd.Parameters.AddWithValue("@address", customer["address"]);
                    }
                    if (!string.IsNullOrEmpty(customer["phone1"]))
                    {
                        queryP1 += ",Phone1 ";
                        queryP2 += ",@phone1 ";
                        cmd.Parameters.AddWithValue("@phone1", customer["phone1"]);
                    }
                    if (!string.IsNullOrEmpty(customer["phone2"]))
                    {
                        queryP1 += ",Phone2 ";
                        queryP2 += ",@phone2 ";
                        cmd.Parameters.AddWithValue("@phone2", customer["phone2"]);
                    }
                    if (!string.IsNullOrEmpty(customer["email"]))
                    {
                        queryP1 += ",Email ";
                        queryP2 += ",@email ";
                        cmd.Parameters.AddWithValue("@email", customer["email"]);
                    }
                    if (!string.IsNullOrEmpty(customer["webSite"]))
                    {
                        queryP1 += ",WebSite ";
                        queryP2 += ",@website ";
                        cmd.Parameters.AddWithValue("@website", customer["webSite"]);
                    }
                    if (!string.IsNullOrEmpty(customer["commercialRegistry"]))
                    {
                        queryP1 += ",CommercialRegistry ";
                        queryP2 += ",@registry ";
                        cmd.Parameters.AddWithValue("@registry", customer["commercialRegistry"]);
                    }
                    if (!string.IsNullOrEmpty(customer["taxCard"]))
                    {
                        queryP1 += ",TaxCard ";
                        queryP2 += ",@taxcard ";
                        cmd.Parameters.AddWithValue("@taxcard", customer["taxCard"]);
                    }
                    #endregion

                    cmd.Parameters.AddWithValue("@name", customer["name"]);
                    cmd.Parameters.AddWithValue("@ctype", Convert.ToInt32(customer["customerType"]));
                    cmd.Parameters.AddWithValue("@deleted", 0);
                    queryP1 += ") ";
                    queryP2 += ") ";
                    cmd.CommandText = "" + queryP1 + queryP2;
                    cmd.ExecuteNonQuery();
                    con.Close();
                    return true;
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public static bool UpdateCustomer(int customerID, Dictionary<string, string> customer)
        {
            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                string queryP1 = " update tblCustomers set Name=@name , CustomerType =@ctype ";
                string queryP2 = " where ID = " + customerID;

                try
                {
                    con.Open();

                    #region Nullable Values
                    if (!string.IsNullOrEmpty(customer["address"]))
                    {
                        queryP1 += " ,Address = @adress ";
                        cmd.Parameters.AddWithValue("@adress", customer["address"]);
                    }
                    if (!string.IsNullOrEmpty(customer["phone1"]))
                    {
                        queryP1 += " ,Phone1 = @phone1 ";
                        cmd.Parameters.AddWithValue("@phone1", customer["phone1"]);
                    }
                    if (!string.IsNullOrEmpty(customer["phone2"]))
                    {
                        queryP1 += " ,Phone2 = @phone2 ";
                        cmd.Parameters.AddWithValue("@phone2", customer["phone2"]);
                    }
                    if (!string.IsNullOrEmpty(customer["email"]))
                    {
                        queryP1 += " ,Email = @email ";
                        cmd.Parameters.AddWithValue("@email", customer["email"]);
                    }
                    if (!string.IsNullOrEmpty(customer["webSite"]))
                    {
                        queryP1 += " ,WebSite = @website ";
                        cmd.Parameters.AddWithValue("@website", customer["webSite"]);
                    }
                    if (!string.IsNullOrEmpty(customer["commercialRegistry"]))
                    {
                        queryP1 += " ,CommercialRegistry = @registry ";
                        cmd.Parameters.AddWithValue("@registry", customer["commercialRegistry"]);
                    }
                    if (!string.IsNullOrEmpty(customer["taxCard"]))
                    {
                        queryP1 += " ,TaxCard = @taxcard ";
                        cmd.Parameters.AddWithValue("@taxcard", customer["taxCard"]);
                    }
                    #endregion

                    cmd.Parameters.AddWithValue("@name", customer["name"]);
                    cmd.Parameters.AddWithValue("@ctype", Convert.ToInt32(customer["customerType"]));
                    //cmd.Parameters.AddWithValue("@deleted", Convert.ToInt32(customer["deleted"]));
                    queryP1 += ") ";
                    queryP2 += ") ";
                    cmd.CommandText = "" + queryP1 + queryP2;
                    cmd.ExecuteNonQuery();
                    con.Close();
                    return true;
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        #endregion

        #region Customers Delegates
        public static DataTable GetCustomersDelegatesBasicInfo(Dictionary<string, string> Filters)
        {
            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand cmd = new SqlCommand())
            using (DataTable CustomersDelegates = new DataTable())
            {
                cmd.Connection = con;
                string queryP1 = " select CD.ID,CD.Name,CD.Phone1,CD.Phone2,CD.Email,C.Name as Customer from tblCustomersDelegates CD "
                               + " left join tblCustomers C on CD.Customer = C.ID ";
                string queryP2 = " where 1=1 ";
                //filtering the query by function params.
                if (!string.IsNullOrEmpty(Filters["nameOrPhones"].ToString()))
                {
                    queryP2 += " AND CD.Name LIKE '%' + @namephone + '%' OR CD.Phone1 LIKE '%' + @namephone + '%' OR CD.Phone2 LIKE '%' + @namephone + '%' ";
                    cmd.Parameters.AddWithValue("@namephone", Filters["nameOrPhones"]);
                }
                if (!string.IsNullOrEmpty(Filters["customer"].ToString()))
                {
                    queryP2 += " AND CD.Customer = @customer ";
                    cmd.Parameters.AddWithValue("@customer", Convert.ToInt32(Filters["customer"]));
                }
                if (!string.IsNullOrEmpty(Filters["deleted"].ToString()))
                {
                    queryP2 += " AND CD.Deleted = @deleted ";
                    cmd.Parameters.AddWithValue("@deleted", Convert.ToInt32(Filters["deleted"]));
                }
                cmd.CommandText = "" + queryP1 + queryP2;
                try
                {
                    con.Open();
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                    {
                        dataAdapter.Fill(CustomersDelegates);
                    }
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                }
                finally
                {
                    con.Close();
                }
                return CustomersDelegates;
            }
        }

        public static Dictionary<string, string> GetOneCustomerDelegatesAllInfoByID(int customerDelegateID)
        {
            Dictionary<string, string> CustomerDelegate = new Dictionary<string, string>();
            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = " select CD.ID,CD.Name,CD.Phone1,CD.Phone2,CD.Email,C.Name as CustomerName  "
                                + " from tblCustomersDelegates CD "
                                + " left join tblCustomers C on CD.Customer = C.ID "
                                + " where CD.ID = @delegateid and CD.Deleted = 0 ";
                cmd.Parameters.AddWithValue("@delegateid", customerDelegateID);
                try
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            CustomerDelegate.Add("id", reader["ID"].ToString());
                            CustomerDelegate.Add("name", reader["Name"].ToString());
                            CustomerDelegate.Add("phone1", reader["Phone1"].ToString());
                            CustomerDelegate.Add("phone2", reader["Phone2"].ToString());
                            CustomerDelegate.Add("email", reader["Email"].ToString());
                            CustomerDelegate.Add("customer", reader["Customer"].ToString());
                        }
                        reader.Close();
                    }
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            return CustomerDelegate;
        }

        public static Dictionary<string, int> GetCustomersDelegatesNames()
        {
            Dictionary<string, int> customersDelegates = new Dictionary<string, int>();
            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "select ID,Name from tblCustomersDelegates where Deleted = 0 ";
                try
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                        while (reader.Read())
                        {
                            customersDelegates.Add(reader["Name"].ToString(), Convert.ToInt32(reader["ID"]));
                        }
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            return customersDelegates;
        }

        public static bool InsertCustomerDelegate(Dictionary<string, string> customerDelegate)
        {
            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                string queryP1 = " insert into tblCustomersDelegates ( Name,Customer,Deleted ";
                string queryP2 = " values ( @name,@customer,@deleted ";

                try
                {
                    con.Open();

                    #region Nullable Values
                    if (!string.IsNullOrEmpty(customerDelegate["email"]))
                    {
                        queryP1 += ",Email ";
                        queryP2 += ",@email ";
                        cmd.Parameters.AddWithValue("@email", customerDelegate["email"]);
                    }
                    if (!string.IsNullOrEmpty(customerDelegate["phone1"]))
                    {
                        queryP1 += ",Phone1 ";
                        queryP2 += ",@phone1 ";
                        cmd.Parameters.AddWithValue("@phone1", customerDelegate["phone1"]);
                    }
                    if (!string.IsNullOrEmpty(customerDelegate["phone2"]))
                    {
                        queryP1 += ",Phone2 ";
                        queryP2 += ",@phone2 ";
                        cmd.Parameters.AddWithValue("@phone2", customerDelegate["phone2"]);
                    }

                    #endregion

                    cmd.Parameters.AddWithValue("@name", customerDelegate["name"]);
                    cmd.Parameters.AddWithValue("@customer", Convert.ToInt32(customerDelegate["customer"]));
                    cmd.Parameters.AddWithValue("@deleted", 0);
                    queryP1 += ") ";
                    queryP2 += ") ";
                    cmd.CommandText = "" + queryP1 + queryP2;
                    cmd.ExecuteNonQuery();
                    con.Close();
                    return true;
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }

        }

        public static bool UpdateCustomerDelegate(int customerDelegateID, Dictionary<string, string> customerDelegate)
        {
            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                string queryP1 = " update tblCustomersDelegates set Name=@name , Customer =@customer ";
                string queryP2 = " where ID = " + customerDelegateID;

                try
                {
                    con.Open();

                    #region Nullable Values
                    if (!string.IsNullOrEmpty(customerDelegate["phone1"]))
                    {
                        queryP1 += " ,Phone1 = @phone1 ";
                        cmd.Parameters.AddWithValue("@phone1", customerDelegate["phone1"]);
                    }
                    if (!string.IsNullOrEmpty(customerDelegate["phone2"]))
                    {
                        queryP1 += " ,Phone2 = @phone2 ";
                        cmd.Parameters.AddWithValue("@phone2", customerDelegate["phone2"]);
                    }
                    if (!string.IsNullOrEmpty(customerDelegate["email"]))
                    {
                        queryP1 += " ,Email = @email ";
                        cmd.Parameters.AddWithValue("@email", customerDelegate["email"]);
                    }
                    #endregion

                    cmd.Parameters.AddWithValue("@name", customerDelegate["name"]);
                    cmd.Parameters.AddWithValue("@customer", Convert.ToInt32(customerDelegate["customer"]));
                    //cmd.Parameters.AddWithValue("@deleted", Convert.ToInt32(customer["deleted"]));
                    queryP1 += ") ";
                    queryP2 += ") ";
                    cmd.CommandText = "" + queryP1 + queryP2;
                    cmd.ExecuteNonQuery();
                    con.Close();
                    return true;
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        #endregion

        #region Products
        public static DataTable GetProductsBasicInfo(Dictionary<string, string> Filters)
        {
            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand cmd = new SqlCommand())
            using (DataTable Products = new DataTable())
            {
                cmd.Connection = con;
                string queryP1 = " select P.Code,P.Name,P.Quantity,PC.Name as Category,P.Price from tblProducts P "
                               + " left join tblProductsCategories PC on P.ID = PC.ID ";
                string queryP2 = " where 1=1 ";
                //filtering the query by function params.
                if (!string.IsNullOrEmpty(Filters["pCode"].ToString()))
                {
                    queryP2 += " AND P.Code = @code ";
                    cmd.Parameters.AddWithValue("@code", Filters["pCode"]);
                }
                if (!string.IsNullOrEmpty(Filters["pCategory"].ToString()))
                {
                    queryP2 += " AND PC.ID = @ategory ";
                    cmd.Parameters.AddWithValue("@category", Convert.ToInt32(Filters["pCategory"]));
                }
                if (!string.IsNullOrEmpty(Filters["deleted"].ToString()))
                {
                    queryP2 += " AND P.Deleted = @deleted ";
                    cmd.Parameters.AddWithValue("@deleted", Convert.ToInt32(Filters["deleted"]));
                }
                cmd.CommandText = "" + queryP1 + queryP2;
                try
                {
                    con.Open();
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                    {
                        dataAdapter.Fill(Products);
                    }
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                }
                finally
                {
                    con.Close();
                }
                return Products;
            }
        }

        public static Dictionary<string, string> GetOneProductAllInfoByCode(string productCodeOrBarcode)
        {
            Dictionary<string, string> Product = new Dictionary<string, string>();
            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = " select P.ID,P.Code,P.Barcode,P.Name,P.PartNumber,P.Quantity,PC.Name as Category,P.Price,P.LatestCost "
                                + " from tblProducts P "
                                + " left join tblProductsCategories PC on P.ID = PC.ID "
                                + " where P.Deleted = 0 AND (P.Code = @pcode OR P.Barcode = @pcode) ";
                cmd.Parameters.AddWithValue("@pcode", productCodeOrBarcode);
                try
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Product.Add("id", reader["ID"].ToString());
                            Product.Add("name", reader["Name"].ToString());
                            Product.Add("code", reader["Code"].ToString());
                            Product.Add("partNumber", reader["PartNumber"].ToString());
                            Product.Add("qty", reader["Quantity"].ToString());
                            Product.Add("category", reader["Category"].ToString());
                            Product.Add("price", reader["Price"].ToString());
                            Product.Add("cost", reader["LatestCost"].ToString());
                        }
                        reader.Close();
                    }
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            return Product;
        }

        public static bool InsertProduct()
        {
            return true;
        }

        public static bool UpdateProduct()
        {
            return true;
        }

        #endregion

        #region Products Categories

        #endregion

        #region Vendors
        public static DataTable GetVendorsBasicInfo(Dictionary<string, string> Filters)
        {
            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand cmd = new SqlCommand())
            using (DataTable Vendors = new DataTable())
            {
                cmd.Connection = con;
                string queryP1 = " select ID,Name,Phone1,Phone2,Email from tblVendors  ";
                string queryP2 = " where 1=1 ";
                //filtering the query by function params.
                if (!string.IsNullOrEmpty(Filters["nameOrPhones"].ToString()))
                {
                    queryP2 += " AND Name LIKE '%' + @namephone + '%' OR Phone1 LIKE '%' + @namephone + '%' OR Phone2 LIKE '%' + @namephone + '%' ";
                    cmd.Parameters.AddWithValue("@namephone", Filters["nameOrPhones"]);
                }
                if (!string.IsNullOrEmpty(Filters["otherProducts"].ToString()))
                {
                    queryP2 += " AND OtherProductsDesc LIKE '%' + @otherproducts + '%' ";
                    cmd.Parameters.AddWithValue("@otherproducts", Filters["otherProducts"]);
                }
                if (!string.IsNullOrEmpty(Filters["deleted"].ToString()))
                {
                    queryP2 += " AND Deleted = @deleted ";
                    cmd.Parameters.AddWithValue("@deleted", Convert.ToInt32(Filters["deleted"]));
                }
                cmd.CommandText = "" + queryP1 + queryP2;
                try
                {
                    con.Open();
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                    {
                        dataAdapter.Fill(Vendors);
                    }
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                }
                finally
                {
                    con.Close();
                }
                return Vendors;
            }
        }

        public static Dictionary<string, string> GetOneVendorAllInfoByID(int vendorID)
        {
            Dictionary<string, string> Vendor = new Dictionary<string, string>();
            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = " select * from tblVendors "
                                + " where ID = @vendorid  and Deleted = 0 ";
                cmd.Parameters.AddWithValue("@vendorid", vendorID);
                try
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Vendor.Add("id", reader["ID"].ToString());
                            Vendor.Add("name", reader["Name"].ToString());
                            Vendor.Add("phone1", reader["Phone1"].ToString());
                            Vendor.Add("phone2", reader["Phone2"].ToString());
                            Vendor.Add("taxCard", reader["TaxCard"].ToString());
                            Vendor.Add("address", reader["Address"].ToString());
                            Vendor.Add("commercialRegistry", reader["CommercialRegistry"].ToString());
                            Vendor.Add("email", reader["Email"].ToString());
                            Vendor.Add("lastBillValue", reader["LastBillValue"].ToString());
                            Vendor.Add("lastBillDate", reader["LastBillDate"].ToString());
                            Vendor.Add("otherProductsDesc", reader["OtherProductsDesc"].ToString());
                        }
                        reader.Close();
                    }
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            return Vendor;
        }

        public static Dictionary<string, int> GetVendorsNames()
        {
            Dictionary<string, int> vendors = new Dictionary<string, int>();
            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "select ID,Name from tblVendors where Deleted = 0 ";
                try
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                        while (reader.Read())
                        {
                            vendors.Add(reader["Name"].ToString(), Convert.ToInt32(reader["ID"]));
                        }
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            return vendors;
        }

        public static bool InsertVendor(Dictionary<string, string> vendor)
        {
            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                string queryP1 = " insert into tblVendors ( Name,LastBillValue,Deleted ";
                string queryP2 = " values ( @name,@lastbill,@deleted ";

                try
                {
                    con.Open();

                    #region Nullable Values
                    if (!string.IsNullOrEmpty(vendor["address"]))
                    {
                        queryP1 += ",Address ";
                        queryP2 += ",@adress ";
                        cmd.Parameters.AddWithValue("@adress", vendor["address"]);
                    }
                    if (!string.IsNullOrEmpty(vendor["phone1"]))
                    {
                        queryP1 += ",Phone1 ";
                        queryP2 += ",@phone1 ";
                        cmd.Parameters.AddWithValue("@phone1", vendor["phone1"]);
                    }
                    if (!string.IsNullOrEmpty(vendor["phone2"]))
                    {
                        queryP1 += ",Phone2 ";
                        queryP2 += ",@phone2 ";
                        cmd.Parameters.AddWithValue("@phone2", vendor["phone2"]);
                    }
                    if (!string.IsNullOrEmpty(vendor["email"]))
                    {
                        queryP1 += ",Email ";
                        queryP2 += ",@email ";
                        cmd.Parameters.AddWithValue("@email", vendor["email"]);
                    }
                    if (!string.IsNullOrEmpty(vendor["lastBillDate"]))
                    {
                        queryP1 += ",LastBillDate ";
                        queryP2 += ",@lastdate ";
                        cmd.Parameters.AddWithValue("@lastdate", DateTime.ParseExact(vendor["lastBillDate"], "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
                    }
                    if (!string.IsNullOrEmpty(vendor["commercialRegistry"]))
                    {
                        queryP1 += ",CommercialRegistry ";
                        queryP2 += ",@registry ";
                        cmd.Parameters.AddWithValue("@registry", vendor["commercialRegistry"]);
                    }
                    if (!string.IsNullOrEmpty(vendor["taxCard"]))
                    {
                        queryP1 += ",TaxCard ";
                        queryP2 += ",@taxcard ";
                        cmd.Parameters.AddWithValue("@taxcard", vendor["taxCard"]);
                    }
                    if (!string.IsNullOrEmpty(vendor["otherProductsDesc"]))
                    {
                        queryP1 += ",OtherProductsDesc ";
                        queryP2 += ",@otherproducts ";
                        cmd.Parameters.AddWithValue("@otherproducts", vendor["otherProductsDesc"]);
                    }
                    #endregion

                    cmd.Parameters.AddWithValue("@name", vendor["name"]);
                    cmd.Parameters.AddWithValue("@lastbill", Convert.ToDecimal(vendor["lastBillValue"]));
                    cmd.Parameters.AddWithValue("@deleted", 0);
                    queryP1 += ") ";
                    queryP2 += ") ";
                    cmd.CommandText = "" + queryP1 + queryP2;
                    cmd.ExecuteNonQuery();
                    con.Close();
                    return true;
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public static bool UpdateVendor(int vendorID, Dictionary<string, string> vendor)
        {
            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                string queryP1 = " update tblVendors set Name=@name , LastBillValue =@lastbillvaue ";
                string queryP2 = " where ID = " + vendorID;

                try
                {
                    con.Open();

                    #region Nullable Values
                    if (!string.IsNullOrEmpty(vendor["address"]))
                    {
                        queryP1 += " ,Address = @adress ";
                        cmd.Parameters.AddWithValue("@adress", vendor["address"]);
                    }
                    if (!string.IsNullOrEmpty(vendor["phone1"]))
                    {
                        queryP1 += " ,Phone1 = @phone1 ";
                        cmd.Parameters.AddWithValue("@phone1", vendor["phone1"]);
                    }
                    if (!string.IsNullOrEmpty(vendor["phone2"]))
                    {
                        queryP1 += " ,Phone2 = @phone2 ";
                        cmd.Parameters.AddWithValue("@phone2", vendor["phone2"]);
                    }
                    if (!string.IsNullOrEmpty(vendor["email"]))
                    {
                        queryP1 += " ,Email = @email ";
                        cmd.Parameters.AddWithValue("@email", vendor["email"]);
                    }
                    if (!string.IsNullOrEmpty(vendor["otherProductsDesc"]))
                    {
                        queryP1 += " ,OtherProductsDes = @otherproducts ";
                        cmd.Parameters.AddWithValue("@otherproducts", vendor["otherProductsDesc"]);
                    }
                    if (!string.IsNullOrEmpty(vendor["commercialRegistry"]))
                    {
                        queryP1 += " ,CommercialRegistry = @registry ";
                        cmd.Parameters.AddWithValue("@registry", vendor["commercialRegistry"]);
                    }
                    if (!string.IsNullOrEmpty(vendor["taxCard"]))
                    {
                        queryP1 += " ,TaxCard = @taxcard ";
                        cmd.Parameters.AddWithValue("@taxcard", vendor["taxCard"]);
                    }
                    if (!string.IsNullOrEmpty(vendor["lastBillDate"]))
                    {
                        queryP1 += " ,LastBillDate = @lastdate ";
                        cmd.Parameters.AddWithValue("@lastdate", DateTime.ParseExact(vendor["lastBillDate"], "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
                    }
                    #endregion

                    cmd.Parameters.AddWithValue("@name", vendor["name"]);
                    cmd.Parameters.AddWithValue("@lastbillvalue", Convert.ToDecimal(vendor["lastBillValue"]));
                    //cmd.Parameters.AddWithValue("@deleted", Convert.ToInt32(customer["deleted"]));
                    queryP1 += ") ";
                    queryP2 += ") ";
                    cmd.CommandText = "" + queryP1 + queryP2;
                    cmd.ExecuteNonQuery();
                    con.Close();
                    return true;
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        #endregion

        #region Vendors Delegates
        public static DataTable GetVendorsDelegatesBasicInfo(Dictionary<string, string> Filters)
        {
            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand cmd = new SqlCommand())
            using (DataTable VendorsDelegates = new DataTable())
            {
                cmd.Connection = con;
                string queryP1 = " select VD.ID,VD.Name,VD.Phone1,VD.Phone2,VD.Email, V.Name as Vendor from tblVendorsDelegates VD "
                               + " left join tblVendors V on VD.Vendor = V.ID ";
                string queryP2 = " where 1=1 ";
                //filtering the query by function params.
                if (!string.IsNullOrEmpty(Filters["nameOrPhones"].ToString()))
                {
                    queryP2 += " AND VD.Name LIKE '%' + @namephone + '%' OR VD.Phone1 LIKE '%' + @namephone + '%' OR VD.Phone2 LIKE '%' + @namephone + '%' ";
                    cmd.Parameters.AddWithValue("@namephone", Filters["nameOrPhones"]);
                }
                if (!string.IsNullOrEmpty(Filters["vendor"].ToString()))
                {
                    queryP2 += " AND VD.Vendor = @vendor ";
                    cmd.Parameters.AddWithValue("@vendor", Convert.ToInt32(Filters["vendor"]));
                }
                if (!string.IsNullOrEmpty(Filters["deleted"].ToString()))
                {
                    queryP2 += " AND VD.Deleted = @deleted ";
                    cmd.Parameters.AddWithValue("@deleted", Convert.ToInt32(Filters["deleted"]));
                }
                cmd.CommandText = "" + queryP1 + queryP2;
                try
                {
                    con.Open();
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                    {
                        dataAdapter.Fill(VendorsDelegates);
                    }
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                }
                finally
                {
                    con.Close();
                }
                return VendorsDelegates;
            }
        }

        public static Dictionary<string, string> GetOneVendorDelegateAllInfoByID(int vendorDelegateID)
        {
            Dictionary<string, string> VendorDelegate = new Dictionary<string, string>();
            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = " select VD.ID,VD.Name,VD.Phone1,VD.Phone2,VD.Email,V.Name as VendorName  "
                                + " from tblVendorsDelegates VD "
                                + " left join tblVendors V on VD.Vendor = V.ID "
                                + " where VD.ID = @delegateid and VD.Deleted = 0 ";
                cmd.Parameters.AddWithValue("@delegateid", vendorDelegateID);
                try
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            VendorDelegate.Add("id", reader["ID"].ToString());
                            VendorDelegate.Add("name", reader["Name"].ToString());
                            VendorDelegate.Add("phone1", reader["Phone1"].ToString());
                            VendorDelegate.Add("phone2", reader["Phone2"].ToString());
                            VendorDelegate.Add("email", reader["Email"].ToString());
                            VendorDelegate.Add("vendor", reader["Vendor"].ToString());
                        }
                        reader.Close();
                    }
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            return VendorDelegate;
        }

        public static Dictionary<string, int> GetVendorsDelegatesNames()
        {
            Dictionary<string, int> vendorsDelegates = new Dictionary<string, int>();
            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "select ID,Name from tblVendorsDelegates where Deleted = 0 ";
                try
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                        while (reader.Read())
                        {
                            vendorsDelegates.Add(reader["Name"].ToString(), Convert.ToInt32(reader["ID"]));
                        }
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            return vendorsDelegates;
        }

        public static bool InsertVendorDelegate(Dictionary<string, string> vendorDelegate)
        {
            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                string queryP1 = " insert into tblVendorsDelegates ( Name,Vendor,Deleted ";
                string queryP2 = " values ( @name,@vendor,@deleted ";

                try
                {
                    con.Open();

                    #region Nullable Values
                    if (!string.IsNullOrEmpty(vendorDelegate["email"]))
                    {
                        queryP1 += ",Email ";
                        queryP2 += ",@email ";
                        cmd.Parameters.AddWithValue("@email", vendorDelegate["email"]);
                    }
                    if (!string.IsNullOrEmpty(vendorDelegate["phone1"]))
                    {
                        queryP1 += ",Phone1 ";
                        queryP2 += ",@phone1 ";
                        cmd.Parameters.AddWithValue("@phone1", vendorDelegate["phone1"]);
                    }
                    if (!string.IsNullOrEmpty(vendorDelegate["phone2"]))
                    {
                        queryP1 += ",Phone2 ";
                        queryP2 += ",@phone2 ";
                        cmd.Parameters.AddWithValue("@phone2", vendorDelegate["phone2"]);
                    }

                    #endregion

                    cmd.Parameters.AddWithValue("@name", vendorDelegate["name"]);
                    cmd.Parameters.AddWithValue("@vendor", Convert.ToInt32(vendorDelegate["vendor"]));
                    cmd.Parameters.AddWithValue("@deleted", 0);
                    queryP1 += ") ";
                    queryP2 += ") ";
                    cmd.CommandText = "" + queryP1 + queryP2;
                    cmd.ExecuteNonQuery();
                    con.Close();
                    return true;
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public static bool UpdateVendorDelegate(int vendorDelegateID, Dictionary<string, string> vendorDelegate)
        {
            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                string queryP1 = " update tblVendorsDelegates set Name=@name , Vendor =@vendor ";
                string queryP2 = " where ID = " + vendorDelegateID;

                try
                {
                    con.Open();

                    #region Nullable Values
                    if (!string.IsNullOrEmpty(vendorDelegate["phone1"]))
                    {
                        queryP1 += " ,Phone1 = @phone1 ";
                        cmd.Parameters.AddWithValue("@phone1", vendorDelegate["phone1"]);
                    }
                    if (!string.IsNullOrEmpty(vendorDelegate["phone2"]))
                    {
                        queryP1 += " ,Phone2 = @phone2 ";
                        cmd.Parameters.AddWithValue("@phone2", vendorDelegate["phone2"]);
                    }
                    if (!string.IsNullOrEmpty(vendorDelegate["email"]))
                    {
                        queryP1 += " ,Email = @email ";
                        cmd.Parameters.AddWithValue("@email", vendorDelegate["email"]);
                    }
                    #endregion

                    cmd.Parameters.AddWithValue("@name", vendorDelegate["name"]);
                    cmd.Parameters.AddWithValue("@vendor", Convert.ToInt32(vendorDelegate["vendor"]));
                    //cmd.Parameters.AddWithValue("@deleted", Convert.ToInt32(customer["deleted"]));
                    queryP1 += ") ";
                    queryP2 += ") ";
                    cmd.CommandText = "" + queryP1 + queryP2;
                    cmd.ExecuteNonQuery();
                    con.Close();
                    return true;
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        #endregion

        #region Users

        #endregion

        #region Common Functions

        private static bool IsUnique(string input, string column, string table)
        {
            if (!string.IsNullOrEmpty(input))
            {
                using (SqlConnection con = new SqlConnection())
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "select count(" + column + ") from " + table + " where " + column + " = @input ";
                    try
                    {
                        con.Open();
                        cmd.Parameters.AddWithValue("@input", input);
                        int count;
                        count = (int)cmd.ExecuteScalar();
                        con.Close();
                        if (count > 0)
                            return false;
                        else
                            return true;
                    }
                    catch
                    {
                        con.Close();
                        return false;
                    }
                }
            }
            else
                return false;
        }

        #endregion

    }
}
