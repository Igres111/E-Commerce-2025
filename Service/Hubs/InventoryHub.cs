using Microsoft.AspNetCore.SignalR;

namespace E_Commerce_2025.Hubs
{
    public class InventoryHub:Hub
    {
        public async Task SendStockUpdate(Guid productId, int newStock)
        {
            await Clients.All.SendAsync("ReceiveStockUpdate", productId, newStock);
        }

        public async Task SubscribeToProduct(string productGroup)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, productGroup);
        }

        public async Task UnsubscribeFromProduct(string productGroup)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, productGroup);
        }
    }
}
