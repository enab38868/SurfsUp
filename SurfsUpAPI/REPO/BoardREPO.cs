using System.Security.Cryptography.X509Certificates;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace SurfsUpAPI.REPO
{
    public class BoardREPO
    {
        private IConfiguration configuration;

        public BoardREPO(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public string ConString()
        {

            return configuration.GetConnectionString("APIContext");
            // SurfsUpProjektContext 

        }
    }
}
