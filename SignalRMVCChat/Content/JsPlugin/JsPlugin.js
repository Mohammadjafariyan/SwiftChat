
var ADP = function () { var e = function (e) { return !e.classList.contains("adp-hide") }, n = function (e) { e && e() }, i = function (i, t, a) { if (e(i)) n(a); else { var d = function () { i.classList.remove(t + "-adp-show"), n(a), i.removeEventListener(s, d) }; i.addEventListener(s, d), i.classList.remove("adp-hide"), i.classList.add(t + "-adp-show") } }, t = function (i, t, a) { if (e(i)) { var d = function () { i.classList.add("adp-hide"), i.classList.remove(t + "-adp-hide"), n(a), i.removeEventListener(s, d) }; i.addEventListener(s, d), i.classList.add(t + "-adp-hide") } else n(a) }, s = function () { var e = document.createElement("div"); return e.style.webkitAnimation ? "webkitAnimationEnd" : e.style.mozAnimation ? "mozAnimationEnd" : "animationend" }(); return { show: i, hide: t, toggle: function (n, s, a) { e(n) ? t(n, s, a) : i(n, s, a) } } }();

var url = '@@@';
var baseUrl = "#baseUrl#";
var token = "#token#";
var baseUrlForapi = "#baseUrlForapi#";
var websiteToken = "#websiteToken#";
var debugMode = true;
let shadow;

/*=========================================== live assist ==================================================*/

function traverse(eventData, seenObjects, currentDepth, maxDepth) {
    if (currentDepth >= maxDepth) return '[object Object]';

    const json = {};

    for (let key in eventData) {
        const value = eventData[key];

        switch (Object.prototype.toString.call(value)) {
            case '[object String]':
            case '[object Number]':
            case '[object Boolean]':
                json[key] = value;
                break;

            case '[object Null]':
                json[key] = null;
                break;

            case '[object Function]':
                json[key] = '[Function: ' + (value.name || 'anonymous') + ']';
                break;

            case '[object Object]':
            case '[object Array]':
            case '[object HTMLCollection]':
            case '[object HTMLDocument]':
            case '[object HTMLBodyElement]':
            case '[object HTMLInputElement]':
            case '[object HTMLDivElement]':
            case '[object NamedNodeMap]':
            case '[object NodeList]':
            case '[object DOMTokenList]':
            case '[object DOMStringMap]':
            case '[object CSSStyleDeclaration]':
            case '[object ValidityState]':
            case '[object Arguments]':
                if (seenObjects.indexOf(value) >= 0) {
                    json[key] = '[object Circular]';
                } else {
                    seenObjects.push(value);
                    json[key] = traverse(
                        value,
                        seenObjects,
                        currentDepth + 1,
                        maxDepth
                    );
                }
                break;
            case '[object Symbol]':
                json[key] = '[object Symbol]';
                break;
            default:
                json[key] = '[unhandled typeof]';
        }
    }

    return json;
}

const parse = (eventData, depth) => {
    const maxDepth = depth || 4;
    const seenObjects = [eventData]; // start with itself

    return traverse(eventData, seenObjects, 0, maxDepth);
};

class SenderLiveAssistService {


    html;

    liveAssistRequestCallback(RES) {

        if (!RES.Content.MyAccountId) {
            alert('کد ادمین صحیح ارسال نشده است')
            return;
        }

        let html = document.querySelector('html').outerHTML;

        let base64 = btoa(unescape(encodeURIComponent(html)));

        this.html = html;

        var width = getWidth();

        var height = getHeight();

        CurrentUserInfo.MyAccountId = RES.Content.MyAccountId;
        MyCaller.Send("LiveAssistSendDoc", {
            MyAccountId: RES.Content.MyAccountId, htmlbase64: base64,
            width: width, height: height
        });


        this.ConfigUserBehaviour();

    }


    fireEvent(type, e) {
        let ev = getMousePos(e);


        this.firedEvents = e;

        let scroll = this.getScroll();

        MyCaller.Send("LiveAssistFireEvent", {
            MyAccountId: CurrentUserInfo.MyAccountId, x: ev.x, y: ev.y, event: JSON.stringify(parse(e)),
            type: type,
            sx: scroll.sx,
            sy: scroll.sy


        });
    }

    getScroll() {
        if (window.pageYOffset != undefined) {
            return {sx: pageXOffset, sy: pageYOffset};
        } else {
            var sx, sy, d = document,
                r = d.documentElement,
                b = d.body;
            sx = r.scrollLeft || b.scrollLeft || 0;
            sy = r.scrollTop || b.scrollTop || 0;
            return {sx, sy};
        }
    }

    ConfigUserBehaviour() {

        let html = document.querySelector('html');

        html.addEventListener('mousemove', (e) => {


            this.fireEvent('mousemove', e)
        })

        html.addEventListener('click', (e) => {
            this.fireEvent('click', e)

        })

        window.addEventListener('scroll', (e) => {
            this.fireEvent('scroll', e)

        })

        html.addEventListener('keydown', (e) => {
            this.fireEvent('keydown', e)

        })

        html.addEventListener('focus', (e) => {
            this.fireEvent('focus', e)

        })


    }
}

class ReceiverLiveAssistService {

    html

    isValid(data, name) {
        if (!data || !data.Content || !data.Content[name]) {
            console.error('data is null : LiveAssistSendDocCallback')
            return false;
        }

        return true;
    }

    LiveAssistSendDocCallback(data) {
        if (!this.isValid(data, 'htmlbase64')) {
            return;
        }


        let html = atob(data.Content.htmlbase64);

        this.html = html;

    }

    getMagicBrowser() {
        return document;
    }

    LiveAssistFireEventCallback(data) {
        if (!this.isValid(data, 'Event')) {
            return;
        }
        let _event = JSON.parse(data.Content.Event);

        this.firedEvents = _event;

        if (!this._LiveAssistShower) {

            this._LiveAssistShower = new LiveAssistShower();

        }

        if (data.Content.sx != null && data.Content.sy != null) {

            window.scrollTo(data.Content.sx, data.Content.sy)
        }

        this._LiveAssistShower.fireMouseEventAndReRender(data.Content)


        if (data.Content.type === 'click') {

            const x = data.Content.x;
            const y = data.Content.y;

            let spinner = getDoc().querySelector('.pulsating-circle');

            spinner.style.left = `${x}px`;
            spinner.style.top = `${y}px`;

            spinner.style.display = 'block';

            if (!this.isTimeOutSet) {
                this.isTimeOutSet = true;

                setTimeout(() => {
                    spinner.style.display = 'none';
                    this.isTimeOutSet = false;
                }, 1000);
            }


        }


        //   this.getMagicBrowser().dispatchEvent(_event)


        /*  switch (data.Content.type){
              case 'mousemove':
  
                  break;
              case 'click':
                  break;
              case 'wheel':
                  break;
              case 'keydown':
                  break;
          }*/


    }

}

function getMousePos(e) {
    return {
        x: e.clientX + document.body.scrollLeft,
        y: e.clientY + document.body.scrollTop
    };
}

class LiveAssistShower {
    el = getDoc().querySelector('.mouse');

    fireMouseEventAndReRender(mouseEvent) {

        this.mouseEvent = mouseEvent;

        // Ask the browser to call render to start our animation
        requestAnimationFrame((a) => {
            this.render(a)
        });

    }

    getMousePos(e) {
        return {
            x: e.clientX + document.body.scrollLeft,
            y: e.clientY + document.body.scrollTop
        }
    }

    // The render function is called on every frame
    render(a) {
        // The a variable is the amount of milliseconds since we started our script

        // Get a noise value based on the elapsed time
        // This noise algorithm is returning values between [-1, 1] so we need to map them to [0, 1] by adding one to the value and dividing it by 2
        //  const noiseX = (noise.simplex2(0, a*0.0005) + 1) / 2;
        // We get another noise value for the y axis but because we don't want the same value than x, we need to use another value for the first parameter
        //  const noiseY = (noise.simplex2(1, a*0.0005) + 1) / 2;

        // Convert the noise values from [0, 1] to the size of the window


        const x = this.mouseEvent.x;// this.getMousePos(this.mouseEvent).x;//noiseX * window.innerWidth;
        const y = this.mouseEvent.y;//this.getMousePos(this.mouseEvent).y;//noiseY * window.innerHeight;

        // Apply the x & y coordinates on our element
        this.el.style.transform = `translate(${x}px, ${y}px)`;
        this.el.style.display = null;

        // Call the render function once the browser is ready to make it an infinite loop
    }

}


let _SenderLiveAssistService = new SenderLiveAssistService();
let _ReceiverLiveAssistService = new ReceiverLiveAssistService();


/*===========================================  END ==================================================*/

/*=========================================== event Trigger ==================================================*/

function detectMob() {
    return ((window.innerWidth <= 800) && (window.innerHeight <= 600));
}

class EventTriggerManager {
    eventTriggerBeans = [];
    isInited = false;

    eventTriggers = [];

    isInitialized() {
        return this.isInited;
    }

    getEventTriggers() {
        MyCaller.Send('GetEventTriggers')
    }

    getEventTriggersCallback(res) {
        if (!res || !res.Content || !res.Content) {
            alert('دیتای بازگشتی نال است')
        }

        this.isInited = true;

        this.eventTriggers = [];

        this.eventTriggerBeans = res.Content;

        this.Execute();


    }


    Execute() {
        if (!this.eventTriggerBeans || !this.eventTriggerBeans.length)
            return;


        for (let i = 0; i < this.eventTriggerBeans.length; i++) {
            this.eventTriggers.push(new EventTrigger(this.eventTriggerBeans[i]))
        }

        for (let i = 0; i < this.eventTriggerBeans.length; i++) {
            this.eventTriggers[i].ConfigEvents();
        }


    }


}


class EventTrigger {
    bean;

    //for testing purposes
    firedEvent;

    constructor(bean) {
        this.bean = bean;
    }

    tryCatch(callback) {

        try {
            callback();
        } catch (e) {
            console.error(e);
        }
    }

    ConfigEvents() {

        if (!this.bean.RunInMobileDevices && detectMob()) {
            return;

        }

        if (!this.bean.RunInDesktopDevices && !detectMob()) {
            return;
        }


        this.tryCatch(() => {
            this.configOnExitTab();

        })
        this.tryCatch(() => {
            this.configClickOnLink();

        })

        this.tryCatch(() => {
            this.configOnQueryParameters();

        })

        this.tryCatch(() => {
            this.configOnSpecificPages();

        })

        this.tryCatch(() => {
            this.configUserCustomEvents();

        })

    }

    /*-------------------------private configs:*/

    configOnExitTab() {
        if (!this.bean || !this.bean.EventOnExitTab)
            return;

        const parent = this;

        function addEvent(obj, evt, fn) {
            if (obj.addEventListener) {
                obj.addEventListener(evt, fn, false);
            } else if (obj.attachEvent) {
                obj.attachEvent("on" + evt, fn);
            }
        }

        addEvent(document, "mouseleave", function (e) {
            e = e ? e : window.event;
            var from = e.relatedTarget || e.toElement;
            if (!from || from.nodeName == "HTML") {

                parent.fireEvent('beforeunload');

                //.... do_this
            }
        });
        /*   window.onbeforeunload = onExit;
   
   
           function onExit(event) {
   
                let message=parent.bean.localizedMessages[0].textArea;
               var e = event || window.event;
               if (e) {
                   e.returnValue =message;
                   e.preventDefault();
               }
               setTimeout(()=>{
                   parent.fireEvent('beforeunload');
               },100);
   
   
   return message;
           }*/

        /*   window.addEventListener('beforeunload', () => {
   
   
               this.fireEvent('beforeunload');
   
           })*/


    }

    configClickOnLink() {

        if (!this.bean || !this.bean.EventOnLinkClick)
            return;

        if (!this.bean.links || !this.bean.links.length) {
            alert('لینک های تعریف شده خالی هستند و یا نال است');
            return;
        }


        // links defined in admin side
        for (let i = 0; i < this.bean.links.length; i++) {

            let selector = this.bean.links[i].Name;


            // select in dom
            let findedLinks = document.querySelectorAll(selector);

            if (!findedLinks)
                continue;

            for (let j = 0; j < findedLinks.length; j++) {
                findedLinks[j].addEventListener('click', () => {

                    this.fireEvent('click ' + findedLinks[j].innerText);

                })
            }

        }


    }


    configOnSpecificPages() {

        if (!this.bean || !this.bean.EventSpecificPages)
            return;

        if (!this.bean.pages || !this.bean.pages.length) {
            alert('پارامتر های تعریف شده خالی هستند و یا نال است');
            return;
        }


        for (let i = 0; i < this.bean.pages.length; i++) {


            ///DEFINED PARAMETERS
            let name = this.bean.pages[i].Name;


            let tupple = getSplitted(name)

            let splitted = tupple.splitted;
            name = tupple.name;

            var currentURL = window.location.href,
                // create a regular expression that will match all pages under user
                usersPattern = new RegExp(name);


            let URLParts = getSplitted(currentURL).splitted

            let notEqualAny = false;
            if (URLParts && URLParts.length > 0) {
                for (let j = 0; j < URLParts.length; j++) {


                    if (j >= splitted.length) {
                        continue;
                    }

                    name = splitted[j];
                    name = name.split("**").join("NNNNNNNNNNNNNNNNNNNNNNNNNNNN")
                    name = name.split("*").join("MMMMMMMMMMMMMM")

                    name = name.split("NNNNNNNNNNNNNNNNNNNNNNNNNNNN").join(".*")
                    name = name.split("MMMMMMMMMMMMMM").join(".*")


                    if (name.indexOf("!") >= 0) {

                        let notEqualpart = name.replace('!', '');

                        if (notEqualpart === URLParts[j]) {
                            notEqualAny = true;

                            break;
                        }

                        continue;

                    }

                    var pattern = new RegExp(name);
                    if (pattern.test(URLParts[j])) {
                    } else {
                        notEqualAny = true;
                        break;

                    }

                }
            }

            if (!notEqualAny) {
                this.firedEvent = "configOnSpecificPagesFired";
                this.fireEvent('configOnSpecificPages');
                break;

            } else {

            }

        }


    }

    configOnQueryParameters() {

        if (!this.bean || !this.bean.EventAddressParameters)
            return;

        if (!this.bean.pageParameters || !this.bean.pageParameters.length) {
            alert('پارامتر های تعریف شده خالی هستند و یا نال است');
            return;
        }


        const fire = () => {


            for (let i = 0; i < this.bean.pageParameters.length; i++) {


                ///DEFINED PARAMETERS
                let name = this.bean.pageParameters[i].Name;
                let secondName = this.bean.pageParameters[i].SecondName;


                //GET VLAUE FROM QUERY STRING
                let value = getParameterByName(name);

                if (value == null)
                    continue;

                // IF EQUALS FIRE
                if (value === secondName) {

                    this.firedEvent = "configOnQueryParametersFired";
                    this.fireEvent('configOnQueryParametersFired');
                }

            }

        }

        //call and check if any exist
        fire();

        RegisterOnUrlChange(() => {
            fire();
        })


    }

    configUserCustomEvents() {
        if (!this.bean || !this.bean.EventUserCustomName)
            return;

        if (!this.bean.userEventNames || !this.bean.userEventNames.length) {
            alert('پارامتر های تعریف شده خالی هستند و یا نال است');
            return;
        }
        for (let i = 0; i < this.bean.userEventNames.length; i++) {


            window.addEventListener(this.bean.userEventNames[i], () => {

                this.fireEvent('configUserCustomEvents');

            })


        }


    }


    /*-------------------------private actions:*/

    Action() {

        if (this.bean.IsOpenChatBox) {

            this.openChat();

        }

        if (this.bean.IsShowMessageEnabled) {

            // this.showMessage();
        }


        if (this.bean.IsPlayASound) {
            this.playSound();

        }


    }


    showMessage() {
        if (!this.bean.localizedMessages || !this.bean.localizedMessages.length) {
            return;
        }


        for (let i = 0; i < this.bean.localizedMessages.length; i++) {

            // IF NOT CHAT OPEN , SHOW ON THE FLY
            /* if (this.bean.IsOpenChatBox) {
                 showNewOnTheFlyMessage(this.bean.localizedMessages[i].textArea)
             } else {
 
 
                 let res = {
                     Content: {
                         Message: this.bean.localizedMessages[i].textArea
                     }
                 };
 
                 CurrentUserInfo.plugin.adminSendToCustomerCallback(res, false);
 
             }*/

        }


        if (getDoc().querySelector('#dot').style.display !== 'none') {
            getDoc().querySelector('#dot').click();
        }
    }

    openChat() {

        if (getDoc().querySelector('#dot').style.display !== 'none') {
            getDoc().querySelector('#dot').click();
        }
    }

    playSound() {

        var audio = new Audio(`${baseUrlForapi}/Content/music/alert.mp3`);
        audio.play();

    }


    /*-------------------------fire Event:*/

    fireEvent(name) {


        /*fire once*/
        if (!this.firedEvents) {
            this.firedEvents = [];
        }
        if (!this.firedEvents.find(f => f === name)) {
            this.firedEvents.push(name)
        } else {
            return;
        }
        /*END*/


        this.Action();

        debugger;

        MyCaller.Send('EventFired', {name: name, id: this.bean.Id})

    }

    eventFiredSaveCallback(res) {

    }

    /*-------------------------device:*/


    isMobileOrDesktop() {

    }

}


function RegisterOnUrlChange(callback) {

    (function (history) {
        const pushState = history.pushState;
        history.pushState = function (state) {
            if (typeof history.onpushstate == "function") {
                history.onpushstate({state: state});
            }
            callback();
            return pushState.apply(history, arguments);
        }
    })(window.history);
}

function getParameterByName(name, url = window.location.href) {
    name = name.replace(/[\[\]]/g, '\\$&');
    var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, ' '));
}

function getSplitted(s) {
    let url = s.toString();

    url = url.replace('http://', '')
    url = url.replace('https://', '')

    let name = url;
    let splitted = url.split('/')

    return {splitted, name};
}

const _EventTriggerManager = new EventTriggerManager();
/*=========================================== END ==================================================*/

let Logger = function (msg) {
    try {
        xhttp = new XMLHttpRequest();


        /// درخواست کد html دایره ای شکل در معلق
        xhttp.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {

            }

        };

        // درخواست html ها
        xhttp.open("POST", baseUrlForapi + "/log/log", true);
        xhttp.send(JSON.stringify({log: msg}));
    } catch (e) {
        console.error(e);
        console.error('عدم امکان ارسال لاگ سیستم');

    }
}

class PushManager {

    push(header, msg) {
        if (Push && header && msg) {

            if (msg.length > 50) {
                msg = msg.substring(0, 50) + '...';
            }

            Push.create(header, {
                body: msg,
                icon: '/icon.png',
                timeout: 4000,
                onClick: function () {
                    window.focus();
                    this.close();
                }
            });
        }

    }
}

const pushManager = new PushManager();

if (!typeof (showError)) {
    showError = function () {

    }
}

function getDoc() {
    return shadow;
}

document.body.addEventListener("mouseover", function () {
    let isOnline = false;
    let doc = getDoc();
    if (doc) {
        let element = doc.querySelector('#gapCurPersonStatus');
        if (element) {
            if (element.style.backgroundColor == 'green') {
                isOnline = true;
            } else {
                isOnline = false;

            }
            if (!isOnline) {
                let sendingArr = getDoc().querySelectorAll('.gapSending');
                if (sendingArr) {
                    for (var i = 0; i < sendingArr.length; i++) {
                        sendingArr[i].innerText = 'کاربر آفلاین است';
                    }
                }

            }
        }
    }


});


function socketConnect(responseText) {

    // بعد از اتصال به وب سوکت ، نمایش می دهد
    configWebSocket(function () {


        var gapPlugin = document.getElementById("gapPlugin");
        if (gapPlugin) {

            CurrentUserInfo.IsCustomer = true;
        } else {
            gapPlugin = document.getElementById("gapPluginAdmin");
        }

        if (!shadow) {

            shadow = gapPlugin.attachShadow({mode: 'open'});
        }


        // در دوجا استفاده شده onClose
        if (responseText) {
            let div = document.createElement('div');
            div.innerHTML = responseText;
            shadow.appendChild(div);
        }

        //gapPlugin.innerHTML = ;
        changeDotPic();


        setTimeout(function () {


            if (CurrentUserInfo.IsCustomer) {


                dragElement(getDoc().getElementById("onTheFly"));
                CurrentUserInfo.plugin = new CustomerPlugin();
                CurrentUserInfo.plugin.bind(getDoc().getElementById("onTheFly"))

                CurrentUserInfo.plugin.Register();

            } else {
                CurrentUserInfo.plugin = new AdminPlugin();
                CurrentUserInfo.plugin.bind(getDoc().getElementById("onTheFly"))

            }

            if (!document.body) {
                alert('صفحه دارای تگ body نیست لذا فونت ها لود نمی شوند')
                return;
            }

            if (!document.body.querySelector('#gap_fonts')) {
                document.body.prepend(getDoc().querySelector('#gap_fonts'));

            }
            // CurrentUserInfo.plugin.Register()
            // اطلاعات کاربر انتخاب شده را برمیگرداند در صورت انتخاب 
            MyCaller.Send('GetSelectedAdmin');

            //  getDoc().querySelector('#dot').click();
            if (CurrentUserInfo.IsRestart) {
                CurrentUserInfo.IsRestart = false;
            }


        }, 1000)
    });


}

// همه چیز از اینجا شروع می شود
let xhttp;
let startUp = function () {


    try {
        xhttp = new XMLHttpRequest();


        /// درخواست کد html دایره ای شکل در معلق
        xhttp.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {


                // پاسخ html از سرور
                socketConnect(this.responseText)

            }

        };

        // درخواست html ها
        xhttp.open("GET", url, true);
        xhttp.send();
    } catch (e) {

        Logger(e);

        console.error(e);
        console.error('خطایی اتفاق افتاد');

        console.log('اتصال مجدد بعد از 20 ثانیه مکس')
        setTimeout(function () {

            console.log('تلاش برای برقراری اتصال مجدد')

            startUp();


        }, 1000 * 20);
    }

}


startUp();
/*bind*/

const cookieManager = {
    getItem: function (sKey) {
        return decodeURIComponent(document.cookie.replace(new RegExp("(?:(?:^|.*;)\\s*" + encodeURIComponent(sKey).replace(/[\-\.\+\*]/g, "\\$&") + "\\s*\\=\\s*([^;]*).*$)|^.*$"), "$1")) || null;
    },
    setItem: function (sKey, sValue, vEnd, sPath, sDomain, bSecure) {
        if (!sKey || /^(?:expires|max\-age|path|domain|secure)$/i.test(sKey)) {
            return false;
        }
        var sExpires = "";
        if (vEnd) {
            switch (vEnd.constructor) {
                case Number:
                    sExpires = vEnd === Infinity ? ";SameSite=None ; expires=Fri, 31 Dec 9999 23:59:59 GMT" : "; max-age=" + vEnd;
                    break;
                case String:
                    sExpires = "; expires=" + vEnd;
                    break;
                case Date:
                    sExpires = "; expires=" + vEnd.toUTCString();
                    break;
            }
        }
        document.cookie = encodeURIComponent(sKey) + "=" + encodeURIComponent(sValue) + sExpires + (sDomain ? "; domain=" + sDomain : "") + (sPath ? "; path=" + sPath : "") + (bSecure ? "; secure" : "");
        return true;
    },
    removeItem: function (sKey, sPath, sDomain) {

        document.cookie = encodeURIComponent(sKey) + "=; expires=Thu, 01 Jan 1970 00:00:00 GMT" + (sDomain ? "; domain=" + sDomain : "") + (sPath ? "; path=" + sPath : "");
        return true;
    },
    hasItem: function (sKey) {
        return (new RegExp("(?:^|;\\s*)" + encodeURIComponent(sKey).replace(/[\-\.\+\*]/g, "\\$&") + "\\s*\\=")).test(getDoc().cookie);
    },
    keys: /* optional method: you can safely remove it! */ function () {
        var aKeys = getDoc().cookie.replace(/((?:^|\s*;)[^\=]+)(?=;|$)|^\s*|\s*(?:\=[^;]*)?(?:\1|$)/g, "").split(/\s*(?:\=[^;]*)?;\s*/);
        for (var nIdx = 0; nIdx < aKeys.length; nIdx++) {
            aKeys[nIdx] = decodeURIComponent(aKeys[nIdx]);
        }
        return aKeys;
    }
};

function removeShowLoadingChats() {
    let gapShowLoadingChats = getDoc().querySelector('#gapShowLoadingChats');
    if (gapShowLoadingChats) {
        gapShowLoadingChats.remove();
    }
}

function showLoadingChats(target) {
    /*   let html = `
   <div  id="gapShowLoadingChats" style="
       position: absolute;
       right: 30%;
       color:black;
   ">
   <span >در حال خواندن اطلاعات</span>                    
                   </div>`;
   
       removeShowLoadingChats();
   
       getDoc().querySelector(target).append(createElementFromHTML(html));*/

    window.gapMsg("در حال خواندن اطلاعات")
}

