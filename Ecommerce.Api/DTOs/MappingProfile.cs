
using AutoMapper;
using Ecommerce.Api.DTOs.EcommerceStore;



namespace Ecommerce.Api.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapping #1: Entity -> DTO
            // This tells AutoMapper to map from EcommerceStore (database model) to ReadEcommerceStoreDto (DTO for API responses).
            CreateMap<Ecommerce.Data.Models.EcommerceStore, ReadEcommerceStoreDto>();

               // Mapping #2: DTO -> Entity (for Create)
            // This tells AutoMapper to map from CreateEcommerceStoreDto (data provided by client when creating a store) to EcommerceStore (database entity).
            CreateMap<CreateEcommerceStoreDto, Ecommerce.Data.Models.EcommerceStore>();

               // Mapping #3: DTO -> Entity (for Update)
            // This tells AutoMapper to map from UpdateEcommerceStoreDto (data provided by client when updating a store) to EcommerceStore (database entity).
            CreateMap<UpdateEcommerceStoreDto, Ecommerce.Data.Models.EcommerceStore>();

        }

    }


}