import React, {Component} from "react";
import {MyCaller} from "./../Help/Socket";
import AddTag from "./AddTag";
import {ListGroup, Form, Button} from "react-bootstrap";
import {DataHolder} from "../Help/DataHolder";
import {CurrentUserInfo} from "../CurrentUserInfo";

export default class TagList extends Component {

    constructor(args) {
        super(args);

        this.state = {list: []};

        CurrentUserInfo.TagList = this;

    }


    getTagsCallback(res) {
        if (!res || !res.Content || !res.Content.EntityList) {
            CurrentUserInfo.LayoutPage.showError('دیتای خوانده شده برای برچسب ها نال است')
            return;
        }
        this.getAllTagsForCurrentAdminCallback(res);

    }

    getAllTagsForCurrentAdmin() {
        CurrentUserInfo.LayoutPage.showMsg('در حال خواندن برچسب ها')
        MyCaller.Send("GetAllTagsForCurrentAdmin");

    }


    componentDidMount() {
        this.getAllTagsForCurrentAdmin();

        /*
        
            let chat = {
              Message: "با سلام خوش آمدید",
            };
            let chats = [];
            chats.push(chat);
        
            let arr = [];
        
            let addres = "Tehran,Tehran";
            arr.push({
              Name: "کاربر آنلاین",
              Message: "با سلام خوش آمدید",
              Time: "5 دقیقه قبل",
              Address: addres,
            });
            arr.push({
              Name: "کاربر آنلاین",
              Message: "با سلام خوش آمدید",
              Time: "5 دقیقه قبل",
              Address: addres,
            });
            arr.push({
              Name: "کاربر آنلاین",
              Message: "با سلام خوش آمدید",
              Time: "5 دقیقه قبل",
              Address: addres,
            });
            arr.push({
              Name: "کاربر آنلاین",
              Message: "با سلام خوش آمدید",
              Time: "5 دقیقه قبل",
              Address: addres,
            });
            arr.push({
              Name: "کاربر آنلاین",
              Message: "با سلام خوش آمدید",
              Time: "5 دقیقه قبل",
              Address: addres,
            });
            arr.push({
              Name: "کاربر آنلاین",
              Message: "با سلام خوش آمدید",
              Time: "5 دقیقه قبل",
              Address: addres,
            });
            arr.push({
              Name: "کاربر آنلاین",
              Message: "با سلام خوش آمدید",
              Time: "5 دقیقه قبل",
              Address: addres,
            });
            arr.push({
              Name: "کاربر آنلاین",
              Message: "با سلام خوش آمدید",
              Time: "5 دقیقه قبل",
              Address: addres,
            });
            arr.push({
              Name: "کاربر آنلاین",
              Message: "با سلام خوش آمدید",
              Time: "5 دقیقه قبل",
              Address: addres,
            });
            arr.push({
              Name: "کاربر آنلاین",
              Message: "با سلام خوش آمدید",
              Time: "5 دقیقه قبل",
              Address: addres,
            });
            arr.push({
              Name: "کاربر آنلاین",
              Message: "با سلام خوش آمدید",
              Time: "5 دقیقه قبل",
              Address: addres,
            });
        
            this.setState({ list: arr });*/
    }

    getAllTagsForCurrentAdminCallback(res) {
        if (!res || !res.Content || !res.Content.EntityList) {
            CurrentUserInfo.LayoutPage.showError("دیتای بازگشتی برای برچسب ها نال است");
            return;
        }

        this.setState({tags: res.Content.EntityList});
    }