let previeusScrollTopchatPanel;

function bindChatPanelScrollPaging() {

    getDoc().querySelector('#chatPanel').onscroll = function (e) {


       setTimeout(function (){
           // یعنی نزولی 
           if (50 > this.scrollTop) {
               // element is at the end of its scroll, load more content

               if (!CurrentUserInfo.AllChatsRead){
                   CurrentUserInfo.pageNumber++;
                   MyCaller.Send("ReadChat", {targetId: CurrentUserInfo.targetId, pageNumber: CurrentUserInfo.pageNumber});
               }
           }
       },5000)

     //   previeusScrollTopchatPanel = this.scrollTop;


    }


    getDoc().querySelector('#chatPanel').addEventListener("wheel", function () {
        if (!CurrentUserInfo.pageNumber || CurrentUserInfo.pageNumber === 1) {
            CurrentUserInfo.pageNumber++;
            MyCaller.Send("ReadChat", {targetId: CurrentUserInfo.targetId, pageNumber: CurrentUserInfo.pageNumber});
        }
    });

}

function gapShowUserActivityDetailInhtml(el) {

    let customerId = el.attributes.getNamedItem('customerid').value;
    gapShowUserActivityDetail(customerId);
}

function gapShowUserActivityDetail(customerId) {

    MyCaller.Send("GetCustomerActivityDetail", {
        customerId: customerId
    });
}

function getCustomerActivityDetailCallback(res) {

    if (!res.Content) {
        return;
    }

    this.makeItemForInfo = function (contentElement, i) {
        i++;
        return "            <tr>\n" +
            "            <td> " + contentElement.Time + "</td>\n" +
            "            <td>" + contentElement.Region + "</td>\n" +
            "                            <td> " + contentElement.CityName + "</td>\n" +
            "                            <td>" + contentElement.PageTitle + "</td>\n" +
            "                            <td style=\"    text-overflow: ellipsis;\n" +
            "                                           max-width: 250px;\n" +
            "                                           overflow: hidden;\"> " + contentElement.Url + "</td>\n" +
            "    </tr>\n";

    };

    let content = "";
    for (let i = 0; i < res.Content.length; i++) {
        content += this.makeItemForInfo(res.Content[i], i)
    }


    let table = " " +
        "<div><button onclick='closeBigPanel(this)' >x</button> " +
        //     "<span>" + res.Temp.Name + "</span>" +
        "" +
        "</div>" +
        "<hr/>" +
        "<table class=\"gapTable\">\n" +
        `<thead>
 <tr>
 <th colspan='99' style='text-align: center'>جزئیات حرکات و پیمایش کاربر در صفحه</th>
</tr>
 <tr>
 <th>زمان</th>
 <th>استان</th>
 <th>شهر</th>
 <th>عنوان صفحه</th>
 <th>آدرس صفحه</th>
</tr>
 
</thead>` +
        "            <tbody>\n" + content + "            </tbody>\n" +
        "        </table>";


    getDoc().querySelector('#bigPanel').innerHTML = table;
    getDoc().querySelector('#bigPanel').style.display = 'block';
    getDoc().querySelector('#bigPanel').style.top = '262px';
    getDoc().querySelector('#bigPanel').style.left = '229px';


    getDoc().querySelector('#bigPanel').click();
    dragElement(getDoc().querySelector('#bigPanel'))


}

function closeBigPanel(el) {
    getDoc().querySelector(`#bigPanel`).style.display = 'none'
}

function OpenChatScreen(AccountId, AccountName, ProfilePhotoId) {
    let gap_onlines = getDoc().querySelector('#gap_onlines');
    if (gap_onlines) {
        gap_onlines.innerHTML = '';
    }

    let bgImage = ProfilePhotoId ? `background-image: url(${baseUrlForapi}/Upload/Upload?id=${ProfilePhotoId})` : '';
    let sum = !ProfilePhotoId ? `<span style="font-size: 20px !important;" class="gap_online_admin_content gap_online_admin_come_right">${AccountName.charAt(0)}</span>` : '';

    let onlineStatusText;
    if (CurrentUserInfo.targetStatus == '0' || CurrentUserInfo.targetStatus == 0) {

        onlineStatusText = '  پشتیبانی آنلاین می باشد';
    } else {
        onlineStatusText = '  پشتیبانی آفلاین است';

    }

    let html = `
     <span accountid="AccountId" class="gap_online_admin" aria-label="${AccountName}" data-microtip-position="left" role="tooltip"
         style="height: 44px;
         width: 44px;    margin-right: 30px;
         margin-top: 10px;${bgImage}"
>
         ${sum}
         
         </span>
                <div class="gap_title">
                    <h4 id="gap_AccountName">${AccountName}</h4>
                      <div  id="gap_OnlineStatus">
                                      ${onlineStatusText}
                                    </div>
                </div>`;


    getDoc().querySelector('#gapAfterChat').style.display = 'flex';
    getDoc().querySelector('#gapAfterChat').innerHTML = html;
    getDoc().querySelector('#gapBeforeChat').style.display = 'none';

}

function makeOnlineAdminCircle(arrItem) {
    if (!arrItem) {
        console.error('makeOnlineAdminCircle is null');
        return;
    }


    let sum = 'ک';
    if (arrItem.Name) {
        sum = arrItem.Name.charAt(0);
    }

    //todo:server

    let img = '';
    if (arrItem.ProfileImageId) {
        img = `style="background-image: url(${baseUrlForapi}/Upload/Upload?id=${arrItem.ProfileImageId})"`;
    }

    //todo:server ImageId
    let html = `
        
         <span  onlineaccountid="${arrItem.Id}" class="gap_online_admin gap_online_admin_account" aria-label="${arrItem.Name}" data-microtip-position="left" role="tooltip"
         
         ${img}
         
         
         >
                    <span class="gap_online_admin_content gap_online_admin_come_right">${sum}</span>

                </span>

`;


    return html;
}

function addCircleOnNewOrOnlineAgain(res) {
    if (!res) {
        alert('addCircleOnNewOrOnlineAgain is null');
        return;
    }

    if (!cookieManager.getItem('targetId')) {


        let gap_online_admin_accounts = getDoc().querySelectorAll(`.gap_online_admin_account`);
        if (gap_online_admin_accounts && gap_online_admin_accounts.length < 3) {

            let exist = getDoc().querySelector(`.gap_online_admin_account[onlineaccountid="${res.Id}"]`)

            if (!exist) {
                let html = makeOnlineAdminCircle(res)

                let gapContent = getDoc().querySelector('#gap_onlines');

                if (gapContent) {
                    gapContent.innerHTML = html;
                }

            }

        }


    }
}

function changeDotPic() {
    CurrentUserInfo.targetId = cookieManager.getItem('targetId')
    CurrentUserInfo.targetName = cookieManager.getItem('targetName')
    CurrentUserInfo.targetStatus = cookieManager.getItem('targetStatus')
    CurrentUserInfo.ProfileImageId = cookieManager.getItem('ProfileImageId')


    if (!CurrentUserInfo.targetName)
        return;
    let html = ``;

    if (CurrentUserInfo.ProfileImageId && CurrentUserInfo.ProfileImageId !== 'null') {

        getDoc().querySelector('#dot').style.backgroundImage = `url(${baseUrlForapi}/Upload/Upload?id=${CurrentUserInfo.ProfileImageId})`
        // html=`<div><div style="background-image: url(${baseUrlForapi}/Upload/Upload?id=${CurrentUserInfo.ProfileImageId})"> </div></div>`;
        getDoc().querySelector('#dot').innerHTML = '';
        getDoc().querySelector('#dot').style.backgroundSize = 'contain';
        getDoc().querySelector('#dot').style.height = '50px';
        getDoc().querySelector('#dot').style.width = '50px';
    } else {
        html += `<div aria-label="${CurrentUserInfo.targetName}"  title="${CurrentUserInfo.targetName}" data-microtip-position="left" role="tooltip">

<i class="fa fa-user-circle-o" aria-hidden="true" style="font-size: 32px" 
 
></i></div>`;
        getDoc().querySelector('#dot').innerHTML = html;

    }

}

function showNewOnTheFlyMessage(Message) {
    if (!getDoc().querySelector('#gapContent').classList.contains('adp-hide')) {
        return;
    }
    getDoc().querySelector('#gapOnTheFlyMessageText').innerHTML = Message;

    //   getDoc().querySelector('#gapOnTheFlyMessageText').append(createElementFromHTML(Message));


    if (cookieManager.getItem('ProfileImageId') && cookieManager.getItem('ProfileImageId').toLowerCase() !== 'null') {
        getDoc().querySelector('#gapOnTheFlyMessageProfileImage').style.backgroundImage = `url(${baseUrlForapi}/Upload/Upload?id=${CurrentUserInfo.ProfileImageId})`;
        getDoc().querySelector('#gapOnTheFlyMessageProfileImage').style.backgroundSize = 'contain';
    } else {
        let letter = (CurrentUserInfo.targetName ? CurrentUserInfo.targetName : "ک").charAt(0);
        getDoc().querySelector('#gapOnTheFlyMessageProfileText').innerText = letter;

    }


    if ($ && $('#gapOnTheFlyMessage')) {
        $('#gapOnTheFlyMessage').show('fast');
        setTimeout(function () {
          //  getDoc().querySelector('#gapOnTheFlyMessage').style.display = null;

            toggle(getDoc().querySelector('#gapOnTheFlyMessage'))
        }, 1000)
    } else {
        toggle(getDoc().querySelector('#gapOnTheFlyMessage'))
//        getDoc().querySelector('#gapOnTheFlyMessage').style.display = null;
    }

}

class BasePlugin {
    adminSelectCustomerCallback(res, isGapMe) {
        let AccountId = res.Content.AccountId;


        CurrentUserInfo.targetId = AccountId;
        CurrentUserInfo.targetName = res.Content.AccountName;
        CurrentUserInfo.targetStatus = res.Content.OnlineStatus;
        CurrentUserInfo.ProfileImageId = res.Content.ProfileImageId;

        cookieManager.setItem('targetId', CurrentUserInfo.targetId)
        cookieManager.setItem('targetName', CurrentUserInfo.targetName)
        cookieManager.setItem('targetStatus', CurrentUserInfo.targetStatus)
        cookieManager.setItem('ProfileImageId', CurrentUserInfo.ProfileImageId)


        OpenChatScreen(AccountId, CurrentUserInfo.targetName, res.Content.ProfileImageId);

//اطلاعات شبکه های اجتماعی را بر میگرداند
        if (!getDoc().querySelector('#gapSocialCirlces')) {
            MyCaller.Send('GetSocialChannelsInfo')

        }


        changeDotPic()
    }

    adminSendToCustomerCallback(res, isGapMe) {


        let AccountId = res.Content.AccountId;
        let Message = res.Content.Message;
        let TotalReceivedMesssages = res.Content.TotalReceivedMesssages;


        if (!cookieManager.getItem('targetId')) {
            this.adminSelectCustomerCallback(res)
        }

        pushManager.push('پیغام جدید', Message);

        CurrentUserInfo.ProfileImageId = res.Content.ProfilePhotoId;
        CurrentUserInfo.targetName = res.Content.AccountName;

        //on the fly showing message:

        showNewOnTheFlyMessage(Message)


        //end

        this.handleNewMessageCome(AccountId, Message, TotalReceivedMesssages, function () {

            // در دوجا استفاده شده است ، اگر پغامی که در جای دیگری متصل است انجا فرستاده و اینجا هم میخواهیم نشان دهیم باشد ،
            // لازم نیست خبر رسیدن پیغام را به کسی بدهی چون پیغام خودش است

            if (!isGapMe && !res.Content.IsFromBot) {

                let isDelivered = CurrentUserInfo.targetId == AccountId + "" ? true : false;
                MyCaller.Send("CustomerReceivedMsg", {
                    chatId: res.Content.ChatId,
                    target: res.Content.AccountId,
                    isDelivered: isDelivered
                })
            }
        }, isGapMe, res.Content.gapFileUniqId, res.Content.Chat.Id, res.Content.Chat.Time)


    }

    adminOnlineAgainCallback(res) {

        let targetId = res.Content.Id;

        this.onlineStatusChangeCallback(res, targetId, true);
    }


    adminOfflineAgainCallback(res) {
        let targetId = res.Content.Id;

        this.onlineStatusChangeCallback(res, targetId, false);
    }

    newAccountOnlineCallback(res) {
        //this.addSingleRow(res);
        addCircleOnNewOrOnlineAgain(res.Content);

    }

    newSendPMByMeInAnotherPlaceCallback(res) {


        /*  // اگر خودش باشد دیگر لازم نیست دوباره پیغامی که ودش ارسال کرده اینجا دوباره وارد کند
          if(res.Content.MyAccountId==CurrentUserInfo.targetId  ||
              res.Content.CustomerId==CurrentUserInfo.targetId)
              return;*/


        if (res.Content.MultimediaContent) {

            let file = getDoc().querySelector("#muf_" + res.Content.gapFileUniqId);
            if (!file) {
                CurrentUserInfo.plugin.multimediaPmSendCallback(res, true);
            }

        } else {

            let msg = getDoc().querySelector("#msg_" + res.Content.gapFileUniqId);

            if (!msg) {
                if (CurrentUserInfo.IsCustomer) {

                    res.Content.AccountId = res.Content.MyAccountId;
                    res.Content.Chat = res.Content;

                    CurrentUserInfo.plugin.adminSendToCustomerCallback(res, true);
                } else {
                    CurrentUserInfo.plugin.customerSendToAdminCallback(res, true);

                }
            }


        }
    }

    loadReadyPmCallback(res) {

        if (CurrentUserInfo.targetId == "SavedPms") {

            this.readChatCallback(res)
        } else {

            if (res.Content && res.Content.EntityList) {
                let html = '';
                for (let i = 0; i < res.Content.EntityList.length; i++) {
                    let str = res.Content.EntityList[i].Message;
                    if (str.length > 10) str = str.substring(0, 10);

                    html += '<span msg="' + res.Content.EntityList[i].Message + '" onclick="readyPmOnclick(this)" class="readyPm">' + str + '</span>';
                }


                let readyPmHolder = getDoc().querySelector('.readyPmHolder');
                readyPmHolder.style.display = "block";
                readyPmHolder.innerHTML = html;

            }


        }


    }

    constructor() {
        CurrentUserInfo.commonDomManager = new CommonDomManager();
    }

    multimediaSend() {
        let content = getDoc().querySelector('#gapFileContent').value;
        let gapFileUniqId = getDoc().querySelector('#gapFileUniqId').value;

        gapFileUniqId = gapFileUniqId.replace('muf_', '');

        MyCaller.Send("MultimediaPmSend", {
            targetId: CurrentUserInfo.targetId,
            gapFileUniqId: gapFileUniqId,
            MultimediaContent: content,
            uniqId: CurrentUserInfo.uniqId
        });
    }

    multimediaDeliveredCallback(res) {
        var gapFileUniqId = res.Content.gapFileUniqId;


        console.warn('multimediaDeliveredCallback', getDoc().querySelector("#muf_" + gapFileUniqId))

        getDoc().querySelector("span[id='muf_" + gapFileUniqId + "']")
            .replaceWith(createElementFromHTML('<i style="margin: 10px;">√</i>'));
        //getDoc().querySelector("#muf_" + gapFileUniqId).children[0].appendChild(createElementFromHTML('<i style="margin: 10px;">√</i>'));
    }


    multimediaPmSendCallback(res, isGapMe) {


        pushManager.push('پیغام مولتی مدیای جدید', "عکس یا ویدئو جدید برای شما ارسال شده است");

        let targetId = (CurrentUserInfo.currentUsersIsAdmins || CurrentUserInfo.IsCustomer) ? res.Content.MyAccountId : res.Content.CustomerId;

        CurrentUserInfo.plugin.handleNewMessageCome(targetId, null, 5, res.Content.gapFileUniqId, res.Content.UniqId);
        var fileContent = res.Content.MultimediaContent;
        addNewMultimediaMessage(fileContent, null, function () {


            // در دوجا استفاده شده است ، اگر پغامی که در جای دیگری متصل است انجا فرستاده و اینجا هم میخواهیم نشان دهیم باشد ،
            // لازم نیست خبر رسیدن پیغام را به کسی بدهی چون پیغام خودش است

            if (!isGapMe) {
                MyCaller.Send("MultimediaDeliverd",
                    {chatId: res.Content.Id})
            }


        }, isGapMe, false, res.Content.gapFileUniqId, false, res.Content.UniqId, res.Content.Time)


    }

    msgDeliveredCallback(res) {
        var msg = res.Content.Message;


        let gaps = getDoc().querySelectorAll('.gapMe');

        for (let i = 0; i < gaps.length; i++) {
            if (gaps[i].innerText === msg) {
                gaps[i].innerHTML += '<i>√</i>';
            }
        }
    }

    addSingleRow(res) {

        let AccountId = res.Content.Id;
        let gapRowTmp = getDoc().querySelector(".gapRow[accountId='" + AccountId + "']");


        // اگر قبلا موجود باشد اضافه نکن فقط وضعیت او را انلاین نشان بده
        if (gapRowTmp) {
            gapRowTmp.querySelector('.gapStat').style.backgroundColor = "green";
            return;
        }


        var gapContent = getDoc().querySelector('#gapContent');

        gapContent.querySelectorAll('p').forEach(function (val, i, arr) {

            val.remove();
        })
        let item = res.Content;

        let htmlRow = this.makeRowItem(item);


        gapContent.innerHTML = gapContent.innerHTML + htmlRow;
        CurrentUserInfo.plugin.bindAfterRegister()

    }


    Register() {


        let URL = window.location.href;

        let Title = document.title;


        // let content= document.querySelector('meta[name="description"]');

        let desc = '';
        MyCaller.Send("Register", {Description: desc, Title: Title, URL: URL});
    }

    registerCallback(res) {


        // getDoc().querySelector('#dot').style.display = 'none';


        var gapContent = getDoc().querySelector('#gap_onlines');

        gapContent.innerHTML = '';

        var arr = [];
        arr = res.Content.EntityList;


        if (!CurrentUserInfo.IsCustomer && arr.length === 0) {
            gapContent.innerHTML = gapContent.innerHTML + '<p >هیچ کاربر آنلاینی یافت نشد</p>'
        } else {
            let p = gapContent.querySelector('p');
            if (p) {
                p.innerText = "";
            }
        }

        var _html = "";
        let length = arr.length >= 3 ? 3 : arr.length;

        for (let i = 0; i < length; i++) {

            let item = this.makeRowItem(arr[i]);
            _html += item;
        }

        if (arr.length > 3) {
            let item = `   <span aria-label="ادمین های آنلاین دیگر" data-microtip-position="left" role="tooltip" class="gap_online_admin gap_online_admin_come_right">
                    
                    <span class="gap_online_admin_content">${arr.length - length}+</span>

                </span>`;
            _html += item;

        }

        gapContent.innerHTML = gapContent.innerHTML + _html;


        /*    if (CurrentUserInfo.IsCustomer) {
                CurrentUserInfo.commonDomManager.toggleSelectChat();
    
            }*/
        CurrentUserInfo.plugin.bindAfterRegister()
        //res ==> MyDataTableResponse<MyAccount>

        // CurrentUserInfo.plugin.readChat(CurrentUserInfo.targetId);


        if (_EventTriggerManager.isInitialized() === false) {
            _EventTriggerManager.getEventTriggers();

        }

        MyCaller.Send('CustomerGetUsersSeparationConfig');


        return _html;
    }

    registerCallbackBackup(res) {


        // یعنی پنجره چت باز است
        if (getDoc().querySelector('#gapChat').style.display != 'none') {
            return;
        }

        this.removeGapRows();
        var gapContent = getDoc().querySelector('#gapContent');


        var arr = [];
        arr = res.Content.EntityList;


        if (!CurrentUserInfo.IsCustomer && arr.length === 0) {
            gapContent.innerHTML = gapContent.innerHTML + '<p >هیچ کاربر آنلاینی یافت نشد</p>'
        } else {
            let p = gapContent.querySelector('p');
            if (p) {
                p.innerText = "";
            }
        }

        var _html = "";
        for (let i = 0; i < arr.length; i++) {

            let item = this.makeRowItem(arr[i]);
            _html += item;
        }

        if (!CurrentUserInfo.IsCustomer) {
            _html = this.makeRowItem({Name: 'پیام های آماده', Id: 'SavedPms'}) + _html;

        }

        tmp = _html;
        gapContent.innerHTML = gapContent.innerHTML + _html;


        if (CurrentUserInfo.IsCustomer) {
            CurrentUserInfo.commonDomManager.toggleSelectChat();

        }
        CurrentUserInfo.plugin.bindAfterRegister()
        //res ==> MyDataTableResponse<MyAccount>


        return _html;
    }

    makeRowItem(arrItem) {

        return makeOnlineAdminCircle(arrItem);
    }

    makeRowItemBackup(arrItem) {


        let addres = '';
        if (arrItem.LastTrackInfo) {
            addres = arrItem.LastTrackInfo.CityName + "-" + arrItem.LastTrackInfo.Region + "-" + arrItem.LastTrackInfo.PageTitle;
        }
        let item = "" +
            "\n" +
            "        <div accountAddress='" + addres + "'  accountName='" + arrItem.Name + "'  accountStatus='" + arrItem.OnlineStatus + "'   accountId='" + arrItem.Id + "' class=\"gapRow\"\n" +
            "             >\n" +
            "            <i class=\"ti-user\"></i>\n" +
            "            <label> " + arrItem.Name + "</label>\n";

        if (arrItem.OnlineStatus === 0) {
            item += "            <i class=\"gapStat\" style=\"background-color: green\"></i>\n";
        }
        if (arrItem.OnlineStatus === 1) {
            item += "            <i class=\"gapStat\" style=\"background-color: grey !important\"></i>\n";
        }
        if (arrItem.OnlineStatus === 2) {
            item += "            <i class=\"gapStat\" style=\"background-color: orange !important\"></i>\n";
        }


        if (arrItem.LastTrackInfo) {
            item += "            <label onclick='gapShowUserActivityDetail(" + arrItem.LastTrackInfo.CustomerId + ")' title='" + arrItem.LastTrackInfo.Region + "' class='lastInfo'> " + arrItem.LastTrackInfo.CityName + "</label>\n";
            item += "            <label onclick='gapShowUserActivityDetail(" + arrItem.LastTrackInfo.CustomerId + ")' title='" + arrItem.LastTrackInfo.Region + "'  class='lastInfo'> " + arrItem.LastTrackInfo.PageTitle + "</label>\n";

        }

        if (arrItem.TotalUnRead) {
            // item += '<i class="MsgCount">' + arrItem.TotalUnRead + '</i>';
            item += '<i class="MsgCount">..</i>';
            addDotNewMsgCome();
        }

        item += "        </div>";

        return item;
    }


    removeGapRows() {

        let arr = getDoc().querySelectorAll('.gapRow');
        let i = 0;
        while (arr.length > i) {
            arr[i].remove();
            i++;

        }

        var gapContent = getDoc().querySelector('#gapContent');


        arr = gapContent.querySelectorAll('p');
        i = 0;
        while (arr.length > i) {
            arr[i].remove();
            i++;

        }
    }

    readSavedChats() {
        setCurrentPersonOnTop();
        MyCaller.Send("LoadReadyPm");

    }

    readChat(accountId, userId) {

        CurrentUserInfo.pageNumber = 1;

        MyCaller.Send("ReadChat", {
            targetId: accountId, pageNumber: 1,
            searchMessageId: CurrentUserInfo.searchMessageId
        });


        setCurrentPersonOnTop();

    }


