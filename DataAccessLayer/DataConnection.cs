using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using DEL;

namespace DataAccessLayer
{
    public class DataConnection
    {

        public static string pathOne = ConfigurationManager.AppSettings["XmlFileCreate"];

        public static string pathTwo = ConfigurationManager.AppSettings["UpdatedDbFileCreate"];

        public SqlConnection GetConnection()
        {

            GlobalVariableClass gv = new GlobalVariableClass();
            gv.ConnectionString = ConfigurationManager.ConnectionStrings["MyCon"].ConnectionString;

            SqlConnection sqlConnection = new SqlConnection(gv.ConnectionString);
            return sqlConnection;

        }

        public void Serialize()
        {
            EmployeeDetail employeeDetail = new EmployeeDetail();
            XmlSerializer xs = new XmlSerializer(typeof(List<EmployeeDetail>), new XmlRootAttribute("EmployeeDetails"));
            var employeeList = employeeDetail.GetEmployeeList();

            using (StreamWriter writer = new StreamWriter(pathOne))
            {
                xs.Serialize(writer, employeeList);
            }
        }

        public void CreateTableQuery()
        {

            var sqlConnection = GetConnection();
            sqlConnection.Open();


            string employeeDetailTable = @"Create Table [EmployeeDetail] ( id int, employeeName char(50), age int)";
            SqlCommand com = new SqlCommand(employeeDetailTable, sqlConnection);
            com.ExecuteNonQuery();

            string deptTable = @"Create Table [DepartmentDetail] (Id int, DepartmentId int, DepartmentName char(50) )";
            SqlCommand comTwo = new SqlCommand(deptTable, sqlConnection);
            comTwo.ExecuteNonQuery();

            string addressTable = @"Create Table [AddressDetail] (Id int, HouseNumber int, HouseName char(50)) ";
            SqlCommand comThree = new SqlCommand(addressTable, sqlConnection);
            comThree.ExecuteNonQuery();

            string salaryTable = @"Create Table [Salary] (Id int, SalaryOfEmployee int) ";
            SqlCommand comFour = new SqlCommand(salaryTable, sqlConnection);
            comFour.ExecuteNonQuery();

            sqlConnection.Close();

        }

        public void InsertData()
        {

            var sqlConnection = GetConnection();
            sqlConnection.Open();

            DataSet ds = new DataSet();
            ds.ReadXml(pathOne);

            DataTable dtEmp = ds.Tables["EmployeeDetail"];
            DataTable dtAddress = ds.Tables["AddressDetail"];
            DataTable dtDept = ds.Tables["DepartmentDetail"];
            DataTable dtSal = ds.Tables["Salary"];

            using (SqlBulkCopy bc = new SqlBulkCopy(sqlConnection))
            {
                bc.DestinationTableName = "EmployeeDetail";
                bc.ColumnMappings.Add("id", "id");
                bc.ColumnMappings.Add("employeeName", "employeeName");
                bc.ColumnMappings.Add("age", "age");
                bc.WriteToServer(dtEmp);
            }

            using (SqlBulkCopy bc = new SqlBulkCopy(sqlConnection))
            {
                bc.DestinationTableName = "AddressDetail";
                bc.ColumnMappings.Add("Id", "Id");
                bc.ColumnMappings.Add("HouseNumber", "HouseNumber");
                bc.ColumnMappings.Add("HouseName", "HouseName");
                bc.WriteToServer(dtAddress);
            }

            using (SqlBulkCopy bc = new SqlBulkCopy(sqlConnection))
            {
                bc.DestinationTableName = "DepartmentDetail";
                bc.ColumnMappings.Add("Id", "Id");
                bc.ColumnMappings.Add("DepartmentId", "DepartmentId");
                bc.ColumnMappings.Add("DepartmentName", "DepartmentName");
                bc.WriteToServer(dtDept);
            }

            using (SqlBulkCopy bc = new SqlBulkCopy(sqlConnection))
            {
                bc.DestinationTableName = "Salary";
                bc.ColumnMappings.Add("Id", "Id");
                bc.ColumnMappings.Add("SalaryOfEmployee", "SalaryOfEmployee");
                bc.WriteToServer(dtSal);
            }

            sqlConnection.Close();
        }

        public void FetchData()
        {

            var sqlConnection = GetConnection();
            sqlConnection.Open();

            var query = @"select e.employeeName, a.HouseName, d.DepartmentId
                            from EmployeeDetail e 
                            join DepartmentDetail d
                            on e.id = d.Id
                            join AddressDetail a
                            on d.Id = a.Id
                            where d.DepartmentId = 10;";

            SqlDataAdapter sda = new SqlDataAdapter(query, sqlConnection);
            DataSet ds = new DataSet();
            sda.Fill(ds);

            ds.WriteXml(pathTwo, XmlWriteMode.IgnoreSchema);

        }

    }
}
