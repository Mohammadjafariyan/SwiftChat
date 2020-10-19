import React, {Component} from "react";
import {CurrentUserInfo} from "../Help/Socket";
import {DataHolder} from "./../Help/DataHolder";

import "../styles/myStyle.css";
import {cookieManager} from "./../Help/CookieManager";
import {MyCaller} from "./../Help/Socket";
import MyHeader from "./Header";
import ChatForm from "./ChatForm";
import TagList from "./TagList";
import CustomerTags from "./CustomerTags";
import FormShowerInChat from "./FormShowerInChat";
import {_showError} from "../Pages/LayoutPage";
import MarkAsResovled from "./MarkAsResovled";
import ScreenRecordShower from "./ScreenRecordShower";
import DOMPurify from 'dompurify';
export default class Chat extends Component {
    constructor(arg) {
        super(arg);

        this.state = {};
        CurrentUserInfo.ChatPage = this;
        this.submit = this.submit.bind(this);
    }


    componentDidMount() {


    }

    newSendPMByMeInAnotherPlaceCallback(res) {
        if (res.Content.MultimediaContent) {
            // اضافه می کند ، جایی دیگر خود ادمین ارسال کرده
            var chat = res.Content;

            if (this.state.chats) {
                var isFind = this.state.chats.find((c) => c.UniqId == chat.UniqId);

                if (isFind) {
                } else {
                    this.addChat(chat, true);
                }
            }
        } else {
            this.adminSendToCustomerCallback(res, true);
            // this.customerSendToAdminCallback(res,true);
        }
    }

    clearSearch() {
        this.setState({chats: this.prevChatsList});
    }

    searchHandlerCallback(searchChatsList) {
        this.prevChatsList = this.state.chats;
        this.setState({chats: searchChatsList});
    }

    multimediaPmSendCallback(res) {
        var chat = res.Content;
        CurrentUserInfo.ChatPage.setState({scroll: false});

        if (this.state.chats) {
            var isFind = this.state.chats.find((c) => c.UniqId == chat.UniqId);

            if (isFind) {
            } else {
                chat.IsReceive = true;
                this.addChat(chat, true);
            }
        }

        MyCaller.Send("MultimediaDeliverd", {chatId: res.Content.Id});
    }

    multimediaDeliveredCallback(res) {
        this.msgDeliveredCallback(res);
    }

    readChatCallback(res) {
        var arr = [];
        arr = res.Content.EntityList;

        let chats = this.state.chats;
        if (!chats) {
            chats = [];

        }

       /* if (!arr || !arr.length) {
            this.setState({chats: []})
        }*/
        /*   CustomerToAccount=1,
                AccountToCustomer=2,
                AccountToAccount=3,
                SaveAsFastAnswering=4, */
        for (let i = 0; i < arr.length; i++) {
            let isFind = chats.find((a) => a.Id == arr[i].Id);
            if (isFind) {
                continue;
            }
            const element = arr[i];
            if (element.SenderType == 1 || element.SenderType == 3) {
                element.IsReceive = true;
            }
            this.addChat(element, true);
        }

      let chatsSorted=  this.state.chats.sort((c1,c2)=>{
            if (c1.Id>=c2.Id){
                return  1;
            }else if (c1.Id<=c2.Id){
                return  -1;
            }
            return 0;
        })
        this.setState({chats:chatsSorted})

        /*if (arr && arr.length == 0) {
                this.setState({ chats: arr})
            }*/
    }


    LoadForms(){
        
        //debugger;
        if (!this.state.chats || this.state.chats.length==0)
            return ;
        
        if (!DataHolder.selectedCustomer){
            _showError('کاربر را مجددا انتخاب کنید');
            return ;
        }


        let forms= this.state.chats.filter(c=>c.formId );


        for (let i = 0; i < forms.length; i++) {
            MyCaller.Send('CustomerGetFormSingle', {formId: forms[i].formId, chatId: forms[i].Id,
            customerId:DataHolder.selectedCustomer.Id})
        }
        
    }