    readChatCallback(res) {


        removeShowLoadingChats();


        if (CurrentUserInfo.pageNumber <= 1) {

            this.removePrevieusChats();
        }


        var html = "";

        var arr = [];
        arr = res.Content.EntityList;
        
    if(!arr || arr.length==0){
        CurrentUserInfo.AllChatsRead=true;
    }


        let chatPanel = getDoc().querySelector('#chatPanel');

        for (let i = 0; i < arr.length; i++) {

            var gapMe = arr[i].SenderType === 1;
            if (!CurrentUserInfo.IsCustomer) {
                gapMe = !gapMe;
            }

            if (arr[i].SenderType === 3) // AccountToAccount
            {
                if (arr[i].ReceiverMyAccountId + "" === CurrentUserInfo.targetId) {
                    gapMe = true;
                } else {
                    gapMe = false;
                }
            }

            if (arr[i].MultimediaContent) {
                arr[i].MultimediaContentHtml = addNewMultimediaMessage(arr[i].MultimediaContent, null, function () {

                }, gapMe, true, arr[i].gapFileUniqId, false, arr[i].UniqId, arr[i].Time);


            }

        }

        let prevChatDate;
        html = "";
        for (let i = 0; i < arr.length; i++) {
            var gapMe = arr[i].SenderType === 1;
            if (!CurrentUserInfo.IsCustomer) {
                gapMe = !gapMe;
            }

            if (arr[i].SenderType === 3) // AccountToAccount
            {
                if (arr[i].ReceiverMyAccountId + "" === CurrentUserInfo.targetId) {
                    gapMe = true;
                } else {
                    gapMe = false;
                }
            }

            var deliverdSign = gapMe && arr[i].DeliverDateTime;


            let dateExist = Array.from(getDoc().querySelectorAll('.gapHline'))
                .find(el => el.textContent.indexOf(arr[i].Date) >= 0);

            console.log('dateExist === > ', dateExist)
            console.log('arr[i].Date === > ', arr[i].Date)

            if (dateExist) {
                dateExist.style.color = "#ddd";
            }
            /*نمایش جداگانه تاریخ چت ها*/
            if (!prevChatDate) {
                html += `<h6 style=" width: 100%;     height: 1px;
   text-align: center; 
   border-bottom: 1px solid #ddd; 
   line-height: 0.1em;
   margin: 10px 0 20px;" ><span class="gapHline" style=" background:#fff; 
    padding:0 10px">${arr[i].Date} </span></h6>`

                prevChatDate = arr[i].Date;
            } else {

                // تاریخ عوض شده یا نه ؟
                if (prevChatDate === arr[i].Date) {

                } else {
                    html += `<h6 style=" width: 100%;     height: 1px;
   text-align: center; 
   border-bottom: 1px solid #ddd; 
   line-height: 0.1em;
   margin: 10px 0 20px;" ><span  class="gapHline" style=" background:#fff; 
    padding:0 10px">${arr[i].Date} </span></h6>`
                    prevChatDate = arr[i].Date;
                }
            }
            /*   End : نمایش جداگانه تاریخ چت ها*/

            if (arr[i].MultimediaContent) {
                html += arr[i].MultimediaContentHtml;
            } else {


                /*
                * CHAT TYPES:
                * 
                * 
                * 
                * */
                if (arr[i].ChatType === 5) {
                    // private Admin send
                    continue;
                }

                //screen record request from admin
                if (arr[i].ChatContentType === 3 && !arr[i].ChatContentTypeJobDone) {
                    html += screenRecordAccessRequestCallback({Content: arr[i]}, true);

                } else {
                    html += CurrentUserInfo.commonDomManager.makeChatDom(arr[i].Message, gapMe, deliverdSign, arr[i].gapFileUniqId, arr[i].UniqId, arr[i].Time, arr[i]);

                }


                /*
                * 
                * 
                * CHAT TYPES END
                * 
                * */
            }


        }


        if (CurrentUserInfo.pageNumber > 1) {
            chatPanel.innerHTML = html + chatPanel.innerHTML;

        } else {
            chatPanel.innerHTML += html;

            // اگر از سمت اسکرین جستجو آمده باشد 
            if (CurrentUserInfo.isSearch) {
                CurrentUserInfo.isSearch = false;

                //اسکرول نمی کنیم تا آخرین پیغام را در بالا نشان دهد
            } else {
                scrollToBottomChatPanel();
            }
        }


        for (let i = 0; i < arr.length; i++) {

            if (arr[i].formId) {
                let formParent = getDoc().querySelector(`#gap_formId_${arr[i].Id}_${arr[i].formId}`);
                if (!formParent || !formParent.querySelector('form')) {

                    if (!arr[i].FormPassed) {
                        MyCaller.Send('CustomerGetFormSingle', {formId: arr[i].formId, chatId: arr[i].Id})

                    } else {
                        formParent.innerHTML = `<p>${arr[i].FormPassed}</p>`
                    }
                }
            }


        }


        testDo();


        //     getDoc().getElementById('chatPanel').innerHTML = html;


    }

    removePrevieusChats() {
        getDoc().querySelector('#chatPanel').innerHTML = '';

    }

    sendNewText() {

        MyCaller.Send('CustomerSendToAdmin',
            {
                token: CurrentUserInfo.GetCurrentCustomerToken()
                , targetAccountId: CurrentUserInfo.targetId,
                typedMessage: CurrentUserInfo.typedMessage,
                gapFileUniqId: CurrentUserInfo.typedMessageGapFileUniqId,
                uniqId: CurrentUserInfo.uniqId

            });

    }

    bind(elementById) {


        this.configSignalR();

        // positionONTheFly(elementById)
        CurrentUserInfo.commonDomManager.correctOnTheFlyPosition();


        CurrentUserInfo.commonDomManager.bindDotOnClick();


        this.bindAfterRegister();

        CurrentUserInfo.commonDomManager.bindOntheFlyFocus(elementById)
    }

    bindAfterRegister() {
        CurrentUserInfo.commonDomManager.bindGapRowClick();
        CurrentUserInfo.commonDomManager.bindBackButton();
        CurrentUserInfo.commonDomManager.bindCloseButton();


        if (!CurrentUserInfo.IsCustomer) {
            CurrentUserInfo.commonDomManager.bindgapSearchButton();
        }

        CurrentUserInfo.commonDomManager.bindSubmitButton();
        CurrentUserInfo.commonDomManager.bindGapChatInput();
        bindChatPanelScrollPaging();


        bindIsTyping();

    }

    configSignalR() {

    }


    findByAttributeMakeChangeIfChatOpen(AccountId, ifCurrentChatPanelSameDoSomeThing, doSomethingIfSameOpenOrNot) {
        let gapRowTmp = getDoc().querySelector(".gapRow[accountId='" + AccountId + "']");

        var gapCurId = CurrentUserInfo.targetId + '';

        doSomethingIfSameOpenOrNot(gapRowTmp);

        ifCurrentChatPanelSameDoSomeThing(getDoc().getElementById('chatPanel'), gapRowTmp)

    }

    handleNewMessageCome(AccountId, Message, TotalReceivedMesssages, callback, isGapMe, gapFileUniqId, UniqId) {

        let gapRowTmp = getDoc().querySelector(".gapRow[accountId='" + AccountId + "']");


        let bef;
        if (gapRowTmp) {
            bef = gapRowTmp.querySelector('.MsgCount');

        }

        if (bef) {
            bef.remove();
        }
        if (gapRowTmp) {
            let div = createElementFromHTML('<i class="MsgCount">..</i>');
            gapRowTmp.appendChild(div);
            addDotNewMsgCome();
        }

        //let div = createElementFromHTML('<i class="MsgCount">' + TotalReceivedMesssages + '</i>');


        let gapRow = getDoc().querySelector(".gapRow[accountId='" + AccountId + "']");

        var gapCurId = CurrentUserInfo.targetId + '';


        if (Message) {


            var html = CurrentUserInfo.commonDomManager.makeChatDom(Message, isGapMe, false, gapFileUniqId, UniqId,null
                ,null,true);

            getDoc().querySelector('#chatPanel').innerHTML = getDoc().querySelector('#chatPanel').innerHTML
                + html ;

            


            scrollToBottomChatPanel();

            toggle(getDoc().querySelector(`.msg-${UniqId}`),'slide-up')


            setTimeout(function (){
                scrollToBottomChatPanel();

            },500)

            if (callback) {
                callback();
            }


        }


    }

    onlineStatusChangeCallback(res, targetId, isOnline) {

        /*  if (targetId===CurrentUserInfo.targetId){
              
          }*/
        /*  this.findByAttributeMakeChangeIfChatOpen(targetId, function (chatPanel, gapRow) {
  
              
              addCircleOnNewOrOnlineAgain(res);
              
              
              
  
              if (targetId + '' === CurrentUserInfo.targetId + '') {
                  let element = getDoc().querySelector('#gap_OnlineStatus');
                  if (isOnline) {
                      element.innerText = 'پشتیبانی آنلاین می باشد';
  
                  } else {
                      element.innerText = 'پشتیبانی آفلاین می باشد';
  //                element.style.backgroundColor = 'grey';
                  }
              }
  
  
          }, function (gapRow) {
  
              let gapStat = gapRow.querySelector('.gapStat');
  
              if (isOnline) {
                  if (gapStat)
                      gapStat.style.backgroundColor = "green";
                  gapRow.setAttribute('accountstatus', '0');
              } else {
                  if (gapStat)
                      gapStat.style.backgroundColor = "grey";
                  gapRow.setAttribute('accountstatus', '1');
              }
          })*/
    }
}

let configWebSocket = function (onOpen) {
    CurrentUserInfo.ws = new WebSocket("ws://" + baseUrl + ":8181/");
    CurrentUserInfo.ws.onopen = function () {
        console.log('اتصال برقرار شد');
        /*alert("About to send data");
        ws.send("Hello World"); // I WANT TO SEND THIS MESSAGE TO THE SERVER!!!!!!!!
        alert("Message sent!");*/


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
    CurrentUserInfo.ws.onclose = function () {
        // websocket is closed.
        console.error("اتصال قطع شد");


        socketConnect();
        CurrentUserInfo.dotColorBackup = getDoc().querySelector('#dot').style.backgroundColor;
        getDoc().querySelector('#dot').style.backgroundColor = 'grey';
    };
}

class CustomerPlugin extends BasePlugin {

}

function createElementFromHTML(htmlString) {
    var div = document.createElement('div');
    div.innerHTML = htmlString.trim();

    // Change this to div.childNodes to support multiple top-level nodes
    return div.firstChild;
}


function readyPmOnclick(el) {

    getDoc().querySelector('#gapChatInput').value = el.getAttribute('msg');
    //    CurrentUserInfo.commonDomManager.enterNewText();

    enterTextMessageAndSend();

}

function login(el) {

    let admin = getDoc().querySelector('#gaploginUsername').value;
    let password = getDoc().querySelector('#gaploginPassword').value;


    //prompt('لطفا نام کاربری خود را وارد نمایید', 'admin');
    if (!admin) {
        alert('نام کاربری  وارد نشده است')
        return;
    }
    // prompt('لطفا رمز عبور خود را وارد نمایید', 'admin');
    if (!password) {
        alert('رمز عبور صحیح وارد نشده است')
        return;
    }

    if (!admin || !password) {
        alert('نام کاربری یا رمز عبور صحیح وارد نشده است')
        return;
    }

    el.innerText = "در حال ورود به سیتم";

    MyCaller.Send("AdminLogin", {username: admin, password});
}

class AdminPlugin extends BasePlugin {
    customerSendToAdminCallback(res, isGapMe) {


        let CustomerId = res.Content.CustomerId;
        let Message = res.Content.Message;
        let TotalReceivedMesssages = res.Content.TotalReceivedMesssages;


        pushManager.push('پیغام جدید', Message);

        this.handleNewMessageCome(CustomerId, Message, TotalReceivedMesssages, function () {


            // در دوجا استفاده شده است ، اگر پغامی که در جای دیگری متصل است انجا فرستاده و اینجا هم میخواهیم نشان دهیم باشد ،
            // لازم نیست خبر رسیدن پیغام را به کسی بدهی چون پیغام خودش است
            if (!isGapMe) {
                let isDelivered = CurrentUserInfo.targetId == CustomerId + "" ? true : false;
                MyCaller.Send("AdminReceivedMsg", {
                    chatId: res.Content.ChatId,
                    target: res.Content.CustomerId,
                    isDelivered: isDelivered
                })
            }
        }, isGapMe, res.Content.gapFileUniqId, res.Content.Chat.UniqId, res.Content.Chat.Time);


    }

    sendNewText() {
        MyCaller.Send('AdminSendToCustomer',
            {
                adminToken: _currentAdminInfo.adminToken
                , targetUserId: CurrentUserInfo.targetId,
                typedMessage: CurrentUserInfo.typedMessage
                ,
                gapFileUniqId: CurrentUserInfo.typedMessageGapFileUniqId,
                uniqId: CurrentUserInfo.uniqId

            });
    }

    Register() {


        /*
              this.checkLoginAndGetClientsForAdmin();
        */

    }

    checkLoginAndGetClientsForAdmin() {

        if (_currentAdminInfo.adminToken) {
            if (_currentAdminInfo.adminToken.length < 20) {
                _currentAdminInfo.adminToken = null;
            }
        }

        // loggedInbefore
        if (_currentAdminInfo.adminToken) {
            MyCaller.Send("GetClientsListForAdmin", {
                userType: CurrentUserInfo.UserType
            });
            dragElement(getDoc().querySelector("#onTheFly"));


        } else {
            this.showLogin()
        }
    }


    showLoginBackup() {

        let admin = prompt('لطفا نام کاربری خود را وارد نمایید', 'admin');
        if (!admin) {
            return;
        }
        let password = prompt('لطفا رمز عبور خود را وارد نمایید', 'admin');
        if (!password) {
            return;
        }

        if (!admin || !password) {
            alert('نام کاربری یا رمز عبور صحیح وارد نشده است')
            return;
        }


        MyCaller.Send("AdminLogin", {username: admin, password});

    }

    showLogin() {

        var gapLoginForm = getDoc().querySelector('#gapLoginForm');

        if (gapLoginForm)
            return;

        var loginFormHtml = `<form id='gapLoginForm'>

            <div>
                  <label>نام کاربری</label>
                  <input id='gaploginUsername' style='height:30px; width: 100%;text-align: left;direction: ltr' type='text' />
            </div >
<br/>
             <div>
                  <label>رمز عبور</label>
                  <input style='height:30px;width: 100%;text-align: left;direction: ltr' id='gaploginPassword' type='password'/>
            </div >
    <div>
<br/>

                  <button style='float:left' type='button' onclick='login(this)'>ورود</button>
            </div >

</form > `;


        let div = document.createElement('div');
        div.innerHTML = loginFormHtml;

        let gapiloading = getDoc().querySelector('#gapiloading')
        if (gapiloading) {
            gapiloading.innerText = 'در انتظار برای ورود به سیستم';
        }
        getDoc().querySelector('#gapContent').appendChild(div);


    }

    getClientsListForAdminCallback(res) {
        this.ArchiveButtonSet();

        this.registerCallback(res);
    }

    ArchiveButtonSet() {
        let goArchive = getDoc().querySelector('#goArchive');
        goArchive.style.display = 'block';
        // goArchive.href += "&adminToken=" + _currentAdminInfo.adminToken;

        let gapAutomaticPmSend = getDoc().querySelector('#gapAutomaticPmSend');
        gapAutomaticPmSend.style.display = 'block';


        if (getDoc().querySelector('#changeUsers').innerText === 'بازدید کنندگان') {
            getDoc().querySelector('#toolsPanel').style.display = 'none';


        } else {
            let x2 = getDoc().querySelector('#toolsPanel');
            x2.style.display = 'inherit';


        }

        let gapBackButton = getDoc().querySelector('#gapBackButton');
        gapBackButton.style.display = 'none';

        let gpSearchPanel = getDoc().querySelector('#gapSearchButton');
        gpSearchPanel.style.display = 'block';


        getDoc().querySelector('#adminTokenArchive').setAttribute('value', _currentAdminInfo.adminToken);

        let changeUsers = getDoc().querySelector('#changeUsers');
        changeUsers.style.display = 'block';


        let goPanel = getDoc().querySelector('#goPanel');
        goPanel.style.display = 'block';
        // goPanel.action += "&adminToken=" + _currentAdminInfo.adminToken+"&websiteToken="+websiteToken;

        getDoc().querySelector('#adminToken').setAttribute('value', _currentAdminInfo.adminToken);
        getDoc().querySelector('#websiteToken').setAttribute('value', websiteToken);


        let seps = getDoc().querySelectorAll('.seps');

        for (let i = 0; i < seps.length; i++) {
            seps[i].style.display = 'block';
        }

        let Comment = getDoc().querySelector('#Comment');
        Comment.style.display = 'block';

        let gaptags = getDoc().querySelector('#gaptags');
        if (!CurrentUserInfo.currentUsersIsAdmins) {
            gaptags.style.display = null;

        } else {
            gaptags.style.display = 'none';

        }


        let selectedTag = getDoc().querySelector('#selectedTag');
        if (selectedTag) {
            selectedTag.style.display = null;

        }


    }


    adminSendToCustomerFailCallback(res) {

    }


    newCustomerOnlineCallback(res) {
        this.addSingleRow(res);

    }

    customerOfflineAgainCallback(res) {
        let targetId = res.Content.Id;

        this.onlineStatusChangeCallback(res, targetId, false);
    }


    customerOnlineAgainCallback(res) {

        let targetId = res.Content.Id;

        this.onlineStatusChangeCallback(res, targetId, true);
    }

    adminLoginCallback(res) {

        dragElement(getDoc().querySelector("#onTheFly"));

        if (res.Type === 0) {
            var token = res.Token;

            cookieManager.setItem('adminToken', token);
            _currentAdminInfo.adminToken = token;
            CurrentUserInfo.targetId = res.Content.Id;
            CurrentUserInfo.targetName = res.Content.Name;

            this.checkLoginAndGetClientsForAdmin();


            getDoc().querySelector('#gapLoginForm').remove();

        } else {

            alert('نام کاربری یا رمز عبور صحیح نیست')
        }


    }
}

/**
 * This handler retrieves the images from the clipboard as a blob and returns it in a callback.
 *
 * @param pasteEvent
 * @param callback
 */
function retrieveImageFromClipboardAsBlob(pasteEvent, callback) {
    if (pasteEvent.clipboardData == false) {
        if (typeof (callback) == "function") {
            callback(undefined);
        }
    }
    ;

    var items = pasteEvent.clipboardData.items;

    if (items == undefined) {
        if (typeof (callback) == "function") {
            callback(undefined);
        }
    }
    ;

    for (var i = 0; i < items.length; i++) {
        // Skip content if not image
        if (items[i].type.indexOf("image") == -1) continue;
        // Retrieve image on clipboard as blob
        var blob = items[i].getAsFile();

        if (typeof (callback) == "function") {
            callback(blob);
        }
    }
}

function newChatMsg(isGapMe, contentMedia, isReturn, _gapFileUniqId, UniqId, time) {


    let gapMsg = document.createElement('div');

    gapMsg.setAttribute('id', _gapFileUniqId);
    gapMsg.setAttribute('uniqid', UniqId);
    ;
    let div = document.createElement('div');

    div.innerHTML = contentMedia;

    gapMsg.className = "gapMsg gapMultimedia";

    gapMsg.appendChild(div);
    if (isGapMe)
        div.className = "gapMe gapInnerMsg";
    else
        div.className = "gapHe gapInnerMsg";


    div.append(createElementFromHTML(getSenderProfile()))

    if (time) {

        let t = document.createElement('div');

        t.innerHTML = `<small style="font-size: 10px" class="gapInnerMsg">${time}</small>`;

        div.appendChild(t);


    }


    if (isGapMe) {
        gapMsg.appendChild(createElementFromHTML(GetMakeEditDeleteButtons(UniqId, _gapFileUniqId)));
    }
    if (isReturn) {

    } else {


        getDoc().querySelector('#chatPanel').appendChild(gapMsg);
    }


    return gapMsg;
}

function validURL(str) {
    var pattern = new RegExp('^(https?:\\/\\/)?' + // protocol
        '((([a-z\\d]([a-z\\d-]*[a-z\\d])*)\\.)+[a-z]{2,}|' + // domain name
        '((\\d{1,3}\\.){3}\\d{1,3}))' + // OR ip (v4) address
        '(\\:\\d+)?(\\/[-a-z\\d%_.~+]*)*' + // port and path
        '(\\?[;&a-z\\d%_.~+=-]*)?' + // query string
        '(\\#[-a-z\\d_]*)?$', 'i'); // fragment locator
    return !!pattern.test(str);
}

function httpGet(theUrl, div) {
    if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    } else {// code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {

            let _div = document.createElement('div');
            _div.style.width = 200;
            _div.style.height = 100;

            _div.style.maxWidth = 200;
            _div.style.maxHeight = 100;

            _div.innerHTML = xmlhttp.responseText;

            div.innerHTML += _div.outerHTML;
        }
    }
    xmlhttp.open("GET", theUrl, false);
    xmlhttp.send();
}


function randomIntFromInterval(min, max) { // min and max included 
    return Math.floor(Math.random() * (max - min + 1) + min);
}

function multimediaPmSetAndSend(content, uniqId) {

    getDoc().querySelector('#gapFileContent').value = content;


    if (!uniqId) {
        let id = GetChats() + 1;
        uniqId = "muf_" + id;
        CurrentUserInfo.uniqId = id;

    }

    getDoc().querySelector('#gapFileUniqId').value = uniqId;


    return uniqId;
}


function closeImgPreview(el) {
    let maxImgPrev = document.getElementById('maxImgPrev');
    if (maxImgPrev) {
        maxImgPrev.remove();
    }
}

function showImageMaximized(el) {

    closeImgPreview();


    var w = window.innerWidth + "px";
    var h = window.innerHeight + "px";

    let img = createElementFromHTML(
        `<div  id="maxImgPrev"><div style="    z-index: 999999;background-color:black;opacity:0.5; position:absolute;top:0px;left:0px;right:0px;bottom:0px;
width:1000%;height:1000%;"></div><div style="z-index:999999999;margin:10%;  position:absolute;top:0px;left:0px;right:0px;bottom:0px;
width:100%;height:100%;"><img src="${el.src}" style="width:80% !important;"/>
<button type="button" onclick="closeImgPreview(this)">x</button>
</div></div>`);

    document.querySelector('body').append(img);

    if ($) {
        $("html, body").animate({scrollTop: 0}, "slow");

    } else {
        window.scrollTo(0, 0);

    }

}

function addNewMultimediaMessage(pastedData, e, sendData, isGapMe, isReturn
    , gapFileUniqId, isSendFile, UniqId, time) {


    let _gapFileUniqId = multimediaPmSetAndSend(pastedData, gapFileUniqId);


    /*uniqid='"+UniqId+"'*/
    sendData();

    let gapSending = isGapMe && isSendFile ? "<span id='" + _gapFileUniqId + "'  class='gapSending'></span>" : isGapMe ? '<i>√</i>' : '';

    if (pastedData.indexOf('video') >= 0) {

        let vide = "<video id='muf_" + _gapFileUniqId + "' height='50' width='200' controls poster=" + pastedData + ">" +
            "  <source src=" + pastedData + " type=\"video/mp4\">\n" +
            "  Your browser does not support the video tag.\n" +
            "</video>" + gapSending;


        let div = newChatMsg(isGapMe, vide, isReturn, _gapFileUniqId, UniqId, time);

        scrollToBottomChatPanel();

        return div.outerHTML;
    }


    if (pastedData.indexOf('audio') >= 0) {

        let vide = "<audio  id='muf_" + _gapFileUniqId + "'   controls >" +
            "  <source src=" + pastedData + " >\n" +
            "  Your browser does not support the video tag.\n" +
            "</audio >" + gapSending;

        let div = newChatMsg(isGapMe, vide, isReturn, _gapFileUniqId, UniqId, time);
        scrollToBottomChatPanel();

        return div.outerHTML;
    }

    if (pastedData.indexOf('image') >= 0) {

        let img = "<img onclick='showImageMaximized(this)' id='muf_" + _gapFileUniqId + "'   src='" + pastedData + "' height='100'  />" +
            "" + gapSending;

        let div = newChatMsg(isGapMe, img, isReturn, _gapFileUniqId, UniqId, time);

        scrollToBottomChatPanel();
        return div.outerHTML;
    }

    if (validURL(pastedData)) {

        /*     let a = document.createElement('a');
             a.href = pastedData;
     
             a.setAttribute("id", 'muf_'+ _gapFileUniqId);
     
             a.target = "_blank";
             a.innerText = pastedData;*/
        let a = "<a target = \"_blank\" href='" + pastedData + "'>" + pastedData + "</a>" + gapSending;

        let div = newChatMsg(isGapMe, a, isReturn, _gapFileUniqId, UniqId, time);

        //httpGet(pastedData,div);
        scrollToBottomChatPanel();

        return div.outerHTML;
    }


    if (e) {
        // Handle the event
        retrieveImageFromClipboardAsBlob(e, function (imageBlob) {
            // If there's an image, display it in the canvas
            if (imageBlob) {
                let gapMsg = document.createElement('div');
                ;
                let div = document.createElement('div');
                gapMsg.className = "gapMsg";

                gapMsg.appendChild(div);

                if (isGapMe)
                    div.className = "gapMe";
                else
                    div.className = "gapHe";


                if (time) {

                    let t = document.createElement('div');

                    t.innerHTML = `<span style="font-size: 8px" class="gapInnerMsg">${time}</span>`;

                    gapMsg.appendChild(t);
                }

                if (isReturn) {

                } else {
                    getDoc().querySelector('#chatPanel').appendChild(gapMsg);

                }


                // Create an image to render the blob on the canvas
                var img = new Image();
                div.appendChild(img)

                img.id = 'muf_' + _gapFileUniqId;

                img.setAttribute('uniqid', UniqId);
                img.width = 100;
                img.height = 100;
                img.style.objectFit = 'cover';


                // Once the image loads, render the img on the canvas
                img.onload = function () {

                };

                // Crossbrowser support for URL
                var URLObj = window.URL || window.webkitURL;

                // Creates a DOMString containing a URL representing the object given in the parameter
                // namely the original Blob
                img.src = URLObj.createObjectURL(imageBlob);


                let span = document.createElement('span');
                span.innerHTML = gapSending;
                div.appendChild(span);
                scrollToBottomChatPanel();
                return gapMsg.outerHTML;
            }
        });
    }


}

