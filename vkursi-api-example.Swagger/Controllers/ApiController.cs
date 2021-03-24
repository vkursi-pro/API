using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using vkursi_api_example.organizations;
using vkursi_api_example.person;
using vkursi_api_example.token;

namespace vkursi_api_example.Swagger.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly ILogger<ApiController> _logger;

        public ApiController(ILogger<ApiController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 1. Авторизація, отримання токену
        /// </summary>
        /// <remarks> 
        /// 1. Авторизація, отримання токену
        /// curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/token/authorize' \
        /// --header 'Content-Type: application/json' \
        /// --data-raw '{"email":"test@testemail.com","password":"123456"}'
        /// </remarks>
        /// <param name="AuthorizeRequestBody"></param>
        /// <response code="200">Authorized</response>
        /// <returns>returnsExample</returns>
        /// 

        [Route("1.0/token/authorize")]
        [HttpPost]
        public AuthorizeResponseModel Authorize(AuthorizeRequestBodyModel AuthorizeRequestBody)
        {
            AuthorizeClass Authorize = new AuthorizeClass();

            var token = Authorize.Authorize(AuthorizeRequestBody);

            return token;
        }

        /// <summary>
        /// 29. Отримання інформації по фізичній особі
        /// </summary>
        /// <remarks>
        /// 29. Отримання інформації по фізичній особі
        /// curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/person/checkperson' \
        /// --header 'ContentType: application/json' \
        /// --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
        /// --header 'Content-Type: application/json' \
        /// --data-raw '{"Id":null,"FullName":"ШЕРЕМЕТА ВАСИЛЬ АНАТОЛІЙОВИЧ","FirstName":null,"SecondName":null,"LastName":null,"Ipn":"2301715013","Doc":null,"Birthday":null,"RuName":null}'
        /// </remarks>
        /// <returns></returns>


        [Route("1.0/person/checkperson")]
        [HttpPost]
        public CheckPersonResponseModel CheckPerson([FromHeader(Name = "Authorization2")] string Authorization, CheckPersonRequestBodyModel CheckPersonRequestBodyRow)
        {
            var headerAuth = Request.Headers["Authorization"];


            Console.WriteLine();

            CheckPersonClass _сheckPerson = new CheckPersonClass();

            var CPResponseRow = _сheckPerson.CheckPerson(Authorization, CheckPersonRequestBodyRow);

            return CPResponseRow;
        }
    }
}
