using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNet.SignalR.Hubs;
using Moq;
using System.Dynamic;
using ShareTube.Hubs;
using ShareTube.Controllers;
using ShareTube.Models;
using Microsoft.AspNet.SignalR;
using System.Security.Principal;
using System.Net;
using ShareTube.Infrastructure;
using ShareTube.Data;

namespace ShareTube.Tests
{

    public class ShareTubeHubTests
    {
        public Mock<IShareTubeClientContract> contract;
        Mock<IHubCallerConnectionContext<dynamic>> mockClients;
        ShareTubeHub hub;

        Guid connId = Guid.NewGuid();
        string connectionId { get { return connId.ToString(); } }
        Guid userID = Guid.NewGuid();
        User user = new User
        {
            Name = "Test User"
        };
        Room room = new Room
        {
            ID = Guid.NewGuid()
        };


        public ShareTubeHubTests()
        {
            //setup client contract
            contract = new Mock<IShareTubeClientContract>();

            var cookieHelper = new Mock<ICookieHelper>();
            cookieHelper.Setup(x => x.GetOrAddUserID()).Returns(userID);

            var service = new Mock<IShareTubeService>();
            service.Setup(x => x.GetUserByConnection(It.IsAny<Guid>())).Returns(user);
            service.Setup(x => x.GetRoom(It.IsAny<Guid>())).Returns(room);
            service.Setup(x => x.GetVideos(It.IsAny<Guid>())).Returns(new List<Video>());
            service.Setup(x => x.GetHostConnectionID(It.IsAny<Guid>())).Returns(connId);

            hub = new ShareTubeHub(service.Object, contract.Object, cookieHelper.Object);
            
            var mockUser = new Mock<IPrincipal>();
            var mockRequest = new Mock<IRequest>();
            mockClients = new Mock<IHubCallerConnectionContext<dynamic>>();
            mockRequest.Setup(r => r.User).Returns(mockUser.Object);

            hub.Context = new HubCallerContext(mockRequest.Object, connectionId);
            hub.Groups = new Mock<IGroupManager>().Object;
            hub.Clients = mockClients.Object;
        }

        [Fact]
        public void Can_Join_Room()
        {
            contract.Setup(m => m.BroadcastMessage(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Verifiable();
            contract.Setup(m => m.IsHostCallback(It.IsAny<object>(), It.IsAny<bool>(), It.IsAny<string>())).Verifiable();
            mockClients.Setup(m => m.All).Returns(contract.Object);
            mockClients.Setup(m => m.Caller).Returns(contract.Object);

            hub.Server_JoinRoom(Guid.NewGuid().ToString(), "");

            contract.VerifyAll();
        }

        [Fact]
        public void Can_Broadcast_Message()
        {
            contract.Setup(m => m.BroadcastMessage(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Verifiable();
            contract.Setup(m => m.IsHostCallback(It.IsAny<object>(), It.IsAny<bool>(), It.IsAny<string>()));
            mockClients.Setup(m => m.All).Returns(contract.Object);
            mockClients.Setup(m => m.Caller).Returns(contract.Object);
            hub.Server_JoinRoom(Guid.NewGuid().ToString(), "");

            hub.Server_SendChatMessage("TestMessage");
            contract.VerifyAll();
        }

        [Fact]
        public void Can_Broadcast_User_List()
        {
            contract.Setup(m => m.BroadcastUserList(It.IsAny<object>(), It.IsAny<string>())).Verifiable();
            contract.Setup(m => m.IsHostCallback(It.IsAny<object>(), It.IsAny<bool>(), It.IsAny<string>()));
            mockClients.Setup(m => m.All).Returns(contract.Object);
            mockClients.Setup(m => m.Caller).Returns(contract.Object);
            hub.Server_JoinRoom(Guid.NewGuid().ToString(), "");

            hub.Server_GetUserList();

            contract.VerifyAll();
        }

        [Fact]
        public void Can_Broadcast_Current_Time()
        {
            contract.Setup(m => m.BroadcastCurrentTime(It.IsAny<object>(), It.IsAny<double>(), It.IsAny<long>())).Verifiable();
            contract.Setup(m => m.IsHostCallback(It.IsAny<object>(), It.IsAny<bool>(), It.IsAny<string>()));
            mockClients.Setup(m => m.All).Returns(contract.Object);
            mockClients.Setup(m => m.Caller).Returns(contract.Object);
            hub.Server_JoinRoom(Guid.NewGuid().ToString(), "");

            //act
            hub.Server_UpdatePlayerTime(It.IsAny<double>(), It.IsAny<long>());

            contract.VerifyAll();
        }

        [Fact]
        public void Can_Broadcast_Loop_Status()
        {
            contract.Setup(m => m.BroadcastLoopStatus(It.IsAny<object>(), It.IsAny<bool>()))
                .Verifiable();
            contract.Setup(m => m.IsHostCallback(It.IsAny<object>(), It.IsAny<bool>(), It.IsAny<string>()));
            mockClients.Setup(m => m.All).Returns(contract.Object);
            mockClients.Setup(m => m.Caller).Returns(contract.Object);
            hub.Server_JoinRoom(Guid.NewGuid().ToString(), "");

            hub.Server_SetLoop(It.IsAny<bool>());

            contract.VerifyAll();
        }

        [Fact]
        public void Can_Broadcast_Player_Status()
        {
            contract.Setup(m => m.BroadcastPlayerStatus(It.IsAny<object>(), It.IsAny<int>(), It.IsAny<bool>()))
                .Verifiable();
            contract.Setup(m => m.IsHostCallback(It.IsAny<object>(), It.IsAny<bool>(), It.IsAny<string>()));
            mockClients.Setup(m => m.All).Returns(contract.Object);
            mockClients.Setup(m => m.Caller).Returns(contract.Object);
            hub.Server_JoinRoom(Guid.NewGuid().ToString(), "");

            hub.Server_UpdateStatus(It.IsAny<int>());

            contract.VerifyAll();
        }
    }
}
