
using AutoMapper;
using DesafioPonta.Api.Application.Mappings;

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
