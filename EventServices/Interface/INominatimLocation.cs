using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventServices.Interface
{
    public interface INominatimLocation
    {
        public Task<string> GetMapUrlForAddress(string address);
        public Task<RestResponse> IfCorrectAddress(string address);
    }
}