     customerGetFormSingleCallback(res) {
        
        
        
    }

    adminSendToCustomerCallback(res) {
        CurrentUserInfo.ChatPage.setState({scroll: false});

        let AccountId = res.Content.AccountId;
        let Message = res.Content.Message;
        let TotalReceivedMesssages = res.Content.TotalReceivedMesssages;
        let chat = res.Content;
        let chats = this.state.chats;
        if (!chats) {
            chats = [];
        }

        var isFind = chats.find((c) => c.UniqId == chat.UniqId);

        if (isFind) {
        } else {
            this.addChat(chat, true);
        }
    }

    customerSendToAdminCallback(res) {
        CurrentUserInfo.ChatPage.setState({scroll: false});

        let CustomerId = res.Content.CustomerId;
        let Message = res.Content.Message;
        let TotalReceivedMesssages = res.Content.TotalReceivedMesssages;

        let chat = res.Content.Chat;

        /*    if(Push){
                Push.create("پیغام جدید", {
                    body: Message,
                    icon: '/icon.png',
                    timeout: 4000,
                    onClick: function () {
                        window.focus();
                        this.close();
                    }
                });
            }*/

        chat.IsReceive = true;
        this.addChat(chat, true);

        MyCaller.Send("AdminReceivedMsg", {
            chatId: res.Content.ChatId,
            target: res.Content.CustomerId,
        });
    }


    msgDeliveredCallback(res) {
        var msgUniqId = res.Content.UniqId;

        if (!this.state.chats) return;

        var i = this.state.chats.findIndex((c) => c.UniqId === msgUniqId);

        if (i != -1) {
            let chats = this.state.chats;
            chats[i].DeliverDateTime = new Date();

            this.setState({chats: this.state.chats, temp: Math.random()});
        } else {
            console.error("i is not found");
        }
    }


    userAddedToTagsCallback(res) {

        //debugger
        if (!res || !res.Content || !res.Content.EntityList) {
            CurrentUserInfo.LayoutPage.showError('دیتای خوانده شده برای برچسب های کاربر نال است')
            return;
        }


        if (!DataHolder.selectedCustomer) {
            return;
        }

        DataHolder.selectedCustomer.Tags = res.Content.EntityList;
        DataHolder.selectedCustomer.CustomerTags = res.Content.EntityList;


        this.setState({tmp: Math.random()});
        
        if (CurrentUserInfo.CustomerTags){
            CurrentUserInfo.CustomerTags.setState({tmp:Math.random()})
        }

    }

    render() {
        return (
            <div>
                <MarkAsResovled/>

                <div className="card ">
                    <div className="card-header ">
                        {DataHolder.SelectedUserName}
                        <MyHeader/>
                    </div>
                    <div className="card-body ">
                        <h5 className="card-title"></h5>


                        {this.state.err && (
                            <div className="alert alert-danger">{this.state.err}</div>
                        )}

                        <div
                            className="chatPanel"
                            id="chatPanel"
                            onScroll={(e) => {
                                this.onScroll(e);
                            }}
                        >
                            {this.state.chats && this.state.chats.length === 0 && (
                                <p>هیچ چتی یافت نشد</p>
                            )}
                            <ChatPannel chats={this.state.chats} parent={this}/>
                        </div>
                        
                        

                        <ChatForm
                            onPaste={(e) => {
                                this.onPaste(e);
                            }}
                            upload={(e) => {
                                this.uploadFile(e);
                            }}
                            onSubmit={(e) => {
                                this.submit(e);
                            }}
                            onChange={(e) => {

                                this.chatFormOnChange(e);
                            }}
                        ></ChatForm>
                    </div>
                </div>
            </div>
        );
    }


    chatFormOnChange(e) {
        let multiMedia = showMultimedia(e.target.value);

        if (!multiMedia) {
            this.setState({text: e.target.value});
        } else {
            this.setState({text: ""});
        }
    }


