using AutoMapper;
using ToDoListApp1.Models;
using ToDoListApp1.Models.VMs;

namespace ToDoListApp1
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Entity to ViewModel Mappings
            // ToDoItem modelinden ToDoListVM view modeline dönüşüm yapar
            CreateMap<ToDoItem, ToDoListVM>();

            // ToDoItem modelinden ToDoEditVM view modeline dönüşüm yapar
            CreateMap<ToDoItem, ToDoEditVM>();

            // ToDoItem modelinden ToDoDetailsVM view modeline dönüşüm yapar
            CreateMap<ToDoItem, ToDoDetailsVM>();

            // ToDoItem modelinden ToDoDeleteVM view modeline dönüşüm yapar
            CreateMap<ToDoItem, ToDoDeleteVM>();

            // ViewModel to Entity Mappings
            // ToDoCreateVM view modelinden ToDoItem modeline dönüşüm yapar
            CreateMap<ToDoCreateVM, ToDoItem>();

            // ToDoEditVM view modelinden ToDoItem modeline dönüşüm yapar
            CreateMap<ToDoEditVM, ToDoItem>();

            // RegisterVM view modelinden User modeline dönüşüm yapar
            // RegisterVM'deki Email alanını hem UserName hem de Email olarak map eder
            CreateMap<RegisterVM, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            // Eğer LoginVM ile User arasında da bir eşleme yapmanız gerekirse, burada ekleyebilirsiniz
            // CreateMap<LoginVM, User>(); gibi
        }
    }
}
