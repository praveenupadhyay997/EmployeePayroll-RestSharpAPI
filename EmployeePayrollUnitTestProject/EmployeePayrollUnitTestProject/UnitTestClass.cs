// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitTestClass.cs" company="Bridgelabz">
//   Copyright © 2018 Company
// </copyright>
// <creator Name="Praveen Kumar Upadhyay"/>
// --------------------------------------------------------------------------------------------------------------------
namespace EmployeePayrollUnitTestProject
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using RestSharp;
    using System.Collections.Generic;
    using System.Net;

    [TestClass]
    public class UnitTestClass
    {
        /// <summary>
        /// Instantinating the rest client class which translates a dedicated resp api operation to https request
        /// </summary>
        RestClient restClient;
        /// <summary>
        /// Initialising the base url as the base for the underlying data
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            restClient = new RestClient("http://localhost:3000");
        }
        /// <summary>
        /// Method to get the data in json format requested from the api's data hosting server
        /// </summary>
        /// <returns></returns>
        public IRestResponse GetEmployeeList()
        {
            /// Arrange
            RestRequest request = new RestRequest("/employees", Method.GET);
            /// Act
            IRestResponse response = restClient.Execute(request);
            /// Returning the json formatted result block
            return response;
        }
        /// <summary>
        /// TC 1 -- On calling the employee rest API return the list of the schema stored inside the database
        /// </summary>
        [TestMethod]
        public void OnCallingTheEmplyeeRestAPI_RetrievesAllData()
        {
            /// Act 
            IRestResponse response = GetEmployeeList();
            /// Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

            List<Employee> employeesDataResponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(5, employeesDataResponse.Count);

            foreach(Employee employee in employeesDataResponse)
            {
                System.Console.WriteLine($"ID : {employee.id} , Name : {employee.name}, Salary : {employee.salary}");
            }
        }
        /// <summary>
        /// TC 2 -- On calling the employee rest API return the Employee data of the schema stored inside the database
        /// </summary>
        [TestMethod]
        public void OnAddingTheEmplyeeRestAPI_ValidateSuccessFullCount()
        {
            /// Arrange
            RestRequest restRequest = new RestRequest("/employees", Method.POST);
            /// Instantinating a json object
            JObject jObject = new JObject();
            /// Adding the data attribute with data elements
            jObject.Add("name", "Maruti");
            jObject.Add("salary", "10000");
            /// Adding parameter to the rest request
            restRequest.AddParameter("application/json", jObject, ParameterType.RequestBody);
            /// Act
            IRestResponse restResponse = restClient.Execute(restRequest);
            /// Assert
            Assert.AreEqual(restResponse.StatusCode, HttpStatusCode.Created);
            /// Getting the recently added data as json format and then deserialise it to Employee object
            Employee employeeDateResponse = JsonConvert.DeserializeObject<Employee>(restResponse.Content);
            Assert.AreEqual("Maruti", employeeDateResponse.name);
            Assert.AreEqual("10000", employeeDateResponse.salary);
        }
    }
}
