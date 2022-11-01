using Catalog.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = Catalog.DAL.Entity.Type;

namespace Catalog.DAL.Repository.Interface
{
    public interface ITypeRepository : IGenericRepository<Type>
    {
    }
}