    onScroll(e) {
        //todo:
        if (e.target.scrollTop === 0) {
            // element is at the end of its scroll, load more content
            if (!CurrentUserInfo.pageNumber) {
                CurrentUserInfo.pageNumber = 1;
            }

            this.setState({scroll: true});
            CurrentUserInfo.pageNumber++;
            MyCaller.Send("ReadChat", {
                targetId: DataHolder.selectedCustomer.Id,
                pageNumber: CurrentUserInfo.pageNumber,
            });
        }
    }

    uploadFile(e) {
        CurrentUserInfo.ChatPage.setState({scroll: false});

        for (let i = 0; i < e.target.files.length; i++) {
            let file = e.target.files[i];

            var fileReader = new FileReader();
            fileReader.onload = (e) => {
                let multiMedia = showMultimedia(e.target.result);

                if (multiMedia) {
                    this.addChat({MultimediaContent: e.target.result});
                } else {
                    this.setState({err: "نوع فایل قابل ارسال نیست"});
                }
            };

            fileReader.readAsDataURL(file);
        }
    }

    onPaste(event) {
        CurrentUserInfo.ChatPage.setState({scroll: false});

        var paste = event.clipboardData.getData("text/plain");
        let multiMedia = showMultimedia(paste);

        if (multiMedia) {
            this.addChat({MultimediaContent: paste});
        } else {
            this.setState({text: paste});
            //            this.addChat({Message: paste});
        }
    }

    getChats() {
        let chats = this.state.chats;
        if (!chats || !chats.length) {
            chats = [];
        }

        return chats;
    }

    addChat(chat, dontSend) {
        if (!dontSend) {
            if (!DataHolder.selectedCustomer) {
                CurrentUserInfo.LayoutPage.showError(
                    "هیچ کاربری برای ارسال پیام انتخاب نشده است"
                );
                return;
            }
        }

        let chats = this.getChats();


        chats.sort((a, b) => {
            if (a.Id < b.Id) {
                return -1;
            }
            if (a.Id > b.Id) {
                return 1;
            }
            // a must be equal to b
            return 0;
        });

        if (!dontSend) {
            let id = chats.length == 0 ? 0 : chats[chats.length - 1].UniqId;

            if (!id) {
                id = 1;
            }

            chat.UniqId = id + 1;


            this.sendChat(chat);
        }
        chats.push(chat);


        this.setState({chats: chats});

        if (!this.state.scroll) {
            setTimeout(() => {
                gotoBottom("chatPanel");
            }, 500);
        }
    }

    DeleteMessageCallback(res) {
        console.log("رسپانس حذف پیام");

        if (!res || !res.Content || !res.Content.uniqId || !res.Content.targetId) {
            console.error(" مقدار بازگشتی از سرور نال است ");
            return;
        }

        let uniqId = res.Content.uniqId;
        let targetId = res.Content.targetId;

        let message = CurrentUserInfo.ChatPage.state.chats.find(
            (f) => f.UniqId === uniqId
        );

        if (!message) {
            console.error(uniqId + " یافت نشد ");
            return;
        }
        console.log("در حال حذف پیام");

        let inner = message;

        if (inner) {
            inner.Message = "حذف شد";
            console.log("پیام حذف شد در رسپانس");
        }

        message.IsDisabled = false;

        this.setState({tmp: Math.random()});
    }

    EditMessageCallback(res) {
        console.log("رسپانس درخواست ویرایش");

        if (!res || !res.Content || !res.Content.uniqId || !res.Content.targetId) {
            console.error(" مقدار بازگشتی از سرور نال است ");
            return;
        }

        let uniqId = res.Content.uniqId;
        let targetId = res.Content.targetId;

        let message = CurrentUserInfo.ChatPage.state.chats.find(
            (f) => f.UniqId === uniqId
        );

        if (!message) {
            console.error(uniqId + " یافت نشد ");
            return;
        }
        console.log("انجام ویرایش در رسپاسن");

        let inner = message;

        if (inner) {
            inner.Message = res.Content.Message;
        }
        this.setState({tmp: Math.random()});
    }

