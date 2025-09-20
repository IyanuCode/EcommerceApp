
using AutoMapper;
using Ecommerce.Api.DTOs.EcommerceStore;
using Ecommerce.Data.Models.Auth;
using Ecommerce.Api.DTOs.AuthDto;


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

              /* -------------------- Auth / User -------------------- */
            // Mapping #4: DTO -> Entity (for Register)
            // This tells AutoMapper to map from RegisterRequestDto (data provided by client when registering a user) to User (database entity).
            CreateMap<RegisterRequestDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // Ignore PasswordHash because we hash manually
        }

    }


}