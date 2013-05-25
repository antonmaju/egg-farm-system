using EggFarmSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Services
{
    public interface IHenService
    {
        SearchResult<Hen> Search();

        Hen Save(HenService hen);

        bool Delete(Guid id);
    }


    public class HenService : IHenService
    {

    }
}