    sendChat(chat) {
        if (!DataHolder.selectedCustomer) {
            CurrentUserInfo.LayoutPage.showError(
                "هیچ کاربری برای ارسال پیام انتخاب نشده است"
            );
            return;
        }

        if (chat.MultimediaContent) {
            MyCaller.Send("MultimediaPmSend", {
                targetId: DataHolder.selectedCustomer.Id,
                gapFileUniqId: chat.UniqId,
                MultimediaContent: chat.MultimediaContent,
                uniqId: chat.UniqId,
            });
        } else {
            MyCaller.Send("AdminSendToCustomer", {
                adminToken: cookieManager.getItem("adminToken"),
                targetUserId: DataHolder.selectedCustomer.Id,
                typedMessage: chat.Message,
                gapFileUniqId: chat.UniqId,
                uniqId: chat.UniqId,
            });
        }
    }

    submit(e) {
        e.preventDefault();
        if (!this.state.text) return false;
        CurrentUserInfo.ChatPage.setState({scroll: false});

        this.addChat({Message: this.state.text});

        this.setState({text: ""});
        return false;
    }

    DeleteMsgOnClick(uniqId, gapFileUniqId, THIS) {
        console.log("جذف پیغام");

        let message = CurrentUserInfo.ChatPage.state.chats.find(
            (f) => f.UniqId === uniqId
        );
        if (!message) {
            console.error(uniqId + " یافت نشد ");
            return;
        }
        console.log("ارسال درخواست حذف پیغام ");

        message.IsDisabled = true;

        CurrentUserInfo.ChatPage.setState({tmp: Math.random()});

        MyCaller.Send("DeleteMessage", {
            uniqId,
            targetId: CurrentUserInfo.targetId,
        });
        console.log(" درخواست حذف پیغام ارسال شد ");
    }

    EditMsgOnClick(uniqId, gapFileUniqId, THIS) {
        console.log("ویرایش فراخوانی شد");

        let message = CurrentUserInfo.ChatPage.state.chats.find(
            (f) => f.UniqId === uniqId
        );
        if (!message) {
            console.error(uniqId + " یافت نشد ");
            return;
        }

        if (!CurrentUserInfo.ChatForm) {
            CurrentUserInfo.LayoutPage.showError("CurrentUserInfo.ChatForm is null");
        }

        CurrentUserInfo.ChatForm.setState({text: message.Message});

        if (!window["submitBackup"]) {
            window["submitBackup"] = Object.assign({}, this.submit);
            window["uploadFileBackup"] = Object.assign({}, this.uploadFile);
            window["onPasteBackup"] = Object.assign({}, this.onPaste);
            window["chatFormOnChangeBackup"] = Object.assign(
                {},
                this.chatFormOnChange
            );
        }

        this.uploadFile = (e) =>
            CurrentUserInfo.LayoutPage.showError("در ویرایش فایل مجاز نمی باشد");

        this.onPaste = (e) => {
            var paste = e.clipboardData.getData("text/plain");
            this.setState({text: paste});
        };

        this.uploadFile = (e) =>
            CurrentUserInfo.LayoutPage.showError("در ویرایش فایل مجاز نمی باشد");

        this.chatFormOnChange = (e) => {
        };

        this.submit = (e) => {
            e.preventDefault();

            console.log("انجام ویرایش توسط کاربر");
            console.log("ارسال درخواست ویرایش");

            MyCaller.Send("EditMessage", {
                uniqId,
                targetId: CurrentUserInfo.targetId,
                message: e.target.value,
            });

            CurrentUserInfo.ChatForm.setState({text: ""});

            this.submit = window["submitBackup"];
            this.uploadFile = window["uploadFileBackup"];
            this.onPaste = window["onPasteBackup"];
            this.chatFormOnChange = window["chatFormOnChangeBackup"];
        };
    }
}

