using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public interface IEmailer
    {
        bool Send(string emailAddress,string password);
        Task SendAsync(string emailAddress);
    }
}
