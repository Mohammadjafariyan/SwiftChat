using System;
using System.Threading.Tasks;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket.Tracking
{
    public class CustomerTabClosedSocketHandler:BaseMySocket
    {
        public override async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.ExecuteAsync(request, currMySocketReq);

            
            var customerTrackerService = Injector.Inject<CustomerTrackerService>();

            var trackInfo= MyGlobal.Clone(currMySocketReq.ChatConnection.Customer.LastTrackInfo);
            if (trackInfo==null)
            {
                trackInfo=new CustomerTrackInfo
                {
                    DateTime = DateTime.Now,
                    CustomerId = currMySocketReq.ChatConnection.CustomerId.Value,
                };
            }
            
            trackInfo.CustomerTrackInfoType = CustomerTrackInfoType.ExitWebsite;

            trackInfo.PrevTrackInfoId = trackInfo.Id;
            trackInfo.PrevTrackInfoDateTime = trackInfo.DateTime;
            trackInfo.TimeSpent = MySpecificGlobal.CalculateTimeSpentOnPage(trackInfo.DateTime);
            trackInfo.TimeSpentNum = MySpecificGlobal.CalculateTimeSpentOnPageNum(trackInfo.DateTime);

            trackInfo.Id = 0;
            
            trackInfo.DateTime=DateTime.Now;
            

            customerTrackerService.Save(trackInfo);

            currMySocketReq.ChatConnection.Customer.LastTrackInfo = trackInfo;

            return await Task.FromResult<MyWebSocketResponse>(null);
        }
    }
}