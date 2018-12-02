using Entities;

namespace Interface
{
    public interface IMessage
    {
        ServiceRes Send(Messages messages);
        ServiceRes AllMessages(Messages messages);
    }
}