export function ChatPannel(props) {
    if (!props.chats || !props.chats.length) {
        return <></>;
    }

    return props.chats.map((el, i, arr) => {
        if (!el.IsReceive) {
            return (
                <div className="card post  offset-md-4 " key={el.UniqId}>
                    {props.onDelete && (
                        <div className="card-header card-header-left">
                            <button
                                onClick={(e) => {
                                    props.onDelete(el);
                                }}
                            >
                                x
                            </button>
                            {el.Delay && <p dir="rtl"> بعد از {el.Delay} دقیقه </p>}
                            {el.Time && <span style={{fontSize: '10px'}} dir="rtl"> {el.Time} </span>}

                            {props.parent && props.parent.DeleteMsgOnClick && <div style={{display: 'inline-flex'}}>
                                <button
                                    onClick={() => {
                                        props.parent.DeleteMsgOnClick(el.UniqId, el);
                                    }}
                                    className="gapB gapRemB"
                                >
                                    <i className="fa fa-trash-o" aria-hidden="true"></i>
                                </button>
                                <button
                                    onClick={() => {
                                        props.parent.EditMsgOnClick(el.UniqId, el);
                                    }}
                                    className="gapB gapEdB"
                                >
                                    <i className="fa fa-pencil" aria-hidden="true"></i>
                                </button>
                            </div>}

                        </div>
                    )}

                    <div className="card-body" style={{wordBreak: 'break-all', display: 'flex', direction: 'rtl'}}>


                        {el.formId  && <FormShowerInChat chatId={el.Id} chatUniqId={el.UniqId} formId={el.formId} ></FormShowerInChat>}

                        {el.MultimediaContent && showMultimedia(el.MultimediaContent)}
                        {!el.MultimediaContent && <p key={el.Message}

                                                     dangerouslySetInnerHTML={{__html: el.Message}}
                        /> }


                    

                        <IsDelivered DeliverDateTime={el.DeliverDateTime}/>


                    </div>
                </div>
            );
        } else {
            return (
                <div className="card post card post col-6" key={el.UniqId}>
                    {props.onDelete && (
                        <div className="card-header card-header-left">
                            <button
                                onClick={(e) => {
                                    props.onDelete(el);
                                }}
                            >
                                x
                            </button>

                            {el.Delay && <p dir="rtl"> بعد از {el.Delay}دقیقه </p>}
                        </div>
                    )}
                    <div className="card-body" style={{wordBreak: 'break-all', display: 'flex', direction: 'ltr'}}>
                        {el.formId  && <FormShowerInChat chatId={el.Id} formName={el.formName}  chatUniqId={el.UniqId} formId={el.formId} elements={el.elements}></FormShowerInChat>}
                        
                        {el.MultimediaContent && showMultimedia(el.MultimediaContent)}
                        {!el.MultimediaContent && <p key={el.Message}
                                                    
                        > {el.Message} </p>}
                    </div>
                </div>
            );
        }
    });
}

function IsDelivered(props) {
    if (props.DeliverDateTime) {
        return <i>√</i>;
    }
    if (props.IsDelivered === false) {
        return <i>کاربر آفلاین است</i>;
    }

    return <i></i>;
}

export function gotoBottom(id) {
    var element = document.getElementById(id);
    if (element) {
        element.scrollTop = element.scrollHeight - element.clientHeight;

    }
}

function validURL(str) {
    var pattern = new RegExp(
        "^(https?:\\/\\/)?" + // protocol
        "((([a-z\\d]([a-z\\d-]*[a-z\\d])*)\\.)+[a-z]{2,}|" + // domain name
        "((\\d{1,3}\\.){3}\\d{1,3}))" + // OR ip (v4) address
        "(\\:\\d+)?(\\/[-a-z\\d%_.~+]*)*" + // port and path
        "(\\?[;&a-z\\d%_.~+=-]*)?" + // query string
        "(\\#[-a-z\\d_]*)?$",
        "i"
    ); // fragment locator
    return !!pattern.test(str);
}

