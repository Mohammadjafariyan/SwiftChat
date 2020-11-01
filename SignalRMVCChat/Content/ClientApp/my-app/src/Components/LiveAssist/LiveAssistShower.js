import React, { Component } from "react";
import { DataHolder } from "../../Help/DataHolder";
import { CurrentUserInfo,MyCaller } from "../../Help/Socket";
import ChattedAndReturnedCustomerListPage from "../../Pages/ChattedAndReturnedCustomerListPage";
import { _showError } from "../../Pages/LayoutPage";
import {Spinner} from "react-bootstrap";

import '../../styles/myStyle.css'
import root from 'react-shadow';
import Button from "react-bootstrap/Button";


import scale from 'scale-number-range'

class LiveAssistShower extends Component {
  state={};
  scaller;
  constructor(ar) {
    super(ar);
   // this.iframeRef = React.createRef();

    CurrentUserInfo.LiveAssistShower=this;

    this.mouse = React.createRef();
    this.spinner = React.createRef();
    this.shadowRoot = React.createRef();
    this.scroller = React.createRef();

    this.fakeMouse = React.createRef();


  }

  setScroll(x,y){

    debugger;
    this.shadowRoot.current.scrollTo(x,y);
  }

  isValid(data, name) {
    if (!data || !data.Content || !data.Content[name]) {
      console.error("data is null : LiveAssistSendDocCallback");
      return false;
    }

    return true;
  }

  liveAssistSendDocCallback(data) {
    if (!this.isValid(data, "htmlbase64")) {
      return;
    }

    if (!DataHolder.selectedCustomer) {
        _showError("هیچ کاربری انتخاب نشده است");
        return;
    }

    if (data.Content.CustomerId!=DataHolder.selectedCustomer.Id) {
        _showError("خطا در سیستم ، اطلاعات بازگشتی با کد کاربر انتخاب شده یکسان نیست لطفا مجددا امتحان کنید");
        return;
    }

    let html =decodeURIComponent(escape( atob(data.Content.htmlbase64)));

    this.html = html;

/*    let iframedoc =
      this.iframeRef.current.contentDocument || this.iframeRef.current.contentWindow.document;

    iframedoc.body.innerHTML += html;*/

    this.setState({ html: html ,CustomerId:data.Content.CustomerId,   width:data.Content.width,
      height:data.Content.height});
    CurrentUserInfo.LiveAssistLayout.setState({ loading: false});



/*
    let shadow = this.shadowRoot.current.attachShadow({mode: 'open'});

    shadow.innerHTML+=html;*/
    
    
  }

  
  //    MyCaller.Send("LiveAssistFireEvent", {x: ev.x, y: ev.y, event: JSON.stringify(parse(e)), type: type});

  render() {
    return (
      <div>
        
        
        <Button variant={'primary'} onClick={()=>{
          this.configToSendMouseEvents();
        }}>


          {!this.state.sendingMyEvents &&
          <span> ارسال حرکات ماوس من</span>}

          {this.state.sendingMyEvents &&
          <>
            <Spinner animation="border" role="status">
              <span className="sr-only">در حال ارسال حرکات ماوس</span>
            </Spinner>

            <span> توقف ارسال حرکات ماوس من</span>
          </>}
         
          
        </Button>
        <hr/>
        
          <div style={{overflow:'auto'}} ref={this.scroller}>
           
      {/*      <iframe style={{zIndex:5,width:'100%',height:'100%'}}  ref={this.iframeRef}>

            </iframe>*/}

            
            
            
            
            <root.div  >


              <div className={'fakeMouse'} ref={this.fakeMouse} style={{
                top:'80px',
                left:'15px',
                right:'15px',
                bottom:'15px',
                
                position:this.state.html ? 'absolute': 'relative',zIndex:99999999999999}}>
                <svg className="mouse" ref={this.mouse} style={{position: "absolute", left: '0px',width:'25px',height:'25px'}}>
                  <path d="M20,21l4.5,8l-3.4,2l-4.6-8.1L12,29V9l16,12H20z"></path>

                </svg>
                <div ref={this.spinner} style={{position: 'absolute', display: 'none'}}>
                  <Spinner animation="grow" variant="warning"/>
                </div>

              </div>
              <div  style={{height:'100vh',overflow:'auto'}} ref={this.shadowRoot} dangerouslySetInnerHTML={{__html: this.state.html}}>
                
              </div>
            
              
            </root.div>
          </div>
      
      </div>
    );
  }
  
  
  /*======================================*/

  isValid(data, name) {
    if (!data || !data.Content || !data.Content[name]) {
      console.error("data is null : LiveAssistSendDocCallback");
      return false;
    }

    return true;
  }