    render() {
        return (
            <div>
                <AddTag/>


                <ListGroup>
                    {CurrentUserInfo.NewTagModeEnabled && <>

                        <h6>جهت برچسب زدن به کاربر انتخاب نمایید :</h6>


                        <Button variant="primary" type="button" onClick={() => {
                            this.saveSelectedTagsForCustomer()
                        }}>
                            ثبت
                        </Button>
                    </>
                    }

                    {this.state.tags &&
                    this.state.tags.map((el, i, arr) => {
                        return (

                            <>

                                <ListGroup.Item key={el.Id}>
                                    {!CurrentUserInfo.NewTagModeEnabled &&
                                    <>
                                        <button
                                            style={{
                                                float: 'left', backgroundColor: 'white',
                                                border: 'none'
                                            }}
                                            type="button"
                                            onClick={() => {
                                                this.deleteTagById(el.Id);
                                            }}
                                        >
                                            x
                                        </button>

                                        <a
                                            href="#"
                                            onClick={() => {
                                                this.getUsersByTagId(el.Id, el.Name);
                                            }}
                                        >

                                            <i
                                                style={{fontSize: "14px", float: 'right'}}
                                                className="fa fa-tags"
                                                aria-hidden="true"
                                            ></i>
                                            <span>{el.Name}</span>


                                        </a></>}
                                    {CurrentUserInfo.NewTagModeEnabled && <>

                                        <Form.Group controlId="formBasicCheckbox2">
                                            <Form.Check type="checkbox" checked={el.checked} label={el.Name}
                                                        onChange={(e) => {
                                                            el.checked = e.target.checked;
                                                            if (!CurrentUserInfo.selectedTags) {
                                                                CurrentUserInfo.selectedTags = [];
                                                            }

                                                            // حذف از ارایه
                                                            CurrentUserInfo.selectedTags = CurrentUserInfo.selectedTags.filter(f => f.Id !== el.Id);


                                                            if (e.target.checked) {
                                                                CurrentUserInfo.selectedTags.push(el.Id);
                                                            }

                                                            this.setState({tmp: Math.random()})
                                                        }}/>
                                        </Form.Group>


                                    </>

                                    }
                                </ListGroup.Item>
                            </>
                        );
                    })}


                </ListGroup>
            </div>
        );
    }

    saveSelectedTagsForCustomer() {
        if (!CurrentUserInfo.NewTagModeCustomerId) {
            CurrentUserInfo.LayoutPage.showError('CurrentUserInfo.NewTagModeCustomerId is null');
            return;
        }
        if (!CurrentUserInfo.selectedTags || CurrentUserInfo.selectedTags.length == 0) {
            CurrentUserInfo.LayoutPage.showError('هیچ برچسبی انتخاب نشده است');
            return;
        }


        CurrentUserInfo.LayoutPage.showMsg('در حال ذخیره برچسب های انتخاب شده به کاربر');

        MyCaller.Send("SetCurrentUserToTags", {
            tags: CurrentUserInfo.selectedTags, target: CurrentUserInfo.NewTagModeCustomerId,
        })

        // بستن فرم برچسب ها
        CurrentUserInfo.NewTagModeEnabled = false;


        CurrentUserInfo.selectedTags = [];

        for (let i = 0; i < CurrentUserInfo.TagList.state.tags.length; i++) {
            CurrentUserInfo.TagList.state.tags[i].checked = false;
        }
        CurrentUserInfo.TagList.setState({tmp: Math.random()})

    }

    getUsersByTagId(tagId, tagName) {
        CurrentUserInfo.selectedTagId = tagId;
        CurrentUserInfo.selectedTagName = tagName;


        if (CurrentUserInfo.OnlineCustomerListHolder) {
            CurrentUserInfo.OnlineCustomerListHolder.GetClientsListForAdmin();
        }
    }

    deleteTagById(tagId) {


        CurrentUserInfo.LayoutPage.showMsg('در حال حذف تگ')
        MyCaller.Send("DeleteTagById", {tagId: tagId});
    }

    deleteTagByIdCallback(res) {


        CurrentUserInfo.LayoutPage.showMsg('برچسب با موفقیت حذف شد');

        // آپدیت لیست برچسب ها
        this.getAllTagsForCurrentAdmin();

        // آپدیت لیست کاربران
        CurrentUserInfo.OnlineCustomerListHolder.GetClientsListForAdmin();

        if (DataHolder.selectedCustomer)
            MyCaller.Send("GetUserAddedToTags", {target: DataHolder.selectedCustomer.Id});


    }
}