function bindgapFileAdder() {

    getDoc().querySelector('#gapFileAdder').onchange = function (e) {

        let elementById = e;

        for (let i = 0; i < elementById.target.files.length; i++) {
            let file = elementById.target.files[i];


            var fileReader = new FileReader();
            fileReader.onload = function (e) {
                console.log(e.target.result)

                let uniqId = GetChats() + 1;
                CurrentUserInfo.uniqId = uniqId;

                addNewMultimediaMessage(e.target.result, null, function () {
                    CurrentUserInfo.plugin.multimediaSend();
                }, true, null, null, true, uniqId, "همین الان")

            }

            fileReader.readAsDataURL(file)


        }
    }
}

function enterTextMessageAndSend() {


    CurrentUserInfo.commonDomManager.enterNewText();


    CurrentUserInfo.plugin.sendNewText();
}


function DeleteMessageCallback(res) {
    console.log('رسپانس حذف پیام');

    if (!res || !res.Content || !res.Content.uniqId) {

        console.error(' مقدار بازگشتی از سرور نال است ');
        return;
    }

    if (CurrentUserInfo.targetId && CurrentUserInfo.targetId != res.Content.targetId) {

        console.error('کاربر تارگت درست نیست');
        return;
    }
    let uniqId = res.Content.uniqId;
    let targetId = res.Content.targetId;

    let message = getDoc().querySelector(`.gapMsg[uniqid='${uniqId}']`)
    if (!message) {
        console.error(uniqId + " یافت نشد ");
        return;
    }
    console.log('در حال حذف پیام');

    let inner = GetInnerTextElement(message);


    if (inner) {
        inner.innerText = "حذف شد";
        console.log('پیام حذف شد در رسپانس');

    }

    let buttons = message.querySelectorAll('button');
    if (buttons) {
        for (let i = 0; i < buttons.length; i++) {
            buttons[i].disabled = false;
        }
    } else {
        console.error('دکمه ها یافت نشد');

    }

    gapChatInputFocus();


}

function gapChatInputFocus() {

    let input = getDoc().querySelector('#gapChatInput');
    if (input) {
        input.focus();
    }
}

function GetInnerTextElement(message, dontRemoveContent) {
    let inner = message.querySelector('.gapMe');
    if (!inner) {
        inner = message.querySelector('.gapHe');


    }
    if (!inner) {
        return inner;

    }

    let gapInnerMsg = message.querySelector('.gapInnerMsg');
    if (gapInnerMsg) {

        let text = gapInnerMsg.innerText;
        if (text) {


            return gapInnerMsg;
        }
        return gapInnerMsg;
    }

    return gapInnerMsg;


    /*if(dontRemoveContent){
       return inner; 
    }
    
    if (!inner){
        console.error('پیغام حذف نشد ، یافت نشد');
return inner;
    }

    let cloned= inner.cloneNode(true);

    let buttons= cloned.querySelectorAll('button');
    if (buttons){
        let i =0;
        while (buttons && buttons.length>0 && i<buttons.length){
            buttons[i].remove();
            i++;
        }
    }

    let iElements= cloned.querySelectorAll('i');
    if (iElements){
        let i =0;
        while (iElements && iElements.length>0 && i<iElements.length){
            iElements[i].remove();
            i++;
        }
    }
    
    
    
    if (cloned.innerText){
        cloned.innerText=cloned.innerText.trim() ;
        }

    return cloned;
        */

}

function replaceAll(str, find, replace) {
    return str.replace(new RegExp(find, 'g'), replace);
}

function EditMessageCallback(res) {

    console.log('رسپانس درخواست ویرایش');

    if (!res || !res.Content || !res.Content.uniqId) {

        console.error(' مقدار بازگشتی از سرور نال است ');
        return;
    }

    if (CurrentUserInfo.targetId && CurrentUserInfo.targetId != res.Content.targetId) {

        console.error('کاربر تارگت درست نیست');
        return;
    }
    let uniqId = res.Content.uniqId;
    let targetId = res.Content.targetId;

    let message = getDoc().querySelector(`.gapMsg[uniqid='${uniqId}']`)
    if (!message) {
        console.error(uniqId + " یافت نشد ");
        return;
    }
    console.log('انجام ویرایش در رسپاسن');


    let inner = GetInnerTextElement(message, true);


    if (inner) {

        inner.innerText = res.Content.Message;


    }
    console.log('ویرایش انجام شد');

}

function DeleteMsgOnClick(uniqId, gapFileUniqId, THIS) {
    console.log('جذف پیغام');

    let message = getDoc().querySelector(`.gapMsg[uniqid='${uniqId}']`)
    if (!message) {
        console.error(uniqId + " یافت نشد ");
        return;
    }
    console.log('ارسال درخواست حذف پیغام ');


    let buttons = message.querySelectorAll('button');
    if (buttons) {
        for (let i = 0; i < buttons.length; i++) {
            buttons[i].disabled = true;
        }
    } else {
        console.error('دکمه ها یافت نشد');

    }

    MyCaller.Send("DeleteMessage", {uniqId, targetId: CurrentUserInfo.targetId});
    console.log(' درخواست حذف پیغام ارسال شد ');

}

function EditMsgOnClick(uniqId, gapFileUniqId, THIS) {
    console.log('ویرایش فراخوانی شد');

    let message = getDoc().querySelector(`.gapMsg[uniqid='${uniqId}']`)
    if (!message) {
        console.error(uniqId + " یافت نشد ");
        return;
    }
    if (!message.parentElement) {
        console.error(message.parentElement + " یافت نشد ");
        return;
    }

    console.log('ورودی پر می شود جهت ویرایش');
    let inner = GetInnerTextElement(message);

    if (inner.classList.contains('gapMe') || inner.classList.contains('gapHe')) {
        // از نوع پیام مولتی مدیا
    } else {
        let text = inner.innerText;
        if (text) {

            inner.innerText = text.trim();
        }
    }
    if (!inner) {
        console.error('inner is null');
        return;
        ;
    }
    gapChatInputFocus();


    getDoc().querySelector('#gapChatInput').value = inner.innerText;


    let chatInput = getDoc().querySelector('#gapChatInput');
    chatInput.onpaste = function (e) {
    };
    getDoc().querySelector('#gapFileAdder').onchange = function (e) {

        alert('امکان ویرایش عکس وجود ندارد');
    };

    console.log('خالی کردن بایند ها انجام شد');

    getDoc().querySelector('#gapChatForm').onsubmit = function (e) {
        e.preventDefault();

        console.log('انجام ویرایش توسط کاربر');
        console.log('ارسال درخواست ویرایش');

        MyCaller.Send("EditMessage",
            {
                uniqId, targetId: CurrentUserInfo.targetId
                , message: getDoc().querySelector('#gapChatInput').value
            });


        getDoc().querySelector('#gapChatInput').value = '';
        CurrentUserInfo.commonDomManager.bindSubmitButton();
        CurrentUserInfo.commonDomManager.bindGapChatInput();

        bindIsTyping();
        console.log('بایند ها برگردانده شد');

        return false;
    }

}

/**
 * @return {string}
 */
function GetMakeEditDeleteButtons(uniqId, gapFileUniqId) {
    let dom = "";
    dom += `<div style="display:inline-flex;    margin-right: 20px;">
    <button onclick="DeleteMsgOnClick('${uniqId}','${gapFileUniqId}',this)" class="gapB gapRemB"><i class="fa fa-trash-o" aria-hidden="true"></i></button>
    <button  onclick="EditMsgOnClick('${uniqId}','${gapFileUniqId}',this)" class="gapB gapEdB"><i class="fa fa-pencil" aria-hidden="true"></i></button>
</div>`;
    return dom;
}

function getSenderProfile(chat) {

    
    if (!chat){
        chat={};
        chat.AccountName=CurrentUserInfo.targetName;
        chat.ProfilePhotoId=CurrentUserInfo.ProfileImageId;

    }
    
   
    
    
    let fullname=chat.AccountName;

    let name = (chat.AccountName ? chat.AccountName : "ک").charAt(0);


    name = chat.ProfilePhotoId ? '' : name;
    
    let ProfilePhotoId = chat.ProfilePhotoId;
    let bgImage = ProfilePhotoId ? `background-image: url(${baseUrlForapi}/Upload/Upload?id=${ProfilePhotoId})` : '';
    let sum = `<span  aria-label='${fullname}' data-microtip-position='right' role="tooltip" style="position:relative;  float: left; font-size: 20px !important;${bgImage}" class="gap_online_admin">
<span style="    right: 35%;
    position: absolute;
    top: 15%;
}">${name}</span></span>`;

    return sum;
}

class DomManager {

    bindGapChatInput() {
        bindgapFileAdder();

        let chatInput = getDoc().querySelector('#gapChatInput');
        chatInput.onpaste = function (e) {
            var clipboardData, pastedData;

            // Stop data actually being pasted into div
            e.stopPropagation();
            e.preventDefault();

            // Get pasted data via clipboard API
            clipboardData = e.clipboardData || window.clipboardData;
            pastedData = clipboardData.getData('Text');

            let uniqId = GetChats() + 1;
            CurrentUserInfo.uniqId = uniqId;

            addNewMultimediaMessage(pastedData, e, function () {

                CurrentUserInfo.plugin.multimediaSend();
            }, true, null, null, true, uniqId, "همین الان");
        }
    }

    enterNewText() {
        var text = getDoc().querySelector('#gapChatInput').value;

        let gapFileUniqId = randomIntFromInterval(1, 99999);

        let messages = GetChats();
        let uniqId = messages + 1;
        var html = CurrentUserInfo.commonDomManager.makeChatDom(text, true, false, gapFileUniqId, uniqId, 'همین الان',null,true);

        var chatPanel = getDoc().querySelector('#chatPanel');
        chatPanel.innerHTML = chatPanel.innerHTML + html ;

        toggle(getDoc().querySelector(`.msg-${uniqId}`),'slide-right')

        scrollToBottomChatPanel()

        getDoc().querySelector('#gapChatInput').value = '';

        CurrentUserInfo.typedMessage = text;
        CurrentUserInfo.uniqId = uniqId;
        CurrentUserInfo.typedMessageGapFileUniqId = gapFileUniqId;


        gapChatSubmit();
    }

    makeChatDom(msg, gapMe, delivered, gapFileUniqId, uniqId, time, chat,isAnim) {
        
        
        
        var dom = ` <div uniqid='${uniqId}' class=\"gapMsg msg-${uniqId} ${isAnim ? 'adp-hide' : ''}\"  id='msg_${gapFileUniqId}'>\n`;

        if (gapMe) {
            dom += "<div class=\"gapMe\">\n";
        } else {


            if (chat && chat.formId) {
                dom += "<div class=\"gapHe\" style='width: 80%'>\n";

            } else {
                dom += "<div class=\"gapHe\">\n";

            }

            /*profile img*/

            dom += getSenderProfile(chat);
            /*profile img end*/
        }


        let formId = chat && chat.formId ? chat.formId : '';
        let chatId = chat && chat.Id ? chat.Id : '';
        dom += `<span class="gapInnerMsg" id="gap_formId_${chatId}_${formId}">${msg}</span>`;

        if (time) {
            dom += `<small style="font-size: 10px;direction: rtl;float: left" class="gapInnerMsg">${time}</small>`;
        }


        if (delivered) {
            dom += "                        <i>√</i>\n";
        }

        dom += "                    </div>\n";
        if (gapMe) {
            dom += GetMakeEditDeleteButtons(uniqId, gapFileUniqId);
        }


        dom += "                </div>\n";


        return dom;

    }

    toggleSelectChat(el) {

        let changeUsers = getDoc().querySelector('#changeUsers');
        if (changeUsers) {
            changeUsers.style.display = 'none';
        }

        let gaptags = getDoc().querySelector('#gaptags');
        if (gaptags) {
            gaptags.style.display = 'none';
        }

        let selectedTag = getDoc().querySelector('#selectedTag');
        if (selectedTag) {
            selectedTag.style.display = 'none';
        }

        let gaptagUser = getDoc().querySelector('#gaptagUser');


        if (!CurrentUserInfo.IsCustomer) {
            if (gaptagUser)
                gaptagUser.style.display = null;
        } else {
            if (gaptagUser)
                gaptagUser.style.display = 'none';
        }


        let gapBackButton = getDoc().querySelector('#gapBackButton');

        // یعنی چت انتخاب شده
        if (el) {
            gapBackButton.style.display = null;
        } else {
            gapBackButton.style.display = 'none';
        }


        let gapScreens = getDoc().querySelectorAll('#gapScreen');
        for (let i = 0; i < gapScreens.length; i++) {
            gapScreens[i].remove();
        }
        getDoc().querySelector('#gapContent').style.display = 'block';


        if (!CurrentUserInfo.IsCustomer) {

            let x2 = getDoc().querySelector('#toolsPanel');
            x2.style.display = 'none';

        }


        CurrentUserInfo.pageNumber = 1;
        var arr = getDoc().querySelectorAll('.gapRow');
        for (let i = 0; i < arr.length; i++) {
            toggle(arr[i])
        }

        /*  var gapChat = getDoc().querySelector('#gapChat');
  
          if (CurrentUserInfo.isSearch) {
              gapChat.style.display = null;
          } else {
              toggle(gapChat)
  
          }*/


        getDoc().querySelector('#gapChatInput').focus()


        // یعنی یک چت انتخاب شده باشد 
        //در دوجا استفاده شده است در بک 
        if (el) {


            let msgCount = el.querySelector('.MsgCount');
            if (msgCount) {
                msgCount.remove();
            }

            var accountId = el.attributes.getNamedItem('accountId').value;
            var accountName = el.attributes.getNamedItem('accountName').value;
            var accountStatus = el.attributes.getNamedItem('accountStatus').value;
            var accountAddress = el.attributes.getNamedItem('accountAddress').value;


            //tag
            if (accountId !== 'SavedPms' && !CurrentUserInfo.IsCustomer) {
                GetUserAddedToTags(accountId);
            }

            if (CurrentUserInfo.isSearch) {
                let messageIdAtr = el.attributes.getNamedItem('messageId');
                if (!messageIdAtr)
                    alert('messageIdAtr is null');

                CurrentUserInfo.searchMessageId = messageIdAtr.value;

            }

            /*کلید نمایش جزئیات گردش کاربر */
            if (!CurrentUserInfo.IsCustomer) {// فقط نمایش برای ادمین ها
                getDoc().querySelector('#gapShowUserActivityDetailInhtml').setAttribute('customerId', accountId);

                if (accountId != "SavedPms") {
                    getDoc().querySelector('#gapShowUserActivityDetailInhtml').style.display = 'initial';
                }
                getDoc().querySelector('#GapAddress').style.display = 'list-item';
                getDoc().querySelector('#GapAddress').innerText = accountAddress;


            }

            /*پایان */

            CurrentUserInfo.targetId = accountId;
            CurrentUserInfo.targetName = accountName;
            CurrentUserInfo.targetStatus = accountStatus;


            ForwardChatButtonToggle();

            if (CurrentUserInfo.targetId === "SavedPms") {


                CurrentUserInfo.plugin.readSavedChats();

            } else {
                CurrentUserInfo.plugin.readChat(CurrentUserInfo.targetId);

                if (!CurrentUserInfo.IsCustomer) {

                    CurrentUserInfo.plugin.readSavedChats();
                }

            }

        }

    }

    bindSubmitButton() {
        getDoc().querySelector('#gapChatForm').onsubmit = function (e) {
            e.preventDefault();
            enterTextMessageAndSend();
            return false;
        }
    }

    createScreenPanel(id) {


        let arr = getDoc().querySelectorAll('.gapContent');
        for (let i = 0; i < arr.length; i++) {
            arr[i].style.display = 'none';
        }

        getDoc().querySelector('#onTheFly').onmousedown = null;
        getDoc().querySelector('#onTheFly').appendChild(getSearchScreenPanel());

    }

    getScreenPanel(id) {
        return getDoc().querySelector(id);
    }

    // 
    bindgapSearchButton() {

        let gpSearchPanel = getDoc().querySelector('#gapSearchButton');
        gpSearchPanel.onclick = function () {

            CurrentUserInfo.commonDomManager.createNewPanel();

        }
    }

    createNewPanel() {
        console.log('\'#gapSearchButton\').onclick');
        let gapSearchPanel = CurrentUserInfo.commonDomManager
            .getScreenPanel('#gapSearchPanel');

        console.log('gapSearchPanel', gapSearchPanel);

        if (!gapSearchPanel) {
            console.log('CurrentUserInfo.commonDomManager.createScreenPanel', gapSearchPanel);

            CurrentUserInfo.commonDomManager.createScreenPanel('gapSearchPanel');
        }
    }


    bindCloseButton() {
        getDoc().querySelector('#gapCloseButton').onclick = function () {
           // toggle(this.parentNode.parentNode);
            ADP.hide(this.parentNode.parentNode, 'flip-down');

            var x = getDoc().querySelector('#dot');
            toggle(x);



            if (!CurrentUserInfo.IsCustomer) {

                if (_currentAdminInfo.adminToken && _currentAdminInfo.adminToken.length > 20) {
                    let x2 = getDoc().querySelector('#toolsPanel');
                    x2.style.display = 'none';
                }


            }
            backOnTheFlyPosision();

        }
    }

    bindBackButton() {

        getDoc().querySelector('#gapBackButton').onclick = function () {
            CurrentUserInfo.commonDomManager.toggleSelectChat();

            CurrentUserInfo.targetId = null;

            if (!CurrentUserInfo.IsCustomer) {
                CurrentUserInfo.plugin.checkLoginAndGetClientsForAdmin();
                changeToolsPanelButtonColors();

            } else {
                CurrentUserInfo.plugin.Register();

            }


        }
    }

    bindOntheFlyFocus(elementById) {
        /*  elementById.onclick = function () {
              getDoc().getElementById('gapChatInput').focus()
  
  
          }*/
    }

    bindGapRowClick() {

        var arr = getDoc().querySelectorAll('.gapRow');

        for (let i = 0; i < arr.length; i++) {
            arr[i].onclick = function () {

                CurrentUserInfo.commonDomManager.toggleSelectChat(this);

            }
        }
    }

    bindDotOnClick() {

        getDoc().querySelector('#dot').onclick = function () {


            getDoc().querySelector('#gapOnTheFlyMessage').style.display = 'none'


            clearDotNewMsgCome();


            toggle(this)

            var x = getDoc().querySelector('#gapContent');
           // toggle(x)
            ADP.show(x, 'slide-down');

            
            setTimeout(()=>{

            },1000)
          //  ADP.hide(x, 'slide-down');




            // اطلاعات کاربر انتخاب شده را برمیگرداند در صورت انتخاب 
            MyCaller.Send('GetSelectedAdmin');


            if (cookieManager.getItem('targetId')) {
                CurrentUserInfo.targetId = cookieManager.getItem('targetId')
                CurrentUserInfo.targetName = cookieManager.getItem('targetName')
                CurrentUserInfo.targetStatus = cookieManager.getItem('targetStatus')
                CurrentUserInfo.ProfileImageId = cookieManager.getItem('ProfileImageId')


                //اطلاعات شبکه های اجتماعی را بر میگرداند
                if (!getDoc().querySelector('#gapSocialCirlces')) {
                    MyCaller.Send('GetSocialChannelsInfo')

                }


                OpenChatScreen(CurrentUserInfo.targetId, CurrentUserInfo.targetName, CurrentUserInfo.ProfileImageId);


            }

            if (CurrentUserInfo.targetId) {
                CurrentUserInfo.plugin.readChat(CurrentUserInfo.targetId);

            }


            /*     if (!CurrentUserInfo.IsCustomer) {
                     
                     if(_currentAdminInfo.adminToken && _currentAdminInfo.adminToken.length>20 ){
                         let x2 = getDoc().querySelector('#toolsPanel');
                         toggle(x2) 
                     }
                     
     
                 }*/
            // show chats 
            if (this.style.display === 'none') {
                CurrentUserInfo.commonDomManager.correctOnTheFlyPosition()

            }


            if (CurrentUserInfo.IsCustomer) {
                CurrentUserInfo.plugin.Register();

            } else {

                if (!CurrentUserInfo.targetId) {

                    CurrentUserInfo.plugin.checkLoginAndGetClientsForAdmin();
                }
            }


        }

    }

    correctOnTheFlyPosition() {
        /*  var x = getDoc().getElementById('gapContent');
          var ontheFly = getDoc().getElementById('onTheFly');
          lastTopOnTheFlyPos = ontheFly.style.top;
          lastLeftOnTheFlyPos = ontheFly.style.left;
  
          if ((x.getBoundingClientRect().y + x.getBoundingClientRect().height) > window.innerHeight) {
              ontheFly.style.top = window.innerHeight - (x.getBoundingClientRect().height + 50) + 'px';
          }
  
          if ((x.getBoundingClientRect().x + x.getBoundingClientRect().width + 50) > window.innerWidth) {
              ontheFly.style.left = window.innerWidth - (x.getBoundingClientRect().width + 50) + 'px';
          }*/
    }
}

class CommonDomManager extends DomManager {

}

class NavigationDomManager extends DomManager {

}

class ChatDomManager extends DomManager {

}


class WebsocketManager {

}


/*
*     accountId
* userId
*/
const CurrentUserInfo = {
    /**
     * @return {number}
     */
    GetCurrentCustomerToken() {
        return CurrentUserInfo.customerToken;
    },
    AddCurrentCustomerToken(Message) {

        CurrentUserInfo.customerToken = Message;
        cookieManager.setItem('customerToken', Message);
    }
}

/*
const cookieManager = {

    addToCookies(name, value) {

        var days;
        if (days)
        {
            var date = new Date();
            date.setTime(date.getTime()+days*24*60*60*1000); // ) removed
            var expires = "; expires=" + date.toGMTString(); // + added
        }
        else
            var expires = "";
        getDoc().cookie+= name+"=" + value+expires + ";path=/";    },

    getCookie(cname) {
        var name = cname + "=";
        var decodedCookie = decodeURIComponent(getDoc().cookie);
        var ca = decodedCookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    }
}*/


var lastTopOnTheFlyPos;
var lastLeftOnTheFlyPos;


function backOnTheFlyPosision() {
    var ontheFly = getDoc().getElementById('onTheFly');

    /* if (lastTopOnTheFlyPos) {
 
         ontheFly.style.top = lastTopOnTheFlyPos;
         ontheFly.style.left = lastLeftOnTheFlyPos;
     }*/
}


function addMore(num) {
    if (num < 100) {
        num += 200
    }
    return num;

}


function positionONTheFly(elementById) {
    elementById.style.top = '100px';
    elementById.style.left = '100px';


}

function scrollToBottomChatPanel() {
    getDoc().querySelector('#chatPanel').scrollTop = 99999999999;//getDoc().querySelector('#chatPanel').scrollHeight;

}


function toggle(x,type) {
    /*if (x.style.display === 'none') {
        x.style.display = 'inherit';
    } else {
        x.style.display = 'none';
    }*/

    ADP.toggle(x, type? type:'flip-down');

}

/*drag */

function dragElement(elmnt) {
}

function dragElementback(elmnt) {
    var pos1 = 0, pos2 = 0, pos3 = 0, pos4 = 0;
    /* if (getDoc().getElementById(elmnt.id + "header")) {
         // if present, the header is where you move the DIV from:
         getDoc().getElementById(elmnt.id + "header").onmousedown = dragMouseDown;
     } else {
         // otherwise, move the DIV from anywhere inside the DIV:
     }*/
    elmnt.onmousedown = dragMouseDown;


    function dragMouseDown(e) {
        e = e || window.event;
        e.preventDefault();
        // get the mouse cursor position at startup:
        pos3 = e.clientX;
        pos4 = e.clientY;
        document.onmouseup = closeDragElement;
        // call a function whenever the cursor moves:
        document.onmousemove = elementDrag;
    }

    function elementDrag(e) {
        e = e || window.event;
        e.preventDefault();
        // calculate the new cursor position:
        pos1 = pos3 - e.clientX;
        pos2 = pos4 - e.clientY;
        pos3 = e.clientX;
        pos4 = e.clientY;
        // set the element's new position:
        elmnt.style.top = (elmnt.offsetTop - pos2) + "px";
        elmnt.style.left = (elmnt.offsetLeft - pos1) + "px";
    }

    function closeDragElement() {
        // stop moving when mouse button is released:
        document.onmouseup = null;
        document.onmousemove = null;
    }
}


