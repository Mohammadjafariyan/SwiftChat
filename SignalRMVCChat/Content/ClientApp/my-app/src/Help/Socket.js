import {_dispatcher} from './Dispatcher';
import {cookieManager} from './CookieManager';
import {MyGlobal} from './MyGlobal';
import {CurrentUserInfo} from "../CurrentUserInfo";
import $ from 'jquery'


export const MyCaller = {

    Send(name, data) {
        if (MyGlobal.isTestingEnvirement) {

            if (CurrentUserInfo.FakeServerMonitor) {


                CurrentUserInfo.FakeServerMonitor.ServerCall(name, data);

            }/*else{
                CurrentUserInfo.LayoutPage.showError('CurrentUserInfo.FakeServerMonitor is null:' + name);

            }*/

            return;
        }

        /*  if (CurrentUserInfo.ws.readyState != WebSocket.OPEN) {
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
          }*/


        var req = {};
        req.Name = name;
        req.Body = data;
        req.Token = cookieManager.getItem('adminToken');


        req.SelectedTagId = CurrentUserInfo.selectedTagId;
        req.gapIsOnlyOnly = CurrentUserInfo.gapIsOnlyOnly;

        req.IsAdminMode = CurrentUserInfo.currentUsersIsAdmins;

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
            let websiteToken = localStorage.getItem("mjChatWebPanelToken") ? localStorage.getItem("mjChatWebPanelToken") : document.getElementById('websiteToken').value;

            req.WebsiteToken = websiteToken;

            req.IsAdminOrCustomer = 1; // admin
            //debugger


            if (server)
                server.send(JSON.stringify(req));
            else {
                setTimeout(() => {
                    MyCaller.Send(name, data);
                }, 7000)
            }
        }


    }
}

/*
export const Socket = function (onOpen) {

    let baseUrl= document.getElementById('baseUrl') ? "wss://" +document.getElementById('baseUrl').value :  "ws://" +'localhost' ;
    CurrentUserInfo.ws = new WebSocket( baseUrl + ":8181/");
    CurrentUserInfo.ws.onopen = function () {
        console.log('اتصال برقرار شد');
        /!*alert("About to send data");
        ws.send("Hello World"); // I WANT TO SEND THIS MESSAGE TO THE SERVER!!!!!!!!
        alert("Message sent!");*!/ 


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
})*/

let server;
export const Socket = function (onOpen) {

    $(function () {
        // Reference the auto-generated proxy for the hub.  


        var chat = window["$"].connection.customerHub;
        // Create a function that the hub can call back to display messages.
        chat.client.onmessage = function (evt) {

            var data = JSON.parse(evt);

            console.log(evt);
            _dispatcher.dispatch(data);

        };

        CurrentUserInfo.ws = chat.server;
        server = chat.server;

        let baseUrl = document.getElementById('baseUrl').value !== '@baseUrl@' ? '' : 'http://localhost:60518'
        window["$"].connection.hub.url = baseUrl + window["$"].connection.hub.url

        window["$"].connection.hub.start().done(function () {

            console.log('اتصال برقرار شد');


            if (onOpen) {
                onOpen();


            }

        });

          window["$"].connection.hub.disconnected(function() {
              console.log('reconnecting in 5 seconds...');
              setTimeout(function() {
                  console.log('attempt to reconnect..');
  
                  window["$"].connection.hub.start();
              }, 5000); // Restart connection after 5 seconds.
          });

        window["$"].connection.hub.error(async function (error) {
            console.log('SignalrAdapter: ' + error);


            console.log(error);
            console.error("اتصال قطع شد");


        });


        window["$"].connection.hub.connectionSlow(function () {
            // notifyUserOfConnectionProblem(); // Your function to notify user.
        });

        var tryingToReconnect = false;

        window["$"].connection.hub.reconnecting(function () {
            tryingToReconnect = true;
        });

        window["$"].connection.hub.reconnected(function () {
            tryingToReconnect = false;
        });

        window["$"].connection.hub.disconnected(function () {
            if (tryingToReconnect) {
                //  notifyUserOfDisconnect(); // Your function to notify user.
            }
        });

    });


    /* CurrentUserInfo.ws = new WebSocket("wss://" + baseUrl + ":8181/");
     CurrentUserInfo.ws.onopen = function () {
         console.log('اتصال برقرار شد');
         /!*alert("About to send data");
         ws.send("Hello World"); // I WANT TO SEND THIS MESSAGE TO THE SERVER!!!!!!!!
         alert("Message sent!");*!/
 
 
         if (onOpen) {
             onOpen();
             if (CurrentUserInfo.dotColorBackup) {
                 getDoc().getElementById('dot').style.backgroundColor = CurrentUserInfo.dotColorBackup;
             }
 
         }
     };
 
     CurrentUserInfo.ws.onmessage = function (evt) {
         var received_msg = evt.data;
 
         console.log(evt);
         _dispatcher.dispatch(JSON.parse(received_msg));
     };
     CurrentUserInfo.ws.onclose = function (e) {
         // websocket is closed.
 
         console.log(e);
         console.error("اتصال قطع شد");
 
 
         //    socketConnect();
         //    CurrentUserInfo.dotColorBackup = getDoc().querySelector('#dot').style.backgroundColor;
         //    getDoc().querySelector('#dot').style.backgroundColor = 'grey';
     };*/
}

/*function tryConnect() {
    if (window["$"] && window["$"].connection && window["$"].connection.customerHub) {
        Socket(function () {
            console.log('opened !');
        })
        return;
    }

    setTimeout(() => {
        tryConnect();
    }, 5000)

}

tryConnect();*/

Socket(function () {
    console.log('opened !');
})
