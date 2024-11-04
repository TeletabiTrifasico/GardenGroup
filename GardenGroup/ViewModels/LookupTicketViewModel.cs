using MongoDB.Bson;
using Service;

namespace GardenGroup.ViewModels;

public class LookupTicketViewModel(IServiceManager serviceManager, ObjectId ticketId)
{
    public IServiceManager ServiceManager => serviceManager;
    public ObjectId TicketId => ticketId;
}