using System;
using System.Linq;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models.Compaign;
using SignalRMVCChat.Service.Compaign;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.Compaign
{
    public class CompaignSaveSocketHandler : SaveSocketHandler<Models.Compaign.Compaign, CompaignService>
    {
        public CompaignSaveSocketHandler() : base("compaignSaveCallback")
        {
        }


        protected override Models.Compaign.Compaign SetParams(Models.Compaign.Compaign record,
            Models.Compaign.Compaign existRecord)
        {
            if (_currMySocketReq.MySocket.MyAccountId.HasValue == false)
            {
                Throw("این عملیات مخصوص اوپراتور است");
            }

            record.LastChangeDateTime = DateTime.Now;


            record.MyWebsiteId = _currMySocketReq.MyWebsite.Id;
            record.MyAccountId = _currMySocketReq.MySocket.MyAccountId.Value;


            record.selectedBotId = record.selectedBot != null ? record.selectedBot.Id : record.selectedBotId.HasValue ? record.selectedBotId : null;
            record.selectedEventTriggerId = record.selectedEventTrigger != null ? record.selectedEventTrigger.Id : record.selectedEventTriggerId.HasValue ? record.selectedEventTriggerId : null;

            record.selectedBot = null;
            record.selectedEventTrigger = null;



            if (record.saveAsTemplate && record.Template != null)
            {

                var CompaignTemplateService = Injector.Inject<CompaignTemplateService>();

                var exist = CompaignTemplateService.GetQuery().Any(q =>
                  q.Name == record.saveAsTemplateName
                 );

                if (!exist)
                {

                    CompaignTemplateService.Save(new CompaignTemplate
                    {
                        Html = record.Template.Html,
                        Name = record.saveAsTemplateName,
                        MyWebsiteId = _currMySocketReq.MyWebsite.Id,
                        CompaignId = record.Id
                    });
                }


            }

            record.IsConfigured = true;



            if (record.Id == 0)
            {
                record.Name = "کمپین جدید" + $@"{DateTime.Now.Second}-{DateTime.Now.Minute}";

            }


            return base.SetParams(record, existRecord);
        }
    }
    }