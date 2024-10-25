using Microsoft.AspNetCore.SignalR;

namespace CourseProject.Hubs
{
    public class CommentsHub : Hub
    {
        public async Task SendComment(string templateId, string commentText, string commentedBy, string createdDate)
        {
            await Clients.Others.SendAsync("ReceiveComment", templateId, commentText, commentedBy, createdDate);
        }
    }
}