  liveAssistFireEventCallback(res) {
    //      const {x: ev.x, y: ev.y, event: JSON.stringify(parse(e)), type: type}=res.Content;

    if (!this.isValid(res, "Event")) {
      return;
    }
    let _event = JSON.parse(res.Content.Event);

    this.firedEvents = _event;

    if (res.Content.x && res.Content.y)
    this.fireMouseEventAndReRender(res.Content);


    if (res.Content && res.Content.sx!=null && res.Content.sy!=null)
      CurrentUserInfo.LiveAssistShower.setScroll(res.Content.sx,res.Content.sy);

    if (res.Content.type === 'click') {

      const x = this.mouseEvent.x;
      const y = this.mouseEvent.y;
      this.spinner.current.style.left =`${x}px`;
      this.spinner.current.style.top =`${y}px`;

      this.spinner.current.style.display='block';

      if (!this.isTimeOutSet){
        this.isTimeOutSet=true;

        setTimeout(()=>{
          this.spinner.current.style.display='none';
          this.isTimeOutSet=false;
        },1000);
      }


    } else if (res.Content.type === 'scroll') {




    } else if (res.Content.type === 'keydown') {

    } else if (res.Content.type === 'focus') {

    }

  }
  
  
  
  
  configToSendMouseEvents(){
    
    this.setState({sendingMyEvents:true})
    
    let html= this.fakeMouse.current;

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

  getMousePos(e){


    let distance= this.shadowRoot.current.getBoundingClientRect();
    
    let y= e.offsetY   ;
   let x= e.offsetX;

  /*  y=Math.round((y<0 ? y*-1:y)  /0.6) ;
    x=Math.round((x<0 ? x*-1:x)/0.6);
    
    */

   // x=  scale(x, 0,this.state.width*0.6,0,this.state.width)
  //  y=  scale(y, 0,this.state.height*0.6,0,this.state.height)

    
    x=x/0.6;

    y=Math.round((y<0 ? y*-1:y));
    x=Math.round((x<0 ? x*-1:x));

 //   x= scaleValue(x,[distance.width,distance.height],[this.state.width,this.state.height])
   // y= scaleValue(y,[distance.width,distance.height],[this.state.width,this.state.height])
   // let tupple=getPosition( e,this.shadowRoot.current)

    debugger;
  /*  let x= Math.round((e.pageX-element.left ))+ this.scroller.current.scrollLeft ;//scaleValue( e.offsetX + this.scroller.current.scrollLeft, [ element.width , element.height ], [ this.state.width, this.state.height ] );
    let y= Math.round((e.pageY-element.top )) +this.scroller.current.scrollTop;//scaleValue( e.offsetY + this.scroller.current.scrollTop, [ element.width , element.height ], [ this.state.width, this.state.height ] );
*/

    return {
      x:x,
      y: y
    };
  }

  fireEvent(type, e) {
    let ev = this.getMousePos(e);

  /*  ev.x=ev.x > 0 ? ev.x : 0;
    ev.y=ev.y > 0 ? ev.y : 0;*/

    this.firedEvents = e;

    let scroll=this.shadowRoot.current;

    MyCaller.Send("LiveAssistFireEventByAdmin", {
      CustomerId: DataHolder.selectedCustomer.Id , x: ev.x, y: ev.y, event: JSON.stringify(e),
      type: type,
      sx:scroll.scrollLeft,
      sy:scroll.scrollTop

    });
  }

  fireMouseEventAndReRender(mouseEvent) {
    this.mouseEvent = mouseEvent;

    // Ask the browser to call render to start our animation
    requestAnimationFrame((a) => {
      this.renderMouse(a);
    });
  }

  renderMouse(a) {
    // The a variable is the amount of milliseconds since we started our script

    // Get a noise value based on the elapsed time
    // This noise algorithm is returning values between [-1, 1] so we need to map them to [0, 1] by adding one to the value and dividing it by 2
    //  const noiseX = (noise.simplex2(0, a*0.0005) + 1) / 2;
    // We get another noise value for the y axis but because we don't want the same value than x, we need to use another value for the first parameter
    //  const noiseY = (noise.simplex2(1, a*0.0005) + 1) / 2;

    // Convert the noise values from [0, 1] to the size of the window
    const x = this.mouseEvent.x; //this.getMousePos(this.mouseEvent).x; //noiseX * window.innerWidth;
    const y = this.mouseEvent.y; // this.getMousePos(this.mouseEvent).y; //noiseY * window.innerHeight;

    // Apply the x & y coordinates on our element
    this.mouse.current.style.transform = `translate(${x}px, ${y}px)`;

    // Call the render function once the browser is ready to make it an infinite loop
  }
}

export default LiveAssistShower;



function getScroll () {
  if (window.pageYOffset != undefined) {
    return {sx:window.pageXOffset, sy:window.pageYOffset};
  } else {
    var sx, sy, d = document,
        r = d.documentElement,
        b = d.body;
    sx = r.scrollLeft || b.scrollLeft || 0;
    sy = r.scrollTop || b.scrollTop || 0;
    return {sx, sy};
  }
}

function scaleValue(value, from, to) {
  var scale = (to[1] - to[0]) / (from[1] - from[0]);
  var capped = Math.min(from[1], Math.max(from[0], value)) - from[0];
  return Math.round((capped * scale + to[0]));
}


function getPosition (event, referenceElement) {

  const position = {
    x: event.pageX,
    y: event.pageY
  };

  const offset = {
    left: referenceElement.offsetLeft,
    top: referenceElement.offsetTop
  };

  let reference = referenceElement.offsetParent;

  /*while(reference){
    offset.left += reference.offsetLeft;
    offset.top += reference.offsetTop;
    reference = reference.offsetParent;
  }*/

  return {
    x: position.x - offset.left,
    y: position.y - offset.top,
  };

}