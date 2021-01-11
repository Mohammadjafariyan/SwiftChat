import {_dispatcher} from './Dispatcher';
import {cookieManager} from './CookieManager';
import {MyGlobal} from './MyGlobal';
import {CurrentUserInfo} from "../CurrentUserInfo";

export const MyCaller = {

    Send(name, data) {
        if(MyGlobal.isTestingEnvirement){
            
            if (CurrentUserInfo.FakeServerMonitor){


                CurrentUserInfo.FakeServerMonitor.ServerCall(name,data);
                
            }/*else{
                CurrentUserInfo.LayoutPage.showError('CurrentUserInfo.FakeServerMonitor is null:' + name);

            }*/
            
return;
        }

        if (CurrentUserInfo.ws.readyState != WebSocket.OPEN) {
            CurrentUserInfo.LayoutPage.showError('در حال اتصال به سرور و ارسال درخواست');
            setTimeout(() => {

                    if (CurrentUserInfo.ws.readyState === WebSocket.CLOSED ||
                        CurrentUserInfo.ws.readyState === WebSocket.CLOSED) {

                        Socket(function() {
                            console.log('opened !');
                        })
                    }

                    CurrentUserInfo.LayoutPage.showError('در حال اتصال');
                    this.Send(name, data);
                },
                1000);

            return;
        } else {
            if (CurrentUserInfo.LayoutPage.state.err && 
                CurrentUserInfo.LayoutPage.state.err.indexOf('اتصال')>=0) {
                CurrentUserInfo.LayoutPage.showError(null);

            }
        }


        var req = {};
        req.Name = name;
        req.Body = data;
        req.Token = cookieManager.getItem('adminToken');

        
        req.SelectedTagId=CurrentUserInfo.selectedTagId;
        req.gapIsOnlyOnly = CurrentUserInfo.gapIsOnlyOnly;

        req.IsAdminMode=CurrentUserInfo.currentUsersIsAdmins;

        // req.WebsiteToken = DataHolder.WebsiteToken;
        if (MyGlobal.fakeWebsite) {
            let baseUrl = document.getElementById('baseUrl').value;
            let port = document.getElementById('port').value;


            fetch(`http://${baseUrl}:${port}/Home/GetWebsiteToken`)
                .then(response => response.json())
                .then(data => {
                    req.WebsiteToken = data;




                    req.IsAdminOrCustomer = 1; // admin
                    //debugger

                    CurrentUserInfo.ws.send(JSON.stringify(req));
                });

            return;
            /*req.WebsiteToken =
                "N09XVk1peG5Gc2FtQWhLSHk4MjIrZ2xMeVN2T0VoR1JaOThRY3BHbnJwYXBORHpBTDZ0Zjlpb084Tkd6dWFpYlRrcWZVQjdtRnV2LzdNL3ozVkM3cGc9PQ==";
*/


        } else {
            let websiteToken = document.getElementById('websiteToken').value;

            req.WebsiteToken = websiteToken;

            req.IsAdminOrCustomer = 1; // admin
            //debugger

            CurrentUserInfo.ws.send(JSON.stringify(req));
        }

        
        


    }
}


export const Socket = function (onOpen) {

    let baseUrl=document.getElementById('baseUrl').value;
    CurrentUserInfo.ws = new WebSocket("ws://" + baseUrl + ":8181/");
    CurrentUserInfo.ws.onopen = function () {
        console.log('اتصال برقرار شد');
        /*alert("About to send data");
        ws.send("Hello World"); // I WANT TO SEND THIS MESSAGE TO THE SERVER!!!!!!!!
        alert("Message sent!");*/


        if (onOpen) {
            onOpen();

        }
    };

    CurrentUserInfo.ws.onmessage = function (evt) {

        if (!process.env.NODE_ENV || process.env.NODE_ENV === 'development') {
            console.log(evt);
        } else {
            // production code
        }
        var received_msg = evt.data;

        _dispatcher.dispatch(JSON.parse(received_msg));
    };
    CurrentUserInfo.ws.onclose = function () {
        // websocket is closed.
        console.error("اتصال قطع شد");

    };
}


Socket(function() {
    console.log('opened !');
})