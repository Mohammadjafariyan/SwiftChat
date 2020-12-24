using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket.FormCreator
{
    public class AdminSendFormToCustomerSocketHandler : BaseFormCreatorSocketHandler
    {
        private ChatProviderService ChatProviderService = Injector.Inject<ChatProviderService>();
        private MyAccountProviderService MyAccountProviderService = Injector.Inject<MyAccountProviderService>();

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
            var systemMyAccount = MyAccountProviderService.GetSystemMyAccount(currMySocketReq.MyWebsite.Id);

            int myAccountId = currMySocketReq.MySocket.MyAccountId ?? systemMyAccount.Id;
            int mySocketId = currMySocketReq.MySocket.MyAccountId.HasValue ? currMySocketReq.MySocket.Id : systemMyAccount.MySockets.ToList().Select(t => t.Id).FirstOrDefault();

            int chatId = ChatProviderService.AdminSendToCustomer(myAccountId,
                customerId, "", mySocketId, 0, UniqId, form.Id).Single;

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

            return  Task.FromResult<MyWebSocketResponse>(null).GetAwaiter().GetResult();
        }
    }
}