﻿using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface ICommonRepository
    {
        ServiceRes GetStates();
        ServiceRes GetCitiesByState(States states);
        ServiceRes GetGenders();
        ServiceRes GetBusinessType();
        string Base64toImage(string base64string, string directory, string subdirectory,string fileName);
        bool RemoveFileFromDirectory(string directory, string subdirectory, string fileLocation);
    }
}