export function showMultimedia(pastedData) {
    if (pastedData.indexOf("video") >= 0) {
        return (
            <video height="50" width="200" controls>
                <source src={pastedData}/>
            </video>
        );
    }

    if (pastedData.indexOf("audio") >= 0) {
        return (
            <audio controls>
                <source src={pastedData}/>
            </audio>
        );
    }

    if (pastedData.indexOf("image") >= 0) {
        return (
            <img
                onClick={() => {
                    showImageMaximized(pastedData);
                }}
                alt="عکس"
                src={pastedData}
                height="100"
                controls
            />
        );
    }

    if (validURL(pastedData)) {
        return (
            <a target="_blank" href={pastedData}>
                {pastedData}
            </a>
        );
    }

    return null;
}

function closeImgPreview(el) {
    let maxImgPrev = document.getElementById("maxImgPrev");
    if (maxImgPrev) {
        maxImgPrev.remove();
    }
}

function showImageMaximized(src) {
    closeImgPreview();

    let img = createElementFromHTML(
        `<div  id="maxImgPrev"><div style="    z-index: 999999;background-color:black;opacity:0.5; position:absolute;top:0px;left:0px;right:0px;bottom:0px;
width:1000%;height:1000%;"></div><div style="z-index:999999999;margin:10%;  position:absolute;top:0px;left:0px;right:0px;bottom:0px;
width:100%;height:100%;"><img src="${src}" style="width:80%;height:80%"/>
<button type="button" onclick='let maxImgPrev = document.getElementById("maxImgPrev");
if (maxImgPrev) {
  maxImgPrev.remove();
}'>x</button>
</div></div>`
    );

    document.querySelector("body").append(img);
}

function createElementFromHTML(htmlString) {
    var div = document.createElement("div");
    div.innerHTML = htmlString.trim();

    // Change this to div.childNodes to support multiple top-level nodes
    return div.firstChild;
}








export class AutomaticSendPage extends Chat {
    constructor(arg){
        super(arg);

        this.state={};
        CurrentUserInfo.AutomaticSendPage=this;
    }




    successCallback(res){
        this.setState({sending:false});
    }

    getAutomaticSendChatsSocketHandlerCallback(res){

        this.setState({sending:false});


        if (!res || !res.Content){
            CurrentUserInfo.LayoutPage.showError('پیام های اتوماتیک خوانده شده صحیح نیست');

            return;
        }
        let arr= res.Content;

        if (arr && arr.length>0){
            this.setState({chats:arr});
        }else{

        }
    }


    componentDidMount() {
        MyCaller.Send("GetAutomaticSendChatsSocketHandler");
    }







