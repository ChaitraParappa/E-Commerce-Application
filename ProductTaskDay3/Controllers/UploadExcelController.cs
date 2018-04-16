using ProductTaskDay3.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProductTaskDay3.Controllers
{
    public class UploadExcelController : Controller
    {
        
        public List<Product> product = new List<Product>();
        Utility Utility = new Utility();

        [HttpPost]
        public async Task<ActionResult> Importexcel1()
        {
            string userid = Request.Form["UserId"];
            try
            {
                if (Request.Files["FileUpload1"].ContentLength > 0)
                {
                    string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
                    string query = null;
                    string connString = "";




                    string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                    string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), Request.Files["FileUpload1"].FileName);
                    if (!Directory.Exists(path1))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                    }
                    if (validFileTypes.Contains(extension))
                    {
                        if (System.IO.File.Exists(path1))
                        { System.IO.File.Delete(path1); }
                        Request.Files["FileUpload1"].SaveAs(path1);
                        if (extension == ".csv")
                        {
                            DataTable dt = Utility.ConvertCSVtoDataTable(path1);
                            ViewBag.Data = dt;
                        }
                      //  Connection String to Excel Workbook
                        else if (extension.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                            int success  =await Utility.ConvertXSLXtoDataTable(path1, connString, userid);
                        }
                        else if (extension.Trim() == ".xlsx")
                        {
                            connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                           int success= await Utility.ConvertXSLXtoDataTable(path1, connString, userid);
                           // ViewBag.Data = dt;
                            return RedirectToAction("Display","Home");
                        }

                    }
                    else
                    {
                        ViewBag.Error = "Please Upload Files in .xls, .xlsx or .csv format";

                    }

                }

                return View();
            }
            catch (Exception e)
            {
                return View();
            }
        }
    }

    public class Utility
    {
        public List<Product> product = new List<Product>();

        public DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            try
            {
                DataTable dt = new DataTable();
                using (StreamReader sr = new StreamReader(strFilePath))
                {
                    string[] headers = sr.ReadLine().Split(',');
                    foreach (string header in headers)
                    {
                        dt.Columns.Add(header);
                    }

                    while (!sr.EndOfStream)
                    {
                        string[] rows = sr.ReadLine().Split(',');
                        if (rows.Length > 1)
                        {
                            DataRow dr = dt.NewRow();
                            for (int i = 0; i < headers.Length; i++)
                            {
                                dr[i] = rows[i].Trim();
                            }
                            dt.Rows.Add(dr);
                        }
                    }

                }


                return dt;
            }catch(Exception e)
            {
                return null;
            }
        }
      
        public async  Task<int> ConvertXSLXtoDataTable(string strFilePath, string connString,string userid)
        {

            Product p = new Product();
            
            OleDbConnection oledbConn = new OleDbConnection(connString);
            DataTable dt = new DataTable();
            try
            {

                oledbConn.Open();
                using (OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn))
                {
                    OleDbDataAdapter oleda = new OleDbDataAdapter();
                    oleda.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    oleda.Fill(ds);

                    dt = ds.Tables[0];


                    foreach (DataRow row in dt.Rows)
                    {
                        product.Add(new Product {
                            Name = row["Name"].ToString(),
                            Description = row["Description"].ToString(),
                            Price = Convert.ToDouble(row["Price"]),
                            //image = row["Image"].ToString()



                    });
                        //p.Quantity = Convert.ToInt32("Quantity");
                        //if (p.Quantity == 0)
                        //{

                        //}
                        //if (p.Quantity < 15)
                        //{

                        //}

                        if (p != null)
                        {
                            p.UserId = userid;
                          
                            p.Name = row["Name"].ToString();
                            p.Description = row["Description"].ToString();
                            p.Price = Convert.ToDouble(row["Price"]);
                          //  p.image = row["Image"].ToString();

                            await DocumentDBRepository<Product>.CreateItemAsync(p);
                        }
                    }
                            
                            
                            
                        //    new CustomerModel
                        //{
                        //    CustomerId = Convert.ToInt32(row["Id"]),
                        //    Name = row["Name"].ToString(),
                        //    Country = row["Country"].ToString()
                        //});
                    
                }
            }
            catch(Exception e)
            {
            }
            finally
            {

                oledbConn.Close();
            }

            return 1;
        }
    }
}