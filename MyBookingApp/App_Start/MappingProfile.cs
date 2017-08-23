using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyBookingApp.Models;
using AutoMapper;


namespace MyBookingApp.App_Start
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
           
            
           CreateMap<Book, BookDto>();
           CreateMap<Book, BookDetailDto>();
           CreateMap<BookDto, Book>().ForMember(m => m.Id, opt => opt.Ignore());
           CreateMap<BookDetailDto, Book>().ForMember(m => m.Id, opt => opt.Ignore());
           

            
        }
    }
}


