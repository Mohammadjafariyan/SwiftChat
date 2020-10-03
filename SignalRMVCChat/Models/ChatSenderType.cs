namespace SignalRMVCChat.Service
{
    public enum ChatSenderType
     {
         CustomerToAccount=1,
         AccountToCustomer=2,
         AccountToAccount=3,
         SaveAsFastAnswering=4,
         SaveAsFastAnsweringForGroup=5,// وقتی میخواهد همه گروه بتوانند از این نوع پاسخگویی سریع استفاده کنند
     }
 }