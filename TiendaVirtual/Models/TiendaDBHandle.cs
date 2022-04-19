using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TiendaVirtual.Models
{
    public class TiendaDBHandle
    {
        private SqlConnection con;
        private void connection()
        {
            
            string constring = @"Server= localhost; Database= TiendaVirtual;Integrated Security=SSPI;";
            con = new SqlConnection(constring);
        }

        public int GetTotalPages()
        {
            int TotalPages = 0;
            connection();
            SqlCommand cmd = new SqlCommand("spGetTotalPage", con);
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    TotalPages = dr.GetInt32(0);
                }
            }
            else
            {
                Console.WriteLine("No data found.");
            }

            //close data reader
            dr.Close();

            //close connection
            con.Close();

            return TotalPages;
        }

        public List<Product> GetProducts(int pageIndex, int categoryId)
        {
            connection();
            List<Product> products = new List<Product>();

            SqlCommand cmd = new SqlCommand("spGetProducts", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
            cmd.Parameters.AddWithValue("@CategoryId", categoryId);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                products.Add(
                    new Product
                    {
                        ProductId = Convert.ToInt32(dr["ProductId"]),
                        ItemCode = Convert.ToString(dr["ItemCode"]),
                        ProductName = Convert.ToString(dr["ProductName"]),
                        Price = Convert.ToDecimal(dr["Price"]),
                        CategoryId = Convert.ToInt32(dr["CategoryId"]),
                        CategoryName = Convert.ToString(dr["CategoryName"]),
                        ImageUrl = Convert.ToString(dr["ImageUrl"])
                    });
            }

            return products;
        }

        public int NewFeedback(Feedback feedback)
        {
            connection();
            int result = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("spNewFeedback", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FeedbackTypeId", feedback.FeedbackTypeId);
                cmd.Parameters.AddWithValue("@Nombre", feedback.Nombre);
                cmd.Parameters.AddWithValue("@Email", feedback.Email);
                cmd.Parameters.AddWithValue("@Comentarios", feedback.Nota);
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                con.Open();
                sd.Fill(dt);
                con.Close();

                result = 1;
            }
            catch
            {
                result = -1;
            }            
            
            return result;
        
        }
    }
}


