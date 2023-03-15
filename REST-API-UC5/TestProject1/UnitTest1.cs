using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
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
        public IRestResponse GetEmployeeList()
        {
            /// Arrange
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
        // TC 2 -- On calling the employee rest API after the data addition return the Employee data of the schema stored inside the database
        [TestMethod]
        public void OnAddingTheEmplyeeRestAPI_ValidateSuccessFullCount()
        {
            // Arrange
            RestRequest restRequest = new RestRequest("/employees", Method.Post);
            // Instantinating a json object
            JObject jObject = new JObject();
            // Adding the data attribute with data elements
            jObject.Add("name", "Meghana");
            jObject.Add("salary", "10000");
            // Adding parameter to the rest request
            restRequest.AddParameter("application/json", jObject, ParameterType.RequestBody);
            // Act
            IRestResponse restResponse = restClient.Execute(restRequest);
            // Assert 201-- Code for post
            Assert.AreEqual(restResponse.StatusCode, HttpStatusCode.Created);
            // Getting the recently added data as json format and then deserialise it to Employee object
            Employee employeeDateResponse = JsonConvert.DeserializeObject<Employee>(restResponse.Content);
            Assert.AreEqual("Meghana", employeeDateResponse.name);
            Assert.AreEqual("10000", employeeDateResponse.salary);
        }
        // TC 3 -- On calling the employee rest API after the data addition return the Employee data of the schema stored inside the database
        [TestMethod]
        public void MultipleAdditionToTheEmplyeeRestAPI_ValidateSuccessFullCount()
        {
            // Storing multiple employee data to a list
            List<Employee> employeeList = new List<Employee>();
            // Adding the data to the list
            employeeList.Add(new Employee { name = "Kanna", salary = "60000" });
            employeeList.Add(new Employee { name = "Keerthi", salary = "50000" });
            employeeList.Add(new Employee { name = "Ananya", salary = "40000" });
            // Iterating over the employee list to get each instance
            employeeList.ForEach(employeeData =>
            {
                // Arrange
                // adding the request to post data to the rest api
                RestRequest request = new RestRequest("/employees", Method.Post);

                // Instantinating a Json object to host the employee in json format
                JObject jObject = new JObject();
                // Adding the data attribute with data elements
                jObject.Add("name", employeeData.name);
                jObject.Add("salary", employeeData.salary);
                // Adding parameter to the rest request jObject - contains the parameter list of the json database
                request.AddParameter("application/json", jObject, ParameterType.RequestBody);
                // Act
                // Adding the data to the json server in json format
                IRestResponse response = restClient.Execute(request);
                // Assert
                // 201-- Code for post
                Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Created);
                // Getting the recently added data as json format and then deserialise it to Employee object
                Employee employeeDataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
                Assert.AreEqual(employeeData.name, employeeDataResponse.name);
                Assert.AreEqual(employeeData.salary, employeeDataResponse.salary);
            });
        }
        // TC 4 -- On calling the employee rest API after the data update return the updated Employee data of the schema stored inside the database
        [TestMethod]
        public void UpdateDataInEmplyeeRestAPI_ValidateUpdateSuccess()
        {
            // Arrange
            // Adding the request to put or update data to the rest api
            RestRequest request = new RestRequest("/employees/9", Method.Put);

            // Instantinating a Json object to host the employee in json format
            JObject jObject = new JObject();
            // Adding the data attribute with data elements
            jObject.Add("name", "Ananaya");
            jObject.Add("salary", "50000");
            // Adding parameter to the rest request jObject - contains the parameter list of the json database
            request.AddParameter("application/json", jObject, ParameterType.RequestBody);
            // Act
            // Adding the data to the json server in json format
            IRestResponse response = restClient.Execute(request);
            // Assert
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            // Getting the recently added data as json format and then deserialise it to Employee object
            Employee employeeDataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            // Assert updated data
            Assert.AreEqual("Ananaya", employeeDataResponse.name);
            Assert.AreEqual("50000", employeeDataResponse.salary);
            Console.WriteLine(response.Content);
        }
        // TC 5 -- On calling the employee rest API after the data delete return the arret value with status code
        [TestMethod]
        public void DeleteDataInEmplyeeRestAPI_ValidateDeleteSuccess()
        {
            // Arrange
            // Adding the request to put or update data to the rest api
            RestRequest request = new RestRequest("/employees/9", Method.Delete);
            // Act
            // Adding the data to the json server in json format
            IRestResponse response = restClient.Execute(request);
            // Assert
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            // Printing the respose content after delete operation
            Console.WriteLine(response.Content);
        }
    }
}