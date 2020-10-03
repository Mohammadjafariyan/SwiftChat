


const Server=function(){
    
   
}

function randomArr(items){
    var item = items[Math.floor(Math.random() * items.length)];
return item;
}
function getRandomCityName(){

    let arr=['تبریز','تهران','شیراز'];
    return randomArr(arr);

}

function getRandomPersonName(){

    let arr=['محمد','علی','فاطمه'
        ,'قاسم','مسعود','سجاد','هاشمی','نفیسه','قربان','کاظم','جاسم','علیرضا'];
    return randomArr(arr)+" "+ randomArr(arr)+" زاده";

}

function getRandomRegionName(){
    let arr=['آذربایجان شرقی','خراسان','شیراز'];
    return randomArr(arr);
}


function getRandomPageTitle(){
    let arr=['صفحه اصلی','درباره ماه','ارتباط با ما در حوزله این چیز ها  و آن چیز ها'];
    return randomArr(arr);
}

Server.Send=function (req) {

    function GetOnlineAdmins() {
let arr=[];
        for (let i = 0; i < 30; i++) {
            arr.push({Name:'ادمین یک',LastTrackInfo:{
                    CityName:getRandomCityName(),
                    Region:getRandomRegionName(),
                    PageTitle:getRandomPageTitle(),
                },Id:i,OnlineStatus:i,TotalUnRead:i});
        }
        return arr;

    }

    if (req.Name=="Register"){
        var admins= GetOnlineAdmins();
        let res={};
        res.Content={};
        res.Content.EntityList=admins;
        res.Name='registerCallback';
        _dispatcher.dispatch(res)
    }

    if (req.Name=="CustomerSendToAdmin"){
        var admins= GetOnlineAdmins();
        let res={};
        res.Content={};
        res.Content.Chat={};
        res.Content.Chat.Message= req.Body.typedMessage;
        res.Name='msgDeliveredCallback';
        _dispatcher.dispatch(res)
    }


    function GetCustomerActivityDetailInfo() {
        let arr=[];
        for (let i = 0; i < 30; i++) {
            arr.push({CityName:getRandomCityName(),
                Region:getRandomRegionName(),
                PageTitle:getRandomPageTitle(),CustmerId:i});
        }
        return arr;
    }

    if (req.Name=="GetCustomerActivityDetail"){
        var data= GetCustomerActivityDetailInfo();
        let res={};
        res.Content=data;
        res.Temp={Name:'کاربر آنلاین'};
        res.Name='getCustomerActivityDetailCallback';
        _dispatcher.dispatch(res)
    }

    if (req.Name=="MultimediaPmSend"){

        let res={};
        res.Content={};
        res.Content=req.Body;
        res.Name='multimediaPmSendCallback';
        _dispatcher.dispatch(res)
    }
    

    function MakeDummyChats() {
        let arr=[];
        for (let i = 0; i < 30; i++) {
            if (i%2===0){
                arr.push(
                    {SenderType:randomArr([1,1,2,2])
                        ,Message:getRandomCityName(),
                    MultimediaContent:"http://www.google.com"});
            }else{
                arr.push({SenderType:randomArr([1,1,2,2]),
                    Message:getRandomCityName()});
            }
        }
        return arr;
    }

    if (req.Name=="ReadChat"){

        let data=MakeDummyChats();

        let res={};
        res.Content={};
        res.Content.EntityList=data;
        res.Name='readChatCallback';
        _dispatcher.dispatch(res)
    }

    if (req.Name=="LoadReadyPm"){

        let data=MakeDummyChats();

        let res={};
        res.Content={};
        res.Content.EntityList=data;
        res.Name='loadReadyPmCallback';
        _dispatcher.dispatch(res)
    }

    if (req.Name=="SearchHandler"){

        let res=MakeDummySearchResult();

        res.Name='searchHandlerCallback';
        _dispatcher.dispatch(res)
    }
    
    
    
};




MyCaller = {

    Send(name, data) {


        var req = {};
        req.Name = name;
        req.Body = data;
        req.Token = CurrentUserInfo.IsCustomer ? CurrentUserInfo.GetCurrentCustomerToken() : _currentAdminInfo.adminToken;
        req.WebsiteToken = websiteToken;


        req.IsAdminOrCustomer = CurrentUserInfo.IsCustomer ? 2 : 1;
     //   debugger

        
        Server.Send(req);
       


    }
}

configWebSocket=function(op){
    op();
}

startUp=function() {


    try {


        readTextFile("file:///E:/workplace/git/1399/3-xordad/online-support-hamrah-postiban/online-support-master/SignalRMVCChat/Areas/Customer/Views/JsPlugin/CustomerChatHtml.cshtml",
           function (html) {
               socketConnect(html)

           });




    }catch (e) {

        console.error(e);
        console.error('خطایی اتفاق افتاد');

        console.log('اتصال مجدد بعد از 20 ثانیه مکس')
        setTimeout(function () {

            console.log('تلاش برای برقراری اتصال مجدد')

            startUp();


        },1000*20);
    }

}
startUp();


function readTextFile(file,callback)
{
    var rawFile = new XMLHttpRequest();
    rawFile.open("GET", file, false);
    rawFile.onreadystatechange = function ()
    {
        if(rawFile.readyState === 4)
        {
            if(rawFile.status === 200 || rawFile.status == 0)
            {
                var allText = rawFile.responseText;
                callback(allText)
            }
        }
    }
    rawFile.send(null);
}


function MakeDummySearchResult() {


    let res={};
    res.Content={};

    res.Content.customerlist=[];
    res.Content.sendMsgList=[];
    res.Content.receiveMsgList=[];

    for (let i = 0; i < 3; i++) {
        let Customer={};
        Customer.Name=getRandomPersonName();
        Customer.Id=Math.random();
        res.Content.customerlist.push({Customer})
    }


    for (let i = 0; i < 3; i++) {
        let Customer={};
        Customer.Name=getRandomPersonName();
        Customer.CustomerId=Math.random();

        res.Content.sendMsgList.push({Customer,Message:getRandomCityName()})
    }

    for (let i = 0; i < 3; i++) {
        let Customer={};
        Customer.Name=getRandomPersonName();
        Customer.CustomerId=Math.random();

        res.Content.receiveMsgList.push({Customer,Message:getRandomCityName()})
    }
    return res;
}