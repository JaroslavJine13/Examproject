using WebMud.Models;

namespace WebMud.Data
{
    public interface ITicketsService
    {

        Task<List<Tickets>> GetTickets();

        Task<bool> AddTicket(Tickets tickets);

        Task<bool> DeleteTicket(Guid id);

        Task<Tickets> GetTicket(Guid id);

        Task<bool> TicketIsNoticed(Guid id, bool isnoticed);

        Task<bool> TicketIsCompleted(Guid id);

        Task<bool> UpdateTicket(Tickets tickets);

        Task<int> TicketTotalCount();

        Task<List<Tickets>> GetDashboardTickets();

    }
}