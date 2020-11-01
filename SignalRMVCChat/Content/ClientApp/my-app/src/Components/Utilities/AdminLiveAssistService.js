
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


        let html = document.querySelector('html').outerHTML;

        let base64 = btoa(html);

        this.html = html;

        MyCaller.Send("LiveAssistSendDoc", {htmlbase64: base64});


        this.ConfigUserBehaviour();

    }


    fireEvent(type, e) {
        let ev = getMousePos(e);


        this.firedEvents = e;

        MyCaller.Send("LiveAssistFireEvent", {x: ev.x, y: ev.y, event: JSON.stringify(parse(e)), type: type});
    }

    ConfigUserBehaviour() {

        let html = document.querySelector('html');

        html.addEventListener('mousemove', (e) => {


            this.fireEvent('mousemove', e)
        })

        html.addEventListener('click', (e) => {
            this.fireEvent('click', e)

        })

        html.addEventListener('wheel', (e) => {
            this.fireEvent('wheel', e)

        })

        html.addEventListener('keydown', (e) => {
            this.fireEvent('wheel', e)

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
        if (!this.isValid(data, 'event')) {
            return;
        }
        let _event = JSON.parse(data.Content.event);

        this.firedEvents = _event;

        if (!this. _LiveAssistShower){

            this. _LiveAssistShower=new LiveAssistShower();

        }

        _LiveAssistShower.fireMouseEventAndReRender(e)


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
class LiveAssistShower{
    el = getDoc().querySelector('.mouse');

    fireMouseEventAndReRender(mouseEvent){

        this.mouseEvent=mouseEvent;

        // Ask the browser to call render to start our animation
        requestAnimationFrame((a)=>{
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
    render (a) {
        // The a variable is the amount of milliseconds since we started our script

        // Get a noise value based on the elapsed time
        // This noise algorithm is returning values between [-1, 1] so we need to map them to [0, 1] by adding one to the value and dividing it by 2
        //  const noiseX = (noise.simplex2(0, a*0.0005) + 1) / 2;
        // We get another noise value for the y axis but because we don't want the same value than x, we need to use another value for the first parameter
        //  const noiseY = (noise.simplex2(1, a*0.0005) + 1) / 2;

        // Convert the noise values from [0, 1] to the size of the window
        const x = this.getMousePos(this.mouseEvent).x;//noiseX * window.innerWidth;
        const y = this.getMousePos(this.mouseEvent).y;//noiseY * window.innerHeight;

        // Apply the x & y coordinates on our element
        this.el.style.transform = `translate(${x}px, ${y}px)`;

        // Call the render function once the browser is ready to make it an infinite loop
    }

}




export const _SenderLiveAssistService = new SenderLiveAssistService();
export const _ReceiverLiveAssistService = new ReceiverLiveAssistService();