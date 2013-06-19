using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Models;
using EggFarmSystem.Services;

namespace EggFarmSystem.Client.Core.Services
{
    public class ConsumableServiceClient : ServiceClient, IEmployeeService
    {
        public ConsumableServiceClient(IClientContext clientContext) : base(clientContext)
        {
            
        }

        protected override string ResourceUrl
        {
            get { return "/consumables"; }
        }

        public IList<Employee> GetAll()
        {
            return CreateGetAllRequest<List<Employee>>();
        }

        public Employee Get(Guid id)
        {
            return CreateGetRequest<Employee>(id);
        }

        public bool Save(Employee model)
        {
            return model.IsNew ? CreatePostRequest(model) : CreatePutRequest(model.Id, model);
        }

        public bool Delete(Guid id)
        {
            return CreateDeleteRequest(id);
        }
    }
}