/*SignalR*/
let MyCaller = {

    Send(name, data) {


        if (name === "ReadChat") {
            showLoadingChats('#chatPanel');

        }


        if (CurrentUserInfo.ws.readyState != WebSocket.OPEN) {
            alert('در حال اتصال به سرور و ارسال درخواست');
            setTimeout(() => {

                    if (CurrentUserInfo.ws.readyState === WebSocket.CLOSED ||
                        CurrentUserInfo.ws.readyState === WebSocket.CLOSED) {

                        startUp();
                    }

                    alert('در حال اتصال');
                    this.Send(name, data);
                },
                1000);

            return;
        } else {
            clearMsg(null);

        }

        var req = {};
        req.Name = name;
        req.Body = data;
        req.Token = CurrentUserInfo.IsCustomer ? CurrentUserInfo.GetCurrentCustomerToken() : _currentAdminInfo.adminToken;
        req.WebsiteToken = websiteToken;


        req.SelectedTagId = CurrentUserInfo.selectedTagId;


        let gapIsOnlyOnly = getDoc().querySelector('#gapIsOnlyOnly');
        if (gapIsOnlyOnly) {
            req.gapIsOnlyOnly = gapIsOnlyOnly.checked;

        }
        //todo:goooooooo
        req.IsAdminMode = CurrentUserInfo.currentUsersIsAdmins;


        req.IsAdminOrCustomer = CurrentUserInfo.IsCustomer ? 2 : 1;
        //

        CurrentUserInfo.ws.send(JSON.stringify(req));


    }
}
var tmp;

//        Online=0,Offline=1,Busy=2
function getBackgroundColorForStatus(accountStatus) {
    if (accountStatus === '0') {
        return "green";
    }
    if (accountStatus === '1') {
        return "grey";
    }
    if (accountStatus === '2') {
        return "orange";
    }
    return Error('not find');
}

function setCurrentPersonOnTop() {
    /*getDoc().getElementById('gapCurPerson').innerText = CurrentUserInfo.targetName;
    getDoc().getElementById('gapCurId').innerText = CurrentUserInfo.targetId;

    getDoc().getElementById('gapCurPersonStatus').style.backgroundColor
        = getBackgroundColorForStatus(CurrentUserInfo.targetStatus)*/
}

class dispatcher {

    dispatch(res) {
        if (res.Type == -1)//error
        {
            console.error(res.Message);
        }
        switch (res.Name) {


            /*============================ Rating ===========================*/
            case 'adminSendRatingRequestCallback':
                adminSendRatingRequestCallback(res);
                break;

            /*============================ END ===========================*/





            /*============================ Users Separation ===========================*/
            case 'customerGetUsersSeparationConfigCallback':
                customerGetUsersSeparationConfigCallback(res);
                break;


            /*============================ END ===========================*/


            /*============================ live Assist ===========================*/


            case 'LiveAssistFireEventByAdminCallback':
                _ReceiverLiveAssistService.LiveAssistFireEventCallback(res);
                break;

            case 'liveAssistRequestCallback':
                _SenderLiveAssistService.liveAssistRequestCallback(res);
                break;

            /*====================== END ===========================*/

            case 'getEventTriggersCallback' :
                _EventTriggerManager.getEventTriggersCallback(res);
                break;

            case 'screenRecordAdminShareCallback':
                screenRecordAdminShareCallback(res);
                break;
            case 'screenRecordAdminShareRequestCallback':
                screenRecordAdminShareRequestCallback(res);
                break;

            case 'screenRecordAdminCloseCallback':
                screenRecordAdminCloseCallback(res);
                break;
            case 'screenRecordAccessRequestCallback':
                screenRecordAccessRequestCallback(res);
                break;

            case 'customerGetFormSingleCallback':
                customerGetFormSingleCallback(res);
                break;
            case 'getFormSingleCallback':
                getFormSingleCallback(res);
                break;

            case 'adminSendFormToCustomerCallback':
                adminSendFormToCustomerCallback(res);
                break;
            case 'saveFormDataCallback':
                saveFormDataCallback(res);
                break;
            case 'getSocialChannelsInfoCallback':
                getSocialChannelsInfoCallback(res);
                break;
            case 'adminStartTypingCallback':
                adminStartTypingCallback(res);
                break;
            case 'adminStopTypingCallback':
                adminStopTypingCallback(res);
                break;

            case 'adminSelectCustomerCallback':
                CurrentUserInfo.plugin.adminSelectCustomerCallback(res);
                break;

            case"getAllTagsForCurrentAdminCallback":
                getAllTagsForCurrentAdminCallback(res);
                break;

            case "userAddedToTagsCallback":
                userAddedToTagsCallback(res);
                break;
            case "getTagsCallback":
                getTagsCallback(res);
                break;
            case 'getVisitedPagesForCurrentSiteCallback':
                getVisitedPagesForCurrentSiteCallback(res);
                break;
            case "DeleteMessageCallback":
                DeleteMessageCallback(res);
                break;
            case "EditMessageCallback":
                EditMessageCallback(res);
                break;
            case "ClearCookie":


                cookieManager.removeItem('customerToken')
                cookieManager.removeItem('adminToken')
                _currentAdminInfo.adminToken = null;
                CurrentUserInfo.customerToken = null;


                CurrentUserInfo.targetId = null;
                CurrentUserInfo.targetName = null;
                CurrentUserInfo.targetStatus = null;
                CurrentUserInfo.ProfileImageId = null;


                cookieManager.removeItem('targetId')
                cookieManager.removeItem('targetName')
                cookieManager.removeItem('targetStatus')
                cookieManager.removeItem('ProfileImageId')

                CurrentUserInfo.IsRestart = true;
                // getDoc().innerHTML = '';

                CurrentUserInfo.ws.close();
                //startUp();


                break;


            case "GetAdminsListCallback":
                GetAdminsListCallback(res);
                break;

            case "newSendPMByMeInAnotherPlaceCallback":
                CurrentUserInfo.plugin.newSendPMByMeInAnotherPlaceCallback(res);
                break;
            case "registerCallback":
                CurrentUserInfo.AddCurrentCustomerToken(res.Token);
                CurrentUserInfo.plugin.registerCallback(res);
                break;
            case "readChatCallback":
                CurrentUserInfo.plugin.readChatCallback(res);
                break;
            case "adminLoginCallback":
                CurrentUserInfo.plugin.adminLoginCallback(res);
                break;
            case "getClientsListForAdminCallback":
                CurrentUserInfo.plugin.getClientsListForAdminCallback(res);
                break;
            case "adminSendToCustomerCallback":
                CurrentUserInfo.plugin.adminSendToCustomerCallback(res);
                break;
            case "adminSendToCustomerFailCallback":
                CurrentUserInfo.plugin.adminSendToCustomerFailCallback(res);
                break;

            case "customerSendToAdminCallback":
                CurrentUserInfo.plugin.customerSendToAdminCallback(res);
                break;
            case "msgDeliveredCallback":
                CurrentUserInfo.plugin.msgDeliveredCallback(res);
                break;
            case "multimediaPmSendCallback":
                CurrentUserInfo.plugin.multimediaPmSendCallback(res);
                break;
            case "multimediaDeliveredCallback":
                CurrentUserInfo.plugin.multimediaDeliveredCallback(res);
                break;



            //new Accont or Customer
            case "newAccountOnlineCallback":
                CurrentUserInfo.plugin.newAccountOnlineCallback(res);
                break;

            case "newCustomerOnlineCallback":
                CurrentUserInfo.plugin.newCustomerOnlineCallback(res);
                break;
            //end


            //customer online again
            case "customerOnlineAgainCallback":
                CurrentUserInfo.plugin.customerOnlineAgainCallback(res);
                break;
            case "customerOfflineAgainCallback":
                CurrentUserInfo.plugin.customerOfflineAgainCallback(res);
                break;
            //end

            //admin online again
            case "adminOnlineAgainCallback":
                CurrentUserInfo.plugin.adminOnlineAgainCallback(res);
                break;
            case "adminOfflineAgainCallback":
                CurrentUserInfo.plugin.adminOfflineAgainCallback(res);
                break;
            //end


            case "getCustomerActivityDetailCallback":
                getCustomerActivityDetailCallback(res);
                break;
            case "loadReadyPmCallback":
                CurrentUserInfo.plugin.loadReadyPmCallback(res);
                break;

            case "searchHandlerCallback":
                searchHandlerCallback(res);
                break;


            default:
                if (res && res.Message) {

                    console.error(res.Message);

                    if (debugMode) {
                        alert(res.Message)
                    }

                    if (res.Message.indexOf('کانکشکن متفاوت') >= 0) {
                        /*cookieManager.removeItem('customerToken')
                        cookieManager.removeItem('adminToken')
                        CurrentUserInfo.customerToken=null;
                        _currentAdminInfo.adminToken=null;*/

                        /*   if (debugMode){
                               alert('اتصال مجدد')
                           }
                           cookieManager.removeItem('customerToken')
                           cookieManager.removeItem('adminToken')
                           CurrentUserInfo.customerToken=null;
                           _currentAdminInfo.adminToken=null;
                           startUp();*/
                    }
                }
                break;

        }

    }

}

const _dispatcher = new dispatcher();
let connection;
let ws;

const _currentAdminInfo = {}
_currentAdminInfo.adminToken = cookieManager.getItem('adminToken');


CurrentUserInfo.customerToken = cookieManager.getItem('customerToken');


/*htmls:*/


function ScreenBack(el) {
    CurrentUserInfo.UserType = null;
    CurrentUserInfo.isSearch = false;
    dragElement(getDoc().querySelector('#onTheFly'));


    el.parentNode.remove();
    let arr = getDoc().querySelectorAll('#gapContent');

    for (let i = 0; i < arr.length; i++) {
        arr[i].style.display = 'block';
    }
    //.style.display='block';


}


function searchHandlerCallback(res) {
    let prevSearches = getDoc().querySelectorAll('.gapSearchDiv');
    for (let i = 0; i < prevSearches.length; i++) {
        prevSearches[i].remove();
    }

    let customerList = res.Content.customerlist;
    let sendMsgList = res.Content.sendMsgList;
    let receiveMsgList = res.Content.receiveMsgList;

    let lastHtml = "";
    for (let i = 0; i < customerList.length; i++) {

        let item = customerList[i];

        let name;
        let id;
        if (CurrentUserInfo.currentUsersIsAdmins) {
            if (!item.MyAccount || !item.MyAccount.Name) {
                console.error('item.MyAccount is null', item);
            }

            id = item.MyAccount.Id;
            name = item.MyAccount.Name;
        } else {
            if (!item.Customer || !item.Customer.Name) {
                console.error('item.Customer is null', item);
            }
            name = item.Customer.Name;
            id = item.Customer.Id;
        }


        /*      var accountId = el.attributes.getNamedItem('accountId').value;
            var accountName = el.attributes.getNamedItem('accountName').value;
            var accountStatus = el.attributes.getNamedItem('accountStatus').value;
            var accountAddress = el.attributes.getNamedItem('accountAddress').value;
*/
        let html = "\n" +
            "<div  onclick='MsgGapSearchPersonOnclick(this)'  class=\"gapSearchDiv\" accountId='" + id + "'" +
            " accountName='" + name + "' " +
            " accountStatus='' accountAddress=''>\n" +
            "    <label>" + name + "</label>\n" +
            "    <span>...</span>\n" +
            "    \n" +
            "</div>";
        lastHtml += html;
    }

    this.MakeMsgGapSearchDiv = function (item) {

        let name;
        let id;
        let chatId;
        if (CurrentUserInfo.currentUsersIsAdmins) {
            if (!item.MyAccount || !item.MyAccount.Name) {
                console.error('item.MyAccount is null', item);
            }

            id = item.MyAccount.Id;
            name = item.MyAccount.Name;
            chatId = item.Id;
        } else {
            if (!item.Customer || !item.Customer.Name) {
                console.error('item.Customer is null', item);
            }
            name = item.Customer.Name;
            id = item.Customer.Id;
            chatId = item.Id;
        }

        let html = "\n" +
            "<div  accountStatus='' accountAddress='' onclick='MsgGapSearchOnclick(this)' class=\"gapSearchDiv\" " +
            "accountId='" + id + "'" +
            "accountName='" + name + "' " +
            "messageId='" + chatId + "' \">\n" +
            "    <label>" + name + "</label>\n" +
            "    <span>" + item.Message + "</span>\n" +
            "    \n" +
            "</div>";

        return html;
    };

    this.MakeListToGapSearchDiv = function (list) {
        for (let i = 0; i < list.length; i++) {

            let item = list[i];

            let html = this.MakeMsgGapSearchDiv(item);

            lastHtml += html;
        }
    };


    this.MakeListToGapSearchDiv(sendMsgList);
    this.MakeListToGapSearchDiv(receiveMsgList);


    let gapSearchLoading = getDoc().querySelector('#gapSearchLoading');
    if (gapSearchLoading)
        gapSearchLoading.remove();

    getDoc().querySelector('#gapScreen').appendChild(createElementFromHTML("<div>" + lastHtml + "</div>"));//(createElementFromHTML(lastHtml));
}

function gapChatSearchInputOnChange(el, event) {
    if (el.parentNode.querySelector('p') == null) {
        el.parentNode.appendChild(createElementFromHTML("<p id='gapSearchLoading'>در حال خواندن</p>"))
    }

    let val = el.value;

    let prevSearches = getDoc().querySelectorAll('.gapSearchDiv');
    for (let i = 0; i < prevSearches.length; i++) {
        prevSearches[i].remove();
    }

    MyCaller.Send("SearchHandler",
        {
            customerId: CurrentUserInfo.targetId,
            searchTerm: val
        });


}

function getSearchScreenPanel() {


    let html = " \n" +
        "    <div class=\"gapScreen\" id=\"gapScreen\" ><button type=\"button\" " +
        "style=\"float: left; background-color: white;border: none;\" " +
        " onclick='ScreenBack(this)'>x</button>\n" +
        "<label>جستجو</label>" +
        " <input style='height: 25px;' onkeyup='gapChatSearchInputOnChange(this,event)' placeholder=\"در اینجا تایپ نمایید\" id=\"gapChatSearchInput\"/>\n" +
        "</div>"
    ;
    return createElementFromHTML(html);
}

function MsgGapSearchOnclick(el) {

    CurrentUserInfo.isSearch = true;
    CurrentUserInfo.commonDomManager.toggleSelectChat(el);
    dragElement(getDoc().querySelector("#onTheFly"));

}

function MsgGapSearchPersonOnclick(el) {
    // اگر کاربر انتخاب شده باشد دیگر لازم نیست دنبال پیغام خاصی بگردیم
    CurrentUserInfo.isSearch = false;
    CurrentUserInfo.commonDomManager.toggleSelectChat(el);
    dragElement(getDoc().querySelector("#onTheFly"));

}

