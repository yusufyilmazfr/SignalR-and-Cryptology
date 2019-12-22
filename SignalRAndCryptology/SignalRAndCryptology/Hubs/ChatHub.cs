using Microsoft.AspNetCore.SignalR;
using SignalRAndCryptology.Cryptology.Abstract;
using SignalRAndCryptology.Cryptology.Concrete;
using SignalRAndCryptology.Cryptology.Concrete.Enum;
using SignalRAndCryptology.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SignalRAndCryptology.Hubs
{
    public class ChatHub : Hub
    {
        private static List<string> _activeUserKeys = new List<string>();

        public string CurrentConnectionId => Context.ConnectionId;

        public async Task CreateNewMessage(MessageModel messageModel)
        {
            messageModel.ToLongTimeString = DateTime.Now.ToLongTimeString();

            ICryptor cryptor = CryptorFactory.CreateInstance(messageModel.CryptorType);

            messageModel.Message = await cryptor.Encrypt(messageModel);

            await Clients.All.SendAsync("GetEncryptNewMessage", messageModel);
        }

        public async Task Decrypt(MessageModel messageModel)
        {
            ICryptor cryptor = CryptorFactory.CreateInstance(messageModel.CryptorType);

            messageModel.Message = await cryptor.Decrypt(messageModel);

            await Clients.Caller.SendAsync("GetDecryptNewMessage", messageModel);
        }

        public void SendOnlineMemberCount()
        {
            Clients.All.SendAsync("GetOnlineMemberCount", _activeUserKeys.Count);
        }

        public override Task OnConnectedAsync()
        {
            _activeUserKeys.Add(CurrentConnectionId);

            SendOnlineMemberCount();

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _activeUserKeys.Remove(CurrentConnectionId);

            SendOnlineMemberCount();

            return base.OnDisconnectedAsync(exception);
        }

    }
}
