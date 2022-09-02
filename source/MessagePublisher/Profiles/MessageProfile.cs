using AutoMapper;
using MessagePublisher.Models;
using RabbitMqShared.Dtos;

namespace MessagePublisher.Profiles
{
    internal class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<Message, MessageUploadDto>();

            CreateMap<MessageModifyDto, Message>();

            CreateMap<MessageModifyDto, MessageUploadDto>();
        }
    }
}