function getRandomInt(min, max) {
    min = Math.ceil(min);
    max = Math.floor(max);
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

function showText(text) {
    let str = "";

    if (text) {
        var arr = text.split(' ');
        if (arr && arr.length === 0) {
            return text;
        }
        for (let i = 0; i < arr.length; i++) {
            if (validURL(arr[i])) {
                str += arr[i].link(arr[i]) + " ";
            } else {
                str += arr[i] + " ";
            }


        }
        return str;
    }
    return text;

}


let forwardChatTargetUserId;

// مربوط به انتقال گفتگو
function ForwardChatButtonToggle() {
    let forwardChatButton = getDoc().querySelector('#forwardChatButton');
    if (!forwardChatButton) {
        return;
    }
    if (!CurrentUserInfo.IsCustomer) {// فقط نمایش برای ادمین ها

        forwardChatTargetUserId = CurrentUserInfo.targetId;
        forwardChatButton.style.display = "block";
        forwardChatButton.onclick = ForwardChatButtonClick;
    } else {
        forwardChatButton.style.display = "none";
        forwardChatButton.onclick = function () {

        };
    }
}

// مربوط به انتقال گفتگو
function ForwardChatButtonClick() {

    if (!forwardChatTargetUserId) {
        ShowForwardChatPanel('هیچ چتی برای فوروارد انتخاب نشده است');
        return;
    }

    MyCaller.Send('GetAdminsList')

    ShowForwardChatPanel()
}

// مربوط به انتقال گفتگو
function ShowForwardChatPanel(list) {
    var forwardChatPanel = getDoc().querySelector('#ShowForwardChatPanel');

    if (forwardChatPanel) {
        forwardChatPanel.remove();
    }


    var forwardChatPanel;

    if (!list) {
        forwardChatPanel = `<div id='ShowForwardChatPanel'>

            <p>در حال خواندن اطلاعات ادمین ها</p>

</div > `;
    } else {
        forwardChatPanel = `<div id='ShowForwardChatPanel'>

           ${list}

</div > `;
    }


    let div = document.createElement('div');
    div.innerHTML = forwardChatPanel;

    getDoc().querySelector('#chatPanel').innerHTML = '';
    getDoc().querySelector('#chatPanel').appendChild(div);

}

// مربوط به انتقال گفتگو
function GetAdminsListCallback(res) {
    if (!res || !res.Content || !res.Content.EntityList || !res.Content.EntityList.length) {
        ShowForwardChatPanel('<p>هیچ ادمین دیگری آنلاین نیست</p>');
        return;
    }
    let arr = res.Content.EntityList;

    let html = '';
    for (let i = 0; i < arr.length; i++) {
        let item = `<div style="padding: 10px;
    border: 1px solid #ddd;" onclick="forwardChatAction('${arr[i].Id}')" accountName='${arr[i].Name}'  accountStatus='${arr[i].OnlineStatus}'   accountId='${arr[i].Id}'>
 
 <a href="#">${arr[i].Name}</a>
 
 </div>`;

        html += item;
    }

    ShowForwardChatPanel(html);


}

// مربوط به انتقال گفتگو
function forwardChatAction(myAccountId) {
    if (!forwardChatTargetUserId) {
        ShowForwardChatPanel('هیچ چتی برای فوروارد انتخاب نشده است');
        return;
    }


    MyCaller.Send('forwardChat', {
        myAccountId,
        targetUserId: forwardChatTargetUserId
    })

    ShowForwardChatPanel(`<p>درخواست انتقال گفتگو ارسال گردید</p>`);
    CurrentUserInfo.plugin.getClientsListForAdminCallback(res);

}

function ChangeUsers() {

    if (CurrentUserInfo.IsCustomer) {
        return; // ححتما باید ادمین باشد
    }


    if (CurrentUserInfo.currentUsersIsAdmins) {
        CurrentUserInfo.currentUsersIsAdmins = false;

        getDoc().querySelector('#changeUsers').innerText = 'ادمین ها';


    } else {

        getDoc().querySelector('#changeUsers').innerText = 'بازدید کنندگان';
        CurrentUserInfo.currentUsersIsAdmins = true;


    }
    CurrentUserInfo.plugin.checkLoginAndGetClientsForAdmin();

}


function GetChats() {

    let gapMsgs = getDoc().querySelectorAll('.gapMsg');
    if (gapMsgs) {
        let lastMsg = gapMsgs[gapMsgs.length - 1];

        if (lastMsg) {
            let uniqId = lastMsg.getAttribute('uniqid');
            if (!uniqId) {
                alert('کد پیغام نال است');
            }
            try {
                return parseInt(uniqId + "");
            } catch (e) {

                alert('فرمت کد پیغام درست نیست');
            }
        }

    }
    return 0;
}


/**/

(function (proxied) {
    window.alert = function () {
        let text = '';
        if (arguments) {
            for (let i = 0; i < arguments.length; i++) {
                text += arguments[i];
            }
        }

        let gapAlert = getDoc().querySelector('#gapAlert');
        if (gapAlert) {
            gapAlert.remove();
        }
        getDoc().querySelector('#gapContent').prepend(createElementFromHTML(`<div id="gapAlert" class="alert" style=" padding: 20px;
  background-color: #f44336;
  color: white;">
  <span style="margin-left: 15px;
  color: white;
  font-weight: bold;
  float: right;
  font-size: 22px;
  line-height: 20px;
  cursor: pointer;
  transition: 0.3s;" class="closebtn" onclick="this.parentElement.style.display='none';">&times;</span> 
  <strong>خطا!</strong> ${text}
</div>`));

        Logger(text);

        // return proxied.apply(this, arguments);
    };
})(window.alert);

(function (proxied) {
    window.gapMsg = function () {
        let text = '';
        if (arguments) {
            for (let i = 0; i < arguments.length; i++) {
                text += arguments[i];
            }
        }

        let gapMsg = getDoc().querySelector('#gapMsg');
        if (gapMsg) {
            gapMsg.remove();
        }
        getDoc().querySelector('#gapChat').prepend(createElementFromHTML(`<div  id="gapMsg" class="alert gapMsgAlert" style=" padding: 20px;

  color: white;">
  <span style="margin-left: 15px;
  color: white;
  font-weight: bold;
  float: right;
  font-size: 22px;
  line-height: 20px;
  cursor: pointer;
  transition: 0.3s;" class="closebtn" onclick="this.parentElement.style.display='none';">&times;</span> 
  <strong>پیغام!</strong> ${text}
  
</div>`));


        setTimeout(function () {

            gapMsg = getDoc().querySelector('#gapMsg');
            if (gapMsg) {
                gapMsg.remove();
            }

        }, 1000)


        // return proxied.apply(this, arguments);
    };
})(window.alert);


function changeToolsPanelButtonColors(THIS) {
    let changeScreenBack = getDoc().querySelector('#changeScreenBack');
    if (changeScreenBack) {
        changeScreenBack.click();
    }

    let arr = getDoc().querySelectorAll('#toolsPanel button');
    for (let i = 0; i < arr.length; i++) {

        if (!THIS) {
            if (i == 0) {
                arr[i].style.backgroundColor = 'blue';
                arr[i].style.color = '#ddd';
                continue;
            }
        }
        arr[i].style.backgroundColor = '#ddd';
        arr[i].style.color = 'black';

    }

    if (THIS) {
        THIS.style.backgroundColor = 'blue';
        THIS.style.color = '#ddd';
    }

}

function gapIsOnlyOnlyChange(THIS) {

    if (CurrentUserInfo.LastUserType &&
        CurrentUserInfo.LastSelectedFilterButton
    ) {
        changeUserTypes(CurrentUserInfo.LastUserType, CurrentUserInfo.LastSelectedFilterButton);
    } else {
        CurrentUserInfo.UserType = null;
        CurrentUserInfo.plugin.checkLoginAndGetClientsForAdmin();
    }
}

function changeUserTypes(type, THIS) {

    changeToolsPanelButtonColors(THIS);


    CurrentUserInfo.UserType = type;
    CurrentUserInfo.LastUserType = type;
    CurrentUserInfo.LastSelectedFilterButton = THIS;


    switch (type) {

        case 'all':
            CurrentUserInfo.UserType = null;
            CurrentUserInfo.plugin.checkLoginAndGetClientsForAdmin();
            return;
            break;
        case 'chatted':
            break;
        case 'notChat':
            break;
        case 'left':
            break;
        case 'separateWithPages':

            MyCaller.Send('GetVisitedPagesForCurrentSite')
            return;
            break;
        case 'returnedChatted':
            break;
        case 'returnedNotChatted':
            break;
        default:
            alert('نوع شناخته نشد');
            return;
            break;
    }
    CurrentUserInfo.plugin.checkLoginAndGetClientsForAdmin();

}

function getVisitedPagesForCurrentSiteCallback(res) {
    /*response is : CustomerTrackInfoViewModel
        {
            public string BaseUrl { get; set; }
            public int VisitedCount { get; set; }
            public List<Customer> Customers { get; set; }
        }*/


    let trackinfosViewModellist = res.Content;
    if (!trackinfosViewModellist) {
        alert('دیتا نال است');
        return;
    }

    changeScreen('getVisitedPagesForCurrentSite', 'gapContent');

    let html = ' ';

    if (!trackinfosViewModellist.length) {
        let getVisitedPagesForCurrentSite = getDoc().querySelector('#getVisitedPagesForCurrentSite');
        if (!getVisitedPagesForCurrentSite) {
            alert('getVisitedPagesForCurrentSite is null');
            return;
        }
        getVisitedPagesForCurrentSite.append(createElementFromHTML('<p>اطلاعاتی یافت نشد</p>'));
        return;
    }

    window['trackinfosViewModellist'] = trackinfosViewModellist;

    for (let i = 0; i < trackinfosViewModellist.length; i++) {


        let pageTitle = trackinfosViewModellist[i].PageTitle;
        let baseUrl = trackinfosViewModellist[i].BaseUrl;
        let VisitedCount = trackinfosViewModellist[i].VisitedCount;
        let Customers = trackinfosViewModellist[i].Customers;

        /*  if (pageTitle && pageTitle.length>40){
              pageTitle=pageTitle.substring(pageTitle.length-41,pageTitle.length-1)+'...';
          }*/


        html += `
        <div title="${baseUrl}" class="gapRow" onclick="showCustomersPerPage('${i}')">
        `;
        html += `        <label  dir="ltr" style="text-align: left;max-width:50px;max-height: 30px;overflow: auto">${pageTitle} </label>\n`;

        html += `        <p  style="color: #ddd;">تعداد بازدید:  ${VisitedCount}
`;
        if (Customers && Customers.length > 0) {
            let msg = 'تعداد آنلاین :'
            html += `${msg} ${Customers.length}   </p>`

            /*    html+=`<ul>`;
    
                for (let i = 0; i <Customers.length; i++) {
                        html+=`<li>`
                }
                html+=`</ul>`;*/

        }

        html += '</div>  ';


    }
    html += '</div>  ';

    let getVisitedPagesForCurrentSite = getDoc().querySelector('#getVisitedPagesForCurrentSite');
    if (!getVisitedPagesForCurrentSite) {
        alert('getVisitedPagesForCurrentSite is null');
        return;
    }
    getVisitedPagesForCurrentSite.append(createElementFromHTML(`<div>     <h5>پر بازدید ترین صفحات بترتیب</h5>\n` + html));


}

function showCustomersPerPage(i) {

    let trackinfosViewModellist = window['trackinfosViewModellist'];
    if (!trackinfosViewModellist) {
        alert('trackinfosViewModellist is null');
        return;
    }
    let rows = getDoc().querySelectorAll('#getVisitedPagesForCurrentSite .gapRow ');
    if (!rows) {
        alert('getVisitedPagesForCurrentSite is null');
        return;
    }
    for (let j = rows.length - 1; j >= 0; j--) {
        rows[j].remove();
    }


    let baseUrl = trackinfosViewModellist[i].BaseUrl;
    let VisitedCount = trackinfosViewModellist[i].VisitedCount;
    let Customers = trackinfosViewModellist[i].Customers;

    if (baseUrl && baseUrl.length > 40) {
        baseUrl = baseUrl.substring(baseUrl.length - 41, baseUrl.length - 1) + '...';
    }


    let html = `
        <div class="gapRow" >
        <h5>پر بازدید ترین صفحات بترتیب</h5>
        <p >تعداد بازدید:  ${VisitedCount}
        `;

    if (Customers && Customers.length > 0) {
        let msg = 'تعداد آنلاین :'
        html += `${msg} ${Customers.length}   </p>`

        /*    html+=`<ul>`;

            for (let i = 0; i <Customers.length; i++) {
                    html+=`<li>`
            }
            html+=`</ul>`;*/

    }
    if (Customers && Customers.length > 0) {

        let changeScreenBack = getDoc().querySelector('#changeScreenBack');
        if (!changeScreenBack) {
            alert('changeScreenBack is null');
            return;
        }
        changeScreenBack.click();

        let res = {};
        res.Content = {};
        res.Content.EntityList = Customers;

        CurrentUserInfo.pageNumber = 1;
        CurrentUserInfo.plugin.registerCallback(res);
    }
    /*   html+=`<ul>`;

       let res={};
       res.Content={};
       res.Content.EntityList=Customers;

       CurrentUserInfo.pageNumber=1;
       CurrentUserInfo.plugin.registerCallback(res);
       html+=                 CurrentUserInfo.plugin.registerCallback(res);


       html+=`</ul>`;
   }
   html+=`        <label  dir="ltr" style="color: #ddd;text-align: left">${baseUrl} </label>\n`;

   html+='</div>';

    getVisitedPagesForCurrentSite= getDoc().querySelector('#getVisitedPagesForCurrentSite');
   if (!getVisitedPagesForCurrentSite){
       alert('getVisitedPagesForCurrentSite is null');
       return;
   }
   getVisitedPagesForCurrentSite.append(createElementFromHTML(html));*/

}


function changeScreen(id, cls) {
    let gapSearchPanel = getDoc().querySelector('#' + id);


    if (!gapSearchPanel) {

        let arr = getDoc().querySelectorAll('.gapContent');
        for (let i = 0; i < arr.length; i++) {
            arr[i].style.display = 'none';
        }

        //  getDoc().querySelector('#onTheFly').onmousedown = null;
        ///  getDoc().querySelector('#onTheFly').appendChild(getSearchScreenPanel());
        let html = " \n" +
            "    <div class='" + cls + "' id='" + id + "' ><button id='changeScreenBack' type=\"button\" " +
            "style=\"float: left; background-color: white;border: none;\" " +
            " onclick='ScreenBack(this)'>x</button>\n" +
            "</div>"
        ;
        getDoc().querySelector('#onTheFly').appendChild(createElementFromHTML(
            html
        ));
    }
}


let CommentService = function (msg, callback) {
    try {


        fetch(baseUrlForapi + "/Comment/Comment", {
            method: "POST", headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({text: msg}),
            // -- or --
            // body : JSON.stringify({
            // user : document.getElementById('user').value,
            // ...
            // })
        }).then(
            response => response.text() // .json(), etc.
            // same as function(response) {return response.text();}
        ).then(
            html => {
                if (callback) {
                    callback();
                }
            }
        );

    } catch (e) {
        console.error(e);
        console.error('عدم امکان ارسال لاگ سیستم');
    }
}


function Comment() {
    changeScreen('commentScreen', 'gapContent');

    let html = '';

    let getVisitedPagesForCurrentSite = getDoc().querySelector('#commentScreen');
    if (!getVisitedPagesForCurrentSite) {
        alert('commentScreen is null');
        return;
    }
    getVisitedPagesForCurrentSite.append(createElementFromHTML(`
    <div>
    
    <div>
    <label> نظر
    <textarea  id="commentText" rows="7" style="width: 100%;resize: none"></textarea>
</label>

<button id="commentSubmitter" onclick="commentSubmit()">ارسال نظر</button>
</div>
    </div>
    
    `));

    getDoc().querySelector('#onTheFly').onmousedown = null;
}

function commentSubmit() {


    let commentText = getDoc().querySelector('#commentText');

    let changeScreenBack = getDoc().querySelector('#changeScreenBack');
    let commentSubmitter = getDoc().querySelector('#commentSubmitter');
    if (!changeScreenBack) {
        alert('changeScreenBack is null');
    }

    if (!commentText) {
        alert('commentText is null');
    }

    commentSubmitter.innerText = 'در حال ارسال';
    CommentService(commentText.value, function () {
        commentSubmitter.innerText = 'با موفقیت ارسال شد';
        setTimeout(function () {
            changeScreenBack.click();
        }, 1000)
    });


}

function addDotNewMsgCome() {

    let dot = getDoc().querySelector('#dot');
    if (dot) {
        let MsgCount = dot.querySelector('.MsgCount');

        if (!MsgCount) {
            dot.append(createElementFromHTML(`<i class="MsgCount">..</i>`));
        }
    }
}

function clearDotNewMsgCome() {

    let dot = getDoc().querySelector('#dot');
    if (dot) {
        let MsgCount = dot.querySelector('.MsgCount');

        if (MsgCount) {
            MsgCount.remove();
        }
    }
}


function tagUser() {


    MyCaller.Send("GetTags");


    /* let arr = [];
 
     arr.push({Name: 'مشتری بالقوه', Id: 0});
     arr.push({Name: 'شرکت فلان', Id: 1});
 
     let res = {};
     res.Content = {};
     res.Content.EntityList = arr;
     getTagsCallback(res);*/

}

function getTagsCallback(res) {
    let gapChat = getDoc().querySelector('#gapChat');
    let gaptags = getDoc().querySelector('#gaptags');

    if (gapChat && gaptags) {

        /// اگر در صفحه چت با کاربر باشد
        if (gapChat.style.display === "none") {
            gaptags.click();

        } else {
            // در صفحه اصلی است
        }
    }

    removeShowLoadingChats();

    let chatPanel = getDoc().querySelector('#chatPanel');


    if (!res || !res.Content || !res.Content.EntityList) {
        alert('دیتای خوانده شده برای برچسب ها نال است')
        return;
    }

    if (!chatPanel) {
        return;
    }

    if ($) {
        $(chatPanel).animate({scrollTop: chatPanel.scrollHeight}, "slow");

    } else {
        chatPanel.scrollTo(0, chatPanel.scrollHeight);
    }

    getDoc().querySelector('#onTheFly').onmousedown = null;
    getDoc().querySelector('#onTheFly').onclick = null;

    let lis = '';
    for (let i = 0; i < res.Content.EntityList.length; i++) {
        let rec = res.Content.EntityList[i];
        lis += `<li><input type="checkbox" class="gapTag" name="gapTag" value="${rec.Id}"/>${rec.Name} </li>`;
    }


    let html = `<div class="gapOnTheFlyScreen" id="gapOnTheFlyScreen">

<div>
<button style="float: left" type="button"  onclick="newTagAddScreen(this)">x</button>

<label>
عنوان تگ
<input id="gapTagTitle" />
</label>

<button type="button" onclick="newTagAdd()">ثبت برچسب جدید</button>
</div>
<ul >
${lis}
</ul>


<hr/>
<button id="setCurrentUserToTags" onclick="setCurrentUserToTags()" type="button">ثبت 
<i class="fa fa-thumbs-o-up" aria-hidden="true"></i>
</button>
</div>`;

    let gapOnTheFlyScreen = getDoc().querySelector('#gapOnTheFlyScreen');
    if (gapOnTheFlyScreen) {
        gapOnTheFlyScreen.remove();
    }


    if (chatPanel) {
        chatPanel.append(createElementFromHTML(html));
    }


}


function setCurrentUserToTags() {


    let gapTags = getDoc().querySelectorAll('.gapTag:checked');

    if (!gapTags || gapTags.length == 0) {
        alert('هیچ تگی انتخاب نشده است');
        return;
    }

    let arr = [];
    for (let i = 0; i < gapTags.length; i++) {
        arr.push(gapTags[i].value);
    }

    if (!CurrentUserInfo.targetId) {
        alert('کد کاربر انتخاب نشده است');
        return;
    }


    MyCaller.Send("SetCurrentUserToTags", {
        tags: arr, target: CurrentUserInfo.targetId,
        targetName: CurrentUserInfo.targetName,

    })

    /*  let arr2 = [];
  
      arr2.push({Name: 'مشتری بالقوه', Id: 0});
      arr2.push({Name: 'شرکت فلان', Id: 1});
  
      let res = {};
      res.Content = {};
      res.Content.EntityList = arr2;
  
      userAddedToTagsCallback(res);*/

}

function userAddedToTagsCallback(res) {

    if (!res || !res.Content || !res.Content.EntityList) {
        alert('دیتای خوانده شده برای برچسب های کاربر نال است')
        return;
    }


    //todo:
    let gapCurrUserTags = getDoc().querySelector('#gapCurrUserTags');

    if (!gapCurrUserTags) {
        return;
    }
    gapCurrUserTags.innerHTML = '';

    let html = '';
    for (let i = 0; i < res.Content.EntityList.length; i++) {
        let rec = res.Content.EntityList[i];
        html += `
     <div class="aTagForUser" style="margin:1px">
     <i style="color: #0edbf9" class="fa fa-tag" aria-hidden="true"></i>
     ${rec.Name}
     <span class="aTagForUserButton" onclick="deleteTagFormUserTagsById('${rec.Id}',this)">x</span>
</div>
     `;
    }

    gapCurrUserTags.innerHTML = html;


}

function newTagAdd() {

    let gapTagTitle = getDoc().querySelector('#gapTagTitle');
    if (!gapTagTitle || !gapTagTitle.value) {

        alert('لطفا ابتدا عنوان برچسب را وارد نمایید');
        return;
    }
    showLoadingChats("#chatPanel");

    MyCaller.Send("NewTagAdd", {tagTitle: gapTagTitle.value});


    //newTagAddCallback x 
    // مبجای خط بالا
    // متد getTagsCallback از سمت سرور فراخوانی می شود

    /*
        //todo:
        let arr = [];
    
        arr.push({Name: 'مشتری بالقوه', Id: 0});
        arr.push({Name: 'شرکت فلان', Id: 1});
        arr.push({Name: gapTagTitle.value, Id: 2});
    
        let res = {};
        res.Content = {};
        res.Content.EntityList = arr;
        getTagsCallback(res);*/

}

function newTagAddScreen(THIS) {


    let gapOnTheFlyScreen = getDoc().querySelector('#gapOnTheFlyScreen');
    if (gapOnTheFlyScreen) {

        gapOnTheFlyScreen.remove();
    }
    let onTheFly = getDoc().querySelector("#onTheFly");
    CurrentUserInfo.commonDomManager.bindOntheFlyFocus(onTheFly)


    dragElement(onTheFly)
}


function GetUserAddedToTags(target) {

    MyCaller.Send('GetUserAddedToTags', {target: target});
}

//button
function GetAllTagsForCurrentAdmin() {
    MyCaller.Send('GetAllTagsForCurrentAdmin');

}

function showAddTagForm() {

    getDoc().querySelector('#onTheFly').onmousedown = null;
    getDoc().querySelector('#onTheFly').onclick = null;


    let html = `<div class="gapOnTheFlyScreen" id="gapOnTheFlyScreen">

<div>
<button class="aTagForSelect" style="float: left" type="button"  onclick="newTagAddScreen(this)">x</button>

<label>
عنوان تگ
<input id="gapTagTitle" />
</label>

<button type="button" onclick="newTagAdd()">ثبت برچسب جدید</button>
</div>
<ul >
</ul>

</div>`;

    let gapOnTheFlyScreen = getDoc().querySelector('#gapOnTheFlyScreen');
    if (gapOnTheFlyScreen) {

        gapOnTheFlyScreen.remove();
    }

    let gapContent = getDoc().querySelector('#gapContent');
    let gapChat = getDoc().querySelector('#gapChat');


    if (gapContent) {

        // اگر چت باز نشده باشد
        if (gapChat && gapContent.children && gapContent.children.length > 1) {

            gapContent.insertBefore(createElementFromHTML(html), gapContent.children[1])
        } else {
            gapContent.append(createElementFromHTML(html));

        }
    }
}

function getAllTagsForCurrentAdminCallback(res) {

    if (!res || !res.Content || !res.Content.EntityList) {
        alert('دیتای بازگشتی برای برچسب ها نال است')
        return;
    }
    showAddTagForm();

    /* if (res.Content.EntityList.length === 0) {
         
         
         return;
     }*/
    let toolsPanel = getDoc().querySelector('#toolsPanel');
    if (!toolsPanel) {
        alert('toolsPanel isn ull');
        return;
    }

    let gapTagsPanel = getDoc().querySelector('#gapTagsPanel');
    if (gapTagsPanel) {
        gapTagsPanel.remove();
    }


    let buttons = toolsPanel.querySelectorAll('button');
    if (!buttons || !buttons.length) {
        alert('toolsPanel buttons is null');
        return;
    }
    for (let i = 0; i < buttons.length; i++) {
        buttons[i].style.display = 'none';
    }

    let html = `<div id="gapTagsPanel"><span class="aTagForSelect" onclick="closeTagsPanel(this)">x</span><div style="list-style-type: none">`;

    for (let i = 0; i < res.Content.EntityList.length; i++) {
        let rec = res.Content.EntityList[i];

        if (rec.Id == CurrentUserInfo.selectedTagId) {
            html += `<li style="background-color: #0edbf9" class="aTagForSelect" onclick="getUsersByTagId('${rec.Id}','${rec.Name}')"><i style="    font-size: 14px;" class="fa fa-tags" aria-hidden="true"></i>${rec.Name}
</li>`;
        } else {
            html += `<li ><span class="aTagForSelect" onclick="getUsersByTagId('${rec.Id}','${rec.Name}')"><i  style="    font-size: 14px;" class="fa fa-tags" aria-hidden="true"></i>${rec.Name}</span>
            <span class="aTagForSelect" onclick="deleteTagById('${rec.Id}')">x</span> </li>`;
        }

    }
    html += `<li class="aTagForSelect" onclick="getUsersByTagId(null)"><i style="    font-size: 14px;" class="fa fa-times" aria-hidden="true"></i>
حذف تگ
</li>`;

    html += `</div></div>`;


    toolsPanel.append(createElementFromHTML(html));


}


function deleteTagById(tagId) {

    MyCaller.Send('DeleteTagById', {tagId: tagId});
}

function closeTagsPanel(THIS) {
    let toolsPanel = getDoc().querySelector('#toolsPanel');
    if (!toolsPanel) {
        return;
    }

    let buttons = toolsPanel.querySelectorAll('button');
    if (!buttons || !buttons.length) {
        alert('toolsPanel buttons is null');
        return;
    }
    for (let i = 0; i < buttons.length; i++) {
        buttons[i].style.display = null;
    }
    let gapTagsPanel = getDoc().querySelector('#gapTagsPanel');
    if (gapTagsPanel) {
        gapTagsPanel.remove();

    }


}

function getUsersByTagId(tagId, tagName) {
    CurrentUserInfo.selectedTagId = tagId;


    gapIsOnlyOnlyChange();

    closeTagsPanel();


    let gaptags = getDoc().querySelector('#gaptags');
    if (!gaptags) {
        alert('gaptags not found');
        return;
    }

    if (CurrentUserInfo.selectedTagId) {
        gaptags.style.color = "#5ab7dd"
    } else {
        gaptags.style.color = null;
    }


    let selectedTag = getDoc().querySelector('#selectedTag');
    if (selectedTag) {
        selectedTag.remove();
    }

    if (CurrentUserInfo.selectedTagId && getDoc().querySelector('.gapTopToolsPanel') &&
        getDoc().querySelector('.gapTopToolsPanel').nextSibling) {


        getDoc().querySelector('#gapContent').insertBefore(
            createElementFromHTML(`<span id="selectedTag"> <i class="fa fa-tag" aria-hidden="true"></i>${tagName} <span class="aTagForSelect" onclick="getUsersByTagId(null)">x</span> 
 </span>`),

            getDoc().querySelector('.gapTopToolsPanel').nextSibling
        );
    }


    let gapOnTheFlyScreen = getDoc().querySelector('#gapOnTheFlyScreen');
    if (gapOnTheFlyScreen) {
        gapOnTheFlyScreen.remove();
    }


}

function deleteTagFormUserTagsById(tagId, THIS) {
    THIS.parentNode.remove();
    MyCaller.Send("DeleteTagFormUserTagsById", {tagId: tagId, target: CurrentUserInfo.targetId});
}


/*0128512-128513-128514-128515-128516-128517-128518-128519-128520-128521-128522-128523-128524-128525-128526-128527-128528-128529-128530-128531-128532-128533-128534-128535-128536-128537-128538-128539-128540-128541-128542-128543-128544-128545-128546-128547-128548-128549-128550-128551-128552-128553-128554-128555-128556-128557-128558-128559-128560-128561-128562-128563-128564-128565-128566-128567-128568-128569-128570-128571-128572-128573-128574-128575-128576-128577-128578-128579-128580-128581-128582-128583-128584-128585-128586-128587-128588-128589-128590-128591-8986-8987-9193-9194-9195-9196-9197-9198-9199-9200-9201-9202-9203-9208-9209-9210-9410-9748-9749-9757-9800-9801-9802-9803-9804-9805-9806-9807-9808-9809-9810-9811-9823-9855-9875-9889-9898-9899-9917-9918-9924-9925-9934-9935-9937-9939-9940-9961-9962-9968-9969-9970-9971-9972-9973-9975-9976-9977-9978-9981-9986-9989-9992-9993-9994-9995-9996-9997-9999-10002-10004-10006-10013-10017-10024-10035-10036-10052-10055-10060-10062-10067-10068-10069-10071-10083-10084-10133-10134-10135-10145-10160-10175-10548-10549-11013-11014-11015-11035-11036-11088-11093-12336-12349-12951-12953-126980-127183-127344-127345-127358-127359-127374-127377-127378-127379-127380-127381-127382-127383-127384-127385-127386-127489-127490-127514-127535-127538-127539-127540-127541-127542-127543-127544-127545-127546-127568-127569-127744-127745-127746-127747-127748-127749-127750-127751-127752-127753-127754-127755-127756-127757-127758-127759-127760-127761-127762-127763-127764-127765-127766-127767-127768-127769-127770-127771-127772-127773-127774-127775-127776-127777-127780-127781-127782-127783-127784-127785-127786-127787-127788-127789-127790-127791-127792-127793-127794-127795-127796-127797-127798-127799-127800-127801-127802-127803-127804-127805-127806-127807-127808-127809-127810-127811-127812-127813-127814-127815-127816-127817-127818-127819-127820-127821-127822-127823-127824-127825-127826-127827-127828-127829-127830-127831-127832-127833-127834-127835-127836-127837-127838-127839-127840-127841-127842-127843-127844-127845-127846-127847-127848-127849-127850-127851-127852-127853-127854-127855-127856-127857-127858-127859-127860-127861-127862-127863-127864-127865-127866-127867-127868-127869-127870-127871-127872-127873-127874-127875-127876-127877-127878-127879-127880-127881-127882-127883-127884-127885-127886-127887-127888-127889-127890-127891-127894-127895-127897-127898-127899-127902-127903-127904-127905-127906-127907-127908-127909-127910-127911-127912-127913-127914-127915-127916-127917-127918-127919-127920-127921-127922-127923-127924-127925-127926-127927-127928-127929-127930-127931-127932-127933-127934-127935-127936-127937-127938-127939-127940-127941-127942-127943-127944-127945-127946-127947-127948-127949-127950-127951-127952-127953-127954-127955-127956-127957-127958-127959-127960-127961-127962-127963-127964-127965-127966-127967-127968-127969-127970-127971-127972-127973-127974-127975-127976-127977-127978-127979-127980-127981-127982-127983-127984-127987-127988-127989-127991-127992-127993-127994-127995-127996-127997-127998-127999-128000-128001-128002-128003-128004-128005-128006-128007-128008-128009-128010-128011-128012-128013-128014-128015-128016-128017-128018-128019-128020-128021-128022-128023-128024-128025-128026-128027-128028-128029-128030-128031-128032-128033-128034-128035-128036-128037-128038-128039-128040-128041-128042-128043-128044-128045-128046-128047-128048-128049-128050-128051-128052-128053-128054-128055-128056-128057-128058-128059-128060-128061-128062-128063-128064-128065-128066-128067-128068-128069-128070-128071-128072-128073-128074-128075-128076-128077-128078-128079-128080-128081-128082-128083-128084-128085-128086-128087-128088-128089-128090-128091-128092-128093-128094-128095-128096-128097-128098-128099-128100-128101-128102-128103-128104-128105-128106-128107-128108-128109-128110-128111-128112-128113-128114-128115-128116-128117-128118-128119-128120-128121-128122-128123-128124-128125-128126-128127-128128-128129-128130-128131-128132-128133-128134-128135-128136-128137-128138-128139-128140-128141-128142-128143-128144-128145-128146-128147-128148-128149-128150-128151-128152-128153-128154-128155-128156-128157-128158-128159-128160-128161-128162-128163-128164-128165-128166-128167-128168-128169-128170-128171-128172-128173-128174-128175-128176-128177-128178-128179-128180-128181-128182-128183-128184-128185-128186-128187-128188-128189-128190-128191-128192-128193-128194-128195-128196-128197-128198-128199-128200-128201-128202-128203-128204-128205-128206-128207-128208-128209-128210-128211-128212-128213-128214-128215-128216-128217-128218-128219-128220-128221-128222-128223-128224-128225-128226-128227-128228-128229-128230-128231-128232-128233-128234-128235-128236-128237-128238-128239-128240-128241-128242-128243-128244-128245-128246-128247-128248-128249-128250-128251-128252-128253-128255-128256-128257-128258-128259-128260-128261-128262-128263-128264-128265-128266-128267-128268-128269-128270-128271-128272-128273-128274-128275-128276-128277-128278-128279-128280-128281-128282-128283-128284-128285-128286-128287-128288-128289-128290-128291-128292-128293-128294-128295-128296-128297-128298-128299-128300-128301-128302-128303-128304-128305-128306-128307-128308-128309-128310-128311-128312-128313-128314-128315-128316-128317-128329-128330-128331-128332-128333-128334-128336-128337-128338-128339-128340-128341-128342-128343-128344-128345-128346-128347-128348-128349-128350-128351-128352-128353-128354-128355-128356-128357-128358-128359-128367-128368-128371-128372-128373-128374-128375-128376-128377-128378-128391-128394-128395-128396-128397-128400-128405-128406-128420-128421-128424-128433-128434-128444-128450-128451-128452-128465-128466-128467-128476-128477-128478-128481-128483-128488-128495-128499-128506-128507-128508-128509-128510-128511-128640-128641-128642-128643-128644-128645-128646-128647-128648-128649-128650-128651-128652-128653-128654-128655-128656-128657-128658-128659-128660-128661-128662-128663-128664-128665-128666-128667-128668-128669-128670-128671-128672-128673-128674-128675-128676-128677-128678-128679-128680-128681-128682-128683-128684-128685-128686-128687-128688-128689-128690-128691-128692-128693-128694-128695-128696-128697-128698-128699-128700-128701-128702-128703-128704-128705-128706-128707-128708-128709-128715-128716-128717-128718-128719-128720-128721-128722-128736-128737-128738-128739-128740-128741-128745-128747-128748-128752-128755-128756-128757-128758-128759-128760-128761-128762-129296-129297-129298-129299-129300-129301-129302-129303-129304-129305-129306-129307-129308-129309-129310-129311-129312-129313-129314-129315-129316-129317-129318-129319-129320-129321-129322-129323-129324-129325-129326-129327-129328-129329-129330-129331-129332-129333-129334-129335-129336-129337-129338-129340-129341-129342-129344-129345-129346-129347-129348-129349-129351-129352-129353-129354-129355-129356-129357-129358-129359-129360-129361-129362-129363-129364-129365-129366-129367-129368-129369-129370-129371-129372-129373-129374-129375-129376-129377-129378-129379-129380-129381-129382-129383-129384-129385-129386-129387-129408-129409-129410-129411-129412-129413-129414-129415-129416-129417-129418-129419-129420-129421-129422-129423-129424-129425-129426-129427-129428-129429-129430-129431-129472-129488-129489-129490-129491-129492-129493-129494-129495-129496-129497-129498-129499-129500-129501-129502-129503-129504-129505-129506-129507-129508-129509-12951--*/


function gapStickerOpen(THIS) {

    let stickers =
        `0128512-128513-128514-128515-128516-128517-128518-128519-128520-128521-128522-128523-128524-128525-128526-128527-128528-128529-128530-128531-128532-128533-128534-128535-128536-128537-128538-128539-128540-128541-128542-128543-128544-128545-128546-128547-128548-128549-128550-128551-128552-128553-128554-128555-128556-128557-128558-128559-128560-128561-128562-128563-128564-128565-128566-128567-128568-128569-128570-128571-128572-128573-128574-128575-128576-128577-128578-128579-128580-128581-128582-128583-128584-128585-128586-128587-128588-128589-128590-128591-8986-8987-9193-9194-9195-9196-9197-9198-9199-9200-9201-9202-9203-9208-9209-9210-9410-9748-9749-9757-9800-9801-9802-9803-9804-9805-9806-9807-9808-9809-9810-9811-9823-9855-9875-9889-9898-9899-9917-9918-9924-9925-9934-9935-9937-9939-9940-9961-9962-9968-9969-9970-9971-9972-9973-9975-9976-9977-9978-9981-9986-9989-9992-9993-9994-9995-9996-9997-9999-10002-10004-10006-10013-10017-10024-10035-10036-10052-10055-10060-10062-10067-10068-10069-10071-10083-10084-10133-10134-10135-10145-10160-10175-10548-10549-11013-11014-11015-11035-11036-11088-11093-12336-12349-12951-12953-126980-127183-127344-127345-127358-127359-127374-127377-127378-127379-127380-127381-127382-127383-127384-127385-127386-127489-127490-127514-127535-127538-127539-127540-127541-127542-127543-127544-127545-127546-127568-127569-127744-127745-127746-127747-127748-127749-127750-127751-127752-127753-127754-127755-127756-127757-127758-127759-127760-127761-127762-127763-127764-127765-127766-127767-127768-127769-127770-127771-127772-127773-127774-127775-127776-127777-127780-127781-127782-127783-127784-127785-127786-127787-127788-127789-127790-127791-127792-127793-127794-127795-127796-127797-127798-127799-127800-127801-127802-127803-127804-127805-127806-127807-127808-127809-127810-127811-127812-127813-127814-127815-127816-127817-127818-127819-127820-127821-127822-127823-127824-127825-127826-127827-127828-127829-127830-127831-127832-127833-127834-127835-127836-127837-127838-127839-127840-127841-127842-127843-127844-127845-127846-127847-127848-127849-127850-127851-127852-127853-127854-127855-127856-127857-127858-127859-127860-127861-127862-127863-127864-127865-127866-127867-127868-127869-127870-127871-127872-127873-127874-127875-127876-127877-127878-127879-127880-127881-127882-127883-127884-127885-127886-127887-127888-127889-127890-127891-127894-127895-127897-127898-127899-127902-127903-127904-127905-127906-127907-127908-127909-127910-127911-127912-127913-127914-127915-127916-127917-127918-127919-127920-127921-127922-127923-127924-127925-127926-127927-127928-127929-127930-127931-127932-127933-127934-127935-127936-127937-127938-127939-127940-127941-127942-127943-127944-127945-127946-127947-127948-127949-127950-127951-127952-127953-127954-127955-127956-127957-127958-127959-127960-127961-127962-127963-127964-127965-127966-127967-127968-127969-127970-127971-127972-127973-127974-127975-127976-127977-127978-127979-127980-127981-127982-127983-127984-127987-127988-127989-127991-127992-127993-127994-127995-127996-127997-127998-127999-128000-128001-128002-128003-128004-128005-128006-128007-128008-128009-128010-128011-128012-128013-128014-128015-128016-128017-128018-128019-128020-128021-128022-128023-128024-128025-128026-128027-128028-128029-128030-128031-128032-128033-128034-128035-128036-128037-128038-128039-128040-128041-128042-128043-128044-128045-128046-128047-128048-128049-128050-128051-128052-128053-128054-128055-128056-128057-128058-128059-128060-128061-128062-128063-128064-128065-128066-128067-128068-128069-128070-128071-128072-128073-128074-128075-128076-128077-128078-128079-128080-128081-128082-128083-128084-128085-128086-128087-128088-128089-128090-128091-128092-128093-128094-128095-128096-128097-128098-128099-128100-128101-128102-128103-128104-128105-128106-128107-128108-128109-128110-128111-128112-128113-128114-128115-128116-128117-128118-128119-128120-128121-128122-128123-128124-128125-128126-128127-128128-128129-128130-128131-128132-128133-128134-128135-128136-128137-128138-128139-128140-128141-128142-128143-128144-128145-128146-128147-128148-128149-128150-128151-128152-128153-128154-128155-128156-128157-128158-128159-128160-128161-128162-128163-128164-128165-128166-128167-128168-128169-128170-128171-128172-128173-128174-128175-128176-128177-128178-128179-128180-128181-128182-128183-128184-128185-128186-128187-128188-128189-128190-128191-128192-128193-128194-128195-128196-128197-128198-128199-128200-128201-128202-128203-128204-128205-128206-128207-128208-128209-128210-128211-128212-128213-128214-128215-128216-128217-128218-128219-128220-128221-128222-128223-128224-128225-128226-128227-128228-128229-128230-128231-128232-128233-128234-128235-128236-128237-128238-128239-128240-128241-128242-128243-128244-128245-128246-128247-128248-128249-128250-128251-128252-128253-128255-128256-128257-128258-128259-128260-128261-128262-128263-128264-128265-128266-128267-128268-128269-128270-128271-128272-128273-128274-128275-128276-128277-128278-128279-128280-128281-128282-128283-128284-128285-128286-128287-128288-128289-128290-128291-128292-128293-128294-128295-128296-128297-128298-128299-128300-128301-128302-128303-128304-128305-128306-128307-128308-128309-128310-128311-128312-128313-128314-128315-128316-128317-128329-128330-128331-128332-128333-128334-128336-128337-128338-128339-128340-128341-128342-128343-128344-128345-128346-128347-128348-128349-128350-128351-128352-128353-128354-128355-128356-128357-128358-128359-128367-128368-128371-128372-128373-128374-128375-128376-128377-128378-128391-128394-128395-128396-128397-128400-128405-128406-128420-128421-128424-128433-128434-128444-128450-128451-128452-128465-128466-128467-128476-128477-128478-128481-128483-128488-128495-128499-128506-128507-128508-128509-128510-128511-128640-128641-128642-128643-128644-128645-128646-128647-128648-128649-128650-128651-128652-128653-128654-128655-128656-128657-128658-128659-128660-128661-128662-128663-128664-128665-128666-128667-128668-128669-128670-128671-128672-128673-128674-128675-128676-128677-128678-128679-128680-128681-128682-128683-128684-128685-128686-128687-128688-128689-128690-128691-128692-128693-128694-128695-128696-128697-128698-128699-128700-128701-128702-128703-128704-128705-128706-128707-128708-128709-128715-128716-128717-128718-128719-128720-128721-128722-128736-128737-128738-128739-128740-128741-128745-128747-128748-128752-128755-128756-128757-128758-128759-128760-128761-128762-129296-129297-129298-129299-129300-129301-129302-129303-129304-129305-129306-129307-129308-129309-129310-129311-129312-129313-129314-129315-129316-129317-129318-129319-129320-129321-129322-129323-129324-129325-129326-129327-129328-129329-129330-129331-129332-129333-129334-129335-129336-129337-129338-129340-129341-129342-129344-129345-129346-129347-129348-129349-129351-129352-129353-129354-129355-129356-129357-129358-129359-129360-129361-129362-129363-129364-129365-129366-129367-129368-129369-129370-129371-129372-129373-129374-129375-129376-129377-129378-129379-129380-129381-129382-129383-129384-129385-129386-129387-129408-129409-129410-129411-129412-129413-129414-129415-129416-129417-129418-129419-129420-129421-129422-129423-129424-129425-129426-129427-129428-129429-129430-129431-129472-129488-129489-129490-129491-129492-129493-129494-129495-129496-129497-129498-129499-129500-129501-129502-129503-129504-129505-129506-129507-129508-129509-12951`;

    let gapStickers = getDoc().querySelector('#gapStickers');
    if (gapStickers) {

        toggle(getDoc().querySelector('.gapStickerHolder'))
        setTimeout(function (){
            gapStickers.remove();
        },500)

       

        return;
    }


    let stickerArrays = stickers.split('-');

    let temp = '';
    let j = 0;
    for (let i = 0; i < stickerArrays.length; i++) {

        temp += `<div class="gapEmoji  " onclick="addEmoji('&#${stickerArrays[i]}')" style="display: inline; font-size: 24px !important;overflow: hidden">&#${stickerArrays[i]}</div>`;


    }


    let html = `

            <ul class="gapStickerHolder adp-hide" id="gapStickers" style="overflow: auto">

${temp}

</ul>
`;

    let chatPanel = getDoc().querySelector('#chatPanel');

    if (!chatPanel) {
        alert('chatPanel is null');
        return;
    }

    chatPanel.append(createElementFromHTML(html));

    
    
    toggle(getDoc().querySelector('.gapStickerHolder'))
}


function addEmoji(emoji) {

    let gapChatInput = getDoc().querySelector('#gapChatInput');

    if (!gapChatInput) {
        alert('gapChatInput is null');
        return;
    }

    gapChatInput.value = gapChatInput.value + '' + emoji;


}

/*TYPING*/
function adminStartTypingCallback() {
    getDoc().querySelector('#gapwhileWriting').style.display = null;
}


function adminStopTypingCallback() {
    getDoc().querySelector('#gapwhileWriting').style.display = 'none';
}


let CustomerStartTypingSent = false;

function bindIsTyping() {
    var searchTimeout;
    getDoc().querySelector('#gapChatInput').onchange = function () {
        if (searchTimeout !== undefined) clearTimeout(searchTimeout);

        let strg = getDoc().querySelector('#gapChatInput').value;

        if (strg || strg.trim() === '') {
            //   if (strg || strg.trim() === '' && !CustomerStartTypingSent) {
            MyCaller.Send('CustomerStartTyping', {text: strg});
            CustomerStartTypingSent = true;

        } else {

            CustomerStartTypingSent = false;

            callServerScript();
        }
        // searchTimeout = setTimeout(callServerScript, 1000);
    };

}

function callServerScript() {
    MyCaller.Send('CustomerStopTyping');

}

/*TYPING END*/


function gapChatSubmit() {
    let gapStickers = getDoc().querySelector('#gapStickers');
    if (gapStickers) {
        gapStickers.remove();

        return;
    }
    let element = getDoc().querySelector('#gapChatInput');

    element.style.height = "92%";


    getDoc().querySelector('#gpwaterMark').style.display = null;


}


function getSocialChannelsInfoCallback(res) {

    let helpDeskApi = res.Content.helpDeskApi;
    if (helpDeskApi && helpDeskApi.indexOf('?') >= 0) {

        helpDeskApi += '&websiteToken=' + websiteToken
    } else {
        helpDeskApi += '?websiteToken=' + websiteToken
    }


    const info = {
        email: res.Content.email,
        telegram: res.Content.telegram,
        whatsapp: res.Content.whatsapp,
        helpDeskApi: helpDeskApi,
        helpDeskUrlLink: res.Content.helpDeskUrlLink,
    };

    //تکراری نزند
    if (getDoc().querySelector('#gapSocialCirlces')) {
        return;
    }

    let html = '<div id="gapSocialCirlces" class="gapSocialCirlces" style="text-align: center">';


    if (info.email) {
        html += `<a target="_blank" href = "mailto:${info.email}" class="gap_online_admin" aria-label="ادامه در ایمیل" data-microtip-position="left" role="tooltip">
                                    <i class="fa fa-envelope-o" aria-hidden="true"></i>
                                </a>`;
    }
    if (info.telegram) {
        html += `<a target="_blank" href = "${info.telegram}" class="gap_online_admin" aria-label="ادامه در تلگرام" data-microtip-position="left" role="tooltip">
                                  <i class="fa fa-telegram" aria-hidden="true"></i>

                                </a>`;
    }

    if (info.whatsapp) {
        html += `<a target="_blank" href = "${info.whatsapp}" class="gap_online_admin" aria-label="ادامه در واتساپ" data-microtip-position="left" role="tooltip">
                            <i class="fa fa-whatsapp" aria-hidden="true"></i>
                                </a>`;
    }


    html += `<a  onclick="vc_CustomerCallInit()" class="gap_online_admin" aria-label="برقراری تماس صوتی" 
data-microtip-position="left" role="tooltip">
                            <i class="fa fa-phone" aria-hidden="true"></i>
                                </a>`;

    /* html += `<a   class="gap_online_admin VIDEOrecord" aria-label="نمایش برخط مانیتور" data-microtip-position="left" role="tooltip">
                             <i style="right: 23% !important;" class="fa fa-television " aria-hidden="true"></i>
                                 </a>`;*/

    if (info.helpDeskUrlLink) {

        if (info.helpDeskApi) {

            html += `<a href="#"  onclick="openHelpCenter('${info.helpDeskApi}')"  class="gap_online_admin" aria-label="جستجو در HelpDesk" data-microtip-position="left" role="tooltip">
                           <i class="fa fa-search" aria-hidden="true"></i>

                                </a>`;
        } else {

            html += `<a target="_blank" href = "${info.helpDeskUrlLink}"  class="gap_online_admin" aria-label="جستجو در HelpDesk" data-microtip-position="left" role="tooltip">
                           <i class="fa fa-search" aria-hidden="true"></i>

                                </a>`;
        }

    }

    html += '</div>'


    getDoc().querySelector('#gapAfterChat').parentNode.append(createElementFromHTML(html));

    // VOICE_CALL_INIT();
}

let gapHelpDeskInitialized = false;

function openHelpCenter(api) {
    getDoc().querySelector('#gapHelpDeskModal').style.display = null;
    getDoc().querySelector('#gapHelpDeskBlackBg').style.display = null;


    if (!gapHelpDeskInitialized) {
        gapHelpDeskSearchChanged(getDoc().querySelector('#gapHelpDeskSearch'), api)
    }


}

function closeGapHelpDeskModal() {
    getDoc().querySelector('#gapHelpDeskModal').style.display = 'none';
    getDoc().querySelector('#gapHelpDeskBlackBg').style.display = 'none';
    getDoc().querySelector('.gapHelpDeskLinkOpener').style.display = 'none';

    if (getDoc().querySelector('#gapModal'))
        getDoc().querySelector('#gapModal').style.display = 'none';

}

function closeGapModal() {
    getDoc().querySelector('#gapModal').style.display = 'none';
    getDoc().querySelector('#gapHelpDeskBlackBg').style.display = 'none';

}


let gapDeskApiCaller = function (url, data, callback) {
    try {
        xhttp = new XMLHttpRequest();


        /// درخواست کد html دایره ای شکل در معلق
        xhttp.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                callback(this.responseText)
            }

        };

        // درخواست html ها
        xhttp.open("POST", url, true);
        xhttp.setRequestHeader('Content-type', 'application/json')

        xhttp.send(JSON.stringify(data));
    } catch (e) {
        console.error(e);
        console.error('خطا در درخواست اطلاعات HelpDesk');

    }
}