    render() {

        return (
            <div className="container ">

                <div className='row'>


                    <div className="col-6 ">
                        <div className="col-md-12 ">

                            <ul dir={'rtl'}>
                                <li>اتوماتیک از طرف ادمین کم مراجعه ارسال می شود</li>
                                <li>از طرف ادمین های مخصوص هر بخش سایت پیغام ارسال می شود</li>
                            </ul>
                        </div>
                        <ChatPannel chats={this.state.chats}   onDelete={(chat)=>{

                            this.state.chats.splice(this.state.chats.indexOf(chat),1);
                            this.setState({temp:Math.random(),
                                chats:this.state.chats})
                        }}/>

                    </div>
                    <div className="col-6  ">


                        <div className="form-group">
                            <label htmlFor="exampleInputEmail1"> زمان تاخیر به دقیقه </label>
                            <input min={0} max={60} type="number" className="form-control" id="exampleInputEmail1"
                                   aria-describedby="emailHelp" placeholder="زمان به دقیقه  "
                                   value={this.state.delay}
                                   onChange={(e) => {
                                       if (e.target.value > 60 || e.target.value <= 0) {
                                           CurrentUserInfo.LayoutPage.showError('زمان تاخیر فقط مابین 1 الی 60 مورد قبول است');
                                           if(e.target.value>60){
                                               this.setState({delay: 60});
                                           }else{
                                               this.setState({delay: 1});

                                           }

                                       }else{
                                           this.setState({delay: e.target.value})

                                       }
                                   }
                                   }
                            />

                            <small id="emailHelp" className="form-text text-muted">مدت زمانی که کاربر وارد سایت شده است اما
                                هیچ پشتیبانی دریافت نکرده است </small>
                        </div>
                        <div className="form-group">

                            <ChatForm
                                onPaste={(e) => {
                                    this.onPaste(e)
                                }} upload={(e) => {
                                this.uploadFile(e);
                            }} onSubmit={(e) => {
                                return    this.submit(e)
                            }} onChange={(e) => {
                                let multiMedia = showMultimedia(e.target.value);

                                if (!multiMedia) {
                                    this.setState({text: e.target.value});
                                } else {
                                    this.setState({text: ""});
                                }
                            }}



                            />
                            {/* <form onSubmit={this.submit}>
                            <input
                                value={this.state.text}
                                onPaste={(e) => {
                                    this.onPaste(e);
                                }}
                                onChange={(e) => {
                                    let multiMedia = showMultimedia(e.target.value);

                                    if (!multiMedia) {
                                        this.setState({ text: e.target.value });
                                    } else {
                                        this.setState({ text: "" });
                                    }
                                }}
                            />

                            <button className="btn btn-default" type="button" onClick={() => {
                                document.getElementById('addFile').click();
                            }}>ارسال فایل</button>
                            <input
                                type="file"
                                id="addFile"
                                hidden="hidden"
                                onChange={(e) => {
                                    this.uploadFile(e);
                                }}
                            />
                        </form>*/}
                        </div>
                        <button onClick={()=>{

                            this.saveAutomaticSendChats()

                        }} type="submit" disabled={this.state.sending} className="btn btn-primary">ثبت </button>
                    </div>

                </div>



            </div>
        )
    }

    saveAutomaticSendChats(){
        if(!this.state.chats || !this.state.chats.length){
            CurrentUserInfo.LayoutPage.showError('هیچ پیغامی ثبت نشده است ');
            return;
        }


        this.setState({sending:true});


        MyCaller.Send("SaveAutomaticSendChatsSocketHandler",{
            chats:this.state.chats
        });
    }
    showAdmins() {
        if (!this.state.admins)
            return <></>;


        return (
            <table>

                <tbody>
                {
                    this.state.admins.map((el, i, arr) => {

                        return <tr key={el.Id} className="arow" onClick={() => {
                            this.setState({selectedAdmin:el})
                        }}>
                            <td>{el.Name}</td>
                        </tr>
                    })}
                </tbody>
            </table>)

    }

    addChat(chat, dontSend) {

        let chats = this.getChats();


        chat.Delay=this.state.delay ? this.state.delay : 1;
        chat.DeliverDateTime=new Date();



        chats.push(chat);

        chats.sort((a, b) => {
            if (a.Id < b.Id) {
                return -1;
            }
            if (a.Id > b.Id) {
                return 1;
            }
            // a must be equal to b
            return 0;
        });

        chat.UniqId = chats.length + 1;

        this.setState({ chats: chats });


        if (!this.state.scroll) {
            /*setTimeout(() => {
                gotoBottom("chatPanel");
            }, 500);*/
        }

    }
    submit(e) {
        e.preventDefault();
        if (!this.state.text) return false;
        CurrentUserInfo.ChatPage.setState({ scroll: false });

        this.addChat({ Message: this.state.text },true);

        this.setState({ text: "" });
        return false;
    }


}