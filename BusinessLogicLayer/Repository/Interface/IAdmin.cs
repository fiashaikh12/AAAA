using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IAdmin
    {
        ServiceRes IsAdminValid(AdminRequest adminRequest);
        ServiceRes UpdateCategory(int id, string name);
        ServiceRes DeleteCategory(int id);
        ServiceRes AddCategory(string name);
        ServiceRes AddSubCategory(string name, int categoryId);
        ServiceRes UpdateSubCategory(string name, int subCategoryId);
        ServiceRes DeleteSubCategory(int subCategoryId);
    }
}
