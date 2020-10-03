using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket.FormCreator
{
    public class AdminSendFormToCustomerSocketHandler : BaseFormCreatorSocketHandler
    {
        private ChatProviderService ChatProviderService = Injector.Inject<ChatProviderService>();

        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.ExecuteAsync(request, currMySocketReq);

            //=============================================================================
            _logService.LogFunc("AdminSendFormToCustomerSocketHandler");
            //=============================================================================

            #region validation

            var form = ValidateForm();

            int customerId = GetParam<int>("customerId", true, "کد کاربر یافت نشد");
            CustomerProviderService.GetById(customerId, "بازدید کننده یافت نشد");

            #endregion

            //=============================================================================
            _logService.LogFunc("validation done");
            //=============================================================================


            var formElements = FormElementService.GetFormElements(form.Id);
            form.Elements = formElements;


            //=============================================================================
            _logService.LogFunc("formElements" + formElements?.Count);
            //=============================================================================

            //=============================================================================
            _logService.LogFunc("GET UNIQ ID");
            //=============================================================================

            
            int UniqId = GetParam<int>("UniqId", true, "UniqId not found");

            
            
            //=============================================================================
            _logService.LogFunc("save chat");
            //=============================================================================

            int chatId=ChatProviderService.AdminSendToCustomer(currMySocketReq.MySocket.MyAccountId.Value,
                customerId, "", currMySocketReq.MySocket.Id, 0, UniqId, form.Id).Single;

            //=============================================================================
            _logService.LogFunc("END");
            //=============================================================================

            
            
            
            await MySocketManagerService.SendToCustomer(customerId, currMySocketReq.MyWebsite.Id,
                new MyWebSocketResponse
                {
                    Content = new
                    {
                        form,
                        chatId
                    },
                    Name = "adminSendFormToCustomerCallback"
                });

            //=============================================================================
            _logService.LogFunc("adminSendFormToCustomerCallback customerId" + customerId);
            //=============================================================================

            return new MyWebSocketResponse();
        }
    }
}