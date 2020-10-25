let bean = {
    /*------------------------------info------------------------------*/
    /// <summary>
    /// نام 
    /// </summary>
    Name: '',
    IsEnabled: '',

    /*------------------------------actions------------------------------*/
    IsShowMessageEnabled: '',
    IsOpenChatBox: '',
    IsPlayASound: '',

    localizedMessages: [],

    localizedMessagesJson: '',

    /*------------------------------Behaviours------------------------------ */
    ExecuteOnlyIfOnline: '',
    ExecuteOnlyIfFirstTimeVisit: '',
    ExecuteOnlyIfNoOtherTriggerFired: '',
    ExecuteOnlyIfFromACountry: '',
    countries: [],
    CountiesJson: '',


    /*------------------------------Events------------------------------*/
    EventOnExitTab: '',
    EventOnLinkClick: '',
    EventSpecificPages: true,
    EventAddressParameters: true,
    EventUserCustomName: true,
    EventDelay: '',

    delay: '',


    linksJson: '',
    links: [],

    pagesJson: '',
    pages: [
        {Name: 'http://at.cu.co.ir/!student/*/ww', SecondName: 'saeed'},
        {Name: '*', SecondName: 'saeed'},
    ],

    pageParametersJson: '',
    pageParameters: [
        {Name: 'username', SecondName: 'saeed'}

    ],
    userEventNamesJson: '',
    userEventNames: [],

    /*------------------------------Platforms------------------------------*/
    RunInMobileDevices: true,
    RunInDesktopDevices: true,

    MyWebsiteId: '', MyWebsite: '',
    MyAccountId: '',
    MyAccount: ''

};

test('onbeforeunload fire', () => {

    debugger;

    let _EventTrigger = new EventTrigger(bean);

    _EventTrigger.ConfigEvents();

    window.dispatchEvent(new Event('beforeunload'));

    expect(_EventTrigger.firedEvent).toBe('onbeforeunloadFired');
});


test('configOnQueryParameters fire', () => {

    debugger;

    let _EventTrigger = new EventTrigger(bean);


    Object.defineProperty(window, 'location', {value: {href: window.location.href + '?username=saeed'}});

    _EventTrigger.ConfigEvents();


    expect(_EventTrigger.firedEvent).toBe('configOnQueryParametersFired');
});


test('configOnSpecificPages fire', () => {

    debugger;

    let _EventTrigger = new EventTrigger(bean);


    Object.defineProperty(window, 'location', {value: {href: 'http://at.cu.co.ir/student/gallery?username=saeed'}});


    _EventTrigger.ConfigEvents();


    expect(_EventTrigger.firedEvent).toBe('configOnSpecificPagesFired');
});


/*imp*/


class EventTriggerManager {
    eventTriggerBeans = [];

    eventTriggers = [];

    getEventTriggers() {
        MyCaller.Send('GetEventTriggers')
    }

    getEventTriggersCallback(res) {
        if (!res || !res.Content || !res.Content.EntityList) {
            alert('دیتای بازگشتی نال است')
        }

        this.eventTriggers = [];

        this.eventTriggerBeans = res.Content.EntityList;

        this.Execute();
    }


    Execute() {
        if (!this.eventTriggerBeans || !this.eventTriggerBeans.length)
            return;


        for (let i = 0; i < this.eventTriggerBeans.length; i++) {
            this.eventTriggers.push(new EventTrigger(this.eventTriggerBeans[i]))
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

        window.addEventListener('beforeunload', () => {


            this.fireEvent('beforeunload');

        })


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

            let selector = this.bean.links[i];


            // select in dom
            let findedLinks = document.querySelectorAll(selector);

            if (!findedLinks)
                continue;

            for (let j = 0; j < findedLinks; j++) {
                findedLinks[j].addEventListener('click', () => {

                    this.fireEvent('click ' + findedLinks[j]);

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

                            break;
                            notEqualAny = true;

                        }

                        continue;

                    }

                    var pattern = new RegExp(name);
                    if (pattern.test(URLParts[j])) {
                    } else {
                        notEqualAny = true;

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

            this.showMessage();
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
            if (this.bean.IsOpenChatBox) {
                showNewOnTheFlyMessage(this.bean.localizedMessages[i])
            } else {


                let res = {
                    Content: {
                        Message: this.bean.localizedMessages[i]
                    }
                };
                
                CurrentUserInfo.plugin.adminSendToCustomerCallback(res, false);

            }

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

    }


    /*-------------------------fire Event:*/

    fireEvent(name) {


        this.Action();

        MyCaller.Send('EventFired', {name: name})

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