function gapHelpDeskSearchChanged(THIS, api) {
    getDoc().querySelector('.gapHelpDeskLinks').innerHTML = '<p style="text-align: center">در حال خواندن اطلاعات</p>';
    let searchTerm = THIS.value;
    gapDeskApiCaller(api, {searchTerm: searchTerm, websiteToken: websiteToken}, function (responseText) {

        if (!responseText) {

            gapHelpDeskShowEmpty();
            return;
        }
        var data = JSON.parse(responseText);
        if (!data) {
            gapHelpDeskShowEmpty();
            return;
        }

        if (!data.array || !data.array.length) {
            gapHelpDeskShowEmpty(data.emptyText);
            return;
        }

        gapHelpDeskShowArrayLinks(data.array);

    });
}


function gapHelpDeskShowArrayLinks(res) {

    let html = '';
    for (let i = 0; i < res.length; i++) {

        html += ` <li onclick="this.children[0].click()"  >
            <a onclick="gapHelpDeskLinkOpener('${res[i].link}')" href="#"  rel="noopener noreferrer" data-active="false" class="crisp-joczs8 crisp-1fmeyoi" data-cshid="dbb46ff6-95dc-ec00-d1bc-ed3f16deaaa5">
                <span >${res[i].title}</span>
            </a>
        </li>`
    }

    getDoc().querySelector('.gapHelpDeskLinks').innerHTML = html;

}

function gapHelpDeskShowEmpty(msg) {
    getDoc().querySelector('.gapHelpDeskLinks').innerHTML = msg ? msg : "<p  style=\"text-align: center\">هیچ اطلاعاتی دریافت نگردید</p>";
}

function gapHelpDeskLinkOpener(link) {
    getDoc().querySelector('.gapHelpDeskModal').style.display = 'none';
    getDoc().querySelector('.gapHelpDeskLinkOpener').style.display = null;

    getDoc().querySelector('#gapHelpDeskBlackBg').style.display = null;

    getDoc().querySelector('.gapHelpDeskLinkOpenerIframe').src = link;
    getDoc().querySelector('.gapHelpDeskLinkOpenerLink').href = link;


}


function closegapHelpDeskLinkOpener() {
    getDoc().querySelector('#gapHelpDeskModal').style.display = 'none';
    getDoc().querySelector('#gapHelpDeskBlackBg').style.display = 'none';
    getDoc().querySelector('.gapHelpDeskLinkOpener').style.display = 'none';

}

function textAreaAdjust(element) {
    element.style.height = "1px";
    element.style.height = (25 + element.scrollHeight) + "px";

    getDoc().querySelector('#gpwaterMark').style.display = 'none';

}


function makeCheckOrRadio(element, checkbox, el, chatId) {
    return `<div style="display: flex">  <input formelementid="${el.Id}"
  value="${element.Value}"  style="margin-top: 4px;" type="${checkbox}" name="${el.FieldName}"  elid="${el.Id}" chatid="${chatId}"/>
  <label style="    margin-right: 6px;">${element.FieldName}</label></div>`;

}

/*  public string Help { get; set; }

        public string FieldName { get; set; }
        public Form Form { get; set; }


        public string Name { get; set; }
        public int FormId { get; set; }*/
function adminSendFormToCustomerCallback(res) {
    if (!res || !res.Content) {
        alert('adminSendFormToCustomerCallback res is null');
    }

    let form = res.Content.form;
    let chatId = res.Content.chatId;
    let chat = res.Content.chat;

    let Name = form.Name;
    let Elements = form.Elements;
    let Message = form.Message;


    /*
    * 
    * make form:
    * 
    * */

    let html = '';
    html = `<form onsubmit="gapFormSubmit(this,event,'${chatId}')">
<h5>${Name}</h5>

<p>${Message}</p>

<div>
`

    html += `<input   name="formId" value="${form.Id}" type="hidden"/>`;
    html += `<input  name="chatId" value="${chatId}" type="hidden"/>`;


    for (let i = 0; i < Elements.length; i++) {
        let el = Elements[i];


        html += `<div style="display: grid"> <label>${el.Name}</label>`;

        if (el.type == 'input')
            html += `<input formelementid="${el.Id}" style="color: black"  name="${el.FieldName}"  type="text"/>`;


        if (el.type == 'textarea') {
            html += `  <textarea  formelementid="${el.Id}" style="color: black"  name="${el.FieldName}"  elid="${el.Id}"  chatid="${chatId}"> </textarea>`;

        }
        /*SubElementsTemp*/
        if (el.type == 'checkbox') {

            for (let j = 0; j < el.SubElements.length; j++) {

                let element = el.SubElements[j];
                html += makeCheckOrRadio(element, 'checkbox', el, chatId)
            }
        }


        if (el.type == 'radio') {
            for (let j = 0; j < el.SubElements.length; j++) {

                let element = el.SubElements[j];
                html += makeCheckOrRadio(element, 'radio', el, chatId)
            }

        }


        if (el.Help) {
            html += `<small>${el.Help}</small>`
        }

        html += '</div>'

    }
    html += '<hr/><button type="submit" class="gapMainColor" ' +
        'style="padding: 5px;border-radius: 3px 3px 3px 3px ;border:none" >ارسال</button></div>'

    html += `</form><br/>`

    //html=  CurrentUserInfo.commonDomManager.makeChatDom(html, false, null, 0, Math.random(), null);


    let formParent = getDoc().querySelector(`#gap_formId_${chatId}_${form.Id}`);

    if (!formParent) {

        html = CurrentUserInfo.commonDomManager.makeChatDom(html, false, null, 0,
            Math.random(), 'همین الان', {formId: form.Id, Id: chatId});


        getDoc().querySelector('#chatPanel').append(createElementFromHTML(html));

    } else {

        formParent.innerHTML = '';
        formParent.append(createElementFromHTML(html));

    }

    /*
    * 
    * end : make form:
    * 
    * */


}


function customerGetFormSingleCallback(res) {
    adminSendFormToCustomerCallback(res);
}


function saveFormDataCallback(res) {


    /*form.AfterMessage,
                    formId,
                    chatId*/

    let formId = res.Content.formId;
    let AfterMessage = res.Content.AfterMessage;
    let chatId = res.Content.chatId;

    let formParent = getDoc().querySelector(`#gap_formId_${chatId}_${formId}`);

    if (!formParent) {
        alert('فرم یافت نشد !');
        return;
    }


    formParent.innerHTML = `<p>${AfterMessage}</p>`;


}

function gapFormSubmit(THIS, e, chatId) {
    //debugger;
    e.preventDefault();


    let data = seriJson(THIS);

    console.log(data)

    if (!data.requestArray['formId']) {
        alert('کد فرم یافت نشد');
        return;
    }

    if (!data.arr || data.arr.length == 0) {
        alert('هیچ دیتایی برای ارسال فرم نیست');
        return;
    }

    MyCaller.Send('SaveFormData', {
        formId: data.requestArray['formId'],
        elements: data.arr, chatId: chatId
    });


    return false;

}

function seriJson(form) {
    let requestArray = {};
    let arr = []
    form.querySelectorAll('[name]').forEach((elem) => {
        requestArray[elem.name] = elem.value;


        // این دو مربوط به المنت های فرم نیستند و نباید به سرور ارسال شوند
        if (elem.name == 'formId' || elem.name == 'chatId') {

        } else {

            if (elem.getAttribute("type") == 'checkbox' || elem.getAttribute("type") == 'radio') {
                if (!elem.checked) {

                } else {
                    let Id = elem.getAttribute("formelementid")
                    arr.push({FieldName: elem.name, Value: elem.value, Id: Id ? Id : 0});
                }
            } else {
                let Id = elem.getAttribute("formelementid")
                arr.push({FieldName: elem.name, Value: elem.value, Id: Id ? Id : 0});
            }

        }


    });
    return {requestArray, arr};
}


function startCapture() {
    let captureStream = null;


    document.body.append(createElementFromHTML(`
    <video id="sdlfjksdlkfj" autoplay>
    
    
    </video>
    
    `));

    videoElem = document.getElementById('sdlfjksdlkfj')

    var displayMediaOptions = {
        video: {
            cursor: "always"
        },
        audio: false
    };

    return navigator.mediaDevices.getDisplayMedia(displayMediaOptions)
        .then(res => {
            //debugger;
            videoElem.srcObject = res;


            var mediaRecorder = new MediaRecorder(res);


            mediaRecorder.ondataavailable = function (e) {
                //chunks.push(e.data);
                //debugger;
                console.log(e)
            }

            mediaRecorder.start();

            return res;
        })
        .catch(err => {
            console.error("Error:" + err);
            return null;
        });
}


function startCaptureVideo() {
    startCapture().then(res => {

        console.log(res)
        dumpOptionsInfo();
    })
}

function stopCapture(evt) {
    let tracks = videoElem.srcObject.getTracks();

    tracks.forEach(track => track.stop());
    videoElem.srcObject = null;
}

function dumpOptionsInfo() {
    const videoTrack = videoElem.srcObject.getVideoTracks()[0];

    console.info("Track settings:");
    console.info(JSON.stringify(videoTrack.getSettings(), null, 2));
    console.info("Track constraints:");
    console.info(JSON.stringify(videoTrack.getConstraints(), null, 2));
}


/*VIDEO_CALL :*/
let videoElem;
let mediaSource;
let chunks = [];
let mediaRecorder;

let captureStream;

function configStream() {
    /*   let blob = new Blob(buffer, { 'type' : 'video/webm' });
                   let audioURL = window.URL.createObjectURL(blob);
                   */


    videoElem.captureStream();
}

