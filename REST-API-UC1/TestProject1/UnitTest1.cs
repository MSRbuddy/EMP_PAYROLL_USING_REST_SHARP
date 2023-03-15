using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Net;

namespace EmployeePayrollUnitTestProject
{
    
    [TestClass]
    public class UnitTestClass
    {
        // Instantinating the rest client class which translates a dedicated resp api operation to https request        
        RestClient restClient;
        // Initialising the base url as the base for the underlying data
        [TestInitialize]
        public void Setup()
        {
            restClient = new RestClient("http://localhost:3000");
        }
        // Method to get the data in json format requested from the api's data hosting server
        private IRestResponse GetEmployeeList()
        {
            // Arrange
            RestRequest request = new RestRequest("/employees", Method.Get);
            // Act
            IRestResponse response = restClient.Execute(request);
            // Returning the json formatted result block
            return response;
        }
        // TC 1 -- On calling the employee rest API return the list of the schema stored inside the database
        [TestMethod]
        public void OnCallingTheEmplyeeRestAPI_RetrievesAllData()
        {
            // Act 
            IRestResponse response = GetEmployeeList();
            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

            List<Employee> employeesDataResponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(5, employeesDataResponse.Count);

            foreach (Employee employee in employeesDataResponse)
            {
                System.Console.WriteLine($"ID : {employee.id} , Name : {employee.name}, Salary : {employee.salary}");
            }
        }
    }
}