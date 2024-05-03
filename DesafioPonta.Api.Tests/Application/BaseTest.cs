using AutoMapper;
using DesafioPonta.Api.Application.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioPonta.Api.Tests.Application
{
    public class BaseTest
    {

        public IMapper GetMapperProfile()
        {
            var config = new MapperConfiguration(options =>
            {
                options.AddProfile<DomainToDtoMapping>();
                options.AddProfile<DtoToDomainMapping>();
            });

            return config.CreateMapper();
        }

    }
}
