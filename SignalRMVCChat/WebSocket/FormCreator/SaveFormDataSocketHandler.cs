using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket.FormCreator
{
    public class SaveFormDataSocketHandler : BaseFormCreatorSocketHandler
    {
        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            //=============================================================================
            _logService.LogFunc(request);
            //=============================================================================

            _request = MyWebSocketRequest.Deserialize(request);

            if (_request == null)
            {
                Throw("درخواست نال است");
            }

            //=============================================================================
            _logService.LogFunc("SaveFormDataSocketHandler");
            //=============================================================================
            // await base.ExecuteAsync(request, currMySocketReq);


            #region validation

            if (currMySocketReq.IsAdminOrCustomer != (int)MySocketUserType.Customer)
            {
                Throw("این عملیات فقط برای بازدید کننده ها مجاز است");
            }

            if (currMySocketReq.ChatConnection.CustomerId.HasValue == false)
            {
                Throw("بازدیدکننده ارسال نشده است");
            }


            #region form

            var formIdstr = _request?.Body?.formId?.ToString();

            int formId = 0;
            bool isParsed = int.TryParse(formIdstr?.ToString(), out formId);
            if (!isParsed)
            {
                Throw("کد فرم باشد ارسال نشده است");
            }

            //=============================================================================
            _logService.LogFunc("_request.Body?.formId:" + _request.Body?.formId);
            //=============================================================================

            var form = FormService.GetById(formId, "فرم یافت نشد").Single;

            #endregion

            #region FormElementsData

            var elements = _request.Body?.elements;

            //=============================================================================
            _logService.LogFunc("elements" + elements);
            //=============================================================================

            /*id,name,value*/
            List<FormElement> formElements = GetElementsFromJson(elements);


            if (formElements == null || formElements?.Count == 0)
            {
                Throw("المت های فرم ها خالی است");
            }

            if (formElements.Any(f => f.Id == 0) == true)
            {
                Throw("المنت ها کد ندارند");
            }

            /*var existCount = FormElementService.GetQuery()
                .ToList()
                .Count(c => formElements.Select(f => f.Id).Contains(c.Id));

            if (existCount != formElements?.Count)
            {
                Throw("المنت های ارسال شده درست نیست ");
            }*/


            //=============================================================================
            _logService.LogFunc("لود المنت های  فرم ");
            var formAllelements = FormElementService.GetQuery().Where(f => f.FormId == formId).ToList();

            _logService.LogFunc("لود المنت های  فرم " + formAllelements.Count);
            //=============================================================================


            if (formAllelements.Any(f => !formElements.Select(fe => fe.FieldName).Contains(f.FieldName)))
            {
                Throw("همه المنت های فرم ارسال نشده است ");
            }

            #endregion

            #endregion


            //=============================================================================
            _logService.LogFunc("GET CHAT ");
            //=============================================================================

            int chatId = GetParam<int>("chatId", true, "کد کاربر یافت نشد");


            var ChatProviderService = Injector.Inject<ChatProviderService>();
            var chat = ChatProviderService.GetById(chatId, "چت یافت نشد").Single;

            if (chat.MyAccountId.HasValue == false)
            {
                Throw("چت کد ادمین ندارد  ");
            }


            //=============================================================================
            _logService.LogFunc("ذخیره علامت ثبت شدن فرم در چت ");
            //=============================================================================

            chat.FormPassed = form.AfterMessage;
            ChatProviderService.Save(chat);

            #region Bussiness = save values

            //=============================================================================
            _logService.LogFunc("CUSTOMER SEND CHAT FORM DATA TO ADMIN ");
            //=============================================================================

            var newChatId = ChatProviderService.CustomerSendToAdmin(chat.MyAccountId.Value,
                currMySocketReq.ChatConnection.CustomerId.Value, "", currMySocketReq.ChatConnection.Id,
                0,


                ChatProviderService.GetQuery().Count(c => c.CustomerId == currMySocketReq.ChatConnection.CustomerId.Value
                                                          && c.MyAccountId == chat.MyAccountId.Value) + 1


                , formId).Single;



            //=============================================================================
            _logService.LogFunc("SAVE FormValues ");
            //=============================================================================

            List<FormValue> FormValues = new List<FormValue>();
            foreach (var formElement in formElements)
            {
                FormValues.Add(new FormValue
                {
                    FormId = formId,
                    CustomerId = currMySocketReq.ChatConnection.CustomerId.Value,
                    FormElementId = formElement.Id,
                    Value = formElement.Value,
                    ChatId = newChatId
                });
            }


            FormValueService.Save(FormValues);

            //=============================================================================
            _logService.LogFunc("چک کن و اگر از اطلاعات کاربر بود ذخیره کن");
            //=============================================================================

            CheckForUserInfoAndSet(formElements, currMySocketReq, _request);


            //=============================================================================
            _logService.LogFunc("GENERATE RESPONSE ");
            //=============================================================================


            var response = new MyWebSocketResponse
            {
                Name = "saveFormDataCallback",
                Content = new
                {
                    form.AfterMessage,
                    formId,
                    chatId,
                    chat.UniqId

                }
            };


            //=============================================================================
            _logService.LogFunc("GENERATE RESPONSE FOR ADMIN WITH FORM AND ITS DATA ");
            //=============================================================================

            FormValues = new List<FormValue>();
            foreach (var formElement in formElements)
            {
                FormValues.Add(new FormValue
                {
                    FormId = formId,
                    CustomerId = currMySocketReq.ChatConnection.CustomerId.Value,
                    FormElementId = formElement.Id,
                    Value = formElement.Value,
                    FormElement = formElement,
                    ChatId = newChatId
                });
            }



            //=============================================================================
            _logService.LogFunc("GET formElements FOR ALL ARGS LIKE TYPE AND ECT.. ");
            //=============================================================================


            formElements = FormElementService.GetQuery().Where(c => c.FormId == formId).ToList();


            //=============================================================================
            _logService.LogFunc("SET formElements VALUES FOR SHIWING IN UI");
            //=============================================================================

            foreach (var formValue in FormValues)
            {
                var i = formElements.FindIndex(f => f.Id == formValue.FormElementId);
                formElements[i].Value = formValue.Value;
            }


            var adminresponse = new MyWebSocketResponse
            {
                Name = "saveFormDataCallback",
                Content = new
                {
                    form.AfterMessage,
                    formId,
                    chatId,
                    FormValues,
                    formElements,
                    formName = form.Name,
                    chat.UniqId

                }
            };
            await MySocketManagerService.SendToAdmin(chat.MyAccountId.Value, currMySocketReq.MyWebsite.Id, adminresponse);


            return response;

            #endregion
        }

        private void CheckForUserInfoAndSet(List<FormElement> formElements, MyWebSocketRequest currMySocketReq, MyWebSocketRequest request)
        {
            var customerProviderService = Injector.Inject<CustomerProviderService>();
            var customer = customerProviderService.GetById(currMySocketReq.ChatConnection.CustomerId.Value).Single;
            foreach (var formElement in formElements)
            {
                if (CheckContains(formElement, "ایمیل") == true ||
                    CheckContains(formElement, "email") == true ||
                    CheckContains(formElement, "gmail") == true)
                {
                    if (string.IsNullOrEmpty(customer.Email))
                    {
                        customer.Email = formElement.Value;
                    }
                }


                if (CheckContains(formElement, "phone") == true ||
                  CheckContains(formElement, "شماره") == true ||
                  CheckContains(formElement, "mobile") == true ||
                  CheckContains(formElement, "تماس") == true ||
                  CheckContains(formElement, "تلفن") == true ||
                  CheckContains(formElement, "telephone") == true ||
                  CheckContains(formElement, "موبایل") == true)
                {
                    if (string.IsNullOrEmpty(customer.Phone))
                    {
                        customer.Phone = formElement.Value;
                    }
                }


                if (CheckContains(formElement, "phone") == true ||
                CheckContains(formElement, "شماره") == true ||
                CheckContains(formElement, "mobile") == true ||
                CheckContains(formElement, "تماس") == true ||
                CheckContains(formElement, "تلفن") == true ||
                CheckContains(formElement, "telephone") == true ||
                CheckContains(formElement, "موبایل") == true)
                {
                    if (string.IsNullOrEmpty(customer.Phone))
                    {
                        customer.Phone = formElement.Value;
                    }
                }


                if (CheckContains(formElement, "company") == true ||
             CheckContains(formElement, "شرکت") == true ||
             CheckContains(formElement, "دفتر") == true ||
             CheckContains(formElement, "office") == true)
                {
                    if (string.IsNullOrEmpty(customer.CompanyName))
                    {
                        customer.CompanyName = formElement.Value;
                    }
                }

                if (CheckContains(formElement, "job") == true ||
          CheckContains(formElement, "work") == true ||
          CheckContains(formElement, "شغل") == true ||
          CheckContains(formElement, "صنعت") == true ||
          CheckContains(formElement, "کار") == true)
                {
                    if (string.IsNullOrEmpty(customer.JobTitle))
                    {
                        customer.JobTitle = formElement.Value;
                    }
                }

            

                if (CheckContains(formElement, "address") == true ||
       CheckContains(formElement, "آدرس") == true ||
       CheckContains(formElement, "ادرس") == true)
                {
                    if (string.IsNullOrEmpty(customer.Address))
                    {
                        customer.Address = formElement.Value;
                    }
                }

                //public string Address { get; set; }
            }



            customerProviderService.Save(customer);
        }

        private bool CheckContains(FormElement formElement, string name)
        {
            if (formElement?.Name?.Contains(name) == true)
            {
                return true;
            }
            if (formElement?.FieldName?.Contains(name) == true)
            {
                return true;
            }
            return false;
        }
    }
}
