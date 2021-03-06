﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClientManagement.Core.Models;
using static ClientManagement.Tests.Constants;
using ClientManagement.Tests.Helpers;
using Moq;
using ClientManagement.Core.Repositories;
using ClientManagement.Core.Services;
using ClientManagement.Core.Exceptions;

namespace ClientManagement.Tests.Core.EmployeeTest
{

    [TestClass]
    public class EmployeeServiceTest
    {
        private Mock<IEmployeeRepository> _employeeRepoMock;

        [TestInitialize]
        public void BeforeEach()
        {

            var Employees = EmployeeData.Employees;
            var Projects = ProjectData.Projects;

            _employeeRepoMock = new Mock<IEmployeeRepository>();
            _employeeRepoMock.Setup(x => x.GetAllEmployees()).Returns(Employees);

            _employeeRepoMock
                .Setup(x => x.GetEmployee(It.IsAny<Guid>()))
                .Returns((Guid input) =>
                {
                    return Employees.FirstOrDefault(y => y.Id == input);
                });

            _employeeRepoMock
                .Setup(x => x.GetProject(It.IsAny<Guid>()))
                .Returns((Guid input) =>
                {
                    return Projects.FirstOrDefault(y => y.Id == input);
                });
            
            

        }

        [TestMethod, TestCategory(UnitTest)]
        public void Should_Be_Able_To_Create_An_EmployeeService_Instance()
        {
            var employeeService = new EmployeeServices(_employeeRepoMock.Object);

            Assert.IsNotNull(employeeService);
        }

        [TestMethod, TestCategory(UnitTest)]
        public void Should_Be_Able_To_Retrieve_An_Employee()
        {
            var employeeService = new EmployeeServices(_employeeRepoMock.Object);

            var employee = employeeService.GetEmployee(EmployeeData.User2Id);


            Assert.AreEqual("Lola", employee.Firstname);
            Assert.IsInstanceOfType(employee, typeof(Employee));

        }

        [TestMethod, TestCategory(UnitTest)]
        public void Should_Be_Able_To_Retrieve_An_All_Employees()
        {
            var employeeService = new EmployeeServices(_employeeRepoMock.Object);

            var employees = employeeService.GetAllEmployees();


            Assert.AreEqual(2, employees.Count);
            Assert.IsInstanceOfType(employees, typeof(List<Employee>));
        }


        [TestMethod, TestCategory(UnitTest)]
        public void Should_Be_Able_To_Assign_Project_To_An_Employee()
        {
            var employeeService = new EmployeeServices(_employeeRepoMock.Object);
            var employeeProject = EmployeeData.employeeProject;
            var EmployeeId = employeeProject.EmployeeId;
            var ProjectId = employeeProject.ProjectId;

            employeeService.AssignProjectToEmployee(employeeProject);

            _employeeRepoMock.Verify(x => x.AssignProject(EmployeeId, ProjectId), Times.Once);
        }


        [TestMethod, TestCategory(UnitTest)]
        public void Should_Be_Able_To_ReAssign_Project_To_An_Employee()
        {
            var employeeService = new EmployeeServices(_employeeRepoMock.Object);
            var employeeProject = EmployeeData.employeeProject;
            var EmployeeId = employeeProject.EmployeeId;
            var ProjectId = employeeProject.ProjectId;

            employeeService.RemoveEmployeeFromProject(employeeProject);

            _employeeRepoMock.Verify(x => x.RemoveProject(EmployeeId, ProjectId), Times.Once);
        }

        [TestMethod, TestCategory(UnitTest)]
        public void Should_Be_Able_To_Save_An_Employee()
        {
            var employeeService = new EmployeeServices(_employeeRepoMock.Object);
            var employee = EmployeeData.employee;

            employeeService.Save(employee);


            _employeeRepoMock.Verify(x => x.Update(employee), Times.Once);
        }


        [TestMethod, TestCategory(UnitTest)]
        public void Should_Be_Able_To_Save_A_Unique_Employee()
        {
            var employeeService = new EmployeeServices(_employeeRepoMock.Object);
            var employee = EmployeeData.employee3;

            employeeService.Save(employee);


            _employeeRepoMock.Verify(x => x.Create(employee), Times.Once);
        }
    }
}