/*VOICE CALL END*/
function VIDEO_CALL_STOP(THIS, msg) {
    try {
        mediaRecorder.stop();

    } catch (e) {
        //ingore
    }
    console.log(mediaRecorder.state);
    console.log("recorder stopped");

    THIS.parentNode.remove();

    captureStream.getTracks()
        .forEach(track => track.stop())


    MyCaller.Send('ScreenRecordCustomerClose', {
        myAccountId: CurrentUserInfo.targetId,
        msg: msg
    });
}

function setRecordVideo(chatId) {
    /*remove prev videl*/
    let gapOnlineVideo = document.body.querySelector('#gapOnlineVideo');
    if (gapOnlineVideo) {
        gapOnlineVideo.remove();
    }

    let html = `
                
                <div style="position: fixed;
    /* width: 200px; */
    /* height: 200px; */
    top: 55vh;
    left: 10vh;
    background-color: #f8f9fa;
    padding: 20px;
}">
                <p>صفحه مانیتور شما در حال نمایش برای پشتیبانی است</p>
                
                <video autoplay id="gapOnlineVideo"  width="200" height="200">
                
</video>

  <button class="gapMainColor " style="background-color: lightgrey;padding: 5px;border-radius: 3px 3px 3px 3px ;border:none"
         onclick="VIDEO_CALL_STOP(this,'توسط کاربر بسته شد');">لغو نمایش</button>
         
          <button class="gapMainColor GAP_VIDEO_CALL_STOP" style="display: none"
         onclick="VIDEO_CALL_STOP(this,'توسط پشتیبانی بسته شد');"></button>
</div>`;

    /* html = CurrentUserInfo.commonDomManager.makeChatDom(html, true, null, 0,
         651, 'همین الان', {Id: 651561});*/

    document.body.append(createElementFromHTML(html));
}

function VIDEO_CALL_INIT(THIS, callback, chatId) {


    if (navigator.mediaDevices.getDisplayMedia) {
        console.log('getUserMedia supported.');

        var constraints = {
            video: {
                cursor: "always"
            },
            audio: false
        };


        let onSuccess = function (stream) {

            setRecordVideo(chatId);

            let gapOnlineVideo = document.body.querySelector('#gapOnlineVideo');

            captureStream = stream;
            debugger;

            gapOnlineVideo.srcObject = stream;

            callback(THIS, 'اجازه دسترسی داده شد');
            //debugger;
            mediaRecorder = new MediaRecorder(stream);
            mediaRecorder.start(3000);


            //videoElem.srcObject=stream;


            console.log(mediaRecorder.state);


            /* mediaRecorder.onstop = function (e) {
                 console.log("data available after MediaRecorder.stop() called.");
 
                 const clipName = prompt('Enter a name for your sound clip?', 'My unnamed clip');
 
                 const clipContainer = document.createElement('article');
                 const clipLabel = document.createElement('p');
                 const audio = document.createElement('audio');
                 const deleteButton = document.createElement('button');
 
                 clipContainer.classList.add('clip');
                 audio.setAttribute('controls', '');
                 deleteButton.textContent = 'Delete';
                 deleteButton.className = 'delete';
 
                 if (clipName === null) {
                     clipLabel.textContent = 'My unnamed clip';
                 } else {
                     clipLabel.textContent = clipName;
                 }
 
                 clipContainer.appendChild(audio);
                 clipContainer.appendChild(clipLabel);
                 clipContainer.appendChild(deleteButton);
                 soundClips.appendChild(clipContainer);
 
                 audio.controls = true;
                 const blob = new Blob(chunks, {'type': 'audio/ogg; codecs=opus'});
                 chunks = [];
                 const audioURL = window.URL.createObjectURL(blob);
                 audio.src = audioURL;
                 console.log("recorder stopped");
 
                 deleteButton.onclick = function (e) {
                     let evtTgt = e.target;
                     evtTgt.parentNode.parentNode.removeChild(evtTgt.parentNode);
                 }
 
                 clipLabel.onclick = function () {
                     const existingName = clipLabel.textContent;
                     const newClipName = prompt('Enter a new name for your sound clip?');
                     if (newClipName === null) {
                         clipLabel.textContent = existingName;
                     } else {
                         clipLabel.textContent = newClipName;
                     }
                 }
             }
 */

            mediaRecorder.ondataavailable = function (e) {

                new Promise(resolve => {

                    const fileReader = new FileReader();
                    fileReader.addEventListener("loadend", () => {
//debugger
                        // videoElem.src=fileReader.result;
                        /*  var dataUrl = fileReader.result;
                          var base64 = dataUrl.split(',')[1];*/

                        resolve(fileReader.result)
                    });

                    fileReader.readAsBinaryString(e.data);

                }).then(function (buffer) {


                    MyCaller.Send('ScreenRecordSave', {targetMyAccountId: CurrentUserInfo.targetId, buffer})
                })
                chunks.push(e.data);
            }
        }

        let onError = function (err) {
            callback(THIS, 'اجازه دسترسی داده نشد', err);
            console.error('The following error occured: ' + err);


        }

        navigator.mediaDevices.getDisplayMedia(constraints).then(onSuccess, onError);

    } else {
        callback(THIS, 'این امکان در مرورگر شما پشتیبانی نمی شود');
        alert('این امکان در مرورگر شما پشتیبانی نمی شود')
        console.error('getUserMedia not supported on your browser!');
    }


    window.onresize = function () {
        //canvas.width = mainSection.offsetWidth;
    }

    window.onresize();


}

/*VOICE CALL:*/

function VOICE_CALL_INIT() {

    const record = getDoc().querySelector('.record');
    const stop = getDoc().querySelector('.stop');
    const soundClips = getDoc().querySelector('.sound-clips');
    const canvas = getDoc().querySelector('.visualizer');
    const mainSection = getDoc().querySelector('.main-controls');

// disable stop button while not recording

    stop.disabled = true;

// visualiser setup - create web audio api context and canvas

    let audioCtx;
    const canvasCtx = canvas.getContext("2d");

//main block for doing the audio recording

    if (navigator.mediaDevices.getUserMedia) {
        console.log('getUserMedia supported.');

        const constraints = {audio: true};
        let chunks = [];

        let onSuccess = function (stream) {
            const mediaRecorder = new MediaRecorder(stream);

            visualize(stream);

            record.onclick = function () {
                mediaRecorder.start();
                console.log(mediaRecorder.state);
                console.log("recorder started");
                record.style.background = "red";

                stop.disabled = false;
                record.disabled = true;
            }

            stop.onclick = function () {
                mediaRecorder.stop();
                console.log(mediaRecorder.state);
                console.log("recorder stopped");
                record.style.background = "";
                record.style.color = "";
                // mediaRecorder.requestData();

                stop.disabled = true;
                record.disabled = false;
            }

            mediaRecorder.onstop = function (e) {
                console.log("data available after MediaRecorder.stop() called.");

                const clipName = prompt('Enter a name for your sound clip?', 'My unnamed clip');

                const clipContainer = document.createElement('article');
                const clipLabel = document.createElement('p');
                const audio = document.createElement('audio');
                const deleteButton = document.createElement('button');

                clipContainer.classList.add('clip');
                audio.setAttribute('controls', '');
                deleteButton.textContent = 'Delete';
                deleteButton.className = 'delete';

                if (clipName === null) {
                    clipLabel.textContent = 'My unnamed clip';
                } else {
                    clipLabel.textContent = clipName;
                }

                clipContainer.appendChild(audio);
                clipContainer.appendChild(clipLabel);
                clipContainer.appendChild(deleteButton);
                soundClips.appendChild(clipContainer);

                audio.controls = true;
                const blob = new Blob(chunks, {'type': 'audio/ogg; codecs=opus'});
                chunks = [];
                const audioURL = window.URL.createObjectURL(blob);
                audio.src = audioURL;
                console.log("recorder stopped");

                deleteButton.onclick = function (e) {
                    let evtTgt = e.target;
                    evtTgt.parentNode.parentNode.removeChild(evtTgt.parentNode);
                }

                clipLabel.onclick = function () {
                    const existingName = clipLabel.textContent;
                    const newClipName = prompt('Enter a new name for your sound clip?');
                    if (newClipName === null) {
                        clipLabel.textContent = existingName;
                    } else {
                        clipLabel.textContent = newClipName;
                    }
                }
            }

            mediaRecorder.ondataavailable = function (e) {
                chunks.push(e.data);
            }
        }

        let onError = function (err) {
            console.log('The following error occured: ' + err);
        }

        navigator.mediaDevices.getUserMedia(constraints).then(onSuccess, onError);

    } else {
        console.log('getUserMedia not supported on your browser!');
    }

    function visualize(stream) {
        if (!audioCtx) {
            audioCtx = new AudioContext();
        }

        const source = audioCtx.createMediaStreamSource(stream);

        const analyser = audioCtx.createAnalyser();
        analyser.fftSize = 2048;
        const bufferLength = analyser.frequencyBinCount;
        const dataArray = new Uint8Array(bufferLength);

        source.connect(analyser);
        //analyser.connect(audioCtx.destination);

        draw()

        function draw() {
            const WIDTH = canvas.width
            const HEIGHT = canvas.height;

            requestAnimationFrame(draw);

            analyser.getByteTimeDomainData(dataArray);

            canvasCtx.fillStyle = 'rgb(200, 200, 200)';
            canvasCtx.fillRect(0, 0, WIDTH, HEIGHT);

            canvasCtx.lineWidth = 2;
            canvasCtx.strokeStyle = 'rgb(0, 0, 0)';

            canvasCtx.beginPath();

            let sliceWidth = WIDTH * 1.0 / bufferLength;
            let x = 0;


            for (let i = 0; i < bufferLength; i++) {

                let v = dataArray[i] / 128.0;
                let y = v * HEIGHT / 2;

                if (i === 0) {
                    canvasCtx.moveTo(x, y);
                } else {
                    canvasCtx.lineTo(x, y);
                }

                x += sliceWidth;
            }

            canvasCtx.lineTo(canvas.width, canvas.height / 2);
            canvasCtx.stroke();

        }
    }

    window.onresize = function () {
        canvas.width = mainSection.offsetWidth;
    }

    window.onresize();


}

const b64toBlob = (b64Data, contentType = '', sliceSize = 512) => {
    const byteCharacters = atob(b64Data);
    const byteArrays = [];

    for (let offset = 0; offset < byteCharacters.length; offset += sliceSize) {
        const slice = byteCharacters.slice(offset, offset + sliceSize);

        const byteNumbers = new Array(slice.length);
        for (let i = 0; i < slice.length; i++) {
            byteNumbers[i] = slice.charCodeAt(i);
        }

        const byteArray = new Uint8Array(byteNumbers);
        byteArrays.push(byteArray);
    }

    const blob = new Blob(byteArrays, {type: contentType});
    return blob;
}

/*VOICE CALL END*/


function screenRecordAccessRequestCallback(res, isReturn) {


    let html = `
    
    <p>درخواست اجازه دسترسی به دیدن صفحه مانیتور شما از طرف ادمین ارسال شده است</p>
    
   <div>
    <button class="gapMainColor" style="padding: 5px;border-radius: 3px 3px 3px 3px ;border:none"
         onclick="VIDEO_CALL_INIT(this,function (THIS,msg,err){
             setMsg(THIS,msg,err,false,${res.Content.Id})
         },${res.Content.Id});">می پذیرم</button>
    <button  class="gapMainColor" style="padding: 5px;border-radius: 3px 3px 3px 3px ;border:none" 
    onclick="setMsg(this,'اجازه دسترسی داده نشد',null,false,${res.Content.Id})">اجازه نمی دهم</button>
</div>
    `;

    html = CurrentUserInfo.commonDomManager.makeChatDom(html, false, null, 0,
        res.Content.UniqId, 'همین الان', {Id: res.Content.Id});

    if (isReturn)
        return html;

    getDoc().querySelector('#chatPanel').append(createElementFromHTML(html));

}

function setMsg(THIS, msg, err, isAccepted, chatId, callback) {

    if (THIS.parentNode && THIS.parentNode.previousSibling && THIS.parentNode.previousSibling.previousSibling)
        THIS.parentNode.previousSibling.previousSibling.innerText = msg;

    THIS.parentNode.style.display = "none";

    getDoc().querySelector('#gapChatInput').value = msg;

    CurrentUserInfo.commonDomManager.enterNewText();

    CurrentUserInfo.plugin.sendNewText();

    if (err) {
        msg;
    }

    // در دو جا استفاده شده است در موقع بستن لازم نیست این متد فراخوانی شود
    if (!callback && THIS.parentNode && THIS.parentNode.previousSibling && THIS.parentNode.previousSibling.previousSibling)
        MyCaller.Send('SetScreenRecordAccessRequestIsAccepted',
            {
                msg: msg, err: null, isAccepted: isAccepted, myAccountId: CurrentUserInfo.targetId,
                chatId: chatId
            })

    if (callback)
        callback(msg, isAccepted, CurrentUserInfo.targetId, chatId)

}

function screenRecordAdminCloseCallback(res) {


    let GAP_VIDEO_CALL_STOP = document.querySelector('.GAP_VIDEO_CALL_STOP');
    if (GAP_VIDEO_CALL_STOP) {
        GAP_VIDEO_CALL_STOP.click();
    }

}


function onTabExitBind(callback) {

    window.addEventListener('onbeforeunload', callback)
    window.addEventListener('onpopstate', callback)
}


function screenRecordAdminShareRequestCallback(res) {
    videoData = [];


    let html = `
    
    <p>پشتیبانی در حال ارسال تصویر مانیتور خود به شما می باشد</p>
    
   <div>
    
    <video style="width: 100%; height: 100%" muted="muted" controls id="screenRecordAdminShareRequestCallbackVideo_${res.Content.Id}" autoplay>
    
    
    </video>
    
    
    
    
  
     <button  class="gapMainColor" style="padding: 5px;border-radius: 3px 3px 3px 3px ;border:none" 
    onclick="setMsg(this,'کاربر اسکین پشتیبانی را بست',null,false,${res.Content.Id})">بستن</button>
    
    <button  class="gapMainColor" style="padding: 5px;border-radius: 3px 3px 3px 3px ;border:none" 
    onclick="fullScreen('screenRecordAdminShareRequestCallbackVideo_${res.Content.Id}')">بزرگنمایی</button>
</div>
    `;

    html = CurrentUserInfo.commonDomManager.makeChatDom(html, false, null, 0,
        res.Content.UniqId, 'همین الان', {Id: res.Content.Id});

    getDoc().querySelector('#chatPanel').append(createElementFromHTML(html));

}

function fullScreen(id) {
    debugger;
    getDoc().querySelector('#gapModal').style.display = null;
    getDoc().querySelector('#gapHelpDeskBlackBg').style.display = null;


    getDoc().querySelector('#gapModalBody')
        .innerHTML = `<video style="width: 100%; height: 100%" muted="muted" 
controls id="${id}_big" autoplay>
    
    </video>`;
}

let videoData = [];

function screenRecordAdminShareCallback(res) {

    if (!res || !res.Content || !res.Content.buffer) {
        alert("ویدئو برگشتی فرمت صحیح ندارد");
        return;
    }


    let base64 = btoa(res.Content.buffer)
    let blob = b64toBlob(base64, 'video/webm');

    const videoElem = getDoc()
        .querySelector(`#screenRecordAdminShareRequestCallbackVideo_${res.Content.chatId}`)


    const videoElemBig = getDoc()
        .querySelector(`#screenRecordAdminShareRequestCallbackVideo_${res.Content.chatId}_big`)

    videoData.push(blob);

    let wholeVideoBlob = new Blob(videoData, {'type': 'video/webm'})

    let blobUrl = URL.createObjectURL(wholeVideoBlob);

    // برای اولین بار نال است
    if (videoElem) {
        // blobUrl= URL.revokeObjectURL(blobUrl)

        let lastTime = videoElem.currentTime ? videoElem.currentTime : 0;

        videoElem.src = blobUrl;

        if (videoElemBig)
            videoElemBig.src = blobUrl;
        videoElem.load()

        videoElem.currentTime = lastTime;
        videoElem.play();
    }
    return blobUrl;

}


function VC_AdminCallInitCallback(res) {


    let html = `
    
    <p>تماس صوتی از پشتیبانی</p>
    
   <div>
    
    <voice style="width: 100%; height: 100%" muted="muted" controls id="VC_AdminCallInitCallback_${res.Content.Id}" autoplay>
    
    
    </voice>
  
     <button  class="gapMainColor" style="padding: 5px;border-radius: 3px 3px 3px 3px ;border:none" 
    onclick="vc_voiceCallReject(this,'${res.Content.Id}')">رد </button>
    
    <button  class="gapMainColor" style="padding: 5px;border-radius: 3px 3px 3px 3px ;border:none" 
    onclick="vc_voiceCallAccept(this,'${res.Content.Id}')">جواب</button>
</div>
    `;

    html = CurrentUserInfo.commonDomManager.makeChatDom(html, false, null, 0,
        res.Content.UniqId, 'همین الان', {Id: res.Content.Id});

    getDoc().querySelector('#chatPanel').append(createElementFromHTML(html));

}

function vc_voiceCallReject(THIS, chatId) {


    setMsg(THIS, 'تماس توسط کاربر رد شد', null, true, chatId,
        function (msg, isAccepted, targetId, chatId) {
            MyCaller.Send('VC_CustomerIsAcceptOrReject',
                {
                    msg: msg, err: null, isAccepted: false, myAccountId: targetId,
                    chatId: chatId
                })
        });
}

function vc_voiceCallAccept(THIS, chatId) {


    setMsg(THIS, 'کاربر جواب داد', null, true, chatId,
        function (msg, isAccepted, targetId, chatId) {
            MyCaller.Send('VC_CustomerIsAcceptOrReject',
                {
                    msg: msg, err: null, isAccepted: true, myAccountId: targetId,
                    chatId: chatId
                })
        });

    //todo:toooodo

}

let voiceData = [];

function vC_AdminSpeakCallback(res) {

    if (!res || !res.Content || !res.Content.buffer) {
        alert("صدا برگشتی فرمت صحیح ندارد");
        return;
    }


    const voiceElem = getDoc().querySelector(`VC_AdminCallInitCallback_${res.Content.chatId}`)

    let base64 = btoa(res.Content.buffer)
    let blob = b64toBlob(base64, 'video/mp4');

    voiceData.push(blob);

    let wholeVideoBlob = new Blob(voiceData, {'type': 'video/webm'})

    let blobUrl = URL.createObjectURL(wholeVideoBlob);

    // برای اولین بار نال است
    if (voiceElem) {
        // blobUrl= URL.revokeObjectURL(blobUrl)

        let lastTime = voiceElem.currentTime ? voiceElem.currentTime : 0;

        voiceElem.src = blobUrl;

        voiceElem.load()

        voiceElem.currentTime = lastTime;
        voiceElem.play();
    }
    return blobUrl;
}


function vc_CustomerCallInit() {

    if (!CurrentUserInfo.targetId) {
        alert("ادمین یافت نشد می توانید صفحه را از نو باز کنید تا اشکالات احتمالی حل شود");
        return;
    }

    MyCaller.Send('VC_CustomerCallInit', {myAccountId: CurrentUserInfo.targetId})

}

function vC_AdminIsAcceptOrRejectCallback(res) {
    if (!res || !res.Content) {
        alert("vC_AdminIsAcceptOrRejectCallback res is null");
        return;
    }


    const voiceElem = getDoc().querySelector(`VC_AdminCallInitCallback_${res.Content.chatId}`)


    if (res.Content.IsAccepted)
        voiceElem.parentNode.append(createElementFromHTML(`<p>به تماس پاسخ داد</p>`))
    else {
        voiceElem.parentNode.append(createElementFromHTML(`<p>تماس شما توسط پشتیبانی رد شد</p>`))

    }
}


function cC_AdminInAnotherCallingCallback(res) {
    if (!res || !res.Content || !res.Content.buffer) {
        alert("صدا برگشتی فرمت صحیح ندارد");
        return;
    }


    const voiceElem = getDoc().querySelector(`VC_AdminCallInitCallback_${res.Content.chatId}`)


    voiceElem.parentNode.append(createElementFromHTML(`<p>پشتیبانی در حال مکالمه است لطفا منتظر بمانید</p>`))

}


function getWidth() {
    return Math.max(
        document.body.scrollWidth,
        document.documentElement.scrollWidth,
        document.body.offsetWidth,
        document.documentElement.offsetWidth,
        document.documentElement.clientWidth
    );
}

function getHeight() {
    return Math.max(
        document.body.scrollHeight,
        document.documentElement.scrollHeight,
        document.body.offsetHeight,
        document.documentElement.offsetHeight,
        document.documentElement.clientHeight
    );
}


/*---------------------------------------------Users Separation-------------------------------------*/


function customerGetUsersSeparationConfigCallback(res) {
    if (!res || !res.Content) {
        return;
    }

    let usersSeparation = {
        enabled: res.Content.enabled,
        type: res.Content.type,
        RestApiUrl: res.Content.RestApiUrl,
        params: res.Content.params,
        UrlPattern: res.Content.UrlPattern,
        Id: res.Content.Id
    };


    if (usersSeparation.type === 'rest-api-type') {


        if (usersSeparation.RestApiUrl && usersSeparation.params && usersSeparation.params.length) {

            fetch(usersSeparation.RestApiUrl).then(
                response => response.json() // .json(), etc.
                // same as function(response) {return response.text();}
            ).then(
                res => {
                    if (res) {
                        for (let i = 0; i < usersSeparation.params.length; i++) {

                            usersSeparation.params[i].paramValue = res[usersSeparation.params[i].paramName];

                        }

                        CustomerSaveUsersSeparationValues(usersSeparation);
                    }


                }
            );


            ``
        }

    } else {

        if (!usersSeparation.params || usersSeparation.params.length) {
            return;
        }

        for (let i = 0; i < usersSeparation.params.length; i++) {


            if (usersSeparation.params[i].paramType === 'param-type-rest') {

            } else {

                let param = document.querySelector(usersSeparation.params[i].paramName);

                if (param) {
                    let value = param.value;

                    if (!value) {
                        value = param.value.innerText;
                    }


                    usersSeparation.params[i].paramValue = value;

                } else {
                    usersSeparation.params[i].paramValue = "پارامتر یافت نشد";

                }


            }

        }

    }


}


function CustomerSaveUsersSeparationValues(usersSeparation) {


    MyCaller.Send("CustomerSaveUsersSeparationValues", usersSeparation);
}

/*--------------------------------------------- END -------------------------------------*/


/*--------------------------------------------- rating -------------------------------------*/

function adminSendRatingRequestCallback(res) {
    /* if (getDoc().querySelector('#gapComment'))
         return ;*/

    let html = `
    <div id="gapComment" style="margin:20px;border-radius: 5px 5px 5px 5px;background-color: white;
    padding:20px;border:1px solid black">
    <h3>
    <i class="fa fa-comment" aria-hidden="true"></i>

    از این که از خدمات ما استفاده کردید از شما متشکریم  </h3>
    <p>با ارسال بازخورد ما را در ارائه هرچه بهتر خدمات یاری فرمایید</p>
    
    <button onclick="gapComment(6,this)" style="background-color: greenyellow;border-radius: 5px 5px 5px 5px;border:none;padding:5px">عالی</button>
    <button onclick="gapComment(3,this)" style="background-color: #0df3a3;border-radius: 5px 5px 5px 5px;border:none;padding:5px">خوب</button>
    <button onclick="gapComment(2,this)" style="background-color: yellow;border-radius: 5px 5px 5px 5px;border:none;padding:5px">بد نبود</button>
    <button  onclick="gapComment(1,this)" style="background-color: red;border-radius: 5px 5px 5px 5px;border:none;padding:5px"> بد</button>
    
    <i style="font-size: 25px;float:left" class="fa fa-paper-plane" aria-hidden="true"></i>

</div>
    `;


    if (getDoc().querySelector('#dot').style.display === 'none') {
        getDoc().querySelector('#chatPanel').append(createElementFromHTML(html));

    } else {
        getDoc().querySelector('#gapOnTheFlyMessageText').innerHTML = html;
        getDoc().querySelector('#gapOnTheFlyMessage').style.display = null;
    }

}

function gapComment(rate, el) {


    MyCaller.Send("CustomerRate", {rate});
    let html = `
   
    <h3>
    
    <i style="font-size: 25px;color:#0df3a3" class="fa fa-smile-o" aria-hidden="true"></i>

    بازخورد شما دریافت شد متشکریم
    
    <i style="font-size: 25px;" class="fa fa-check" aria-hidden="true"></i>

    </h3>
  

    `;

    if (getDoc().querySelector('#dot').style.display === 'none') {
        html += `    <i style="font-size: 25px;float:left" class="fa fa-comment" aria-hidden="true"></i>`;

    } else {
        html += `    <i style="font-size: 25px;" class="fa fa-comment" aria-hidden="true"></i>`;
    }

    el.parentNode.innerHTML = html;

}


function testDo() {

    //  adminSendRatingRequestCallback();
}


window.addEventListener('beforeunload',function (){

    debugger;
    MyCaller.Send("CustomerTabClosed");
});




