using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Models;
using EggFarmSystem.Services;

namespace EggFarmSystem.Client.Core.Services
{
    public class EmployeeServiceClient : ServiceClient, IEmployeeService
    {
        public EmployeeServiceClient(IClientContext clientContext) : base(clientContext)
        {
            
        }

        protected override string ResourceUrl
        {
            get { return "/employees"; }
        }

        public IList<Employee> GetAll()
        {
            return CreateGetAllRequest<List<Employee>>();
        }

        public Models.Employee Get(Guid id)
        {
            return CreateGetRequest<Employee>(id);
        }

        public void Save(Models.Employee model)
        {
            if(model.IsNew)
                CreatePostRequest(model);
            else
                CreatePutRequest(model.Id, model);
        }

        public void Delete(Guid id)
        {
            CreateDeleteRequest(id);
        }
    }
}
