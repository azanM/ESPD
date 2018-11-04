using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Text;
using System.Security.Cryptography;

/// <summary>
/// Summary description for Helper
/// </summary>
public class Helper
{
    SqlConnection objCon = new SqlConnection();
    string connection;

    public Helper()
    {

        MyConnection();

    }

    public bool MyConnection()
    {
        connection = ConfigurationManager.ConnectionStrings["SPDConnectionString"].ConnectionString;
        objCon = new SqlConnection(connection);
        try
        {
            if (objCon.State == ConnectionState.Closed)
            {
                objCon.Open();
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
   
    
    public DataSet wsEmail(string strKet)
    {
        try
        {
            string con = ConfigurationManager.ConnectionStrings["SPDConnectionString"].ConnectionString;
            SqlConnection objCon = new SqlConnection(con);
            if (objCon.State == ConnectionState.Closed)
            {
                objCon.Open();
            }
            //StoreProcedure
            DataSet ds = new DataSet("[sp_GetMilis]");
            SqlDataAdapter adp = new SqlDataAdapter("[sp_GetMilis]", objCon);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.Add(new SqlParameter("@ket", strKet));
            adp.Fill(ds);
            adp.Dispose();
            objCon.Close();
            SqlConnection.ClearAllPools();
            return ds;
        }
        catch (Exception ex)
        {
            if (objCon.State == ConnectionState.Open)
            {
                objCon.Close();
                SqlConnection.ClearAllPools();
            }
            throw ex;
        }
    }
   
}

