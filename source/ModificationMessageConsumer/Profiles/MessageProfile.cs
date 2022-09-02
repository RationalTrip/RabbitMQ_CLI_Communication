using AutoMapper;
using ModificationMessageConsumer.Models;
using RabbitMqShared.Dtos;

namespace ModificationMessageConsumer.Profiles
{
    internal class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<MessageUploadDto, Message>();
            CreateMap<Message, MessageModifyDto>();
        }
    }
}
